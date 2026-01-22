using Cinema.Infrastructure.Data;
using Cinema.Infrastructure.Repositories;
using Cinema.WinForms.Forms;
using Cinema.WinForms.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Cinema.WinForms
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            // Dependency Injection
            var serviceProvider = DependencyInjection.ConfigureServices();

            // Запуск главной формы
            var mainForm = serviceProvider.GetRequiredService<MainForm>();
            Application.Run(mainForm);
        }

        /// <summary>
        /// Dependency Injection (DI, Внедрение зависимостей) — это шаблон проектирования, 
        /// при котором объекты получают свои зависимости извне, а не создают их самостоятельно.
        /// </summary>
        public static class DependencyInjection
        {
            public static IServiceProvider ConfigureServices()
            {
                var services = new ServiceCollection();
                var configuration = CreateConfiguration();

                // Регистрация DbContext
                services.AddDbContext<CinemaDbContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

                // Unit of Work
                services.AddScoped<IUnitOfWork, UnitOfWork>();

                // Репозитории 
                services.AddScoped<IMovieRepository, MovieRepository>();
                services.AddScoped<ISessionRepository, SessionRepository>();
                services.AddScoped<ITicketRepository, TicketRepository>();

                // Сервисы
                services.AddScoped<ITicketService, TicketService>();

                // Регистрация форм
                services.AddTransient<MainForm>();
                services.AddTransient<CinemaBookingForm>();

                return services.BuildServiceProvider();
            }

            private static IConfiguration CreateConfiguration()
            {
                var confBuilder = new ConfigurationBuilder();
                confBuilder.AddJsonFile("appsettings.json", false);
                return confBuilder.Build();
            }
        }
    }
}