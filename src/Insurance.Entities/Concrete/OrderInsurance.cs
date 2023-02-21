using Insurance.Entities.ComplexTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Entities.Concrete
{
    public class OrderInsurance
    {
        public List<OrderProductDetail> OrderProductDetails { get; set; }

        public float TotalInsuranceAmount { get; set; }

        public OrderInsurance()
        {
            OrderProductDetails = new List<OrderProductDetail>();
            TotalInsuranceAmount = 0;
        }
    }
}
