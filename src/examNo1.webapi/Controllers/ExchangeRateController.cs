using exam_no1.SwaggerExamples;
using examNo1.application.Dtos.Errors;
using examNo1.application.Dtos.Responses;
using examNo1.application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System.ComponentModel;
using System.Net;
using System.Net.Mime;

namespace exam_no1.Controllers;

[SwaggerTag("Controls currencies exchange rates and history")]
[Route("/exchange-rates")]
[ApiController]
public class ExchangeRateController : ControllerBase
{
    private readonly IExchangeRateService _exchangeRateService;

    public ExchangeRateController(IExchangeRateService exchangeRateService)
    {
        _exchangeRateService = exchangeRateService;
    }

    [HttpGet("changes")]
    [Consumes(MediaTypeNames.Text.Plain)]
    [SwaggerOperation(Summary = "Changes of currency rates",
        Description = "Gets changes of currency rates for the selected date.")]
    [SwaggerResponse(StatusCodes.Status200OK, 
        "The list of currencies and their exchange ratee changes", 
        typeof(IEnumerable<ExchangeRateResponse>), 
        MediaTypeNames.Application.Json)]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized")]
    [SwaggerResponse(StatusCodes.Status404NotFound)]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, 
        "Internal Server Error", 
        typeof(ErrorViewModel),
        MediaTypeNames.Application.Json)]
    [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ExchangeRateResponseExample))]
    [SwaggerResponseExample((int)HttpStatusCode.InternalServerError, typeof(ErrorResponseExample))]
    public async Task<IActionResult> GetExchangeRateChanges([FromQuery, DefaultValue("2001.01.01")] DateTime date)
    {
        return Ok(await _exchangeRateService.GetCurrencyExchangeRateChanges(date));
    }
}
