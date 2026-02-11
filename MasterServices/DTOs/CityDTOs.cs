namespace TheSeatLineApi.MasterServices.DTOs
{
    public class CityInsertDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public bool IsActive { get; set; } = true;

    }
    public class CitySelectDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
