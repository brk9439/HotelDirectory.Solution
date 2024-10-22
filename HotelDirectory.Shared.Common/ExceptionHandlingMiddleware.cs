using HotelDirectory.Shared.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using HotelDirectory.Shared.Common;
using HotelDirectory.Shared.ElasticSearch;
using HotelDirectory.Shared.ElasticSearch.Model;
using Type = HotelDirectory.Shared.ElasticSearch.Model.Type;


public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context); // İstek işleme devam et
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        //_logger.AddLog(new GenericLogModel { Object = exception, Type = Type.Error, Message = exception.Message});

        var response = new BaseResponseModel<object>
        {
            Success = false,
            Message = "Bir hata oluştu. Lütfen daha sonra tekrar deneyin.",
            Data = null,
            StatusCode = HttpStatusCode.InternalServerError
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var json = System.Text.Json.JsonSerializer.Serialize(response);
        return context.Response.WriteAsync(json);
    }
}
