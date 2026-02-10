using ENTOS.Application.Features.TranslateText;
using ENTOS.Domain.Interfaces;
using System.Text;
using System.Text.Json;

namespace ENTOS.Infrastructure.Services.Translate
{

    public class GoogleTranslateDataSerivce : BaseTranslateDataService, ITranslationDataService
    {
        public bool CanHandle(Application.DTOs.DataServiceDto dataServiceDto)
        {
            if (string.IsNullOrEmpty(dataServiceDto?.Address))
                return false;
            if (dataServiceDto.Address.StartsWith("https://translate.googleapis.com/translate_a"))
            {
                Initialize(dataServiceDto);
                _dataServiceService = new Module.Services.DataServiceService();
                return true;
            }
            return false;
        }
        public override void ParseTranslateInput(object[] inputs)
        {
            if (inputs.Length >= 3 && inputs[2] is null)
                inputs[2] = "auto";
        }


        public override async Task ParseTranslateResponse(TranslateSegmentBase segment, HttpContent httpContent)
        {
            try
            {
                var json = await httpContent.ReadAsStringAsync();
                var arr = JsonDocument.Parse(json).RootElement;
                if (arr.GetArrayLength() > 0 && arr[0].ValueKind == JsonValueKind.Array)
                {
                    var translatedBuilder = new StringBuilder();
                    var originalBuilder = new StringBuilder();

                    foreach (var sentence in arr[0].EnumerateArray())
                    {
                        if (sentence.GetArrayLength() >= 2)
                        {
                            translatedBuilder.Append(sentence[0].GetString());
                            originalBuilder.Append(sentence[1].GetString());
                        }
                    }

                    segment.TranslatedText = translatedBuilder.ToString();
                    segment.OriginalText = originalBuilder.ToString();
                }
            }
            catch (Exception ex)
            {
                //segment.TranslatedText = $"[Lỗi dịch Google: {ex.Message}]";
            }
        }

        public override async Task ParseTranslateBlockResponse(TranslateSegmentBlock block, HttpContent httpContent, string startWrapper = "<li>", string endWrapper = "</li>\n")
        {
            try
            {
                var json = await httpContent.ReadAsStringAsync();
                var arr = JsonDocument.Parse(json).RootElement;
                int segmentIndex = 0, totalResults = 0;
                // string startWrapper = "<li>"; // bỏ khai báo cục bộ
                //if(arr[0].EnumerateArray().Count() == 1)
                //{

                //}
                var total = arr[0].EnumerateArray();
                foreach (var item in arr[0].EnumerateArray())
                {
                    try
                    {
                        var itemLength = item.GetArrayLength();
                        if (item.GetArrayLength() > 0)
                        {
                            var resultText = item[0].GetString();
                            if (string.IsNullOrEmpty(resultText))
                                continue;
                            var sourceText = item[1].GetString();
                            if (segmentIndex >= block.TranslateSegments.Count)
                                break;
                            //Kiểm tra dữ liệu có được tách không
                            if (block.TranslateSegments.Count > 1)
                            {
                                var sourceTextArray = sourceText.Split(new[] { startWrapper, endWrapper }, StringSplitOptions.RemoveEmptyEntries);
                                if (sourceTextArray.Length > 1)
                                {
                                    var resultArray = resultText.Split(new[] { startWrapper, endWrapper }, StringSplitOptions.RemoveEmptyEntries);
                                    for (int i = 0; i < resultArray.Length && i < sourceTextArray.Length; i++)
                                    {
                                        if (segmentIndex >= block.TranslateSegments.Count)
                                            break;
                                        if (resultArray[i].EndsWith('\n') && !sourceTextArray[i].EndsWith('\n'))
                                            resultArray[i] = resultArray[i].Substring(0, resultArray[i].Length - 1); // Xóa ký tự xuống dòng cuối cùng nếu có
                                        block.TranslateSegments[segmentIndex].TranslatedText = resultArray[i];
                                        segmentIndex++;
                                    }
                                    continue;
                                }

                            }
                            var translateText = Module.Helpers.TextHelper.RemoveXmlNode(resultText);
                            if (!string.IsNullOrEmpty(translateText))
                            {
                                if (segmentIndex > 0 && !sourceText.Contains(startWrapper))
                                    block.TranslateSegments[segmentIndex - 1].TranslatedText += translateText;
                                else
                                {
                                    block.TranslateSegments[segmentIndex].TranslatedText = translateText;
                                    segmentIndex++;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Có thể log lỗi từng dòng nếu cần
                    }
                    totalResults++;
                }
            }
            catch (Exception ex)
            {
                foreach (var seg in block.TranslateSegments)
                {
                    //seg.TranslatedText = $"[Lỗi dịch Google Block: {ex.Message}]";
                }
            }
        }
    }


}
