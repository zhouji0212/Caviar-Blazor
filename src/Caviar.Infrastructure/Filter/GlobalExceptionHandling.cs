﻿using Caviar.Core.Services;
using Caviar.SharedKernel.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Caviar.Infrastructure
{
    public class GlobalExceptionHandling
    {
        private readonly RequestDelegate _next;
        private readonly LogServices<GlobalExceptionHandling> _logServices;
        public GlobalExceptionHandling(RequestDelegate next,LogServices<GlobalExceptionHandling> logServices)
        {
            _next = next;
            _logServices = logServices;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (ResultException ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, ResultException ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 200;
            var json = JsonSerializer.Serialize(ex.ResultMsg);
            return context.Response.WriteAsync(json);
        }


        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var syslog = _logServices.Error(ex.InnerException.Message);
            //todo
            ResultMsg resultMsg = new ResultMsg()
            {
                Status = System.Net.HttpStatusCode.InternalServerError,
                Title = CurrencyConstant.InternalServerError,
                Detail = new Dictionary<string, string>()
                {
                    { "异常信息",ex.Message },
                    { "异常已记录","异常id：" + syslog.Id},
                }
            };
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 200;
            var json = JsonSerializer.Serialize(resultMsg);
            return context.Response.WriteAsync(json);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class CustomExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GlobalExceptionHandling>();
        }
    }
}

