using System.Threading.Tasks;
using ERPack.Models.TokenAuth;
using ERPack.Web.Controllers;
using Shouldly;
using Xunit;

namespace ERPack.Web.Tests.Controllers
{
    public class HomeController_Tests: ERPackWebTestBase
    {
        [Fact]
        public async Task Index_Test()
        {
            await AuthenticateAsync(null, new AuthenticateModel
            {
                UserNameOrEmailAddress = "admin",
                Password = "123qwe"
            });

            //Act
            var response = await GetResponseAsStringAsync(
                GetUrl<HomeController>(nameof(HomeController.Index))
            );

            //Assert
            response.ShouldNotBeNullOrEmpty();
        }
    }
}