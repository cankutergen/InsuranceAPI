﻿using Insurance.Business.Abstract;
using Insurance.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Business.Builder
{
    public class InsuranceBuilder
    {
        private InsuranceModel _insuranceModel;

        public InsuranceBuilder()
        {
            _insuranceModel = new InsuranceModel();
        }

        public InsuranceModel BuildWithProductInformation(Product product, ProductType productType)
        {
            BuildProductType(product, productType);
            BuildSalesPrice(product);

            return _insuranceModel;
        }

        private void BuildProductType(Product product, ProductType productType)
        {
            _insuranceModel.ProductId = product.Id;
            _insuranceModel.ProductTypeName = productType.Name;
            _insuranceModel.ProductTypeHasInsurance = productType.CanBeInsured;
            _insuranceModel.ProductTypeId = productType.Id;
        }

        private void BuildSalesPrice(Product product)
        {
            _insuranceModel.SalesPrice = product.SalesPrice;
        }
    }
}
