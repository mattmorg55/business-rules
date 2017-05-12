using AJBoggs.ADAP.BR.Core.Rules;
using AJBoggs.ADAP.BR.Model.Domain;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AJBoggs.ADAP.BR.Core.Builders
{
    public class CreateFormDecisionTableBuilder : ICreateFormDecisionTableBuilder
    {
        private readonly ILogger<CreateFormDecisionTableBuilder> mLogger;

        public CreateFormDecisionTableBuilder(ILogger<CreateFormDecisionTableBuilder> logger)
        {
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }
            mLogger = logger;
        }

        public CreateFormDecisionTable FromJson(string jsonFilePath)
        {
            if (String.IsNullOrWhiteSpace(jsonFilePath))
            {
                throw new ArgumentException("String was null or whitespace.", "jsonFilePath");
            }
            if (!File.Exists(jsonFilePath))
            {
                throw new Exception(String.Format("File does not exist {0}", jsonFilePath));
            }
            string json = File.ReadAllText(jsonFilePath);
            if (String.IsNullOrWhiteSpace(json))
            {
                throw new Exception("JSON string was null or whitespace");
            }

            return JsonConvert.DeserializeObject<CreateFormDecisionTable>(json);
        }

        public bool Compile(CreateFormDecisionTable table, out CreateFormDecisionTable compiledTable, out List<string> errors)
        {
            if (table == null)
            {
                throw new ArgumentNullException("table");
            }

            compiledTable = null;
            errors = null;
            //Explode rows containing CSVs or *
            //Set 'null' and '' to null
            CreateFormDecisionTable _compiledTable = table.Clone();
            _compiledTable.Compiled = true;
            List<string> _errors = new List<string>();

            CompileFormColumn(_compiledTable, _errors);
            CompileColumn(nameof(CreateFormDecisionRow.InitialEnrollmentStatus), AdapApplicationStatus.List, _compiledTable, _errors);
            CompileColumn(nameof(CreateFormDecisionRow.ReenrollmentStatus), AdapApplicationStatus.List, _compiledTable, _errors);
            CompileColumn(nameof(CreateFormDecisionRow.SvfWithChangesStatus), AdapApplicationStatus.List, _compiledTable, _errors);
            CompileColumn(nameof(CreateFormDecisionRow.SvfNoChangesStatus), SvfNoChangesStatus.List, _compiledTable, _errors);
            CompileColumn(nameof(CreateFormDecisionRow.UpdateFormStatus), AdapApplicationStatus.List, _compiledTable, _errors);

            //MoopStatus

            if (_errors.Count > 0)
            {
                errors = _errors;
                return false;
            }
            compiledTable = _compiledTable;
            return true;
        }

        private void CompileColumn(string columnName, string[] allowedStatusValues, CreateFormDecisionTable _compiledTable, List<string> _errors)
        {
            if (String.IsNullOrWhiteSpace(columnName))
            {
                throw new ArgumentException("String was null or whitespace.", "columnName");
            }
            if (allowedStatusValues == null)
            {
                throw new ArgumentNullException("allowedStatusValues");
            }
            if (_compiledTable == null)
            {
                throw new ArgumentNullException("_compiledTable");
            }
            if (_errors == null)
            {
                throw new ArgumentNullException("_errors");
            }
            CreateFormDecisionTable tempTable = _compiledTable.Clone();
            _compiledTable.Rows.Clear();
            foreach (var row in tempTable.Rows)
            {
                PropertyInfo columnPropertyInfo = typeof(CreateFormDecisionRow).GetProperty(columnName);
                if (columnPropertyInfo == null)
                {
                    throw new Exception(String.Format("Failed to get PropertyInfo for {0}", columnName));
                }
                string cellValue = columnPropertyInfo.GetValue(row) as string;
                if (String.IsNullOrWhiteSpace(cellValue))
                {
                    columnPropertyInfo.SetValue(row, null);
                    _compiledTable.Rows.Add(row);
                    continue;
                }
                cellValue.Trim().ToUpperInvariant();
                if (cellValue.Equals("NULL", StringComparison.InvariantCultureIgnoreCase))
                {
                    columnPropertyInfo.SetValue(row, null);
                    _compiledTable.Rows.Add(row);
                    continue;
                }
                if (cellValue.Equals("*", StringComparison.InvariantCultureIgnoreCase))
                {
                    //Exlode row including adding null
                    var _row = new CreateFormDecisionRow(row);
                    columnPropertyInfo.SetValue(_row, null);
                    _compiledTable.Rows.Add(_row);
                    foreach (string status in allowedStatusValues)
                    {
                        _row = new CreateFormDecisionRow(row);
                        columnPropertyInfo.SetValue(_row, status);
                        _compiledTable.Rows.Add(_row);
                    }
                    continue;
                }
                //Handle CSV
                if (cellValue.Contains(","))
                {
                    string[] statusList = cellValue
                        .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => x.Trim())
                        .ToArray();
                    CreateFormDecisionRow _row;
                    foreach (string status in statusList)
                    {
                        if (String.IsNullOrWhiteSpace(status))
                        {
                            continue;
                        }
                        if (status.Equals("NULL", StringComparison.InvariantCultureIgnoreCase))
                        {
                            _row = new CreateFormDecisionRow(row);
                            columnPropertyInfo.SetValue(_row, null);
                            _compiledTable.Rows.Add(_row);
                            continue;
                        }
                        if (!allowedStatusValues.Contains(status))
                        {
                            string error = String.Format("row.{0} {1} is not a known value. Row {2}: {3}", columnName, status, row.RowNumber, row);
                            mLogger.LogError(error);
                            _errors.Add(error);
                            continue;
                        }
                        _row = new CreateFormDecisionRow(row);
                        columnPropertyInfo.SetValue(_row, status.ToUpperInvariant());
                        _compiledTable.Rows.Add(_row);
                    }
                    continue;
                }
                //Handle simple case
                if (!allowedStatusValues.Contains(cellValue))
                {
                    string error = String.Format("row.{0} {1} is not a known value. Row {2}: {3}", columnName, cellValue, row.RowNumber, row);
                    mLogger.LogError(error);
                    _errors.Add(error);
                    continue;
                }
                _compiledTable.Rows.Add(row);
            }
        }

        private void CompileFormColumn(CreateFormDecisionTable _compiledTable, List<string> _errors)
        {
            CreateFormDecisionTable tempTable = _compiledTable.Clone();
            _compiledTable.Rows.Clear();
            int rowNumber = 0;
            foreach (var row in tempTable.Rows)
            {
                row.RowNumber = ++rowNumber;

                if (String.IsNullOrWhiteSpace(row.Form))
                {
                    string error = String.Format("row.Form was null or white space. Row {0}: {1}", row.RowNumber, row);
                    mLogger.LogError(error);
                    _errors.Add(error);
                    continue;
                }
                row.Form = row.Form.Trim();
                if (!Form.LIST.Contains(row.Form))
                {
                    string error = String.Format("row.Form {0} is not a known value. Row {1}: {2}", row.Form, row.RowNumber, row);
                    mLogger.LogError(error);
                    _errors.Add(error);
                    continue;
                }
                _compiledTable.Rows.Add(row);
            }
        }
    }
}
