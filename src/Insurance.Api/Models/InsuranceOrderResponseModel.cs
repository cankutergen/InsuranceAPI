using System.Collections.Generic;

namespace Insurance.Api.Models
{
    public class InsuranceOrderResponseModel
    {
        public List<int> productIdList { get; set; }

        public float TotalInsuranceAmount { get; set; }
    }
}
