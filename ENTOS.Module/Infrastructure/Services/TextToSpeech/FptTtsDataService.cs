using ENTOS.Application.DTOs;
using ENTOS.Application.Features.TextToSpeech;
using ENTOS.Domain.Interfaces;

namespace ENTOS.Infrastructure.Services.TextToSpeech
{
    public class FptTtsDataService : BaseTtsDataService, ITextToSpeechDataService
    {
        public bool CanHandle(DataServiceDto dataServiceDto)
        {
            if (!string.IsNullOrEmpty(dataServiceDto?.Address)
                && dataServiceDto.Address.StartsWith("https://api.fpt.ai/"))
                return true;
            if (string.Equals(dataServiceDto.ServiceCode, this.GetType().Name))
                return true;
            return false;
        }


        public override async Task ParserResultDataAsync(TextToSpeechCommand textToSpeechCommand, HttpContent httpContent, Application.DTOs.DataServiceDto dataServiceDto)
        {
            if (!textToSpeechCommand.IsSuccess)
            {

                try
                {
                    var contentType = httpContent.Headers.ContentType?.MediaType?.ToLower();

                    if (contentType != null && contentType == "application/json")
                    {
                        var responseContent = await httpContent.ReadAsStringAsync();
                        //string responseText = System.Text.Encoding.UTF8.GetString(responseContent);
                        if (!string.IsNullOrEmpty(responseContent))
                        {
                            var json = Newtonsoft.Json.Linq.JObject.Parse(responseContent);
                            //Nếu là json thì xử lý tiếp trường hợp google
                            string resultUrl = (string)json["async"];
                            textToSpeechCommand.ResultContent = await Module.Helpers.HttpHelper.DownloadFileAsync(resultUrl);
                            textToSpeechCommand.IsSuccess = true;
                        }
                    }

                }
                catch (Exception ex)
                {
                    Module.Helpers.LogHelper.Error(nameof(FptTtsDataService), ex);
                    throw;
                }
            }
        }

    }
}
