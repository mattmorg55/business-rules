using AJBoggs.ADAP.BR.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AJBoggs.ADAP.BR.Model.Questions
{
    public class CreateFormsQuestion
    {
        public DateTimeOffset AnniversaryDate { get; set; }
        public string Culture { get; set; }
        public long SubjectId { get; set; }
        public DateTimeOffset SvfDate { get; set; }
        public long UserId { get; set; }
        public List<FormStatus> MostRecentForms { get; set; }

        public CreateFormsQuestion()
        {
            MostRecentForms = new List<FormStatus>();
        }
    }
}
