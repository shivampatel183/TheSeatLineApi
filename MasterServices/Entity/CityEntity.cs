namespace TheSeatLineApi.MasterServices.Entity
{
    public class CityEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
