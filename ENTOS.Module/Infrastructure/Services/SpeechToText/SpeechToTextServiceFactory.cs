using ENTOS.Domain.Interfaces;
using ENTOS.Application.DTOs;
using ENTOS.SharedKernel.Interfaces;
using ENTOS.Application.Features.SpeechToText;

namespace ENTOS.Infrastructure.Services.SpeechToText
{
    public class SpeechToTextServiceFactory : ISpeechToTextServiceFactory, ITransientDependency
    {
        private readonly IEnumerable<ISpeechToTextDataService> _services;

        public SpeechToTextServiceFactory(IEnumerable<ISpeechToTextDataService> services)
        {
            _services = services;
        }

        public ISpeechToTextDataService GetService(DataServiceDto provider)
        {
            return _services.FirstOrDefault(s => s.CanHandle(provider))
                ?? throw new NotSupportedException($"Không tìm thấy mã thực hiện của '{provider}'");
        }
    }

}
