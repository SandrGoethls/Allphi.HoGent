using System.Text;

namespace DatabaseDataGenerator
{
    internal class Program
    {
        static Random random = new Random();
        static string[] firstNames = { "Jef", "Marie", "Els", "Luc", "Sofie", "Koen" };
        static string[] lastNames = { "Peeters", "Janssens", "Maes", "Jacobs", "Mertens", "Willems" };
        static string[] cities = { "Antwerpen", "Gent", "Brugge", "Leuven", "Mechelen", "Hasselt" };
        static string[] streets = { "Meir", "Korenmarkt", "Steenstraat", "Diesterstraat", "Bruul", "Bondgenotenlaan" };
        static string[] fuelTypes = { "Benzine", "Diesel", "Elektrisch", "Hybride" };

        static void Main(string[] args)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 100; i++)
            {
                sb.AppendLine(GenerateDriver());
                sb.AppendLine("GO");
            }

            for (int i = 0; i < 100; i++)
            {
                sb.AppendLine(GenerateVehicle());
                sb.AppendLine("GO");
            }

            for (int i = 0; i < 100; i++)
            {
                sb.AppendLine(GenerateFuelCard(out string fuelCardId));
                sb.AppendLine("GO");
                sb.AppendLine(GenerateFuelCardFuelTypes(fuelCardId));
                sb.AppendLine("GO");
            }

            Console.WriteLine(sb.ToString());
        }

        static string GenerateDriver()
        {
            var id = Guid.NewGuid();
            var firstName = firstNames[random.Next(firstNames.Length)];
            var lastName = lastNames[random.Next(lastNames.Length)];
            var city = cities[random.Next(cities.Length)];
            var street = streets[random.Next(streets.Length)];
            var houseNumber = random.Next(1, 100);
            var postalCode = random.Next(1000, 10000).ToString();
            var registerNumber = random.Next(100000000, 999999999).ToString();
            var dateOfBirth = new DateTime(random.Next(1950, 2000), random.Next(1, 13), random.Next(1, 29));
            var typeOfDriverLicense = random.Next(1, 4); // A1, B1, C1, etc.
            var status = random.Next(0, 2); // Active or Inactive

            return $"INSERT INTO [dbo].[Drivers] ([Id], [FirstName], [LastName], [City], [Street], [HouseNumber], [PostalCode], [RegisterNumber], [DateOfBirth], [TypeOfDriverLicense], [Status]) VALUES ('{id}', N'{firstName}', N'{lastName}', N'{city}', N'{street}', {houseNumber}, '{postalCode}', '{registerNumber}', '{dateOfBirth:yyyy-MM-dd}', N'{typeOfDriverLicense}', {status})";
        }

        static string GenerateVehicle()
        {
            var id = Guid.NewGuid();
            var chassisNumber = GenerateRandomString(12);
            var licensePlate = $"1-ABC-{random.Next(100, 1000)}";
            var carBrands = new string[] { "Volkswagen", "Peugeot", "Renault", "Audi", "BMW", "Mercedes" };
            var carBrand = carBrands[random.Next(carBrands.Length)];
            var numberOfDoors = random.Next(2, 5);
            var fuelType = fuelTypes[random.Next(fuelTypes.Length)];
            var typesOfCar = new string[] { "Sedan", "SUV", "Hatchback", "Coupe" };
            var typeOfCar = typesOfCar[random.Next(typesOfCar.Length)];
            var colors = new string[] { "Zwart", "Wit", "Blauw", "Rood", "Groen", "Geel" };
            var color = colors[random.Next(colors.Length)];
            var status = random.Next(0, 2);
            var inspectionDate = DateTime.Now;

            return $"INSERT INTO [dbo].[Vehicles] ([Id], [ChassisNumber], [LicensePlate], [CarBrand], [NumberOfDoors], [FuelType], [TypeOfCar], [VehicleColor], [Status], [InspectionDate]) VALUES ('{id}', '{chassisNumber}', '{licensePlate}', '{carBrand}', {numberOfDoors}, '{fuelType}', '{typeOfCar}', '{color}', {status}, '{inspectionDate:yyyy-MM-dd}')";
        }

        static string GenerateFuelCard(out string fuelCardId)
        {
            fuelCardId = Guid.NewGuid().ToString();
            var pin = random.Next(1000, 10000);
            var cardNumber = GenerateRandomString(16);
            var validityDate = DateTime.Now.AddYears(random.Next(1, 5));
            var status = random.Next(0, 2);

            return $"INSERT INTO [dbo].[FuelCards] ([Id], [Pin], [CardNumber], [ValidityDate], [Status]) VALUES ('{fuelCardId}', {pin}, '{cardNumber}', '{validityDate:yyyy-MM-dd}', {status})";
        }

        static string GenerateFuelCardFuelTypes(string fuelCardId)
        {
            var fuelType = fuelTypes[random.Next(fuelTypes.Length)];
            var id = Guid.NewGuid().ToString();
            return $"INSERT INTO [dbo].[FuelCardFuelTypes] ([Id], [FuelCardId], [FuelType]) VALUES ('{id}', '{fuelCardId}', N'{fuelType}')";
        }

        static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var stringChars = new char[length];
            for (int i = 0; i < length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            return new string(stringChars);
        }
    }
}
