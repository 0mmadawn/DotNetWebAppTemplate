using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using web.Common.Enums;
using web.Controllers;
using web.Models.Entities;
using web.Models.Parameters;
using web.Services;
using web.test.TestUtils;
using web.ViewModels.Users;
using Xunit;

namespace web.test.Controllers
{
    public class UserControllerTest
    {
        private Mock<IUserService> serviceMock;
        private IUserService service;
        private MockHttpSession mockSession { get; set; }
        private Mock<HttpContext> mockHttpContext { get; set; }

        public UserControllerTest()
        {
            serviceMock = new Mock<IUserService>();
            service = serviceMock.Object;

            // Edit, Update用
            mockHttpContext = new Mock<HttpContext>();
            mockSession = new MockHttpSession();
        }

        public class IndexTest
        {
            private UserControllerTest test;

            public IndexTest()
            {
                test = new UserControllerTest();
            }

            [Fact]
            public void IndexTest_正常系()
            {
                test.serviceMock
                    .Setup(service => service.GetIndexViewModel())
                    .Returns(new IndexViewModel());

                var controller = new UserController(test.service);
                var actionResult = controller.Index();

                var viewResult = Assert.IsType<ViewResult>(actionResult);
                Assert.IsType<IndexViewModel>(viewResult.Model);
            }
        }

        public class ShowTest
        {
            private readonly User _testUser = new User(1, "Test", Group.Aeug, "TestDesc");

            private UserControllerTest test;

            public ShowTest()
            {
                test = new UserControllerTest();
            }

            [Fact]
            public void ShowTest_正常系()
            {
                var targetId = _testUser.Id;
                test.serviceMock
                    .Setup(service => service.GetShowViewModel(targetId))
                    .Returns(new ShowViewModel() { User = _testUser });

                var controller = new UserController(test.service);
                controller.ModelState.Clear();
                var actionResult = controller.Show(new UserParamModel() { Id = targetId });

                var viewResult = Assert.IsType<ViewResult>(actionResult);
                var model = Assert.IsType<ShowViewModel>(viewResult.Model);
                Assert.NotNull(model.User);
            }

            [Fact]
            public void ShowTest_準正常系_パラメータエラー()
            {
                var controller = new UserController(test.service);
                controller.ModelState.AddModelError(nameof(UserParamModel.Id), "some error");
                var param = new UserParamModel();
                var actionResult = controller.Show(param);

                // assert 404
                Assert.IsType<NotFoundResult>(actionResult);
            }

            [Fact]
            public void ShowTest_準正常系_ユーザ存在せず()
            {
                var targetId = 1; // IDはダミー（特に意味無し）
                test.serviceMock
                    .Setup(service => service.GetShowViewModel(targetId))
                    .Returns((ShowViewModel)null);
                var controller = new UserController(test.service);
                var actionResult = controller.Show(new UserParamModel() { Id = targetId });

                // assert 404
                Assert.IsType<NotFoundResult>(actionResult);
            }
        }

        // ShowTestとSessionのAssert以外同じ
        public class EditTest
        {
            private readonly User _testUser = new User(1, "Test", Group.Aeug, "TestDesc");
            private UserControllerTest test;

            public EditTest()
            {
                test = new UserControllerTest();

                // SesisonのテストのためHttpContextのMockを準備
                test.mockHttpContext.Setup(s => s.Session).Returns(test.mockSession);
            }

            [Fact]
            public void EditTest_正常系()
            {
                var targetId = _testUser.Id;
                test.serviceMock
                    .Setup(service => service.GetEditViewModel(targetId))
                    .Returns(new EditViewModel() { User = _testUser });

                var controller = new UserController(test.service);
                controller.ModelState.Clear();
                controller.ControllerContext.HttpContext = test.mockHttpContext.Object;

                var actionResult = controller.Edit(new UserParamModel() { Id = targetId });

                var viewResult = Assert.IsType<ViewResult>(actionResult);
                var model = Assert.IsType<EditViewModel>(viewResult.Model);
                Assert.NotNull(model.User);
                Assert.NotNull(test.mockSession.GetInt32("USER_SESSION_ID"));
            }

            [Fact]
            public void EditTest_準正常系_パラメータエラー()
            {
                var controller = new UserController(test.service);
                controller.ModelState.AddModelError(nameof(UserParamModel.Id), "some error");
                controller.ControllerContext.HttpContext = test.mockHttpContext.Object;

                var param = new UserParamModel();
                var actionResult = controller.Edit(param);

                // assert 404
                Assert.IsType<NotFoundResult>(actionResult);
                Assert.Null(test.mockSession.GetInt32("USER_SESSION_ID"));
            }

            [Fact]
            public void EditTest_準正常系_ユーザ存在せず()
            {
                var targetId = 1; // IDはダミー（特に意味無し）
                test.serviceMock
                    .Setup(service => service.GetEditViewModel(targetId))
                    .Returns((EditViewModel)null);

                var controller = new UserController(test.service);
                controller.ControllerContext.HttpContext = test.mockHttpContext.Object;

                var actionResult = controller.Edit(new UserParamModel() { Id = targetId });

                // assert 404
                Assert.IsType<NotFoundResult>(actionResult);
                Assert.Null(test.mockSession.GetInt32("USER_SESSION_ID"));
            }
        }

        public class UpdateTest
        {
            private readonly User _testUser = new User(1, "Test", Group.Aeug, "TestDesc");
            private UserControllerTest test;

            public UpdateTest()
            {
                test = new UserControllerTest();

                // SesisonのテストのためHttpContextのMockを準備                
                test.mockSession.SetInt32("USER_SESSION_ID", _testUser.Id);
                test.mockHttpContext.Setup(s => s.Session).Returns(test.mockSession);
            }

            [Fact]
            // Update 成功
            public void UpdateTest_正常系()
            {
                var targetId = _testUser.Id;
                var serviceMock = new Mock<IUserService>();
                serviceMock
                    .Setup(service => service.GetShowViewModel(targetId))
                    .Returns(new ShowViewModel() { User = _testUser });

                var controller = new UserController(serviceMock.Object);
                controller.ControllerContext.HttpContext = test.mockHttpContext.Object;
                controller.ModelState.Clear();

                // パラメータは特に意味なし
                var actionResult = controller.Update(new UserParamModel() { Id = targetId });

                var viewResult = Assert.IsType<ViewResult>(actionResult);
                var model = Assert.IsType<ShowViewModel>(viewResult.Model);
                Assert.NotNull(model.User);
                Assert.Null(test.mockSession.GetInt32("USER_SESSION_ID"));
            }

            [Fact]
            public void UpdateTest_準正常系_パラメータエラー()
            {
                var targetId = _testUser.Id;
                var param = new UserParamModel() { Id = targetId };
                var serviceMock = new Mock<IUserService>();
                serviceMock
                    .Setup(service => service.GetEditViewModel(param))
                    .Returns(new EditViewModel() { User = _testUser });

                var controller = new UserController(serviceMock.Object);
                controller.ControllerContext.HttpContext = test.mockHttpContext.Object;
                controller.ModelState.AddModelError(nameof(UserParamModel.Id), "some error");

                // ID以外のパラメータは特に意味なし
                var actionResult = controller.Update(param);

                var viewResult = Assert.IsType<ViewResult>(actionResult);
                var model = Assert.IsType<EditViewModel>(viewResult.Model);
                Assert.NotNull(model.User);
                Assert.NotNull(test.mockSession.GetInt32("USER_SESSION_ID"));
            }

            [Fact]
            public void UpdateTest_準正常系_セッションエラー_セッションなし()
            {
                var targetId = _testUser.Id;
                var serviceMock = new Mock<IUserService>();

                // セッションがないことを想定
                test.mockSession.Remove("USER_SESSION_ID");

                var controller = new UserController(serviceMock.Object);
                controller.ControllerContext.HttpContext = test.mockHttpContext.Object;
                controller.ModelState.Clear();

                // パラメータは特に意味なし
                var actionResult = controller.Update(new UserParamModel() { Id = targetId });

                // assert 404
                Assert.IsType<NotFoundResult>(actionResult);
                Assert.Null(test.mockSession.GetInt32("USER_SESSION_ID"));
            }

            [Fact]
            public void UpdateTest_準正常系_セッションエラー_パラメータ改竄()
            {
                var targetId = _testUser.Id;
                var serviceMock = new Mock<IUserService>();

                var controller = new UserController(serviceMock.Object);
                controller.ControllerContext.HttpContext = test.mockHttpContext.Object;
                controller.ModelState.Clear();

                // Sessionで指定したIDと異なるIDをパラメータで指定
                var actionResult = controller.Update(new UserParamModel() { Id = 2 });

                // assert 404
                Assert.IsType<NotFoundResult>(actionResult);
                Assert.Null(test.mockSession.GetInt32("USER_SESSION_ID"));
            }
        }
    }
}
