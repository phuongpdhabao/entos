using ENTOS.Domain.Interfaces;
using ENTOS.Application.DTOs;
using ENTOS.Application.Features.TextToSpeech;
using ENTOS.Module.SystemObjects;
using ENTOS.SharedKernel.Interfaces;
using System.Globalization;
using ENTOS.Module.Helpers;


namespace ENTOS.Infrastructure.Services.TextToSpeech
{

    public abstract class BaseTtsDataService : ISingletonDependency
    {
        public async Task TextToSpeechObjectsAsync(DataServiceDto dataServiceDto, IList<TextToSpeechCommand> items, LongTaskContext context, int? currentItem = null, int? totalItem = null)
        {
            var dataServiceService = new Module.Services.DataServiceService();
            //System.Diagnostics.Debug.WriteLine($"TranslateBlockAsync bắt đầu: {client.DefaultRequestHeaders.UserAgent} gửi dữ liệu block: {block.StartIndex}");            
            int maxConcurrency = dataServiceDto.MaxConcurrency ?? 10; // Số lượng đồng thời tối đa, có thể cấu hình trong DataServiceDto
            using var semaphore = new SemaphoreSlim(maxConcurrency, maxConcurrency);
            var tasks = new List<Task>();
            CancellationToken cancellationToken = context?.Control?.CancellationToken ?? default;
            int currentIndex = currentItem ?? 0;
            int total = totalItem ?? items.Count;
            var enCul = new CultureInfo("en");
            foreach (var item in items)
            {
                await semaphore.WaitAsync(cancellationToken);
                try
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    using var client = Module.Helpers.HttpHelper.CreateHttpClient(30, true);
                    var cul = new CultureInfo("en");
                    var responseContent = await dataServiceService.GetResultAsync(client, dataServiceDto, new object[] { item.Text, item.Voice, item.VowelSpeed, item.Language });
                    await ParserResultDataAsync(item, responseContent, dataServiceDto);
                    if (item.IsSuccess)
                    {
                        DetectExtension(item);
                        SaveFileAsync(item);//Nếu ParserResultData trả về true thì sẽ lưu file
                    }
                    if (context?.Progress != null && total > 1)
                    {
                        Interlocked.Increment(ref currentIndex);
                        int percentComplete = context.StepProgressConfig.MapStepProgressPercent(currentIndex, total);
                        //System.Diagnostics.Debug.WriteLine($"TranslateContextAsyncProgress percentComplete: {block.StartIndex} / {percentComplete}");
                        if (context?.Control == null || !context.Control.IsMinimized || percentComplete % 20 == 0)
                        {
                            context.Progress.PercentComplete = percentComplete;

                            context.Progress.ProgressMessage = $"🔄 {context.StepProgressConfig?.CurrentStepName} {currentIndex + 1}/{total} - {percentComplete}%";
                        }

                    }
                }
                finally
                {
                    semaphore.Release();
                }
            }
            await Task.WhenAll(tasks);
            System.Diagnostics.Debug.WriteLine($"TranslateContextAsync xong kết quả: {items.Count}");
        }


        public virtual async Task ParserResultDataAsync(TextToSpeechCommand textToSpeechCommand, HttpContent httpContent, Application.DTOs.DataServiceDto dataServiceDto)
        {
            //Chờ mỗi loại tts xử lý
            await Task.CompletedTask; // đ
        }
        protected void DetectExtension(TextToSpeechCommand textToSpeechCommand)
        {
            var fileExtension = FileFormatHelper.DetectExtension(textToSpeechCommand.ResultContent);
            if (!string.IsNullOrEmpty(fileExtension))
                textToSpeechCommand.ResultUrl += $".{fileExtension}";
        }

        protected void SaveFileAsync(TextToSpeechCommand textToSpeechCommand)
        {
            if (!string.IsNullOrEmpty(textToSpeechCommand.ResultUrl) && textToSpeechCommand.ResultContent != null)
                Module.Helpers.FileSystemHelper.WriteAllBytes(textToSpeechCommand.ResultUrl, textToSpeechCommand.ResultContent);
        }
    }
}
