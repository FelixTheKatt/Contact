using Microsoft.Extensions.Logging;
using Contact.Services;
using Microsoft.EntityFrameworkCore;
using Contact.Data;
using Contact.Repositories;

namespace Contact
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            var connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=ContactDb;Trusted_Connection=True;TrustServerCertificate=True;";
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Injection du repo et service
            builder.Services.AddScoped<IContactRepository, ContactRepository>();
            builder.Services.AddScoped<ContactService>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
