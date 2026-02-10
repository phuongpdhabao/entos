using ENTOS.Domain.Interfaces;
using ENTOS.Application.DTOs;
using ENTOS.SharedKernel.Interfaces;
using ENTOS.Application.Features.SpeechToText;
using ENTOS.Application.Features.TranslateText;

namespace ENTOS.Infrastructure.Services.SpeechToText
{
    public class TranslateDataServiceFactory : ITranslationServiceFactory, ITransientDependency
    {
        private readonly IEnumerable<ITranslationDataService> _services;

        public TranslateDataServiceFactory(IEnumerable<ITranslationDataService> services)
        {
            _services = services;
        }

        public ITranslationDataService GetService(DataServiceDto provider)
        {
            return _services.FirstOrDefault(s => s.CanHandle(provider))
                ?? throw new NotSupportedException($"Không tìm thấy mã thực hiện của '{provider}'");
        }

    }

}
