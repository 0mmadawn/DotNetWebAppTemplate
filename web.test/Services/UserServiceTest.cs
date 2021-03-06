using Moq;
using web.Common.Enums;
using web.Models.Entities;
using web.Repositories;
using web.Services;
using web.ViewModels.Users;
using Xunit;

namespace web.test.Services
{
    public class UserServiceTest
    {
        private Mock<IUserRepository> repositoryMock;
        private IUserRepository repo;

        public UserServiceTest()
        {
            repositoryMock = new Mock<IUserRepository>();
            repo = repositoryMock.Object;
        }

        public class GetShowViewModelTest
        {
            private readonly User _testUser = new User(1, "Test", Group.Aeug, "TestDesc");
            private UserServiceTest test;

            public GetShowViewModelTest()
            {
                test = new UserServiceTest();
            }

            [Fact]
            public void GetShowViewModelTest_正常系()
            {
                var targetId = _testUser.Id;
                test.repositoryMock
                    .Setup(repo => repo.LoadUser(targetId))
                    .Callback(() =>
                        test.repositoryMock
                            .SetupGet(x => x.User).Returns(_testUser)
                    );

                var service = new UserService(test.repo);
                var model = service.GetShowViewModel(targetId);

                Assert.IsType<ShowViewModel>(model);
                Assert.NotNull(model.User);
            }

            [Fact]
            public void GetShowViewModelTest_準正常系_指定したIDのユーザなし()
            {
                var targetId = _testUser.Id;
                test.repositoryMock
                    .Setup(repo => repo.LoadUser(targetId))
                    .Callback(() =>
                        test.repositoryMock
                            .SetupGet(x => x.User).Returns((User)null)
                    );

                var service = new UserService(test.repo);
                var model = service.GetShowViewModel(targetId);

                Assert.Null(model);
            }
        }

        // 似たような形で他もテスト...
    }
}
