using examNo1.domain.Clients;
using examNo1.domain.Configuration;
using examNo1.domain.Entities;
using examNo1.domain.Responses;
using Microsoft.Extensions.Options;
using System.Xml;
using System.Xml.Serialization;
using System.Net;

namespace examNo1.infrastructure.Clients;

public class ExchangeRateApiClient : IExchangeRateApiClient
{
    private IHttpClientFactory _httpClient;
    private readonly DateOptions _dateOptions;

    public ExchangeRateApiClient(
        IHttpClientFactory httpClient,
        IOptions<DateOptions> dateOptions)
    {
        _httpClient = httpClient;
        _dateOptions = dateOptions.Value;
    }

    public async Task<ExchangeRateApiResult<ExchangeRatesEntity>> GetAllExchangeRatesByDate(DateTime date)
    {
        var httpClient = _httpClient.CreateClient();

        string dateTransformed = date.ToString(_dateOptions.Format);

        using (var response = await httpClient.GetAsync($"https://www.lb.lt/webservices/ExchangeRates/ExchangeRates.asmx/getExchangeRatesByDate?Date={dateTransformed}"))
        {
            return await CreateResult<ExchangeRatesEntity>(response);
        }
    }

    // Creates and returns Client result object
    private async Task<ExchangeRateApiResult<T>> CreateResult<T>(HttpResponseMessage response) where T : class
    {
        var stream = await response.Content.ReadAsStreamAsync();

        if (response.IsSuccessStatusCode)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(T));

                return new ExchangeRateApiResult<T>
                {
                    StatusCode = response.StatusCode,
                    Data = (T?)serializer.Deserialize(stream),
                };
            }
            catch(Exception)
            {
                return await CreateErrorResult<T>(stream);
            }
        }

        // default error message
        return new ExchangeRateApiResult<T>
        {
            StatusCode = response.StatusCode,
            ErrorMessage = "Failed to fetch data from the server."
        };
    }

    // Deserialises xml
    private static T? DeserializeXml<T>(Stream stream)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(T));

        return (T?)serializer.Deserialize(stream);
    }

    // Creates and returns error result object
    private async Task<ExchangeRateApiResult<T>> CreateErrorResult<T>(Stream stream) where T : class
    {
        stream.Position = 0;

        XmlNode? xmlNode = DeserializeXml<XmlNode>(stream);

        return await Task.FromResult(new ExchangeRateApiResult<T>
        {
            StatusCode = HttpStatusCode.NotFound,
            ErrorMessage = xmlNode!.InnerXml,
        });
    }
}
