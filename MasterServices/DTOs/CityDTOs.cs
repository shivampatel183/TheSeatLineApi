namespace TheSeatLineApi.MasterServices.DTOs
{
    public record CitySelectDto
    {
        public string Name { get; init; }
        public string State { get; init; }
        public string Country { get; init; }
    }
}
