using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TheSeatLineApi.Modules.AuthModule.Services;
using TheSeatLineApi.Modules.AuthModule.Helpers;
using TheSeatLineApi.Modules.AuthModule.Repositories;
using TheSeatLineApi.Infrastructure.Persistence;
using TheSeatLineApi.Modules.MasterModule.Services;
using TheSeatLineApi.Modules.MasterModule.Repositories;
using TheSeatLineApi.Modules.BookingModule.Services;
using TheSeatLineApi.Modules.BookingModule.Repositories;
using TheSeatLineApi.Shared.Middleware;

namespace TheSeatLineApi
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngular", policy =>
                {
                    policy.WithOrigins("http://localhost:4200")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")
                )
            );

            builder.Services.AddAuthentication("Bearer")
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(
                                builder.Configuration["Jwt:Key"]!
                            )
                        )
                    };
                });

            // Auth Services
            builder.Services.AddScoped<IUserRepository, UserBusiness>();
            builder.Services.AddScoped<IAuthService, AuthBusiness>();
            builder.Services.AddSingleton<JwtTokenGenerator>();
            

            // Master Services
            builder.Services.AddScoped<ICityRepository, CityBusiness>();
            builder.Services.AddScoped<IVenueRepository, VenueBusiness>();
            builder.Services.AddScoped<IEventRepository, EventBusiness>();
            builder.Services.AddScoped<IEventShowRepository, EventShowBusiness>();
            builder.Services.AddScoped<ISeatRepository, SeatBusiness>();

            // Booking Services
            builder.Services.AddScoped<IBookingRepository, BookingBusiness>();
            builder.Services.AddScoped<IPaymentRepository, PaymentBusiness>();
            builder.Services.AddScoped<ICouponRepository, CouponBusiness>();

            // Review Service
            builder.Services.AddScoped<IReviewRepository, ReviewBusiness>();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Enter: {your JWT token}"
                });

                options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });


            var app = builder.Build();

            app.UseCors("AllowAngular");
            app.UseMiddleware<ExceptionMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}














