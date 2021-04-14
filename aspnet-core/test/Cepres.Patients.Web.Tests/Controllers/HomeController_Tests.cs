using System.Threading.Tasks;
using Cepres.Patients.Models.TokenAuth;
using Cepres.Patients.Web.Controllers;
using Shouldly;
using Xunit;

namespace Cepres.Patients.Web.Tests.Controllers
{
    public class HomeController_Tests: PatientsWebTestBase
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