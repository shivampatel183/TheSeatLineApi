namespace TheSeatLineApi.MasterServices.DTOs
{
    public class VenueSelectDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CityId { get; set; }

        public string CityName { get; set; }

        public string? Address { get; set; }
    }
    public class VenueInsertDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CityId { get; set; }

        public string? Address { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        public bool IsActive { get; set; }
    }

}
