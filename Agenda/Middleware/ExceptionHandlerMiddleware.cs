using Agenda.Middleware;
using Agenda.Models;
using Newtonsoft.Json;
using System.Net;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";

        var errorDetails = new ErrorDetails
        {
            Details = ex.Message
        };

        switch (ex)
        {
            case NotFoundException:
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                errorDetails.Message = "O recurso solicitado não foi encontrado.";
                break;
            case BadRequestException:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorDetails.Message = "A solicitação foi malformada ou inválida.";
                break;
            case UnauthorizedException:
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                errorDetails.Message = "Você não tem autorização para realizar essa operação.";
                break;
            case ConflictException:
                context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                errorDetails.Message = "Um conflito foi indentificado.";
                break;
            default:
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                errorDetails.Message = "Ocorreu um erro interno no servidor.";
                break;
        }

        errorDetails.StatusCode = context.Response.StatusCode;

        var result = JsonConvert.SerializeObject(errorDetails);
        return context.Response.WriteAsync(result);
    }
}
