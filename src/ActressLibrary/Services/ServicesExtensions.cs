using ActressLibrary.Infrastructure.Repository;
using ActressLibrary.Interfaces;

namespace ActressLibrary.Services
{
    public static class ServicesExtensions
    {
        public static MauiAppBuilder ConfigureServices(this MauiAppBuilder builder, string path)
        {
            builder.Services.AddSingleton<IPersonalInfoRepository, PersonalInfoRepository>(_ =>
            new PersonalInfoRepository(path));

            return builder;
        }
    }
}