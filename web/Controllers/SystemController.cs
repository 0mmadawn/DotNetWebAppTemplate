using System;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace web.Controllers
{
    // エラーハンドリングしかしてないけど
    /// <summary>システム基盤コントローラ</summary>
    public class SystemController : Controller
    {
        /// <summary>ILogger</summary>
        private readonly ILogger<SystemController> _logger;

        /// <summary>コンストラクタ</summary>
        /// <param name="logger">ILogger</param>
        public SystemController(ILogger<SystemController> logger)
        {
            _logger = logger;
        }

        /// <summary>エラーハンドラ</summary>
        /// <param name="statusCode">HttpStatusCode</param>
        /// <returns>エラーに基づいたView</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(HttpStatusCode? statusCode)
        {
            switch (statusCode)
            {
                //404
                case HttpStatusCode.NotFound:
                    return View(viewName: ((int)HttpStatusCode.NotFound).ToString());
                //500 or エラーページへの直アクセス
                case HttpStatusCode.InternalServerError:
                default:
                    var exceptionHandlerPathFeature =
                        HttpContext.Features.Get<IExceptionHandlerPathFeature>();
                    // errorページへの直アクセスはログなし
                    var error = exceptionHandlerPathFeature?.Error;
                    if (error != null)
                    {
                        _logger.LogError(error, $"{error.Message}{Environment.NewLine}{error.StackTrace}");
                    }
                    return View(viewName: ((int)HttpStatusCode.InternalServerError).ToString());
            }
        }
    }
}
