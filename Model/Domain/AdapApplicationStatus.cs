using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AJBoggs.ADAP.BR.Model.Domain
{
    public static class AdapApplicationStatus
    {
        public const string IN_PROGRESS = "IN_PROGRESS";
        public const string NEEDS_REVIEW = "NEEDS_REVIEW";
        public const string NEEDS_INFORMATION = "NEEDS_INFORMATION";
        public const string DENIED = "DENIED";
        public const string APPROVED = "APPROVED";
        public const string APPROVED_HIPP = "APPROVED_HIPP";
        public const string APPROVED_MEDICARE_PART_D = "APPROVED_MEDICARE_PART_D";
        public const string CANCELLED = "CANCELLED";

        public static readonly string[] List = new string[] {
            IN_PROGRESS,
            NEEDS_REVIEW,
            NEEDS_INFORMATION,
            DENIED,
            APPROVED,
            APPROVED_HIPP,
            APPROVED_MEDICARE_PART_D,
            CANCELLED
        };

        public static readonly Dictionary<string, string> Transforms = new Dictionary<string, string>
        {
            { "IN_PROCESS", IN_PROGRESS },
            { "APPROVED - MEDICATION ASSISTANCE ONLY", APPROVED },
            { "APPROVED - MEDICATION ASSISTANCE BUT OA-HIPP NEEDS MORE INFORMATION", APPROVED },
            { "APPROVED - MEDICATION ASSISTANCE BUT OA-HIPP STILL NEEDS REVIEW", APPROVED },
            { "APPROVED - MEDICATION ASSISTANCE BUT OA-HIPP DENIED", APPROVED },
            { "APPROVED - MEDICATION ASSISTANCE WITH OA-HIPP", APPROVED_HIPP },
            { "APPROVED - MEDICATION ASSISTANCE BUT MEDICARE PART D NEEDS MORE INFORMATION", APPROVED },
            { "APPROVED - MEDICATION ASSISTANCE BUT MEDICARE PART D STILL NEEDS REVIEW", APPROVED },
            { "APPROVED - MEDICATION ASSISTANCE BUT MEDICARE PART D DENIED", APPROVED },
            { "APPROVED - MEDICATION ASSISTANCE WITH MEDICARE PART D", APPROVED_MEDICARE_PART_D }
        };
    }
}
