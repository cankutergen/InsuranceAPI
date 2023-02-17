using Insurance.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Entities.Concrete
{
    public class ErrorModel : IError
    {
        public string Type { get; set; }

        public string Title { get; set; }

        public int Status { get; set; }

        public string TraceId { get; set; }
    }
}
