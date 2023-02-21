using Insurance.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Entities.ComplexTypes
{
    public class OrderProductDetail
    {
        public int ProductId { get; set; }

        public int ProductTypeId { get; set; }

        public int Quantity { get; set; }

        public float InsurancePerProduct { get; set; }

        public float TotalInsurance { get; set; }
    }
}
