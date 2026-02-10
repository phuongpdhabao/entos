using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.XtraGrid;
using ListView = DevExpress.ExpressApp.ListView;
using ENTOS.Module.BusinessObjects;
using ENTOS.Module.SystemObjects;
using System.Linq;


namespace ENTOS.Module.Controllers 
{
    public partial class INewObjectSessionViewController: ViewController<ListView>
    {      
        
        public INewObjectSessionViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(Module.BusinessObjects.INewObjectSession);    
            TargetViewNesting = Nesting.Nested;
            
        }
		
		protected override void OnActivated()
        {
            base.OnActivated();
            if (View is DetailView)
            {   
                //if (Frame is WinWindow)
                    //((WinWindow) Frame).KeyDown += WindowController1_KeyDown;
                //if (Frame is WebWindow)
                    //((WebWindow) Frame).PagePreRender += CurrentRequestWindow_PagePreRender;           
            }else if (View is ListView){
                //var parent = View.ObjectSpace.Owner as DetailView;
            }
            //Chỉ hiện action khi đối tượng mẹ là Folder
            var masterObject = Module.SystemObjects.Tools.GetMasterObjectFromView(View);
            if(masterObject is null || !(masterObject is Module.BusinessObjects.Folder))
            {
                NewObjectSession.Active["NonFolder"] = false;                
            }
        }
        
        
        protected override void OnViewControlsCreated()
        {
             base.OnViewControlsCreated();
             if(View is ListView){
                
             }
        }
        
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
        
        //Code: 1999            Oid: 551a0eb1-e0f3-4d7a-ba15-552ec6d9bf40
		private void NewObjectSession_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
            #region NewObjectSessionImportCode
            var objSpace = Application.CreateObjectSpace();
            if (e.SelectedChoiceActionItem.Id.Equals("Create"))
            {                
                var newObject = objSpace.CreateObject(View.ObjectTypeInfo.Type);
                var masterObject = Module.SystemObjects.Tools.GetMasterObjectFromView(View);
                if(masterObject != null)
                {
                    var masterObjKey = objSpace.GetKeyValue(masterObject);
                    if(masterObjKey != null)
                    {
                        var masterObj = objSpace.GetObjectByKey(masterObject.GetType(), masterObjKey);
                        if (masterObj != null)
                        {
                            bool bindingList = false;
                            if (View is ListView)
                            {
                                var collection = ((PropertyCollectionSource)((ListView)View).CollectionSource);
                                if (collection.MemberInfo != null && collection.MemberInfo.AssociatedMemberInfo != null &&
                                    !collection.MemberInfo.AssociatedMemberInfo.IsList && !string.IsNullOrEmpty(collection.MemberInfo.AssociatedMemberInfo.Name))
                                {
                                    collection.MemberInfo.AssociatedMemberInfo.SetValue(newObject, masterObj);
                                    bindingList = true;
                                }
                            }
                            if(!bindingList)
                                ((Module.BusinessObjects.INewObjectSession)newObject).Folder = masterObj as Module.BusinessObjects.Folder;
                        }
                    }
                }
                Module.Helpers.XafXpoHelper.CreateDialogControllerDetailView(this, null, newObject, objSpace);
                if(!View.ObjectSpace.IsModified)
                    Module.SystemObjects.Tools.RefreshGridView(View);
                //var refObjKey = View.ObjectSpace.GetKeyValue(newObject);
                //if (refObjKey != null)
                //{

                //    var refObject = View.ObjectSpace.GetObjectByKey(View.ObjectTypeInfo.Type, refObjKey);
                //    if (refObject != null && View is ListView)
                //        ((ListView)View).CollectionSource.Add(refObject);
                //}
            }
            else if (e.SelectedChoiceActionItem.Id.Equals("Open"))
            {
                foreach (var obj in e.SelectedObjects)
                {
                    if (View.ObjectSpace.IsNewObject(obj))
                    {
                        Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Đối tượng này chưa được lưu", InformationType.Error);
                        return;
                    }
                    var refObjKey = objSpace.GetKeyValue(obj);
                    if (refObjKey != null)
                    {
                        var refObject = objSpace.GetObjectByKey(View.ObjectTypeInfo.Type, refObjKey);
                        Module.Helpers.XafXpoHelper.CreateDialogControllerDetailView(this, null, refObject, objSpace);
                    }
                    else
                    {
                        Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Không tìm thấy khóa chính của đối tượng này", InformationType.Error);
                        return;
                    }
                }
            }
            


            #endregion NewObjectSessionImportCode
		}
     }   
}