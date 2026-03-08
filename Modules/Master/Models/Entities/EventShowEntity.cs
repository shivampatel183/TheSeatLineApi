using System;
using System.Collections.Generic;
using TheSeatLineApi.Modules.BookingModule.Models.Entities;
using TheSeatLineApi.Modules.MasterModule.Models.Entities;

public class EventShow : BaseEntity
{
    public Guid EventId { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public byte Status { get; set; }
    public int MaxCapacity { get; set; }

    public Event Event { get; set; } = null!;
    public ICollection<TicketCategory> TicketCategories { get; set; } = new List<TicketCategory>();
    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    public ICollection<Seat> Seats { get; set; } = new List<Seat>();
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    public ICollection<SeatReservation> SeatReservations { get; set; } = new List<SeatReservation>();
    public virtual ICollection<WaitingList> WaitingLists { get; set; }
}



