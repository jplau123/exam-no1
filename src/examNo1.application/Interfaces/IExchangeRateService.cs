using examNo1.application.Dtos.Responses;

namespace examNo1.application.Interfaces;

public interface IExchangeRateService
{
    public Task<List<ExchangeRateResponse>> GetCurrencyExchangeRateChanges(DateTime date);
}
