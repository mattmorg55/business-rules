using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AJBoggs.ADAP.BR.Model.Domain
{
    public static class SvfNoChangesStatus
    {
        public const string IN_PROGRESS = "IN_PROGRESS";
        public const string APPROVED = "APPROVED";
        public const string CANCELLED = "CANCELLED";

        public static readonly string[] List = new string[]
        {
            IN_PROGRESS,
            APPROVED,
            CANCELLED
        };

        public static readonly Dictionary<string, string> Transforms = new Dictionary<string, string>
        {
            { "IN PROGRESS", IN_PROGRESS }
        };
    }
}
