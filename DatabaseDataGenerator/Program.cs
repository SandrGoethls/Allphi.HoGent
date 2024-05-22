using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DatabaseDataGenerator
{
    internal class Program
    {
        private static Random random = new Random();

        private static string[] firstNames = {
    "Jef", "Marie", "Els", "Luc", "Sofie", "Koen", "Emma", "Arthur", "Noah", "Louis",
    "Laura", "Liam", "Julie", "Anna", "Milan", "Thomas", "Nora", "Lena", "Lucas", "Ella"
};

        private static string[] lastNames = {
    "Peeters", "Janssens", "Maes", "Jacobs", "Mertens", "Willems", "Claes", "Goossens", "Wouters", "De Smet",
    "Dubois", "Lambert", "Dupont", "Fontaine", "Lemmens", "Martens", "Desmet", "Claeys", "De Jong", "Simon"
};

        private static string[] cities = { "Antwerpen", "Gent", "Brugge", "Leuven", "Mechelen", "Hasselt" };
        private static string[] streets = { "Meir", "Korenmarkt", "Steenstraat", "Diesterstraat", "Bruul", "Bondgenotenlaan" };
        private static string[] fuelTypes = { "Benzine", "Diesel", "Elektrisch", "Hybride" };

        



        private static void Main(string[] args)
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

        private static string GenerateDriver()
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
            var typeOfDriverLicense = (TypeOfDriverLicense)random.Next(1, Enum.GetNames(typeof(TypeOfDriverLicense)).Length + 1); // Get random enum value
            var status = (Status)random.Next(0, 2); // Get random enum value

            return $"INSERT INTO [dbo].[Drivers] ([Id], [FirstName], [LastName], [City], [Street], [HouseNumber], [PostalCode], [RegisterNumber], [DateOfBirth], [TypeOfDriverLicense], [Status]) VALUES ('{id}', N'{firstName}', N'{lastName}', N'{city}', N'{street}', {houseNumber}, '{postalCode}', '{registerNumber}', '{dateOfBirth:yyyy-MM-dd}', {(int)typeOfDriverLicense}, {(int)status})";
        }

        private static string GenerateVehicle()
        {
            var id = Guid.NewGuid();
            var chassisNumber = GenerateRandomString(12);
            var licensePlate = $"1-ABC-{random.Next(100, 1000)}";
            var carBrand = (CarBrand)random.Next(1, Enum.GetNames(typeof(CarBrand)).Length + 1); // Get random enum value
            var numberOfDoors = (NumberOfDoors)random.Next(1, Enum.GetNames(typeof(NumberOfDoors)).Length + 1); // Get random enum value
            var fuelType = (FuelType)random.Next(1, Enum.GetNames(typeof(FuelType)).Length + 1); // Get random enum value
            var typeOfCar = (TypeOfCar)random.Next(1, Enum.GetNames(typeof(TypeOfCar)).Length + 1); // Get random enum value
            var color = (VehicleColor)random.Next(0, Enum.GetNames(typeof(VehicleColor)).Length); // Get random enum value
            var status = (Status)random.Next(0, 2); // Get random enum value
            var inspectionDate = DateTime.Now.AddDays(random.Next(1, 366));

            return $"INSERT INTO [dbo].[Vehicles] ([Id], [ChassisNumber], [LicensePlate], [CarBrand], [NumberOfDoors], [FuelType], [TypeOfCar], [VehicleColor], [Status], [InspectionDate]) VALUES ('{id}', '{chassisNumber}', '{licensePlate}', {(int)carBrand}, {(int)numberOfDoors}, {(int)fuelType}, {(int)typeOfCar}, {(int)color}, {(int)status}, '{inspectionDate:yyyy-MM-dd}')";
        }

        private static string GenerateFuelCard(out string fuelCardId)
        {
            fuelCardId = Guid.NewGuid().ToString();
            var pin = random.Next(1000, 10000);
            var cardNumber = GenerateRandomString(16);
            var validityDate = DateTime.Now.AddDays(random.Next(1, 366));
            var status = (Status)random.Next(0, 2); // Get random enum value

            return $"INSERT INTO [dbo].[FuelCards] ([Id], [Pin], [CardNumber], [ValidityDate], [Status]) VALUES ('{fuelCardId}', {pin}, '{cardNumber}', '{validityDate:yyyy-MM-dd}', {(int)status})";
        }

        private static string GenerateFuelCardFuelTypes(string fuelCardId)
        {
            var fuelType = (FuelType)random.Next(1, Enum.GetNames(typeof(FuelType)).Length + 1); // Get random enum value
            var id = Guid.NewGuid().ToString();
            return $"INSERT INTO [dbo].[FuelCardFuelTypes] ([Id], [FuelCardId], [FuelType]) VALUES ('{id}', '{fuelCardId}', {(int)fuelType})";
        }

        private static string GenerateRandomString(int length)
        {
            const string chars = "0123456789";
            var stringChars = new char[length];
            for (int i = 0; i < length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            return new string(stringChars);
        }

        public enum CarBrand
        {
            Bmw = 1,
            Audi = 2,
            Ford = 3,
            Citroën = 4,
            Mini = 5,
            Mercedes = 6,
            Volkswagen = 7,
            Volo = 8,
            Kia = 9,
            Škoda = 10,
            Suzuki = 11,
            RangeRover = 12,
            Toyota = 13,
            Porsche = 14,
            Nissan = 15,
            Mazda = 16,
            Mitsubishi = 17,
            Peugeot = 18,
            Dacia = 19,
            Dodge = 20,
            Lamborghini = 21,
        }
        public enum TypeOfCar
        {
            PassangerCar = 1,
            Van = 2,
            SUV = 3,
            Hatchback = 4,
            Sedan = 5,
            Coupé = 6,
            Cabriolet = 7
        }
        public enum FuelType
        {
            Benzine = 1,

            Diesel = 2,

            Hybride = 3,

            Elektrisch = 4,
        }
        public enum NumberOfDoors
        {
            [Display(Name = "1 deur")]
            OneDoor = 1,

            [Display(Name = "2 deuren")]
            TwooDoors = 2,

            [Display(Name = "3 deuren")]
            ThreeDoors = 3,

            [Display(Name = "4 deuren")]
            FourDoors = 4,

            [Display(Name = "5 deuren")]
            FiveDoors = 5,
        }
        public enum Status
        {
            Deactive = 0,
            Active = 1,
        }
        public enum TypeOfDriverLicense
        {
            AM = 1,
            A, A1, A2 = 2,
            B = 3,
            BE = 4,
            C = 5,
            CE = 6,
            C1 = 7,
            C1E = 8,
            D = 9,
            DE = 10,
            D1 = 11,
            D1E = 12,
            T = 13,
        }
        public enum VehicleColor
        {
            Red = 0,

            Blue = 1,

            Black = 2,

            White = 3,

            Yellow = 4,
        }
    }
}