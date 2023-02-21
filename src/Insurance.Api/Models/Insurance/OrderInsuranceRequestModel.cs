using Insurance.Entities.Concrete;
using System.Collections.Generic;

namespace Insurance.Api.Models.Insurance
{
    public class OrderInsuranceRequestModel
    {
        public List<OrderProduct> OrderProducts { get; set; }
    }
}
