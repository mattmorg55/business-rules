using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AJBoggs.ADAP.BR.Core.Rules
{
    public class FunctionColumn 
    {
        public const string YES = "YES";
        public const string NO = "NO";
        public const string NEGATE = "NEGATE";

        public static readonly string[] List = new string[]
        {
            YES,
            NO,
            NEGATE
        };

        private string mValue;

        public string Value {
            get {
                return mValue;
            }
            set {
                if (String.IsNullOrWhiteSpace(value))
                {
                    throw new Exception("value was null or whitespace");
                }
                var _value = value.Trim().ToUpperInvariant();
                if (!List.Contains(_value))
                {
                    throw new Exception(String.Format("Value {0} is invalid", _value));
                }
                mValue = _value;
            }
        }

        public FunctionColumn()
        {
        }
        
        public FunctionColumn(FunctionColumn functionColumn)
        {
            Value = functionColumn.Value;
        }
    }
}
