using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sino;
using Sino.ViewModels;
using System;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder
{
	/// <summary>
	/// 异常默认输出
	/// </summary>
    public static class ExceptionHandlerMiddleware
	{
		public const string EXCEPTION_ERROR_NAME = "GlobalError";

		/// <summary>
		/// 未知异常ErrorCode
		/// </summary>
		public static string DEFAULT_ERROR_CODE = "-1";

		/// <summary>
		/// 未知异常ErrorMessage
		/// </summary>
		public static string DEFAULT_ERROR_MESSAGE = "未知错误";

		private static ILogger _log;

		public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app, ILoggerFactory loggerFactory)
		{
			if (app == null)
				throw new ArgumentNullException(nameof(app));
			if (loggerFactory == null)
				throw new ArgumentNullException(nameof(loggerFactory));

			_log = loggerFactory.CreateLogger(EXCEPTION_ERROR_NAME);
			return app.UseExceptionHandler(new ExceptionHandlerOptions
			{
				ExceptionHandler = Invoke
			});
		}

		public static async Task Invoke(HttpContext context)
		{
			context.Response.StatusCode = 200;
			context.Response.ContentType = "application/json";

			BaseResponse response = new BaseResponse
			{
				success = false,
				errorCode = DEFAULT_ERROR_CODE,
				errorMessage = DEFAULT_ERROR_MESSAGE
			};

			var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerFeature>();
			if (exceptionHandlerPathFeature?.Error is SinoException)
			{
				var sex = exceptionHandlerPathFeature.Error as SinoException;
				response.errorCode = sex.Code.ToString();
				response.errorMessage = sex.Message;
			}

			if (exceptionHandlerPathFeature?.Error != null)
			{
				var errorLog = new
				{
					context.Request.Path,
					context.TraceIdentifier,
					exceptionHandlerPathFeature.Error.Message
				};
				_log.LogError(exceptionHandlerPathFeature.Error, JsonConvert.SerializeObject(errorLog));
			}
			await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
		}
	}
}
