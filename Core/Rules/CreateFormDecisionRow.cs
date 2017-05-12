using Newtonsoft.Json;
using System;

namespace AJBoggs.ADAP.BR.Core.Rules
{
    public class CreateFormDecisionRow : ICreateFormDecisionRow
    {
        internal int RowNumber { get; set; }

        //Match on these
        public string Form { get; set; }
        public string InitialEnrollmentStatus { get; set; }
        public string ReenrollmentStatus { get; set; }
        public string SvfWithChangesStatus { get; set; }
        public string SvfNoChangesStatus { get; set; }
        public string UpdateFormStatus { get; set; }
        public string MoopExpenseFormStatus { get; set; }
        //Results
        public bool? CanCreate { get; set; }
        public string Explanation { get; set; }
        //Potentially alter CanCreate with these checks if cell value is true
        public FunctionColumn CheckReenrollmentWindow { get; set; }
        public FunctionColumn CheckSvfWindow { get; set; }

        //Constructors

        public CreateFormDecisionRow() { }

        public CreateFormDecisionRow(CreateFormDecisionRow row)
        {
            if (row == null)
            {
                throw new ArgumentNullException("row");
            }
            RowNumber = row.RowNumber;
            Form = row.Form;
            InitialEnrollmentStatus = row.InitialEnrollmentStatus;
            ReenrollmentStatus = row.ReenrollmentStatus;
            SvfWithChangesStatus = row.SvfWithChangesStatus;
            SvfNoChangesStatus = row.SvfNoChangesStatus;
            UpdateFormStatus = row.UpdateFormStatus;
            CanCreate = row.CanCreate;
            Explanation = row.Explanation;
            CheckReenrollmentWindow = new FunctionColumn(row.CheckReenrollmentWindow);
            CheckSvfWindow = new FunctionColumn(row.CheckSvfWindow);
        }

        //Public instance methods

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

    }
}
