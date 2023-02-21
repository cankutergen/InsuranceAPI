using Insurance.Entities.ComplexTypes;
using System.Collections.Generic;

namespace Insurance.Api.Models.Insurance
{
    public class OrderInsuranceResponseModel
    {
        public List<OrderProductDetail> OrderProductDetails { get; set; }

        public float TotalInsuranceAmount { get; set; }
    }
}
