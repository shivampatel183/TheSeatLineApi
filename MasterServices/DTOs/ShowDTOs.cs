namespace TheSeatLineApi.MasterServices.DTOs
{
    public class ShowInsertDTO
    {
        public int Id { get; set; }

        public int EventId { get; set; }

        public int VenueId { get; set; }

        public DateTime ShowTime { get; set; }

        public bool IsActive { get; set; }
    }
    public class ShowSelectDTO
    {
        public int Id { get; set; }

        public int EventId { get; set; }

        public string EventTitle { get; set; }

        public int VenueId { get; set; }

        public string VenueName { get; set; }

        public DateTime ShowTime { get; set; }
    }
}
