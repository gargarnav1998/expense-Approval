using ExpenseApproval.Model;

namespace ExpenseApproval.BLL
{
    public interface ILoginUser
    {
        LoginModel LoginDetails(string googleToken);
        LoginModel RefreshLoginDetails(string refreshToken);
    }
}