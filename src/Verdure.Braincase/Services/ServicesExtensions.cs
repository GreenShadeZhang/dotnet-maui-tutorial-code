using System;
using Verdure.Braincase.Services.ClockView;

namespace Verdure.Braincase.Services
{
    public static class ServicesExtensions
    {
        public static MauiAppBuilder ConfigureServices(this MauiAppBuilder builder)
        {
            builder.Services.AddTransient<IClockViewProvider, DefaultClockViewProvider>();
            builder.Services.AddTransient<IClockViewProvider, SmoothClockViewProvider>();
            builder.Services.AddTransient<IClockViewProviderFactory, ClockViewProviderFactory>();

            return builder;
        }
    }
}

