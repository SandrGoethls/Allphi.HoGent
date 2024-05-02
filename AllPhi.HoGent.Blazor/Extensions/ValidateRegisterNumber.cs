namespace AllPhi.HoGent.Blazor.Extensions
{
    public class ValidateRegisterNumber
    {
        public static bool IsValidDriverRegisterNumber(string registerNumber)
        {

            if (string.IsNullOrEmpty(registerNumber))
            {
                return false;
            }

            if (registerNumber.Length != 11 || !IsDatePartValid(registerNumber.Substring(0, 6)))
            {
                return false;
            }

            int controlNumber = int.Parse(registerNumber.Substring(9, 2));
            int numberToCheck = int.Parse(registerNumber.Substring(0, 9));

            if (registerNumber.StartsWith("00"))
            {
                numberToCheck = int.Parse("2" + registerNumber.Substring(0, 9));
            }

            return (97 - (numberToCheck % 97)) == controlNumber;
        }

        private static bool IsDatePartValid(string datePart)
        {
            // Check if the string has the correct format for YYMMDD
            if (datePart.Length != 6)
            {
                return false;
            }

            int year = int.Parse(datePart.Substring(0, 2));
            int month = int.Parse(datePart.Substring(2, 2));
            int day = int.Parse(datePart.Substring(4, 2));

            year += (year < 20) ? 2000 : 1900;

            if (month < 1 || month > 12 || day < 1 || day > 31)
            {
                return false;
            }

            try
            {
                DateTime birthDate = new DateTime(year, month, day);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
