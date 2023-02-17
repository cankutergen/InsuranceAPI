using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Entities.Concrete
{
    public class InsuranceOrder
    {
        public List<InsuranceModel> InsuranceList { get; set; }

        public float TotalInsuranceAmount { get; set; }

        public InsuranceOrder()
        {
            InsuranceList = new List<InsuranceModel>();
            TotalInsuranceAmount = 0;
        }
    }
}
