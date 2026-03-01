-- Official Ahmedabad Dummy Data Script
-- City ID: DC87FA55-B7EF-431C-8DE7-1E783C833E3E
-- Matches TheSeatLineDB Table Structure (March 2026)

-- ============================================
-- SETUP VARIABLES
-- ============================================
DECLARE @TenantId UNIQUEIDENTIFIER = '00000000-0000-0000-0000-000000000001';
DECLARE @AhmedabadCityId UNIQUEIDENTIFIER = 'DC87FA55-B7EF-431C-8DE7-1E783C833E3E';
DECLARE @AhmedabadVenueId UNIQUEIDENTIFIER = 'A11EDBA0-0000-0000-0000-A11EDBA00000';

-- ============================================
-- 1. CITY (Ahmedabad)
-- ============================================
IF NOT EXISTS (SELECT 1 FROM Cities WHERE Id = @AhmedabadCityId)
BEGIN
    INSERT INTO Cities (Id, Name, State, Country, IsActive, CreatedAt)
    VALUES (@AhmedabadCityId, 'Ahmedabad', 'Gujarat', 'India', 1, GETUTCDATE());
END

-- ============================================
-- 2. VENUE (Ahmedabad)
-- ============================================
IF NOT EXISTS (SELECT 1 FROM Venues WHERE Id = @AhmedabadVenueId)
BEGIN
    INSERT INTO Venues (
        Id, TenantId, Name, VenueType, Description, 
        AddressLine1, PostalCode, Timezone, TotalCapacity, 
        IsActive, IsDeleted, CreatedAt, UpdatedAt, CityId
    )
    VALUES (
        @AhmedabadVenueId, @TenantId, 'PVR Acropolis, Ahmedabad', 1, 'Premium cinema experience in Ahmedabad', 
        'Thaltej Cross Roads, SG Highway', '380054', 'Asia/Kolkata', 450, 
        1, 0, GETUTCDATE(), GETUTCDATE(), @AhmedabadCityId
    );
END

-- ============================================
-- 3. EVENTS (in Ahmedabad)
-- ============================================
DECLARE @Event1Id UNIQUEIDENTIFIER = 'AE000001-0000-0000-0000-AE0000000001';
DECLARE @Event2Id UNIQUEIDENTIFIER = 'AE000002-0000-0000-0000-AE0000000002';

INSERT INTO Events (
    Id, TenantId, VenueId, Title, Slug, Description, 
    EventType, StartDateTime, EndDateTime, Timezone, 
    IsRecurring, MaxCapacity, Status, Language, 
    IsDeleted, CreatedAt, UpdatedAt
)
VALUES 
(
    @Event1Id, @TenantId, @AhmedabadVenueId, 'Pathaan', 'pathaan-ahmedabad', 'Action-packed thriller starring Shah Rukh Khan', 
    1, DATEADD(DAY, 1, GETUTCDATE()), DATEADD(HOUR, 3, DATEADD(DAY, 1, GETUTCDATE())), 'Asia/Kolkata', 
    0, 200, 1, 'Hindi', 
    0, GETUTCDATE(), GETUTCDATE()
),
(
    @Event2Id, @TenantId, @AhmedabadVenueId, 'Jawan', 'jawan-ahmedabad', 'High-octane action entertainer', 
    1, DATEADD(DAY, 2, GETUTCDATE()), DATEADD(HOUR, 3, DATEADD(DAY, 2, GETUTCDATE())), 'Asia/Kolkata', 
    0, 150, 1, 'Hindi', 
    0, GETUTCDATE(), GETUTCDATE()
);

-- ============================================
-- 4. SEATS (Sample seats for Event 1 in Ahmedabad)
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
(NEWID(), @Event1Id, 'General', 'B', 'B1', 1, 1, 300.00, 'INR', 1, 0, 0);

PRINT 'Ahmedabad Events and related data inserted successfully!';
