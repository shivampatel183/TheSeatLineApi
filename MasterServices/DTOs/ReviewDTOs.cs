using System.ComponentModel.DataAnnotations;

namespace TheSeatLineApi.MasterServices.DTOs
{
    public class ReviewInsertDto
    {
        public Guid? EventId { get; set; }
        public Guid? VenueId { get; set; }

        [Required]
        public Guid BookingId { get; set; }

        [Required, Range(1, 5)]
        public byte Rating { get; set; }

        [MaxLength(200)]
        public string? Title { get; set; }

        public string? Body { get; set; }
    }

    public class ReviewSelectDto
    {
        public Guid Id { get; set; }
        public Guid? EventId { get; set; }
        public Guid? VenueId { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; } = null!;
        public byte Rating { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }
        public bool IsVerified { get; set; }
        public int HelpfulVotes { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
