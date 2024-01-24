using AutoMapper;
using Domain.Exceptions;
using examNo1.application.Dtos.Internal;
using examNo1.application.Dtos.Responses;
using examNo1.application.Interfaces;
using examNo1.domain.Clients;
using examNo1.domain.Responses;
using System.Net;

namespace examNo1.application.Services;

public class ExchangeRateService : IExchangeRateService
{
    private readonly IExchangeRateApiClient _exchangeRateApiClient;
    private readonly IMapper _mapper;

    public ExchangeRateService(IExchangeRateApiClient exchangeRateApiClient, IMapper mapper)
    {
        _exchangeRateApiClient = exchangeRateApiClient;
        _mapper = mapper;
    }

    public async Task<List<ExchangeRateResponse>> GetCurrencyExchangeRateChanges(DateTime dateSelected)
    {
        var resultSelected = await GetCurrencyExchangeRates(dateSelected);

        // Calculate relative date (one day before)
        DateTime dateRelative = dateSelected - TimeSpan.FromDays(1);

        var resultRelative = await GetCurrencyExchangeRates(dateRelative);

        var changeOfExchangeRates = CalculateExchangeRateChanges(resultSelected, resultRelative);

        return _mapper.Map<List<ExchangeRateResponse>>(changeOfExchangeRates);
    }

    private async Task<List<ExchangeRateInternal>> GetCurrencyExchangeRates(DateTime date)
    {
        var resultSelected = await _exchangeRateApiClient.GetAllExchangeRatesByDate(date);
        HandleExceptions(resultSelected);

        return _mapper.Map<List<ExchangeRateInternal>>(resultSelected.Data!.Items);
    }

    private List<ExchangeRateInternal> CalculateExchangeRateChanges(
        List<ExchangeRateInternal> exchangeRatesSelected, 
        List<ExchangeRateInternal> exchangeRatesRelative
    ){
        return exchangeRatesSelected
            .Zip(exchangeRatesRelative, (selected, relative) =>
            new ExchangeRateInternal
            {
                Date = selected.Date,
                Currency = selected.Currency,
                Quantity = selected.Quantity,
                Rate = selected.Rate - relative.Rate,
                Unit = selected.Unit
            })
            .ToList();
    }

    private static void HandleExceptions<T>(ExchangeRateApiResult<T> exchangeRateApiResult) where T : class
    {
        if (exchangeRateApiResult.StatusCode == HttpStatusCode.NotFound)
        {
            throw new NotFoundException(exchangeRateApiResult.ErrorMessage);
        }
        else if ((int)exchangeRateApiResult.StatusCode >= 400)
        {
            throw new Exception(exchangeRateApiResult.ErrorMessage);
        }
    }
}
