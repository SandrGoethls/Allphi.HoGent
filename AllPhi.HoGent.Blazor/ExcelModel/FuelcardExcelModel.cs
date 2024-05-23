
using AllPhi.HoGent.Blazor.Dto.Enums;

namespace AllPhi.HoGent.Blazor.ExcelModel
{
    public class FuelcardExcelModel
    {
        public Guid Id { get; set; }

        public int Pin { get; set; }

        public string CardNumber { get; set; } = string.Empty;

        public string ValidityDate { get; set; } = string.Empty;

        public string CreatedAt { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;
    }
}
