
using ENTOS.Application.DTOs;
using ENTOS.Infrastructure.Services.Translate;
using ENTOS.Module.Interfaces;
using ENTOS.SharedKernel.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ENTOS.Application.Features.TranslateText
{
    public class TranslateTextHandler : ITransientDependency
    {
        private readonly ITranslationServiceFactory _factory;

        public TranslateTextHandler(ITranslationServiceFactory factory)
        {
            _factory = factory;
        }
        public TranslateTextHandler()
        {
            //Không dùng DI
        }
        public async Task<string> TranslateAsync(DataServiceDto dataServiceDto, string text, string targetLanguage, string sourceLanguage = null, string prompt = null)
        {
            if (_factory is null)
                throw new ArgumentNullException(nameof(_factory), "Translation service factory cannot be null");
            var service = _factory.GetService(dataServiceDto);
            return await service.TranslateAsync(text, targetLanguage, sourceLanguage, prompt);
        }
        public async Task<string[]> TranslateAsync(DataServiceDto dataServiceDto, string[] texts, string targetLanguage, string sourceLanguage = null, string prompt = null)
        {
            if (_factory is null)
                throw new ArgumentNullException(nameof(_factory), "Translation service factory cannot be null");
            var service = _factory.GetService(dataServiceDto);
            return await service.TranslateAsync(texts, targetLanguage, sourceLanguage, prompt);
        }
        public async Task<string> DetectLanguageAsync(DataServiceDto dataServiceDto, string text)
        {
            if (_factory is null)
                throw new ArgumentNullException(nameof(_factory), "Translation service factory cannot be null");
            var service = _factory.GetService(dataServiceDto);
            return await service.DetectLanguageAsync(text);
        }
        public async Task TranslateContextAsync(DataServiceDto dataServiceDto, List<TranslateSegmentBase> segments,
            string targetLanguage,
            string sourceLanguage = null,
            int maxBatchLength = 4000,
            string tagName = "␟␟␟",
            int maxConcurrency = 10,
            Module.SystemObjects.LongTaskContext context = null)
        {
            var service = _factory.GetService(dataServiceDto);
            bool urlEncode = dataServiceDto.ApiMethodType == Module.BusinessObjects.ApiMethodType.Get;
            await service.TranslateContextAsync(segments, targetLanguage, sourceLanguage, maxBatchLength, tagName, maxConcurrency, urlEncode, context);
        }

        public async Task<List<TranslateSegmentBase>> TranslateObjects<T>(
               AutoMapper.IMapper mapper,
               Module.BusinessObjects.DataService defaultDataService,
               IList<T> items,
               Func<object, string> contentSelector,
               string targetLanguage,
               string sourceLanguage,
               bool translateContextAsync = true,
               Module.SystemObjects.LongTaskContext context = null)
        {
            var dataServiceDto = mapper.Map<Application.DTOs.DataServiceDto>(defaultDataService);
            //var dataServiceService = new Module.Services.DataServiceService();
            //var translateService = new BaseTranslateDataService(dataServiceDto, dataServiceService);
            var translateService = _factory.GetService(dataServiceDto);
            //translateService.In
            var translateSegments = items
                .Select(x => new TranslateSegmentBase
                {
                    OriginalText = contentSelector(x)
                })
                .ToList();
            if (translateContextAsync)
            {
                await translateService.TranslateContextAsync(
                    translateSegments,
                    targetLanguage,
                    sourceLanguage,
                    3000,
                    maxConcurrency: dataServiceDto.MaxConcurrency ?? 10,
                    context: context
                );
            }
            else
            {
                await translateService.TranslateAsync(
                    translateSegments,
                    targetLanguage,
                    sourceLanguage,
                    maxConcurrency: dataServiceDto.MaxConcurrency ?? 10,
                    context: context
                );
            }


            return translateSegments;
        }


        public void TranslateObjects<T>(DevExpress.ExpressApp.ViewController viewController,
                AutoMapper.IMapper mapper,
                IList<T> items,
                string sourcePropertyName,
                string targetPropertyName,
                string targetLanguage,
                string sourceLanguage,
                string caption)
        {
            if (string.IsNullOrEmpty(targetLanguage))
                throw new ArgumentNullException(nameof(targetLanguage), "Ngữ dịch bị trống");
            var dataServiceService = new Module.Services.DataServiceService();
            var defaultDataService = dataServiceService.GetDataService(viewController, "Translate");
            if (defaultDataService != null)
            {
                //var audioList = View.SelectedObjects.Cast<Module.BusinessObjects.Audio>().Where(x => !string.IsNullOrEmpty(x.Content) && x.Content.Length > 1).ToList();
                var translateList = items.Where(x => x != null && Module.Helpers.ReflectionHelper.GetPropertyValue(x, sourcePropertyName) is string content && content.Length > 1).ToList();
                var longTaskService = viewController.Application.ServiceProvider.GetRequiredService<ILongTaskService>();
                if (longTaskService != null)
                {
                    var uiContext = SynchronizationContext.Current;
                    bool translateContextAsync = Module.Helpers.ParameterHelper.GetBooleanOrDefault(defaultDataService.Session, "TranslateContextAsync", true);
                    // Fire-and-forget task với proper error handling
                    Task.Run(async () =>
                    {
                        var progressConfig = new Module.SystemObjects.StepProgressConfig
                        {
                            Steps = new List<Module.SystemObjects.StepInfo>
                            {
                                new Module.SystemObjects.StepInfo { Name = "Bước 1/2: Đang dịch qua API: ", Weight = 0.4 },
                                new Module.SystemObjects.StepInfo { Name = "Bước 2/2: Đang gán dữ liệu: ", Weight = 0.6 },
                            }
                        };
                        //var translateSegments = await audioWinService.TranslateElement_TranslateDataService(mapper, defaultDataService, progressConfig, audioList, video.LanguageTranslate.Code, video.LanguageOrigin.Code);
                        await longTaskService.ExecuteTaskAsync(
                            caption,
                            async (progress, control) =>
                            {

                                //Bước 1
                                var context = new Module.SystemObjects.LongTaskContext(progress, control, progressConfig, uiContext);
                                var translateSegments = await TranslateObjects(
                                    mapper,
                                    defaultDataService,
                                    translateList,
                                    Module.Helpers.ReflectionHelper.BuildStringPropertyAccessor(sourcePropertyName),
                                    targetLanguage,
                                    sourceLanguage,
                                    translateContextAsync,
                                    context
                                );
                                System.Diagnostics.Debug.WriteLine($"TranslateDataService chuyển bước tiếp theo");
                                progressConfig.CurrentStepIndex++;
                                //Bước 1

                                await Module.Helpers.ListHelper.CopyPropertyWithProgress(
                                        context,
                                        translateSegments,
                                        translateList,
                                        segment => segment.TranslatedText,
                                        Module.Helpers.ReflectionHelper.BuildPropertySelector<T, string>(targetPropertyName)
                                        );
                            },
                            canCancel: true,
                            canMinimize: true
                        );

                    });
                }


            }
        }


    }
}
