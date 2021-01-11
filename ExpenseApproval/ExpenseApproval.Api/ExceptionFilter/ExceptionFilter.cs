using ExpenseApproval.Model;
using ExpenseApproval.Utility;
using Google;
using System;
using System.Data.Entity.Core;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Http.Tracing;

namespace ExpenseApproval
{
    public class ExpenseExceptionFilterAttribute : ExceptionFilterAttribute
    {
        #region

        private ITraceWriter _tracer;

        #endregion

        #region Constructor

        public ExpenseExceptionFilterAttribute()
        {
            _tracer = GlobalConfiguration.Configuration.Services.GetTraceWriter();
        }

        #endregion

        public override void OnException(HttpActionExecutedContext context)
        {
            
            HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent(context.Exception.Message),
            };

            if (context.Exception is GoogleApiException)
            {
                resp.Content = new StringContent(Constant.ErrorMessageInvalidGoogleToken);
                resp.StatusCode = HttpStatusCode.Unauthorized;
            }

            else if (context.Exception is FileNotFoundException)
            {
                resp.Content = new StringContent(Constant.ErrorMessageNoImage);
                resp.StatusCode = HttpStatusCode.NotFound;
            }

            else if (context.Exception is EntityException)
            {
                resp.Content = new StringContent(Constant.ErrorMessageServerDown);
                resp.StatusCode = HttpStatusCode.InternalServerError;
            }

            else if (context.Exception is FormatException)
            {
                resp.Content = new StringContent(context.Exception.Message + Constant.ErrorMessageInvalidDateFormat);
                resp.StatusCode = HttpStatusCode.BadRequest;
            }

            else if (context.Exception is ExpenseException)
            {
                resp.Content = new StringContent(context.Exception.Message);
                resp.StatusCode = HttpStatusCode.BadRequest;
            }
            _tracer.Error(context.Request, context.ActionContext.ControllerContext.ControllerDescriptor.ControllerType.FullName, resp.Content.ReadAsStringAsync().Result);
            throw new HttpResponseException(resp);
        }
    }
}