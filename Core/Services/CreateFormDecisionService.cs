using AJBoggs.ADAP.BR.Core.Extensions;
using AJBoggs.ADAP.BR.Core.Facts;
using AJBoggs.ADAP.BR.Core.Rules;
using AJBoggs.ADAP.BR.Model.Answers;
using AJBoggs.ADAP.BR.Model.Questions;
using AJBoggs.Common.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text;

namespace AJBoggs.ADAP.BR.Core.Services
{
    public class CreateFormDecisionService
    {
        private readonly ILogger<CreateFormDecisionService> mLogger;
        private readonly ICreateFormDecisionTable mDecisionTable;

        public CreateFormDecisionService(ILogger<CreateFormDecisionService> logger, ICreateFormDecisionTable decisionTable)
        {
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }
            if (decisionTable == null)
            {
                throw new ArgumentNullException("decisionTable");
            }
            mLogger = logger;
            mDecisionTable = decisionTable;

            if (!mDecisionTable.Compiled)
            {
                throw new ArgumentException("Decision table must be compiled", "decisionTable");
            }
        }

        public CreateFormAnswer CanCreateForm(CreateFormQuestion question)
        {
            if (question == null)
            {
                throw new ArgumentNullException("question");
            }
            CreateFormAnswer answer = new CreateFormAnswer();
            try
            {
                CreateFormFact fact = question.ToFact();
                var rules = mDecisionTable.Rows
                    .Where(x => x.Form == fact.Form
                        && x.InitialEnrollmentStatus == fact.InitialEnrollmentStatus
                        && x.ReenrollmentStatus == fact.ReenrollmentStatus
                        && x.SvfWithChangesStatus == fact.SvfWithChangesStatus
                        && x.SvfNoChangesStatus == fact.SvfNoChangesStatus
                        && x.UpdateFormStatus == fact.UpdateFormStatus)
                    .ToList();
                if (rules.Count == 0)
                {
                    answer.Error = true;
                    answer.ErrorMessage = String.Format("Your question did not match any rules in the decision table. "
                        + "Question was converted to the following fact. {0}", JsonConvert.SerializeObject(fact));
                }
                if (rules.Count > 1)
                {
                    answer.Error = true;
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("Your question matched more than 1 rule in the decision table.");
                    sb.AppendLine(String.Format("Question was converted to the following fact. {0}", JsonConvert.SerializeObject(fact)));
                    sb.AppendLine("The following rules matched the fact:");
                    foreach (var _rule in rules)
                    {
                        sb.AppendLine(JsonConvert.SerializeObject(_rule));
                    }
                }
                CreateFormDecisionRow rule = rules[0];
                if (rule.CheckReenrollmentWindow.Value == FunctionColumn.NO && rule.CheckSvfWindow.Value == FunctionColumn.NO)
                {
                    if (!rule.CanCreate.HasValue)
                    {
                        throw new Exception(String.Format("Rule.CanCreate does not have a value specified. {0}", JsonConvert.SerializeObject(rule)));
                    }
                    answer.CanCreate = rule.CanCreate.Value;
                    answer.Explanation = rule.Explanation;
                }
                else
                {
                    bool checkReenrollmentWindowResult = false;
                    string checkReenrollmentWindowExplanation = null;
                    string checkReenrollmentWindow = rule.CheckReenrollmentWindow.Value;
                    if (checkReenrollmentWindow == FunctionColumn.YES || checkReenrollmentWindow == FunctionColumn.NEGATE)
                    {
                        checkReenrollmentWindowResult = mDecisionTable.CheckAnniversaryWindow(fact, question.Culture, 
                            checkReenrollmentWindow == FunctionColumn.NEGATE, out checkReenrollmentWindowExplanation);
                    }
                    bool checkSvfWindowResult = false;
                    string checkSvfWindowExplantion = "";
                    string checkSvfWindow = rule.CheckSvfWindow.Value;
                    if (checkSvfWindow == FunctionColumn.YES || checkSvfWindow == FunctionColumn.NEGATE)
                    {
                        checkSvfWindowResult = mDecisionTable.CheckAnniversaryWindow(fact, question.Culture,
                            checkSvfWindow == FunctionColumn.NEGATE, out checkSvfWindowExplantion);
                    }
                    answer.CanCreate = checkReenrollmentWindowResult && checkSvfWindowResult;
                    StringBuilder sb = new StringBuilder();
                    if (!String.IsNullOrWhiteSpace(checkReenrollmentWindowExplanation))
                    {
                        sb.Append(checkReenrollmentWindowExplanation);
                    }
                    if (!String.IsNullOrWhiteSpace(checkSvfWindowExplantion))
                    {
                        if (!String.IsNullOrWhiteSpace(checkReenrollmentWindowExplanation)) {
                            sb.Append(" ");
                        }
                        sb.Append(checkSvfWindowExplantion);
                    }
                    answer.Explanation = sb.ToString();
                }
            }
            catch (Exception ex)
            {
                string errorMessage = String.Format("An error occurred while evaluating the question. {0}", ex.ToStringVerbose());
                mLogger.LogError(errorMessage);
                answer.Error = true;
                answer.ErrorMessage = errorMessage;
            }
            return answer;
        }
    }
}
