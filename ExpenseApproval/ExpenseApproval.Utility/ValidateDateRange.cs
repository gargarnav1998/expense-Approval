using System;
using System.ComponentModel.DataAnnotations;

namespace ExpenseApproval.Utility
{
    public class ValidateDateRange : ValidationAttribute
    {
        public static DateTime currentDate = DateTime.Now;
        public static string secondDate = currentDate.ToString(Constant.DateFormat);
        public string FirstDate = currentDate.AddYears(Constant.PreviousYear).ToString(Constant.DateFormat);

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            try
            {
                if (Convert.ToDateTime(value) >= Convert.ToDateTime(FirstDate) && Convert.ToDateTime(value) <= Convert.ToDateTime(secondDate))
                    return ValidationResult.Success;
                else
                {
                    if (Convert.ToDateTime(value) > DateTime.Now)
                        return new ValidationResult(Constant.ErrorMessageFutureDateExpense);
                    else
                        return new ValidationResult(Constant.ErrorMessagePastYearExpense);
                }
            }
            catch (FormatException)
            {
                return new ValidationResult(Constant.ErrorMessageInvalidDateFormat);
            }
        }
    }
}