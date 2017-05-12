using AJBoggs.ADAP.BR.Model.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AJBoggs.ADAP.BR.Core.Facts
{
    public class CreateFormFact
    {
        public string Form { get; set; }
        public string InitialEnrollmentStatus { get; set; }
        public string ReenrollmentStatus { get; set; }
        public string SvfWithChangesStatus { get; set; }
        public string SvfNoChangesStatus { get; set; }
        public string UpdateFormStatus { get; set; }
        public DateTimeOffset AnniversaryDate { get; set; }
        public DateTimeOffset SvfDate { get; set; }

        public CreateFormFact()
        {

        }
    }
}
