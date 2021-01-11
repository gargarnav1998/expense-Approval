using ExpenseApproval.BLL;
using System.Net;
using System.Web.Http;
using System.Web.Http.Tracing;
using ExpenseApproval.Utility;
using System;
using ExpenseApproval.Api.AuthenticationFilter;

namespace ExpenseApproval.Api.Controllers
{
    [RoutePrefix(Constant.LoginRoutePrefix)]
    public class LoginController : ApiController
    {
        #region Private Members

        private ILoginUser _loginUser;
        private ITraceWriter _tracer;

        #endregion

        #region Constructor

        public LoginController(ILoginUser loginUser)
        {
            _loginUser = loginUser;
            _tracer = GlobalConfiguration.Configuration.Services.GetTraceWriter();
        }

        #endregion

        #region API

        [Route]
        [HttpGet]
        public IHttpActionResult Login(string googleToken)
        {
            return Content(HttpStatusCode.OK, _loginUser.LoginDetails(googleToken));
        }

        [Route(Constant.RefreshToken)]
        [HttpPost]
        public IHttpActionResult RefreshToken(string refreshToken)
        {
            return Content(HttpStatusCode.OK, _loginUser.RefreshLoginDetails(refreshToken));
        }

        #endregion
    }
}