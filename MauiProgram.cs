using ipr1.Pages;
using ipr1.Services;
using Microsoft.Extensions.Logging;

namespace ipr1;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>();

        builder.Services.AddHttpClient<IRateService, RateService>(opt =>
            opt.BaseAddress = new Uri("https://api.nbrb.by/exrates/rates"));

        builder.Services.AddTransient<IntegralPage>();
        builder.Services.AddTransient<CurrencyConverterPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
