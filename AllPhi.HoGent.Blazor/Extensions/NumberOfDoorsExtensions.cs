using AllPhi.HoGent.Blazor.Dto.Enums;

namespace AllPhi.HoGent.Blazor.Extansions
{
    public static class NumberOfDoorsExtensions
    {
        private static readonly Dictionary<NumberOfDoors, string> _doorNames = new Dictionary<NumberOfDoors, string>
        {
            { NumberOfDoors.OneDoor, "1 deur" },
            { NumberOfDoors.TwooDoors, "2 deuren" },
            { NumberOfDoors.ThreeDoors, "3 deuren" },
            { NumberOfDoors.FourDoors, "4 deuren" },
            { NumberOfDoors.FiveDoors, "5 deuren" }
        };

        public static string GetDescription(this NumberOfDoors numberOfDoors)
        {
            if (_doorNames.TryGetValue(numberOfDoors, out var description))
            {
                return description;
            }

            return "Onbekend aantal deuren";
        }

        public static Dictionary<NumberOfDoors, string> GetAllDoorDescriptions()
        {
            return _doorNames;
        }
    }
}
