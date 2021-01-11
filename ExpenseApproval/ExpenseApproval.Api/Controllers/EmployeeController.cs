using ExpenseApproval.BLL;
using System.Web.Http.Tracing;
using System.Net;
using System.Web.Http;
using ExpenseApproval.Api.AuthenticationFilter;
using ExpenseApproval.Utility;

namespace ExpenseApproval.Controllers
{
    [AuthenticateUser]
    [RoutePrefix(Constant.EmployeeRoutePrefix)]
    public class EmployeeController : ApiController
    {
        #region Private members 

        private IEmployeeDetails _employeeDetails;
        private readonly ITraceWriter _tracer;

        #endregion

        #region Constructor

        public EmployeeController(IEmployeeDetails employeeDetails)
        {
            _employeeDetails = employeeDetails;
            _tracer = GlobalConfiguration.Configuration.Services.GetTraceWriter();
        }

        #endregion

        #region API methods

        [Route]
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            return Content(HttpStatusCode.OK, _employeeDetails.GetAll());
        }

        [HttpGet]
        [Route(Constant.BudgetDetailsRoute)]
        public IHttpActionResult GetBudgetDetails()
        {
            Request.Properties.TryGetValue(Constant.EmployeeId, out object employeeId);
            return Content(HttpStatusCode.OK, _employeeDetails.GetBudgetDetails((int)employeeId));
        }

        #endregion
    }
}