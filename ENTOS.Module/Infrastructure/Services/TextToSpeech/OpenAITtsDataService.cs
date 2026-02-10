using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTOS.Domain.Interfaces;
using ENTOS.Application.DTOs;
using ENTOS.Application.Features.TextToSpeech;
using ENTOS.SharedKernel.Interfaces;

namespace ENTOS.Infrastructure.Services.TextToSpeech
{
    public class OpenAITtsDataService : BaseTtsDataService, ITextToSpeechDataService
    {
        public bool CanHandle(DataServiceDto dataServiceDto)
        {
            if (!string.IsNullOrEmpty(dataServiceDto?.Address)
                && dataServiceDto.Address.StartsWith("https://api.openai.com/"))
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
                    if (contentType != null)
                    {
                        textToSpeechCommand.ResultContent = await httpContent.ReadAsByteArrayAsync();
                        if (textToSpeechCommand.ResultContent != null)
                            textToSpeechCommand.IsSuccess = true;
                    }
                }
                catch (Exception ex)
                {
                    Module.Helpers.LogHelper.Error(nameof(OpenAITtsDataService), ex);
                    throw;
                }

            }
        }
    }
}
