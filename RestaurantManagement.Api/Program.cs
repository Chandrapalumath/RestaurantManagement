
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Api.Middlewares;
using RestaurantManagement.Backend.Services;
using RestaurantManagement.Backend.Services.Interfaces;
using RestaurantManagement.DataAccess;
using RestaurantManagement.DataAccess.Repositories;
using RestaurantManagement.DataAccess.Repositories.Interfaces;

namespace RestaurantManagement.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContextPool<RestaurantDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("RestaurantDBConnection"))
            );
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
            builder.Services.AddScoped<IMenuRepository, MenuRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IBillingRepository, BillingRepository>();
            builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
            builder.Services.AddScoped<ISettingsRepository, SettingsRepository>();

            
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IMenuService, MenuService>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IChefService, ChefService>();
            builder.Services.AddScoped<IBillingService, BillingService>();
            builder.Services.AddScoped<IReviewService, ReviewService>();
            builder.Services.AddScoped<ISettingsService, SettingsService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.MapControllers();

            app.Run();
        }
    }
}
