using AJBoggs.ADAP.BR.Core.Facts;
using System.Collections.Generic;

namespace AJBoggs.ADAP.BR.Core.Rules
{
    public interface ICreateFormDecisionTable
    {
        bool Compiled { get; set; }
        int ReenrollmentWindowDays { get; set; }
        int SvfWindowDays { get; set; }
        List<CreateFormDecisionRow> Rows { get; set; }

        CreateFormDecisionTable Clone();

        bool CheckAnniversaryWindow(CreateFormFact fact, string culture, bool negate, out string explanation);

        bool CheckSvfWindow(CreateFormFact fact, string culture, bool negate, out string explanation);
    }
}