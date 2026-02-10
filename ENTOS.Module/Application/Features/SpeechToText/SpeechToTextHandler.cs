using ENTOS.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTOS.Application.Features.SpeechToText
{
    public class SpeechToTextHandler : ITransientDependency
    {
        private readonly ISpeechToTextServiceFactory _factory;

        public SpeechToTextHandler(ISpeechToTextServiceFactory factory)
        {
            _factory = factory;
        }

        public async Task<byte[]> HandleAsync(SpeechToTextCommand command)
        {
            var service = _factory.GetService(command.Provider);
            return await service.SynthesizeAsync(command.Provider, command.Text, command.Voice, command.Language);
        }
    }

}
