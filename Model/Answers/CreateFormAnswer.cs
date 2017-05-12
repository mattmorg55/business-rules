using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AJBoggs.ADAP.BR.Model.Answers
{
    public class CreateFormAnswer : Answer
    {
        public bool CanCreate { get; set; }
        public string Explanation { get; set; }
    }
}
