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
    public class HomeControllerTests
    {
        private readonly Mock<IInsuranceService> _insuranceServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogBuilder> _logBuilderMock;

        private readonly HomeController _controller;

        public HomeControllerTests()
        {
            _insuranceServiceMock = new Mock<IInsuranceService>();
            _mapperMock = new Mock<IMapper>();
            _logBuilderMock = new Mock<ILogBuilder>();

            _controller = new HomeController(_insuranceServiceMock.Object, _mapperMock.Object, _logBuilderMock.Object);
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
                .Setup(x => x.Map<InsuranceApiModel>(It.IsAny<InsuranceModel>()))
                .Returns(new InsuranceApiModel());

            var result = await _controller.CalculateInsurance(new InsuranceApiModel()) as OkObjectResult;
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task CalculateInsurance_GivenInvalidInput_ShouldReturnBadRequestResult()
        {
            _insuranceServiceMock
                .Setup(x => x.PopulateInsuranceByProductId(It.IsAny<int>()))
                .Throws(new NullReferenceException());

            var result = await _controller.CalculateInsurance(new InsuranceApiModel()) as BadRequestObjectResult;
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}