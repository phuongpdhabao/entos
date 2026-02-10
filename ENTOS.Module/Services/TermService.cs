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


 
namespace ENTOS.Module.Services 
{

    public partial class TermService : BaseService
    {

        public TermService() : base()
        {
        }
        #region DependencyInjection
  
     
        private VideoService videoService;  
        protected VideoService _videoService => videoService ??= new VideoService(ViewController);        
       
  
        #endregion DependencyInjection

        public TermService(ViewController viewController) : base(viewController)
        {

        }
        //public event EventHandler<string>? OnError;
        //public event EventHandler<string>? OnSuccess;
        //public event EventHandler<string>? OnResponse; 
        
        #region SourceCode3331ImportCode
                                public void ImportTermsFromDictionaries(
    Video video,
    IEnumerable<Dictionary> selectedDictionaries,
    IObjectSpace objectSpace,
    Dictionary<string, Term> existedTermsList, // ✅ đổi kiểu ở đây
    System.Diagnostics.Stopwatch stopWatch,
    bool isReversed = false,
    bool searchInSubtitle = false,
    string caption = "Đang nạp từ phức"
)
        {
            DateTime startTime = DateTime.Now;

            foreach (var dictionary in selectedDictionaries)
            {
                // Đảo ngữ gốc / ngữ dịch nếu là chế độ đảo chiều
                var langOrigin = isReversed ? video.LanguageTranslate : video.LanguageOrigin;
                var langTranslate = isReversed ? video.LanguageOrigin : video.LanguageTranslate;

                CriteriaOperator dicCriteria;

                if (isReversed)
                {
                    dicCriteria = CriteriaOperator.Parse(
                        "Dictionary.Oid = ? and LanguageTranslate.Oid = ?", dictionary.Oid, langOrigin.Oid);
                }
                else
                {
                    dicCriteria = CriteriaOperator.Parse(
                        "Dictionary.Oid = ? and LanguageOrigin.Oid = ?", dictionary.Oid, langOrigin.Oid);
                }

                // Tùy chọn: chỉ lấy từ phức (nếu muốn)
                //dicCriteria = CriteriaOperator.And(dicCriteria, 
                //    CriteriaOperator.Parse("Contains([Name], ' ')"));

                var dictionaryWordList = objectSpace.GetObjects<DictionaryWord>(dicCriteria);
                var termList = new List<Term>();
                decimal countNumber = 0;
                int total = dictionaryWordList.Count;
                Tools.ShowOrCloseDefaultWaitForm(caption, null, stopWatch.Elapsed, true);

                foreach (var dictionaryWord in dictionaryWordList)
                {
                    if (Tools.DefaultSplashScreenManager is null)
                        break;

                    // Tạo danh sách từ: name hoặc translate, thêm s/es nếu là tiếng Anh
                    string baseWord = isReversed ? dictionaryWord.Translate : dictionaryWord.Name;
                    if (string.IsNullOrWhiteSpace(baseWord))
                        continue;

                    var wordForms = new[] { baseWord, baseWord + "s", baseWord + "es" };

                    foreach (var wordName in wordForms)
                    {
                        if (string.IsNullOrWhiteSpace(wordName))
                            continue;

                        string key = wordName.ToLowerInvariant();
                        if (existedTermsList.ContainsKey(key))
                            continue;

                        var term = new Term(video.Session)
                        {
                            Video = video,
                            TermType = TermType.Dictionary,
                            Name = wordName,
                            Language = langOrigin
                        };
                        video.TermList.Add(term);

                        string translate = null;

                        if (isReversed)
                        {
                            translate = dictionaryWord.Name;
                        }
                        else if (dictionaryWord.LanguageTranslate != null &&
                                 !dictionaryWord.LanguageTranslate.Oid.Equals(langTranslate.Oid))
                        {
                            // Tìm dịch phù hợp với ngôn ngữ đích
                            translate = dictionaryWord.TranslateWordList
                                .FirstOrDefault(tw => tw.Language != null && tw.Language.Oid.Equals(langTranslate.Oid))?.Name;

                            if (translate == null)
                            {
                                term.Delete();
                                break;
                            }
                        }
                        else
                        {
                            translate = dictionaryWord.Translate;
                        }

                        term.Translate = translate;

                        // Nếu không bị xóa thì cập nhật vị trí
                        if (!term.IsDeleted)
                        {
                            UpdatePosition(term, true, useSubtitle: searchInSubtitle);
                            if (!term.IsDeleted)
                                termList.Add(term);
                        }
                    }

                    if (Tools.DefaultSplashScreenManager is null)
                        break;

                    if (total > 5)
                    {
                        countNumber++;
                        Tools.ShowOrCloseDefaultWaitForm(null, (countNumber / total).ToString("p0"), stopWatch.Elapsed, true);
                    }
                }

                // Cập nhật lại vị trí và dịch cho TermLocation
                for (int i = termList.Count - 1; i >= 0; i--)
                {
                    if (Tools.DefaultSplashScreenManager is null)
                        break;

                    var term = termList[i];
                    if (!term.IsDeleted)
                    {
                        UpdatePosition(term,true, useSubtitle: searchInSubtitle);

                        if (!string.IsNullOrEmpty(term.Translate))
                        {
                            foreach (var loc in term.TermLocationList)
                            {
                                if (string.IsNullOrEmpty(loc.Translate))
                                    loc.Translate = term.Translate;
                            }
                        }
                    }

                    if (total > 5)
                    {
                        countNumber++;
                        Tools.ShowOrCloseDefaultWaitForm(null, (countNumber / total).ToString("p0"), stopWatch.Elapsed, true);
                    }
                }

                if (Tools.DefaultSplashScreenManager is null)
                    break;

                if (stopWatch.Elapsed.TotalMinutes > 1)
                {
                    _videoService.LogToNote(video, startTime, caption, total, (int)countNumber, stopWatch.Elapsed);
                }
            }
        }


        #endregion SourceCode3331ImportCode

        #region SourceCode4514ImportCode
                                        public DictionaryWord GetDictionaryWord(string word, Language orgin, Dictionary dictionary)
{
            
            var critera = DevExpress.Data.Filtering.CriteriaOperator.Parse(
        "((Name = ? and LanguageOrigin.Oid = ?) or TranslateWordList[Name = ? and Language.Oid = ?]) and Dictionary.Oid = ?", 
        word, orgin.Oid, word, orgin.Oid, dictionary.Oid);
    var dictionaryWord = ObjectSpace.FindObject<DictionaryWord>(critera);           
    if (dictionaryWord is null)
    {
        //2023-06-26: Tra từ điển em cho phép tiếp vĩ ngữ s, es, ed, ing cho anh
        DevExpress.Data.Filtering.CriteriaOperator criteriaOperatorDictionaryWord = null;
        DevExpress.Data.Filtering.CriteriaOperator criteriaOperatorTranslateWord = null;
        string[] suffixList = new string[] { "s", "es", "ed", "ing" };
        foreach (string suffix in suffixList)
        {
            criteriaOperatorDictionaryWord = DevExpress.Data.Filtering.CriteriaOperator.Or(criteriaOperatorDictionaryWord,
               DevExpress.Data.Filtering.CriteriaOperator.Parse("Name = ?", word + suffix));
            criteriaOperatorTranslateWord = DevExpress.Data.Filtering.CriteriaOperator.Or(criteriaOperatorTranslateWord,
               DevExpress.Data.Filtering.CriteriaOperator.Parse("Name = ?", word + suffix));
        }
        criteriaOperatorDictionaryWord = DevExpress.Data.Filtering.CriteriaOperator.And(criteriaOperatorDictionaryWord,
               DevExpress.Data.Filtering.CriteriaOperator.Parse("LanguageOrigin.Oid = ? and Dictionary.Oid = ?", orgin.Oid, dictionary.Oid));
        criteriaOperatorTranslateWord = DevExpress.Data.Filtering.CriteriaOperator.And(criteriaOperatorTranslateWord,
               DevExpress.Data.Filtering.CriteriaOperator.Parse("Language.Oid = ? and DictionaryWord.Dictionary.Oid = ?", orgin.Oid, dictionary.Oid));
        dictionaryWord = ObjectSpace.FindObject<DictionaryWord>(criteriaOperatorDictionaryWord);
        if (dictionaryWord is null)
        {
            var translateWord = ObjectSpace.FindObject<TranslateWord>(criteriaOperatorTranslateWord);
            if (translateWord != null)
                dictionaryWord = translateWord.DictionaryWord;
        }
    }
    return dictionaryWord;



    //Module.Helpers.LogHelper.Info(logMessage + " - End");
}

        private char importCharTag = '[';
        public int ImportTermUpperCaseAndNumberCharacter(Video video, System.Collections.Generic.Dictionary<string, Term> existedTermsList, bool upperCase, System.Diagnostics.Stopwatch stopWatch, bool IsReverse)
        {
            //Nạp tham số
            var startime = System.DateTime.Now;
            //2025-02-12: Bỏ tham số này
            //var parameter = Module.Helpers.ParameterHelper.GetParameterValueOrDefault(ObjectSpace, "MaxTermLocationWhenImport", "1");            
            //int maxTermLocation = parameter.GetIntValue();

            // 2023 - 06 - 01: Start: Bỏ trường hợp này
            //string[] removeWords = new string[] { "I", "I'm","I've","I'd","I'll" };
            //System.Collections.Generic.IList<string> result = new System.Collections.Generic.List<string>();
            //2024-08-05:Nếu thuật ngữ tồn tại 1 thuật vị là đủ tiêu chuẩn là Viết hoa thì sẽ xếp loại Viết hoa, dựng cờ nếu tồn tại cả thuật vị viết thường
            //2023-07-12: Chat : Nạp viết hoa: 1 từ sẽ k coi là viết tắt hay viết hoa nếu tồn tại từ đó dạng viết thường trong tư liệu
            //var removesTerms = new System.Collections.Generic.List<string>();
            var unitCharacterParameter = GetValueOrDefault("UnitCharacter", "-/°$%^₫¢$€£¥₮৲৳௹฿៛₠₡₢₣₤₥₦₧₨₩₪₫₭₯₰₱₲₳₴₵￥﷼¤ƒλδθΩΔσ");
            var unitCharacters = unitCharacterParameter.ToCharArray();
            var flagTerms = new System.Collections.Generic.List<string>();
            var resultTerm = new System.Collections.Generic.Dictionary<string, Term>();
            int add = 0;
            System.Collections.Generic.IDictionary<string, int> resultQuantity = new System.Collections.Generic.Dictionary<string, int>();
            //System.Collections.Generic.IDictionary<string, int> resultPosition = new System.Collections.Generic.Dictionary<string, int>();
            //int position = 0;
            decimal countNumber = 0;
            string caption = upperCase ? "Nạp viết hoa" : "Nạp số và ký tự";
            Tools.ShowOrCloseDefaultWaitForm(caption, null, stopWatch.Elapsed, true);
            foreach (var audio in video.GetAudioListWithSort())
            {
                string content = audio.Content;
                if (IsReverse)
                    content = audio.Subtitle;

                if (string.IsNullOrEmpty(content))
                    continue;
                //091: Nạp viết hoa: không xét các Thành phần có CaseType = Đầu hoa hoặc Toàn hoa
                if (upperCase && (audio.CaseType == CaseType.UpperCase || audio.CaseType == CaseType.UpperCaseAll))
                    continue;
                //int position = 0;
                //Cắt theo dòng                                            
                var sentencesArray = Module.Helpers.TextHelper.GetSentences(content);
                for (int m = 0; m < sentencesArray.Count(); m++)
                {
                    var wordsArray = Module.Helpers.TextHelper.GetWords(sentencesArray[m]);
                    //var contents = rows[m].Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
                    //int rowPosition = 0;
                    for (int n = 0; n < wordsArray.Length; n++)
                    {
                        int workPositionInRow = n;
                        string word = Module.Helpers.TextHelper.RemoveStartEndSpecialCharacters(wordsArray[n], null, !upperCase ? unitCharacters : Module.Helpers.TextHelper.CharsStartEndWord);
                        if (string.IsNullOrEmpty(word))
                            continue;

                        if (word.Length == 1)
                        {
                            //rowPosition++;
                            //Nếu là từ có 1 ký tự thì nạp luôn
                            if (existedTermsList.ContainsKey(word.ToLower()))
                                continue;
                            if (!upperCase)
                                add = AddWordToTerm(word, m, workPositionInRow,
                                    add, video, audio, resultTerm, resultQuantity, upperCase);
                        }
                        else if (word.Length > 1)
                        {
                            //2023-10-27: Đơn giản hóa tách câu tách từ: bỏ 2 list kí tự để Thuật vị chính xác
                            //Không quan tâm đến ký tự ngăn cách câu, chỉ tách từ bằng dấu cách
                            //string text = wordsArray[n][0].ToString();
                            ////Kiểm tra xem ở giữa có ký tự đặc biệt ngăn cách ở giữa không
                            //bool hasIsSeparator = false;
                            //for (int i = 1; i < word.Length; i++)
                            //{
                            //    //2023-07-26: YC: Coi dấu gạch ngang là kí tự khi nạp thuật ngữ, trừ khi là đầu câu.
                            //    if (char.IsLetterOrDigit(word[i]) || Tools.CheckSpecialCharactersValidate(word[i]))
                            //    {
                            //        text += word[i];
                            //    }
                            //    else
                            //    {
                            //        text += ' ';
                            //        hasIsSeparator = true;
                            //    }
                            //}
                            //if (hasIsSeparator)
                            //{
                            //    var words = text.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
                            //    //bool wAdd = false;
                            //    foreach (var childWord in words)
                            //    {
                            //        //Thêm ký tự đơn
                            //        if (childWord.Length == 1)
                            //        {
                            //            ////vị trí chỉ tăng thêm 1 lần, các từ sau giữ nguyên vị trí
                            //            //if (!wAdd)
                            //            //    rowPosition++;
                            //            if (e.SelectedChoiceActionItem.Id.Equals("NumberCharacter"))
                            //                add = AddWordToTerm(childWord, m, workPositionInRow, maxTermLocation,
                            //                    add, video, audio, resultTerm, resultQuantity);
                            //        }
                            //        //2023-07-12: Chat : Nạp viết hoa: 1 từ sẽ k coi là viết tắt hay viết hoa nếu tồn tại từ đó dạng viết thường trong tư liệu
                            //        else if (Module.Helpers.TextHelper.ListContains(removesTerms, childWord) < 0)
                            //        {
                            //            if (char.IsLower(childWord[0]))
                            //            {
                            //                string key = Module.Helpers.TextHelper.ReplaceSpecialCharacters(resultTerm.Keys, childWord);
                            //                if (!string.IsNullOrEmpty(key))
                            //                    removesTerms.Add(key);
                            //            }
                            //            //Nếu là 2 ký tự hoa hoặc có ký tự số thì thêm vào
                            //            bool validate = char.IsUpper(childWord[0]) && char.IsUpper(childWord[1]);
                            //            if (!validate)
                            //            {
                            //                for (int j = 0; j < childWord.Length; j++)
                            //                {
                            //                    if (char.IsNumber(childWord[j]))
                            //                    {
                            //                        validate = true;
                            //                        break;
                            //                    }
                            //                }
                            //            }
                            //            if (validate)
                            //            {
                            //                //if (!wAdd)
                            //                //    rowPosition++;
                            //                bool allNumber = true;
                            //                foreach (var c in childWord)
                            //                {
                            //                    //Nếu ký tự thường cũng coi là số
                            //                    if (!char.IsNumber(c) && !char.IsLower(c) && !Tools.CheckSpecialCharactersValidate(c))
                            //                    {
                            //                        allNumber = false;
                            //                        break;
                            //                    }
                            //                }
                            //                if ((allNumber && e.SelectedChoiceActionItem.Id.Equals("NumberCharacter")) || (!allNumber && e.SelectedChoiceActionItem.Id.Equals("UpperCase")))
                            //                    add = AddWordToTerm(childWord, m, workPositionInRow, maxTermLocation,
                            //                            add, video, audio, resultTerm, resultQuantity);
                            //            }
                            //        }
                            //    }
                            //}
                            //else


                            //if (Module.Helpers.TextHelper.ListContains(removesTerms, word) < 0)
                            //2024-08-05:Nếu thuật ngữ tồn tại 1 thuật vị là đủ tiêu chuẩn là Viết hoa thì sẽ xếp loại Viết hoa, dựng cờ nếu tồn tại cả thuật vị viết thường
                            if (char.IsLower(word[0]))
                            {
                                var lowerWord = word.ToLower();
                                if (!flagTerms.Contains(lowerWord))
                                    flagTerms.Add(lowerWord);
                                //string key = Module.Helpers.TextHelper.ReplaceSpecialCharacters(resultTerm.Keys, word);
                                //if (!string.IsNullOrEmpty(key))
                                //    removesTerms.Add(key);
                            }
                            if (char.IsUpper(word[0]) && n == 0)
                            {
                                //2023-06-08
                                //Từ đầu câu mà có chữ hoa ở giữa câu hoặc từ tiếp theo là hoa thì vẫn tính là hoa và cụm hoa
                                bool validate = false;

                                for (int k = 1; k < word.Length; k++)
                                {
                                    if (!char.IsLetterOrDigit(word[k]))
                                        break;
                                    if (char.IsUpper(word[k]) || char.IsNumber(word[k]))
                                    {
                                        validate = true;
                                        break;
                                    }
                                }
                                if (!validate)
                                    continue;
                            }
                            //Nếu là ký tự hoa thì hợp lệ
                            bool wordValidate = char.IsUpper(word[0]) || char.IsUpper(word[1]);
                            if (!wordValidate)
                            {
                                //Nếu có ký tự số thì hợp lệ
                                //for (int j = 0; j < word.Length; j++)
                                //{
                                //    if (char.IsNumber(word[j]))
                                //    {
                                //        wordValidate = true;
                                //        break;
                                //    }
                                //}
                                if (Tools.IsNumber(word, unitCharacters))
                                {
                                    if (!upperCase)
                                    {
                                        add = AddWordToTerm(word, m, workPositionInRow,
                                            add, video, audio, resultTerm, resultQuantity, upperCase, TermType.Number);
                                    }
                                    //Nếu số ở đầu thì sẽ không ghép với từ hoa sau trong trường hợp ghép sau
                                    continue;
                                }
                                else if (char.IsNumber(word[0]))
                                {
                                    //2025-02-15: 004: Số coi là 1 ký tự hoa: 4k, 1000kg > đầu hoa, 4K > toàn hoa
                                    wordValidate = true;
                                }
                            }
                            if (wordValidate && char.IsUpper(word[0]) && n >= 1)
                            {
                                //Không coi là từ Hoa với các từ viết hoa đầu câu hoặc sau dâu: " ; ( ; { ; [ ; : (2 chấm)
                                var beforeWord = wordsArray[n - 1];
                                //if (!Tools.BeforeChars.Contains(beforeWord[beforeWord.Length - 1]))
                                //    wordValidate = true;
                                wordValidate = !Module.Helpers.TextHelper.CheckPositonIsStartSentence(beforeWord, beforeWord.Length);
                            }

                            if (wordValidate)
                            {
                                //Nếu là ký tự hoa thì ghép các từ sau nó
                                //Nếu sau cuối không phải là ký tự đặc biệt thì ghép
                                if (wordsArray[n].EndsWith(word))
                                {
                                    for (int j = n + 1; j < wordsArray.Length; j++)
                                    {
                                        if (char.IsUpper(wordsArray[j][0]) || char.IsNumber(wordsArray[j][0]))
                                        {
                                            word += " " + Module.Helpers.TextHelper.RemoveStartEndSpecialCharacters(wordsArray[j], null, Module.Helpers.TextHelper.CharsStartEndWord);
                                            n = j;
                                            //Nếu ký tự cuối không phải ký tự hoặc số thì dừng ghép
                                            var lastedChar = wordsArray[j][wordsArray[j].Length - 1];
                                            if (!char.IsLetterOrDigit(lastedChar))
                                                break;
                                            //if (j + 1 < contents.Length)
                                            //{                                                        
                                            //    var lastedChar = contents[j + 1][contents[j + 1].Length - 1];
                                            //    if (!char.IsLetterOrDigit(lastedChar))
                                            //        break;
                                            //}                                                    
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }
                                //rowPosition++;
                                //2025-02-15: 004
                                //bool allNumber = true;
                                //foreach (var c in word)
                                //{
                                //    //Nếu ký tự thường cũng coi là số
                                //    if (!char.IsNumber(c) && !char.IsLower(c) && !Tools.CheckSpecialCharactersValidate(c) && !Tools.SpecialCharactersIsChar.Contains(c))
                                //    {
                                //        allNumber = false;
                                //        break;
                                //    }
                                //}

                                var lowerWord = word.ToLower();
                                if (existedTermsList.ContainsKey(lowerWord))
                                    continue;
                                //2024-08-05:Nếu thuật ngữ tồn tại 1 thuật vị là đủ tiêu chuẩn là Viết hoa thì sẽ xếp loại Viết hoa, dựng cờ nếu tồn tại cả thuật vị viết thường
                                //2023-07-12: Chat : Nạp viết hoa: 1 từ sẽ k coi là viết tắt hay viết hoa nếu tồn tại từ đó dạng viết thường trong tư liệu
                                //if (word.Split(' ').Length == 1 && removesTerms.Contains(lowerWord))
                                //    continue;
                                //2025-02-15: 004
                                //if ((allNumber && e.SelectedChoiceActionItem.Id.Equals("NumberCharacter")) || (!allNumber && e.SelectedChoiceActionItem.Id.Equals("UpperCase")))
                                //    add = AddWordToTerm(word, m, workPositionInRow,
                                //        add, video, audio, resultTerm, resultQuantity, e);

                                if (upperCase)
                                    add = AddWordToTerm(word, m, workPositionInRow,
                                        add, video, audio, resultTerm, resultQuantity, upperCase);
                            }
                        }
                    }
                    //Cắt row để không bị lệch khi thay đổi vị trí
                    //position += sentencesArray[m].Split(' ', System.StringSplitOptions.RemoveEmptyEntries).Length;
                }
                countNumber++;
                if (Tools.DefaultSplashScreenManager is null)
                    break;
                Tools.ShowOrCloseDefaultWaitForm(null, (countNumber / video.AudioList.Count / 2).ToString("p0"), stopWatch.Elapsed, true);
            }

            if (add > 0)
            {
                countNumber = 0;
                decimal total = resultTerm.Keys.Count;
                //Thêm những thuật ngữ mới vào đối tượng
                foreach (var key in resultTerm.Keys)
                {
                    if (Tools.DefaultSplashScreenManager is null)
                        break;
                    //2024-08-07: Phương sửa lại: Nếu thuật ngữ tồn tại 1 thuật vị là đủ tiêu chuẩn là Viết hoa thì sẽ xếp loại Viết hoa, dựng cờ nếu tồn tại cả thuật vị viết thường
                    if (flagTerms.Contains(key))
                    {
                        resultTerm[key].Flag = true;
                        resultTerm[key].AddTextNode(importCharTag, "Lẫn hoa thường");
                    }
                    else if (key.Contains(" "))
                    {
                        //Nếu thuật ngữ kép cần kiểm tra lại
                    }
                    //2023-07-12: Chat : Nạp viết hoa: 1 từ sẽ k coi là viết tắt hay viết hoa nếu tồn tại từ đó dạng viết thường trong tư liệu
                    if (resultTerm[key].Video is null
                        //2024-08-05:Nếu thuật ngữ tồn tại 1 thuật vị là đủ tiêu chuẩn là Viết hoa thì sẽ xếp loại Viết hoa, dựng cờ nếu tồn tại cả thuật vị viết thường
                        //&& !removesTerms.Contains(key)
                        )
                    {
                        video.TermList.Add(resultTerm[key]);
                        if (upperCase)
                        {
                            //Cập nhật thuật vị
                            UpdatePosition(resultTerm[key], true, useSubtitle: IsReverse);
                        }
                        countNumber++;
                        Tools.ShowOrCloseDefaultWaitForm(null, ((decimal)0.5 + (countNumber / total / 2)).ToString("p0"), stopWatch.Elapsed, true);
                    }

                }

                Tools.RefreshGridView(View);
                string message = "Có " + add + " được nạp";
                //2024-08-05:Nếu thuật ngữ tồn tại 1 thuật vị là đủ tiêu chuẩn là Viết hoa thì sẽ xếp loại Viết hoa, dựng cờ nếu tồn tại cả thuật vị viết thường
                //if (removesTerms.Count > 0)
                //{
                //    message += System.Environment.NewLine + removesTerms.Count + " Từ viết thường: " + string.Join(", ", removesTerms);
                //}
                _notificationService.NotifySuccess("Thành công", message);
            }
            Tools.ShowOrCloseDefaultWaitForm(null);
            //video.Note += string.Format("\r\n{0} : {1} : {2} : {3} : {4}", startime.ToString("dd/MM/yyyy h:mm"), ImportTerm.Caption + " " + e.SelectedChoiceActionItem.Caption, video.AudioList.Count, add, System.Math.Round(stopWatch.Elapsed.TotalMinutes, 0));
            //video.LogToNote(startime, ImportTerm.Caption + " " + caption, video.AudioList.Count, add, stopWatch.Elapsed);
            return add;
        }
        public void ImportCompoundWordFromDictionary(Video video, System.Collections.Generic.List<string> exceptionWordList, System.Collections.Generic.Dictionary<string, Term> existedTermsList, DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventArgs e, System.Diagnostics.Stopwatch stopWatch, ref int existCount, bool isReversed)
        {
            //2025-02-11: Ghép cả 2 và 3
            //int maxText = e.SelectedChoiceActionItem.Id.Equals("CompoundWord2") ? 2 : 3;
            System.Collections.Generic.Dictionary<int, System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>>> dictionary = null;

            if (isReversed)
                dictionary = video.GetDictionarySubtitle();
            else
                dictionary = video.GetDictionary();

            //if (dictionary is null || !dictionary.ContainsKey(maxText))
            if (dictionary is null)
            {
                throw new UserFriendlyException("Không tìm thấy từ phức này");
                return;
            }
            var startime = System.DateTime.Now;
            //int successCount = 0;
            string[] singleCharValidate = new string[] { "e", "y", "u", "i", "o", "a" };
            var resultTerm = new System.Collections.Generic.Dictionary<string, Term>();

            //var termNotCorrect = new System.Collections.Generic.Dictionary<string, bool>();
            var noneCharDictionary = new System.Collections.Generic.Dictionary<string, bool>();
            var resultTermWords = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>>();
            bool existTerm = video.TermList.Count > 0;
            if (existTerm)
            {
                //Nếu tồn tại thuật ngữ thì nạp danh sách thuật ngữ vào
                foreach (var term in video.TermList)
                {
                    if (string.IsNullOrEmpty(term.Name))
                        continue;
                    resultTerm.Add(term.Name, term);
                    var wordsList = new System.Collections.Generic.List<string>();
                    wordsList.Add(term.Name);
                    resultTermWords.Add(term.Name, wordsList);

                }
            }
            decimal countNumber = 0;
            int total = video.AudioList.Count;
            var audioListWithSort = video.GetAudioListWithSort();
            Tools.ShowOrCloseDefaultWaitForm("Đang kiểm tra", null, stopWatch.Elapsed, true);
            foreach (var audio in audioListWithSort)
            {
                string content = audio.Content;
                if (isReversed)
                    content = audio.Subtitle;

                if (Tools.DefaultSplashScreenManager is null)
                    break;
                if (string.IsNullOrEmpty(content))
                    continue;
                int position = 0;

                //Kiểm tra xem thuật ngữ có sẵn thì loại
                //var audioContent = Module.Helpers.TextHelper.RemoveUnicode(content);
                var audioContent = content.Replace("  ", " ").Trim();
                //var rows = audioContent.Split(Module.Helpers.TextHelper.NewLineText, System.StringSplitOptions.RemoveEmptyEntries);
                var sentencesArray = Module.Helpers.TextHelper.GetSentences(content);

                int wordIndex = 0;
                for (int i = 0; i < sentencesArray.Count(); i++)
                {
                    //Tách position và rowPosition để tránh trường hợp từ thay thế khác từ hiện tại
                    int rowPosition = 0;
                    var wordsArray = Module.Helpers.TextHelper.GetWords(sentencesArray[i]);

                    //var childContentArray = sentencesArray[i].Split(Module.Helpers.TextHelper.SeperateChars, System.StringSplitOptions.RemoveEmptyEntries);
                    //foreach (var childContentText in childContentArray)
                    //{
                    //    var contents = childContentText.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);

                    //}
                    bool?[] noneCharContents = new bool?[wordsArray.Length];
                    //bool?[] notCorrectContents = new bool?[contents.Length];                    
                    //if (maxText == 3)
                    //{
                    int fourWordIndex = wordIndex;
                    int fourRowPosition = rowPosition;
                    int fourPosition = position;

                    int fiveWordIndex = wordIndex;
                    int fiveRowPosition = rowPosition;
                    int fivePosition = position;

                    int threeWordIndex = wordIndex;
                    int threeRowPosition = rowPosition;
                    int threePosition = position;

                    //Nạp 5
                    if (dictionary.ContainsKey(5))
                        ImportCompoundWordFromText(wordsArray, audio, i, sentencesArray[i], dictionary, resultTermWords,
                            resultTerm, 5, noneCharDictionary, singleCharValidate, exceptionWordList,
                            ref noneCharContents, ref fiveWordIndex, ref fiveRowPosition, ref fivePosition, ref existCount);
                    //Nạp 4
                    if (dictionary.ContainsKey(4))
                        ImportCompoundWordFromText(wordsArray, audio, i, sentencesArray[i], dictionary, resultTermWords,
                            resultTerm, 4, noneCharDictionary, singleCharValidate, exceptionWordList,
                            ref noneCharContents, ref fourWordIndex, ref fourRowPosition, ref fourPosition, ref existCount);
                    //Nạp 3
                    if (dictionary.ContainsKey(3))
                        ImportCompoundWordFromText(wordsArray, audio, i, sentencesArray[i], dictionary, resultTermWords,
                            resultTerm, 3, noneCharDictionary, singleCharValidate, exceptionWordList,
                            ref noneCharContents, ref threeWordIndex, ref threeRowPosition, ref threePosition, ref existCount);

                    //}
                    //else
                    //{
                    if (dictionary.ContainsKey(2))
                        ImportCompoundWordFromText(wordsArray, audio, i, sentencesArray[i], dictionary, resultTermWords,
                            resultTerm, 2, noneCharDictionary, singleCharValidate, exceptionWordList,
                            ref noneCharContents, ref wordIndex, ref rowPosition, ref position, ref existCount);
                    //}

                    wordIndex++;
                    //rowPosition++;

                    position += sentencesArray[i].Split(' ', System.StringSplitOptions.RemoveEmptyEntries).Length;
                }

                if (Tools.DefaultSplashScreenManager is null) //Hủy phải trước sự kiện hiện form
                    break;
                if (total > 5 && countNumber < total)
                {
                    countNumber++;
                    Tools.ShowOrCloseDefaultWaitForm(null, (countNumber / total).ToString("p0"), stopWatch.Elapsed, true);
                }
            }

            countNumber = 0;
            total = resultTerm.Keys.Count;
            //Cẩu trúc cũ
            //foreach (var key in resultTerm.Keys)
            //{
            //    //Nếu từ ít hơn thì bỏ, mặc định là 2
            //    //Nếu mà thuật ngữ không sai chính tả thì không hiện
            //    //int compoundWordRecurrence = video.CompoundWordRecurrence != null ? video.CompoundWordRecurrence.Value : 2;
            //    //if (!termNotCorrect[key] || (resultTerm[key].Quantity != null && compoundWordRecurrence > resultTerm[key].Quantity))
            //    //if (!termNotCorrect[key])
            //    //{
            //    //    resultTerm[key].Session.Delete(resultTerm[key].TermLocationList);
            //    //    resultTerm[key].Delete();
            //    //}
            //    //else
            //    //{
            //        resultTerm[key].Video = video;         
            //    //}                
            //    countNumber++;                
            //    Tools.ShowOrCloseDefaultWaitForm((countNumber / total).ToString("p0"), caption, stopWatch.Elapsed);
            //}
            //if (maxText == 3)
            //{
            //    var listTermLocationsWithSort = SortTermLocations(listTermLocations);
            //    for(int i = 1; i < listTermLocationsWithSort.Count(); i++)
            //    {
            //        if (listTermLocationsWithSort[i].Element == listTermLocationsWithSort[i].Element &&
            //            listTermLocationsWithSort[i].Sentence == listTermLocationsWithSort[i].Sentence &&
            //            listTermLocationsWithSort[i].Location != null &&  listTermLocationsWithSort[i].Location != null)
            //        {
            //            //var diff =
            //        }
            //    }
            //}
            Tools.ShowOrCloseDefaultWaitForm(null, "Đang nạp", stopWatch.Elapsed, true);
            var resultList = resultTerm.Values.Where(m => m.Video is null && !m.IsDeleted && m.Quantity > 0);
            video.TermList.AddRange(resultList);
            //foreach (var term in resultTerm.Values)
            //{     
            //    //Chỉ gán những kết quả mới
            //    if(term.Video is null)
            //    {
            //        term.Video = video;
            //        countNumber++;
            //        Tools.ShowOrCloseDefaultWaitForm((countNumber / total).ToString("p0"), "Đang nạp", stopWatch.Elapsed);
            //    }                
            //}
            //stopWatch.Stop();
            if (stopWatch.Elapsed.TotalMinutes > 1)
            {
                //Nếu nhỏ hơn 1 phút thì không log
                //StartTime : Chức năng : SL multiselect : SL kết quả : Tổng thời gian xử lý (phút làm tròn)                
                //video.LogToNote(startime, ImportTerm.Caption + " " + e.SelectedChoiceActionItem.Caption, video.AudioList.Count, resultList.Count(), stopWatch.Elapsed);
            }
            Tools.ShowOrCloseDefaultWaitForm(null);
        }



        private System.Collections.Generic.List<TermLocation> SortTermLocations(System.Collections.Generic.List<TermLocation> termLocations)
        {
            return termLocations.Where(m => m.Audio != null && m.Audio.Start != null).OrderBy(m => m.Audio.Start).ThenBy(m => m.Sentence).ThenBy(m => m.Location).ThenBy(m => m.Term.WordQuantity).ToList();
        }
        private void ImportCompoundWordFromText(string[] contents, Audio audio, int i, string sentenceContent, System.Collections.Generic.Dictionary<int, System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>>> dictionary, System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> resultTermWords, System.Collections.Generic.Dictionary<string, Term> resultTerm, int maxText, System.Collections.Generic.Dictionary<string, bool> noneCharDictionary, string[] singleCharValidate, System.Collections.Generic.List<string> exceptionWordList, ref bool?[] noneCharContents, ref int wordIndex, ref int rowPosition, ref int position, ref int existCount)
        {
            //2023-11-11: Đổi vị trí từ là vị trí trong câu
            wordIndex = 0;
            for (int m = 0; m < contents.Length; m++)
            {
                //Bổ sung thêm đấu cách
                wordIndex += contents[m].Length + 1;
                rowPosition++;
                if (m >= contents.Length - maxText + 1)
                    continue;
                bool noneChar = false;
                for (int n = 0; n < maxText; n++)
                {
                    var curentIndex = m + n;
                    var word = Module.Helpers.TextHelper.RemoveStartEndSpecialCharacters(contents[curentIndex]);
                    if (noneCharContents[curentIndex] is null)
                    {
                        if (!noneCharDictionary.ContainsKey(word))
                        {
                            if (word.Length == 1)
                            {
                                if (!singleCharValidate.Contains(Module.Helpers.TextHelper.RemoveUnicode(word)))
                                {
                                    noneChar = true;
                                }
                            }
                            if (!noneChar)
                            {
                                foreach (var c in word)
                                {
                                    if (!char.IsLetter(c))
                                    {
                                        noneChar = true;
                                        break;
                                    }
                                }
                            }
                            if (!noneChar)
                            {
                                //Khi tìm từ ghép bỏ qua các từ đơn phi thuật
                                if (Module.Helpers.TextHelper.ListContains(exceptionWordList, word) > 0)
                                {
                                    noneChar = true;
                                    break;
                                }
                            }
                            noneCharDictionary.Add(word, noneChar);
                        }
                        else
                        {
                            noneChar = noneCharDictionary[word];
                        }

                        if (noneChar)
                        {
                            noneCharContents[curentIndex] = true;
                            if (n > 0 && maxText > 2)
                            {
                                //Reset lại từ vị trí này
                                var increase = n - 1;
                                m += increase;
                                for (int l = 0; l < increase; l++)
                                {
                                    if (m + l + 1 < contents.Length)
                                        wordIndex += contents[m + 1 + l].Length;
                                    else break;
                                }

                                rowPosition += increase;
                            }
                            break;
                        }
                        else
                        {
                            noneCharContents[curentIndex] = false;
                            //notCorrectContents[curentIndex] = !hunspell.Spell(word);
                        }

                    }
                    else if (noneCharContents[curentIndex] == true)
                    {
                        noneChar = true;
                        break;
                    }
                }
                if (!noneChar)
                {
                    //Nếu chỉ là chữ thì hợp lệ                                    
                    var text = Module.Helpers.TextHelper.RemoveStartEndSpecialCharacters(contents[m], true);
                    if (string.IsNullOrEmpty(text))
                        continue;
                    //Nếu cuối từ không phải ký tự hoặc số thì không xử lý  
                    if (!char.IsLetterOrDigit(text[text.Length - 1]))
                        continue;
                    bool textValidate = true;
                    for (int k = 1; k < maxText; k++)
                    {
                        var nextText = contents[m + k];
                        if (!char.IsLetterOrDigit(nextText[0]))
                        {
                            textValidate = false;
                            break;
                        }
                        if (!string.IsNullOrEmpty(text))
                            text += " ";
                        text += Module.Helpers.TextHelper.RemoveStartEndSpecialCharacters(nextText);
                    }
                    if (!textValidate)
                        continue;
                    var lowerText = text.ToLower();
                    if (resultTerm.ContainsKey(lowerText) && resultTerm[lowerText].Video != null)
                    {
                        //Nếu đã thuật ngữ thì bỏ qua, không xử lý
                        existCount++;
                        continue;
                    }
                    var key = Module.Helpers.TextHelper.RemoveUnicode(lowerText);
                    //Kiểm tra xem từ điển có từ này không
                    if (!dictionary[maxText].ContainsKey(key))
                        continue;
                    //string overlapText = null;
                    //string overlapLocationText = null;
                    bool overlap = false;
                    bool validate = true;
                    if (audio.TermLocationList != null && audio.TermLocationList.Count > 0)
                    {
                        //Kiểm tra xem có thuật vị nào trùng không
                        //var currentLocation = position + rowPosition;                                               
                        var currentSentence = i + 1;
                        if (audio.Video != null && audio.Start != null)
                        {
                            bool tFlag = false;
                            validate = VideoService.CheckAndUpdateLocationInTermIsValidate(audio.Video, text, audio, currentSentence, rowPosition, true, ref overlap, ref tFlag);
                        }
                        if (!validate)
                            continue;
                        #region Cấu trúc cũ

                        //var overlapList = IsOverlap(maxText, currentLocation, currentSentence, audio.Start, listTermLocations);
                        //if (overlapList != null && overlapList.Count > 0)
                        //{
                        //    foreach (var oldTermLocation in overlapList)
                        //    {
                        //        if (oldTermLocation.Term is null)
                        //            continue;
                        //        //Nếu IsCover(TVM, TVC) = True > Thuật vị nào Số từ lớn hơn sẽ win (mặc dù có thể có lỗi)
                        //        if (Module.Helpers.TextHelper.GetIndexWordInContent(text, oldTermLocation.Term.Name) >= 0 ||
                        //            Module.Helpers.TextHelper.GetIndexWordInContent(oldTermLocation.Term.Name, text) >= 0)
                        //        {
                        //            if (oldTermLocation.Term.WordQuantity >= maxText)
                        //            {
                        //                continue;
                        //            }
                        //            else if (!oldTermLocation.Term.IsDeleted)
                        //            {
                        //                //Thuật vị nào Số từ lớn hơn sẽ win 
                        //                var refterm = oldTermLocation.Term;
                        //                if (refterm.Quantity == 1)
                        //                {
                        //                    if (resultTerm.ContainsKey(refterm.Name))
                        //                        resultTerm.Remove(refterm.Name);
                        //                    if (resultTermWords.ContainsKey(refterm.Name))
                        //                        resultTermWords.Remove(refterm.Name);
                        //                    refterm.Delete();
                        //                }
                        //                else
                        //                {
                        //                    //Giảm số lượng;
                        //                    refterm.Quantity--;
                        //                }
                        //                oldTermLocation.Delete();
                        //            }
                        //        }
                        //        else
                        //        {
                        //            bool currentIsCorrect = Module.Helpers.TextHelper.ListContains(dictionary[maxText][key], text) >= 0;
                        //            var refKey = Module.Helpers.TextHelper.RemoveUnicode(oldTermLocation.Term.Name);
                        //            bool refIsCorrect = Module.Helpers.TextHelper.ListContains(dictionary[oldTermLocation.Term.WordQuantity.Value][refKey], oldTermLocation.Term.Name) >= 0;
                        //            if (currentIsCorrect == refIsCorrect)
                        //            {
                        //                //> TNM và TNC đều sai chính tả(VD: Ẩn Độ từ: Ấn độ và Độ tụ) > vẫn tạo cả 2 nhưng đánh dấu cờ Overlap cho cả 2 thuật vị để Người kiểm tra
                        //                //oldTermLocation.Term.Flag = true;
                        //                if (System.Diagnostics.Debugger.IsAttached)
                        //                {
                        //                    if (!string.IsNullOrEmpty(oldTermLocation.Term.Note))
                        //                        oldTermLocation.Term.Note += "; ";
                        //                    oldTermLocation.Term.Note += string.Format("Overlap({0})", text);

                        //                    if (!string.IsNullOrEmpty(overlapText))
                        //                        overlapText += ", ";
                        //                    overlapText += oldTermLocation.Term?.Name;
                        //                }

                        //                //Nạp vào trường overlap của thuật vị                                        
                        //                oldTermLocation.Overlap = true;
                        //            }
                        //            else if (refIsCorrect)
                        //            {
                        //                //> TNM sai chính tả, TNC đúng chính tả > không tạo TVM
                        //                continue;
                        //            }
                        //            else
                        //            {
                        //                // >TNM đúng chính tả, TNC sai chính tả > cướp thuật vị
                        //                var refterm = oldTermLocation.Term;
                        //                if (refterm.Quantity == 1)
                        //                {
                        //                    if (resultTerm.ContainsKey(refterm.Name))
                        //                        resultTerm.Remove(refterm.Name);
                        //                    if (resultTermWords.ContainsKey(refterm.Name))
                        //                        resultTermWords.Remove(refterm.Name);
                        //                    refterm.Delete();
                        //                }
                        //                else
                        //                {
                        //                    //Giảm số lượng;
                        //                    refterm.Quantity--;
                        //                }
                        //                oldTermLocation.Delete();
                        //            }
                        //        }
                        //    }

                        //}
                        #endregion

                    }

                    Term term = null;
                    if (!resultTerm.ContainsKey(lowerText))
                    {
                        term = CreateObject<Term>();
                        term.Name = lowerText;
                        term.Quantity = 1;
                        term.Overlap = overlap;
                        term.TermType = TermType.MergeTerm;
                        //term.Position = position + rowPosition;                                   
                        //video.TermList.Add(term);
                        resultTerm.Add(lowerText, term);
                        //term.NumberValue = dictionary[maxText][key].Count;
                        term.LikeWord = dictionary[maxText][key].Count;
                        var wordsList = new System.Collections.Generic.List<string>();
                        wordsList.Add(lowerText);
                        resultTermWords.Add(lowerText, wordsList);
                        //termNotCorrect.Add(text, false);
                    }
                    else
                    {
                        term = resultTerm[lowerText];
                        term.Quantity++;
                        if (!resultTermWords[lowerText].Contains(lowerText))
                        {
                            //var fd = resultTermWords[key];
                            if (term.NumberValue is null)
                                term.NumberValue = 1;
                            else
                                term.NumberValue++;
                            //resultTermWords[lowerText].Add(lowerText);
                            //if (term.LikeTerm is null)
                            //    term.LikeTerm = 1;
                            //else term.LikeTerm++;
                        }
                        if (overlap && !term.Overlap)
                            term.Overlap = overlap;
                    }


                    var childDic = dictionary[maxText][key];
                    bool flag = Module.Helpers.TextHelper.ListContains(childDic, lowerText) < 0;
                    //Dựng cờ nếu có thuật vị bị sai chính tả
                    if (flag && !term.Flag)
                    {
                        term.Flag = true;
                        term.Note = Module.Helpers.TextHelper.AddTextWithTagNode(term.Note, '(', "Sai chính tả");
                        //termNotCorrect[key] = flag;                                                
                    }
                    //2023-11-01: Cấu trúc cũ
                    //if (System.Diagnostics.Debugger.IsAttached && !string.IsNullOrEmpty(overlapText))
                    //{
                    //    var termOverlapText = string.Format("Overlap({0})", overlapText);
                    //    if (!string.IsNullOrEmpty(term.Note))
                    //    {
                    //        if (!term.Note.Contains(termOverlapText))
                    //        {
                    //            term.Note += "; " + termOverlapText;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        term.Note += termOverlapText;
                    //    }
                    //}

                    //084 Nạp từ ghép thì nạp Thuật vị luôn
                    //Thêm vào thuật vị 
                    //if (maxTermLocation >= 0 && term.TermLocationList.Count < maxTermLocation)
                    //{
                    //Xác định đúng vị trí
                    //var startIndex = (wordIndex - contents[m].Length - 1);   
                    //if(startIndex < 0)
                    //{

                    //}
                    //var startIndex = wordIndex - contents[m].Length;
                    //if(startIndex < 0 || startIndex>= sentenceContent.Length)
                    //{
                    //    //Lỗi
                    //    continue;
                    //}                    
                    //var indexLocation = sentenceContent.IndexOf(lowerText, startIndex, System.StringComparison.OrdinalIgnoreCase);
                    //if (indexLocation == 0)
                    //{
                    //    indexLocation = 1;
                    //}
                    //else if (indexLocation > 0)
                    //{
                    //    string beforeContent = sentenceContent.Substring(0, indexLocation);
                    //    indexLocation = beforeContent.Split(' ', System.StringSplitOptions.RemoveEmptyEntries).Length + 1;
                    //    //if (position > 0)
                    //    //{

                    //    //}
                    //}
                    //if (indexLocation < 0)
                    //{
                    //    //indexLocation = position + rowPosition;
                    //    indexLocation = rowPosition;
                    //}                     
                    var currentTermLocation = new TermLocation(term.Session)
                    {
                        Term = term,
                        //Location = indexLocation,
                        Location = m + 1,
                        Audio = audio,
                        Sentence = i + 1,
                        Flag = flag,
                        Overlap = overlap
                    };
                    //Cấu trúc cũ
                    //if (!string.IsNullOrEmpty(overlapLocationText))
                    //    currentTermLocation.Overlap = true;
                    term.TermLocationList.Add(currentTermLocation);

                    //}                                        
                    //if(position + rowPosition > 30)
                    //{

                    //}
                }
            }
        }

        private System.Collections.Generic.List<TermLocation> IsOverlap(int maxText, int currentLocation, int currentSentence, Audio currentElement, System.Collections.Generic.List<TermLocation> listTermLocations)
        {
            if (listTermLocations is null)
                return null;
            //Kiểm tra xem có thuật vị nào trùng không
            var result = new System.Collections.Generic.List<TermLocation>();
            foreach (var tl in listTermLocations)
            {
                if (tl.Audio == currentElement && tl.Sentence == currentSentence && tl.Term != null && tl.Location != null
                    //Trường hợp nested
                    //&& tl.Term.WordQuantity > maxText 
                    )
                {
                    if (currentLocation <= tl.Location)
                    {
                        if (tl.Location - currentLocation < maxText)
                        {
                            //Từ này bị trùng
                            result.Add(tl);
                            //var termName = tl.Term.Name;
                        }
                    }
                    else
                    {
                        if (currentLocation - tl.Location < tl.Term.WordQuantity)
                        {
                            //Từ này bị trùng
                            result.Add(tl);
                            //Debug
                            //var termName = tl.Term.Name;
                        }
                    }

                }
            }
            return result;
        }

        //private void ImportCompoundWord(Video video, int maxText, System.Collections.Generic.List<string> exceptionWordList)
        //{
        //    System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
        //    stopWatch.Start();
        //    int maxTermLocation = Module.Helpers.ParameterHelper.GetIntOrDefault(ObjectSpace, "MaxTermLocationWhenImport", 1);
        //    string aff = "\\\\rd\\CodeGen\\packages\\Dictionaries\\DictionaryAffVN.aff";
        //    string dic = "\\\\rd\\CodeGen\\packages\\Dictionaries\\DictionaryVN.dic";

        //    using (NHunspell.Hunspell hunspell = new NHunspell.Hunspell(aff, dic))
        //    {
        //        var dictionariesText = Module.Helpers.ParameterHelper.GetValueOrDefault(ObjectSpace, "ViDictionaries", "THPT,THCS");
        //        if (!string.IsNullOrEmpty(dictionariesText))
        //        {
        //            var dictionaries = dictionariesText.Split(',');
        //            foreach (var dictionary in dictionaries)
        //                hunspell.Add(dictionary.Trim());
        //        }
        //        string[] singleCharValidate = new string[] { "e", "y", "u", "i", "o", "a" };
        //        var resultTerm = new System.Collections.Generic.Dictionary<string, Term>();
        //        var termNotCorrect = new System.Collections.Generic.Dictionary<string, bool>();
        //        var resultTermWords = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>>();
        //        decimal countNumber = 0;
        //        int total = video.AudioList.Count;
        //        foreach (var audio in video.AudioList)
        //        {
        //            if (string.IsNullOrEmpty(audio.Content))
        //                continue;
        //            //int position = 0;

        //            //Kiểm tra xem thuật ngữ có sẵn thì loại
        //            //var audioContent = Module.Helpers.TextHelper.RemoveUnicode(audio.Content);
        //            var audioContent = audio.Content.Replace("  ", " ").Trim();
        //            var rows = audioContent.Split(Module.Helpers.TextHelper.NewLineText, System.StringSplitOptions.RemoveEmptyEntries);
        //            int wordIndex = 0;
        //            for (int i = 0; i < rows.Count(); i++)
        //            {
        //                //Tách position và rowPosition để tránh trường hợp từ thay thế khác từ hiện tại
        //                int rowPosition = 0;
        //                var childContentArray = rows[i].Split(Tools.BeforeChars, System.StringSplitOptions.RemoveEmptyEntries);
        //                foreach (var childContentText in childContentArray)
        //                {
        //                    var contents = childContentText.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
        //                    bool?[] noneCharContents = new bool?[contents.Length];
        //                    //bool?[] notCorrectContents = new bool?[contents.Length];
        //                    for (int m = 0; m < contents.Length; m++)
        //                    {
        //                        wordIndex += contents[m].Length;
        //                        rowPosition++;
        //                        if (m >= contents.Length - maxText + 1)
        //                            continue;
        //                        bool noneChar = false;
        //                        for (int n = 0; n < maxText; n++)
        //                        {
        //                            var curentIndex = m + n;
        //                            var word = contents[curentIndex];
        //                            if (noneCharContents[curentIndex] is null)
        //                            {
        //                                if (word.Length == 1)
        //                                {
        //                                    if (!singleCharValidate.Contains(Module.Helpers.TextHelper.RemoveUnicode(word)))
        //                                    {
        //                                        noneChar = true;
        //                                    }
        //                                }
        //                                if (!noneChar)
        //                                {
        //                                    foreach (var c in word)
        //                                    {
        //                                        if (!char.IsLetter(c))
        //                                        {
        //                                            noneChar = true;
        //                                            break;
        //                                        }
        //                                    }
        //                                }
        //                                if (!noneChar)
        //                                {
        //                                    //Khi tìm từ ghép bỏ qua các từ đơn phi thuật
        //                                    if (Module.Helpers.TextHelper.ListContains(exceptionWordList, word) > 0)
        //                                    {
        //                                        noneChar = true;
        //                                        break;
        //                                    }
        //                                }
        //                                if (noneChar)
        //                                {
        //                                    noneCharContents[curentIndex] = true;
        //                                    if (n > 0 && maxText > 2)
        //                                    {
        //                                        //Reset lại từ vị trí này
        //                                        var increase = n - 1;
        //                                        m += increase;
        //                                        for (int l = 0; l < increase; l++)
        //                                            wordIndex += contents[m + 1 + l].Length;
        //                                        rowPosition += increase;
        //                                    }
        //                                    break;
        //                                }
        //                                else
        //                                {
        //                                    noneCharContents[curentIndex] = false;
        //                                    //notCorrectContents[curentIndex] = !hunspell.Spell(word);
        //                                }

        //                            }
        //                            else if (noneCharContents[curentIndex] == true)
        //                            {
        //                                noneChar = true;
        //                                break;
        //                            }
        //                        }
        //                        if (!noneChar)
        //                        {
        //                            //Nếu chỉ là chữ thì hợp lệ                                    
        //                            var text = contents[m] + " ";
        //                            if (maxText == 2)
        //                                text += Module.Helpers.TextHelper.RemoveStartEndSpecialCharacters(contents[m + 1]);
        //                            else if (maxText == 3)
        //                                text += contents[m + 1] + " " + Module.Helpers.TextHelper.RemoveStartEndSpecialCharacters(contents[m + 2]);
        //                            text = text.ToLower();
        //                            var key = Module.Helpers.TextHelper.RemoveUnicode(text);
        //                            Term term = null;
        //                            if (!resultTerm.ContainsKey(key))
        //                            {
        //                                term = ObjectSpace.CreateObject<Term>();
        //                                term.Name = key;
        //                                term.Quantity = 1;
        //                                //term.Position = position + rowPosition;                                   
        //                                //video.TermList.Add(term);
        //                                resultTerm.Add(key, term);
        //                                term.NumberValue = 1;
        //                                var wordsList = new System.Collections.Generic.List<string>();
        //                                wordsList.Add(text);
        //                                resultTermWords.Add(key, wordsList);
        //                                termNotCorrect.Add(key, false);
        //                            }
        //                            else
        //                            {
        //                                term = resultTerm[key];
        //                                term.Quantity++;
        //                                if (!resultTermWords[key].Contains(text))
        //                                {
        //                                    //var fd = resultTermWords[key];
        //                                    term.NumberValue++;
        //                                    resultTermWords[key].Add(text);
        //                                }
        //                            }
        //                            bool flag = false;
        //                            var words = text.Split(' ');
        //                            foreach (var w in words)
        //                            {
        //                                if (!hunspell.Spell(w))
        //                                {
        //                                    flag = true;
        //                                    break;
        //                                }
        //                            }
        //                            //Dựng cờ nếu có thuật vị bị sai chính tả
        //                            if (flag && !term.Flag)
        //                            {
        //                                term.Flag = true;
        //                                termNotCorrect[key] = flag;
        //                            }

        //                            //084 Nạp từ ghép thì nạp Thuật vị luôn
        //                            //Thêm vào thuật vị 
        //                            //if (maxTermLocation >= 0 && term.TermLocationList.Count < maxTermLocation)
        //                            //{
        //                            //Xác định đúng vị trí
        //                            //var startIndex = (wordIndex - contents[m].Length - 1);   
        //                            //if(startIndex < 0)
        //                            //{

        //                            //}                                        
        //                            var indexLocation = audioContent.IndexOf(text, (wordIndex - contents[m].Length), System.StringComparison.OrdinalIgnoreCase);
        //                            if (indexLocation > 0)
        //                            {
        //                                string beforeContent = audioContent.Substring(0, indexLocation);
        //                                indexLocation = beforeContent.Split(' ', System.StringSplitOptions.RemoveEmptyEntries).Length + 1;
        //                                //if (position > 0)
        //                                //{

        //                                //}
        //                            }
        //                            if (indexLocation < 0)
        //                            {
        //                                indexLocation = position + rowPosition;
        //                            }
        //                            term.TermLocationList.Add(new TermLocation(term.Session)
        //                            {
        //                                Term = term,
        //                                Location = position + rowPosition,
        //                                Element = audio.Start,
        //                                Sentence = i + 1,
        //                                Flag = flag
        //                            });
        //                            //}                                        
        //                            //if(position + rowPosition > 30)
        //                            //{

        //                            //}
        //                        }
        //                    }
        //                    //Tăng thêm 1 ký tự ngăn cách
        //                    wordIndex++;
        //                    //rowPosition++;                           
        //                }

        //                position += rows[i].Split(' ', System.StringSplitOptions.RemoveEmptyEntries).Length;
        //            }
        //            if (total > 5 && countNumber < total)
        //            {
        //                countNumber++;
        //                Tools.ShowOrCloseDefaultWaitForm((countNumber / total).ToString("p0"), "Đang kiểm tra", stopWatch.Elapsed);
        //            }
        //        }
        //        countNumber = 0;
        //        total = resultTerm.Keys.Count;
        //        foreach (var key in resultTerm.Keys)
        //        {
        //            //Nếu từ ít hơn thì bỏ, mặc định là 2
        //            //Nếu mà thuật ngữ không sai chính tả thì không hiện
        //            int compoundWordRecurrence = video.CompoundWordRecurrence != null ? video.CompoundWordRecurrence.Value : 2;
        //            if (!termNotCorrect[key] || (resultTerm[key].Quantity != null && compoundWordRecurrence > resultTerm[key].Quantity))
        //            {
        //                resultTerm[key].Session.Delete(resultTerm[key].TermLocationList);
        //                resultTerm[key].Delete();
        //            }
        //            else
        //            {
        //                resultTerm[key].Video = video;
        //            }
        //            countNumber++;
        //            Tools.ShowOrCloseDefaultWaitForm((countNumber / total).ToString("p0"), "Đang nạp", stopWatch.Elapsed);
        //        }
        //        stopWatch.Stop();
        //        Tools.ShowOrCloseDefaultWaitForm(null);
        //    }
        //}

        private int AddWordToTerm(string word, int sentencePosition, int rowPosition, int add,
            Video video, Audio audio,
            System.Collections.Generic.IDictionary<string, Term> resultTerm,
            System.Collections.Generic.IDictionary<string, int> resultQuantity,
            bool upperCase, TermType? defaultTermType = null)
        {
            if (string.IsNullOrEmpty(word))
                return add;

            bool overlap = false;
            bool validate = true;
            bool flag = false;

            var currentLocation = rowPosition + 1;
            var currentSentence = sentencePosition + 1;
            if (audio.TermLocationList != null && audio.TermLocationList.Count > 0)
                //validate = resultTerm[lowerWord].CheckLocationIsValidate(listTermLocations, audio.Start.Value, currentSentence, currentLocation, ref overlap, video, e.SelectedChoiceActionItem.Id.Equals("UpperCase"));                         
                validate = VideoService.CheckAndUpdateLocationInTermIsValidate(video, word, audio, currentSentence, currentLocation, true, ref overlap, ref flag, upperCase);
            //Kiểm tra nếu từ này không hợp lệ thì không bổ sung thêm
            if (!validate)
                return add;
            var lowerWord = word.ToLower();
            if (!resultTerm.ContainsKey(lowerWord))
            {
                resultQuantity.Add(lowerWord, 1);
                //Tạo mới thuật ngữ và thuật vị
                //Nếu tồn tại thì bỏ qua
                //Chức năng chữ hoa cần hiệu chỉnh:                             
                //-tất cả các chứ cái hoa đơn lẻ
                //2023-06-01
                //Trường "Loại thuật ngữ" được gán giá trị
                //-Số: từ đơn chứa số
                //-Tắt: từ đơn viết hoa hết, kí tự lẻ
                //- Hoa: từ đơn Hoa hoặc từ phức
                //-Phi thuật: khi Nạp nội dung, so khớp với danh sách Phi thuật
                //-Từ điên: Khi thuật ngữ sinh ra trong chức năng Tra từ điển
                //- Gộp : Thuật ngữ sinh ra trong chức năng Gộp thuật ngữ
                add++;
                var term = CreateObject<Term>();
                term.Name = word;
                term.Quantity = resultQuantity[lowerWord];
                if (flag)
                {
                    term.Flag = true;
                    term.AddTextNode(importCharTag, "Lẫn hoa thường");
                }
                term.Overlap = overlap;
                if (defaultTermType != null)
                {
                    term.TermType = defaultTermType.Value;
                    if (term.TermType == TermType.Number)
                        term.NumberValue = term.GetDefaultNumberValue();
                }
                else if (!upperCase)
                {
                    //Nạp số thì loại luôn là số
                    term.TermType = TermType.Number;
                    term.NumberValue = term.GetDefaultNumberValue();
                }
                else
                {
                    UpdateTermType(term, true);
                }

                //video.TermList.Add(term);
                resultTerm.Add(lowerWord, term);
                //Thêm vào thuật vị
                if (!upperCase)
                {
                    //Nếu UpperCase sẽ cập nhật thuật vị sau
                    if (video.WithTermPosition)
                    {
                        //Mặc định thêm 1;
                        var newLocation = new TermLocation(term.Session)
                        {
                            Term = term,
                            //Vị trí của mảng nhỏ hơn 1 so với vị trí thực tế
                            //Location = position + rowPosition + 1,
                            //Vị trí là vị trí trong câu
                            Location = currentLocation,
                            Audio = audio,
                            Sentence = currentSentence,
                            Overlap = overlap
                        };
                        term.TermLocationList.Add(newLocation);
                    }
                }

            }
            else if (!upperCase)
            {
                //Nếu UpperCase sẽ cập nhật thuật vị sau
                resultQuantity[lowerWord] += 1;
                var term = resultTerm[lowerWord];
                if (term.Video == null)
                {
                    if (video.WithTermPosition)
                    {
                        //Kiểm tra xem có overlap không
                        //- XétTermPositionRelation(TVi, TVk) trong đó TVi là 1 thuật vị của TN, TVk là 1 trong các thuật vị đang tồn tại và cùng câu với TVi
                        //+Nếu = 1 thì phụ thuộc TN(TVi) và TN(TVk) chỉ có 1 bên sai chính tả sẽ bị loại, còn cả 2 sai và cả 2 đúng thì tạo cả 2 và dựng cờ overlap(để xem xét)
                        //+Nếu = 2 thì hủy TVi
                        //+ Nếu = 3 thì hủy TVk và tạo TVi
                        //+Nếu = 4 thì tạo TVi
                        //Mỗi khi Hủy(xóa) 1 thuật vị cần kiểm tra số lượng TV của TN tương ứng, nếu = 0 thì sẽ xóa TN

                        if (validate)
                        {
                            var newLocation = new TermLocation(term.Session)
                            {
                                Term = term,
                                //Location = position + rowPosition + 1,
                                //Vị trí là vị trí trong câu
                                Location = currentLocation,
                                Audio = audio,
                                Sentence = currentSentence,
                                Overlap = overlap
                            };
                            term.TermLocationList.Add(newLocation);
                            term.Quantity++;
                        }

                    }
                    else
                    {
                        //Trường hợp đã tồn tại
                        term.Quantity++;
                        //Tạo mới trong thuật vị
                        //var firstContent = content.Substring(0, i);
                        //var rowPosition = firstContent.Split(' ', System.StringSplitOptions.RemoveEmptyEntries).Length;
                        //Nếu số lượng nhỏ hơn số lượng tối đa
                    }

                }
                if (overlap && !term.Overlap)
                    term.Overlap = overlap;
            }
            return add;
        }




        #endregion SourceCode4514ImportCode

        #region SourceCode4545ImportCode
                        public void UpdateTermType(Term term,bool onlyUpperCase = false)
        {
            //Code: 1136            Oid: 05489077-81fa-418e-8054-52e73782b5d3
            // 2023 - 06 - 01: Số được coi là chữ hoa nên 4K sẽ là từ viết tắt, khi đó 3G SDI là cụm viết hoa
            //2023-06-05
            //Nạp viết hoa: xác định Loại thuật ngữ chỉnh lại như sau
            //- Số và ký tự(cũ là Chữ số): Số thuần túy không dính kí tự(bao gồm cả thập phân 1.1 hay 1, 1), bảng chữ cái(các ngôn ngữ: cần thì lập table)
            //- Viết tắt: từ đơn toàn hoa +Từ chứa cả số và toàn kí tự hoa(3G, 4G)
            //-Viết hoa: như cũ +Từ chứa cả số và kí tự thường(v3.0)
            if (string.IsNullOrEmpty(term.Name))
                return;
            if (!onlyUpperCase && term.Name.Length == 1)
            {
                //Nếu là 1 ký tự thì loại là số và ký tự
                term.TermType = TermType.Number;
                if (char.IsNumber(term.Name[0]))
                    term.NumberValue = System.Int32.Parse(term.Name);
            }
            //else if (Name.Contains(' '))
            //{
            //    Nếu có nhiều từ mặc định là viết hoa
            //        term.TermType = TermType.UpperCase;
            //    //2023-08-09 Chức năng Nạp kí tự và số: sẽ trích xuất giá trị số và lưu vào trường Trị số,
            //    //nếu có 2 hoặc nhiều giá trị số thì chọn giá trị đầu từ trái sang phải
            //    var result = Tools.TryConvertTextToNumber(word);
            //    if (result != null)
            //        term.NumberValue = result;
            //}
            else
            {
                //- Số và ký tự(cũ là Chữ số): Số thuần túy không dính kí tự(bao gồm cả thập phân 1.1 hay 1, 1), bảng chữ cái(các ngôn ngữ: cần thì lập table)
                //Quy đổi phân số dạng a/b về số thập phân để lưu vào Trị số
                bool hasForwardSlash = false;
                bool isNumber = false;
                if (!onlyUpperCase)
                {
                    foreach (var c in term.Name)
                    {
                        if (c == '/')
                        {
                            //Xử lý trường hợp ngày
                            hasForwardSlash = !hasForwardSlash;
                            //hasForwardSlash = true;
                            continue;
                        }
                        if (char.IsNumber(c))
                        {
                            isNumber = true;
                        }
                        else if (c != '.' && c != ',')
                        {
                            isNumber = false;
                            break;
                        }
                    }
                }
                if (isNumber)
                {
                    if (hasForwardSlash)
                    {
                        //Quy đổi phân số dạng a/b về số thập phân để lưu vào Trị số
                        var numberValue = new System.Data.DataTable().Compute(term.Name, null);
                        if (numberValue != null)
                        {
                            term.NumberValue = System.Convert.ToDecimal(numberValue);
                        }
                    }
                    else
                    {
                        term.TermType = TermType.Number;
                        term.NumberValue = Tools.ConvertTextToNumber(term.Name);
                    }

                }
                else
                {
                    //2023-08-09 Chức năng Nạp kí tự và số: sẽ trích xuất giá trị số và lưu vào trường Trị số,
                    //nếu có 2 hoặc nhiều giá trị số thì chọn giá trị đầu từ trái sang phải
                    bool hasNumber = false;
                    var result = Tools.TryConvertTextToNumber(term.Name);
                    if (result != null)
                    {
                        hasNumber = true;
                        term.NumberValue = result;
                    }
                    //2023-06-02 Kiểm tra xem nếu toàn bộ là viết hoa
                    //2023-06-05 Viết tắt: từ đơn toàn hoa +Từ chứa cả số và toàn kí tự hoa(3G, 4G)
                    //091: -Khi nạp viết hoa: Loại từ được phân 3 loại: Đầu hoa, Toàn hoa, Viết tắt
                    //    +Toàn hoa: Toàn bộ các ký tự là viết hoa hoặc chữ số, số từ > 1
                    //    + Viết tắt: Toàn bộ ký tự là hoa hoặc chữ số, số từ = 1
                    //    + Đầu hoa: Toàn bộ các từ trong cụm có kí tự đầu là hoa hoặc số, số từ 1 hoặc nhiều
                    //var upperText = term.Name.ToUpper();
                    ////2024-06-18: Chat riêng: Lẫn chữ và số nếu số đầu thì là số
                    //if (char.IsNumber(Name[0]))
                    //    TermType = TermType.Number;
                    //else if (Name != upperText)
                    //    TermType = TermType.UpperCase;

                    //else if (Name.Split(' ', System.StringSplitOptions.RemoveEmptyEntries).Length > 1)
                    //    TermType = TermType.UpperCaseAll;
                    //else
                    //    TermType = TermType.Short;
                    //2025-02-12: CV004
                    //Nạp menu nào phân loại theo menu đấy
                    //Nạp Từ hoa cần phân loại chi tiết: đầu hoa, toàn hoa, viết tắt
                    //Số coi là 1 ký tự hoa: 4k, 1000kg > đầu hoa, 4K > toàn hoa
                    //Đã là nạp hoa bắt buộc kí tự đầu là số hoặc chữ hoa: p2000 > không phải nên sẽ nạp là thuật ngữ đơn và cần chuyển sang Phi thuật, viết tắt, hay số
                    //Số và ký tự: 1 kí tự hoặc: nhiều kí tự số hoặc dấu chấm hoặc phẩy
                    var upperText = term.Name.ToUpper();
                    var wordsLength = term.Name.Split(' ', System.StringSplitOptions.RemoveEmptyEntries).Length;
                    if (term.Name.Equals(upperText))
                    {
                        //Cứ từ đơn mà viết hoa tất có số hay không số thì là viết tắt
                        if (wordsLength == 1
                            //&& hasNumber
                            )
                        {
                            term.TermType = TermType.Short;
                        }
                        else
                        {
                            term.TermType = TermType.UpperCaseAll;
                        }
                    }
                    else if (onlyUpperCase || Module.Helpers.TextHelper.CheckRealNameIsUpperCaseFirstAll(term.Name))
                    {
                        term.TermType = TermType.UpperCase;
                    }
                }
            }
        }


        public bool CheckTermNameIsCorrectAndFlag(Term term, System.Collections.Generic.List<string> childDic = null, bool checkFlag = true)
        {
            bool flag = true;
            if (!string.IsNullOrEmpty(term.Name))
            {
                var dictionary = childDic;
                var lowerText = term.Name.ToLower();

                if (childDic is null && term.Video != null)
                {
                    var rootDictionary = term.Video.GetDictionary();
                    if (rootDictionary != null)
                    {
                        var wordLength = term.Name.Split(' ', System.StringSplitOptions.RemoveEmptyEntries).Length;
                        var key = Module.Helpers.TextHelper.RemoveUnicode(lowerText);
                        if (rootDictionary[wordLength].ContainsKey(key))
                            dictionary = rootDictionary[wordLength][key];
                    }
                }
                if (dictionary != null)
                {
                    flag = Module.Helpers.TextHelper.ListContains(dictionary, lowerText) < 0;
                }
                //Dựng cờ nếu có thuật vị bị sai chính tả
                if (flag)
                {
                    if (!term.Flag)
                        term.Flag = true;
                    term.Note = Module.Helpers.TextHelper.AddTextWithTagNode(term.Note, '(', "Sai chính tả");
                    //termNotCorrect[key] = flag;                                                
                }
            }
            return !flag;
        }

       

       

        public void UpdateGoogleTranslate(Term term)
        {
            if (term.TermLocationList?.Count > 0 && string.IsNullOrEmpty(term.GoogleTranslate))
            {
                var termLocationList = term.TermLocationList.Where(n => !string.IsNullOrEmpty(n.MachineTranslate)).Select(x => x.MachineTranslate).Distinct();
                if (termLocationList.Count() == 0)
                {
                    //Dựng cờ
                    term.Flag = true;
                    return;
                }

                ////Xóa trắng dịch máy 
                //GoogleTranslate = null;
                int max = 0;
                string maxKey = null;
                foreach (var key in termLocationList)
                {
                    int total = term.TermLocationList.Where(n => key.Equals(n.MachineTranslate, StringComparison.OrdinalIgnoreCase)).Count();
                    if (string.IsNullOrEmpty(maxKey))
                    {
                        maxKey = key;
                        max = total;
                    }
                    else if (total > max)
                    {
                        maxKey = key;
                        max = total;
                    }

                }
                //Chỉ lưu vào dịch máy
                if (!string.IsNullOrEmpty(maxKey))
                {
                    term.GoogleTranslate = maxKey;
                }
            }
        }


        //public void AddTextNode(string text)
        //{
        //    //Flag = true;
        //    string separatorString = "; ";
        //    if (!string.IsNullOrEmpty(Note))
        //    {
        //        //Kiểm tra nếu tồn tại rồi thì bỏ qua
        //        var nodeActions = Note.Split(separatorString, StringSplitOptions.RemoveEmptyEntries);
        //        bool validate = true;
        //        foreach (var nodeAction in nodeActions)
        //        {
        //            if (nodeAction.Equals(text))
        //            {
        //                validate = false;
        //                break;
        //            }
        //        }
        //        if (!validate)
        //            return;
        //        Note += separatorString;
        //    }
        //    Note += text;
        //}

        //public void RemoveFlagOverlap()
        //{
        //    if (!string.IsNullOrEmpty(Note))
        //    {
        //        Note = Note.Replace("; Kiểm tra đè", "").Replace("Kiểm tra đè", "").Replace("; Cờ đè thuật vị", "").Replace("Cờ đè thuật vị", "").Replace("; Đè nhau", "").Replace("Đè nhau", "");
        //    }

        //}

        public static Term FindTermByName(Term term, string name)
        {
            //foreach (var term in Video?.TermList)
            //{
            //    if (!term.Oid.Equals(Oid) && !string.IsNullOrEmpty(term.Name) && term.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
            //    {
            //        return term;
            //    }
            //}

            return VideoService.GetTermsByLength(term.Video, name.Length)
                .FirstOrDefault(m => !m.Oid.Equals(term.Oid) && !string.IsNullOrEmpty(m.Name)
                && m.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        
        public System.Collections.Generic.List<Audio> GetAudioListByOrder(Term term, bool asc = true)
        {
            if (term.Video != null)
            {
                if (asc)
                    return term.Video.GetAudioListWithSort(asc);
                else
                    return term.Video.GetAudioListWithSort(asc);
            }
            return null;
        }
        public static System.Collections.Generic.List<string> GetParrentTerms(Term term)
        {
            var parrentTerms = new System.Collections.Generic.List<string>();
            foreach (Term otherTerm in term.Video.TermList)
            {
                if (otherTerm.Oid.Equals(term.Oid) || string.IsNullOrEmpty(otherTerm.Name))
                    continue;
                if (otherTerm.CheckTermInTerm(term))
                {
                    // Nếu term khác có chứa select thì term khác phải giảm số lượng                                                      
                    parrentTerms.Add(otherTerm.Name);
                }

            }
            return parrentTerms;
        }

        public System.Collections.Generic.List<TermLocation> GetTermLocationsByOrder(Term term, bool asc = true)
        {
            if (asc)
                return term.TermLocationList.Where(n => n.Audio != null).OrderBy(m => m.Audio.Start).ThenBy(m => m.Sentence).ThenBy(m => m.Location).ToList();
            else
                return term.TermLocationList.Where(n => n.Audio != null).OrderByDescending(m => m.Audio.Start).ThenByDescending(m => m.Sentence).ThenByDescending(m => m.Location).ToList();
        }
        public static int GetRealIndexBySpace(string content, int termIndex)
        {
            var resultIndex = termIndex;
            for (int t = termIndex - 1; t >= 0; t--)
            {
                //var c = sentencesArray[m][t];
                if (content[t] == ' ')
                {
                    resultIndex = t + 1;
                    break;
                }
            }
            if (termIndex - resultIndex > 1)
            {
                //Test kiểm tra
            }
            return resultIndex;
        }
        public void UpdatePositionLocation(Term term, bool requireTerm, char charTag, bool byName = false, bool exactSentence = true)
        {
            //Test
            //byName = true;
            var termLocationListSort = GetTermLocationsByOrder(term);
            System.Collections.Generic.List<string> parrentTerms = null;
            if (byName)
                parrentTerms = GetParrentTerms(term);
            var termLocationUpdatedDictionary = new System.Collections.Generic.Dictionary<Guid, System.Collections.Generic.List<TermLocation>>();
            for (int i = 0; i < termLocationListSort.Count; i++)
            {
                if (string.IsNullOrEmpty(termLocationListSort[i].Audio?.Content)) continue;
                if (!termLocationUpdatedDictionary.ContainsKey(termLocationListSort[i].Audio.Oid))
                {
                    termLocationUpdatedDictionary.Add(termLocationListSort[i].Audio.Oid, new System.Collections.Generic.List<TermLocation>());
                }
                TermLocationService.UpdatePositionLocation(termLocationListSort[i], requireTerm, false, exactSentence, parrentTerms, termLocationUpdatedDictionary[termLocationListSort[i].Audio.Oid]);

            }
        }
        TermLocationService termLocationService;

        //public bool CheckLocationIsValidate(System.Collections.Generic.List<TermLocation> termLocationList, Audio currentElement, int currentSentence, int currentPosition, ref bool overlap, Video video = null, bool upperCase = false)
        //{
        //    bool validate = true;
        //    var relation1TermLocationList = new System.Collections.Generic.List<TermLocation>();
        //    var relation2TermLocationList = new System.Collections.Generic.List<TermLocation>();
        //    int wordLength = term.Name.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
        //    foreach (var relationTermLocation in termLocationList)
        //    {
        //        var relation = relationTermLocation.TermPositionRelation(currentElement, currentSentence, currentPosition, term.Name);
        //        if (relation == 0)
        //        {
        //            //Lỗi
        //            if (relationTermLocation.Term != null && !string.IsNullOrEmpty(relationTermLocation.Term.Name) &&
        //                (Name.Contains(relationTermLocation.Term.Name) || relationTermLocation.Term.Name.Contains(Name)))
        //            {
        //                //var f = this.Oid;
        //                validate = false;
        //                break;
        //            }
        //            else
        //            {
        //                var audio = relationTermLocation.GetAudioFromElement().Content;
        //            }
        //        }
        //        else if (relation == 1)
        //        {
        //            //1- TV1 overlap TV2
        //            relation1TermLocationList.Add(relationTermLocation);
        //        }
        //        else if (relation == 2)
        //        {
        //            //2- TV1 belong (thuộc về) TV2
        //            //Thuật ngữ này thuộc về thuật ngữ cần so sánh
        //            relation2TermLocationList.Add(relationTermLocation);
        //        }
        //        else if (relation == 3)
        //        {
        //            //3 - TV2 belong (thuộc về) TV1
        //            //Thuật ngữ so sánh thuộc về thuật ngữ này
        //            //Nạp viết hoa: Cho phép nạp Overlap(1) và belong TV2(2) đều dụng cờ overlap,
        //            //nếu thuật vị trùng thì đổi Loại thuật ngữ thành Viết hoa / Toàn hoa(tồn tại cả viết thường thì dựng cờ: Lẫn hoa thường)
        //            if (upperCase)
        //            {
        //                overlap = true;
        //                relationTermLocation.Overlap = true;
        //            }
        //            else
        //            {
        //                validate = false;
        //                break;
        //            }
        //        }
        //    }
        //    //validate = Module.Helpers.TextHelper.CheckCurrentIndexIsNotParentIndex(content, term.Name, termIndex, parrentTerms.ToArray());
        //    if (validate)
        //    {
        //        if (relation1TermLocationList.Count > 0)
        //        {
        //            var refVideo = video != null ? video : Video;
        //            if (refVideo != null && refVideo.CheckSpelling && refVideo.GetDictionary(null) != null)
        //            {
        //                var relationOverlapListTermLocationList = new System.Collections.Generic.List<TermLocation>();
        //                var relationRemoveListTermLocationList = new System.Collections.Generic.List<TermLocation>();
        //                foreach (var relationTermLocation in relation1TermLocationList)
        //                {
        //                    var dictionary = refVideo.GetDictionary(null);
        //                    //+Nếu = 1 thì phụ thuộc TN(TVi) và TN(TVk) chỉ có 1 bên sai chính tả sẽ bị loại, còn cả 2 sai và cả 2 đúng thì tạo cả 2 và dựng cờ overlap(để xem xét)
        //                    var lowerName = Module.Helpers.TextHelper.RemoveUnicode(Name).ToLower();
        //                    bool currentIsCorrect = dictionary.ContainsKey(wordLength);
        //                    if (currentIsCorrect)
        //                        currentIsCorrect = dictionary[wordLength].ContainsKey(lowerName);
        //                    if (currentIsCorrect)
        //                        currentIsCorrect = Module.Helpers.TextHelper.ListContains(dictionary[wordLength][lowerName], term.Name) >= 0;
        //                    var refIsCorrect = relationTermLocation.Term != null && !string.IsNullOrEmpty(relationTermLocation.Term.Name);
        //                    if (refIsCorrect)
        //                    {
        //                        var reflowerName = Module.Helpers.TextHelper.RemoveUnicode(relationTermLocation.Term.Name).ToLower();
        //                        var refWordLength = relationTermLocation.Term.Name.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
        //                        refIsCorrect = dictionary.ContainsKey(refWordLength);
        //                        if (refIsCorrect)
        //                            refIsCorrect = dictionary[refWordLength].ContainsKey(reflowerName);
        //                        if (refIsCorrect)
        //                            refIsCorrect = Module.Helpers.TextHelper.ListContains(dictionary[refWordLength][reflowerName], relationTermLocation.Term.Name) >= 0;
        //                    }
        //                    if (currentIsCorrect == refIsCorrect)
        //                    {
        //                        overlap = true;
        //                        relationOverlapListTermLocationList.Add(relationTermLocation);
        //                    }
        //                    else if (currentIsCorrect)
        //                    {
        //                        relationRemoveListTermLocationList.Add(relationTermLocation);

        //                    }
        //                    else if (refIsCorrect)
        //                    {
        //                        //Thuật ngữ này không hợp lệ
        //                        validate = false;
        //                        break;
        //                    }
        //                }
        //                if (validate)
        //                {
        //                    foreach (var relationTermLocation in relationOverlapListTermLocationList)
        //                    {
        //                        relationTermLocation.Overlap = true;
        //                    }
        //                    foreach (var relationTermLocation in relationRemoveListTermLocationList)
        //                    {
        //                        //Loại bỏ thuật ngữ tham chiếu                                                
        //                        if (relationTermLocation.Term.Quantity == 1)
        //                        {
        //                            relationTermLocation.Term.Quantity--;
        //                        }
        //                        else
        //                        {
        //                            relationTermLocation.Term.Quantity = relationTermLocation.Term.TermLocationList.Count - 1;
        //                        }
        //                        relationTermLocation.Delete();
        //                    }
        //                }

        //            }
        //            else
        //            {
        //                //dựng cờ overlap(để xem xét)
        //                overlap = true;
        //                foreach (var relationTermLocation in relation1TermLocationList)
        //                {
        //                    relationTermLocation.Overlap = true;
        //                }
        //            }
        //        }
        //        if (validate && relation2TermLocationList.Count > 0)
        //        {
        //            foreach (var relationTermLocation in relation2TermLocationList)
        //            {
        //                //Thuật ngữ tham chiếu thuộc về thuật ngữ này
        //                if (relationTermLocation.Term.Quantity == 1)
        //                {
        //                    relationTermLocation.Term.Quantity--;
        //                }
        //                else
        //                {
        //                    relationTermLocation.Term.Quantity = relationTermLocation.Term.TermLocationList.Count - 1;
        //                }
        //                relationTermLocation.Delete();
        //            }
        //        }
        //    }
        //    return validate;
        //}

        public int? UpdateTermPosition(Term term, bool requireTerm, char charTag, bool updateFlag = true, int? maxTermLocation = null)
        {
            //Khi gộp liền kề Thuật ngữ thì giữ nguyên các giá trị trường theo từ gốc (cờ, loại, từ loại, phi thuật)
            if (term.Video != null && term.Video.AudioList != null && term.Video.AudioList.Count > 0 && !string.IsNullOrEmpty(term.Name))
            {
                //Nạp tham số
                if (maxTermLocation is null)
                {
                    Parameter maxTermLocationParameter = Module.Helpers.ParameterHelper.GetParameterValueOrDefault(term.Session, "MaxTermLocation", "5");
                    maxTermLocation = System.Convert.ToInt32(maxTermLocationParameter.Value);
                }

                //2023-06-07: dấu ngắt câu có thể là: Xuống dòng, dấu chấm, ?, !
                //string[] newLineText = new string[] { ". ", "?", "!", "\r\n" };
                //Tạo danh sách thuật ngữ con nằm trong thuật ngữ cha
                //var parrentTerms = GetParrentTerms();
                //Đổi cập nhật
                term.SetDefaultUpdate();
                //Tìm vị trí
                //2023-06-17: Cập nhật Thuật vị không áp dụng cho Thuật ngữ SL 1
                //if(term.Quantity == 1)
                //    continue;
                //Xóa vị trí cũ
                term.Session.Delete(term.TermLocationList);
                //term.Position = null;
                term.Quantity = 0;
                //2023-06-19 Khi nạp chữ hoa mà có từ hoa lại có chỗ từ thường thì dựng cờ để kiểm tra và quyết định hoa hay thường
                bool currentFlag = false;

                var audioList = GetAudioListByOrder(term, true);

                foreach (var audio in audioList)
                {
                    if (string.IsNullOrEmpty(audio.Content))
                        continue;
                    //int position = 0;
                    //Cắt theo dòng                    
                    var sentencesArray = Module.Helpers.TextHelper.GetSentences(audio.Content);
                    for (int m = 0; m < sentencesArray.Count(); m++)
                    {
                        //var wordsArray = Module.Helpers.TextHelper.GetWords(sentencesArray[m]);
                        ////var contents = rows[m].Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
                        ////int rowPosition = 0;
                        //for (int n = 0; n <= wordsArray.Length - wordLength; n++)
                        //{

                        //}
                        var content = sentencesArray[m].Trim();
                        int startIndex = 0;
                        //2023-07-25 bỏ yêu cầu này
                        //Nếu là phi thuật thì phải tìm đúng từ
                        //int termIndex = content.IndexOf(Name, NoneTerm ? System.StringComparison.Ordinal : System.StringComparison.OrdinalIgnoreCase);
                        int termIndex = content.IndexOf(term.Name, System.StringComparison.OrdinalIgnoreCase);
                        while (termIndex >= 0)
                        {
                            bool validate = true;
                            int currentPosition = -1;
                            //Kiểm tra xem trước đấy có phải dấu trắng hoặc ký tự đặc biệt không
                            if (termIndex >= 1 && char.IsLetterOrDigit(content[termIndex - 1]))
                                validate = false;
                            var currentElement = audio;
                            var currentSentence = m + 1;
                            bool overlap = false;
                            //Kiểm tra sau đấy có phải dấu trắng hoặc ký tự đặc biệt không
                            startIndex = termIndex + term.Name.Length;
                            if (validate && !Module.Helpers.TextHelper.CheckWordIndexIsValidateInContent(content, term.Name, termIndex))
                            {
                                validate = false;
                            }
                            if (validate)
                            {
                                //UpdateTermPosition(TN) trả về số lượng Thuật vị hợp lệ được tạo ra
                                //- XétTermPositionRelation(TVi, TVk) trong đó TVi là 1 thuật vị của TN, TVk là 1 trong các thuật vị đang tồn tại và cùng câu với TVi
                                //+Nếu = 1 thì phụ thuộc TN(TVi) và TN(TVk) chỉ có 1 bên sai chính tả sẽ bị loại, còn cả 2 sai và cả 2 đúng thì tạo cả 2 và dựng cờ overlap(để xem xét)
                                //+Nếu = 2 thì hủy TVi
                                //+ Nếu = 3 thì hủy TVk và tạo TVi
                                //+Nếu = 4 thì tạo TVi
                                //Mỗi khi Hủy(xóa) 1 thuật vị cần kiểm tra số lượng TV của TN tương ứng, nếu = 0 thì sẽ xóa TN
                                //Tìm đến vị trí dấu cách trước đó
                                int termpTermIndex = GetRealIndexBySpace(content, termIndex);
                                string beforeContent = content.Substring(0, termpTermIndex);
                                //beforeContent = Module.Helpers.TextHelper.RemoveStartEndSpecialCharacters(beforeContent, false, new char[] { ' ' });
                                currentPosition = beforeContent.Split(' ', System.StringSplitOptions.RemoveEmptyEntries).Length + 1;

                                //validate = CheckLocationIsValidate(relationTermLocationList.ToList(), currentElement, currentSentence, currentPosition, ref overlap);                                                                
                                validate = VideoService.CheckAndUpdateLocationInTermIsValidate(term.Video, term.Name, currentElement, currentSentence, currentPosition, requireTerm, ref overlap, ref currentFlag);
                            }
                            if (validate)
                            {
                                term.Quantity++;
                                //Cập nhật vị trí
                                //string beforeContent = sentencesArray[m].Substring(0, termIndex);
                                //beforeContent = Module.Helpers.TextHelper.RemoveStartEndSpecialCharacters(beforeContent, false, new char[' ']);
                                //Thêm 1 là thêm vị trí hiện tại
                                //var currentPosition = position + beforeContent.Split(' ', System.StringSplitOptions.RemoveEmptyEntries).Length + 1;
                                //var currentPosition = beforeContent.Split(' ', System.StringSplitOptions.RemoveEmptyEntries).Length + 1;
                                //2023-06-23: Bỏ vị trí
                                //if (term.Position is null)
                                //    term.Position = currentPosition;
                                //Cập nhật thuật vị
                                if (maxTermLocation <= 0 || term.TermLocationList.Count < maxTermLocation)
                                {
                                    term.TermLocationList.Add(new TermLocation(term.Session)
                                    {
                                        Term = term,
                                        Location = currentPosition,
                                        Audio = audio,
                                        Sentence = m + 1,
                                        Overlap = overlap
                                    });
                                }
                                //2023-06-19
                                //Khi nạp chữ hoa mà có từ hoa lại có chỗ từ thường thì dựng cờ để kiểm tra và quyết định hoa hay thường
                                //Trường hợp đầu dòng thì không kiểm tra

                                if (updateFlag && !currentFlag && termIndex > 0)
                                {
                                    string currentTermName = content.Substring(termIndex, term.Name.Length);
                                    if (!term.Name.Equals(currentTermName))
                                    {
                                        currentFlag = true;
                                        if (!string.IsNullOrEmpty(term.Note))
                                        {
                                            //Xóa ghi chú tag trước đó
                                            term.Note = Module.Helpers.TextHelper.GetTextWithTagNode(term.Note, charTag, false);
                                        }
                                        term.Note = Module.Helpers.TextHelper.AddTextWithTagNode(term.Note, charTag, "Chữ hoa hoặc chữ thường");
                                    }

                                }

                            }
                            //content = content.Substring(afterIndex);
                            termIndex = content.IndexOf(term.Name, startIndex, System.StringComparison.OrdinalIgnoreCase);
                        }
                        //position += sentencesArray[m].Split(' ', System.StringSplitOptions.RemoveEmptyEntries).Length;
                    }

                }
                //Xóa thuật vị số lượng = 0;
                if (term.Quantity is null || term.Quantity == 0)
                {
                    term.Delete();
                    return 0;
                }
                if (updateFlag)
                    term.Flag = currentFlag;
            }
            return term.Quantity;
        }


        public int? UpdatePosition(Term term, bool requireTerm, bool updateFlag = true, int? maxTermLocation = null, bool byName = false, char charTag = '(', bool useSubtitle = false)
        {
            if (term.Video != null && term.Video.AudioList != null && term.Video.AudioList.Count > 0 && !string.IsNullOrEmpty(term.Name))
            {
                if (maxTermLocation is null)
                {
                    Parameter maxTermLocationParameter = Module.Helpers.ParameterHelper.GetParameterValueOrDefault(term.Session, "MaxTermLocation", "5");
                    maxTermLocation = Convert.ToInt32(maxTermLocationParameter.Value);
                }

                List<string> parrentTerms = null;
                if (byName)
                    parrentTerms = GetParrentTerms(term);

                term.SetDefaultUpdate();

                term.Session.Delete(term.TermLocationList);
                term.Quantity = 0;

                if (updateFlag && term.Flag)
                    term.Flag = false;

                var audioList = GetAudioListByOrder(term, true);
                foreach (var audio in audioList)
                {
                    // ✅ Lấy nội dung từ Subtitle hay Content tùy theo cờ
                    var fullContent = useSubtitle ? audio.Subtitle : audio.Content;
                    if (string.IsNullOrEmpty(fullContent))
                        continue;

                    var sentencesArray = Module.Helpers.TextHelper.GetSentences(fullContent);
                    for (int m = 0; m < sentencesArray.Length; m++)
                    {
                        var content = sentencesArray[m].Trim();
                        int startIndex = 0;

                        int termIndex = content.IndexOf(term.Name, StringComparison.OrdinalIgnoreCase);
                        while (termIndex >= 0)
                        {
                            bool overlap = false;
                            bool flag = false;
                            bool validate = true;

                            if (termIndex >= 1 && char.IsLetterOrDigit(content[termIndex - 1]))
                                validate = false;

                            startIndex = termIndex + term.Name.Length;

                            if (validate && !Module.Helpers.TextHelper.CheckWordIndexIsValidateInContent(content, term.Name, termIndex))
                                validate = false;

                            if (validate && parrentTerms != null && parrentTerms.Count > 0)
                                validate = Module.Helpers.TextHelper.CheckCurrentIndexIsNotParentIndex(content, term.Name, termIndex, parrentTerms.ToArray());

                            if (validate)
                            {
                                int termpTermIndex = GetRealIndexBySpace(content, termIndex);
                                string beforeContent = content.Substring(0, termpTermIndex);

                                var currentPosition = beforeContent.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length + 1;
                                var currentSentence = m + 1;

                                if (validate && !byName && audio.TermLocationList != null && audio.TermLocationList.Count > 0)
                                {
                                    validate = VideoService.CheckAndUpdateLocationInTermIsValidate(term.Video, term.Name, audio, currentSentence, currentPosition, requireTerm, ref overlap, ref flag);
                                }

                                if (validate)
                                {
                                    term.Quantity++;
                                    if (maxTermLocation <= 0 || term.TermLocationList.Count < maxTermLocation)
                                    {
                                        term.TermLocationList.Add(new TermLocation(term.Session)
                                        {
                                            Term = term,
                                            Location = currentPosition,
                                            Audio = audio,
                                            Sentence = currentSentence,
                                            Overlap = overlap
                                        });
                                    }

                                    if (updateFlag && !term.Flag && termIndex > 0)
                                    {
                                        string currentTermName = content.Substring(termIndex, term.Name.Length);
                                        if (!term.Name.Equals(currentTermName))
                                        {
                                            term.Flag = true;
                                            if (!string.IsNullOrEmpty(term.Note))
                                                term.Note = Module.Helpers.TextHelper.GetTextWithTagNode(term.Note, charTag, false);
                                            term.Note = Module.Helpers.TextHelper.AddTextWithTagNode(term.Note, charTag, "Chữ hoa hoặc chữ thường");
                                        }
                                    }

                                    if (flag && !term.Flag)
                                        term.Flag = flag;
                                    if (!term.Overlap && overlap)
                                        term.Overlap = overlap;
                                }
                            }

                            termIndex = content.IndexOf(term.Name, startIndex, StringComparison.OrdinalIgnoreCase);
                        }
                    }
                }

                if (term.Quantity is null || term.Quantity == 0)
                {
                    term.Delete();
                    return 0;
                }
            }

            return term.Quantity;
        }

        public void TranslateTerm(Term term, ref int susscess, ref int termLocationCount, string option = "TranslateTermContextApostrophe", char charTag = '{')
        {
            //Mặc định là dịch theo dấu nháy
            //-Ngữ cảnh: Tìm từ chung trong các câu bên Dịch nội dung ứng với Thuật vị,
            //2023-06-29 So sánh dịch theo google translate 2 lần
            string seperateKey = "TranslateTermContextSlash".Equals(option) ? "/" : "'";
            //seperateKey = "'";

            term.Flag = false;
            if (term.TermLocationList.Count == 0)
                return;
            termLocationCount += term.TermLocationList.Count;
            if (!string.IsNullOrEmpty(term.Name) && term.TermLocationList != null && term.TermLocationList.Count >= 1
                && (string.IsNullOrEmpty(term.Translate) || string.IsNullOrEmpty(term.GoogleTranslate)))
            {
                System.Collections.Generic.IDictionary<string, int> dictionaryResult = new System.Collections.Generic.Dictionary<string, int>();
                foreach (var termLocation in term.TermLocationList)
                {
                    // 2023 - 07 - 27: Bỏ: Cờ thuật vị chỉ cần dùng trong Thay từ
                    //termLocation.Flag = false;
                    bool termLocationFlag = false;
                    var audio = termLocation.GetAudioFromElement();
                    if (audio is null)
                        continue;
                    if (string.IsNullOrEmpty(audio.Subtitle) || string.IsNullOrEmpty(audio.Content))
                        continue;
                    if (!string.IsNullOrEmpty(termLocation.MachineTranslate))
                        continue;

                    string newTranlate = null;
                    foreach (var key in dictionaryResult.Keys)
                    {
                        var index = Module.Helpers.TextHelper.GetIndexWordInContent(key, audio.Subtitle);
                        if (index >= 0)
                        {
                            newTranlate = audio.Subtitle.Substring(index, key.Length);
                        }
                    }
                    if (string.IsNullOrEmpty(newTranlate))
                    {
                        string newContent = "";
                        var firstIndex = 0;
                        var content = audio.Content.Replace(seperateKey, " ");
                        var index = content.IndexOf(term.Name, System.StringComparison.OrdinalIgnoreCase);
                        while (index >= 0)
                        {
                            newContent += content.Substring(firstIndex, index - firstIndex);
                            firstIndex = index + term.Name.Length;

                            if (Module.Helpers.TextHelper.CheckWordIndexIsValidateInContent(content, term.Name, index))
                            {
                                newContent += seperateKey + content.Substring(index, term.Name.Length) + seperateKey;
                            }
                            else
                            {
                                newContent += content.Substring(index, term.Name.Length);
                            }
                            if (firstIndex >= content.Length)
                                break;
                            index = content.IndexOf(term.Name, firstIndex, System.StringComparison.OrdinalIgnoreCase);
                        }
                        newContent += content.Substring(firstIndex);
                        var newtranlateContent = Tools.TranslateText(newContent);
                        if (string.IsNullOrEmpty(newtranlateContent))
                            continue;
                        int startIndex = newtranlateContent.IndexOf(seperateKey, System.StringComparison.OrdinalIgnoreCase);
                        int endIndex = newtranlateContent.IndexOf(seperateKey, startIndex + 1, System.StringComparison.OrdinalIgnoreCase);
                        if (startIndex < endIndex && (startIndex >= 0 || endIndex > 0))
                        {
                            //Nếu tìm thấy từ viết hoa
                            if (startIndex < 0)
                                startIndex = 0;
                            if (endIndex < 0)
                                endIndex = newtranlateContent.Length;
                            newTranlate = newtranlateContent.Substring(startIndex + 1, endIndex - startIndex - 1);
                            int newStartIndex = audio.Subtitle.IndexOf(newTranlate, System.StringComparison.OrdinalIgnoreCase);
                            if (newStartIndex < 0)
                            {
                                //Từ được dịch không hợp lệ
                                //2023-07-17: Dùng thử tính năng so sánh 2 câu                                       
                                if (startIndex > 0)
                                {
                                    var firstText = newtranlateContent.Substring(0, startIndex).Trim();
                                    startIndex = audio.Subtitle.IndexOf(firstText, System.StringComparison.OrdinalIgnoreCase);
                                    if (startIndex < 0)
                                    {
                                        //Fix trường hợp tìm nhiều lần không thấy
                                        while (startIndex < 0)
                                        {
                                            var spaceIndex = firstText.IndexOf(' ');
                                            if (spaceIndex > 0)
                                                firstText = firstText.Substring(spaceIndex + 1).Trim();
                                            else
                                                break;
                                            if (string.IsNullOrEmpty(firstText))
                                                break;
                                            startIndex = audio.Subtitle.IndexOf(firstText, System.StringComparison.OrdinalIgnoreCase);
                                        }
                                    }
                                    if (startIndex >= 0)
                                        startIndex += firstText.Length;

                                }
                                if (endIndex < newtranlateContent.Length)
                                {
                                    var endText = newtranlateContent.Substring(endIndex + 1);
                                    //Nếu câu có 2 từ trùng nhau thì chỉ lấy từ đầu tiên
                                    var afterIndex = endText.IndexOf(seperateKey, System.StringComparison.OrdinalIgnoreCase);
                                    if (afterIndex > 0)
                                        endText = endText.Substring(0, afterIndex).Trim();
                                    endIndex = audio.Subtitle.IndexOf(endText, System.StringComparison.OrdinalIgnoreCase);
                                    if (endIndex < 0)
                                    {
                                        //Fix trường hợp tìm nhiều lần không thấy
                                        while (endIndex < 0)
                                        {
                                            var spaceIndex = endText.LastIndexOf(' ');
                                            if (spaceIndex > 0)
                                                endText = endText.Substring(0, spaceIndex).Trim();
                                            else
                                                break;
                                            if (string.IsNullOrEmpty(endText) || startIndex < 0 || startIndex >= endText.Length)
                                                break;
                                            endIndex = audio.Subtitle.IndexOf(endText, startIndex, System.StringComparison.OrdinalIgnoreCase);
                                        }
                                    }
                                }
                                if (startIndex < endIndex && startIndex >= 0)
                                {
                                    newTranlate = audio.Subtitle.Substring(startIndex, endIndex - startIndex);
                                    newTranlate = Module.Helpers.TextHelper.RemoveStartEndSpecialCharacters(newTranlate);
                                    if (!string.IsNullOrEmpty(newTranlate))
                                        newTranlate = newTranlate.Trim();
                                }
                                else
                                {
                                    //Fix trường hợp có 2 từ thì không quan tâm trước sau
                                    //Từ gốc:                preview 'indication'
                                    //Từ sau khi thêm nháy:  xem trước 'chỉ định'
                                    //Từ được dịch cả câu: dấu hiệu xem trước
                                    newTranlate = null;
                                    if (content.Split(' ').Length == 2)
                                    {
                                        startIndex = newtranlateContent.IndexOf(seperateKey, System.StringComparison.OrdinalIgnoreCase);
                                        endIndex = newtranlateContent.IndexOf(seperateKey, startIndex + 1, System.StringComparison.OrdinalIgnoreCase);
                                        if (startIndex < endIndex && startIndex >= 0)
                                        {
                                            string otherText = "";
                                            if (startIndex == 0)
                                            {
                                                otherText = newtranlateContent.Substring(endIndex).Trim();
                                                newTranlate = audio.Subtitle.Replace(otherText, "").Trim();
                                            }
                                            else if (endIndex == newtranlateContent.Length - 1)
                                            {
                                                otherText = newtranlateContent.Substring(0, startIndex).Trim();
                                                newTranlate = audio.Subtitle.Replace(otherText, "").Trim();
                                            }

                                        }
                                    }

                                }

                            }
                            else
                            {
                                newTranlate = audio.Subtitle.Substring(newStartIndex, newTranlate.Length);
                            }
                        }
                        else
                        {
                            term.Flag = true;
                            if (!string.IsNullOrEmpty(term.Note))
                            {
                                //Xóa ghi chú tag trước đó
                                term.Note = Module.Helpers.TextHelper.GetTextWithTagNode(term.Note, charTag, false);
                            }
                            term.Note = Module.Helpers.TextHelper.AddTextWithTagNode(term.Note, charTag, "Không tìm thấy dịch ngữ cảnh");

                        }

                        if (string.IsNullOrEmpty(newTranlate))
                        {
                            //Dịch thử thông thường
                            var gTranslate = Tools.TranslateText(term.Name);
                            int newStartIndex = audio.Subtitle.IndexOf(gTranslate, System.StringComparison.OrdinalIgnoreCase);
                            if (newStartIndex >= 0)
                            {
                                newTranlate = gTranslate;
                                newTranlate = audio.Subtitle.Substring(newStartIndex, newTranlate.Length);
                            }
                            else
                            {
                                //Nếu giữ nguyên từ gốc
                                if (Module.Helpers.TextHelper.GetIndexWordInContent(term.Name, audio.Subtitle) >= 0)
                                    newTranlate = term.Name;
                                //2023-07-27: Bỏ : Cờ thuật vị chỉ cần dùng trong Thay từ
                                //termLocation.Flag = true;
                                termLocationFlag = true;
                                //if (System.Diagnostics.Debugger.IsAttached)
                                //    termLocation.Translate = newtranlateContent;
                            }
                        }
                        if (System.Diagnostics.Debugger.IsAttached)
                        {
                            if (string.IsNullOrEmpty(newTranlate))
                                _notificationService.Notify( "Debug: ",
                                    newtranlateContent + System.Environment.NewLine + audio.Subtitle, InformationType.Warning, 10000);
                        }
                    }
                    if (!string.IsNullOrEmpty(newTranlate))
                    {
                        susscess++;
                        newTranlate = newTranlate.Trim();
                        if (newTranlate.Equals(term.Name, System.StringComparison.OrdinalIgnoreCase))
                            newTranlate = term.Name;
                        termLocation.MachineTranslate = newTranlate;
                        //2023 - Khi dịch máy manual trên Thuật ngữ hoặc Thuật vị sẽ xác định bằng tìm kiếm nếu kết quả thấy 1 thì cập nhật
                        //  , 0 thấy hoặc 2 trở lên thì phải cập nhật Vị trí dịch bằng manual
                        int count = 0;
                        var index = -1;
                        while (true)
                        {
                            var newIndex = Module.Helpers.TextHelper.GetIndexWordInContent(newTranlate, audio.Subtitle, null, index + 1);
                            if (newIndex < 0)
                                break;
                            index = newIndex;
                            count++;
                        }
                        if (count == 1)
                        {
                            var firstText = audio.Subtitle.Substring(0, index);
                            var rows = firstText.Split(Module.Helpers.TextHelper.NewLineText, System.StringSplitOptions.RemoveEmptyEntries);
                            int position = 0;
                            for (int m = 0; m < rows.Count(); m++)
                            {
                                var contents = rows[m].Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
                                //Vị trí của mảng nhỏ hơn 1 so với vị trí thực tế, nên vị trí của từ cũng là vị trí của mảng
                                position += contents.Length;
                            }
                            //Tổng số lượng trong mảng mảng nhỏ hơn 1 so với vị trí thực tế
                            termLocation.TranslateLocation = position + 1;
                        }

                        //2023-07-27: Bỏ : Cờ thuật vị chỉ cần dùng trong Thay từ
                        //termLocation.Flag = audio.Subtitle.IndexOf(newTranlate, System.StringComparison.OrdinalIgnoreCase) < 0;
                        if (!termLocationFlag)
                            termLocationFlag = audio.Subtitle.IndexOf(newTranlate, System.StringComparison.OrdinalIgnoreCase) < 0;
                        var key = Module.Helpers.TextHelper.KeyListContains(dictionaryResult.Keys, newTranlate);
                        if (string.IsNullOrEmpty(key))
                        {
                            var newTranlates = Tools.TranslateText(term.Name);
                            dictionaryResult.Add(newTranlate, termLocationFlag ? 0 : 1);
                        }
                        else if (!termLocationFlag)
                        {
                            dictionaryResult[key]++;
                        }
                    }
                    else
                    {
                        //Nếu không tìm thấy thuật ngữ thì dựng cờ
                        // 2023 - 07 - 27: Bỏ: Cờ thuật vị chỉ cần dùng trong Thay từ
                        //termLocation.Flag = true;
                        term.Flag = true;
                        if (!string.IsNullOrEmpty(term.Note))
                        {
                            //Xóa ghi chú tag trước đó
                            term.Note = Module.Helpers.TextHelper.GetTextWithTagNode(term.Note, charTag, false);
                        }
                        term.Note = Module.Helpers.TextHelper.AddTextWithTagNode(term.Note, charTag, "Không tìm thấy dịch ngữ cảnh");
                    }
                }
                if (dictionaryResult.Keys.Count > 0)
                {
                    ////Xóa trắng dịch máy 
                    //GoogleTranslate = null;
                    int max = 0;
                    string maxKey = null;
                    foreach (var key in dictionaryResult.Keys)
                    {
                        if (dictionaryResult[key] == 0)
                        {
                            term.Flag = true;

                            if (!string.IsNullOrEmpty(term.Note))
                            {
                                //Xóa ghi chú tag trước đó
                                term.Note = Module.Helpers.TextHelper.GetTextWithTagNode(term.Note, charTag, false);
                            }
                            term.Note = Module.Helpers.TextHelper.AddTextWithTagNode(term.Note, charTag, "Không tìm thấy dịch ngữ cảnh");

                        }
                        else
                        {
                            if (string.IsNullOrEmpty(maxKey))
                                maxKey = key;
                        }
                        if (dictionaryResult[key] > max)
                        {
                            max = dictionaryResult[key];
                            maxKey = key;
                        }
                    }
                    //Chỉ lưu vào dịch máy
                    if (!string.IsNullOrEmpty(maxKey))
                    {
                        term.GoogleTranslate = maxKey;
                    }
                }
            }
        }



        public void TranslateTermContext(ref int susscess, ref int termLocationCount, XafApplication application = null)
        {
            //Flag = false;
            //if (TermLocationList.Count == 0)
            //    return;
            //termLocationCount += TermLocationList.Count;
            //if (!string.IsNullOrEmpty(Name) && TermLocationList != null && TermLocationList.Count >= 1
            //    && (string.IsNullOrEmpty(Translate) || string.IsNullOrEmpty(GoogleTranslate)))
            //{
            //    System.Collections.Generic.IDictionary<string, int> dictionaryResult = new System.Collections.Generic.Dictionary<string, int>();
            //    foreach (var termLocation in TermLocationList)
            //    {
            //        // 2023 - 07 - 27: Bỏ: Cờ thuật vị chỉ cần dùng trong Thay từ
            //        //termLocation.Flag = false;
            //        bool termLocationFlag = false;
            //        var audio = termLocation.GetAudioFromElement();
            //        if (audio is null)
            //            continue;
            //        if (string.IsNullOrEmpty(audio.Subtitle) || string.IsNullOrEmpty(audio.Content))
            //            continue;
            //        if (!string.IsNullOrEmpty(termLocation.MachineTranslate))
            //            continue;                    
            //        string newTranlate = null;
            //        foreach (var key in dictionaryResult.Keys)
            //        {
            //            var index = Module.Helpers.TextHelper.GetIndexWordInContent(key, audio.Subtitle);
            //            if (index >= 0)
            //            {
            //                newTranlate = audio.Subtitle.Substring(index, key.Length);
            //            }
            //        }
            //        if (string.IsNullOrEmpty(newTranlate))
            //        {
            //            string newContentUpcase = "";
            //            string newContentSlash = "";
            //            string newContentApostrophe = "";
            //            var firstIndex = 0;
            //            var contentUpcase = audio.Content.ToLower();
            //            var contentSlash = audio.Content.Replace("/", " ");
            //            var contentApostrophe = audio.Content.Replace("'", " ");
            //            var index = audio.Content.IndexOf(Name, System.StringComparison.OrdinalIgnoreCase);
            //            while (index >= 0)
            //            {
            //                newContentUpcase += contentUpcase.Substring(firstIndex, index - firstIndex);
            //                newContentSlash += contentSlash.Substring(firstIndex, index - firstIndex);
            //                newContentApostrophe += contentApostrophe.Substring(firstIndex, index - firstIndex);

            //                firstIndex = index + term.Name.Length;
            //                if (Module.Helpers.TextHelper.CheckWordIndexIsValidateInContent(audio.Content, term.Name, index))
            //                {                                
            //                    newContentUpcase += term.Name.ToUpper();
            //                    newContentSlash += "/" + contentSlash.Substring(index, term.Name.Length) + "/";
            //                    newContentApostrophe += "'" + contentApostrophe.Substring(index, term.Name.Length) + "'";
            //                }
            //                else
            //                {                                
            //                    newContentUpcase += contentUpcase.Substring(index, term.Name.Length);
            //                    newContentSlash += "/" + contentSlash.Substring(index, term.Name.Length);
            //                    newContentApostrophe += "'" + contentApostrophe.Substring(index, term.Name.Length);
            //                }
            //                if (firstIndex >= audio.Content.Length)
            //                    break;
            //                index = audio.Content.IndexOf(Name, firstIndex, System.StringComparison.OrdinalIgnoreCase);
            //            }

            //            newContentUpcase += newContentUpcase.Substring(firstIndex);
            //            newContentSlash += newContentSlash.Substring(firstIndex);
            //            newContentApostrophe += newContentApostrophe.Substring(firstIndex);

            //            var newtranlateContentUpcase = Tools.TranslateText(newContentUpcase);
            //            var newtranlateContentSlash = Tools.TranslateText(newContentSlash);
            //            var newtranlateContentApostrophe = Tools.TranslateText(newContentApostrophe);
            //            if (string.IsNullOrEmpty(newtranlateContentUpcase) && string.IsNullOrEmpty(newtranlateContentSlash) && string.IsNullOrEmpty(newtranlateContentApostrophe))
            //                continue;
            //            //int startIndex = newtranlateContent.IndexOf(seperateKey, System.StringComparison.OrdinalIgnoreCase);
            //            //int endIndex = newtranlateContent.IndexOf(seperateKey, startIndex + 1, System.StringComparison.OrdinalIgnoreCase);
            //            //if (startIndex < endIndex && (startIndex >= 0 || endIndex > 0))
            //            //{
            //            //    //Nếu tìm thấy từ viết hoa
            //            //    if (startIndex < 0)
            //            //        startIndex = 0;
            //            //    if (endIndex < 0)
            //            //        endIndex = newtranlateContent.Length;
            //            //    newTranlate = newtranlateContent.Substring(startIndex + 1, endIndex - startIndex - 1);
            //            //    int newStartIndex = audio.Subtitle.IndexOf(newTranlate, System.StringComparison.OrdinalIgnoreCase);
            //            //    if (newStartIndex < 0)
            //            //    {
            //            //        //Từ được dịch không hợp lệ
            //            //        //2023-07-17: Dùng thử tính năng so sánh 2 câu                                       
            //            //        if (startIndex > 0)
            //            //        {
            //            //            var firstText = newtranlateContent.Substring(0, startIndex).Trim();
            //            //            startIndex = audio.Subtitle.IndexOf(firstText, System.StringComparison.OrdinalIgnoreCase);
            //            //            if (startIndex < 0)
            //            //            {
            //            //                //Fix trường hợp tìm nhiều lần không thấy
            //            //                while (startIndex < 0)
            //            //                {
            //            //                    var spaceIndex = firstText.IndexOf(' ');
            //            //                    if (spaceIndex > 0)
            //            //                        firstText = firstText.Substring(spaceIndex + 1).Trim();
            //            //                    else
            //            //                        break;
            //            //                    if (string.IsNullOrEmpty(firstText))
            //            //                        break;
            //            //                    startIndex = audio.Subtitle.IndexOf(firstText, System.StringComparison.OrdinalIgnoreCase);
            //            //                }
            //            //            }
            //            //            if (startIndex >= 0)
            //            //                startIndex += firstText.Length;

            //            //        }
            //            //        if (endIndex < newtranlateContent.Length)
            //            //        {
            //            //            var endText = newtranlateContent.Substring(endIndex + 1);
            //            //            //Nếu câu có 2 từ trùng nhau thì chỉ lấy từ đầu tiên
            //            //            var afterIndex = endText.IndexOf(seperateKey, System.StringComparison.OrdinalIgnoreCase);
            //            //            if (afterIndex > 0)
            //            //                endText = endText.Substring(0, afterIndex).Trim();
            //            //            endIndex = audio.Subtitle.IndexOf(endText, System.StringComparison.OrdinalIgnoreCase);
            //            //            if (endIndex < 0)
            //            //            {
            //            //                //Fix trường hợp tìm nhiều lần không thấy
            //            //                while (endIndex < 0)
            //            //                {
            //            //                    var spaceIndex = endText.LastIndexOf(' ');
            //            //                    if (spaceIndex > 0)
            //            //                        endText = endText.Substring(0, spaceIndex).Trim();
            //            //                    else
            //            //                        break;
            //            //                    if (string.IsNullOrEmpty(endText) || startIndex < 0 || startIndex >= endText.Length)
            //            //                        break;
            //            //                    endIndex = audio.Subtitle.IndexOf(endText, startIndex, System.StringComparison.OrdinalIgnoreCase);
            //            //                }
            //            //            }
            //            //        }
            //            //        if (startIndex < endIndex && startIndex >= 0)
            //            //        {
            //            //            newTranlate = audio.Subtitle.Substring(startIndex, endIndex - startIndex);
            //            //            newTranlate = Module.Helpers.TextHelper.RemoveStartEndSpecialCharacters(newTranlate);
            //            //            if (!string.IsNullOrEmpty(newTranlate))
            //            //                newTranlate = newTranlate.Trim();
            //            //        }
            //            //        else
            //            //        {
            //            //            //Fix trường hợp có 2 từ thì không quan tâm trước sau
            //            //            //Từ gốc:                preview 'indication'
            //            //            //Từ sau khi thêm nháy:  xem trước 'chỉ định'
            //            //            //Từ được dịch cả câu: dấu hiệu xem trước
            //            //            newTranlate = null;
            //            //            if (content.Split(' ').Length == 2)
            //            //            {
            //            //                startIndex = newtranlateContent.IndexOf(seperateKey, System.StringComparison.OrdinalIgnoreCase);
            //            //                endIndex = newtranlateContent.IndexOf(seperateKey, startIndex + 1, System.StringComparison.OrdinalIgnoreCase);
            //            //                if (startIndex < endIndex && startIndex >= 0)
            //            //                {
            //            //                    string otherText = "";
            //            //                    if (startIndex == 0)
            //            //                    {
            //            //                        otherText = newtranlateContent.Substring(endIndex).Trim();
            //            //                        newTranlate = audio.Subtitle.Replace(otherText, "").Trim();
            //            //                    }
            //            //                    else if (endIndex == newtranlateContent.Length - 1)
            //            //                    {
            //            //                        otherText = newtranlateContent.Substring(0, startIndex).Trim();
            //            //                        newTranlate = audio.Subtitle.Replace(otherText, "").Trim();
            //            //                    }

            //            //                }
            //            //            }

            //            //        }

            //            //    }
            //            //    else
            //            //    {
            //            //        newTranlate = audio.Subtitle.Substring(newStartIndex, newTranlate.Length);
            //            //    }
            //            }
            //            else
            //            {
            //                Flag = true;
            //            }

            //            if (string.IsNullOrEmpty(newTranlate))
            //            {
            //                //Dịch thử thông thường
            //                var gTranslate = Tools.TranslateText(Name);
            //                int newStartIndex = audio.Subtitle.IndexOf(gTranslate, System.StringComparison.OrdinalIgnoreCase);
            //                if (newStartIndex >= 0)
            //                {
            //                    newTranlate = gTranslate;
            //                    newTranlate = audio.Subtitle.Substring(newStartIndex, newTranlate.Length);
            //                }
            //                else
            //                {
            //                    //Nếu giữ nguyên từ gốc
            //                    if (Module.Helpers.TextHelper.GetIndexWordInContent(Name, audio.Subtitle) >= 0)
            //                        newTranlate = term.Name;
            //                    //2023-07-27: Bỏ : Cờ thuật vị chỉ cần dùng trong Thay từ
            //                    //termLocation.Flag = true;
            //                    termLocationFlag = true;
            //                    //if (System.Diagnostics.Debugger.IsAttached)
            //                    //    termLocation.Translate = newtranlateContent;
            //                }
            //            }
            //            if (System.Diagnostics.Debugger.IsAttached)
            //            {
            //                //if (string.IsNullOrEmpty(newTranlate) && application != null)
            //                //    Module.Helpers.XafXpoHelper.ShowMessage(application, "Debug: ",
            //                //        newtranlateContent + System.Environment.NewLine + audio.Subtitle, InformationType.Warning, 10000);
            //            }
            //        //}
            //        //if (!string.IsNullOrEmpty(newTranlate))
            //        //{
            //        //    susscess++;
            //        //    newTranlate = newTranlate.Trim();
            //        //    if (newTranlate.Equals(Name, System.StringComparison.OrdinalIgnoreCase))
            //        //        newTranlate = term.Name;
            //        //    termLocation.MachineTranslate = newTranlate;
            //        //    //2023-07-27: Bỏ : Cờ thuật vị chỉ cần dùng trong Thay từ
            //        //    //termLocation.Flag = audio.Subtitle.IndexOf(newTranlate, System.StringComparison.OrdinalIgnoreCase) < 0;
            //        //    if (!termLocationFlag)
            //        //        termLocationFlag = audio.Subtitle.IndexOf(newTranlate, System.StringComparison.OrdinalIgnoreCase) < 0;
            //        //    var key = Module.Helpers.TextHelper.ReplaceSpecialCharacters(dictionaryResult.Keys, newTranlate);
            //        //    if (string.IsNullOrEmpty(key))
            //        //    {
            //        //        var newTranlates = Tools.TranslateText(Name);
            //        //        dictionaryResult.Add(newTranlate, termLocationFlag ? 0 : 1);
            //        //    }
            //        //    else if (!termLocationFlag)
            //        //    {
            //        //        dictionaryResult[key]++;
            //        //    }
            //        //}
            //        //else
            //        //{
            //        //    //Nếu không tìm thấy thuật ngữ thì dựng cờ
            //        //    // 2023 - 07 - 27: Bỏ: Cờ thuật vị chỉ cần dùng trong Thay từ
            //        //    //termLocation.Flag = true;
            //        //    Flag = true;
            //        }
            //    }
            //    //if (dictionaryResult.Keys.Count > 0)
            //    //{
            //    //    ////Xóa trắng dịch máy 
            //    //    //GoogleTranslate = null;
            //    //    int max = 0;
            //    //    string maxKey = null;
            //    //    foreach (var key in dictionaryResult.Keys)
            //    //    {
            //    //        if (dictionaryResult[key] == 0)
            //    //        {
            //    //            Flag = true;
            //    //        }
            //    //        else
            //    //        {
            //    //            if (string.IsNullOrEmpty(maxKey))
            //    //                maxKey = key;
            //    //        }
            //    //        if (dictionaryResult[key] > max)
            //    //        {
            //    //            max = dictionaryResult[key];
            //    //            maxKey = key;
            //    //        }
            //    //    }
            //    //    //Chỉ lưu vào dịch máy
            //    //    if (!string.IsNullOrEmpty(maxKey))
            //    //    {
            //    //        GoogleTranslate = maxKey;
            //    //    }
            //    //}
            //}
        }

        public Term MergeAdjacentTerms(System.Collections.Generic.List<TermLocation> termLocations, string part, bool next)
        {
            if (termLocations == null || termLocations.Count == 0)
                return null;

            // Sắp xếp danh sách vị trí thuật ngữ theo thứ tự xuất hiện
            termLocations = termLocations.OrderBy(t => t).ToList();

            System.Text.StringBuilder mergedTermText = new System.Text.StringBuilder();
            Term firstTerm = termLocations[0].Term;

            foreach (var termLocation in termLocations)
            {
                if (mergedTermText.Length > 0)
                    mergedTermText.Append(" "); // Thêm khoảng trắng giữa các thuật ngữ

                mergedTermText.Append(termLocation.Term.Name);
            }
            if (next)
            {
                if (mergedTermText.Length > 0)
                    mergedTermText.Append(" ");
                mergedTermText.Append(part);
            }
            else
            {
                if (mergedTermText.Length > 0)
                    mergedTermText.Insert(0, " ");
                mergedTermText.Insert(0, part);
            }
            // Tạo thuật ngữ mới với nội dung đã gộp
            var mergedTerm = CreateObject<Term>();
            mergedTerm.Video = firstTerm.Video;
            mergedTerm.Name = mergedTermText.ToString();
            mergedTerm.TermType = firstTerm.TermType;

            // Cập nhật vị trí thuật ngữ mới
            foreach (var termLocation in termLocations)
            {
                mergedTerm.TermLocationList.Add(termLocation);
            }

            // Xóa các thuật ngữ cũ nếu cần
            foreach (var termLocation in termLocations)
            {
                ObjectSpace.Delete(termLocation.Term);
            }

            // Cập nhật số lượng và dịch thuật ngữ mới
            int termLocationCount = 0;
            int success = 0;
            TranslateTerm(mergedTerm, ref termLocationCount, ref success);

            return mergedTerm;
        }


        public static bool CheckTermInDictionary(string termName, System.Collections.Generic.Dictionary<int, System.Collections.Generic.Dictionary<string, System.Collections.Generic.HashSet<string>>> dictionarySpell, Language spellLanguage, int WordQuantity)
        {

            if (string.IsNullOrEmpty(termName) || dictionarySpell == null)
                return false;

            if (!dictionarySpell.TryGetValue(WordQuantity, out var wordDict))
                return false;

            if (spellLanguage.Code == "en")
            {
                // Xử lý tên không dấu
                var nameNoneUnicode = termName.ToLower();
                var nameNoneUnicode2 = char.ToUpper(nameNoneUnicode[0]) + nameNoneUnicode.Substring(1);

                // Hàm stem hoặc lemmatize từ
                var stemmedName = ApplyStemming(nameNoneUnicode);
                var stemmedName2 = ApplyStemming(nameNoneUnicode2);

                // Xử lý các dạng từ có thể thay đổi (vd: bỏ "es", "ed")
                var tmpWords = nameNoneUnicode;
                var tmpWords2 = nameNoneUnicode2;
                var tmpWords3 = nameNoneUnicode;
                var tmpWords4 = nameNoneUnicode2;
                if (nameNoneUnicode.EndsWith("es") || nameNoneUnicode.EndsWith("ed"))
                {
                    tmpWords = nameNoneUnicode.Substring(0, nameNoneUnicode.Length - 1);
                    tmpWords2 = nameNoneUnicode2.Substring(0, nameNoneUnicode2.Length - 1);
                    tmpWords3 = nameNoneUnicode.Substring(0, nameNoneUnicode.Length - 2);
                    tmpWords4 = nameNoneUnicode2.Substring(0, nameNoneUnicode2.Length - 2);
                }

                // Kiểm tra từ trong từ điển
                return dictionarySpell[WordQuantity].ContainsKey(nameNoneUnicode) ||
                       dictionarySpell[WordQuantity].ContainsKey(nameNoneUnicode2) ||
                       dictionarySpell[WordQuantity].ContainsKey(stemmedName) ||
                       dictionarySpell[WordQuantity].ContainsKey(stemmedName2) ||
                       dictionarySpell[WordQuantity].ContainsKey(tmpWords) ||
                       dictionarySpell[WordQuantity].ContainsKey(tmpWords2) ||
                       dictionarySpell[WordQuantity].ContainsKey(tmpWords3) ||
                       dictionarySpell[WordQuantity].ContainsKey(tmpWords4);
            }
            else if (spellLanguage.Code == "vi")
            {
                // Xử lý tên không dấu
                var nameNoneUnicode = termName.ToLower();
                var nameNoneUnicode2 = char.ToUpper(nameNoneUnicode[0]) + nameNoneUnicode.Substring(1);
                return dictionarySpell[WordQuantity].ContainsKey(nameNoneUnicode) ||
                       dictionarySpell[WordQuantity].ContainsKey(nameNoneUnicode2);
            }
            else
                return false;
        }


        public static string ApplyStemming(string word)
        {
            if (string.IsNullOrEmpty(word))
                return word;
            else if (word.EndsWith("ing") && word.Length > 4)
            {
                string baseWord = word.Substring(0, word.Length - 3);
                if (baseWord.EndsWith("e") && !IsVowel(baseWord[baseWord.Length - 2]))
                    return baseWord.Substring(0, baseWord.Length - 1); // Bỏ "e" trước "ing"
                else if (IsDoubleConsonantRule(baseWord))
                    return baseWord.Substring(0, baseWord.Length - 1); // Bỏ phụ âm cuối nếu nhân đôi
                return baseWord;
            }
            else if (word.EndsWith("ed") && word.Length > 3)
            {
                string baseWord = word.Substring(0, word.Length - 2);
                if (word.EndsWith("ied"))
                    return word.Substring(0, word.Length - 3) + "y"; // Loại bỏ "es" chỉ trong một số trường hợp
                else if (word.EndsWith("ved"))
                    return word.Substring(0, word.Length - 2) + "f";
                else if (IsDoubleConsonantRule(baseWord))
                    return baseWord.Substring(0, baseWord.Length - 1);
                else
                    return word.Substring(0, word.Length - 2);
            }
            else if (word.EndsWith("es") && word.Length > 3)
            {
                string baseWord = word.Substring(0, word.Length - 2);
                if (word.EndsWith("ies"))
                    return word.Substring(0, word.Length - 3) + "y"; // Loại bỏ "es" chỉ trong một số trường hợp
                else if (word.EndsWith("ves"))
                    return word.Substring(0, word.Length - 2) + "f";
                else if (IsDoubleConsonantRule(baseWord))
                    return baseWord.Substring(0, baseWord.Length - 1);
                else
                    return word.Substring(0, word.Length - 2);
            }
            else if (word.EndsWith("s") && word.Length > 2)
            {
                return word.Substring(0, word.Length - 1); // Bỏ "s"
            }

            return word;
        }

        private static bool IsVowel(char c)
        {
            return "aeiou".Contains(char.ToLower(c));
        }

        private static bool IsDoubleConsonantRule(string word)
        {
            if (word.Length < 2) return false;

            // Lấy ký tự cuối và ký tự liền trước
            char last = word[word.Length - 1];
            char secondLast = word[word.Length - 2];

            // Kiểm tra xem cả hai ký tự cuối có phải là phụ âm và giống nhau không
            return !IsVowel(last) && !IsVowel(secondLast) && last == secondLast;
        }

        private string GetTextInTranslate(string audioSubtitle, string content, string newtranlateContent, string option = "/")
        {
            if (option == "Upcase")
            {

            }
            else
            {
                int startIndex = newtranlateContent.IndexOf(option, System.StringComparison.OrdinalIgnoreCase);
                int endIndex = newtranlateContent.IndexOf(option, startIndex + 1, System.StringComparison.OrdinalIgnoreCase);
                if (startIndex < endIndex && (startIndex >= 0 || endIndex > 0))
                {
                    //Nếu tìm thấy từ viết hoa
                    if (startIndex < 0)
                        startIndex = 0;
                    if (endIndex < 0)
                        endIndex = newtranlateContent.Length;
                    string newTranlate = newtranlateContent.Substring(startIndex + 1, endIndex - startIndex - 1);
                    int newStartIndex = audioSubtitle.IndexOf(newTranlate, System.StringComparison.OrdinalIgnoreCase);
                    if (newStartIndex < 0)
                    {
                        //Từ được dịch không hợp lệ
                        //2023-07-17: Dùng thử tính năng so sánh 2 câu                                       
                        if (startIndex > 0)
                        {
                            var firstText = newtranlateContent.Substring(0, startIndex).Trim();
                            startIndex = audioSubtitle.IndexOf(firstText, System.StringComparison.OrdinalIgnoreCase);
                            if (startIndex < 0)
                            {
                                //Fix trường hợp tìm nhiều lần không thấy
                                while (startIndex < 0)
                                {
                                    var spaceIndex = firstText.IndexOf(' ');
                                    if (spaceIndex > 0)
                                        firstText = firstText.Substring(spaceIndex + 1).Trim();
                                    else
                                        break;
                                    if (string.IsNullOrEmpty(firstText))
                                        break;
                                    startIndex = audioSubtitle.IndexOf(firstText, System.StringComparison.OrdinalIgnoreCase);
                                }
                            }
                            if (startIndex >= 0)
                                startIndex += firstText.Length;

                        }
                        if (endIndex < newtranlateContent.Length)
                        {
                            var endText = newtranlateContent.Substring(endIndex + 1);
                            //Nếu câu có 2 từ trùng nhau thì chỉ lấy từ đầu tiên
                            var afterIndex = endText.IndexOf(option, System.StringComparison.OrdinalIgnoreCase);
                            if (afterIndex > 0)
                                endText = endText.Substring(0, afterIndex).Trim();
                            endIndex = audioSubtitle.IndexOf(endText, System.StringComparison.OrdinalIgnoreCase);
                            if (endIndex < 0)
                            {
                                //Fix trường hợp tìm nhiều lần không thấy
                                while (endIndex < 0)
                                {
                                    var spaceIndex = endText.LastIndexOf(' ');
                                    if (spaceIndex > 0)
                                        endText = endText.Substring(0, spaceIndex).Trim();
                                    else
                                        break;
                                    if (string.IsNullOrEmpty(endText) || startIndex < 0 || startIndex >= endText.Length)
                                        break;
                                    endIndex = audioSubtitle.IndexOf(endText, startIndex, System.StringComparison.OrdinalIgnoreCase);
                                }
                            }
                        }
                        if (startIndex < endIndex && startIndex >= 0)
                        {
                            newTranlate = audioSubtitle.Substring(startIndex, endIndex - startIndex);
                            newTranlate = Module.Helpers.TextHelper.RemoveStartEndSpecialCharacters(newTranlate);
                            if (!string.IsNullOrEmpty(newTranlate))
                                newTranlate = newTranlate.Trim();
                        }
                        else
                        {
                            //Fix trường hợp có 2 từ thì không quan tâm trước sau
                            //Từ gốc:                preview 'indication'
                            //Từ sau khi thêm nháy:  xem trước 'chỉ định'
                            //Từ được dịch cả câu: dấu hiệu xem trước
                            newTranlate = null;
                            if (content.Split(' ').Length == 2)
                            {
                                startIndex = newtranlateContent.IndexOf(option, System.StringComparison.OrdinalIgnoreCase);
                                endIndex = newtranlateContent.IndexOf(option, startIndex + 1, System.StringComparison.OrdinalIgnoreCase);
                                if (startIndex < endIndex && startIndex >= 0)
                                {
                                    string otherText = "";
                                    if (startIndex == 0)
                                    {
                                        otherText = newtranlateContent.Substring(endIndex).Trim();
                                        newTranlate = audioSubtitle.Replace(otherText, "").Trim();
                                    }
                                    else if (endIndex == newtranlateContent.Length - 1)
                                    {
                                        otherText = newtranlateContent.Substring(0, startIndex).Trim();
                                        newTranlate = audioSubtitle.Replace(otherText, "").Trim();
                                    }

                                }
                            }

                        }

                    }
                    else
                    {
                        newTranlate = audioSubtitle.Substring(newStartIndex, newTranlate.Length);
                    }
                }

            }
            return null;
        }




        #endregion SourceCode4545ImportCode

  
  
        #region Base Object Service
	    		//public string ToolTipControllerText(View view, Module.BusinessObjects.Term term)
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
		//public object NameToolTipControllerText(View view, Module.BusinessObjects.Term term)
        //{
            
        //    return null;
        //}
    

		//Tooltip for Object
		//public object TranslateToolTipControllerText(View view, Module.BusinessObjects.Term term)
        //{
        //    if (Translate != null) 
		//			return Translate;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object LanguageTranslateToolTipControllerText(View view, Module.BusinessObjects.Term term)
        //{
        //    if (LanguageTranslate != null) 
		//			return LanguageTranslate;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object GoogleTranslateToolTipControllerText(View view, Module.BusinessObjects.Term term)
        //{
        //    if (GoogleTranslate != null) 
		//			return GoogleTranslate;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object WordTypeToolTipControllerText(View view, Module.BusinessObjects.Term term)
        //{
        //    if (WordType != null) 
		//			return WordType;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object TermLocationListToolTipControllerText(View view, Module.BusinessObjects.Term term)
        //{
        //    if (TermLocationList != null) 
		//			return TermLocationList;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object UpdateToolTipControllerText(View view, Module.BusinessObjects.Term term)
        //{
        //    if (Update != null) 
		//			return Update;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object VideoToolTipControllerText(View view, Module.BusinessObjects.Term term)
        //{
        //    if (Video != null) 
		//			return Video;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object TermTypeToolTipControllerText(View view, Module.BusinessObjects.Term term)
        //{
        //    if (TermType != null) 
		//			return TermType;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object QuantityToolTipControllerText(View view, Module.BusinessObjects.Term term)
        //{
            
        //    return null;
        //}
    

		//Tooltip for Object
		//public object FlagToolTipControllerText(View view, Module.BusinessObjects.Term term)
        //{
        //    if (Flag != null) 
		//			return Flag;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object NumberValueToolTipControllerText(View view, Module.BusinessObjects.Term term)
        //{
        //    if (NumberValue != null) 
		//			return NumberValue;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object DateValueToolTipControllerText(View view, Module.BusinessObjects.Term term)
        //{
        //    if (DateValue != null) 
		//			return DateValue;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object LengthToolTipControllerText(View view, Module.BusinessObjects.Term term)
        //{
        //    if (Length != null) 
		//			return Length;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object WordQuantityToolTipControllerText(View view, Module.BusinessObjects.Term term)
        //{
        //    if (WordQuantity != null) 
		//			return WordQuantity;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object StatusToolTipControllerText(View view, Module.BusinessObjects.Term term)
        //{
        //    if (Status != null) 
		//			return Status;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object NoteToolTipControllerText(View view, Module.BusinessObjects.Term term)
        //{
        //    if (Note != null) 
		//			return Note;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object LikeTermToolTipControllerText(View view, Module.BusinessObjects.Term term)
        //{
            
        //    return null;
        //}
    

		//Tooltip for Object
		//public object Flag2ToolTipControllerText(View view, Module.BusinessObjects.Term term)
        //{
        //    if (Flag2 != null) 
		//			return Flag2;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object OverlapToolTipControllerText(View view, Module.BusinessObjects.Term term)
        //{
            
        //    return null;
        //}
    

		//Tooltip for Object
		//public object Note2ToolTipControllerText(View view, Module.BusinessObjects.Term term)
        //{
        //    if (Note2 != null) 
		//			return Note2;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object LikeWordToolTipControllerText(View view, Module.BusinessObjects.Term term)
        //{
            
        //    return null;
        //}
    

		//Tooltip for Object
		//public object LanguageToolTipControllerText(View view, Module.BusinessObjects.Term term)
        //{
        //    if (Language != null) 
		//			return Language;
        //    return null;
        //}
    

	    #endregion
  

    }
}
