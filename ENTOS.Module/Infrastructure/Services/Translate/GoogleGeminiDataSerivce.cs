using ENTOS.Application.DTOs;
using ENTOS.Application.Features.TranslateText;
using ENTOS.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ENTOS.Infrastructure.Services.Translate
{


    public class GoogleGeminiDataSerivce : BaseTranslateDataService, ITranslationDataService
    {
        public bool CanHandle(Application.DTOs.DataServiceDto dataServiceDto)
        {
            if (string.IsNullOrEmpty(dataServiceDto?.Address))
                return false;
            if (dataServiceDto.Address.StartsWith("https://generativelanguage.googleapis.com/"))
            {
                Initialize(dataServiceDto);
                _dataServiceService = new Module.Services.DataServiceService();
                ResultKey = "candidates[0].content.parts[0].text";
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
