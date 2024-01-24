using examNo1.application.Dtos.Errors;
using Swashbuckle.AspNetCore.Filters;

namespace exam_no1.SwaggerExamples;

public class ErrorResponseExample : IExamplesProvider<ErrorViewModel>
{
    public ErrorViewModel GetExamples()
    {
        return new ErrorViewModel
        {
            Status = StatusCodes.Status500InternalServerError,
            Message = "Internal Server Error"
        };
    }
}
