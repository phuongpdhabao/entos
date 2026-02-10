using AngleSharp.Html;
using ENTOS.Application.Features.TranslateText;
using ENTOS.Domain.Interfaces;
using Microsoft.CodeAnalysis.Text;
using System.Text;
using System.Text.Json;

namespace ENTOS.Infrastructure.Services.Translate
{



    public class OpenAITranslateDataService : BaseTranslateDataService, ITranslationDataService
    {
        public bool CanHandle(Application.DTOs.DataServiceDto dataServiceDto)
        {
            if (string.IsNullOrEmpty(dataServiceDto?.Address))
                return false;
            if (dataServiceDto.Address.StartsWith("https://api.openai.com"))
            {
                Initialize(dataServiceDto);
                _dataServiceService = new Module.Services.DataServiceService();
                ResultKey = "choices[0].message.content";
                return true;
            }
            return false;
        }



        //public override async Task ParseTranslateResponse(TranslateSegmentBase segment, HttpContent httpContent)
        //{
        //   base.ParseTranslateResponse(segment, httpContent);            
        //}

        //public override async Task ParseTranslateBlockResponse(TranslateSegmentBlock block, HttpContent httpContent, string startWrapper = "<li>", string endWrapper = "</li>\n")
        //{

        //    base.ParseTranslateBlockResponse(block, httpContent, startWrapper, endWrapper);
        //}
    }


}
