namespace RegisterNumberGenerator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Write("Enter the date part of the register number (YYMMDD) or type 'exit' to quit: ");
                string dobPart = Console.ReadLine();
                if (dobPart.ToLower() == "exit")
                {
                    break;
                }

                bool found = false;
                for (int i = 0; i < 1000 && !found; i++)
                {
                    string first9 = dobPart + i.ToString("D3");
                    int numberToCheck = int.Parse(first9);

                    if (first9.StartsWith("00"))
                    {
                        numberToCheck = int.Parse("2" + first9);
                    }

                    int controlNumber = 97 - (numberToCheck % 97);

                    if (controlNumber < 10)
                    {
                        string fullRegisterNumber = first9 + controlNumber.ToString("D2");
                        if (IsValidDriverRegisterNumber(fullRegisterNumber))
                        {
                            Console.WriteLine($"Valid register number found: {fullRegisterNumber}");
                            found = true;
                        }
                    }
                }

                if (!found)
                {
                    Console.WriteLine("No valid register number found.");
                }
            }
        }

        public static bool IsValidDriverRegisterNumber(string registerNumber)
        {
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

