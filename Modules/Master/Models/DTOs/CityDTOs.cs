namespace TheSeatLineApi.Modules.MasterModule.Models.DTOs
{
        public class CityListDto
        {
            public Guid Id { get; set; }
            public string Name { get; set; } = null!;
            public string State { get; set; } = null!;
            public string Country { get; set; } = null!;
            public string Slug { get; set; } = null!;
            public bool IsActive { get; set; }
        }

        public class CityDetailDto : CityListDto { }

        public class CityCreateDto
        {
            public string Name { get; set; } = null!;
            public string State { get; set; } = null!;
            public string Country { get; set; } = null!;
            public string Slug { get; set; } = null!;
            public bool IsActive { get; set; } = true;
        }

        public class CityUpdateDto : CityCreateDto { }
    
}



