using AJBoggs.ADAP.BR.Core.Builders;
using AJBoggs.ADAP.BR.Core.Rules;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using System;
using System.IO;

namespace AJBoggs.ADAP.BR.Core.Tests.Rules
{
    [TestClass]
    public class CreateFormDecisionTableTests
    {
        private readonly ILogger<CreateFormDecisionTableBuilder> mLogger;

        public CreateFormDecisionTableTests()
        {
            mLogger = new Mock<ILogger<CreateFormDecisionTableBuilder>>().Object;
        }

        [TestMethod]
        public void DeserializeJsonTest1()
        {
            //Arrange
            string path = Constants.Rules.CreateForms.DECISION_TABLE_PATH_1;
            if (!File.Exists(path))
            {
                throw new Exception(String.Format("File does not exist {0}", path));
            }
            string json = File.ReadAllText(path);
            if (String.IsNullOrWhiteSpace(json))
            {
                throw new Exception("JSON string was null or whitespace");
            }

            //Act
            CreateFormDecisionTable table = JsonConvert.DeserializeObject<CreateFormDecisionTable>(json);

            //Assert
            Assert.IsNotNull(table);
            Assert.AreEqual(Constants.Rules.CreateForms.REENROLLMENT_WINDOW_DAYS, table.ReenrollmentWindowDays);
            Assert.AreEqual(Constants.Rules.CreateForms.SVF_WINDOW_DAYS, table.SvfWindowDays);
            Assert.IsTrue(table.Rows.Count > 0);
        }

        [TestMethod]
        public void DeserializeJsonTest2()
        {
            //Arrange
            string path = Constants.Rules.CreateForms.DECISION_TABLE_PATH;
            if (!File.Exists(path))
            {
                throw new Exception(String.Format("File does not exist {0}", path));
            }
            string json = File.ReadAllText(path);
            if (String.IsNullOrWhiteSpace(json))
            {
                throw new Exception("JSON string was null or whitespace");
            }

            //Act
            CreateFormDecisionTable table = JsonConvert.DeserializeObject<CreateFormDecisionTable>(json);

            //Assert
            Assert.IsNotNull(table);
            Assert.AreEqual(Constants.Rules.CreateForms.REENROLLMENT_WINDOW_DAYS, table.ReenrollmentWindowDays);
            Assert.AreEqual(Constants.Rules.CreateForms.SVF_WINDOW_DAYS, table.SvfWindowDays);
            Assert.IsTrue(table.Rows.Count > 0);
        }
    }
}
