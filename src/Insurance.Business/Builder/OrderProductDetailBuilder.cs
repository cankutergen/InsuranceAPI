using Insurance.Entities.ComplexTypes;
using Insurance.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Business.Builder
{
    public class OrderProductDetailBuilder
    {
        private readonly OrderProductDetail _orderProductDetail;

        public OrderProductDetailBuilder()
        {
            _orderProductDetail = new OrderProductDetail();
        }

        public OrderProductDetail Build(InsuranceModel insuranceModel, int quantity)
        {
            _orderProductDetail.ProductId = insuranceModel.ProductId;
            _orderProductDetail.Quantity = quantity;
            _orderProductDetail.ProductTypeId = insuranceModel.ProductTypeId;
            _orderProductDetail.InsurancePerProduct = insuranceModel.InsuranceValue;
            _orderProductDetail.TotalInsurance = insuranceModel.InsuranceValue * quantity;

            return _orderProductDetail;
        }
    }
}
