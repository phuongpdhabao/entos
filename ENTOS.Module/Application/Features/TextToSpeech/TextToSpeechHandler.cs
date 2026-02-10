using ENTOS.SharedKernel.Interfaces;

namespace ENTOS.Application.Features.TextToSpeech
{
    public class TextToSpeechHandler : ITransientDependency
    {
        private readonly ITextToSpeechServiceFactory _factory;

        public TextToSpeechHandler(ITextToSpeechServiceFactory factory)
        {
            _factory = factory;
        }

        public async Task TextToSpeechObjectsAsync(IList<Application.Features.TextToSpeech.TextToSpeechCommand> items, Module.SystemObjects.LongTaskContext context = null)
        {
            var ttsListGroup = items.GroupBy(x => x.Provider);
            int currentItem = 0;
            int totalItem = items.Count;
            foreach (var group in ttsListGroup)
            {
                var dataServiceDto = group.Key;
                if (dataServiceDto == null)
                {
                    throw new ArgumentNullException(nameof(dataServiceDto), "DataServiceDto cannot be null");
                }
                var service = _factory.GetService(dataServiceDto);
                await service.TextToSpeechObjectsAsync(dataServiceDto, items, context, currentItem, totalItem);
                currentItem += group.Count();
            }

        }
    }

}
