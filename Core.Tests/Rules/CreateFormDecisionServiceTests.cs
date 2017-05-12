using AJBoggs.ADAP.BR.Core.Builders;
using AJBoggs.ADAP.BR.Core.Rules;
using AJBoggs.ADAP.BR.Core.Services;
using AJBoggs.ADAP.BR.Model.Answers;
using AJBoggs.ADAP.BR.Model.Domain;
using AJBoggs.ADAP.BR.Model.Questions;
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
    public class CreateFormDecisionServiceTests
    {
        private readonly ILogger<CreateFormDecisionService> mLogger;
        private readonly ICreateFormDecisionTableBuilder mTableBuilder;

        public CreateFormDecisionServiceTests()
        {
            mLogger = new Mock<ILogger<CreateFormDecisionService>>().Object;
            var tableBuilderLogger = new Mock<ILogger<CreateFormDecisionTableBuilder>>().Object;
            mTableBuilder = new CreateFormDecisionTableBuilder(tableBuilderLogger);
        }

        [TestMethod]
        public void InstantiateServiceTest()
        {
            //Arrange
            var table = mTableBuilder.FromJson(Constants.Rules.CreateForms.DECISION_TABLE_PATH_7);
            CreateFormDecisionTable compiledTable;
            List<string> errors;
            if (!mTableBuilder.Compile(table, out compiledTable, out errors))
            {
                throw new Exception(String.Format("An error occurred while compiling table from {0}", 
                    Constants.Rules.CreateForms.DECISION_TABLE_PATH_7));
            }

            //Act
            var service = new CreateFormDecisionService(mLogger, compiledTable);

            //Assert
            Assert.IsNotNull(service);
        }

        [TestMethod]
        public void CanCreateTest1()
        {
            //Arrange
            var service = GetDecisionService();
            CreateFormQuestion question = new CreateFormQuestion
            {
                AnniversaryDate = new DateTimeOffset(2017, 10, 31, 0, 0, 0, TimeSpan.Zero),
                Culture = "en-US",
                SubjectId = 12345,
                SvfDate = new DateTimeOffset(2017, 5, 1, 0, 0, 0, TimeSpan.Zero),
                UserId = 12345,
                Form = "Initial Enrollment"
            };
            question.MostRecentForms.Add(new FormStatus
            {
                Form = "Initial Enrollment",
                Status = "CANCELLED",
                StatusChanged = new DateTimeOffset(2017, 2, 27, 0, 0, 0, TimeSpan.Zero)
            });

            //Act
            CreateFormAnswer answer = service.CanCreateForm(question);

            //Assert
            Assert.IsNotNull(answer);
            Assert.IsFalse(answer.Error);
            Assert.IsTrue(answer.CanCreate);
            Assert.IsFalse(String.IsNullOrWhiteSpace(answer.Explanation));
        }

        [TestMethod]
        public void CanCreateTest2()
        {
            //Arrange
            var service = GetDecisionService();
            CreateFormQuestion question = new CreateFormQuestion
            {
                AnniversaryDate = new DateTimeOffset(2017, 10, 31, 0, 0, 0, TimeSpan.Zero),
                Culture = "en-US",
                SubjectId = 12345,
                SvfDate = new DateTimeOffset(2017, 5, 1, 0, 0, 0, TimeSpan.Zero),
                UserId = 12345,
                Form = "Initial Enrollment"
            };
            question.MostRecentForms.Add(new FormStatus
            {
                Form = "Initial Enrollment",
                Status = "IN_PROCESS",
                StatusChanged = new DateTimeOffset(2017, 2, 27, 0, 0, 0, TimeSpan.Zero)
            });

            //Act
            CreateFormAnswer answer = service.CanCreateForm(question);

            //Assert
            Assert.IsNotNull(answer);
            Assert.IsFalse(answer.Error);
            Assert.IsFalse(answer.CanCreate);
            Assert.IsFalse(String.IsNullOrWhiteSpace(answer.Explanation));
        }

        [TestMethod]
        public void CanCreateTest3()
        {
            //Arrange
            var service = GetDecisionService();
            CreateFormQuestion question = new CreateFormQuestion
            {
                AnniversaryDate = new DateTimeOffset(2017, 10, 31, 0, 0, 0, TimeSpan.Zero),
                Culture = "en-US",
                SubjectId = 12345,
                SvfDate = new DateTimeOffset(2017, 5, 1, 0, 0, 0, TimeSpan.Zero),
                UserId = 12345,
                Form = "Reenrollment"
            };
            question.MostRecentForms.Add(new FormStatus
            {
                Form = "Initial Enrollment",
                Status = "DENIED",
                StatusChanged = new DateTimeOffset(2017, 2, 27, 0, 0, 0, TimeSpan.Zero)
            });

            //Act
            CreateFormAnswer answer = service.CanCreateForm(question);

            //Assert
            Assert.IsNotNull(answer);
            Assert.IsFalse(answer.Error);
            Assert.IsTrue(answer.CanCreate);
            Assert.IsFalse(String.IsNullOrWhiteSpace(answer.Explanation));
        }

        [TestMethod]
        public void CanCreateTest4()
        {
            //Arrange
            var service = GetDecisionService();
            CreateFormQuestion question = new CreateFormQuestion
            {
                AnniversaryDate = new DateTimeOffset(2017, 10, 31, 0, 0, 0, TimeSpan.Zero),
                Culture = "en-US",
                SubjectId = 12345,
                SvfDate = new DateTimeOffset(2017, 5, 1, 0, 0, 0, TimeSpan.Zero),
                UserId = 12345,
                Form = Form.REENROLLMENT
            };
            question.MostRecentForms.Add(new FormStatus
            {
                Form = Form.INITIAL_ENROLLMENT,
                Status = AdapApplicationStatus.APPROVED,
                StatusChanged = new DateTimeOffset(2017, 2, 27, 0, 0, 0, TimeSpan.Zero)
            });
            question.MostRecentForms.Add(new FormStatus
            {
                Form = Form.REENROLLMENT,
                Status = AdapApplicationStatus.CANCELLED,
                StatusChanged = new DateTimeOffset(2017, 2, 27, 0, 0, 0, TimeSpan.Zero)
            });
            question.MostRecentForms.Add(new FormStatus
            {
                Form = Form.SVF_WITH_CHANGES,
                Status = AdapApplicationStatus.CANCELLED,
                StatusChanged = new DateTimeOffset(2017, 2, 27, 0, 0, 0, TimeSpan.Zero)
            });
            question.MostRecentForms.Add(new FormStatus
            {
                Form = Form.SVF_NO_CHANGES,
                Status = SvfNoChangesStatus.CANCELLED,
                StatusChanged = new DateTimeOffset(2017, 2, 27, 0, 0, 0, TimeSpan.Zero)
            });
            question.MostRecentForms.Add(new FormStatus
            {
                Form = Form.UPDATE_FORM,
                Status = AdapApplicationStatus.CANCELLED,
                StatusChanged = new DateTimeOffset(2017, 2, 27, 0, 0, 0, TimeSpan.Zero)
            });

            //Act
            CreateFormAnswer answer = service.CanCreateForm(question);

            //Assert
            Assert.IsNotNull(answer);
            Assert.IsFalse(answer.Error);
            Assert.IsFalse(answer.CanCreate);
            Assert.IsFalse(String.IsNullOrWhiteSpace(answer.Explanation));
        }

        private CreateFormDecisionService GetDecisionService()
        {
            var table = mTableBuilder.FromJson(Constants.Rules.CreateForms.DECISION_TABLE_PATH_7);
            CreateFormDecisionTable compiledTable;
            List<string> errors;
            if (!mTableBuilder.Compile(table, out compiledTable, out errors))
            {
                throw new Exception(String.Format("An error occurred while compiling table from {0}",
                    Constants.Rules.CreateForms.DECISION_TABLE_PATH_1));
            }

            var service = new CreateFormDecisionService(mLogger, compiledTable);
            if (service == null)
            {
                throw new Exception("service was null");
            }

            return service;
        }
    }
}
