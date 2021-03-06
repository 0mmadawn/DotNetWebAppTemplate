using web.Common.Enums;
using web.Models.Parameters;
using web.test.TestUtils;
using Xunit;

namespace web.test.Models.Parameters
{
    public class UserParamModelTest
    {
        UserParamModel validParam;

        public UserParamModelTest()
        {
            validParam = new UserParamModel()
            {
                Id = 1,
                Name = "Test",
                Group = Group.Aeug,
                Description = "Test Test"
            };
        }

        public class IdTest
        {
            private UserParamModelTest test;
            public IdTest()
            {
                test = new UserParamModelTest();
            }

            [Theory]
            [InlineData(1)]
            [InlineData(2)]
            [InlineData(10)]
            public void 正常系(int id)
            {
                test.validParam.Id = id;
                var validationRes = ValidationUtils.ValidateModel(test.validParam);
                Assert.False(validationRes.ContainsKey(nameof(UserParamModel.Id)));
            }

            [Theory]
            [InlineData(0)]
            [InlineData(11)]
            [InlineData(-1)]
            public void 準正常系(int id)
            {
                test.validParam.Id = id;
                var validationRes = ValidationUtils.ValidateModel(test.validParam);
                Assert.True(validationRes.ContainsKey(nameof(UserParamModel.Id)));
            }

            [Fact]
            public void 準正常系_null()
            {
                test.validParam = new UserParamModel(); // Idがnull
                var validationRes = ValidationUtils.ValidateModel(test.validParam);
                Assert.True(validationRes.ContainsKey(nameof(UserParamModel.Id)));
            }
        }


    }
}
