using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Insurance.Entities.Concrete
{
    public class InsuranceModel
    {
        public int ProductId { get; set; }

        public float InsuranceValue { get; set; }

        public int ProductTypeId { get; set; }

        public string ProductTypeName { get; set; }

        public bool ProductTypeHasInsurance { get; set; }

        public float SalesPrice { get; set; }
    }
}
