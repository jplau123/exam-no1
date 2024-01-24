using examNo1.application.Dtos.Responses;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.Metrics;
using System.Runtime.ConstrainedExecution;

namespace exam_no1.SwaggerExamples;

public class ExchangeRateResponseExample : IExamplesProvider<List<ExchangeRateResponse>>
{
    public List<ExchangeRateResponse> GetExamples()
    {
        return new List<ExchangeRateResponse>
        {
            new ExchangeRateResponse 
            { 
                Date = DateTime.Now,
                Currency = "AED",
                Quantity = 10,
                Rate = 6.7800m,
                Unit = "LTL per 10 currency units"
            },
        };
    }
}
