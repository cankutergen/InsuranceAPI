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

        [JsonIgnore]
        public string ProductTypeName { get; set; }

        [JsonIgnore]
        public bool ProductTypeHasInsurance { get; set; }

        [JsonIgnore]
        public float SalesPrice { get; set; }
    }
}
