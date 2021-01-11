using System.Net;
using System.Web.Http;
using ExpenseApproval.BLL;
using ExpenseApproval.Model;
using System.Web.Http.Tracing;
using ExpenseApproval.Utility;
using ExpenseApproval.Api.AuthenticationFilter;
using System.Web;
using System.Configuration;
using System.IO;

namespace ExpenseApproval.Controllers
{
    [AuthenticateUser]
    [RoutePrefix(Constant.ExpenseRoutePrefix)]
    public class ExpenseController : ApiController
    {
        #region Private members 

        private IExpenseDetails _expenseDetails;
        private ITraceWriter _tracer;

        #endregion

        #region Constructor

        public ExpenseController(IExpenseDetails expenseDetails)
        {
            _expenseDetails = expenseDetails;
            _tracer = GlobalConfiguration.Configuration.Services.GetTraceWriter();
        }

        #endregion

        #region API methods

        [HttpGet]
        [Route]
        public IHttpActionResult GetExpenses(bool forApproval)
        {
            Request.Properties.TryGetValue(Constant.EmployeeId, out object employeeId);
            Request.Properties.TryGetValue(Constant.Role, out object role);
            return Content(HttpStatusCode.OK, _expenseDetails.GetAllByEmployeeId((int)employeeId, (string)role, forApproval));
        }

        [HttpGet]
        [Route(Constant.GetEditExpenseRoute)]
        public IHttpActionResult GetById(int expenseId)
        {
            Request.Properties.TryGetValue(Constant.EmployeeId, out object employeeId);
            return Content(HttpStatusCode.OK, _expenseDetails.GetById(expenseId, (int)employeeId));
        }

        [HttpGet]
        [Route(Constant.BillImageRoute)]
        public IHttpActionResult DownloadBillForExpense(int expenseId)
        {
            HttpResponse response = _expenseDetails.DownloadBillForExpense(HttpContext.Current.Response, expenseId);
            return Ok();
        }

        [HttpPost]
        [Route]
        public IHttpActionResult Create()
        {
            HttpRequest httpRequest = HttpContext.Current.Request;
            ExpenseDetailModel expense = Mapping.HttpRequestMapping(httpRequest);
            //We have to pass a model in Validate if we have to use it for ModelState.IsValid
            Validate(expense);
            if (ModelState.IsValid)
            {
                Request.Properties.TryGetValue(Constant.EmployeeId, out object creatorId);
                return Ok(_expenseDetails.Create(expense, httpRequest,(int)creatorId));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut]
        [Route]
        public IHttpActionResult Edit(ExpenseDetailModel expenseDetailModel)
        {
            if (ModelState.IsValid)
            {
                Request.Properties.TryGetValue(Constant.EmployeeId, out object creatorId);
                return Ok(_expenseDetails.Edit(expenseDetailModel, (int)creatorId));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [Authorize(Roles = Constant.AuthorizeApprover)]
        [HttpPut]
        [Route(Constant.ApproveExpenseRoute)]
        public IHttpActionResult Approve(ApproveExpenseModel approveExpense, int participantId)
        {
            if (ModelState.IsValid)
            {
                Request.Properties.TryGetValue(Constant.EmployeeId, out object approverId);
                return Ok(_expenseDetails.Approve(approveExpense, participantId, (int)approverId));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete]
        [Route(Constant.DeleteExpenseRoute)]
        public IHttpActionResult Delete(int expenseId)
        {
            Request.Properties.TryGetValue(Constant.EmployeeId, out object creatorId);
            _expenseDetails.Delete(expenseId, (int)creatorId);
            return Ok();
        }

        [HttpGet]
        [Route(Constant.CategoriesRoute)]
        public IHttpActionResult GetCategories()
        {
            return Content(HttpStatusCode.OK, _expenseDetails.ExpenseTypes());
        }

        #endregion
    }
}