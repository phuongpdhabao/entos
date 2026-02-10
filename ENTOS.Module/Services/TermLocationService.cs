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

    public partial class TermLocationService : BaseService
    {

        public TermLocationService() : base()
        {
        }
        #region DependencyInjection
  
     
        private TermService termService;  
        protected TermService _termService => termService ??= new TermService(ViewController);        
       
  
        #endregion DependencyInjection

        public TermLocationService(ViewController viewController) : base(viewController)
        {

        }
        //public event EventHandler<string>? OnError;
        //public event EventHandler<string>? OnSuccess;
        //public event EventHandler<string>? OnResponse; 
        
        #region SourceCode4550ImportCode
        
        public void OverlapTermPosition(TermLocation termLocation, string choice, ref int termCount, ref int termLocationCount)
        {
            if (termLocation.Audio is null || string.IsNullOrEmpty(termLocation.Audio.Content) ||
                termLocation.Term is null || string.IsNullOrEmpty(termLocation.Term.Name))
                return;
            var index = GetIndexContent(termLocation, termLocation.Audio.Content, termLocation.Term.Name);
            if (index >= 0)
            {
                var termWords = termLocation.Term.Name.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
                var sentence = GetSentenceTextFromContent(termLocation, termLocation.Audio.Content);
                var realPosition = GetIndexByLocation(termLocation, sentence, termLocation.Term.Name);
                string otherTermName = OverlapTermPosition_ComputeOtherTermName(termWords, termLocation.Term.Name, sentence, realPosition, choice);
                if (otherTermName == null)
                    return;
                //Đổi vị trí Location
                if (choice.Equals("TwoLeft"))
                    termLocation.Location--;
                else if (choice.Equals("ThreeLeft"))
                    termLocation.Location = termLocation.Location - 2;
                else if (choice.Contains("Right"))
                {
                    termLocation.Location += termWords.Length - 1;
                }

                var existTerm = TermService.FindTermByName(termLocation.Term, otherTermName);
                var oldTerm = termLocation.Term;
                if (existTerm is null && oldTerm.TermLocationList.Count == 1)
                {
                    termLocation.Term.Name = otherTermName;
                    termLocation.Term.LikeTerm = null;
                    termLocation.Term.Overlap = CheckOverlap(termLocation, true);
                    _termService.CheckTermNameIsCorrectAndFlag(termLocation.Term);
                }
                else
                {
                    if (existTerm is null)
                    {
                        existTerm = CreateObject<Term>();
                        existTerm.Video = oldTerm.Video;
                        existTerm.Quantity = 0;
                        existTerm.Name = otherTermName;
                        //Kiểm tra chính tả và dựng cờ nếu có
                        _termService.CheckTermNameIsCorrectAndFlag(existTerm);
                    }
                    existTerm.Quantity++;
                    termLocation.Term = existTerm;
                    oldTerm.Quantity--;
                    if (oldTerm.Length <= 0 || oldTerm.TermLocationList.Count == 0)
                        oldTerm.Delete();
                    termCount++;
                }
                termLocationCount++;
            }

        }



        public string ShiftWord(TermLocation termLocation, string text, string word, int left, int right)
        {
            // Tách chuỗi thành các từ            
            string[] wordsArray = text.Split(' ');

            // Tìm vị trí của từ hoặc cụm từ cần tìm trong chuỗi
            int realLocation = -1;
            if (termLocation.Location != null && termLocation.Location.Value >= 0 && termLocation.Location.Value <= wordsArray.Length &&
                word.StartsWith(wordsArray[termLocation.Location.Value - 1], System.StringComparison.OrdinalIgnoreCase))
            {
                realLocation = termLocation.Location.Value - 1;
            }
            else
            {
                for (int i = 0; i < wordsArray.Length; i++)
                {
                    // Kiểm tra xem các từ liên tiếp có khớp với từ cần tìm hay không
                    if (string.Join(" ", wordsArray, i, Math.Min(wordsArray.Length - i, word.Split(' ').Length)).Equals(word, System.StringComparison.OrdinalIgnoreCase))
                    {
                        realLocation = i;
                    }
                }
            }
            if (realLocation >= 0)
            {
                return ShiftWord_ComputeResult(wordsArray, realLocation, word, left, right);
            }

            return null;
        }


        public void CancelWrongTerm(TermLocation termLocation, ref int deleteTerm, ref int deleteTermLocation)
        {
            var termSelect = termLocation.Term;
            var overlapList = GetOverlap(termLocation, true);
            if (overlapList is null || termLocation.Location is null)
                return;
            foreach (var overlapTermLocation in overlapList)
            {
                if (termSelect.Video != null && overlapTermLocation.Term != null && !string.IsNullOrEmpty(overlapTermLocation.Term.Name))
                {
                    int termPoint = CancelWrongTerm_ComputeScore(overlapTermLocation, termSelect, termLocation.Location.Value, overlapList);
                    if (termPoint < 0)
                    {
                        CancelWrongTerm_HandleNegative(overlapTermLocation, ref deleteTerm, ref deleteTermLocation);
                    }
                    else if (termPoint > 0)
                    {
                        //Xóa thuật vị này (preserve original behavior)
                        termLocation.Session.Delete(this);
                        deleteTermLocation++;

                    }
                }
            }
            if (termSelect.TermLocationList.Count == 0)
            {
                //Loại bỏ bên sai
                termSelect.Delete();
                deleteTerm++;
            }
        }
        private static bool MoreOverlap_CheckCorrectAll(System.Collections.Generic.List<TermLocation> list, Video video, bool requireTerm)
        {
            // Returns true if every TermLocation in the list passes the "correctAll" checks
            foreach (var tl in list)
            {
                var overlap = GetOverlap(tl, requireTerm);
                if (overlap == null || overlap.Count > 1)
                    return false;
                if (tl.Term == null || string.IsNullOrEmpty(tl.Term.Name))
                    return false;
                if (!Module.Helpers.TextHelper.CheckWordIsCorrect(video.GetDictionary(), tl.Term.Name))
                    return false;
            }
            return true;
        }

        public static int Spelling(TermLocation termLocation, Video video, string termName2)
        {
            if (video != null && video.GetDictionary() != null)
            {
                var term1Name = termLocation.Term?.Name;
                var dict = video.GetDictionary();
                var baseResult = Spelling_CompareByDictionary(dict, term1Name, termName2);
                if (baseResult.HasValue)
                    return baseResult.Value;

                // Further check that requires real text from TermLocation
                if (dict.ContainsKey(1))
                {
                    var term1RealName = GetRealNameFromLocation(termLocation);
                    if (!string.IsNullOrEmpty(term1RealName))
                    {
                        var notCorrect1 = SpellingNotCorrect(dict[1], term1RealName);
                        var notCorrect2 = SpellingNotCorrect(dict[1], termName2);
                        if (notCorrect1 < notCorrect2)
                            return 1;
                        else if (notCorrect1 > notCorrect2)
                            return -1;
                    }
                }
            }
            return 0;
        }



        public static int ExistWord(TermLocation termLocation, Video video, string termName2)
        {
            //Nếu 1 thì là thuật ngữ này thắng, -1 là thuật ngữ 2 thắng
            //098: - ExistWord(TV1, TV2): so sánh sự tồn tại thuật ngữ đúng chính tả trong list, không kể TV1 và TV2
            if (termLocation.Term != null && !string.IsNullOrEmpty(termLocation.Term.Name))
            {
                var existWord1 = GetExistTerm(termLocation, video, termLocation.Term.Name);
                var existWord2 = GetExistTerm(termLocation, video, termName2);
                if (existWord1 != null)
                {
                    if (existWord2 is null)
                        return 1;
                }
                else if (existWord2 != null)
                    return -1;
            }
            return 0;
        }
        private static Term GetExistTerm(TermLocation termLocation, Video video, string termName)
        {
            var nonUnicodeName = Module.Helpers.TextHelper.RemoveUnicode(termName);
            if (termLocation.Term?.Video != null)
            {
                //Check theo Term trong Video nếu đã add
                foreach (var refTerm in termLocation.Term.Video.TermList)
                {
                    if (nonUnicodeName.Equals(Module.Helpers.TextHelper.RemoveUnicode(refTerm.Name), System.StringComparison.OrdinalIgnoreCase) &&
                       Module.Helpers.TextHelper.CheckWordIsCorrect(video.GetDictionary(), refTerm?.Name))
                    {
                        return refTerm;
                    }
                }
            }
            else
            {
                //Check theo Term trong đối tượng Audio nếu đã add
                foreach (var audio in video.AudioList)
                {
                    foreach (var tl in audio.TermLocationList)
                    {
                        if (tl.Term != null && !string.IsNullOrEmpty(tl.Term.Name) &&
                            nonUnicodeName.Equals(Module.Helpers.TextHelper.RemoveUnicode(tl.Term.Name), System.StringComparison.OrdinalIgnoreCase) &&
                            Module.Helpers.TextHelper.CheckWordIsCorrect(video.GetDictionary(), tl.Term?.Name))
                        {
                            return tl.Term;
                        }
                    }
                }
            }
            return null;
        }

        public static int Longer(TermLocation termLocation, Video video, string termName2)
        {
            var termName1 = termLocation?.Term?.Name;
            return CompareByWordCount(termName1, termName2);
        }

        public static int MoreOverlap(TermLocation termLocation, Video video, System.Collections.Generic.List<TermLocation> overlap2List, bool requireTerm)
        {
            //Nếu 1 thì là thuật ngữ này thắng, -1 là thuật ngữ 2 thắng
            //098: - MoreiOverlap (TV1, TV2): so sánh số lần overlap (overlap thứ 2 chỉ tính khi TV3 đúng chính tả và không overlap TV4)
            var overlap1List = GetOverlap(termLocation, requireTerm);
            if (overlap2List.Count >= 2 && overlap1List.Count == 1)
            {
                if (MoreOverlap_CheckCorrectAll(overlap2List, video, requireTerm))
                    return 1;
            }
            else if (overlap2List.Count == 1 && overlap1List.Count >= 2)
            {
                if (MoreOverlap_CheckCorrectAll(overlap1List, video, requireTerm))
                    return -1;
            }
            return 0;
        }


        public static int NoneOverlapPart(TermLocation termLocation, Video video, string termName2, int currentLocation)
        {
            //Nếu 1 thì là thuật ngữ này thắng, -1 là thuật ngữ 2 thắng
            //098: NoneOverlapPart (TV1, TV2): so sánh phần không đè của 2 TV bên nào tồn tại thuật ngữ 
            if (termLocation.Term != null && !string.IsNullOrEmpty(termLocation.Term.Name) && termLocation.Location != null)
            {
                var termNameArray = termLocation.Term?.Name?.ToLower().Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
                var termName2Array = termName2?.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
                string term1NoneOverlap = string.Empty;
                string term2NoneOverlap = string.Empty;
                NoneOverlapPart_ComputeNonOverlapStrings(termNameArray, termName2Array, termLocation.Location.Value, currentLocation, out term1NoneOverlap, out term2NoneOverlap);
                bool term1IsNoneOverlapPart = !string.IsNullOrEmpty(term1NoneOverlap) && GetExistTerm(termLocation, video, term1NoneOverlap) != null;
                bool term2IsNoneOverlapPart = !string.IsNullOrEmpty(term2NoneOverlap) && GetExistTerm(termLocation, video, term2NoneOverlap) != null;
                if (!term1IsNoneOverlapPart.Equals(term2IsNoneOverlapPart))
                {
                    return term1IsNoneOverlapPart ? 1 : -1;
                }
            }
            return 0;
        }

        public static int OverlapCaseType(TermLocation termLocation, Video video, string termName2)
        {
            //Nếu 1 thì là thuật ngữ này thắng, -1 là thuật ngữ 2 thắng
            //098- OverlapCaseType (TV1, TV2):  Phần không đè cùng kiểu Hoa/thường với phần đè            
            var term1RealName = GetRealNameFromLocation(termLocation);
            if (!string.IsNullOrEmpty(term1RealName))
            {
                return OverlapCaseType_CompareByRealName(term1RealName, termName2);
            }
            return 0;
        }



        public int ConfirmOrNotTerm(TermLocation termLocation, bool confirm, int deleteTerm, bool requireTerm)
        {
            if (termLocation.Term is null)
                return deleteTerm;
            if (confirm)
            {
                //if(termLocation.Term.Overlap)
                //   Term.Overlap = false;
                //Term.RemoveFlagOverlap();
                termLocation.Overlap = false;
            }
            var overlapList = GetOverlap(termLocation, requireTerm);
            if (overlapList != null && overlapList.Count > 0)
            {
                if (confirm)
                {
                    foreach (var overlapTermLocation in overlapList)
                    {
                        deleteTerm = ConfirmOrNotTerm_HandleConfirmOverlap(overlapTermLocation, deleteTerm, requireTerm);
                    }

                }
                else
                {
                    foreach (var overlapTermLocation in overlapList)
                    {
                        ConfirmOrNotTerm_HandleUnconfirmOverlap(overlapTermLocation, requireTerm);
                    }
                }

            }
            if (!confirm)
                termLocation.Session.Delete(this);
            return deleteTerm;
        }

        private int ConfirmOrNotTerm_HandleConfirmOverlap(TermLocation overlapTermLocation, int deleteTerm, bool requireTerm)
        {
            // Handle the case where an overlapping TermLocation is confirmed as a term - remove overlaps and adjust quantities
            var overlapTerm = overlapTermLocation.Term;
            if (overlapTerm != null && overlapTerm.Quantity != null)
                overlapTerm.Quantity--;
            // Recurse to clear overlaps related to this location
            deleteTerm = ConfirmOrNotTerm(overlapTermLocation, false, deleteTerm, requireTerm);
            overlapTermLocation.Delete();
            if (overlapTerm != null && overlapTerm.TermLocationList.Count == 0)
            {
                overlapTerm.Delete();
                deleteTerm++;
            }
            else if (overlapTerm != null && overlapTerm.Overlap)
            {
                bool overlap = false;
                foreach (var tl2 in overlapTerm.TermLocationList)
                {
                    if (tl2.Overlap)
                    {
                        overlap = true;
                    }
                }
                if (!overlap)
                    overlapTerm.Overlap = false;
            }
            return deleteTerm;
        }

        private void ConfirmOrNotTerm_HandleUnconfirmOverlap(TermLocation overlapTermLocation, bool requireTerm)
        {
            // Handle the case where an overlapping TermLocation is marked not-a-term: clear overlap flags as needed
            if (overlapTermLocation.IsDeleted)
                return;
            var refoverlapList = GetOverlap(overlapTermLocation, requireTerm);
            if (refoverlapList is null)
                return;
            if (refoverlapList.Count == 1)
            {
                overlapTermLocation.Overlap = false;
                if (overlapTermLocation.Term != null && overlapTermLocation.Term.Overlap)
                {
                    bool isOverlap = false;
                    foreach (var tl in overlapTermLocation.Term.TermLocationList)
                    {
                        if (tl.Overlap)
                        {
                            isOverlap = true;
                            break;
                        }
                    }
                    if (!isOverlap)
                    {
                        overlapTermLocation.Term.Overlap = false;
                    }
                }
            }
            else
            {
                // No-op for multi-overlap case (preserve original behavior)
            }
        }

        public static int TermPositionRelation(TermLocation termLocation, TermLocation termLocation2)
        {
            //1- TV1 overlap TV2
            //2- TV1 belong (thuộc về) TV2
            //3 - TV2 belong (thuộc về) TV1
            //4 Not overlap
            if (termLocation.Audio != null && termLocation2.Audio != null && termLocation.Audio.Oid == termLocation2.Audio.Oid && termLocation.Term != null && termLocation2 != null && termLocation2.Term != null && !termLocation.Oid.Equals(termLocation2.Oid)
                && termLocation.Location != null && termLocation2.Location != null && !string.IsNullOrEmpty(termLocation.Term.Name) && !string.IsNullOrEmpty(termLocation2.Term.Name))
            {
                if (termLocation.Sentence == termLocation2.Sentence)
                {
                    var tv1WordQuantity = termLocation.Term.Name.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
                    var tv2WordQuantity = termLocation2.Term.Name.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
                    if (termLocation.Location == termLocation2.Location)
                    {
                        //trường hợp bị lỗi
                        if (tv1WordQuantity == tv2WordQuantity)
                            return 0;
                        else if (tv1WordQuantity > tv2WordQuantity)
                            return 3;
                        else
                            return 2;
                    }
                    else if (termLocation.Location < termLocation2.Location)
                    {
                        var tv1EndLocation = termLocation.Location + tv1WordQuantity - 1;
                        var tv2EndLocation = termLocation2.Location + tv2WordQuantity - 1;
                        if (tv1EndLocation < termLocation2.Location)
                        {
                            //Không bị trùng
                            return 4;
                        }
                        else if (tv1EndLocation >= tv2EndLocation)
                        {
                            //3 - TV2 belong (thuộc về) TV1
                            return 3;
                        }
                        else
                        {
                            //1- TV1 overlap TV2
                            return 1;
                        }
                    }
                    else
                    {
                        var tv2EndLocation = termLocation2.Location + tv2WordQuantity - 1;
                        var tv1EndLocation = termLocation.Location + tv1WordQuantity - 1;
                        if (tv2EndLocation < termLocation.Location)
                        {
                            //Không bị trùng
                            return 4;
                        }
                        else if (tv2EndLocation >= tv1EndLocation)
                        {
                            //2- TV1 belong (thuộc về) TV2
                            return 2;
                        }
                        else
                        {
                            //1- TV1 overlap TV2
                            return 1;
                        }
                    }
                }
            }
            return 4;
        }

        public static int TermPositionRelation(TermLocation termLocation, Audio element2, int sentence2, int location2, string termName2)
        {
            //Thuật ngữ 1 là thuật ngữ hiện tại, thuật ngữ 2 là thuật ngữ cần kiểm tra
            //1- TV1 overlap TV2
            //2- TV1 belong (thuộc về) TV2
            //3 - TV2 belong (thuộc về) TV1
            //4 Not overlap
            if (termLocation.Audio != null && element2 != null && termLocation.Term != null && termLocation.Location != null && !string.IsNullOrEmpty(termLocation.Term.Name) && !string.IsNullOrEmpty(termName2))
            {
                if (termLocation.Audio.Oid == element2.Oid && termLocation.Sentence == sentence2)
                {
                    var tv1WordQuantity = termLocation.Term.Name.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
                    var tv2WordQuantity = termName2.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
                    if (termLocation.Location == location2)
                    {
                        //trường hợp bị lỗi
                        if (tv1WordQuantity == tv2WordQuantity)
                            return 0;
                        else if (tv1WordQuantity > tv2WordQuantity)
                            return 3;
                        else
                            return 2;
                    }
                    else if (termLocation.Location < location2)
                    {
                        var tv1EndLocation = termLocation.Location + tv1WordQuantity - 1;
                        var tv2EndLocation = location2 + tv2WordQuantity - 1;
                        if (tv1EndLocation < location2)
                        {
                            //Không bị trùng
                            return 4;
                        }
                        else if (tv1EndLocation >= tv2EndLocation)
                        {
                            //3 - TV2 belong (thuộc về) TV1
                            return 3;
                        }
                        else
                        {
                            //1- TV1 overlap TV2
                            return 1;
                        }
                    }
                    else
                    {
                        var tv1EndLocation = termLocation.Location + tv1WordQuantity - 1;
                        var tv2EndLocation = location2 + tv2WordQuantity - 1;
                        if (tv2EndLocation < termLocation.Location)
                        {
                            //Không bị trùng
                            return 4;
                        }
                        else if (tv2EndLocation >= tv1EndLocation)
                        {
                            //2- TV1 belong (thuộc về) TV2
                            return 2;
                        }
                        else
                        {
                            //1- TV1 overlap TV2
                            return 1;
                        }
                    }
                }
            }
            return 4;
        }

        public static bool CheckRealNameIsUpperCaseFirstAll(TermLocation termLocation, string realName)
        {
            if (string.IsNullOrEmpty(realName))
                realName = GetRealNameFromLocation(termLocation);
            return Module.Helpers.TextHelper.CheckRealNameIsUpperCaseFirstAll(realName);
        }

        public static string GetRealNameFromLocation(TermLocation termLocation)
        {
            if (termLocation?.Term == null || termLocation?.Audio == null || termLocation.Sentence == null || termLocation.Sentence <= 0
                || termLocation.Location == null || termLocation.Location <= 0 || string.IsNullOrEmpty(termLocation.Term.Name) || string.IsNullOrEmpty(termLocation.Audio.Content))
                return null;

            var content = termLocation.Audio.Content;
            var sentenceIndex = termLocation.Sentence.Value;
            var location = termLocation.Location.Value;
            var termName = termLocation.Term.Name;
            return GetRealNameFromLocation_Compute(content, sentenceIndex, location, termName);
        }


        public static bool ReplaceWord(TermLocation termLocation, string replaceText, bool requireTerm, Term existedTerm = null)
        {
            if (termLocation.Audio is null || string.IsNullOrEmpty(termLocation.Audio.Content))
                return false;
            var content = termLocation.Audio.Content.Replace("  ", " ");
            string termName = termLocation.Term != null ? termLocation.Term.Name : termLocation.MachineTranslate;
            if (string.IsNullOrEmpty(termName))
                return false;
            if (termName.Equals(replaceText))
                return false;
            int index = GetIndexContent(termLocation, content, termName);
            //if (unicode)
            //{
            //    index = GetIndexContent(content, Term.Name);
            //}
            //else
            //{
            //    var contentNoneUnicode = Module.Helpers.TextHelper.RemoveUnicode(content);
            //    index = GetIndexContent(contentNoneUnicode, Term.Name);
            //}

            if (index < 0)
                return false;

            string localReplaceText = replaceText;
            var oldText = content.Substring(index, replaceText.Length);
            var oldTextArray = oldText.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var (computedLocalReplaceText, localReplaceTextArray) = ReplaceWord_ComputeLocalReplaceText(replaceText, oldText);
            localReplaceText = computedLocalReplaceText;
            var newContent = content.Substring(0, index);
            newContent += localReplaceText;
            newContent += content.Substring(index + localReplaceText.Length);
            termLocation.Audio.Content = newContent;
            if (termLocation.Flag)
            {
                termLocation.Flag = false;
            }
            if (oldTextArray.Length == localReplaceTextArray.Length)
            {
                ReplaceWord_HandleOverlap(termLocation, oldTextArray, localReplaceTextArray, requireTerm);
            }
            //2023-10-24: Khi sửa nếu thuật ngữ đúng đã tồn tại thì tạo thêm thuật vị cho TN đúng đó, xóa thuật ngữ sai đi
            //if (existedTerm is null)
            //    existedTerm = Term.FindTermByName(replaceText);
            if (termLocation.Term != null)
            {
                if (existedTerm != null)
                {
                    Term deleteTerm = null;
                    if (termLocation.Term.Quantity == 1)
                    {
                        deleteTerm = termLocation.Term;
                    }
                    else if (termLocation.Term.Quantity != null)
                    {
                        termLocation.Term.Quantity--;
                    }
                    termLocation.Term = existedTerm;
                    existedTerm.Quantity++;
                    if (deleteTerm != null)
                        deleteTerm.Delete();
                }
                else
                {
                    if (termLocation.Term.Quantity == 1)
                    {
                        //Đổi tên thuật ngữ
                        termLocation.Term.Name = replaceText.ToLower();
                    }
                    else
                    {
                        if (termLocation.Term.Quantity != null)
                            termLocation.Term.Quantity--;
                        //Tạo term mới;
                        existedTerm = new Term(termLocation.Session);
                        existedTerm.Video = termLocation.Term.Video;
                        existedTerm.Name = replaceText.ToLower();
                        existedTerm.Quantity = 1;
                        existedTerm.NumberValue = termLocation.Term.NumberValue;
                        termLocation.Term = existedTerm;
                    }

                }
            }


            return true;
        }

        private static void ReplaceWord_HandleOverlap(TermLocation termLocation, string[] oldTextArray, string[] localReplaceTextArray, bool requireTerm)
        {
            //088: Chức năng Sửa chính tả: khi đã Sửa thì mọi thuật vị được sửa sẽ Xóa mọi thuật vị overlap liên quan tới nó
            var overlapList = GetOverlap(termLocation, requireTerm);
            if (overlapList == null)
                return;
            foreach (var tl in overlapList)
            {
                if (tl.Term != null)
                {
                    var refTextArray = tl.Term.Name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    for (int m = 0; m < refTextArray.Length; m++)
                    {
                        for (int n = 0; n < oldTextArray.Length; n++)
                        {
                            if (refTextArray[m].Equals(oldTextArray[n], StringComparison.OrdinalIgnoreCase))
                            {
                                refTextArray[m] = localReplaceTextArray[n];
                            }
                        }
                    }
                    var newName = string.Join(" ", refTextArray);
                    if (tl.Term.Quantity == 1)
                    {
                        tl.Term.Name = newName;
                        tl.Term.Flag = false;
                    }
                    else if (tl.Term.Quantity != null)
                    {
                        tl.Term.Quantity--;
                        //Kiểm tra xem có thuật ngữ ngày không
                        Term newTerm = null;
                        foreach (var findTerm in termLocation.Term.Video.TermList)
                        {
                            if (newName.Equals(findTerm.Name, StringComparison.OrdinalIgnoreCase))
                            {
                                newTerm = findTerm;
                                break;
                            }
                        }
                        if (newTerm is null)
                        {
                            newTerm = new Term(termLocation.Session);
                            newTerm.Video = termLocation.Term.Video;
                            newTerm.Name = newName;
                            newTerm.Quantity = 1;
                            newTerm.NumberValue = termLocation.Term.NumberValue;
                        }
                        tl.Term = newTerm;
                    }
                }
                else
                {
                    tl.Delete();
                }
            }
        }


        public static System.Collections.Generic.List<TermLocation> GetOverlap(TermLocation termLocation, bool requireTerm)
        {
            //059: Chức năng xác định Overlap sẽ bao phủ cả các thuật vị Abbyy (hiện là các thuật vị không trỏ vào thuật ngữ)
            //if (termLocation.Term is null)
            //    return false;
            if (termLocation.Audio is null)
                return null;
            return termLocation.Audio.TermLocationList.Where(m => !m.Oid.Equals(termLocation.Oid) && m.Sentence == termLocation.Sentence && IsOverlap(termLocation, m, requireTerm)).ToList();
            //System.Collections.Generic.List<TermLocation> result = null;
            //Kiểm tra xem có thuật vị nào trùng không                    
            //if(!Session.IsNewObject(termLocation.Term.Video))
            //{
            //    var criteria = DevExpress.Data.Filtering.CriteriaOperator.Parse("Oid <> ? and Element =? and Sentence =? and Location is not null && Term.Video.Oid =?",
            //                    Oid, Element, Sentence, Term.Video.Oid);
            //    var termLocationList = new XPCollection<TermLocation>(PersistentCriteriaEvaluationBehavior.InTransaction, Session, criteria);

            //    foreach (var tl in termLocationList)
            //    {
            //        if (IsOverlap(tl))
            //        {
            //            if (result is null)
            //                result = new System.Collections.Generic.List<TermLocation>();
            //            result.Add(tl);
            //        }
            //    }
            //}
            //else
            //{
            //    foreach(var term in Term.Video.TermList)
            //    {
            //        foreach (var termLocation in term.TermLocationList)
            //        {
            //            if(termLocation.Oid != Oid && termLocation.Element == Element && termLocation.Sentence == Sentence && Location != null)
            //            {
            //                if (IsOverlap(termLocation))
            //                {
            //                    if (result is null)
            //                        result = new System.Collections.Generic.List<TermLocation>();
            //                    result.Add(termLocation);
            //                }
            //            }
            //        }
            //    }
            //}             

            //return result;
        }
        public static bool CheckOverlap(TermLocation termLocation, bool requireTerm)
        {
            //059: Chức năng xác định Overlap sẽ bao phủ cả các thuật vị Abbyy (hiện là các thuật vị không trỏ vào thuật ngữ)
            //if (termLocation.Term is null)
            //    return false;
            if (termLocation.Audio is null)
                return false;
            return termLocation.Audio.TermLocationList.FirstOrDefault(m => !m.Oid.Equals(termLocation.Oid) && m.Sentence == termLocation.Sentence && IsOverlap(termLocation, m, requireTerm)) != null;
        }
        private static bool IsOverlap(TermLocation termLocation, Module.BusinessObjects.TermLocation refTermLocation, bool requireTerm)
        {
            //059: Chức năng xác định Overlap sẽ bao phủ cả các thuật vị Abbyy (hiện là các thuật vị không trỏ vào thuật ngữ)
            //if (termLocation.Term is null)
            //    return false;
            // 2024 - 12 - 20: Cờ đè thuật vị không tính đè thuật vị Abbyy +> vì vậy phải requireTerm phải luôn có
            requireTerm = true;
            if (requireTerm && (termLocation.Term is null || refTermLocation.Term is null))
                return false;

            // Extract primitive values for object-independent helper
            var locA = termLocation.Location;
            var locB = refTermLocation.Location;
            if (!locA.HasValue || !locB.HasValue)
                return false;
            int leftLocation = locA.Value;
            int rightLocation = locB.Value;

            int term1WordQty = termLocation.Term?.WordQuantity ?? 0;
            int term2WordQty = refTermLocation.Term?.WordQuantity ?? 0;
            var machineA = termLocation.MachineTranslate;
            var machineB = refTermLocation.MachineTranslate;

            return IsOverlap_Compute(leftLocation, rightLocation, term1WordQty, term2WordQty, requireTerm, machineA, machineB);
        }


        public static int GetIndexContent(TermLocation termLocation, string content, string find)
        {
            try
            {
                //2023-08-14: Sử dụng Vị trí dịch để hiển thị hover chính xác, trong Thay từ và Trả lại
                if (termLocation.Location != null)
                {
                    int realLocation = termLocation.Location.Value;
                    if (termLocation.Sentence != null && termLocation.Sentence > 1)
                    {
                        var sentences = Module.Helpers.TextHelper.GetSentences(content);
                        if (sentences.Length > termLocation.Sentence.Value)
                        {
                            for (int s = 0; s < termLocation.Sentence.Value - 1 && s < sentences.Length; s++)
                            {
                                realLocation += sentences[s].Split(' ', System.StringSplitOptions.RemoveEmptyEntries).Length;
                            }
                        }
                    }
                    int space = 0;
                    //Xóa bỏ 2 dấu cách liền nhau;                
                    //var refContent = content.Replace("  ", " ");
                    for (int i = 0; i < content.Length - 1; i++)
                    {
                        if (space + 1 == realLocation)
                        {
                            var startIndex = i > 0 ? i - 1 : 0;
                            return content.IndexOf(find, startIndex, System.StringComparison.OrdinalIgnoreCase);
                            //var result = content.IndexOf(find, i, System.StringComparison.OrdinalIgnoreCase);
                            //if(result >= 0)
                            //{
                            //    var resultIndex =  content.IndexOf(find, i, System.StringComparison.OrdinalIgnoreCase);
                            //    if (resultIndex >= 0)
                            //        return resultIndex;
                            //    else
                            //        return result;
                            //}

                        }
                        //Nếu ký tự cách ở đầu hoặc trước đấy là ký tự cách thí sẽ không được tính
                        if (i > 0 && content[i] == ' ' && content[i - 1] != ' ')
                            space++;
                    }
                }


                var index = Module.Helpers.TextHelper.GetIndexWordInContent(find, content);
                if (index < 0)
                    return index;
                if (termLocation.Audio is null)
                    return -1;
                //Lấy danh sách thuật vị trùng thành phần
                var termLocationList = termLocation.Term.TermLocationList
                   .Where(m => m.Audio != null && m.Audio.Oid.Equals(termLocation.Audio.Oid) && m.Location != null)
                           .OrderBy(m => m.Location);
                if (termLocationList.Count() == 1)
                {
                    return index;
                }
                int indexInLocation = 1;
                //var indexValidate = new System.Collections.Generic.List<int>();            
                foreach (var tl in termLocationList)
                {
                    if (termLocation.Oid.Equals(tl.Oid) || index >= content.Length - 1)
                        break;
                    indexInLocation++;
                    var newIndex = Module.Helpers.TextHelper.GetIndexWordInContent(find, content, null, index + 1);
                    if (newIndex < 0)
                        break;
                    else
                        index = newIndex;
                }
                return index;
            }
            catch (System.Exception ex)
            {

            }
            return -1;

        }

        public int GetIndexByLocation(TermLocation termLocation, string sentence, string find)
        {
            try
            {
                //2023-08-14: Sử dụng Vị trí dịch để hiển thị hover chính xác, trong Thay từ và Trả lại
                if (termLocation.Location != null)
                {
                    var computedIndex = GetIndexByLocation_ComputeFromSentenceBySpaces(sentence, find, termLocation.Location.Value);
                    if (computedIndex.HasValue)
                        return computedIndex.Value;
                }


                var index = Module.Helpers.TextHelper.GetIndexWordInContent(find, sentence);
                if (index < 0)
                    return index;
                if (termLocation.Audio is null)
                    return -1;
                //Lấy danh sách thuật vị trùng thành phần
                var termLocationList = termLocation.Term.TermLocationList
                   .Where(m => m.Audio != null && m.Audio.Oid.Equals(termLocation.Audio.Oid) && m.Location != null)
                           .OrderBy(m => m.Location);
                if (termLocationList.Count() == 1)
                {
                    return index;
                }
                int indexInLocation = 1;
                //var indexValidate = new System.Collections.Generic.List<int>();            
                foreach (var tl in termLocationList)
                {
                    if (termLocation.Oid.Equals(tl.Oid) || index >= sentence.Length - 1)
                        break;
                    indexInLocation++;
                    var newIndex = Module.Helpers.TextHelper.GetIndexWordInContent(find, sentence, null, index + 1);
                    if (newIndex < 0)
                        break;
                    else
                        index = newIndex;
                }
                return index;
            }
            catch (System.Exception ex)
            {

            }
            return -1;

        }




        public string GetUnicodeWord(TermLocation termLocation)
        {
            if (termLocation?.Audio is null || termLocation.Term is null)
                return null;
            return GetUnicodeWord_Extracted(
                termLocation.Audio.Content,
                termLocation.Term.Name,
                termLocation.Location,
                termLocation.Sentence);
        }


        private string GetUnicodeWordByLocation(TermLocation termLocation, Audio audioElement, int currentLocation, int currentSentence)
        {
            if (termLocation?.Term == null || audioElement == null)
                return null;
            return GetUnicodeWordByLocation_InstanceExtracted(audioElement.Content, termLocation.Term.Name, currentLocation, currentSentence);
        }

        public bool CheckSpellingFlag(TermLocation termLocation, NHunspell.Hunspell hunspell, System.Collections.Generic.List<string> wordsList = null)
        {
            termLocation.Flag = false;
            string termUnicode = GetUnicodeWord(termLocation);
            if (!string.IsNullOrEmpty(termUnicode))
            {
                if (hunspell != null)
                    termLocation.Flag = !CheckSpelling(hunspell, termUnicode);
                if (!termLocation.Flag && wordsList != null)
                    termLocation.Flag = Module.Helpers.TextHelper.ListContains(wordsList, termUnicode) < 0;
                return termLocation.Flag;
            }
            return termLocation.Flag;
        }

        public bool CheckSpelling(NHunspell.Hunspell hunspell, string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                var words = text.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
                foreach (var word in words)
                {
                    if (!hunspell.Spell(word))
                        return false;
                }
            }
            return true;
        }

        public static string GetSentenceTextFromContent(TermLocation termLocation, string content)
        {
            return Module.Helpers.TextHelper.GetSentenceTextFromContent(termLocation.Sentence, content);
        }

        public bool ReplaceUnReplaceTranslate(TermLocation termLocation, string choice, string choiceCaption, char tag)
        {
            if (termLocation.Term is null && termLocation.Audio is null)
                return false;
            //2023-07-22 Replace: sẽ k thực hiện nếu cờ dựng           
            if (termLocation.ReplaceTranslate && choice != "UnReplace")
                return false;
            //2023-07-22 UnReplace: sẽ k thực hiện nếu cờ xóa (2 cái này có cần k, vì k tìm thấy từ thì cũng k thay được)
            if (!termLocation.ReplaceTranslate && choice == "UnReplace")
                return false;
            if (string.IsNullOrEmpty(termLocation.Translate) || string.IsNullOrEmpty(termLocation.MachineTranslate))
                return false;
            var audio = termLocation.Audio;
            string rootContent = termLocation.Term != null ? termLocation.Audio.Subtitle : termLocation.Audio.Content;
            if (audio != null && !string.IsNullOrEmpty(rootContent))
            {
                if (termLocation.Translate.Equals(termLocation.MachineTranslate, StringComparison.OrdinalIgnoreCase))
                    return false;
                string find = choice != "UnReplace" ? termLocation.MachineTranslate : termLocation.Translate;
                string replace = choice != "UnReplace" ? termLocation.Translate : termLocation.MachineTranslate;
                int firstIndex = GetIndexTranslate(termLocation, rootContent, find);
                if (firstIndex > 0 && (termLocation.TranslateLocation is null || termLocation.TranslateLocation == 0))
                {
                    var parentTerms = termLocation.Term != null ? TermService.GetParrentTerms(termLocation.Term).ToArray() : null;
                    int translateIndex = Module.Helpers.TextHelper.GetIndexWordInContent(termLocation.Translate, audio.Content, parentTerms, 0);
                    int audioContentLength = audio.Content != null ? audio.Content.Length : 1;
                    firstIndex = ReplaceUnReplaceTranslate_SelectFirstIndex(firstIndex, rootContent, find, translateIndex, audioContentLength);
                }

                var content = Module.Helpers.TextHelper.ReplaceWordInContent(rootContent, find, replace, termLocation.Term != null ? TermService.GetParrentTerms(termLocation.Term).ToArray() : null, firstIndex);
                if (!rootContent.Equals(content, StringComparison.OrdinalIgnoreCase))
                {
                    if (termLocation.Term != null)
                        termLocation.Audio.Subtitle = content;
                    else
                        termLocation.Audio.Content = content;
                    //Thay xong: dựng cờ Thuật vị 
                    termLocation.ReplaceTranslate = choice == "Replace" ? true : false;
                    if (choice == "Replace")
                    {
                        if (termLocation.Term != null)
                        {
                            if (termLocation.Term.Status is null || (termLocation.Term.Status != null && !"Replace".Equals(termLocation.Term.Status.Code)))
                            {
                                var status = termLocation.Session.FindObject<Module.SystemObjects.Status>(DevExpress.Data.Filtering.CriteriaOperator.Parse("Code = 'Replace'"));
                                if (status != null)
                                    termLocation.Term.Status = status;
                                //049: Khi thực hiện Thay thế (Dịch > Máy dịch) cho các thuật vị, cần dựng cờ Thành phần có thay thế để biết và nạp lại Phiên âm, âm thanh cho thành phần đó
                                audio.Flag = true;
                                //Xóa Note nếu có dữ liệu
                                if (!string.IsNullOrEmpty(audio.Note))
                                    audio.Note = Module.Helpers.TextHelper.GetTextWithTagNode(audio.Note, tag, false);
                                audio.Note = Module.Helpers.TextHelper.AddTextWithTagNode(audio.Note, tag, choiceCaption);
                            }
                        }
                        else
                        {
                            //Xóa Note nếu có dữ liệu
                            //if (!string.IsNullOrEmpty(audio.Note))
                            //    audio.Note = Module.Helpers.TextHelper.GetTextWithTagNode(audio.Note, '<', false);
                            audio.Note = Module.Helpers.TextHelper.AddTextWithTagNode(audio.Note, tag, choiceCaption);
                        }

                    }
                    else
                    {
                        if (audio.Flag)
                        {
                            audio.Flag = false;
                            //audio.Note = audio.Note.Replace("<Thay dịch>", "");
                            audio.Note = Module.Helpers.TextHelper.AddTextWithTagNode(audio.Note, tag, choiceCaption);
                        }
                    }
                    //2023-08-15: - Chức năng Thay dịch: khi số từ của Dịch khác Dịch máy
                    //      > cập nhật toàn bộ những từ đã dịch đứng sau (có thể là Dịch máy/hoặc Dịch
                    if (termLocation.Term != null && find.Split(' ').Length != replace.Split(' ').Length)
                        UpdateAfterTranslateLocation(termLocation);
                    return true;
                }
                //2023-08-09: Bỏ lựa chọn thay theo ngữ cảnh
                //2023-08-02: Tách thành 2 option:
                //if (choice == "ContextReplace")
                //{
                //    //2023-08-02-Ngữ cảnh: hoa / thường theo ngữ cảnh máy dịch
                //    var content =Module.Helpers.TextHelper.ReplaceWordInContent(rootContent, find, replace, Term.GetParrentTerms().ToArray());
                //    if (!rootContent.Equals(content, StringComparison.OrdinalIgnoreCase))
                //    {
                //        rootContent = content;
                //        //Thay xong: dựng cờ Thuật vị 
                //        Flag = choice != "Replace" ? true : false;
                //        return true;
                //    }
                //}else if (choice == "Replace" || choice == "UnReplace")
                //{
                //    //2023-08-02-Nguyên gốc: y hệt giá trị dịch, trừ trường hợp dịch là chữ thường nhưng đầu câu thì bắt buộc hoa
                //    if (find.Equals(replace, StringComparison.OrdinalIgnoreCase))
                //    {
                //        rootContent = replace;
                //        //Thay xong: dựng cờ Thuật vị 
                //        Flag = choice == "Replace" ? true : false;
                //        return true;
                //    }
                //    else
                //    {
                //        int startIndex = 0;
                //        var content = rootContent;
                //        var invalidsText = Term.GetParrentTerms().ToArray();
                //        string replaceUpperFirst = replace;
                //        if(replaceUpperFirst.Length > 1)
                //            replaceUpperFirst = replaceUpperFirst[0].ToString().ToUpper() + replaceUpperFirst.Substring(1);
                //        else
                //            replaceUpperFirst = replaceUpperFirst.ToUpper();

                //        while (startIndex < content.Length - 1)
                //        {
                //            var index = content.IndexOf(find, startIndex, System.StringComparison.OrdinalIgnoreCase);
                //            if (index < 0)
                //                break;
                //            startIndex = index + find.Length;
                //            if (!Module.Helpers.TextHelper.CheckWordIndexIsValidateInContent(content, find, index))
                //                continue;
                //            else if (invalidsText != null && invalidsText.Length > 0)
                //            {
                //                if (!Module.Helpers.TextHelper.CheckCurrentIndexIsNotParentIndex(content, find, index, invalidsText))
                //                    continue;
                //            }
                //            if (index >= 0)
                //            {
                //                //Xử lý thay từ
                //                string beforeContent = content.Substring(0, index);
                //                string textFind = content.Substring(index, find.Length);
                //                if (replace.Length > 0)
                //                {
                //                    if(Module.SystemObjects.Tools.CheckIndexIsNewLine(content, index))
                //                    {
                //                        //- Nguyên gốc: y hệt giá trị dịch, trừ trường hợp dịch là chữ thường nhưng đầu câu thì bắt buộc hoa
                //                        beforeContent += replaceUpperFirst;
                //                    }
                //                    else
                //                    {
                //                        beforeContent += replace;
                //                    }                                                                           
                //                }
                //                content = beforeContent + content.Substring(startIndex);
                //            }
                //        }
                //        if (!rootContent.Equals(content))
                //        {
                //            if (termLocation.Term.Status is null || (termLocation.Term.Status != null && !"Replace".Equals(termLocation.Term.Status.Code)))
                //            {
                //                var status = Session.FindObject<Module.SystemObjects.Status>(DevExpress.Data.Filtering.CriteriaOperator.Parse("Code = 'Replace'"));
                //                if (status != null)
                //                    Term.Status = status;
                //            }

                //            rootContent = content;
                //            //Thay xong: dựng cờ Thuật vị 
                //            Flag = choice == "Replace" ? true : false;
                //            return true;
                //        }

                //    }                     

                //}
            }
            return false;
        }

        public void TranslateTermLocation(TermLocation termLocation, ref int susscess, ref int termLocationCount, string option = "TranslateTermContextApostrophe", XafApplication application = null)
        {
            //Mặc định là dịch theo dấu nháy
            //-Ngữ cảnh: Tìm từ chung trong các câu bên Dịch nội dung ứng với Thuật vị,
            //2023-06-29 So sánh dịch theo google translate 2 lần
            string seperateKey = "TranslateTermContextSlash".Equals(option) ? "/" : "'";
            string endSeperateKey = seperateKey;
            if (option.Equals("TranslateTermContextStrong"))
            {
                seperateKey = "<strong> ";
                endSeperateKey = "</strong>";
            }


            //seperateKey = "'";
            //2024-06-18:Máy dịch không quan tâm đến điều kiện            
            //if (termLocation.Term != null && !string.IsNullOrEmpty(termLocation.Term.Name) && (string.IsNullOrEmpty(termLocation.Term.Translate) || string.IsNullOrEmpty(termLocation.Term.GoogleTranslate)))
            if (termLocation.Term != null && !string.IsNullOrEmpty(termLocation.Term.Name))
            {
                System.Collections.Generic.IDictionary<string, int> dictionaryResult = new System.Collections.Generic.Dictionary<string, int>();
                // 2023 - 07 - 27: Bỏ: Cờ thuật vị chỉ cần dùng trong Thay từ
                //termLocation.Flag = false;
                bool termLocationFlag = false;
                var audio = termLocation.Audio;
                if (audio is null)
                    return;
                if (string.IsNullOrEmpty(audio.Subtitle) || string.IsNullOrEmpty(audio.Content))
                    return;
                //2023-08-09: Dịch > Máy dịch của Thuật vị không thực hiện khi Máy dịch của Thuật ngữ khác null > cần cho phép
                //if (!string.IsNullOrEmpty(MachineTranslate))
                //    return;
                string newTranlate = null;
                //foreach (var key in dictionaryResult.Keys)
                //{
                //    var index = Module.Helpers.TextHelper.GetIndexWordInContent(key, audio.Subtitle);
                //    if (index >= 0)
                //    {
                //        newTranlate = audio.Subtitle.Substring(index, key.Length);
                //    }
                //}
                var subtitle = GetSentenceTextFromContent(termLocation, audio.Subtitle).Replace(seperateKey, " ");
                if (string.IsNullOrEmpty(newTranlate))
                {
                    var content = GetSentenceTextFromContent(termLocation, audio.Content).Replace(seperateKey, " ");
                    string newContent = BuildMarkedContentForTranslation(termLocation.Term.Name, content, seperateKey, endSeperateKey);
                    var newtranlateContent = Module.SystemObjects.Tools.TranslateText(newContent);
                    if (string.IsNullOrEmpty(newtranlateContent))
                        return;
                    newTranlate = ParseTranslatedContent(newtranlateContent, subtitle, seperateKey, endSeperateKey, content);

                    if (string.IsNullOrEmpty(newTranlate))
                    {
                        //Dịch thử thông thường
                        var gTranslate = Module.SystemObjects.Tools.TranslateText(termLocation.Term.Name);
                        int newStartIndex = subtitle.IndexOf(gTranslate, System.StringComparison.OrdinalIgnoreCase);
                        if (newStartIndex >= 0)
                        {
                            newTranlate = gTranslate;
                            newTranlate = subtitle.Substring(newStartIndex, newTranlate.Length);
                        }
                        else
                        {
                            //Nếu giữ nguyên từ gốc
                            if (Module.Helpers.TextHelper.GetIndexWordInContent(termLocation.Term.Name, subtitle) >= 0)
                                newTranlate = termLocation.Term.Name;
                            //2023-07-27: Bỏ : Cờ thuật vị chỉ cần dùng trong Thay từ
                            //termLocation.Flag = true;
                            termLocationFlag = true;
                            //if (System.Diagnostics.Debugger.IsAttached)
                            //    termLocation.Translate = newtranlateContent;
                        }
                    }
                    if (System.Diagnostics.Debugger.IsAttached)
                    {
                        if (string.IsNullOrEmpty(newTranlate) && application != null)
                            _notificationService.NotifyWarning("Debug: ",
                                newtranlateContent + System.Environment.NewLine + subtitle);
                    }
                }
                if (!string.IsNullOrEmpty(newTranlate))
                {
                    susscess++;
                    newTranlate = newTranlate.Trim();
                    if (newTranlate.Equals(termLocation.Term.Name, System.StringComparison.OrdinalIgnoreCase))
                        newTranlate = termLocation.Term.Name;
                    termLocation.MachineTranslate = newTranlate;
                    //2023 - Khi dịch máy manual trên Thuật ngữ hoặc Thuật vị sẽ xác định bằng tìm kiếm nếu kết quả thấy 1 thì cập nhật
                    //  , 0 thấy hoặc 2 trở lên thì phải cập nhật Vị trí dịch bằng manual
                    int count = 0;
                    var index = -1;
                    while (true)
                    {
                        var newIndex = Module.Helpers.TextHelper.GetIndexWordInContent(newTranlate, subtitle, null, index + 1);
                        if (newIndex < 0)
                            break;
                        index = newIndex;
                        count++;
                    }
                    if (count == 1)
                    {
                        var firstText = subtitle.Substring(0, index);
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
                    //termLocation.Flag = subtitle.IndexOf(newTranlate, System.StringComparison.OrdinalIgnoreCase) < 0;
                    if (!termLocationFlag)
                        termLocationFlag = subtitle.IndexOf(newTranlate, System.StringComparison.OrdinalIgnoreCase) < 0;
                    var key = Module.Helpers.TextHelper.KeyListContains(dictionaryResult.Keys, newTranlate);
                    if (string.IsNullOrEmpty(key))
                    {
                        var newTranlates = Module.SystemObjects.Tools.TranslateText(termLocation.Term.Name);
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
                    //2024-07-29: Máy dịch đang dựng cờ thuật vị khi không tìm thấy từ, bỏ hành động này vì không thấy thì trường máy dịch trống là biết rồi
                    //Flag = true;
                }
                //Nếu dịch máy trống thì thực hiện
                if (dictionaryResult.Keys.Count > 0 && string.IsNullOrEmpty(termLocation.Term.GoogleTranslate))
                {

                    ////Xóa trắng dịch máy 
                    //GoogleTranslate = null;
                    int max = 0;
                    string maxKey = null;
                    foreach (var key in dictionaryResult.Keys)
                    {
                        if (dictionaryResult[key] == 0)
                        {
                            //2024-07-29: Máy dịch đang dựng cờ thuật vị khi không tìm thấy từ, bỏ hành động này vì không thấy thì trường máy dịch trống là biết rồi
                            //Flag = true;
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
                        termLocation.Term.GoogleTranslate = maxKey;
                    }
                }
            }
        }


        public static int GetIndexTranslate(TermLocation termLocation, string content, string translate)
        {
            if (termLocation.Audio is null)
                return -1;
            if (termLocation.Term is null)
                return Module.Helpers.TextHelper.GetIndexWordInContent(translate, content);

            // Prepare primitive inputs for extracted helper
            var orderedList = termLocation.Term.TermLocationList
                .Where(m => m.Audio != null && m.Audio.Oid.Equals(termLocation.Audio.Oid) && m.Location != null
                    && (translate.Equals(m.Translate, System.StringComparison.OrdinalIgnoreCase) || translate.Equals(m.MachineTranslate, System.StringComparison.OrdinalIgnoreCase)))
                .OrderBy(m => m.TranslateLocation).ThenBy(m => m.Location).ToList();

            int totalMatches = orderedList.Count;
            int occurrenceIndex = 1;
            if (totalMatches > 0)
            {
                for (int i = 0; i < orderedList.Count; i++)
                {
                    if (termLocation.Oid.Equals(orderedList[i].Oid))
                    {
                        occurrenceIndex = i + 1; // 1-based
                        break;
                    }
                }
            }

            return GetIndexTranslate_Compute(termLocation.TranslateLocation, content, translate, occurrenceIndex, totalMatches);
        }

        public int InsertWord(TermLocation termLocation, int totalObjectCount, bool before, string word)
        {
            //Theo dịch máy hoặc dịch nếu Đã thay
            string translate = string.IsNullOrEmpty(termLocation.Translate) ? termLocation.MachineTranslate : termLocation.Translate;
            if (string.IsNullOrEmpty(translate) || termLocation.Term is null)
            {
                if (totalObjectCount == 1)
                {
                    _notificationService.NotifyError("Thông báo", "Không tìm thấy dữ liệu dịch");
                    return 0;
                }
            }
            var audio = termLocation.Audio;
            if (audio is null)
                return 0;
            if (string.IsNullOrEmpty(audio.Subtitle))
                return 0;
            if (!termLocation.Term.TermLocationList.Count.Equals(termLocation.Term.Quantity))
            {
                _notificationService.NotifyWarning("Thông báo", "Số lượng thuật vị không khớp, vui lòng cập nhật thuật vị trước khi sử dụng tính năng này");
                return 0;
            }
            var index = GetIndexTranslate(termLocation, audio.Subtitle, translate);
            if (index >= 0)
            {
                //if (e.SelectedChoiceActionItem.Id.Equals("InsertBefore"))
                if (before)
                {
                    var computed = InsertWord_ComputeNewSubtitle(audio.Subtitle, index, true, word, translate);
                    if (index == 0)
                    {
                        var termName = termLocation.Term.Name;
                        var trans = termLocation.Translate;
                        var machine = termLocation.MachineTranslate;
                        var subtitleTmp = computed;
                        InsertWord_AdjustCasingForInsertAtStart(word, ref termName, ref trans, ref machine, ref subtitleTmp);
                        termLocation.Term.Name = termName;
                        termLocation.Translate = trans;
                        termLocation.MachineTranslate = machine;
                        audio.Subtitle = subtitleTmp;
                    }
                    else
                    {
                        audio.Subtitle = computed;
                    }
                    //2023-15-08
                    //Cập nhật trường Vị trí dịch trong Thuật vị
                    //- CN Soạn thảo:
                    //+Chèn / Xóa trước: thuật ngữ và những từ đứng sau
                    //+ Chèn / Xóa sau: những từ đứng sau
                    UpdateTranslateLocation(termLocation);
                    UpdateAfterTranslateLocation(termLocation, audio);
                    return 1;
                }
                else //if (e.SelectedChoiceActionItem.Id.Equals("InsertAfter"))
                {
                    // Use helper which will account for translate length
                    var computed = InsertWord_ComputeNewSubtitle(audio.Subtitle, index, false, word, translate);
                    if (!string.Equals(computed, audio.Subtitle, StringComparison.Ordinal))
                    {
                        audio.Subtitle = computed;
                        UpdateAfterTranslateLocation(termLocation, audio);
                        return 1;
                    }
                    else
                    {
                        if (totalObjectCount == 1)
                        {
                            _notificationService.NotifyError("Lỗi", "Không thể xóa sau từ");
                            return 0;
                        }
                    }
                }
            }
            else
            {
                if (totalObjectCount == 1)
                {
                    _notificationService.NotifyError("Lỗi", "Không tìm thấy từ dịch");
                    return 0;
                }
            }
            return 0;
        }
        public int DeleteWord(TermLocation termLocation, int totalObjectCount, bool before)
        {
            //Theo dịch máy hoặc dịch nếu Đã thay
            string translate = string.IsNullOrEmpty(termLocation.Translate) ? termLocation.MachineTranslate : termLocation.Translate;
            if (string.IsNullOrEmpty(translate) || termLocation.Term is null)
            {
                if (totalObjectCount == 1)
                {
                    _notificationService.NotifyError("Thông báo", "Không tìm thấy dữ liệu dịch");
                    return 0;
                }
            }
            var audio = termLocation.Audio;
            if (audio is null)
                return 0;
            if (string.IsNullOrEmpty(audio.Subtitle))
                return 0;
            if (!termLocation.Term.TermLocationList.Count.Equals(termLocation.Term.Quantity))
            {
                _notificationService.NotifyWarning("Thông báo", "Số lượng thuật vị không khớp, vui lòng cập nhật thuật vị trước khi sử dụng tính năng này");
                return 0;
            }
            var index = GetIndexTranslate(termLocation, audio.Subtitle, translate);
            if (index >= 0)
            {
                //if (e.SelectedChoiceActionItem.Id.Equals("DeleteBefore"))
                if (before)
                {
                    if (index > 1)
                    {
                        if (DeleteWord_ComputeBefore(audio.Subtitle, index, out var newSubtile, out var removedWord))
                        {
                            audio.Subtitle = newSubtile;
                            //2023-15-08
                            //Cập nhật trường Vị trí dịch trong Thuật vị
                            UpdateTranslateLocation(termLocation);
                            UpdateAfterTranslateLocation(termLocation, audio);
                            if (totalObjectCount == 1 && !string.IsNullOrEmpty(removedWord))
                            {
                                _notificationService.NotifySuccess("Kết quả", "Đã xóa từ: " + removedWord);
                                return 1;
                            }
                            return 1;
                        }
                        else
                        {
                            if (totalObjectCount == 1)
                            {
                                _notificationService.NotifyError("Lỗi", "Không thể xóa trước từ");
                                return 0;
                            }
                        }
                    }
                    else
                    {
                        if (totalObjectCount == 1)
                        {
                            _notificationService.NotifyError("Lỗi", "Không thể xóa trước từ");
                            return 0;
                        }

                    }
                }
                else //if (e.SelectedChoiceActionItem.Id.Equals("DeleteAfter"))
                {
                    // call helper (helper will account for translate length)
                    if (DeleteWord_ComputeAfter(audio.Subtitle, index, translate, out var newSubtileAfter, out var removedWordAfter))
                    {
                        audio.Subtitle = newSubtileAfter;
                        UpdateAfterTranslateLocation(termLocation, audio);
                        if (totalObjectCount == 1 && !string.IsNullOrEmpty(removedWordAfter))
                        {
                            _notificationService.NotifySuccess("Kết quả", "Đã xóa từ: " + removedWordAfter);
                            return 1;
                        }
                        return 1;
                    }
                    else
                    {
                        if (totalObjectCount == 1)
                        {
                            _notificationService.NotifyError("Lỗi", "Không thể xóa sau từ");
                            return 0;
                        }
                    }
                }
            }
            else
            {
                if (totalObjectCount == 1)
                {
                    _notificationService.NotifyError("Lỗi", "Không tìm thấy từ dịch");
                    return 0;
                }
            }
            return 0;
        }

        public int MoveWord(TermLocation termLocation, int totalObjectCount, bool forward)
        {
            //Theo dịch máy hoặc dịch nếu Đã thay
            string translate = string.IsNullOrEmpty(termLocation.Translate) ? termLocation.MachineTranslate : termLocation.Translate;
            if (string.IsNullOrEmpty(translate) || termLocation.Term is null)
            {
                if (totalObjectCount == 1)
                {
                    _notificationService.NotifyError("Thông báo", "Không tìm thấy dữ liệu dịch");
                    return 0;
                }
            }
            var audio = termLocation.Audio;
            if (audio is null)
                return 0;
            if (string.IsNullOrEmpty(audio.Subtitle))
                return 0;
            if (!termLocation.Term.TermLocationList.Count.Equals(termLocation.Term.Quantity))
            {
                _notificationService.NotifyWarning("Thông báo", "Số lượng thuật vị không khớp, vui lòng cập nhật thuật vị trước khi sử dụng tính năng này");
                return 0;
            }
            var index = GetIndexTranslate(termLocation, audio.Subtitle, translate);
            if (index >= 0)
            {
                //if (e.SelectedChoiceActionItem.Id.Equals("DeleteBefore"))
                if (!forward)
                {
                    if (index > 1)
                    {
                        if (MoveWord_ComputeBackward(audio.Subtitle, index, translate, out var newSubtile, out var swappedWord))
                        {
                            audio.Subtitle = newSubtile;
                            if (totalObjectCount == 1 && !string.IsNullOrEmpty(swappedWord))
                            {
                                _notificationService.NotifySuccess("Kết quả", "Đã đổi vị trí với từ: " + swappedWord);
                                return 1;
                            }
                            return 1;
                        }
                        else
                        {
                            if (totalObjectCount == 1)
                            {
                                _notificationService.NotifyError("Lỗi", "Không thể đổi vị trí từ");
                                return 0;
                            }
                        }
                    }
                    else
                    {
                        if (totalObjectCount == 1)
                        {
                            _notificationService.NotifyError("Lỗi", "Không thể đổi vị trí từ");
                            return 0;
                        }

                    }
                }
                else
                {
                    if (MoveWord_ComputeForward(audio.Subtitle, index, translate, out var newSubtileF, out var swappedWordF))
                    {
                        audio.Subtitle = newSubtileF;
                        if (totalObjectCount == 1 && !string.IsNullOrEmpty(swappedWordF))
                        {
                            _notificationService.NotifySuccess("Kết quả", "Đã đổi vị trí với từ: " + swappedWordF);
                            return 1;
                        }
                        return 1;
                    }
                    else
                    {
                        if (totalObjectCount == 1)
                        {
                            _notificationService.NotifyError("Lỗi", "Không thể đổi vị trí từ");
                            return 0;
                        }
                    }
                }
            }
            else
            {
                if (totalObjectCount == 1)
                {
                    _notificationService.NotifyError("Lỗi", "Không tìm thấy từ dịch");
                    return 0;
                }
            }
            return 0;
        }


        public static void UpdatePositionLocation(TermLocation termLocation, bool requireTerm, bool byName = false, bool exactSentence = true, System.Collections.Generic.List<string> parrentTerms = null, System.Collections.Generic.List<TermLocation> beforeTermLocationList = null)
        {
            if (termLocation.Term is null || termLocation.Audio is null || termLocation.Term.Video is null)
                return;
            if (string.IsNullOrEmpty(termLocation.Audio.Content) || string.IsNullOrEmpty(termLocation.Term.Name)) return;
            if (byName && parrentTerms is null)
            {
                parrentTerms = TermService.GetParrentTerms(termLocation.Term);
            }
            if (beforeTermLocationList is null && termLocation.Location != null)
            {
                beforeTermLocationList = termLocation.Term.TermLocationList.Where(m => !termLocation.Oid.Equals(m.Oid) && termLocation.Audio.Oid.Equals(m.Audio?.Oid) && m.Location < termLocation.Location).OrderByDescending(m => m.Location).ToList();
            }

            //int position = 0;
            //Cắt theo dòng                    
            var sentencesArray = Module.Helpers.TextHelper.GetSentences(termLocation.Audio?.Content);
            bool updated = false;
            for (int m = 0; m < sentencesArray.Count(); m++)
            {
                var sentence = m + 1;
                if (exactSentence && termLocation.Sentence != sentence)
                    continue;
                var content = sentencesArray[m].Trim();
                int startIndex = 0;
                //2023-07-25 bỏ yêu cầu này
                //Nếu là phi thuật thì phải tìm đúng từ
                //int termIndex = content.IndexOf(Name, NoneTerm ? System.StringComparison.Ordinal : System.StringComparison.OrdinalIgnoreCase);
                int termIndex = content.IndexOf(termLocation.Term.Name, System.StringComparison.OrdinalIgnoreCase);
                while (termIndex >= 0)
                {
                    int startIndexLocal;
                    bool validate = UpdatePositionLocation_EvaluateStart(content, termLocation.Term.Name, termIndex, parrentTerms, out startIndexLocal);
                    startIndex = startIndexLocal;
                    if (validate)
                    {
                        //Cập nhật vị trí
                        //Tìm đến vị trí dấu cách trước đó
                        int termpTermIndex = TermService.GetRealIndexBySpace(content, termIndex);
                        string beforeContent = content.Substring(0, termpTermIndex);
                        //Vị trí là vị trí của từ trong câu
                        var currentPosition = ComputePositionFromBeforeContent(beforeContent);
                        bool flag = false;
                        bool overlap = false;
                        if (validate && termLocation.Audio.TermLocationList?.Count > 0)
                        {
                            validate = VideoService.CheckAndUpdateLocationInTermIsValidate(termLocation.Term.Video, termLocation.Term.Name, termLocation.Audio, sentence, currentPosition, requireTerm, ref overlap, ref flag, false, true);
                        }

                        //Kiểm tra danh sách những cái đã update có thành phần này chưa
                        if (validate && !UpdatePositionLocation_IsDuplicate(beforeTermLocationList, sentence, currentPosition))
                        {
                            termLocation.Location = currentPosition;
                            termLocation.Sentence = sentence;
                            termLocation.Overlap = overlap;
                            //Xem xét dựng cờ
                            termLocation.Flag = flag;
                            beforeTermLocationList.Add(termLocation);
                            updated = true;
                            break;
                        }
                    }
                    termIndex = content.IndexOf(termLocation.Term.Name, startIndex, System.StringComparison.OrdinalIgnoreCase);
                }
                if (updated)
                    break;
                //position += rows[m].Split(' ', System.StringSplitOptions.RemoveEmptyEntries).Length;
            }
        }
        public void UpdateAfterTranslateLocation(TermLocation termLocation, Module.BusinessObjects.Audio audio = null)
        {
            if (termLocation.Term is null || termLocation.Audio is null || termLocation.Location is null)
                return;
            if (audio is null)
                audio = termLocation.Audio;
            //Tìm thuật ngữ của từ tìm thấy
            var criteria = DevExpress.Data.Filtering.CriteriaOperator.Parse("Audio.Oid = ?", audio.Oid);
            criteria = DevExpress.Data.Filtering.CriteriaOperator.And(criteria,
                    DevExpress.Data.Filtering.CriteriaOperator.Parse("TranslateLocation > ? and Term <> ? ", termLocation.TranslateLocation, termLocation.Term));
            var termLocations = new XPCollection<Module.BusinessObjects.TermLocation>
                (termLocation.Session, criteria, new DevExpress.Xpo.SortProperty[] {
                    new DevExpress.Xpo.SortProperty("TranslateLocation", DevExpress.Xpo.DB.SortingDirection.Ascending) }).ToList();
            foreach (var tl in termLocations)
            {
                UpdateTranslateLocation(tl);
            }
        }

        public void UpdateTranslateLocation(TermLocation termLocation, System.Collections.Generic.List<string> parrentTerms = null, TermLocation beforeTermLocation = null)
        {
            if (termLocation.Term is null || termLocation.Audio is null)
                return;

            string translate = string.IsNullOrEmpty(termLocation.Translate) ? termLocation.MachineTranslate : termLocation.Translate;
            var element = termLocation.Audio;
            if (string.IsNullOrEmpty(element.Subtitle) || string.IsNullOrEmpty(translate)) return;
            if (parrentTerms is null)
            {
                parrentTerms = TermService.GetParrentTerms(termLocation.Term);
            }
            if (beforeTermLocation is null && termLocation.TranslateLocation != null)
            {
                //beforeTermLocation = Term.TermLocationList.OrderByDescending(m => m.TranslateLocation).Where(m => m.TranslateLocation < TranslateLocation).FirstOrDefault();
                beforeTermLocation = termLocation.Term.TermLocationList.Where(m => m.TranslateLocation < termLocation.TranslateLocation).OrderByDescending(m => m.TranslateLocation).FirstOrDefault();
            }
            int position = 0;
            //Cắt theo dòng                    
            var rows = element.Subtitle.Split(Module.Helpers.TextHelper.NewLineText, System.StringSplitOptions.RemoveEmptyEntries);
            for (int m = 0; m < rows.Count(); m++)
            {
                var content = rows[m].Trim();
                int startIndex = 0;
                //2023-07-25 bỏ yêu cầu này
                //Nếu là phi thuật thì phải tìm đúng từ
                //int termIndex = content.IndexOf(Name, NoneTerm ? System.StringComparison.Ordinal : System.StringComparison.OrdinalIgnoreCase);
                int termIndex = content.IndexOf(translate, System.StringComparison.OrdinalIgnoreCase);
                while (termIndex >= 0)
                {
                    // Use extracted helper for pure string validation
                    bool validate = UpdateTranslateLocation_IsValidWord(content, translate, termIndex, parrentTerms);
                    startIndex = termIndex + (translate?.Length ?? 0);
                    if (validate)
                    {
                        //Cập nhật vị trí
                        string beforeContent = rows[m].Substring(0, termIndex);
                        //Thêm 1 là thêm vị trí hiện tại
                        var currentPosition = position + beforeContent.Split(' ', System.StringSplitOptions.RemoveEmptyEntries).Length + 1;
                        if (beforeTermLocation is null || !(beforeTermLocation != null && termLocation.Audio == beforeTermLocation.Audio &&
                                currentPosition == beforeTermLocation.TranslateLocation))
                        {
                            termLocation.TranslateLocation = currentPosition;
                            break;
                        }
                        else
                        {
                            //Trường hợp trùng vị trí với vị trí trước đó thì phải bỏ qua
                        }
                    }
                    termIndex = content.IndexOf(translate, startIndex, System.StringComparison.OrdinalIgnoreCase);
                }
                position += rows[m].Split(' ', System.StringSplitOptions.RemoveEmptyEntries).Length;
            }
        }

        public bool SynTerm(TermLocation termLocation, bool isName)
        {
            //2023-08-11 Không quan tâm đến độ dài thay đổi (ví dụ 1 từ biến thành 2 từ làm sai sẽ bị cấm trong hướng dẫn)
            //Sẽ thay Tên và Dịch tại các vị trí như thuật vị theo đúng giá trị hiện tại trong Thuật ngữ,
            //      trừ trường hợp đầu câu thì phải chuyển viết hoa nếu giá trị là viết thường
            //Thay theo Vị trí của thuật vị và Độ dài(mấy từ đơn) của Thuật ngữ,
            //      nếu độ dài thay đổi thì cập nhật vị trí của tất cả các thuật ngữ thuộc cùng thành phần: dùng Method chung
            //Thay xong cập nhật giá trị của Độ dài nếu Tên sửa có độ dài khác
            //Đồng bộ tên: dùng cho 2 trường hợp: Sửa thuật ngữ thành từ khác, Đồng bộ viết hoa cho Thuật ngữ
            //Với Đồng bộ dịch thì phải theo dịch của từng thuật vị và không quan tâm Độ dài vì không sửa
            if (termLocation.Term is null || termLocation.Audio is null)
                return false;
            if (isName)
            {
                if (termLocation.Location is null)
                    return false;
            }
            else
            {
                if (termLocation.TranslateLocation is null)
                    termLocation.TranslateLocation = termLocation.GetDefaultTranslateLocation();
                if (termLocation.TranslateLocation is null)
                    return false;
            }

            string content = isName ? termLocation.Audio.Content : termLocation.Audio.Subtitle;
            string findText = isName ? termLocation.Term.Name : termLocation.MachineTranslate;
            if (string.IsNullOrEmpty(content))
                return false;

            int termLength = findText.Split(' ').Length;
            int location = isName ? termLocation.Location.Value : termLocation.TranslateLocation.Value;
            string result = string.Empty;
            int position = 0;

            //Cắt theo dòng

            if (location == 1)
            {
                var updated = SynTerm_HandleStartOfSentence(content, findText, termLength);
                if (updated != null)
                {
                    if (isName)
                        termLocation.Audio.Content = updated;
                    else
                        termLocation.Audio.Subtitle = updated;
                    return true;
                }
                return false;
            }
            int startIndex;
            int endIndex;
            string lastedChar;
            if (SynTerm_FindIndices(content, findText, location, termLength, out startIndex, out endIndex, out lastedChar))
            {
                var newContent = SynTerm_BuildUpdatedContent(content, findText, startIndex, endIndex, lastedChar);
                if (isName)
                    termLocation.Audio.Content = newContent;
                else
                    termLocation.Audio.Subtitle = newContent;
                return true;
            }
            if (startIndex > 0)
            {
                var newContent = SynTerm_BuildUpdatedContent(content, findText, startIndex, endIndex, lastedChar);
                if (isName)
                    termLocation.Audio.Content = newContent;
                else
                    termLocation.Audio.Subtitle = newContent;
                return true;
            }
            return false;
        }


        private static int CancelWrongTerm_ComputeScore(TermLocation overlapTermLocation, Term termSelect, int currentLocation, System.Collections.Generic.List<TermLocation> overlapList)
        {
            int termPoint = 0;
            termPoint += Spelling(overlapTermLocation, termSelect.Video, termSelect.Name);
            termPoint += ExistWord(overlapTermLocation, termSelect.Video, termSelect.Name);
            termPoint += Longer(overlapTermLocation, termSelect.Video, termSelect.Name);
            termPoint += MoreOverlap(overlapTermLocation, termSelect.Video, overlapList, true);
            termPoint += NoneOverlapPart(overlapTermLocation, termSelect.Video, termSelect.Name, currentLocation);
            termPoint += OverlapCaseType(overlapTermLocation, termSelect.Video, termSelect.Name);
            return termPoint;
        }

        private static void CancelWrongTerm_HandleNegative(TermLocation overlapTermLocation, ref int deleteTerm, ref int deleteTermLocation)
        {
            //Loại bỏ bên sai
            if (overlapTermLocation.Term.Quantity == 1 || overlapTermLocation.Term.TermLocationList.Count == 1)
            {
                overlapTermLocation.Term.Delete();
                deleteTerm++;
            }
            else
                overlapTermLocation.Term.Quantity--;
            overlapTermLocation.Delete();
            deleteTermLocation++;
        }


        #endregion SourceCode4550ImportCode

        #region SourceCode4552ImportCode
        
        private static string BuildMarkedContentForTranslation(string termName, string content, string seperateKey, string endSeperateKey)
        {
            if (string.IsNullOrEmpty(content) || string.IsNullOrEmpty(termName))
                return content ?? string.Empty;
            var newContent = string.Empty;
            var firstIndex = 0;
            var index = content.IndexOf(termName, System.StringComparison.OrdinalIgnoreCase);
            while (index >= 0)
            {
                newContent += content.Substring(firstIndex, index - firstIndex);
                firstIndex = index + termName.Length;
                if (Module.Helpers.TextHelper.CheckWordIndexIsValidateInContent(content, termName, index))
                {
                    newContent += seperateKey + content.Substring(index, termName.Length) + endSeperateKey;
                }
                else
                {
                    newContent += content.Substring(index, termName.Length);
                }
                if (firstIndex >= content.Length)
                    break;
                index = content.IndexOf(termName, firstIndex, System.StringComparison.OrdinalIgnoreCase);
            }
            newContent += content.Substring(firstIndex);
            return newContent;
        }
        private static int CompareByWordCount(string termName1, string termName2)
        {
            // Core logic independent of TermLocation/Video objects: compare word counts
            var termNameLength = termName1?.Split(' ', System.StringSplitOptions.RemoveEmptyEntries).Length ?? 0;
            var termName2Length = termName2?.Split(' ', System.StringSplitOptions.RemoveEmptyEntries).Length ?? 0;
            if (termNameLength == termName2Length)
                return 0;
            return termNameLength > termName2Length ? 1 : -1;
        }

        // Helper for UpdatePositionLocation: compute position from beforeContent
        private static int ComputePositionFromBeforeContent(string beforeContent)
        {
            if (string.IsNullOrEmpty(beforeContent))
                return 1;
            return beforeContent.Split(' ', System.StringSplitOptions.RemoveEmptyEntries).Length + 1;
        }

        private static bool DeleteWord_ComputeAfter(string subtitle, int index, string translate, out string newSubtitle, out string removedWord)
        {
            newSubtitle = subtitle;
            removedWord = string.Empty;
            if (string.IsNullOrEmpty(subtitle) || index < 0 || index > subtitle.Length)
                return false;

            index += translate.Length;
            if (index < subtitle.Length - 1)
            {
                var newSubtile = subtitle.Substring(0, index);
                int afterIndex = -1;
                string word = "";
                for (int j = index + 1; j < subtitle.Length; j++)
                {
                    if ((char.IsLetterOrDigit(subtitle[j]) || Module.Helpers.TextHelper.CheckSpecialCharactersValidate(subtitle[j])) &&
                        !(j + 1 < subtitle.Length && subtitle[j + 1] == ' ' && (subtitle[j] == '.' || subtitle[j] == ',')))
                    {
                        word += subtitle[j];
                    }
                    else
                    {
                        afterIndex = j;
                        break;
                    }
                }
                if (afterIndex >= 0 && afterIndex <= subtitle.Length)
                {
                    newSubtile += subtitle.Substring(afterIndex);
                    newSubtitle = newSubtile;
                    removedWord = word;
                    return true;
                }
                else
                {
                    var temp = subtitle.Substring(0, index);
                    if (!char.IsLetterOrDigit(subtitle[subtitle.Length - 1]))
                        temp += subtitle[subtitle.Length - 1];
                    newSubtitle = temp;
                    removedWord = word;
                    return true;
                }
            }
            return false;
        }

        // Helpers for DeleteWord
        private static bool DeleteWord_ComputeBefore(string subtitle, int index, out string newSubtitle, out string removedWord)
        {
            newSubtitle = subtitle;
            removedWord = string.Empty;
            if (string.IsNullOrEmpty(subtitle) || index < 0 || index > subtitle.Length)
                return false;

            var newSubtile = subtitle.Substring(0, index);
            int beforeIndex = -1;
            string word = "";
            for (int j = newSubtile.Length - 2; j >= 0; j--)
            {
                if (char.IsLetterOrDigit(subtitle[j]) || Module.Helpers.TextHelper.CheckSpecialCharactersValidate(subtitle[j]))
                {
                    word = subtitle[j] + word;
                }
                else
                {
                    beforeIndex = j;
                    break;
                }
            }
            if (beforeIndex >= 0)
            {
                newSubtile = newSubtile.Substring(0, beforeIndex + 1);
                newSubtile += subtitle.Substring(index);
            }
            else
            {
                newSubtile = subtitle.Substring(index);
                if (!string.IsNullOrEmpty(newSubtile) && char.IsLower(newSubtile[0]))
                {
                    newSubtile = char.ToUpper(newSubtile[0]).ToString() + newSubtile.Substring(1);
                }
            }
            newSubtitle = newSubtile;
            removedWord = word;
            return true;
        }

        private static int? GetIndexByLocation_ComputeFromSentenceBySpaces(string sentence, string find, int location)
        {
            if (string.IsNullOrEmpty(sentence) || location <= 0)
                return null;
            int space = 0;
            //Xóa bỏ 2 dấu cách liền nhau;                
            for (int i = 0; i < sentence.Length - 1; i++)
            {
                if (space + 1 == location)
                {
                    var startIndex = i > 0 ? i - 1 : 0;
                    return sentence.IndexOf(find, startIndex, System.StringComparison.OrdinalIgnoreCase);
                }
                //Nếu ký tự cách ở đầu hoặc trước đấy là ký tự cách thí sẽ không được tính
                if (i > 0 && sentence[i] == ' ' && sentence[i - 1] != ' ')
                    space++;
            }
            return null;
        }
        private static int GetIndexTranslate_Compute(int? translateLocation, string content, string translate, int occurrenceIndex, int totalMatches)
        {
            if (translateLocation != null)
            {
                int space = 0;
                content = content?.Replace("  ", " ") ?? string.Empty;
                for (int i = 0; i < (content.Length > 0 ? content.Length - 1 : 0); i++)
                {
                    if (space + 1 == translateLocation)
                    {
                        return content.IndexOf(translate, i, System.StringComparison.OrdinalIgnoreCase);
                    }
                    if (content[i] == ' ')
                        space++;
                }
            }

            var index = Module.Helpers.TextHelper.GetIndexWordInContent(translate, content);
            if (index < 0)
                return index;
            if (totalMatches <= 1)
                return index;

            if (occurrenceIndex <= 1)
                return index;

            int occ = 1;
            while (occ < occurrenceIndex)
            {
                if (index >= (content?.Length ?? 0) - 1)
                    break;
                var newIndex = Module.Helpers.TextHelper.GetIndexWordInContent(translate, content, null, index + 1);
                if (newIndex < 0)
                    break;
                index = newIndex;
                occ++;
            }
            return index;
        }


        private static string GetRealNameFromLocation_Compute(string audioContent, int sentenceIndex, int location, string termName)
        {
            //Cắt theo dòng
            var sentencesArray = Module.Helpers.TextHelper.GetSentences(audioContent);
            if (sentencesArray.Length >= sentenceIndex && sentenceIndex > 0)
            {
                var content = sentencesArray[sentenceIndex - 1];
                var wordsArray = Module.Helpers.TextHelper.GetWords(content);
                var wordCount = termName.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
                var realLocation = location - 1;
                if (realLocation < wordsArray.Length && realLocation + wordCount <= wordsArray.Length)
                {
                    string realName = wordsArray[realLocation];
                    for (int i = 1; i < wordCount; i++)
                    {
                        realName += " " + wordsArray[realLocation + i];
                    }
                    realName = Module.Helpers.TextHelper.RemoveStartEndSpecialCharacters(realName);
                    if (!realName.Equals(termName, StringComparison.OrdinalIgnoreCase))
                    {
                        //Trường hợp lỗi
                        return null;
                    }
                    return realName;
                }
            }
            return null;
        }

        private static string GetUnicodeWord_Extracted(string audioContent, string termName, int? location, int? sentence)
        {
            if (string.IsNullOrEmpty(audioContent) || string.IsNullOrEmpty(termName))
                return null;
            var contentNoneUnicode = Module.Helpers.TextHelper.RemoveUnicode(audioContent);
            int termIndex = contentNoneUnicode.IndexOf(termName, System.StringComparison.OrdinalIgnoreCase);
            if (termIndex > 0 && contentNoneUnicode.IndexOf(termName, termIndex + 1, System.StringComparison.OrdinalIgnoreCase) > 0)
                termIndex = -1;
            if (termIndex >= 0)
            {
                return audioContent.Substring(termIndex, termName.Length);
            }
            else if (location != null && sentence != null)
            {
                var result = GetUnicodeWordByLocation_Extracted(audioContent, termName, location.Value, sentence.Value);
                if (!string.IsNullOrEmpty(result))
                    return result;
                //Có tìm theo sizing
                for (int i = 1; i <= 3; i++)
                {
                    if (location.Value - i > 0)
                    {
                        result = GetUnicodeWordByLocation_Extracted(audioContent, termName, location.Value - i, sentence.Value);
                        if (!string.IsNullOrEmpty(result))
                            return result;
                    }
                    result = GetUnicodeWordByLocation_Extracted(audioContent, termName, location.Value + i, sentence.Value);
                    if (!string.IsNullOrEmpty(result))
                        return result;
                }
            }
            return null;
        }

        private static string GetUnicodeWordByLocation_Extracted(string audioContent, string termName, int currentLocation, int currentSentence)
        {
            if (string.IsNullOrEmpty(audioContent) || string.IsNullOrEmpty(termName))
                return null;
            int termNameCount = termName.Split(' ', System.StringSplitOptions.RemoveEmptyEntries).Length;
            var rows = audioContent.Split(Module.Helpers.TextHelper.NewLineText, System.StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < rows.Length; i++)
            {
                if (currentSentence == i + 1)
                {
                    int rowPosition = 0;
                    var childContentArray = rows[i].Split(Module.Helpers.TextHelper.BeforeChars, System.StringSplitOptions.RemoveEmptyEntries);
                    foreach (var childContentText in childContentArray)
                    {
                        var contents = childContentText.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
                        for (int m = 0; m < contents.Length; m++)
                        {
                            rowPosition++;
                            if (rowPosition == currentLocation)
                            {
                                if (m + termNameCount > contents.Length)
                                    return null;
                                string unicodeWord = contents[m];
                                for (int n = 1; n < termNameCount; n++)
                                {
                                    unicodeWord += " " + contents[m + n];
                                }
                                if (termName.Equals(Module.Helpers.TextHelper.RemoveUnicode(unicodeWord), System.StringComparison.OrdinalIgnoreCase))
                                {
                                    return unicodeWord;
                                }
                                return null;
                            }
                        }
                    }
                }
            }
            return null;
        }


        private static string GetUnicodeWordByLocation_InstanceExtracted(string audioContent, string termName, int currentLocation, int currentSentence)
        {
            if (string.IsNullOrEmpty(audioContent) || string.IsNullOrEmpty(termName))
                return null;
            int termNameCount = termName.Split(' ', System.StringSplitOptions.RemoveEmptyEntries).Length;
            var rows = audioContent.Split(Module.Helpers.TextHelper.NewLineText, System.StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < rows.Length; i++)
            {
                if (currentSentence == i + 1)
                {
                    int rowPosition = 0;
                    var childContentArray = rows[i].Split(Module.Helpers.TextHelper.BeforeChars, System.StringSplitOptions.RemoveEmptyEntries);
                    foreach (var childContentText in childContentArray)
                    {
                        var contents = childContentText.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
                        for (int m = 0; m < contents.Length; m++)
                        {
                            rowPosition++;
                            if (rowPosition == currentLocation)
                            {
                                if (m + termNameCount > contents.Length)
                                    return null;
                                string unicodeWord = contents[m];
                                for (int n = 1; n < termNameCount; n++)
                                {
                                    unicodeWord += " " + contents[m + n];
                                }
                                if (termName.Equals(Module.Helpers.TextHelper.RemoveUnicode(unicodeWord), StringComparison.OrdinalIgnoreCase))
                                {
                                    return unicodeWord;
                                }
                                return null;
                            }
                        }
                    }
                }
            }
            return null;
        }

        // Helper: adjust casing for term name / translate / machineTranslate and subtitle when inserting at start
        private static void InsertWord_AdjustCasingForInsertAtStart(string word, ref string termName, ref string translate, ref string machineTranslate, ref string subtitle)
        {
            if (!string.IsNullOrEmpty(termName) && char.IsUpper(termName[0]))
            {
                if (termName.Length == 1 || (termName.Length > 1 && char.IsLower(termName[1])))
                {
                    termName = termName.ToUpper();
                }
            }
            if (!string.IsNullOrEmpty(translate) && char.IsUpper(translate[0]))
            {
                if (translate.Length == 1 || (translate.Length > 1 && char.IsLower(translate[1])))
                {
                    translate = translate.ToUpper();
                }
            }
            if (!string.IsNullOrEmpty(machineTranslate) && char.IsUpper(machineTranslate[0]))
            {
                if (machineTranslate.Length == 1 || (machineTranslate.Length > 1 && char.IsLower(machineTranslate[1])))
                {
                    machineTranslate = machineTranslate.ToUpper();
                }
            }
            if (!string.IsNullOrEmpty(subtitle) && char.IsUpper(subtitle[0]) && subtitle.Length > 1 && char.IsLower(subtitle[1]))
            {
                subtitle = char.ToUpper(subtitle[0]).ToString() + subtitle.Substring(1);
            }
        }

        // Helper: compute new subtitle when inserting a word (before/after)
        private static string InsertWord_ComputeNewSubtitle(string subtitle, int index, bool before, string word, string translate)
        {
            if (string.IsNullOrEmpty(subtitle))
                return subtitle ?? string.Empty;

            if (before)
            {
                var newSubtile = index > 0 ? subtitle.Substring(0, index) : string.Empty;
                if (index == 0)
                {
                    if (word.Length == 1)
                        newSubtile += word.ToUpper() + " ";
                    else
                        newSubtile += char.ToUpper(word[0]) + word.Substring(1).ToLower() + " ";
                }
                else
                {
                    newSubtile += word + " ";
                }
                newSubtile += subtitle.Substring(index);
                return newSubtile;
            }
            else
            {
                index += translate.Length;
                if (index <= subtitle.Length)
                {
                    var newSubtile = subtitle.Substring(0, index);
                    newSubtile += " " + word;
                    newSubtile += subtitle.Substring(index);
                    return newSubtile;
                }
                return subtitle;
            }
        }


        private static bool IsOverlap_Compute(int leftLocation, int rightLocation, int term1WordQty, int term2WordQty, bool requireTermFlag, string machineTranslateA, string machineTranslateB)
        {
            if (leftLocation <= rightLocation)
            {
                if (term1WordQty > 0)
                {
                    if (rightLocation - leftLocation < term1WordQty)
                        return true;
                }
                else if (!requireTermFlag && !string.IsNullOrEmpty(machineTranslateA))
                {
                    if (rightLocation - leftLocation < (machineTranslateA?.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length ?? 0))
                        return true;
                }
            }
            else
            {
                if (term2WordQty > 0)
                {
                    if (leftLocation - rightLocation < term2WordQty)
                        return true;
                }
                else if (!requireTermFlag && !string.IsNullOrEmpty(machineTranslateB))
                {
                    if (leftLocation - rightLocation < (machineTranslateB?.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length ?? 0))
                        return true;
                }
            }
            return false;
        }

        // Helpers for MoveWord
        private static bool MoveWord_ComputeBackward(string subtitle, int index, string translate, out string newSubtitle, out string swappedWord)
        {
            newSubtitle = subtitle;
            swappedWord = string.Empty;
            if (string.IsNullOrEmpty(subtitle) || index <= 1 || index >= subtitle.Length)
                return false;

            var newSubtile = subtitle.Substring(0, index);
            var currentTranslate = subtitle.Substring(index, Math.Min(translate.Length, Math.Max(0, subtitle.Length - index)));
            int beforeIndex = -1;
            string word = "";
            for (int j = newSubtile.Length - 2; j >= 0; j--)
            {
                if (char.IsLetterOrDigit(subtitle[j]) || Module.Helpers.TextHelper.CheckSpecialCharactersValidate(subtitle[j]))
                {
                    word = subtitle[j] + word;
                }
                else
                {
                    beforeIndex = j;
                    break;
                }
            }
            if (beforeIndex >= 0)
            {
                newSubtile = newSubtile.Substring(0, beforeIndex + 1);
                newSubtile += currentTranslate;
                newSubtile += " ";
                newSubtile += word;
            }
            else
            {
                if (!string.IsNullOrEmpty(word) && char.IsUpper(word[0]) && word.Length > 1 && char.IsLower(word[1]))
                {
                    word = word.ToLower();
                }
                if (!string.IsNullOrEmpty(currentTranslate) && char.IsLower(currentTranslate[0]))
                {
                    currentTranslate = char.ToUpper(currentTranslate[0]).ToString() + currentTranslate.Substring(1);
                }
                newSubtile = currentTranslate + " " + word;
                if (index + currentTranslate.Length < subtitle.Length)
                    newSubtile += subtitle.Substring(index + currentTranslate.Length);
            }
            newSubtitle = newSubtile;
            swappedWord = word;
            return true;
        }

        private static bool MoveWord_ComputeForward(string subtitle, int index, string translate, out string newSubtitle, out string swappedWord)
        {
            newSubtitle = subtitle;
            swappedWord = string.Empty;
            if (string.IsNullOrEmpty(subtitle) || index < 0 || index + translate.Length >= subtitle.Length - 1)
                return false;

            var newSubtile = subtitle.Substring(0, index);
            var currentTranslate = subtitle.Substring(index, Math.Min(translate.Length, Math.Max(0, subtitle.Length - index)));
            index += translate.Length;
            int afterIndex = -1;
            string word = "";
            for (int j = index + 1; j < subtitle.Length; j++)
            {
                if ((char.IsLetterOrDigit(subtitle[j]) || Module.Helpers.TextHelper.CheckSpecialCharactersValidate(subtitle[j])) &&
                    !(j + 1 < subtitle.Length && subtitle[j + 1] == ' ' && (subtitle[j] == '.' || subtitle[j] == ',')))
                {
                    word += subtitle[j];
                }
                else
                {
                    afterIndex = j;
                    break;
                }
            }
            if (afterIndex >= 0 && afterIndex <= subtitle.Length)
            {
                if (string.IsNullOrEmpty(newSubtile) || newSubtile.EndsWith(". "))
                {
                    if (!string.IsNullOrEmpty(currentTranslate) && char.IsUpper(currentTranslate[0]) && currentTranslate.Length > 1 && char.IsLower(currentTranslate[1]))
                    {
                        currentTranslate = currentTranslate.ToLower();
                    }
                    if (!string.IsNullOrEmpty(word) && char.IsLower(word[0]))
                    {
                        word = char.ToUpper(word[0]).ToString() + word.Substring(1);
                    }
                }
                newSubtile += word + " " + currentTranslate;
                newSubtile += subtitle.Substring(afterIndex);
                newSubtitle = newSubtile;
                swappedWord = word;
                return true;
            }
            else
            {
                if (!string.IsNullOrEmpty(word) && !char.IsLetterOrDigit(word[word.Length - 1]))
                {
                    currentTranslate += word[word.Length - 1];
                    word = word.Substring(0, word.Length - 1);
                }
                newSubtile += word + " " + currentTranslate;
                newSubtitle = newSubtile;
                swappedWord = word;
                return true;
            }
        }


        private static void NoneOverlapPart_ComputeNonOverlapStrings(string[] termNameArray, string[] termName2Array, int termLocationValue, int currentLocation, out string term1NoneOverlap, out string term2NoneOverlap)
        {
            term1NoneOverlap = string.Empty;
            term2NoneOverlap = string.Empty;
            if (termNameArray == null || termName2Array == null)
                return;

            if (currentLocation > termLocationValue)
            {
                var notOverlapLength = currentLocation - termLocationValue;
                for (int i = 0; i < notOverlapLength && i < termNameArray.Length; i++)
                {
                    if (!string.IsNullOrEmpty(term1NoneOverlap))
                        term1NoneOverlap += " ";
                    term1NoneOverlap += termNameArray[i];
                }
                int overlapLength = termNameArray.Length - notOverlapLength;
                for (int i = overlapLength; i < termName2Array.Length; i++)
                {
                    if (!string.IsNullOrEmpty(term2NoneOverlap))
                        term2NoneOverlap += " ";
                    term2NoneOverlap += termName2Array[i];
                }
            }
            else if (currentLocation < termLocationValue)
            {
                var notOverlapLength = termLocationValue - currentLocation;
                for (int i = 0; i < notOverlapLength && i < termName2Array.Length; i++)
                {
                    if (!string.IsNullOrEmpty(term2NoneOverlap))
                        term2NoneOverlap += " ";
                    term2NoneOverlap += termName2Array[i];
                }
                int overlapLength = termName2Array.Length - notOverlapLength;
                for (int i = overlapLength; i < termNameArray.Length; i++)
                {
                    if (!string.IsNullOrEmpty(term1NoneOverlap))
                        term1NoneOverlap += " ";
                    term1NoneOverlap += termNameArray[i];
                }
            }
            else if (termNameArray.Length > termName2Array.Length)
            {
                for (int i = termName2Array.Length; i < termNameArray.Length; i++)
                {
                    if (!string.IsNullOrEmpty(term1NoneOverlap))
                        term1NoneOverlap += " ";
                    term1NoneOverlap += termNameArray[i];
                }
            }
            else if (termNameArray.Length < termName2Array.Length)
            {
                for (int i = termNameArray.Length; i < termName2Array.Length; i++)
                {
                    if (!string.IsNullOrEmpty(term2NoneOverlap))
                        term2NoneOverlap += " ";
                    term2NoneOverlap += termName2Array[i];
                }
            }
        }

        private static int OverlapCaseType_CompareByRealName(string term1RealName, string termName2)
        {
            if (string.IsNullOrEmpty(term1RealName))
                return 0;
            var prep = OverlapCaseType_Prepare(term1RealName, termName2);
            var termNameArray = prep.termNameArray;
            var termName2Array = prep.termName2Array;
            var termName1IsLower = prep.termName1IsLower;
            var termName2IsLower = prep.termName2IsLower;
            var termName1IsUpper = prep.termName1IsUpper;
            var termName2IsUpper = prep.termName2IsUpper;

            if (termNameArray == null || termName2Array == null)
                return 0;

            if (termName1IsLower && termName2IsLower)
                return 0;
            if (termName1IsUpper && termName2IsUpper)
                return 0;

            var overlapText = termNameArray.Intersect(termName2Array);
            if (overlapText.Count() > 0)
            {
                var overlapWord = string.Join(" ", overlapText);
                var overlapEval = OverlapCaseType_EvaluateOverlap(overlapWord, termName1IsLower, termName2IsLower, termName1IsUpper, termName2IsUpper);
                if (overlapEval != 0)
                    return overlapEval;
            }
            return 0;
        }

        private static int OverlapCaseType_EvaluateOverlap(string overlapWord, bool termName1IsLower, bool termName2IsLower, bool termName1IsUpper, bool termName2IsUpper)
        {
            if (string.IsNullOrEmpty(overlapWord))
                return 0;
            bool overlapIsLower = overlapWord.Equals(overlapWord.ToLower());
            if (overlapIsLower)
            {
                if (termName1IsLower)
                    return 1;
                if (termName2IsLower)
                    return -1;
            }
            bool overlapIsUpper = Module.Helpers.TextHelper.CheckRealNameIsUpperCaseFirstAll(overlapWord);
            if (overlapIsUpper)
            {
                if (termName1IsUpper)
                    return 1;
                if (termName2IsUpper)
                    return -1;
            }
            return 0;
        }

        private static (string[] termNameArray, string[] termName2Array, bool termName1IsLower, bool termName2IsLower, bool termName1IsUpper, bool termName2IsUpper) OverlapCaseType_Prepare(string term1RealName, string termName2)
        {
            var termNameArray = term1RealName?.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
            var termName2Array = termName2?.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
            bool termName1IsLower = !string.IsNullOrEmpty(term1RealName) && term1RealName.Equals(term1RealName.ToLower());
            bool termName2IsLower = !string.IsNullOrEmpty(termName2) && termName2.Equals(termName2.ToLower());
            bool termName1IsUpper = Module.Helpers.TextHelper.CheckRealNameIsUpperCaseFirstAll(term1RealName);
            bool termName2IsUpper = !string.IsNullOrEmpty(termName2) && Module.Helpers.TextHelper.CheckRealNameIsUpperCaseFirstAll(termName2);
            return (termNameArray, termName2Array, termName1IsLower, termName2IsLower, termName1IsUpper, termName2IsUpper);
        }
        private static string OverlapTermPosition_ComputeOtherTermName(string[] termWords, string termName, string sentence, int realPosition, string choice)
        {
            string otherTermName = "";
            if (choice.Contains("Left"))
                otherTermName = termWords[0];
            else
                otherTermName = termWords[termWords.Length - 1];
            if (choice.Contains("Two") || choice.Contains("Three"))
            {
                bool three = choice.Contains("Three");
                if (string.IsNullOrEmpty(sentence) || realPosition < 0)
                    return null;
                if (choice.Contains("Left"))
                {
                    otherTermName = ' ' + otherTermName;
                    var checkPosition = realPosition - 2;
                    for (int i = checkPosition; i >= 0; i--)
                    {
                        if (sentence[i] == ' ')
                        {
                            if (!three)
                                break;
                            else
                                three = false;
                            otherTermName = sentence[i] + otherTermName;
                        }
                        else
                        {
                            otherTermName = sentence[i] + otherTermName;
                        }
                    }
                }
                else if (choice.Contains("Right"))
                {
                    otherTermName += ' ';
                    var checkPosition = realPosition + termName.Length + 1;
                    for (int i = checkPosition; i < sentence.Length; i++)
                    {
                        if (sentence[i] == ' ')
                        {
                            if (!three)
                                break;
                            else
                                three = false;
                            otherTermName += sentence[i];
                        }
                        else
                        {
                            otherTermName += sentence[i];
                        }
                    }
                    otherTermName = Module.Helpers.TextHelper.RemoveStartEndSpecialCharacters(otherTermName, false);
                }
                if (choice.Contains("Two"))
                {
                    if (otherTermName.Split(' ', System.StringSplitOptions.RemoveEmptyEntries).Length != 2)
                        return null;
                }
                else if (choice.Contains("Three"))
                {
                    if (otherTermName.Split(' ').Length != 3)
                        return null;
                }
            }
            if (string.IsNullOrEmpty(otherTermName))
                return null;
            return otherTermName;
        }

        private static string ParseTranslatedContent(string newtranlateContent, string subtitle, string seperateKey, string endSeperateKey, string content)
        {
            if (string.IsNullOrEmpty(newtranlateContent) || string.IsNullOrEmpty(subtitle))
                return null;
            // Try to extract marked translation between the separate keys and match against subtitle
            int startIndex = newtranlateContent.IndexOf(seperateKey ?? string.Empty, System.StringComparison.OrdinalIgnoreCase);
            if (startIndex < 0)
            {
                var trimmed = (seperateKey ?? string.Empty).Trim();
                if (!string.IsNullOrEmpty(trimmed))
                    startIndex = newtranlateContent.IndexOf(trimmed, System.StringComparison.OrdinalIgnoreCase);
            }
            if (startIndex < 0)
                return null;
            int endIndex = newtranlateContent.IndexOf(endSeperateKey ?? string.Empty, startIndex + 1, System.StringComparison.OrdinalIgnoreCase);
            if (endIndex <= startIndex)
                return null;
            var candidate = newtranlateContent.Substring(startIndex + (seperateKey?.Length ?? 0), endIndex - startIndex - (seperateKey?.Length ?? 0));
            if (string.IsNullOrEmpty(candidate))
                return null;
            // Try to find the candidate in subtitle
            var found = subtitle.IndexOf(candidate, System.StringComparison.OrdinalIgnoreCase);
            if (found >= 0)
            {
                var result = Module.Helpers.TextHelper.RemoveStartEndSpecialCharacters(candidate);
                return string.IsNullOrEmpty(result) ? null : result.Trim();
            }
            return null;
        }

        private static int ReplaceUnReplaceTranslate_SelectFirstIndex(int firstIndex, string rootContent, string find, int translateIndex, int audioContentLength)
        {
            var otherIndex = Module.Helpers.TextHelper.GetIndexWordInContent(find, rootContent, null, firstIndex + 1);
            if (otherIndex > 0)
            {
                if (translateIndex > 0)
                {
                    var firstIndexPercent = System.Convert.ToDecimal(firstIndex) / (rootContent?.Length ?? 1);
                    var otherIndexPercent = System.Convert.ToDecimal(otherIndex) / (rootContent?.Length ?? 1);
                    var translateIndexPercent = System.Convert.ToDecimal(translateIndex) / (audioContentLength <= 0 ? 1 : audioContentLength);
                    var firstT = firstIndexPercent - translateIndexPercent;
                    if (firstT < 0)
                        firstT = -firstT;
                    var otherT = otherIndexPercent - translateIndexPercent;
                    if (otherT < 0)
                        otherT = -otherT;
                    if (otherT < firstT)
                    {
                        firstIndex = otherIndex;
                    }
                }
            }
            return firstIndex;
        }


        private static (string localReplaceText, string[] localReplaceTextArray) ReplaceWord_ComputeLocalReplaceText(string replaceText, string oldText)
        {
            var localReplaceText = replaceText;
            var oldTextArray = oldText.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var localReplaceTextArray = localReplaceText.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (oldText.Equals(oldText.ToLower()))
            {
                localReplaceText = localReplaceText.ToLower();
            }
            else if (oldText.Equals(oldText.ToUpper()))
            {
                localReplaceText = localReplaceText.ToUpper();
            }
            else
            {
                if (oldTextArray.Length == localReplaceTextArray.Length)
                {
                    for (int i = 0; i < oldTextArray.Length; i++)
                    {
                        if (oldTextArray[i].Equals(oldTextArray[i].ToLower()))
                        {
                            localReplaceTextArray[i] = localReplaceTextArray[i].ToLower();
                        }
                        else if (oldTextArray[i].Equals(oldTextArray[i].ToUpper()))
                        {
                            localReplaceTextArray[i] = localReplaceTextArray[i].ToUpper();
                        }
                        else if (!string.IsNullOrEmpty(localReplaceTextArray[i]) && char.IsUpper(oldTextArray[i][0]))
                        {
                            //Nếu viết hoa ký tự đầu tiên
                            localReplaceTextArray[i] = char.ToUpper(localReplaceTextArray[i][0]) + localReplaceTextArray[i].Substring(1);
                        }
                    }
                    localReplaceText = string.Join(" ", localReplaceTextArray);
                }
            }
            return (localReplaceText, localReplaceTextArray);
        }

        private static string ShiftWord_ComputeResult(string[] wordsArray, int realLocation, string word, int left, int right)
        {
            // Tìm vị trí bắt đầu và kết thúc theo số lượng từ bên trái và bên phải
            int startIndex = Math.Max(0, realLocation - left);  // Đảm bảo không vượt quá chỉ số đầu
            int endIndex = Math.Min(wordsArray.Length - 1, realLocation + word.Split(' ').Length - 1 + right);  // Đảm bảo không vượt quá chỉ số cuối

            // Tạo chuỗi kết quả
            string result = string.Join(" ", wordsArray, startIndex, endIndex - startIndex + 1);
            if (result.Split(' ').Length != word.Split(' ').Length)
            {
                // preserve original behavior: do nothing special
            }
            return result;
        }




        private static int? Spelling_CompareByDictionary(System.Collections.Generic.Dictionary<int, System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>>> dict, string term1Name, string termName2)
        {
            // Pure comparison using dictionary + names only (object-independent)
            var termIsCorrect = Module.Helpers.TextHelper.CheckWordIsCorrect(dict, term1Name);
            var term2IsCorrect = Module.Helpers.TextHelper.CheckWordIsCorrect(dict, termName2);
            if (termIsCorrect && term2IsCorrect)
                return 0;
            else if (termIsCorrect)
                return 1;
            else if (term2IsCorrect)
                return -1;

            var term1NotCorrect = Module.Helpers.TextHelper.CountNotCorrectInWord(dict, term1Name);
            var term2NotCorrect = Module.Helpers.TextHelper.CountNotCorrectInWord(dict, termName2);
            if (term1NotCorrect != term2NotCorrect)
            {
                if (term1NotCorrect < 0)
                    return -1;
                if (term2NotCorrect < 0)
                    return 1;
                return term1NotCorrect < term2NotCorrect ? 1 : -1;
            }

            return null; // undecided — caller may perform additional checks
        }


        private static int SpellingNotCorrect(System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> singleWordDict, string content)
        {
            if ((singleWordDict == null) || (singleWordDict.Count == 0))
                throw new System.ArgumentException($"{nameof(singleWordDict)} is null or empty.", nameof(singleWordDict));
            if (string.IsNullOrEmpty(content))
                throw new System.ArgumentException($"{nameof(content)} is null or empty.", nameof(content));
            var nameArray = content?.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
            var notCorrect1 = 0;
            if (nameArray == null)
                return 0;
            foreach (var word in nameArray)
            {
                if (string.IsNullOrEmpty(word))
                    continue;
                if (char.IsLower(word[0]) && !Module.Helpers.TextHelper.CheckSimpleWordIsCorrect(singleWordDict, word))
                {
                    notCorrect1++;
                }
            }
            return notCorrect1;
        }

        // Build replaced content used in SynTerm when swapping name/translate
        private static string SynTerm_BuildUpdatedContent(string originalContent, string replacementText, int startIdx, int endIdx, string trailingChar)
        {
            var newContent = originalContent.Substring(0, startIdx);
            newContent += replacementText;
            if (!string.IsNullOrEmpty(trailingChar))
                newContent += trailingChar;
            newContent += originalContent.Substring(endIdx);
            if (string.IsNullOrEmpty(trailingChar) && endIdx == originalContent.Length && originalContent.Length > 0 && !char.IsLetterOrDigit(originalContent[originalContent.Length - 1]))
            {
                newContent += originalContent[originalContent.Length - 1];
            }
            return newContent;
        }

        // Find start and end indices for SynTerm replacement in content by location
        private static bool SynTerm_FindIndices(string content, string findText, int location, int termLength, out int startIndex, out int endIndex, out string lastedChar)
        {
            startIndex = -1;
            endIndex = content?.Length ?? 0;
            lastedChar = string.Empty;
            if (string.IsNullOrEmpty(content) || string.IsNullOrEmpty(findText) || location <= 0)
                return false;

            int position = 0;
            string result = string.Empty;
            var rows = content.Split(Module.Helpers.TextHelper.NewLineText, System.StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < rows.Length; i++)
            {
                var startPosition = position;
                var contents = rows[i].Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
                position += contents.Length;
                if (startPosition <= location && location < position)
                {
                    int idx = content.IndexOf(rows[i], result.Length);
                    for (int m = 0; m < contents.Length; m++)
                    {
                        var word = contents[m];
                        idx = content.IndexOf(word, idx);
                        if (startPosition + m + 1 == location)
                        {
                            startIndex = idx;
                        }
                        else if (startIndex > 0 && startPosition + m + 1 == location + termLength)
                        {
                            string lastedWord = contents[m - 1];
                            if (!char.IsLetterOrDigit(lastedWord[lastedWord.Length - 1]))
                            {
                                lastedChar = lastedWord[lastedWord.Length - 1].ToString();
                            }
                            endIndex = idx - 1;
                            return startIndex > 0;
                        }
                        idx += word.Length;
                    }
                }
                else if (i > 0)
                {
                    int idx = content.IndexOf(rows[i], result.Length);
                    result = content.Substring(0, idx + rows[i].Length);
                }
            }
            return startIndex > 0;
        }

        // Handle replacement when term is at start of sentence
        private static string SynTerm_HandleStartOfSentence(string content, string findText, int termLength)
        {
            if (string.IsNullOrEmpty(content) || string.IsNullOrEmpty(findText) || termLength <= 0)
                return null;
            int spaceKeys = 0;
            for (int i = 0; i < content.Length; i++)
            {
                if (content[i] == ' ')
                    spaceKeys++;
                if (spaceKeys == termLength)
                {
                    var replacement = findText;
                    if (char.IsLower(replacement[0]))
                    {
                        if (replacement.Length == 1)
                            replacement = replacement.ToUpper();
                        else
                            replacement = char.ToUpper(replacement[0]) + replacement.Substring(1);
                    }
                    return replacement + content.Substring(i);
                }
            }
            return null;
        }

        // Evaluate initial validate logic used in UpdatePositionLocation (pure string checks)
        private static bool UpdatePositionLocation_EvaluateStart(string sentenceContent, string lookup, int idx, System.Collections.Generic.List<string> parentTerms, out int nextStartIndex)
        {
            nextStartIndex = idx + (lookup?.Length ?? 0);
            if (idx >= 1 && char.IsLetterOrDigit(sentenceContent[idx - 1]))
                return false;
            if (parentTerms != null && parentTerms.Count > 0)
                return Module.Helpers.TextHelper.CheckCurrentIndexIsNotParentIndex(sentenceContent, lookup, idx, parentTerms.ToArray());
            return Module.Helpers.TextHelper.CheckCurrentIndexIsNotParentIndex(sentenceContent, lookup, idx);
        }

        // Check duplicate position in before-list
        private static bool UpdatePositionLocation_IsDuplicate(System.Collections.Generic.List<Module.BusinessObjects.TermLocation> beforeList, int sentenceNumber, int position)
        {
            if (beforeList is null || beforeList.Count == 0)
                return false;
            return beforeList.FirstOrDefault(m => m.Sentence == sentenceNumber && m.Location == position) != null;
        }

        // Validate translate word occurrence inside a row for UpdateTranslateLocation
        private static bool UpdateTranslateLocation_IsValidWord(string rowContent, string translateText, int idx, System.Collections.Generic.List<string> parentTerms)
        {
            if (idx >= 1 && char.IsLetterOrDigit(rowContent[idx - 1]))
                return false;
            if (!Module.Helpers.TextHelper.CheckWordIndexIsValidateInContent(rowContent, translateText, idx))
                return false;
            if (parentTerms != null && parentTerms.Count > 0)
                return Module.Helpers.TextHelper.CheckCurrentIndexIsNotParentIndex(rowContent, translateText, idx, parentTerms.ToArray());
            return Module.Helpers.TextHelper.CheckCurrentIndexIsNotParentIndex(rowContent, translateText, idx);
        }
        #endregion SourceCode4552ImportCode

  
  
        #region Base Object Service
	    		//public string ToolTipControllerText(View view, Module.BusinessObjects.TermLocation termlocation)
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
		//public object TermToolTipControllerText(View view, Module.BusinessObjects.TermLocation termlocation)
        //{
        //    if (Term != null) 
		//			return Term;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object LocationToolTipControllerText(View view, Module.BusinessObjects.TermLocation termlocation)
        //{
            
        //    return null;
        //}
    

		//Tooltip for Object
		//public object SentenceToolTipControllerText(View view, Module.BusinessObjects.TermLocation termlocation)
        //{
        //    if (Sentence != null) 
		//			return Sentence;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object TranslateToolTipControllerText(View view, Module.BusinessObjects.TermLocation termlocation)
        //{
        //    if (Translate != null) 
		//			return Translate;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object MachineTranslateToolTipControllerText(View view, Module.BusinessObjects.TermLocation termlocation)
        //{
            
        //    return null;
        //}
    

		//Tooltip for Object
		//public object FlagToolTipControllerText(View view, Module.BusinessObjects.TermLocation termlocation)
        //{
        //    if (Flag != null) 
		//			return Flag;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object TranslateLocationToolTipControllerText(View view, Module.BusinessObjects.TermLocation termlocation)
        //{
            
        //    return null;
        //}
    

		//Tooltip for Object
		//public object OverlapToolTipControllerText(View view, Module.BusinessObjects.TermLocation termlocation)
        //{
            
        //    return null;
        //}
    

		//Tooltip for Object
		//public object AudioToolTipControllerText(View view, Module.BusinessObjects.TermLocation termlocation)
        //{
        //    if (Audio != null) 
		//			return Audio;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ReplaceTranslateToolTipControllerText(View view, Module.BusinessObjects.TermLocation termlocation)
        //{
        //    if (ReplaceTranslate != null) 
		//			return ReplaceTranslate;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object LengthToolTipControllerText(View view, Module.BusinessObjects.TermLocation termlocation)
        //{
        //    if (Length != null) 
		//			return Length;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object NoteToolTipControllerText(View view, Module.BusinessObjects.TermLocation termlocation)
        //{
        //    if (Note != null) 
		//			return Note;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object Flag2ToolTipControllerText(View view, Module.BusinessObjects.TermLocation termlocation)
        //{
        //    if (Flag2 != null) 
		//			return Flag2;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object Note2ToolTipControllerText(View view, Module.BusinessObjects.TermLocation termlocation)
        //{
        //    if (Note2 != null) 
		//			return Note2;
        //    return null;
        //}
    

	    #endregion
  

    }
}
