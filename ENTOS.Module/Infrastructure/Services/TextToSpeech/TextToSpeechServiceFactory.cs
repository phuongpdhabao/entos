using ENTOS.Application.Features.TextToSpeech;
using ENTOS.Domain.Interfaces;
using ENTOS.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTOS.SharedKernel.Interfaces;

namespace ENTOS.Infrastructure.Services.TextToSpeech
{
    public class TextToSpeechServiceFactory : ITextToSpeechServiceFactory, ITransientDependency
    {
        private readonly IEnumerable<ITextToSpeechDataService> _services;

        public TextToSpeechServiceFactory(IEnumerable<ITextToSpeechDataService> services)
        {
            _services = services;
        }

        public ITextToSpeechDataService GetService(DataServiceDto provider)
        {
            return _services.FirstOrDefault(s => s.CanHandle(provider))
                ?? throw new NotSupportedException($"Không tìm thấy mã thực hiện của '{provider}'");
        }
    }

}
