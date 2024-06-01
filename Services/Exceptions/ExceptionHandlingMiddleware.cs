using CodeMechanic.Types;
using evantage.Models;
using evantage.Pages.Logs;
using evantage.Services;
using Microsoft.AspNetCore.Mvc;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly IGlobalLoggingService global_logging_svc;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger
        , IGlobalLoggingService loggingService
    )
    {
        _next = next;
        _logger = logger;
        global_logging_svc = loggingService;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            // Console.WriteLine(nameof(InvokeAsync));
            _logger.LogError(
                exception, "Exception occurred: {Message}", exception.Message);


            var lrs = new LogRecord()
            {
                exception_message = exception.Message
            }.AsList();

            // var res = await global_logging_svc.BulkUpsertLogs(lrs);
            // res.Dump("logs upserted");

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Server Error"
            };

            context.Response.StatusCode =
                StatusCodes.Status500InternalServerError;

            context.Response.Redirect("/error");


            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }

    // public async Task InvokeAsync(HttpContext context)
    // {
    //     try
    //     {
    //         await next(context);
    //     }
    //     catch (Exception ex)
    //     {
    //         await HandleExceptionAsync(context, ex);
    //     }
    // }

    // private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    // {
    //     var code = HttpStatusCode.InternalServerError; // 500 if unexpected
    //     // only change the result if this was an XmlHttp request, 
    //     // otherwise let the global Exceptionhandler move the page along.
    //     if (!string.IsNullOrEmpty(context.Request.Headers["x-requested-with"]))
    //     {
    //         if (context.Request.Headers["x-requested-with"][0]
    //                 .ToLower() == "xmlhttprequest")
    //         {
    //             var result = JsonConvert.SerializeObject(new { error = ex.Message });
    //             context.Response.ContentType = "application/json";
    //             context.Response.StatusCode = (int)code;
    //             return context.Response.WriteAsync(result);
    //         }
    //     }
    //
    //     context.Response.Redirect("/errors/500");
    //     return Task.FromResult<object>(null);
    // }
}