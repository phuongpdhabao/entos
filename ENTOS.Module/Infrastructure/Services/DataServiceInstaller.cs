
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ENTOS.Module.Interfaces;
//using ENTOS.Application.Features.TextToSpeech;
//using ENTOS.Domain.Interfaces;
//using ENTOS.Infrastructure.Services.TextToSpeech;

namespace ENTOS.Infrastructure.Services
{
    public class DataServiceInstaller : IServiceInstaller

    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            // Register TextToSpeech services
            //services.AddSingleton<ITextToSpeechDataService, OpenAITtsService>();
            //services.AddSingleton<ITextToSpeechDataService, GoogleTtsService>();
            //services.AddSingleton<ITextToSpeechDataService, FptTtsService>();

            //services.AddSingleton<ITextToSpeechServiceFactory, TextToSpeechServiceFactory>();
            //services.AddTransient<TextToSpeechHandler>();
        }
    }
}
