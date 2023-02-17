using AutoMapper;
using Insurance.Api.Controllers;
using Insurance.Api.Models;
using Insurance.Business.Abstract;
using Insurance.Core.Logging;
using Insurance.Entities.Concrete;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Insurance.Api.Tests
{
    public class InsuranceControllerTests
    {
        private readonly Mock<IInsuranceService> _insuranceServiceMock;
        private readonly Mock<IInsuranceOrderService> _insuranceOrderServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogBuilder> _logBuilderMock;

        private readonly InsuranceController _controller;

        public InsuranceControllerTests()
        {
            _insuranceServiceMock = new Mock<IInsuranceService>();
            _insuranceOrderServiceMock = new Mock<IInsuranceOrderService>();
            _mapperMock = new Mock<IMapper>();
            _logBuilderMock = new Mock<ILogBuilder>();

            _controller = new InsuranceController(_insuranceServiceMock.Object, _insuranceOrderServiceMock.Object, _mapperMock.Object, _logBuilderMock.Object);
        }

        [Fact]
        public async Task CalculateInsurance_GivenValidInput_ShouldReturnOkResult()
        {
            _insuranceServiceMock
                .Setup(x => x.PopulateInsuranceByProductId(It.IsAny<int>()))
                .Returns(Task.FromResult(new InsuranceModel()));

            _insuranceServiceMock
                .Setup(x => x.CalculateInsuranceAmount(It.IsAny<InsuranceModel>()))
                .Returns(new InsuranceModel());

            _mapperMock
                .Setup(x => x.Map<InsuranceResponseModel>(It.IsAny<InsuranceModel>()))
                .Returns(new InsuranceResponseModel());

            var result = await _controller.CalculateInsurance(new InsuranceRequestModel()) as OkObjectResult;
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task CalculateInsurance_GivenInvalidInputToInsuranceService_ShouldReturnBadRequestResult()
        {
            _insuranceServiceMock
                .Setup(x => x.PopulateInsuranceByProductId(It.IsAny<int>()))
                .Throws(new NullReferenceException());

            var result = await _controller.CalculateInsurance(new InsuranceRequestModel()) as BadRequestObjectResult;
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task CalculateInsurance_GivenInvalidInputToApi_ShouldReturnBadRequestResult()
        {
            var result = await _controller.CalculateInsurance(null) as BadRequestObjectResult;
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task CalculateOrderInsurance_GivenValidInput_ShouldReturnOkResult()
        {
            InsuranceOrderRequestModel requestModel = new InsuranceOrderRequestModel();
            requestModel.productIdList = new List<int>();

            _insuranceOrderServiceMock
                .Setup(x => x.PopulateInsuranceOrderByProductIdList(new List<int>()))
                .Returns(Task.FromResult(new InsuranceOrder()));

            _mapperMock
                .Setup(x => x.Map<InsuranceOrderResponseModel>(It.IsAny<InsuranceOrder>()))
                .Returns(new InsuranceOrderResponseModel());

            var result = await _controller.CalculateOrderInsurance(requestModel) as OkObjectResult;
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task CalculateOrderInsurance_GivenInvalidInputToInsuranceService_ShouldReturnBadRequestResult()
        {
            _insuranceServiceMock
                .Setup(x => x.PopulateInsuranceByProductId(It.IsAny<int>()))
                .Throws(new NullReferenceException());

            var result = await _controller.CalculateOrderInsurance(new InsuranceOrderRequestModel()) as BadRequestObjectResult;
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task CalculateOrderInsurance_GivenInvalidInputToApi_ShouldReturnBadRequestResult()
        {
            var result = await _controller.CalculateOrderInsurance(null) as BadRequestObjectResult;
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task CalculateOrderInsurance_GivenInvalidListToApi_ShouldReturnBadRequestResult()
        {
            var requestModel = new InsuranceOrderRequestModel();

            var result = await _controller.CalculateOrderInsurance(requestModel) as BadRequestObjectResult;
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}