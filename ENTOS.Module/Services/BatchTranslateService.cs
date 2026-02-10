using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Validation;
using DevExpress.Utils;
using DevExpress.Utils.Drawing;
using DevExpress.Xpo;
using System.Linq;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using ENTOS.Module;
using ENTOS.Module.BusinessObjects;
using ENTOS.Module.SystemObjects;
using ENTOS.Module.Helpers;
using ENTOS.Module.Extensions;
using ENTOS.Module.SystemServices;
using ENTOS.Module.Services;
using System;
using System.Text;
using System.Text.RegularExpressions;

 
namespace ENTOS.Module.Services 
{

    public partial class BatchTranslateService : BaseService
    {

        public BatchTranslateService() : base()
        {
        }
        #region DependencyInjection
  
  
        #endregion DependencyInjection

        public BatchTranslateService(ViewController viewController) : base(viewController)
        {

        }
        //public event EventHandler<string>? OnError;
        //public event EventHandler<string>? OnSuccess;
        //public event EventHandler<string>? OnResponse; 
        
        #region SourceCode3342ImportCode
                        public string BuildTranslateClipboardText(ElementBatch elementBatch, string targetLanguage, string symbol = null)
        {
            if (elementBatch?.Video == null)
                return string.Empty;

            var video = elementBatch.Video;
            var originLanguage = video.LanguageOrigin?.Name ?? "";
            var prompt = video.Note ?? "";
            var content = string.Empty;

            foreach (Audio audio in elementBatch.AudioList.OrderBy(x => x.Start))
            {
                if (audio.Content != null && audio.Content.Trim().Length > 0 && symbol != null)
                {
                    content += audio.Content.Trim() + symbol + "\n";
                }
                else if (audio.Content != null && audio.Content.Trim().Length > 0)
                {
                    content += audio.Content.Trim() + "\n";
                }
            }

            if (!string.IsNullOrWhiteSpace(originLanguage) && !string.IsNullOrWhiteSpace(targetLanguage))
            {
                var regex = new Regex(@"tiếng\s+((?:[A-ZÀ-Ỵ][\p{L}\-]*\s*){1,3})", RegexOptions.Multiline);
                prompt = regex.Replace(prompt, match =>
                {
                    var foundLang = match.Value.Trim();
                    return foundLang.Equals($"tiếng {originLanguage}", StringComparison.OrdinalIgnoreCase)
                        ? match.Value
                        : $"tiếng {targetLanguage} ";
                });
            }

            var sb = new StringBuilder();
            sb.AppendLine(prompt);
            sb.AppendLine(content);
            sb.AppendLine();

            return sb.ToString().Trim();
        }

        #endregion SourceCode3342ImportCode

        #region SourceCode3341ImportCode
                        public string BuildReverseTranslatePrompt(string content, string originLanguage, string targetLanguage)
        {
            originLanguage ??= "ngôn ngữ gốc";
            targetLanguage ??= "ngôn ngữ đích";

            var markedLines = content
                .Split('\n')
                .Select(line => string.IsNullOrWhiteSpace(line) ? line : line.TrimEnd());

            var markedContent = string.Join("\n", markedLines);

            var sb = new StringBuilder();
            sb.Append("Hãy dịch nội dung sau đây từ ");
            sb.Append(targetLanguage);
            sb.Append(" sang ");
            sb.Append(originLanguage);
            sb.AppendLine();
            sb.Append(markedContent);

            return sb.ToString();
        }

        #endregion SourceCode3341ImportCode

        #region SourceCode3295ImportCode
                        public static int CreateBatchTranslate(ElementBatch elementBatch)
        {
            var video = elementBatch.Video;
            var translateBatchList = elementBatch.BatchTranslateList;
            int cnt = 0;

            if (video?.LanguageList != null && video.LanguageList.Count > 0)
            {
                foreach (var language in video.LanguageList)
                {
                    if (translateBatchList.Count > 0)
                    {
                        bool skip = false;
                        foreach (var translateBatch in translateBatchList)
                        {
                            if (language == translateBatch.Language)
                            {
                                skip = true;
                                break;
                            }
                        }
                        if (skip == true)
                        {
                            cnt++;
                            continue;
                        }
                    }
                    var batchTranslate = new BatchTranslate(elementBatch.Session);
                    batchTranslate.ElementBatch = elementBatch;
                    batchTranslate.Language = language;
                }
            }
            return cnt;
        }


        #endregion SourceCode3295ImportCode

        #region SourceCode3386ImportCode
                public static List<(string, double)> CalculateInferScores(string[] words, string subtitle, string content, string translated,bool reverse, DataService dataService,Video video, Module.Services.DataServiceService dataServiceService)
        {
            var scores = new List<(string, double)>();
            string built = "";
            string subtitleRemaining = subtitle;
            string contentTranslated = Tools.TranslateText(content, video.LanguageTranslate.Code, video.LanguageOrigin.Code);

            var wordList = reverse ? words.Reverse() : words;

            foreach (var word in wordList)
            {
                built = reverse ? word + " " + built : built + word + " ";
                subtitleRemaining = reverse
                    ? subtitleRemaining.Remove(Math.Max(0, subtitleRemaining.Length - word.Length - 1))
                    : subtitleRemaining.Remove(0, Math.Min(subtitleRemaining.Length, word.Length + 1));

                string builtClean = built.Trim();
                string subtitleClean = subtitleRemaining.Trim();

                if (!(!reverse && EndsWithStrongPunctuation(builtClean) ||
                      reverse && EndsWithStrongPunctuation(subtitleClean) ||
                      !reverse && EndsWithSoftPunctuation(builtClean) ||
                      reverse && EndsWithSoftPunctuation(subtitleClean)))
                {
                    continue;
                }

                double sim1 = Task.Run(() => dataServiceService.GetSentenceSimilarityAsync(dataService, builtClean, translated)).Result;
                double sim2 = 0;
                if (!string.IsNullOrEmpty(subtitleClean))
                    sim2 = Task.Run(() => dataServiceService.GetSentenceSimilarityAsync(dataService, subtitleClean, contentTranslated)).Result;

                double sim12 = Module.Helpers.TextHelper.CalculateWordSimilarity(builtClean, translated);
                double sim22 = Module.Helpers.TextHelper.CalculateWordSimilarity(subtitleClean, contentTranslated);

                double score = (0.7*sim1 + 0.7*sim2 + 0.3*sim12 + 0.3*sim22) / 2;

                if ((!reverse && EndsWithStrongPunctuation(builtClean)) ||
                    (reverse && EndsWithStrongPunctuation(subtitleClean)))
                {
                    score += 0.1;
                }
                if ((!reverse && EndsWithSoftPunctuation(builtClean)) ||
                    (reverse && EndsWithSoftPunctuation(subtitleClean)))
                {
                    score += 0.05;
                }

                scores.Add((builtClean, score));
            }

            return scores;
        }



        #endregion SourceCode3386ImportCode

        #region SourceCode3391ImportCode
                                public static DataServiceService dataServiceService = new DataServiceService();
        public static string FillBlankLines(
          string templateText,
          string inputText,
          DataService dataService,
          Video video,
          Language templateLanguage,
          Language originLanguage,
          double threshold = 0.5)
        {
            var templateLines = templateText.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var inputLines = inputText.Split(new[] { '\r', '\n' }, StringSplitOptions.None); // giữ dòng trống

            var resultLines = new List<string>(inputLines);

            for (int i = 0; i < inputLines.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(inputLines[i]))
                    continue;

                // Tìm dòng kế bên có nội dung (trái và phải)
                int prevIndex = i - 1;
                int nextIndex = i + 1;

                string prevLine = (prevIndex >= 0) ? inputLines[prevIndex] : null;
                string nextLine = (nextIndex < inputLines.Length) ? inputLines[nextIndex] : null;

                string prevTemplate = (prevIndex >= 0 && prevIndex < templateLines.Length) ? templateLines[prevIndex] : null;
                string nextTemplate = (nextIndex >= 0 && nextIndex < templateLines.Length) ? templateLines[nextIndex] : null;

                string inferredFromPrev = null, inferredFromNext = null;
                bool flag1 = false, flag2 = false;

                if (!string.IsNullOrWhiteSpace(prevLine) && !string.IsNullOrWhiteSpace(prevTemplate))
                {
                    inferredFromPrev = InferSubtitle(
                        prevLine, prevTemplate,
                        templateLines[i],
                        true,
                        dataService, video, dataServiceService,
                        threshold,
                        out flag1
                    );
                }

                if (!string.IsNullOrWhiteSpace(nextLine) && !string.IsNullOrWhiteSpace(nextTemplate))
                {
                    inferredFromNext = InferSubtitle(
                        nextLine, nextTemplate,
                        templateLines[i],
                        false,
                        dataService, video, dataServiceService,
                        threshold,
                        out flag2
                    );
                }

                // Ưu tiên dòng có điểm cao hơn
                if (!string.IsNullOrEmpty(inferredFromPrev) && (string.IsNullOrEmpty(inferredFromNext) || flag1 || (!flag2 && flag1)))
                {
                    resultLines[i] = inferredFromPrev;
                    resultLines[i - 1] = resultLines[i-1].Replace(inferredFromPrev, "").Trim();
                }
                else if (!string.IsNullOrEmpty(inferredFromNext))
                {
                    resultLines[i] = inferredFromNext;
                    resultLines[i + 1] = resultLines[i+1].Replace(inferredFromNext, "").Trim();
                }
            }

            return FillBlankText(templateLines, resultLines, templateLanguage, originLanguage).Trim();
        }

        public static string FillBlankText(
          string[] templateText,
          List<string> inputText,
            Language templateLanguage,
            Language originLanguage)
        {
            for (int i = 0; i < inputText.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(inputText[i]) && templateLanguage == originLanguage)
                {
                    inputText[i] = templateText[i] + "**";
                }
                else if (string.IsNullOrWhiteSpace(inputText[i]) && templateLanguage != originLanguage)
                {
                    inputText[i] = Tools.TranslateText(templateText[i], originLanguage.Code, templateLanguage.Code) + "**";
                }
            }
            return string.Join("\n", inputText).Trim();
        }



        #endregion SourceCode3391ImportCode

        #region SourceCode3346ImportCode
                public int? FindPunctuationLine(string content, string translate, string languageCode = "en", int startLineIndex = 0)
{
    var languagesWithoutSpaceAfterPunctuation = new HashSet<string> {
        "ja", // Japanese
        "zh", // Chinese
        "ko", // Korean
        "th", // Thai
        "my", // Burmese
        "km", // Khmer
        "lo"  // Lao
    };

    var contentLines = content.Replace("\r", "").Split('\n');
    var translateLines = translate.Replace("\r", "").Split('\n');
    int maxLineCount = Math.Max(contentLines.Length, translateLines.Length);

    bool requireSpace = !languagesWithoutSpaceAfterPunctuation.Contains(languageCode);
    System.Text.RegularExpressions.Regex punctuationRegex;

    if (requireSpace)
    {
        punctuationRegex = new System.Text.RegularExpressions.Regex(@"[.!?]\s", RegexOptions.Compiled);
    }
    else
    {
        punctuationRegex = new System.Text.RegularExpressions.Regex(@"[.!?]", RegexOptions.Compiled);
    }

    for (int i = startLineIndex; i < maxLineCount; i++)
    {
        string lineA = i < contentLines.Length ? contentLines[i] : "";
        string lineB = i < translateLines.Length ? translateLines[i] : "";

        bool hasPunctA = IsPunctuationInMiddle(lineA, punctuationRegex);
        bool hasPunctB = IsPunctuationInMiddle(lineB, punctuationRegex);

        // Chỉ trả về khi chỉ 1 trong 2 dòng có ngắt câu giữa dòng
        if (hasPunctA ^ hasPunctB) // XOR
        {
            return i;
        }
    }

    return null;

    bool IsPunctuationInMiddle(string line, System.Text.RegularExpressions.Regex regex)
    {
        if (string.IsNullOrWhiteSpace(line) || line.Length < 3)
            return false;

        var matches = regex.Matches(line);
        foreach (System.Text.RegularExpressions.Match match in matches)
        {
            int index = match.Index;

            // Dấu ngắt phải nằm không ở cuối dòng
            if (index > 0 && index < line.Length - 2)
                return true;
        }

        return false;
    }
}

        #endregion SourceCode3346ImportCode

        #region SourceCode3343ImportCode
                public int? FindFirstDifferentLine(string content, string translate, int startLineIndex = 0)
{
    var contentLines = content.Replace("\r", "").Split('\n');
    var translateLines = translate.Replace("\r", "").Split('\n');
    int min = Math.Min(contentLines.Length, translateLines.Length);

    // Bắt đầu từ dòng startLineIndex
    for (int i = Math.Max(1, startLineIndex); i < min - 1; i++)
    {
        int cPrev = contentLines[i - 1].Length;
        int cCurr = contentLines[i].Length;
        int cNext = contentLines[i + 1].Length;

        int tPrev = translateLines[i - 1].Length;
        int tCurr = translateLines[i].Length;
        int tNext = translateLines[i + 1].Length;

        if (Math.Sign(cCurr - cPrev) != Math.Sign(tCurr - tPrev) ||
            Math.Sign(cNext - cCurr) != Math.Sign(tNext - tCurr))
        {
            return i;
        }
    }

    return null;
}

        #endregion SourceCode3343ImportCode

        #region SourceCode3303ImportCode
                        public static void CreateElementTranslate(BatchTranslate batch)
        {
            var elementBatch = batch.ElementBatch;

            List<Audio> audioList = elementBatch?.AudioList.ToList();
            if (audioList == null || audioList.Count == 0)
                return;

            audioList.Sort((a, b) => a.Start.Value.CompareTo(b.Start.Value));

            // Tách từng dòng dịch
            var lines = batch.Content.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            int count = Math.Min(lines.Length, audioList?.Count ?? 0);

            for (int i = 0; i < count; i++)
            {
                var element = new ElementTranslate(elementBatch.Session)
                {
                    Audio = audioList[i],
                    Content = lines[i].Trim(),
                    Language = batch.Language
                };
            }
        }


        #endregion SourceCode3303ImportCode

        #region SourceCode3382ImportCode
                        public static bool EndsWithSoftPunctuation(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return false;
            char last = text.Trim().LastOrDefault();
            return new[] { ',' }.Contains(last);
        }

        public static bool EndsWithStrongPunctuation(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return false;
            char last = text.Trim().LastOrDefault();
            return new[] { '.', ';', ':', '?', '!', '-', '_', '—' }.Contains(last);
        }



        #endregion SourceCode3382ImportCode

        #region SourceCode3384ImportCode
                        public static string InferSubtitle(string subtitle1, string content1, string translated, bool reverse,DataService dataService,Video video,Module.Services.DataServiceService dataServiceService,double threshold,out bool shouldFlag)
        {
            shouldFlag = false;

            var scoreList = !string.IsNullOrEmpty(subtitle1)
                ? CalculateInferScores(subtitle1.Split(' ', StringSplitOptions.RemoveEmptyEntries), subtitle1, content1, translated, reverse, dataService, video, dataServiceService)
                : new List<(string, double)>();

            var best = scoreList.OrderByDescending(s => s.Item2).FirstOrDefault();
            var second = scoreList.OrderByDescending(s => s.Item2).Skip(1).FirstOrDefault();

            if (best.Item2 > threshold)
            {
                if (!string.IsNullOrEmpty(best.Item1) && !string.IsNullOrEmpty(second.Item1))
                {
                    double bestSim = Module.Helpers.TextHelper.CalculateWordSimilarity(best.Item1, translated);
                    double secondSim = Module.Helpers.TextHelper.CalculateWordSimilarity(second.Item1, translated);
                    if (bestSim < secondSim)
                        shouldFlag = true;
                }

                return best.Item1;
            }

            return null;
        }


        #endregion SourceCode3384ImportCode

        #region SourceCode3356ImportCode
                        public static string ProcessMatchLineAndRearrange(
          string string1, // content: nội dung chứa thứ tự đích : kết quả cho ra sẽ có số dòng tương tự như string1
          string string2, // match: chứa nội dung sẽ so sánh vói string1 để tạo ra bảng thứ tự khớp
          string string3, // origin: nội dung cần sắp xếp lại, có số dòng <= string2
          DataService dataService,
          DevExpress.ExpressApp.XafApplication application)
        {
            // Tách content thành danh sách dòng có thứ tự
            List<string> list1 = Module.Helpers.TextHelper.SplitContentToList(string1);

            // Tách match thành danh sách dòng (gắn thêm bool = false)
            List<string> listmatch = Module.Helpers.TextHelper.SplitContentToList(string2);
            var list2 = new List<(string, bool)>();
            foreach (var item in listmatch)
            {
                list2.Add((item, false));
            }

            // Tìm ánh xạ dòng giữa content và match
            var matchLines = Module.Services.AudioService.SemanticMatchLine(list1, list2, dataService);

            // Nếu tìm được ánh xạ dòng → sắp xếp lại origin
            if (matchLines.Count > 0)
            {
                return Module.Services.AudioService.RearrangeString(string3, matchLines, list1.Count, application);
            }

            // Không tìm được ánh xạ → trả về origin không đổi
            return string3;
        }

        #endregion SourceCode3356ImportCode

  
  
        #region Base Object Service
	    		//public string ToolTipControllerText(View view, Module.BusinessObjects.BatchTranslate batchtranslate)
        //{
        //    var result = "";
            //if (this.Name != null)
            //{
            //    if (!string.IsNullOrEmpty(result))
            //        result += "\r\n";
            //    result += "Tiêu đề:" + Name;
            //}            
        //    return result;
        //}
		
		//Tooltip for Object
		//public object LanguageToolTipControllerText(View view, Module.BusinessObjects.BatchTranslate batchtranslate)
        //{
        //    if (Language != null) 
		//			return Language;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object LineQuantityToolTipControllerText(View view, Module.BusinessObjects.BatchTranslate batchtranslate)
        //{
        //    if (LineQuantity != null) 
		//			return LineQuantity;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ContentToolTipControllerText(View view, Module.BusinessObjects.BatchTranslate batchtranslate)
        //{
        //    if (Content != null) 
		//			return Content;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object OriginLanguageToolTipControllerText(View view, Module.BusinessObjects.BatchTranslate batchtranslate)
        //{
        //    if (OriginLanguage != null) 
		//			return OriginLanguage;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object TranslateLineQuantityToolTipControllerText(View view, Module.BusinessObjects.BatchTranslate batchtranslate)
        //{
        //    if (TranslateLineQuantity != null) 
		//			return TranslateLineQuantity;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object TranslateToolTipControllerText(View view, Module.BusinessObjects.BatchTranslate batchtranslate)
        //{
        //    if (Translate != null) 
		//			return Translate;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object Translate2ToolTipControllerText(View view, Module.BusinessObjects.BatchTranslate batchtranslate)
        //{
        //    if (Translate2 != null) 
		//			return Translate2;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ElementBatchToolTipControllerText(View view, Module.BusinessObjects.BatchTranslate batchtranslate)
        //{
        //    if (ElementBatch != null) 
		//			return ElementBatch;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object Translate2LineQuantityToolTipControllerText(View view, Module.BusinessObjects.BatchTranslate batchtranslate)
        //{
        //    if (Translate2LineQuantity != null) 
		//			return Translate2LineQuantity;
        //    return null;
        //}
    

	    #endregion
  

    }
}
