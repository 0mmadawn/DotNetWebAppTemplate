using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using web.Common.Extensions;
using web.Models.Parameters;
using web.Services;

namespace web.Controllers
{
    public class UserController : Controller
    {
        private const string SESSION_KEY_ID = "USER_SESSION_ID";

        private readonly IUserService service;

        public UserController(IUserService service)
        {
            this.service = service;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = service.GetIndexViewModel();
            return View(model);
        }

        [HttpGet]
        public IActionResult Show(UserParamModel param)
        {
            // IDのエラーのみ検証
            // このためにParamModelにIDをもたせるくらいならIDのチェックはRoute制約で行うのもアリ
            var hasIdError = ModelState.HasError(nameof(param.Id));
            if (hasIdError) { return NotFound(); }
            var model = service.GetShowViewModel(param.Id);
            if (model is null) { return NotFound(); }

            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(UserParamModel param)
        {
            // IDのエラーのみ検証
            var hasIdError = ModelState.HasError(nameof(param.Id));
            if (hasIdError) { return NotFound(); }
            var model = service.GetEditViewModel(param.Id);
            if (model is null) { return NotFound(); }

            // リクエスト改竄防止
            HttpContext.Session.SetInt32(SESSION_KEY_ID, model.User.Id);

            return View(model);
        }

        [HttpPost]
        public IActionResult Update(UserParamModel param)
        {
            var sessionId = HttpContext.Session.GetInt32(SESSION_KEY_ID);
            if (sessionId is null || sessionId != param.Id)
            {
                HttpContext.Session.Remove(SESSION_KEY_ID);
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                var editModel = service.GetEditViewModel(param);
                editModel.Errors = ModelState.ToErrorDictionary();
                return View(viewName: "Edit", editModel);
            }

            service.UpdateUser(param);
            var model = service.GetShowViewModel(param.Id);
            HttpContext.Session.Remove(SESSION_KEY_ID);
            return View(viewName: "Show", model);
        }

        // TOOD: delete
        // エラーを明示的に発生させるためのテスト用のAction
        // Startup.csのapp.UseExceptionHandlerによって拾われる
        public IActionResult ErrorSample()
            => throw new Exception("エラーのテスト");
    }
}
