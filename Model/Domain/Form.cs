using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AJBoggs.ADAP.BR.Model.Domain
{
    public static class Form
    {
        public const string INITIAL_ENROLLMENT = "Initial Enrollment";
        public const string REENROLLMENT = "Reenrollment";
        public const string SVF_WITH_CHANGES = "SVF with Changes";
        public const string SVF_NO_CHANGES = "SVF No Changes";
        public const string UPDATE_FORM = "Update Form";

        public static readonly string[] LIST = new string[]
        {
            INITIAL_ENROLLMENT,
            REENROLLMENT,
            SVF_WITH_CHANGES,
            SVF_NO_CHANGES,
            UPDATE_FORM
        };
    }
}
