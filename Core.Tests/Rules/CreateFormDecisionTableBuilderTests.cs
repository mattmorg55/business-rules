using AJBoggs.ADAP.BR.Core.Builders;
using AJBoggs.ADAP.BR.Core.Rules;
using AJBoggs.ADAP.BR.Model.Domain;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AJBoggs.ADAP.BR.Core.Tests.Rules
{
    [TestClass]
    public class CreateFormDecisionTableBuilderTests
    {
        private readonly ILogger<CreateFormDecisionTableBuilder> mLogger;

        public CreateFormDecisionTableBuilderTests()
        {
            mLogger = new Mock<ILogger<CreateFormDecisionTableBuilder>>().Object;
        }

        [TestMethod]
        public void FromJsonTest()
        {
            //Arrange
            string path = Constants.Rules.CreateForms.DECISION_TABLE_PATH_1;
            if (!File.Exists(path))
            {
                throw new Exception(String.Format("File does not exist {0}", path));
            }
            var tableBuilder = GetCreateFormDecisionTableBuilder();

            //Act
            CreateFormDecisionTable table = tableBuilder.FromJson(path);

            //Assert
            Assert.IsNotNull(table);
            Assert.AreEqual(Constants.Rules.CreateForms.REENROLLMENT_WINDOW_DAYS, table.ReenrollmentWindowDays);
            Assert.AreEqual(Constants.Rules.CreateForms.SVF_WINDOW_DAYS, table.SvfWindowDays);
            Assert.IsTrue(table.Rows.Count > 0);
            Assert.IsFalse(table.Compiled);
        }

        [TestMethod]
        public void CompileTest1()
        {
            //Arrange
            string path = Constants.Rules.CreateForms.DECISION_TABLE_PATH_1;
            if (!File.Exists(path))
            {
                throw new Exception(String.Format("File does not exist {0}", path));
            }
            var tableBuilder = GetCreateFormDecisionTableBuilder();
            CreateFormDecisionTable table = tableBuilder.FromJson(path);
            if (table == null)
            {
                throw new Exception("table was null");
            }

            //Act
            CreateFormDecisionTable compiledTable;
            List<string> errors;
            bool result = tableBuilder.Compile(table, out compiledTable, out errors);

            //Assert
            Assert.IsTrue(result);
            Assert.IsNotNull(compiledTable);
            Assert.IsNull(errors);
            Assert.IsTrue(compiledTable.Compiled);
            Assert.AreEqual(Constants.Rules.CreateForms.REENROLLMENT_WINDOW_DAYS, compiledTable.ReenrollmentWindowDays);
            Assert.AreEqual(Constants.Rules.CreateForms.SVF_WINDOW_DAYS, compiledTable.SvfWindowDays);
            Assert.IsTrue(compiledTable.Rows.Count == 1);
            var row = compiledTable.Rows[0];
            Assert.AreEqual(Form.INITIAL_ENROLLMENT, row.Form);
            Assert.IsNull(row.InitialEnrollmentStatus);
            Assert.IsNull(row.ReenrollmentStatus);
            Assert.IsNull(row.SvfWithChangesStatus);
            Assert.IsNull(row.SvfNoChangesStatus);
            Assert.IsNull(row.UpdateFormStatus);
            Assert.IsTrue(row.CanCreate.Value);
            Assert.IsFalse(String.IsNullOrWhiteSpace(row.Explanation));
            Assert.AreEqual(FunctionColumn.NO, row.CheckReenrollmentWindow.Value);
            Assert.AreEqual(FunctionColumn.NO, row.CheckSvfWindow.Value);
        }

        [TestMethod]
        public void CompileTest2()
        {
            //Arrange
            string path = Constants.Rules.CreateForms.DECISION_TABLE_PATH_2;
            if (!File.Exists(path))
            {
                throw new Exception(String.Format("File does not exist {0}", path));
            }
            var tableBuilder = GetCreateFormDecisionTableBuilder();
            CreateFormDecisionTable table = tableBuilder.FromJson(path);
            if (table == null)
            {
                throw new Exception("table was null");
            }

            //Act
            CreateFormDecisionTable compiledTable;
            List<string> errors;
            bool result = tableBuilder.Compile(table, out compiledTable, out errors);

            //Assert
            Assert.IsTrue(result);
            Assert.IsNotNull(compiledTable);
            Assert.IsNull(errors);
            Assert.IsTrue(compiledTable.Compiled);
            Assert.AreEqual(Constants.Rules.CreateForms.REENROLLMENT_WINDOW_DAYS, compiledTable.ReenrollmentWindowDays);
            Assert.AreEqual(Constants.Rules.CreateForms.SVF_WINDOW_DAYS, compiledTable.SvfWindowDays);
            Assert.IsTrue(compiledTable.Rows.Count > 0);
            Assert.IsTrue(compiledTable.Rows.Count >= table.Rows.Count);
            var rowCount = compiledTable.Rows
                .Where(x => x.Form == Form.INITIAL_ENROLLMENT)
                .Where(x => x.InitialEnrollmentStatus == AdapApplicationStatus.IN_PROGRESS)
                .Where(x => !x.CanCreate.Value)
                .Where(x => !String.IsNullOrWhiteSpace(x.Explanation))
                .Where(x => x.CheckReenrollmentWindow.Value == FunctionColumn.NO)
                .Where(x => x.CheckSvfWindow.Value == FunctionColumn.NO)
                .Count();
            Assert.AreEqual(compiledTable.Rows.Count, rowCount);

        }

        [TestMethod]
        public void CompileTest4()
        {
            //Arrange
            string path = Constants.Rules.CreateForms.DECISION_TABLE_PATH_4;
            if (!File.Exists(path))
            {
                throw new Exception(String.Format("File does not exist {0}", path));
            }
            var tableBuilder = GetCreateFormDecisionTableBuilder();
            CreateFormDecisionTable table = tableBuilder.FromJson(path);
            if (table == null)
            {
                throw new Exception("table was null");
            }

            //Act
            CreateFormDecisionTable compiledTable;
            List<string> errors;
            bool result = tableBuilder.Compile(table, out compiledTable, out errors);

            //Assert
            Assert.IsTrue(result);
            Assert.IsNotNull(compiledTable);
            Assert.IsNull(errors);
            Assert.IsTrue(compiledTable.Compiled);
            Assert.AreEqual(Constants.Rules.CreateForms.REENROLLMENT_WINDOW_DAYS, compiledTable.ReenrollmentWindowDays);
            Assert.AreEqual(Constants.Rules.CreateForms.SVF_WINDOW_DAYS, compiledTable.SvfWindowDays);
            Assert.IsTrue(compiledTable.Rows.Count > 0);
            Assert.IsTrue(compiledTable.Rows.Count >= table.Rows.Count);
            var rowCount = compiledTable.Rows
                .Where(x => x.Form == Form.SVF_NO_CHANGES)
                .Where(x => x.SvfNoChangesStatus == SvfNoChangesStatus.IN_PROGRESS)
                .Where(x => !x.CanCreate.Value)
                .Where(x => !String.IsNullOrWhiteSpace(x.Explanation))
                .Where(x => x.CheckReenrollmentWindow.Value == FunctionColumn.NO)
                .Where(x => x.CheckSvfWindow.Value == FunctionColumn.NO)
                .Count();
            Assert.AreEqual(compiledTable.Rows.Count, rowCount);

            Assert.IsTrue(compiledTable.Rows.Where(x => x.InitialEnrollmentStatus == null).Any());
            Assert.IsTrue(compiledTable.Rows.Where(x => x.InitialEnrollmentStatus == AdapApplicationStatus.IN_PROGRESS).Any());
            Assert.IsTrue(compiledTable.Rows.Where(x => x.InitialEnrollmentStatus == AdapApplicationStatus.NEEDS_REVIEW).Any());
            Assert.IsTrue(compiledTable.Rows.Where(x => x.InitialEnrollmentStatus == AdapApplicationStatus.NEEDS_INFORMATION).Any());
            Assert.IsTrue(compiledTable.Rows.Where(x => x.InitialEnrollmentStatus == AdapApplicationStatus.DENIED).Any());
            Assert.IsTrue(compiledTable.Rows.Where(x => x.InitialEnrollmentStatus == AdapApplicationStatus.APPROVED).Any());
            Assert.IsTrue(compiledTable.Rows.Where(x => x.InitialEnrollmentStatus == AdapApplicationStatus.CANCELLED).Any());
        }

        [TestMethod]
        public void CompileTest6()
        {
            //Arrange
            string path = Constants.Rules.CreateForms.DECISION_TABLE_PATH_6;
            if (!File.Exists(path))
            {
                throw new Exception(String.Format("File does not exist {0}", path));
            }
            var tableBuilder = GetCreateFormDecisionTableBuilder();
            CreateFormDecisionTable table = tableBuilder.FromJson(path);
            if (table == null)
            {
                throw new Exception("table was null");
            }

            //Act
            CreateFormDecisionTable compiledTable;
            List<string> errors;
            bool result = tableBuilder.Compile(table, out compiledTable, out errors);

            //Assert
            Assert.IsTrue(result);
            Assert.IsNotNull(compiledTable);
            Assert.IsNull(errors);
            Assert.IsTrue(compiledTable.Compiled);
            Assert.AreEqual(Constants.Rules.CreateForms.REENROLLMENT_WINDOW_DAYS, compiledTable.ReenrollmentWindowDays);
            Assert.AreEqual(Constants.Rules.CreateForms.SVF_WINDOW_DAYS, compiledTable.SvfWindowDays);
            Assert.IsTrue(compiledTable.Rows.Count == 4);
            Assert.IsTrue(compiledTable.Rows.Count >= table.Rows.Count);
            var rowCount = compiledTable.Rows
                .Where(x => x.Form == Form.INITIAL_ENROLLMENT)
                .Where(x => x.InitialEnrollmentStatus != null)
                .Where(x => x.ReenrollmentStatus == null)
                .Where(x => x.SvfWithChangesStatus == null)
                .Where(x => x.SvfNoChangesStatus == null)
                .Where(x => x.UpdateFormStatus == null)
                .Where(x => !x.CanCreate.Value)
                .Where(x => !String.IsNullOrWhiteSpace(x.Explanation))
                .Where(x => x.CheckReenrollmentWindow.Value == FunctionColumn.NO)
                .Where(x => x.CheckSvfWindow.Value == FunctionColumn.NO)
                .Count();
            Assert.AreEqual(compiledTable.Rows.Count, rowCount);

            Assert.IsTrue(compiledTable.Rows.Where(x => x.InitialEnrollmentStatus == AdapApplicationStatus.IN_PROGRESS).Any());
            Assert.IsTrue(compiledTable.Rows.Where(x => x.InitialEnrollmentStatus == AdapApplicationStatus.NEEDS_REVIEW).Any());
            Assert.IsTrue(compiledTable.Rows.Where(x => x.InitialEnrollmentStatus == AdapApplicationStatus.NEEDS_INFORMATION).Any());
            Assert.IsTrue(compiledTable.Rows.Where(x => x.InitialEnrollmentStatus == AdapApplicationStatus.APPROVED).Any());
        }

        private CreateFormDecisionTableBuilder GetCreateFormDecisionTableBuilder()
        {
            return new CreateFormDecisionTableBuilder(mLogger);
        }
    }
}
