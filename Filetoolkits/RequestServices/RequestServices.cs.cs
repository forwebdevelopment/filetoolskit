using Filetoolkits.application.IPersistance;
using Filetoolkits.application.IServices;
using Filetoolkits.infrastructure.Persistance;
using Filetoolkits.infrastructure.Services;

namespace Filetoolkits.RequestServices
{
    public static class RequestServices
    {

        public static IServiceCollection AddServices(this IServiceCollection services) {

            services.AddCors(c =>
            {
                c.AddPolicy("policy", p =>
                {
                    p.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
                });
            });


            services.AddScoped<IPdfFile, PdfFile>();
            services.AddScoped<IPdfServices , FileService>();
            services.AddScoped<ILockUnlockService, LockUnlockService>();
            services.AddScoped<ILockFiles, LockFile>();
            services.AddScoped<IUnlockFiles, UnlockFile>();
            services.AddScoped<IFileConversion, FileConversion>();
            services.AddScoped<IfileConversionService, fileConversionService>();


            return services;
        }

    }
}
