using TheSeatLineApi.BookingServices.DTOs;
using TheSeatLineApi.BookingServices.Entity;
using TheSeatLineApi.BookingServices.Repository;
using TheSeatLineApi.Common;
using TheSeatLineApi.Common.Enums;

namespace TheSeatLineApi.BookingServices.Business
{
    public class BookingBusiness : IBookingBusiness
    {
        private readonly IBookingRepository _bookingRepository;
        private const int BOOKING_EXPIRY_MINUTES = 10;

        public BookingBusiness(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<Response<BookingResponseDto>> CreateBookingAsync(Guid userId, CreateBookingRequestDto request)
        {
            try
            {
                // Check seat availability
                var availableSeats = await _bookingRepository.GetAvailableSeatsAsync(request.ShowSeatCategoryId);
                
                if (availableSeats < request.NumberOfSeats)
                {
                    return Response<BookingResponseDto>.Fail(
                        $"Only {availableSeats} seats available. You requested {request.NumberOfSeats} seats."
                    );
                }

                // Lock seats by updating available seats
                var seatsLocked = await _bookingRepository.UpdateAvailableSeatsAsync(
                    request.ShowSeatCategoryId, 
                    request.NumberOfSeats
                );

                if (!seatsLocked)
                {
                    return Response<BookingResponseDto>.Fail(
                        "Failed to lock seats. Please try again."
                    );
                }

                // Create booking entity
                var booking = new BookingEntity
                {
                    UserId = userId,
                    ShowId = request.ShowId,
                    ShowSeatCategoryId = request.ShowSeatCategoryId,
                    NumberOfSeats = request.NumberOfSeats,
                    TotalAmount = 0, // Will be calculated after fetching seat category price
                    BookingStatus = BookingStatus.Pending,
                    BookingDate = DateTime.UtcNow,
                    ExpiryTime = DateTime.UtcNow.AddMinutes(BOOKING_EXPIRY_MINUTES)
                };

                var createdBooking = await _bookingRepository.CreateBookingAsync(booking);
                
                // Fetch full booking details
                var bookingDetails = await _bookingRepository.GetBookingByIdAsync(createdBooking.Id);
                
                if (bookingDetails == null)
                {
                    return Response<BookingResponseDto>.Fail("Failed to retrieve booking details.");
                }

                // Update total amount based on actual price
                bookingDetails.TotalAmount = bookingDetails.ShowSeatCategory.Price * request.NumberOfSeats;
                await _bookingRepository.UpdateBookingAsync(bookingDetails);

                var responseDto = MapToResponseDto(bookingDetails);
                
                return Response<BookingResponseDto>.Ok(
                    responseDto,
                    "Booking created successfully. Please complete payment within 10 minutes."
                );
            }
            catch (Exception ex)
            {
                return Response<BookingResponseDto>.Fail(
                    $"An error occurred while creating booking: {ex.Message}"
                );
            }
        }

        public async Task<Response<BookingResponseDto>> GetBookingDetailsAsync(Guid bookingId, Guid userId)
        {
            try
            {
                var booking = await _bookingRepository.GetBookingByIdAsync(bookingId);

                if (booking == null)
                {
                    return Response<BookingResponseDto>.Fail("Booking not found.");
                }

                if (booking.UserId != userId)
                {
                    return Response<BookingResponseDto>.Fail("Unauthorized access to booking.");
                }

                var responseDto = MapToResponseDto(booking);
                return Response<BookingResponseDto>.Ok(responseDto);
            }
            catch (Exception ex)
            {
                return Response<BookingResponseDto>.Fail(
                    $"An error occurred while retrieving booking: {ex.Message}"
                );
            }
        }

        public async Task<Response<List<BookingResponseDto>>> GetUserBookingsAsync(Guid userId)
        {
            try
            {
                var bookings = await _bookingRepository.GetUserBookingsAsync(userId);
                var responseDtos = bookings.Select(MapToResponseDto).ToList();
                
                return Response<List<BookingResponseDto>>.Ok(
                    responseDtos,
                    $"Retrieved {responseDtos.Count} booking(s)."
                );
            }
            catch (Exception ex)
            {
                return Response<List<BookingResponseDto>>.Fail(
                    $"An error occurred while retrieving bookings: {ex.Message}"
                );
            }
        }

        public async Task<Response<BookingResponseDto>> ConfirmBookingAsync(Guid bookingId, Guid userId)
        {
            try
            {
                var booking = await _bookingRepository.GetBookingByIdAsync(bookingId);

                if (booking == null)
                {
                    return Response<BookingResponseDto>.Fail("Booking not found.");
                }

                if (booking.UserId != userId)
                {
                    return Response<BookingResponseDto>.Fail("Unauthorized access to booking.");
                }

                if (booking.BookingStatus != BookingStatus.Pending)
                {
                    return Response<BookingResponseDto>.Fail(
                        $"Cannot confirm booking with status: {booking.BookingStatus}"
                    );
                }

                // Check if booking has expired
                if (booking.ExpiryTime.HasValue && booking.ExpiryTime.Value < DateTime.UtcNow)
                {
                    booking.BookingStatus = BookingStatus.Expired;
                    await _bookingRepository.UpdateBookingAsync(booking);
                    
                    // Release seats
                    await _bookingRepository.ReleaseSeatsAsync(
                        booking.ShowSeatCategoryId, 
                        booking.NumberOfSeats
                    );

                    return Response<BookingResponseDto>.Fail("Booking has expired.");
                }

                // Confirm booking
                booking.BookingStatus = BookingStatus.Confirmed;
                booking.ConfirmedAt = DateTime.UtcNow;
                booking.ExpiryTime = null; // Clear expiry time

                var updatedBooking = await _bookingRepository.UpdateBookingAsync(booking);
                var responseDto = MapToResponseDto(updatedBooking);

                return Response<BookingResponseDto>.Ok(
                    responseDto,
                    "Booking confirmed successfully."
                );
            }
            catch (Exception ex)
            {
                return Response<BookingResponseDto>.Fail(
                    $"An error occurred while confirming booking: {ex.Message}"
                );
            }
        }

        public async Task<Response<BookingResponseDto>> CancelBookingAsync(
            Guid bookingId, 
            Guid userId, 
            string? cancellationReason)
        {
            try
            {
                var booking = await _bookingRepository.GetBookingByIdAsync(bookingId);

                if (booking == null)
                {
                    return Response<BookingResponseDto>.Fail("Booking not found.");
                }

                if (booking.UserId != userId)
                {
                    return Response<BookingResponseDto>.Fail("Unauthorized access to booking.");
                }

                if (booking.BookingStatus == BookingStatus.Cancelled)
                {
                    return Response<BookingResponseDto>.Fail("Booking is already cancelled.");
                }

                if (booking.BookingStatus == BookingStatus.Expired)
                {
                    return Response<BookingResponseDto>.Fail("Cannot cancel an expired booking.");
                }

                // Cancel booking
                booking.BookingStatus = BookingStatus.Cancelled;
                booking.CancelledAt = DateTime.UtcNow;
                booking.CancellationReason = cancellationReason;

                // Release seats
                await _bookingRepository.ReleaseSeatsAsync(
                    booking.ShowSeatCategoryId, 
                    booking.NumberOfSeats
                );

                var updatedBooking = await _bookingRepository.UpdateBookingAsync(booking);
                var responseDto = MapToResponseDto(updatedBooking);

                return Response<BookingResponseDto>.Ok(
                    responseDto,
                    "Booking cancelled successfully."
                );
            }
            catch (Exception ex)
            {
                return Response<BookingResponseDto>.Fail(
                    $"An error occurred while cancelling booking: {ex.Message}"
                );
            }
        }

        public async Task<Response<BookingResponseDto>> TransferBookingAsync(
            Guid bookingId, 
            Guid currentUserId, 
            TransferBookingRequestDto request)
        {
            try
            {
                var booking = await _bookingRepository.GetBookingByIdAsync(bookingId);

                if (booking == null)
                {
                    return Response<BookingResponseDto>.Fail("Booking not found.");
                }

                if (booking.UserId != currentUserId)
                {
                    return Response<BookingResponseDto>.Fail("Unauthorized access to booking.");
                }

                if (booking.BookingStatus != BookingStatus.Confirmed)
                {
                    return Response<BookingResponseDto>.Fail(
                        "Only confirmed bookings can be transferred."
                    );
                }

                // Check if booking has already been transferred
                if (booking.BookingStatus == BookingStatus.Transfered)
                {
                    return Response<BookingResponseDto>.Fail("Booking has already been transferred.");
                }

                // Find recipient user by email
                var recipientUserId = await _bookingRepository.GetUserIdByEmailAsync(request.RecipientEmail);

                if (!recipientUserId.HasValue)
                {
                    return Response<BookingResponseDto>.Fail(
                        $"No user found with email: {request.RecipientEmail}"
                    );
                }

                if (recipientUserId.Value == currentUserId)
                {
                    return Response<BookingResponseDto>.Fail(
                        "Cannot transfer booking to yourself."
                    );
                }

                // Transfer booking
                booking.OriginalUserId = currentUserId;
                booking.UserId = recipientUserId.Value;
                booking.BookingStatus = BookingStatus.Transfered;
                booking.TransferredAt = DateTime.UtcNow;
                booking.TransferNote = request.TransferNote;

                var updatedBooking = await _bookingRepository.UpdateBookingAsync(booking);
                
                // Reload with navigation properties
                var bookingDetails = await _bookingRepository.GetBookingByIdAsync(updatedBooking.Id);
                var responseDto = MapToResponseDto(bookingDetails!);

                return Response<BookingResponseDto>.Ok(
                    responseDto,
                    $"Booking transferred successfully to {request.RecipientEmail}."
                );
            }
            catch (Exception ex)
            {
                return Response<BookingResponseDto>.Fail(
                    $"An error occurred while transferring booking: {ex.Message}"
                );
            }
        }

        public async Task ExpireBookingsAsync()
        {
            try
            {
                var expiredBookings = await _bookingRepository.GetExpiredBookingsAsync();

                foreach (var booking in expiredBookings)
                {
                    booking.BookingStatus = BookingStatus.Expired;
                    await _bookingRepository.UpdateBookingAsync(booking);

                    // Release seats
                    await _bookingRepository.ReleaseSeatsAsync(
                        booking.ShowSeatCategoryId, 
                        booking.NumberOfSeats
                    );
                }
            }
            catch (Exception ex)
            {
                // Log error - in production, use proper logging
                Console.WriteLine($"Error expiring bookings: {ex.Message}");
            }
        }

        private BookingResponseDto MapToResponseDto(BookingEntity booking)
        {
            // Generate seat details based on number of seats
            var seats = new List<BookingSeatDto>();
            var pricePerSeat = booking.ShowSeatCategory.Price;
            
            for (int i = 0; i < booking.NumberOfSeats; i++)
            {
                // Generate seat number (A1, A2, B1, B2, etc.)
                int rowIndex = i / 10; // 10 seats per row
                int columnIndex = (i % 10) + 1;
                char rowLetter = (char)('A' + rowIndex);
                
                seats.Add(new BookingSeatDto
                {
                    Id = i + 1,
                    BookingId = booking.Id,
                    SeatId = i + 1, // Placeholder
                    SeatNumber = $"{rowLetter}{columnIndex}",
                    SeatType = booking.ShowSeatCategory.SeatCategoryName,
                    Price = pricePerSeat,
                    Row = rowLetter.ToString(),
                    Column = columnIndex
                });
            }

            // Calculate total capacity from all seat categories for this show
            var totalCapacity = booking.Show.ShowSeatCategories?.Sum(sc => sc.TotalSeats) ?? 0;
            var totalAvailable = booking.Show.ShowSeatCategories?.Sum(sc => sc.AvailableSeats) ?? 0;

            // Map payment status based on booking status
            string paymentStatus = booking.BookingStatus switch
            {
                BookingStatus.Confirmed => "Paid",
                BookingStatus.Pending => "Pending",
                BookingStatus.Failed => "Failed",
                BookingStatus.Cancelled => "Refunded",
                BookingStatus.Refunded => "Refunded",
                _ => "Unknown"
            };

            return new BookingResponseDto
            {
                Id = booking.Id.GetHashCode(), // Convert Guid to int for response
                UserId = booking.UserId,
                VenueId = booking.Show.VenueId,
                ShowId = booking.ShowId,
                BookingDate = booking.BookingDate,
                TotalAmount = booking.TotalAmount,
                Status = booking.BookingStatus.ToString(),
                PaymentStatus = paymentStatus,
                CreatedAt = booking.BookingDate,
                UpdatedAt = booking.ConfirmedAt ?? booking.CancelledAt ?? booking.TransferredAt,
                
                // Transfer details
                OriginalUserId = booking.OriginalUserId,
                OriginalUserName = null, // Can be populated if needed
                TransferredAt = booking.TransferredAt,
                TransferNote = booking.TransferNote,
                
                // Expiry and cancellation
                ExpiryTime = booking.ExpiryTime,
                CancelledAt = booking.CancelledAt,
                CancellationReason = booking.CancellationReason,
                
                // Nested seat details
                Seats = seats,
                
                // Nested venue details
                Venue = new VenueDetailDto
                {
                    Id = booking.Show.Venue.Id,
                    Name = booking.Show.Venue.Name,
                    Address = booking.Show.Venue.Address,
                    City = booking.Show.Venue.City.Name,
                    State = null, // Add if you have state field
                    ZipCode = null, // Add if you have zipcode field
                    ImageUrl = null, // Add if you have venue images
                    Description = null, // Add if you have venue description
                    Capacity = totalCapacity
                },
                
                // Nested show details
                Show = new ShowDetailDto
                {
                    Id = booking.Show.Id,
                    VenueId = booking.Show.VenueId,
                    Title = booking.Show.Event.Title,
                    Description = booking.Show.Event.Description,
                    ShowDate = booking.Show.ShowTime,
                    ShowTime = booking.Show.ShowTime.ToString("h:mm tt"),
                    Duration = booking.Show.Event.DurationMinutes,
                    Genre = booking.Show.Event.Language, // Using language as genre for now
                    ImageUrl = booking.Show.Event.PosterUrl,
                    Price = pricePerSeat,
                    AvailableSeats = totalAvailable
                }
            };
        }
    }
}

