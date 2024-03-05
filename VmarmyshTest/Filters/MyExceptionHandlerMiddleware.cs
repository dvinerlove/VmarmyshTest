using System;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using VmarmyshTest.Models;

namespace VmarmyshTest.Filters
{
    public class MyExceptionHandlerMiddleware
    {
        //private readonly AppDbContext _context;
        private readonly RequestDelegate _next;

        public MyExceptionHandlerMiddleware(RequestDelegate next/*, AppDbContext context*/)
        {
            //_context = context;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                LogExceptionToDatabase(context, ex);

                await HandleExceptionMessageAsync(context, ex).ConfigureAwait(false);
            }
        }
        private static Task HandleExceptionMessageAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            int statusCode = (int)HttpStatusCode.InternalServerError;
            var result = JsonConvert.SerializeObject(new
            {
                Type = exception.GetType().Name,
                ID = Guid.NewGuid(),
                Data = new { Message = exception.Message }
            });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            return context.Response.WriteAsync(result);
        }

        private void LogExceptionToDatabase(HttpContext context, Exception exception)
        {
            var _context = context.RequestServices.GetRequiredService<AppDbContext>();

            var id = Guid.NewGuid();
            _context.JournalInfos.Add(new() { EventId = id });
            _context.Events.Add(new()
            {
                EventId = id,
                Text = $"request ID: {id}\n" +
                    $"{exception.Message}\n" +
                    $"{exception.StackTrace}"
            });
            _context.SaveChanges();
        }
    }
}
