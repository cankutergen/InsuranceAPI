using Insurance.Business.Abstract;
using Insurance.Business.Builder.Insurance;
using Insurance.Business.Concrete;
using Insurance.Core.Logging;
using Insurance.Entities.Concrete;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Business.Tests
{
    public class InsuranceManagerTests
    {
        private readonly Mock<IProductService> _productServiceMock;
        private readonly Mock<IProductTypeService> _productTypeServiceMock;
        private readonly Mock<ILogBuilder> _logBuilderMock;

        private readonly InsuranceManager _insuranceManager;

        public InsuranceManagerTests()
        {
            _productServiceMock = new Mock<IProductService>();
            _productTypeServiceMock = new Mock<IProductTypeService>();
            _logBuilderMock = new Mock<ILogBuilder>();

            _insuranceManager = new InsuranceManager(_productServiceMock.Object, _productTypeServiceMock.Object, _logBuilderMock.Object);
        }

        [Fact]
        public async Task PopulateInsuranceByProductId_GivenProductId_ShouldReturnCorrospondingInsuranceObject()
        {
            InsuranceModel insuranceModel = new InsuranceModel
            {
                InsuranceValue = 0,
                ProductId = 1,
                ProductTypeHasInsurance = true,
                ProductTypeName = "Laptops",
                SalesPrice = 500
            };

            Product product = new Product { Id = 1, Name = "Macbook", ProductTypeId = 1, SalesPrice = 500 };
            ProductType productType = new ProductType { CanBeInsured = true, Id = 1, Name = "Laptops" };

            _productServiceMock.Setup(x => x.GetProductById(1))
                .Returns(Task.FromResult(product));

            _productTypeServiceMock.Setup(x => x.GetProductTypeById(1))
                .Returns(Task.FromResult(productType));

            var result = await _insuranceManager.PopulateInsuranceByProductId(1);
            Assert.Equal(result.ProductId, insuranceModel.ProductId);
        }

        [Fact]
        public async Task PopulateInsuranceByProductId_GivenProductIdWithNullProductResponse_ShouldThrowError()
        {
            _productServiceMock.Setup(x => x.GetProductById(1))
                .Returns(Task.FromResult((Product)null));


            Task result() => _insuranceManager.PopulateInsuranceByProductId(1);
            await Assert.ThrowsAsync<Exception>(result);
        }

        [Fact]
        public async Task PopulateInsuranceByProductId_GivenProductIdWithNullproductTypeResponse_ShouldThrowError()
        {
            _productServiceMock.Setup(x => x.GetProductById(1))
                .Returns(Task.FromResult(new Product {ProductTypeId = 1}));

            _productTypeServiceMock.Setup(x => x.GetProductTypeById(1))
                .Throws(new Exception());

            Task result() => _insuranceManager.PopulateInsuranceByProductId(1);
            await Assert.ThrowsAsync<Exception>(result);
        }

        [Fact]
        public void CalculateInsuranceAmount_GivenHasInsuranceFalse_ShouldReturnZero()
        {
            float expectedInsuranceAmount = 0;

            InsuranceModel insuranceModel = new InsuranceModel
            {
                InsuranceValue = 0,
                ProductId = 1,
                ProductTypeHasInsurance = false,
                ProductTypeName = "Laptops",
                SalesPrice = 500
            };

            var result = _insuranceManager.CalculateInsuranceAmount(insuranceModel);
            Assert.Equal(expectedInsuranceAmount, result.InsuranceValue);
        }

        [Fact]
        public void CalculateInsuranceAmount_GivenSalesPriceLessThan500Euros_ShouldReturnZero()
        {
            float expectedInsuranceAmount = 0;

            InsuranceModel insuranceModel = new InsuranceModel
            {
                InsuranceValue = 0,
                ProductId = 1,
                ProductTypeHasInsurance = true,
                ProductTypeName = "Cameras",
                SalesPrice = 499
            };

            var result = _insuranceManager.CalculateInsuranceAmount(insuranceModel);
            Assert.Equal(expectedInsuranceAmount, result.InsuranceValue);
        }

        [Fact]
        public void CalculateInsuranceAmount_GivenSalesPriceLessThan500EurosAndProductTypeLaptops_ShouldAdd500EurosToInsuranceCost()
        {
            float expectedInsuranceAmount = 500;

            InsuranceModel insuranceModel = new InsuranceModel
            {
                InsuranceValue = 0,
                ProductId = 1,
                ProductTypeHasInsurance = true,
                ProductTypeName = "Laptops",
                ProductTypeId = 21,
                SalesPrice = 499
            };

            var result = _insuranceManager.CalculateInsuranceAmount(insuranceModel);
            Assert.Equal(expectedInsuranceAmount, result.InsuranceValue);
        }

        [Fact]
        public void CalculateInsuranceAmount_GivenSalesPriceBetween500And2000Euros_ShouldAdd1000EurosToInsuranceCost()
        {
            float expectedInsuranceAmount = 1000;

            InsuranceModel insuranceModel = new InsuranceModel
            {
                InsuranceValue = 0,
                ProductId = 1,
                ProductTypeHasInsurance = true,
                ProductTypeName = "Cameras",
                SalesPrice = 1500
            };

            var result = _insuranceManager.CalculateInsuranceAmount(insuranceModel);
            Assert.Equal(expectedInsuranceAmount, result.InsuranceValue);
        }

        [Fact]
        public void CalculateInsuranceAmount_GivenSalesPriceBetween500And2000EurosAndProductTypeSmartphones_ShouldAdd1500EurosToInsuranceCost()
        {
            float expectedInsuranceAmount = 1500;

            InsuranceModel insuranceModel = new InsuranceModel
            {
                InsuranceValue = 0,
                ProductId = 1,
                ProductTypeHasInsurance = true,
                ProductTypeName = "Smartphones",
                ProductTypeId = 32,
                SalesPrice = 1500
            };

            var result = _insuranceManager.CalculateInsuranceAmount(insuranceModel);
            Assert.Equal(expectedInsuranceAmount, result.InsuranceValue);
        }

        [Fact]
        public void CalculateInsuranceAmount_GivenSalesPriceHigherThan2000Euros_ShouldAdd2000EurosToInsuranceCost()
        {
            float expectedInsuranceAmount = 2000;

            InsuranceModel insuranceModel = new InsuranceModel
            {
                InsuranceValue = 0,
                ProductId = 1,
                ProductTypeHasInsurance = true,
                ProductTypeName = "Washing machine",
                SalesPrice = 2390
            };

            var result = _insuranceManager.CalculateInsuranceAmount(insuranceModel);
            Assert.Equal(expectedInsuranceAmount, result.InsuranceValue);
        }

        [Fact]
        public void CalculateInsuranceAmount_GivenSalesPriceHigherThan2000EurosAndProductTypeSmartphones_ShouldAdd2500EurosToInsuranceCost()
        {
            float expectedInsuranceAmount = 2500;

            InsuranceModel insuranceModel = new InsuranceModel
            {
                InsuranceValue = 0,
                ProductId = 1,
                ProductTypeHasInsurance = true,
                ProductTypeName = "Smartphones",
                ProductTypeId = 32,
                SalesPrice = 2390
            };

            var result = _insuranceManager.CalculateInsuranceAmount(insuranceModel);
            Assert.Equal(expectedInsuranceAmount, result.InsuranceValue);
        }
    }
}
