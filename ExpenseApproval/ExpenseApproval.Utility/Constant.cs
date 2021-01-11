using System.Collections.Generic;

namespace ExpenseApproval.Utility
{
    public class Constant
    {
        #region Hardcoded strings 

        public static List<string> Approvers = new List<string> { "Approver", "Admin" };
        public static List<string> ValidFormats = new List<string> { "jpg", "png", "pdf", "jpeg" };
        public const string EmployeeRoutePrefix = "api/employees";
        public const string LoginRoutePrefix = "api/login";
        public const string ExpenseRoutePrefix = "api/expenses";
        public const string RefreshToken = "refreshToken";
        public const string BillImageRoute = "billImage";
        public const string BudgetDetailsRoute = "budgetDetails";
        public const string ApproverRoute = "approver";
        public const string AuthorizeApprover = "Approver, Admin";
        public const string EmployeeId = "id";
        public const string CategoriesRoute = "categories";
        public const string EditExpenseRoute = "edit";
        public const string GetEditExpenseRoute = "{expenseId}";
        public const string DateFormat = "yyyy-MM-dd";
        public const string ApproveExpenseRoute = "approve/{participantId}";
        public const string DeleteExpenseRoute = "{expenseId}";
        public const string EncryptionKey = "norse_gods_of_asgard";
        public const string Role = "role";
        public const string NotIncludedInExpense = "Not Included";
        public const string BillImagePath = "downloadFilePath";
        public const string Approver = "ApproverId";
        public const string ExpenseTypeId = "ExpenseTypeId";
        public const string ExpensePurpose = "ExpensePurpose";
        public const string AmountClaimed = "AmountClaimed";
        public const string ExpenseDate = "ExpenseDate";
        public const string KeyEnd = "[amountClaimed]";
        public const string ParticipantDetails = "ParticipantDetails[";
        public const string ParticipantAmountClaimed = "][AmountClaimed]";
        public const string ParticipantEmployeeId = "][EmployeeId]";
        public const string ContentType = "image/jpeg";
        public const string Attachment = "attachment; filename=bill.jpg";
        public const string ContentDisposition = "Content-Disposition";

        #endregion

        #region Magic Numbers

        public const int ApproverId = 2;
        public const int AdminId = 3;
        public const int PreviousYear = -1;
        public const int MinimumValue = 1;
        public const int MaximumValue = 3;
        public const int InitialAmount = 0;
        public const int InitialId = 0;
        public const int CountofEmployee = 1;
        public const int MinimumIdValue = 0;
        public const int PendingStateId = 1;
        public const int ApproveStateId = 2;
        public const int RejectStateId = 3;

        #endregion

        #region ErrorMessage

        public static string ErrorMessageUnauthorized = "Employee with Id {0} can not access this method";
        public static string ErrorMessageNotValid = "Can not perform update since expense is not in pending state";
        public static string ErrorMessageDuplicateRecord = "Duplicate participants detected while Creating or Editing expense";
        public static string ErrorMessageInvalidCreate = "Creator is not included in the expense";
        public static string ErrorMessageMissingToken = "Missing Token";
        public static string ErrorMessageInvalidToken = "Invalid Token";
        public static string ErrorMessageInvalidGoogleToken = "Invalid Google Token";
        public static string ErrorMessageInvalidAccessToken = "Invalid Access Token";
        public static string ErrorMessageInvalidRefreshToken = "Invalid Refresh Token";
        public static string ErrorMessageNoRecordFound = "No Record Found for Employee Id {0}";
        public static string ErrorMessageNoExpenseFound = "No Record Found for Expense Id {0}";
        public static string ErrorMessageParticipantNotFound = "Participant with Id {0} not found";
        public static string ErrorMessageInvalidAmount = "Sum of individual amounts is not equal to total amount claimed";
        public static string ErrorMessageAmountException = "Approved amount is greater than amount claimed";
        public static string ErrorMessageServerDown = "Server Unavailable";
        public static string ErrorMessageFutureDateExpense = "You Cannot create expense of future date";
        public static string ErrorMessagePastYearExpense = "Please don't enter the expenses more than one year old";
        public static string ErrorMessageInvalidDateFormat = " Correct Date Format(yyyy-mm-dd)";
        public static string ErrorMessageUnauthorizedAction = "Only creator can Delete or Edit an Expense";
        public static string ErrorMessageInvalidOptimusGmailId = "Login with valid Optimus Gmail Id";
        public static string ErrorMessageInvalidExpenseType = "Invalid Expense Category";
        public static string ErrorMessageInvalidParticipantId = "Participant Id {0} is not valid. Please enter a valid Participant Id.";
        public static string ErrorMessageInvalidParticipantCreated = "You cannot add participant in individual type expense";
        public static string ErrorMessageParticipantNotAdded = "Please add participants in Expense";
        public static string ErrorMessageInvalidApprover = "Please choose a diffrent approver. you can't make yourself an approver";
        public static string ErrorMessageInvalidId = "Please enter the id greater than zero";
        public static string ErrorMessageExpenseDate = "Expense Date can not be blank or of future or 1 year old";
        public static string ErrorMessageNoImage = "File not found. Please check the file name.";
        public static string ErrorMessageFileNotFound = "Can not find file: ";
        public static string ErrorMessageInvalidApproverId = "Approver Id {0} is not valid. Please enter valid Approver Id";
        public static string ErrorMessageAmountNotInteger = "Amount must be a numeric value";
        public static string ErrorMessageUnauthorizedApprover = "Not an authorized approver";
        public static string ErrorMessageEmptyFileNameField = "Please enter valid file name";
        public static string ErrorMessageValidApproverId = "Approver Id {0} is not valid. Please enter a valid Approver Id.";
        public static string ErrorMessageInvalidExpenseStatusId = "Please choose a valid ExpenseStatusId";
        public static string ErrorMessageInvalidBillImageFormat = "Invalid image format. Supported formats are jpg, png, jpeg and pdf";

        #endregion

        #region Exceptions

        public const string GoogleApiException = "Google.GoogleApiException";
        public const string FileNotFoundException = "System.IO.FileNotFoundException";
        public const string EntityException = "System.Data.Entity.Core.EntityException";
        public const string FormatException = "System.FormatException";
        public const string ExpenseException = "ExpenseApproval.Model.ExpenseException";

        #endregion

    }
}
