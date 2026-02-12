namespace TheSeatLineApi.MasterServices.DTOs
{
    public class ShowSeatCategoryInsertDTO
    {
        public int Id { get; set; }

        public int ShowId { get; set; }

        public string SeatCategoryName { get; set; }

        public decimal Price { get; set; }

        public int TotalSeats { get; set; }

        public int AvailableSeats { get; set; }

        public bool IsActive { get; set; }
    }
    public class ShowSeatCategorySelectDTO
    {
        public int Id { get; set; }

        public int ShowId { get; set; }

        public string EventTitle { get; set; }

        public string VenueName { get; set; }

        public DateTime ShowTime { get; set; }

        public string SeatCategoryName { get; set; }

        public decimal Price { get; set; }

        public int AvailableSeats { get; set; }
    }
}
