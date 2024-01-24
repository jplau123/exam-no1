using AutoFixture;
using AutoMapper;
using Domain.Exceptions;
using examNo1.application.Dtos.Responses;
using examNo1.application.Interfaces;
using examNo1.application.Mappers;
using examNo1.application.Services;
using examNo1.domain.Clients;
using examNo1.domain.Entities;
using examNo1.domain.Responses;
using FluentAssertions;
using Moq;
using System.Net;

namespace examNo1.application.Tests.Services;

public class ExchangeRateServiceTests
{
    private readonly Mock<IExchangeRateApiClient> _exchangeRateApiClientMock;
    private readonly IMapper _mapper;
    private readonly Fixture _fixture;
    private readonly IExchangeRateService _exchangeRateService;

    public ExchangeRateServiceTests()
    {
        _exchangeRateApiClientMock = new Mock<IExchangeRateApiClient>();
        _mapper = new Mapper(
            new MapperConfiguration(
                cfg => cfg.AddProfile<AutoMapperProfile>())
            );
        _fixture = new Fixture();
        _exchangeRateService = new ExchangeRateService(_exchangeRateApiClientMock.Object, _mapper);
    }

    [Fact]
    public async Task GetCurrencyExchangeRateChanges_ValidDate_ShouldReturnValidResult()
    {
        // Arrange
        DateTime dateSelected = DateTime.Now;
        DateTime dateRelative = DateTime.Now - TimeSpan.FromDays(1);

        List<ExchangeRateItemEntity> resultSelected = _fixture.Build<ExchangeRateItemEntity>()
            .Without(entity => entity.DateString)
            .With(entity => entity.Date, dateSelected)
            .CreateMany(count: 5).ToList();

        List<ExchangeRateItemEntity> resultRelative = _fixture.Build<ExchangeRateItemEntity>()
            .Without(entity => entity.DateString)
            .With(entity => entity.Date, dateSelected - TimeSpan.FromDays(1))
            .CreateMany(count: 5).ToList();

        _exchangeRateApiClientMock.Setup(x => x.GetAllExchangeRatesByDate(dateSelected))
            .ReturnsAsync(new ExchangeRateApiResult<ExchangeRatesEntity>
            {
                StatusCode = HttpStatusCode.OK,
                Data = new ExchangeRatesEntity { Items = resultSelected }
            });

        _exchangeRateApiClientMock.Setup(x => x.GetAllExchangeRatesByDate(dateRelative))
            .ReturnsAsync(new ExchangeRateApiResult<ExchangeRatesEntity>
            {
                StatusCode = HttpStatusCode.OK,
                Data = new ExchangeRatesEntity { Items = resultRelative }
            });

        // Act
        var result = await _exchangeRateService.GetCurrencyExchangeRateChanges(dateSelected);

        // Assert
        result.Should().BeAssignableTo<List<ExchangeRateResponse>>();
        result.Should().NotBeNull();

        // Verify
        _exchangeRateApiClientMock.Verify(x => x.GetAllExchangeRatesByDate(dateSelected), Times.Once);
        _exchangeRateApiClientMock.Verify(x => x.GetAllExchangeRatesByDate(dateRelative), Times.Once);
    }

    [Fact]
    public async Task GetCurrencyExchangeRateChanges_InvalidDate_ShouldThrowNotFoundException()
    {
        // Arrange
        DateTime dateSelected = DateTime.Now + TimeSpan.FromDays(10000);

        _exchangeRateApiClientMock.Setup(x => x.GetAllExchangeRatesByDate(dateSelected))
            .ReturnsAsync(
            new ExchangeRateApiResult<ExchangeRatesEntity> { 
                StatusCode = HttpStatusCode.NotFound, 
                ErrorMessage = "Not found" }
            );

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(async () => await _exchangeRateService.GetCurrencyExchangeRateChanges(dateSelected));

        // Verify
        _exchangeRateApiClientMock.Verify(x => x.GetAllExchangeRatesByDate(dateSelected), Times.Once);
    }
}
