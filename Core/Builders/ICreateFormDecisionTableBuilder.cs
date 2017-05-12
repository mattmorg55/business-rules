using System.Collections.Generic;
using AJBoggs.ADAP.BR.Core.Rules;

namespace AJBoggs.ADAP.BR.Core.Builders
{
    public interface ICreateFormDecisionTableBuilder
    {
        bool Compile(CreateFormDecisionTable table, out CreateFormDecisionTable compiledTable, out List<string> errors);
        CreateFormDecisionTable FromJson(string jsonFilePath);
    }
}