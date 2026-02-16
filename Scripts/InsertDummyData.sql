-- Dummy Data for TheSeatLine API Testing
-- User ID: 3118B74C-92CF-4BA5-9ECF-E06690F9496C

-- ============================================
-- 1. CITIES
-- ============================================
SET IDENTITY_INSERT Cities ON;

INSERT INTO Cities (Id, Name, IsActive, CreatedAt) VALUES
(1, 'Mumbai', 1, GETUTCDATE()),
(2, 'Delhi', 1, GETUTCDATE()),
(3, 'Bangalore', 1, GETUTCDATE()),
(4, 'Hyderabad', 1, GETUTCDATE()),
(5, 'Chennai', 1, GETUTCDATE());

SET IDENTITY_INSERT Cities OFF;

-- ============================================
-- 2. VENUES (with GPS coordinates)
-- ============================================
SET IDENTITY_INSERT Venues ON;

INSERT INTO Venues (Id, Name, CityId, Address, Latitude, Longitude, IsActive, CreatedAt) VALUES
-- Mumbai Venues
(1, 'PVR Juhu', 1, 'Juhu Tara Road, Mumbai', 19.0990, 72.8265, 1, GETUTCDATE()),
(2, 'INOX Marine Drive', 1, 'Nariman Point, Mumbai', 18.9432, 72.8236, 1, GETUTCDATE()),
(3, 'Cinepolis Andheri', 1, 'Andheri West, Mumbai', 19.1136, 72.8697, 1, GETUTCDATE()),

-- Delhi Venues
(4, 'PVR Saket', 2, 'Saket, New Delhi', 28.5244, 77.2066, 1, GETUTCDATE()),
(5, 'Cinepolis DLF Place', 2, 'Saket, New Delhi', 28.5355, 77.2150, 1, GETUTCDATE()),

-- Bangalore Venues
(6, 'PVR Forum Mall', 3, 'Koramangala, Bangalore', 12.9345, 77.6101, 1, GETUTCDATE()),
(7, 'INOX Garuda Mall', 3, 'Magrath Road, Bangalore', 12.9716, 77.6412, 1, GETUTCDATE()),

-- Hyderabad Venues
(8, 'PVR Inorbit', 4, 'Madhapur, Hyderabad', 17.4399, 78.3908, 1, GETUTCDATE()),

-- Chennai Venues
(9, 'PVR Grand Galada', 5, 'Pallavaram, Chennai', 12.9675, 80.1491, 1, GETUTCDATE());

SET IDENTITY_INSERT Venues OFF;

-- ============================================
-- 3. EVENTS
-- ============================================
SET IDENTITY_INSERT Events ON;

INSERT INTO Events (Id, Title, Description, Language, DurationMinutes, PosterUrl, TrailerUrl, ReleaseDate, IsActive, CreatedAt) VALUES
(1, 'Avengers: Endgame', 'Epic conclusion to the Infinity Saga', 'English', 180, 'https://example.com/avengers.jpg', 'https://youtube.com/avengers', '2019-04-26', 1, GETUTCDATE()),
(2, 'Inception', 'A mind-bending thriller', 'English', 148, 'https://example.com/inception.jpg', 'https://youtube.com/inception', '2010-07-16', 1, GETUTCDATE()),
(3, 'RRR', 'Indian epic action drama', 'Telugu', 187, 'https://example.com/rrr.jpg', 'https://youtube.com/rrr', '2022-03-25', 1, GETUTCDATE()),
(4, 'Pathaan', 'Action-packed thriller', 'Hindi', 146, 'https://example.com/pathaan.jpg', 'https://youtube.com/pathaan', '2023-01-25', 1, GETUTCDATE()),
(5, 'Jawan', 'High-octane action entertainer', 'Hindi', 169, 'https://example.com/jawan.jpg', 'https://youtube.com/jawan', '2023-09-07', 1, GETUTCDATE()),
(6, 'The Dark Knight', 'Batman faces the Joker', 'English', 152, 'https://example.com/batman.jpg', 'https://youtube.com/batman', '2008-07-18', 1, GETUTCDATE()),
(7, 'Interstellar', 'Journey through space and time', 'English', 169, 'https://example.com/interstellar.jpg', 'https://youtube.com/interstellar', '2014-11-07', 1, GETUTCDATE()),
(8, 'Baahubali 2', 'The conclusion of epic saga', 'Telugu', 167, 'https://example.com/baahubali.jpg', 'https://youtube.com/baahubali', '2017-04-28', 1, GETUTCDATE());

SET IDENTITY_INSERT Events OFF;

-- ============================================
-- 4. SHOWS (Upcoming shows for next 7 days)
-- ============================================
SET IDENTITY_INSERT Shows ON;

DECLARE @Today DATETIME = GETUTCDATE();

INSERT INTO Shows (Id, EventId, VenueId, ShowTime, IsActive, CreatedAt) VALUES
-- Avengers in Mumbai (Next 3 days)
(1, 1, 1, DATEADD(DAY, 1, @Today), 1, GETUTCDATE()),
(2, 1, 1, DATEADD(HOUR, 3, DATEADD(DAY, 1, @Today)), 1, GETUTCDATE()),
(3, 1, 2, DATEADD(DAY, 2, @Today), 1, GETUTCDATE()),
(4, 1, 3, DATEADD(DAY, 3, @Today), 1, GETUTCDATE()),

-- Inception in Mumbai & Delhi
(5, 2, 1, DATEADD(DAY, 1, @Today), 1, GETUTCDATE()),
(6, 2, 2, DATEADD(DAY, 2, @Today), 1, GETUTCDATE()),
(7, 2, 4, DATEADD(DAY, 1, @Today), 1, GETUTCDATE()),
(8, 2, 5, DATEADD(DAY, 3, @Today), 1, GETUTCDATE()),

-- RRR in Hyderabad & Bangalore
(9, 3, 6, DATEADD(DAY, 1, @Today), 1, GETUTCDATE()),
(10, 3, 7, DATEADD(DAY, 2, @Today), 1, GETUTCDATE()),
(11, 3, 8, DATEADD(DAY, 1, @Today), 1, GETUTCDATE()),

-- Pathaan in Mumbai & Delhi
(12, 4, 1, DATEADD(DAY, 4, @Today), 1, GETUTCDATE()),
(13, 4, 2, DATEADD(DAY, 5, @Today), 1, GETUTCDATE()),
(14, 4, 4, DATEADD(DAY, 4, @Today), 1, GETUTCDATE()),

-- Jawan in Chennai & Mumbai
(15, 5, 9, DATEADD(DAY, 2, @Today), 1, GETUTCDATE()),
(16, 5, 1, DATEADD(DAY, 3, @Today), 1, GETUTCDATE()),

-- Dark Knight in Bangalore
(17, 6, 6, DATEADD(DAY, 5, @Today), 1, GETUTCDATE()),
(18, 6, 7, DATEADD(DAY, 6, @Today), 1, GETUTCDATE()),

-- Interstellar in Delhi
(19, 7, 4, DATEADD(DAY, 6, @Today), 1, GETUTCDATE()),
(20, 7, 5, DATEADD(DAY, 7, @Today), 1, GETUTCDATE()),

-- Baahubali in Hyderabad
(21, 8, 8, DATEADD(DAY, 5, @Today), 1, GETUTCDATE());

SET IDENTITY_INSERT Shows OFF;

-- ============================================
-- 5. SHOW SEAT CATEGORIES
-- ============================================
SET IDENTITY_INSERT ShowSeatCategories ON;

INSERT INTO ShowSeatCategories (Id, ShowId, SeatCategoryName, Price, TotalSeats, AvailableSeats, IsActive, CreatedAt) VALUES
-- Show 1 (Avengers - PVR Juhu)
(1, 1, 'Gold', 500.00, 50, 45, 1, GETUTCDATE()),
(2, 1, 'Silver', 300.00, 100, 95, 1, GETUTCDATE()),
(3, 1, 'Platinum', 800.00, 30, 28, 1, GETUTCDATE()),

-- Show 2 (Avengers - PVR Juhu Evening)
(4, 2, 'Gold', 500.00, 50, 50, 1, GETUTCDATE()),
(5, 2, 'Silver', 300.00, 100, 100, 1, GETUTCDATE()),

-- Show 3 (Avengers - INOX Marine)
(6, 3, 'Premium', 600.00, 60, 55, 1, GETUTCDATE()),
(7, 3, 'Regular', 350.00, 120, 115, 1, GETUTCDATE()),

-- Show 5 (Inception - PVR Juhu)
(8, 5, 'Gold', 450.00, 50, 48, 1, GETUTCDATE()),
(9, 5, 'Silver', 280.00, 100, 98, 1, GETUTCDATE()),

-- Show 7 (Inception - PVR Saket Delhi)
(10, 7, 'Gold', 400.00, 50, 50, 1, GETUTCDATE()),
(11, 7, 'Silver', 250.00, 100, 100, 1, GETUTCDATE()),

-- Show 9 (RRR - PVR Forum Bangalore)
(12, 9, 'Recliner', 700.00, 40, 38, 1, GETUTCDATE()),
(13, 9, 'Premium', 500.00, 80, 75, 1, GETUTCDATE()),
(14, 9, 'Regular', 300.00, 100, 95, 1, GETUTCDATE()),

-- Show 11 (RRR - PVR Inorbit Hyderabad)
(15, 11, 'Premium', 550.00, 60, 58, 1, GETUTCDATE()),
(16, 11, 'Regular', 320.00, 100, 98, 1, GETUTCDATE()),

-- Show 12 (Pathaan - PVR Juhu)
(17, 12, 'Gold', 500.00, 50, 50, 1, GETUTCDATE()),
(18, 12, 'Silver', 300.00, 100, 100, 1, GETUTCDATE()),

-- Show 15 (Jawan - Chennai)
(19, 15, 'Premium', 450.00, 70, 70, 1, GETUTCDATE()),
(20, 15, 'Regular', 280.00, 110, 110, 1, GETUTCDATE());

SET IDENTITY_INSERT ShowSeatCategories OFF;

-- ============================================
-- 6. SAMPLE BOOKINGS for User
-- ============================================

DECLARE @UserId UNIQUEIDENTIFIER = '3118B74C-92CF-4BA5-9ECF-E06690F9496C';

-- Confirmed Booking
INSERT INTO Bookings (Id, UserId, ShowId, ShowSeatCategoryId, NumberOfSeats, TotalAmount, BookingStatus, BookingDate, ExpiryTime, ConfirmedAt, CancelledAt, CancellationReason, OriginalUserId, TransferredAt, TransferNote)
VALUES 
(NEWID(), @UserId, 1, 1, 2, 1000.00, 2, GETUTCDATE(), NULL, DATEADD(MINUTE, 5, GETUTCDATE()), NULL, NULL, NULL, NULL, NULL);

-- Pending Booking (will expire in 10 minutes)
INSERT INTO Bookings (Id, UserId, ShowId, ShowSeatCategoryId, NumberOfSeats, TotalAmount, BookingStatus, BookingDate, ExpiryTime, ConfirmedAt, CancelledAt, CancellationReason, OriginalUserId, TransferredAt, TransferNote)
VALUES 
(NEWID(), @UserId, 5, 8, 3, 1350.00, 1, GETUTCDATE(), DATEADD(MINUTE, 10, GETUTCDATE()), NULL, NULL, NULL, NULL, NULL, NULL);

-- Cancelled Booking
INSERT INTO Bookings (Id, UserId, ShowId, ShowSeatCategoryId, NumberOfSeats, TotalAmount, BookingStatus, BookingDate, ExpiryTime, ConfirmedAt, CancelledAt, CancellationReason, OriginalUserId, TransferredAt, TransferNote)
VALUES 
(NEWID(), @UserId, 3, 6, 2, 1200.00, 4, DATEADD(DAY, -2, GETUTCDATE()), NULL, NULL, DATEADD(DAY, -1, GETUTCDATE()), 'Change of plans', NULL, NULL, NULL);

-- Confirmed Booking for RRR
INSERT INTO Bookings (Id, UserId, ShowId, ShowSeatCategoryId, NumberOfSeats, TotalAmount, BookingStatus, BookingDate, ExpiryTime, ConfirmedAt, CancelledAt, CancellationReason, OriginalUserId, TransferredAt, TransferNote)
VALUES 
(NEWID(), @UserId, 9, 13, 4, 2000.00, 2, DATEADD(HOUR, -2, GETUTCDATE()), NULL, DATEADD(HOUR, -1, GETUTCDATE()), NULL, NULL, NULL, NULL, NULL);

PRINT 'Dummy data inserted successfully!';
PRINT '';
PRINT 'Summary:';
PRINT '- 5 Cities (Mumbai, Delhi, Bangalore, Hyderabad, Chennai)';
PRINT '- 9 Venues with GPS coordinates';
PRINT '- 8 Events (Movies)';
PRINT '- 21 Shows (upcoming in next 7 days)';
PRINT '- 20 Seat Categories with pricing';
PRINT '- 4 Sample Bookings for user ' + CAST(@UserId AS VARCHAR(50));
PRINT '';
PRINT 'You can now test:';
PRINT '1. Location-based discovery: GET /api/event/nearby?latitude=19.0760&longitude=72.8777&radiusKm=10';
PRINT '2. City-based events: GET /api/event/city/1';
PRINT '3. User bookings: GET /api/booking/user';
PRINT '4. Create new booking: POST /api/booking';
