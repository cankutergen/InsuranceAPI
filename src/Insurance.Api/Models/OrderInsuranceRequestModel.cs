using Insurance.Entities.Concrete;
using System.Collections.Generic;

namespace Insurance.Api.Models
{
    public class OrderInsuranceRequestModel
    {
        public List<OrderProduct> OrderProducts { get; set; }
    }
}
