using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AJBoggs.ADAP.BR.Model.Domain
{
    public static class MoopExpenseFormStatus
    {
        public const string IN_PROGRESS = "IN_PROGRESS";
        public const string SUBMITTED = "SUBMITTED";
        public const string CANCELLED = "CANCELLED";

        public static readonly string[] List = new string[]
        {
            IN_PROGRESS,
            SUBMITTED,
            CANCELLED
        };
    }
}
