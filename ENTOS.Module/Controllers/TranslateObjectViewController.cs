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
    public partial class TranslateObjectViewController: BaseViewController<Module.BusinessObjects.TranslateObject>
    {      
        
        public TranslateObjectViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(Module.BusinessObjects.TranslateObject);    
            //TargetViewNesting = Nesting.Nested;
        }

        #region DependencyInjection
  
  
     
        private VideoService videoService;
        protected VideoService _videoService => videoService ??= new VideoService(this);        
      
  
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


        
        //Code: 1309            Oid: 023016c9-b6f3-46bd-9876-dbc26dcd1f03
		private void ExportTranslateObject_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(ExportTranslateObject), "Xuất dịch");              
      
            #region ExportTranslateObjectImportCode
            var video = Module.SystemObjects.Tools.GetMasterObjectFromView(View) as Module.BusinessObjects.Video;
            if (video is null)
                return;
            var htmlDocument = new HtmlAgilityPack.HtmlDocument();
            int result = 0;
            foreach (Module.BusinessObjects.TranslateObject translateObject in View.SelectedObjects)
            {
                
                if (translateObject.ObjectID != null && translateObject.SystemType != null && translateObject.Field != null)
                {
                    var objTranslate = View.ObjectSpace.GetObjectByKey(translateObject.SystemType, translateObject.ObjectID);                    
                    if (objTranslate != null)
                    {
                        htmlDocument.LoadHtml(translateObject.Content);
                        int index = 1;
                        var first = video.GetAudioListWithSort(true).Where(x => x.TranslateObject == translateObject).FirstOrDefault();
                        if (first != null)
                            index = System.Convert.ToInt32(first.Start?.TotalSeconds);
                        videoService.FillContentFromHtmlNode(video, ref index, htmlDocument.DocumentNode, translateObject, null, true);
                        if (objTranslate is Module.BusinessObjects.Post)
                        {
                            var post = (Module.BusinessObjects.Post)objTranslate;
                            if (string.IsNullOrEmpty(post.ContentOrigin))
                                post.ContentOrigin = post.Content;
                        }
                       Module.Helpers.ReflectionHelper.SetPropertyValueInObject(objTranslate, translateObject.Field.Value as string, htmlDocument.DocumentNode.InnerHtml);
                        result++;
                    }
                }
            }
            Module.Helpers.XafXpoHelper.ShowMessage(Application, "Kết quả", result.ToString("D") + "/" + View.SelectedObjects.Count.ToString("D") + " được dịch");

            #endregion ExportTranslateObjectImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 1396            Oid: c47b96a0-af7f-49b2-8b47-2102f737443a
		private void CheckLinkImage_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(CheckLinkImage), "Tồn tại liên kết");              
      
            #region CheckLinkImageImportCode
            // Tạo chức năng kiểm tra trong tất cả TranslateObject trong View.SelectObjects, các node p trong Content có chứa link hoặc ảnh thì set Flag = true
            foreach (Module.BusinessObjects.TranslateObject translateObject in View.SelectedObjects)
            {
                if (string.IsNullOrEmpty(translateObject.Content))
                    continue;
                var htmlDocument = new HtmlAgilityPack.HtmlDocument();
                htmlDocument.LoadHtml(translateObject.Content);
                var nodes = htmlDocument.DocumentNode.Descendants("p");
                foreach (var node in nodes)
                {
                    if (node.Descendants("a")?.Count() > 0 || node.Descendants("img")?.Count() > 0)
                    {
                        translateObject.Flag = true;
                        break;
                    }
                }
            }
            Module.Helpers.XafXpoHelper.ShowMessage(Application, "Kết quả", "Đã kiểm tra xong");

            #endregion CheckLinkImageImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 1307            Oid: 871fd536-1b9e-480c-b30c-37d8dad41c63
		private void ImportTranslateObject_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(ImportTranslateObject), "Nạp");              
      
            #region ImportTranslateObjectImportCode
                                                            using (DevExpress.ExpressApp.SystemModule.DialogController dc =
                        Application.CreateController<DevExpress.ExpressApp.SystemModule.DialogController>())
            {
                dc.Accepting += delegate (object o, DevExpress.ExpressApp.SystemModule.DialogControllerAcceptingEventArgs args)
                {
                    var video = Module.SystemObjects.Tools.GetMasterObjectFromView(View) as Module.BusinessObjects.Video;
                    if(args.AcceptActionArgs.CurrentObject is Module.BusinessObjects.Product)
                    {
                        var fieldList = new string[] { "Feature", "Introduction", "Specification" };
                        foreach (Module.BusinessObjects.Product product in args.AcceptActionArgs.SelectedObjects)
                        {
                            foreach(var field in fieldList)
                            {
                                var translateObject = View.ObjectSpace.CreateObject<Module.BusinessObjects.TranslateObject>();
                                translateObject.Name = product.Name;
                                translateObject.SystemType = typeof(Module.BusinessObjects.Product);
                                foreach (var refField in translateObject.FieldSource)
                                    if (field.Equals(refField.Value))
                                    {
                                        translateObject.Field = refField;
                                        break;
                                    }                                        
                                translateObject.Content = product.GetPropertyValue(field) as string;
                                translateObject.ObjectID = product.Oid;
                                if (View is ListView)
                                    ((ListView)View).CollectionSource.Add(translateObject);
                                if (translateObject.Video is null)
                                    translateObject.Video = video;
                            }
                            
                        }
                    }else if (args.AcceptActionArgs.CurrentObject is Module.BusinessObjects.Post)
                    {
                        string fieldName = "Content";
                        foreach (Module.BusinessObjects.Post post in args.AcceptActionArgs.SelectedObjects)
                        {
                            var translateObject = View.ObjectSpace.CreateObject<Module.BusinessObjects.TranslateObject>();
                            translateObject.Name = post.Name;
                            translateObject.SystemType = typeof(Module.BusinessObjects.Post);
                            foreach (var refField in translateObject.FieldSource)
                                if (fieldName.Equals(refField.Value))
                                {
                                    translateObject.Field = refField;
                                    break;
                                }
                            translateObject.Content = post.Content;
                            translateObject.ObjectID = post.Oid;
                            if (View is ListView)
                                ((ListView)View).CollectionSource.Add(translateObject);
                            if (translateObject.Video is null)
                                translateObject.Video = video;
                        }
                    }
                };
                var showViewParameters = new ShowViewParameters
                {
                    TargetWindow = TargetWindow.NewModalWindow,
                    CreateAllControllers = true,
                    NewWindowTarget = NewWindowTarget.Separate,
                    Context = TemplateContext.PopupWindow
                    //Context = TemplateContext.View
                };
                Module.Helpers.XafXpoHelper.PopupDialogControllerListView(this, dc, e.SelectedChoiceActionItem.Id.Equals("Product") ? typeof(Module.BusinessObjects.Product) : typeof(Module.BusinessObjects.Post), View.ObjectSpace, null, null, false, showViewParameters, false, false);
                //Module.Helpers.XafXpoHelper.PopupDialogControllerListView(this, dc, typeof(Module.BusinessObjects.BookMark), View.ObjectSpace, null, null, false, null, false, false);
            }
            #endregion ImportTranslateObjectImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 1308            Oid: b395c182-6797-469e-a5e2-dad10e70389e
		private void ImportObjectElement_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(ImportObjectElement), "Nạp thành phần");              
      
            #region ImportObjectElementImportCode
            var video = Module.SystemObjects.Tools.GetMasterObjectFromView(View) as Module.BusinessObjects.Video;
            if (video is null)
                return;
            int index = 1;            
            var lasted = video.GetAudioListWithSort(false).FirstOrDefault();
            //Vị trí dòng tiếp theo là vị trí cuối cùng thêm 1
            if (lasted != null)
                index = System.Convert.ToInt32(lasted.Start?.TotalSeconds) + 1;
                        
            //HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
            var htmlDocument = new HtmlAgilityPack.HtmlDocument();
            int add = 0, existed = 0;
            foreach (Module.BusinessObjects.TranslateObject translateObject in View.SelectedObjects)
            {
                if (video.AudioList.FirstOrDefault(x => x.TranslateObject == translateObject) != null)
                {
                    existed++;
                    continue;
                }
                add++;
                htmlDocument.LoadHtml(translateObject.Content);
                videoService.FillContentFromHtmlNode(video, ref index, htmlDocument.DocumentNode, translateObject, null);
            }
            string message = add + " đối tượng được nạp";
            if (existed > 0)
                message += existed + " đối tượng đã tồn tại";
            Module.Helpers.XafXpoHelper.ShowMessage(Application, "Kết quả", message);


            #endregion ImportObjectElementImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
     }
}