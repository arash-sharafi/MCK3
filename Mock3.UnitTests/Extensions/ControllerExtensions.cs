using Moq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace Mock3.UnitTests.Extensions
{
    public static class ControllerExtensions
    {
        public static void MockCurrentUser(this Controller controller, string userId, string userName)
        {
            var fakeHttpContext = new Mock<HttpContextBase>();
            var identity = new GenericIdentity("user1@domain.com");
            identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", userName));
            identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", userId));
            var principal = new GenericPrincipal(identity, null);

            fakeHttpContext.Setup(x => x.User).Returns(principal);
            var controllerContext = new Mock<ControllerContext>();
            controllerContext.Setup(x => x.HttpContext).Returns(fakeHttpContext.Object);

            controller.ControllerContext = controllerContext.Object;
        }
    }
}
