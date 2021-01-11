using Google.Apis.Oauth2.v2;
using Google.Apis.Oauth2.v2.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System;
using System.Text;
using System.Security.Claims;
using ExpenseApproval.Model;
using ExpenseApproval.Entity;
using System.Collections.Generic;
using ExpenseApproval.Utility;
using ExpenseApproval.DAL;
using System.Linq;

namespace ExpenseApproval.BLL
{
    public class LoginUser : ILoginUser
    {
        #region Private Members

        private IUnitOfWork _unitOfWork;

        #endregion

        #region Constructor

        public LoginUser(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region Private Method

        private string ExtractEmail(string googleToken)
        {
            Oauth2Service service = new Oauth2Service(
            new Google.Apis.Services.BaseClientService.Initializer());
            Oauth2Service.TokeninfoRequest request = service.Tokeninfo();
            request.AccessToken = googleToken;
            Tokeninfo info = request.Execute();
            string userEmail = info.Email;
            return userEmail;
        }

        private string GenerateRefreshToken(Employee employee)
        {
            string refreshtoken = Guid.NewGuid().ToString();
            employee.RefreshToken = refreshtoken;
            _unitOfWork.Repository<Employee>().Update(employee);
            _unitOfWork.Commit();
            return refreshtoken;
        }

        private string GenerateJsonToken(string role, string employeeId)
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.Role, role), new Claim(ClaimTypes.Sid, employeeId) };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Constant.EncryptionKey));
            var signInCred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                    expires: DateTime.Now.AddMinutes(1),
                    claims: claims,
                    signingCredentials: signInCred
                    );
            var jsonToken = new JwtSecurityTokenHandler().WriteToken(token);
            return jsonToken;
        }

        #endregion

        public LoginModel LoginDetails(string googleToken)
        {
            string email = ExtractEmail(googleToken);
            Employee employee = _unitOfWork.Repository<Employee>().FindBy(x => x.EmployeeEmail == email).FirstOrDefault();
            if (employee == null)
            {
                throw new ExpenseException(Constant.ErrorMessageInvalidGoogleToken);
            }
            string newRefreshToken = GenerateRefreshToken(employee);
            LoginModel loginModel = new LoginModel
            {
                EmployeeName = employee.EmployeeName,
                RefreshToken = newRefreshToken,
                Token = GenerateJsonToken(employee.Role.RoleName, employee.EmployeeId.ToString())
            };
            return loginModel;
        }

        public LoginModel RefreshLoginDetails(string refreshToken)
        {
            Employee employee = _unitOfWork.Repository<Employee>().FindBy(x => x.RefreshToken == refreshToken).FirstOrDefault();
            if (employee == null || employee.RefreshToken != refreshToken)
            {
                throw new ExpenseException(Constant.ErrorMessageInvalidRefreshToken);
            }

            LoginModel loginModel = new LoginModel
            {
                EmployeeName = employee.EmployeeName,
                RefreshToken = refreshToken,
                Token = GenerateJsonToken(employee.Role.RoleName, employee.EmployeeId.ToString())
            };
            return loginModel;
        }
    }
}