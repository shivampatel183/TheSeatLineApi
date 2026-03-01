-- Official General Dummy Data Script (Guid version)
-- Matches TheSeatLineDB Table Structure (March 2026)

-- ============================================
-- SETUP VARIABLES
-- ============================================
DECLARE @TenantId UNIQUEIDENTIFIER = '00000000-0000-0000-0000-000000000001';
DECLARE @CityIdMumbai UNIQUEIDENTIFIER = '11111111-1111-1111-1111-111111111111';
DECLARE @VenueId UNIQUEIDENTIFIER = '22222222-2222-2222-2222-222222222222';

-- ============================================
-- 1. CITIES
-- ============================================
IF NOT EXISTS (SELECT 1 FROM Cities WHERE Id = @CityIdMumbai)
BEGIN
    INSERT INTO Cities (Id, Name, State, Country, IsActive, CreatedAt)
    VALUES (@CityIdMumbai, 'Mumbai', 'Maharashtra', 'India', 1, GETUTCDATE());
END

-- ============================================
-- 2. VENUE
-- ============================================
IF NOT EXISTS (SELECT 1 FROM Venues WHERE Id = @VenueId)
BEGIN
    INSERT INTO Venues (
        Id, TenantId, Name, VenueType, Description, 
        AddressLine1, PostalCode, Timezone, TotalCapacity, 
        IsActive, IsDeleted, CreatedAt, UpdatedAt, CityId
    )
    VALUES (
        @VenueId, @TenantId, 'PVR ICON, Lower Parel', 1, 'Premium cinema experience', 
        'Phoenix Palladium Mall', '400013', 'Asia/Kolkata', 500, 
        1, 0, GETUTCDATE(), GETUTCDATE(), @CityIdMumbai
    );
END

-- ============================================
-- 3. EVENTS
-- ============================================
DECLARE @Event1Id UNIQUEIDENTIFIER = 'E1E1E1E1-E1E1-E1E1-E1E1-E1E1E1E1E1E1';
DECLARE @Event2Id UNIQUEIDENTIFIER = 'E2E2E2E2-E2E2-E2E2-E2E2-E2E2E2E2E2E2';

INSERT INTO Events (
    Id, TenantId, VenueId, Title, Slug, Description, 
    EventType, StartDateTime, EndDateTime, Timezone, 
    IsRecurring, MaxCapacity, Status, Language, 
    IsDeleted, CreatedAt, UpdatedAt
)
VALUES 
(
    @Event1Id, @TenantId, @VenueId, 'Avengers: Endgame', 'avengers-endgame', 'Epic conclusion to the Infinity Saga', 
    1, DATEADD(DAY, 1, GETUTCDATE()), DATEADD(HOUR, 3, DATEADD(DAY, 1, GETUTCDATE())), 'Asia/Kolkata', 
    0, 200, 1, 'English', 
    0, GETUTCDATE(), GETUTCDATE()
),
(
    @Event2Id, @TenantId, @VenueId, 'Inception', 'inception', 'A mind-bending thriller', 
    1, DATEADD(DAY, 2, GETUTCDATE()), DATEADD(HOUR, 2, DATEADD(DAY, 2, GETUTCDATE())), 'Asia/Kolkata', 
    0, 150, 1, 'English', 
    0, GETUTCDATE(), GETUTCDATE()
);

-- ============================================
-- 4. SEATS (Sample seats for Event 1)
-- ============================================
-- Matches Seats Table: No CreatedAt/UpdatedAt
INSERT INTO Seats (
    Id, EventId, Section, [Row], SeatNumber, 
    SeatType, Status, BasePrice, Currency, 
    IsAisle, IsWindow, IsEmergencyExit
)
VALUES
(NEWID(), @Event1Id, 'Premium', 'A', 'A1', 1, 1, 500.00, 'INR', 1, 0, 0),
(NEWID(), @Event1Id, 'Premium', 'A', 'A2', 1, 1, 500.00, 'INR', 0, 0, 0),
(NEWID(), @Event1Id, 'General', 'B', 'B1', 1, 1, 300.00, 'INR', 1, 0, 0),
(NEWID(), @Event1Id, 'General', 'B', 'B2', 1, 1, 300.00, 'INR', 0, 0, 0);

PRINT 'Dummy events and related data inserted successfully!';
