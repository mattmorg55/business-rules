using AJBoggs.ADAP.BR.Core.Facts;
using System;
using System.Collections.Generic;

namespace AJBoggs.ADAP.BR.Core.Rules
{
    public class CreateFormDecisionTable : ICreateFormDecisionTable
    {
        private const string ANNIVERSARY_WINDOW_TRUE_MESSAGE_FORMAT = "Current date is within {0} days of anniversary date {1:D}.";
        private const string ANNIVERSARY_WINDOW_FALSE_MESSAGE_FORMAT = "Current date is not within {0} days of anniversary date {1:D}.";
        private const string SVF_WINDOW_TRUE_MESSAGE_FORMAT = "Current date is within {0} days of SVF date {1:D}.";
        private const string SVF_WINDOW_FALSE_MESSAGE_FORMAT = "Current date is not within {0} days of SVF date {1:D}.";

        public bool Compiled { get; set; }
        public int ReenrollmentWindowDays { get; set; }
        public int SvfWindowDays { get; set; }
        public List<CreateFormDecisionRow> Rows { get; set; }
        
        //Constructors

        public CreateFormDecisionTable() {
            Rows = new List<CreateFormDecisionRow>();
        }

        //Public instance methods

        public CreateFormDecisionTable Clone()
        {
            CreateFormDecisionTable clone = new CreateFormDecisionTable();
            clone.Compiled = Compiled;
            clone.ReenrollmentWindowDays = ReenrollmentWindowDays;
            clone.SvfWindowDays = SvfWindowDays;
            Rows.ForEach(x => clone.Rows.Add(new CreateFormDecisionRow(x)));
            return clone;
        }

        public bool CheckAnniversaryWindow(CreateFormFact fact, string culture, bool negate, out string explanation)
        {
            if (fact == null)
            {
                throw new ArgumentNullException("fact");
            }
            if (culture == null)
            {
                throw new ArgumentNullException("culture");
            }
            bool result = false;
            var now = DateTimeOffset.UtcNow;
            var min = fact.AnniversaryDate.AddDays((-1) * ReenrollmentWindowDays);
            var max = fact.AnniversaryDate.AddDays(ReenrollmentWindowDays);
            if (now >= min && now <= max)
            {
                result = true;
            }
            if (result)
            {
                explanation = String.Format(ANNIVERSARY_WINDOW_TRUE_MESSAGE_FORMAT, ReenrollmentWindowDays, fact.AnniversaryDate);
            } else
            {
                explanation = String.Format(ANNIVERSARY_WINDOW_FALSE_MESSAGE_FORMAT, ReenrollmentWindowDays, fact.AnniversaryDate);
            }
            return negate ? !result : result;
        }

        public bool CheckSvfWindow(CreateFormFact fact, string culture, bool negate, out string explanation)
        {
            if (fact == null)
            {
                throw new ArgumentNullException("fact");
            }
            if (culture == null)
            {
                throw new ArgumentNullException("culture");
            }
            bool result = false;
            var now = DateTimeOffset.UtcNow;
            var min = fact.SvfDate.AddDays((-1) * SvfWindowDays);
            var max = fact.SvfDate.AddDays(SvfWindowDays);
            if (now >= min && now <= max)
            {
                result = true;
            }
            result = false;
            if (result)
            {
                explanation = String.Format(SVF_WINDOW_TRUE_MESSAGE_FORMAT, SvfWindowDays, now);
            }
            else
            {
                explanation = String.Format(SVF_WINDOW_FALSE_MESSAGE_FORMAT, SvfWindowDays, now);
            }
            return negate ? !result : result;
        }
    }
}
