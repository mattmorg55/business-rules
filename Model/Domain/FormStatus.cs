using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AJBoggs.ADAP.BR.Model.Domain
{
    public class FormStatus
    {
        public string Form { get; set; }
        public string Status { get; set; }
        public DateTimeOffset StatusChanged { get; set; }
    }
}
