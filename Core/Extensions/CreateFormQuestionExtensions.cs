using AJBoggs.ADAP.BR.Core.Facts;
using AJBoggs.ADAP.BR.Model.Domain;
using AJBoggs.ADAP.BR.Model.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AJBoggs.ADAP.BR.Core.Extensions
{
    public static class CreateFormQuestionExtensions
    {
        public static CreateFormFact ToFact(this CreateFormQuestion question)
        {
            if (String.IsNullOrWhiteSpace(question.Form))
            {
                throw new ArgumentException("question.Form was null or whitespace", "question");
            }
            CreateFormFact fact = new CreateFormFact();
            fact.Form = question.Form.Trim();
            fact.AnniversaryDate = question.AnniversaryDate;
            fact.SvfDate = question.SvfDate;

            if (question.MostRecentForms == null || question.MostRecentForms.Count == 0)
            {
                return fact;
            }
            question.MostRecentForms.ForEach(x =>
            {
                if (String.IsNullOrWhiteSpace(x.Form))
                {
                    throw new ArgumentException("FormStatus.Form was null or whitespace", "question");
                }
                if (String.IsNullOrWhiteSpace(x.Status))
                {
                    throw new ArgumentException("FormStatus.Status was null or whitespace", "question");
                }
                string form = x.Form.Trim();
                string status = x.Status.Trim();
                if (form.Equals(Form.INITIAL_ENROLLMENT, StringComparison.OrdinalIgnoreCase))
                {
                    fact.InitialEnrollmentStatus = ValidateAndTransformAdapApplicationStatus(status);
                }
                else if (form.Equals(Form.REENROLLMENT, StringComparison.OrdinalIgnoreCase))
                {
                    fact.ReenrollmentStatus = ValidateAndTransformAdapApplicationStatus(status);
                }
                else if (form.Equals(Form.SVF_WITH_CHANGES, StringComparison.OrdinalIgnoreCase))
                {
                    fact.SvfWithChangesStatus = ValidateAndTransformAdapApplicationStatus(status);
                }
                else if (form.Equals(Form.SVF_NO_CHANGES, StringComparison.OrdinalIgnoreCase))
                {
                    fact.SvfNoChangesStatus = ValidateAndTransformSvfNoChangesStatus(status);
                }
                else if (form.Equals(Form.UPDATE_FORM, StringComparison.OrdinalIgnoreCase))
                {
                    fact.UpdateFormStatus = ValidateAndTransformAdapApplicationStatus(status);
                }
                else
                {
                    throw new Exception(String.Format("Don't know how to handle form {0}", form));
                }
            });

            return fact;
        }

        private static string ValidateAndTransformAdapApplicationStatus(string status)
        {
            if (String.IsNullOrWhiteSpace(status))
            {
                return null;
            }
            status = status.Trim().ToUpperInvariant();
            if (AdapApplicationStatus.List.Contains(status))
            {
                return status;
            }
            if (AdapApplicationStatus.Transforms.ContainsKey(status))
            {
                return AdapApplicationStatus.Transforms[status];
            }
            throw new Exception(String.Format("ADAP Application status value {0} is invalid", status));
        }

        private static string ValidateAndTransformSvfNoChangesStatus(string status)
        {
            if (String.IsNullOrWhiteSpace(status))
            {
                return null;
            }
            status = status.Trim().ToUpperInvariant();
            if (SvfNoChangesStatus.List.Contains(status))
            {
                return status;
            }
            if (SvfNoChangesStatus.Transforms.ContainsKey(status))
            {
                return SvfNoChangesStatus.Transforms[status];
            }
            throw new Exception(String.Format("SVF No Changes status value {0} is invalid", status));
        }
    }
}
