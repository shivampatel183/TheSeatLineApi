namespace TheSeatLineApi.MasterServices.Entity
{
    public class VenueEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CityId { get; set; }

        public string? Address { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public CityEntity City { get; set; }
    }

}
