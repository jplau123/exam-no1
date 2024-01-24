using examNo1.domain.Entities;
using examNo1.domain.Responses;

namespace examNo1.domain.Clients;

public interface IExchangeRateApiClient
{
    public Task<ExchangeRateApiResult<ExchangeRatesEntity>> GetAllExchangeRatesByDate(DateTime date);
}
