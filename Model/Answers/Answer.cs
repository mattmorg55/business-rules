using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AJBoggs.ADAP.BR.Model.Answers
{
    public abstract class Answer
    {
        public bool Error { get; set; }
        public string ErrorMessage { get; set; }
    }
}
