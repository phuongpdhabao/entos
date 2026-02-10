using System;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.XtraGrid;
using Microsoft.Extensions.DependencyInjection;
using ENTOS.Module.BusinessObjects;
using ENTOS.Module.SystemObjects;
using ENTOS.Module.Services;
using ListView = DevExpress.ExpressApp.ListView;


namespace ENTOS.Module.Controllers 
{
    public partial class ParagraphViewController: BaseViewController<Module.BusinessObjects.Paragraph>
    {      
        
        public ParagraphViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(Module.BusinessObjects.Paragraph);    
            //TargetViewNesting = Nesting.Nested;
        }

        #region DependencyInjection
  
  
  
  
        #endregion DependencyInjection
		
		protected override void OnActivated()
        {
            base.OnActivated();
        }
        protected override void OnViewControlsCreated()
        {
             base.OnViewControlsCreated();
             
        }
        
        protected override void OnDeactivated()
        {                 
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }


        
        //Code: 2613            Oid: d1692982-e1fd-458a-9e96-9bb20afa808b
		private void ParagraphFlag_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(ParagraphFlag), "Cờ đoạn văn bản");              
      
            #region ParagraphFlagImportCode
            foreach (Module.BusinessObjects.Paragraph paragraph in View.SelectedObjects)
            {
                // Lấy câu đầu tiên và câu cuối cùng
                string firstSentence = paragraph.Content.Split(new[] { '.', '!', '?' }, System.StringSplitOptions.RemoveEmptyEntries).FirstOrDefault()?.Trim();
                string lastSentence = paragraph.Content.Split(new[] { '.', '!', '?' }, System.StringSplitOptions.RemoveEmptyEntries).LastOrDefault()?.Trim();

                if (string.IsNullOrEmpty(firstSentence) || string.IsNullOrEmpty(lastSentence))
                    continue;

                bool paraFlag = false;
                string paraContent = null;

                if (e.SelectedChoiceActionItem.Id.StartsWith("Begin"))
                {
                    paraContent = firstSentence;
                }
                else if (e.SelectedChoiceActionItem.Id.StartsWith("End"))
                {
                    paraContent = lastSentence;
                }

                if (!string.IsNullOrEmpty(paraContent))
                {
                    if (e.SelectedChoiceActionItem.Id.Equals("BeginNotUpperCase"))
                    {
                        if (paraContent[0] == ' ' && paraContent.Length > 1)
                            paraContent = paraContent.Substring(1);

                        if (char.IsLower(paraContent[0]))
                        {
                            paraFlag = true;
                        }
                    }
                    else if (e.SelectedChoiceActionItem.Id.Equals("BeginAbbreviationOrNumber"))
                    {
                        if (paraContent[0] == ' ' && paraContent.Length > 1)
                            paraContent = paraContent.Substring(1);

                        if (Module.Helpers.TextHelper.CheckUpperCaseAll(paraContent))
                            continue;

                        var audio = paragraph.AudioList.FirstOrDefault();
                        if (audio != null && AudioService.ElementFlagUpperCase(audio.Video, paraContent))
                            continue;

                        if (char.IsNumber(paraContent[0]) || (paraContent.Length > 1 && char.IsUpper(paraContent[0]) && char.IsUpper(paraContent[1])))
                        {
                            paraFlag = true;
                        }
                    }
                    else if (e.SelectedChoiceActionItem.Id.Equals("BeginSignSpecialCharacter"))
                    {
                        if (paraContent[0] == ' ' && paraContent.Length > 1)
                            paraContent = paraContent.Substring(1);

                        if (!char.IsLetterOrDigit(paraContent[0]))
                        {
                            paraFlag = true;
                        }
                    }
                    else if (e.SelectedChoiceActionItem.Id.Equals("BeginSpaces"))
                    {
                        if (paraContent[0] == ' ' && paraContent.Length > 1 && paraContent[1] == ' ')
                        {
                            paraFlag = true;
                        }
                    }
                    else if (e.SelectedChoiceActionItem.Id.Equals("EndNormalCharacter"))
                    {
                        if (paraContent[paraContent.Length - 1] == ' ' && paraContent.Length > 1)
                            paraContent = paraContent.Substring(0, paraContent.Length - 1);

                        if (char.IsLetterOrDigit(paraContent[paraContent.Length - 1]))
                        {
                            var audio = paragraph.AudioList.LastOrDefault();
                            if (audio != null && AudioService.ElementFlagUpperCase(audio.Video, paraContent))
                                continue;

                            paraFlag = true;
                        }
                    }
                    else if (e.SelectedChoiceActionItem.Id.Equals("EndComma"))
                    {
                        if (paraContent[paraContent.Length - 1] == ' ' && paraContent.Length > 1)
                            paraContent = paraContent.Substring(0, paraContent.Length - 1);

                        if (paraContent.EndsWith(",", System.StringComparison.OrdinalIgnoreCase))
                        {
                            paraFlag = true;
                        }
                    }
                    else if (e.SelectedChoiceActionItem.Id.Equals("EndSignOrSpecialCharacter"))
                    {
                        if (paraContent[paraContent.Length - 1] == ' ' || (paraContent[paraContent.Length - 1] != '.' && paraContent[paraContent.Length - 1] != ',' && !char.IsLetterOrDigit(paraContent[paraContent.Length - 1])))
                        {
                            paraFlag = true;
                        }
                    }
                    else if (e.SelectedChoiceActionItem.Id.Equals("EndAbbreviationOrNumber"))
                    {
                        if (paraContent[paraContent.Length - 1] == ' ' && paraContent.Length > 1)
                            paraContent = paraContent.Substring(0, paraContent.Length - 1);

                        if (Module.Helpers.TextHelper.CheckUpperCaseAll(paraContent))
                            continue;

                        var audio = paragraph.AudioList.LastOrDefault();
                        if (audio != null && AudioService.ElementFlagUpperCase(audio.Video, paraContent))
                            continue;

                        if (char.IsNumber(paraContent[paraContent.Length - 1]) || (paraContent.Length > 1 && char.IsUpper(paraContent[paraContent.Length - 1]) && char.IsUpper(paraContent[paraContent.Length - 2])))
                        {
                            paraFlag = true;
                        }
                    }
                    else if (e.SelectedChoiceActionItem.Id.Equals("EndSpaces"))
                    {
                        if (paraContent[paraContent.Length - 1] == ' ' && paraContent.Length > 2 && paraContent[paraContent.Length - 2] == ' ')
                        {
                            paraFlag = true;
                        }
                    }
                }
                if (paragraph.Flag != paraFlag)
                {
                    paragraph.Flag = paraFlag;
                }
            }


            #endregion ParagraphFlagImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 2612            Oid: b9ea8cbd-0cf6-4e98-8646-b3933dbb8ef0
		private void MergeParagraph_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(MergeParagraph), "Gộp");              
      
            #region MergeParagraphImportCode
            if(View.CurrentObject is null)
                return;
            var video = Module.SystemObjects.Tools.GetMasterObjectFromView(View) as Module.BusinessObjects.Video;
            if (video is null)
                return;
            //Fix lỗi nếu order is null
            //Chỉ merge to paragraph có audio
            var paragraphList = video.ParagraphList.Where(p => p.AudioList.Count > 0).OrderBy(p => p.Order.HasValue ? 1 : 0).ThenBy(p => p.Order).ToList();
            int totalAudio = 0;
            foreach (Module.BusinessObjects.Paragraph paragraph in View.SelectedObjects)
            {
                if (paragraph.AudioList.Count == 0)
                    continue;
                Module.BusinessObjects.Paragraph otherParagraph = e.SelectedChoiceActionItem.Id.Contains("Up") ? 
                    paragraphList.Where(x => x.BookMark?.Oid == paragraph.BookMark?.Oid).LastOrDefault(x => x.Order < paragraph.Order) : paragraphList.Where(x => x.BookMark?.Oid == paragraph.BookMark?.Oid).FirstOrDefault(x => x.Order > paragraph.Order);
                foreach(var audio in paragraph.AudioList.ToList())
                {
                    audio.Paragraph = otherParagraph;
                    totalAudio++;
                }
            }
            Module.Helpers.XafXpoHelper.ShowMessage(Application, "Kết quả", $"Đã gộp {totalAudio} thành phần");

            #endregion MergeParagraphImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
     }
}