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
    public partial class WorkViewController: BaseViewController<Module.BusinessObjects.Work>
    {      
        
        public WorkViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(Module.BusinessObjects.Work);    
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
             
            #region WorkRelativeObjectOnViewControlsCreatedCode
		    		    if (WorkRelativeObject.Items.Count == 0)
            {
                WorkRelativeObject.Items.Add(new ChoiceActionItem("Open", "Mở", "Open"));
                var member = SecuritySystem.CurrentUser as Module.BusinessObjects.Member;
                if(member != null)
                {
                    var choiceActionItemList = new System.Collections.Generic.List<ChoiceActionItem>();
                    foreach (var memberSystemType in member.MemberObjectSystemTypeList)
                    {
                        if (memberSystemType.SystemType is null)
                            continue;
                        var viewItem = Application.Model.BOModel.FirstOrDefault(x => x.Name == memberSystemType.SystemType.FullName);
                        var choiceItem = new ChoiceActionItem(viewItem != null ? viewItem.Caption : memberSystemType.SystemType.Name, memberSystemType.SystemType);
                        choiceActionItemList.Add(choiceItem);
                    }
                    foreach(var  choiceActionItem in choiceActionItemList.OrderBy(x => x.Caption))
                    {
                        WorkRelativeObject.Items.Add(choiceActionItem);
                    }
                }
                WorkRelativeObject.Items.Add(new ChoiceActionItem("Delete", "Xóa", "Delete"));
            }
		    #endregion WorkRelativeObjectOnViewControlsCreatedCode
        }
        
        protected override void OnDeactivated()
        {                 
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }


        
        //Code: 1384            Oid: 13660add-99f6-4d55-aa64-a580cf0f8b90
		private void WorkRelativeObject_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(WorkRelativeObject), "Đối tượng");              
      
            #region WorkRelativeObjectImportCode
            if (e.SelectedChoiceActionItem.Id.Equals("Open") || e.SelectedChoiceActionItem.Id.Equals("Delete"))
            {
                int delete = 0;
                string deleteName = "";
                foreach (Module.BusinessObjects.Work work in View.SelectedObjects)
                {
                    if (work.SystemType is null)
                    {
                        Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Đối tượng được chọn không tồn tại kiểu dữ liệu", InformationType.Error);
                        return;
                    }
                    
                    if (e.SelectedChoiceActionItem.Id.Equals("Open"))
                    {
                        var objectSpace = Application.CreateObjectSpace();
                        var parentObject = objectSpace.GetObjectByKey(work.SystemType, work.ObjectID) as DevExpress.Xpo.PersistentBase;
                        if (parentObject is null)
                        {
                            Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Không tìm thấy đối tượng cấp trên", InformationType.Error);
                            return;
                        }
                        Module.Helpers.XafXpoHelper.CreateDialogControllerDetailView(this, null, parentObject, objectSpace, true);
                    }
                    else if (e.SelectedChoiceActionItem.Id.Equals("Delete"))
                    {
                        var parentObject = View.ObjectSpace.GetObjectByKey(work.SystemType, work.ObjectID) as DevExpress.Xpo.PersistentBase;
                        if (parentObject is null)
                        {
                            Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Không tìm thấy đối tượng cấp trên", InformationType.Error);
                            return;
                        }
                        View.ObjectSpace.Delete(parentObject);
                        work.ObjectID = System.Guid.Empty;
                        work.SystemType = null;
                       Module.Helpers.ReflectionHelper.SetPropertyValue(work, "Work", null);
                        if(View.SelectedObjects.Count == 1)
                        deleteName = DevExpress.ExpressApp.Utils.CaptionHelper.GetDisplayText(parentObject);
                           
                    }
                }
                if (e.SelectedChoiceActionItem.Id.Equals("Delete"))
                {
                    if (string.IsNullOrEmpty(deleteName))
                        deleteName = delete + " đối tượng";
                    Module.Helpers.XafXpoHelper.ShowMessage(Application, "Kết quả", deleteName + " bị xóa");
                }
                return;
            }
            var systemType = e?.SelectedChoiceActionItem?.Data as System.Type;
            if(systemType is null)
            {
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Kiểu dữ liệu không hợp lệ", InformationType.Error);
                return;
            }
            var currentObject = View?.CurrentObject as Module.BusinessObjects.Work;
            if (currentObject is null)
            {
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Đối tượng được chọn không hợp lệ", InformationType.Error);
                return;
            }
            
            using (DevExpress.ExpressApp.SystemModule.DialogController dc = Application.CreateController<DevExpress.ExpressApp.SystemModule.DialogController>())
            {
                dc.Accepting += delegate (object oFolder, DevExpress.ExpressApp.SystemModule.DialogControllerAcceptingEventArgs args)
                {
                    var selectObject = args.AcceptActionArgs.CurrentObject as DevExpress.Xpo.PersistentBase;
                    if (selectObject != null)
                    {
                        
                        var oid = selectObject.GetPropertyValue("Oid");
                        if (oid is System.Guid)
                            currentObject.ObjectID = (System.Guid)selectObject.GetPropertyValue("Oid");
                        currentObject.SystemType = selectObject.GetType();
                    }
                };
                DevExpress.Data.Filtering.CriteriaOperator cirtera = null;
                var memberProperty = systemType.GetProperty("Member");
                if(memberProperty != null)
                {
                    cirtera = DevExpress.Data.Filtering.CriteriaOperator.Parse("Member is null or Member.Oid = ?", SecuritySystem.CurrentUserId);
                }
                var inActiveProperty = systemType.GetProperty("InActive");
                if (inActiveProperty != null)
                {
                    cirtera = DevExpress.Data.Filtering.CriteriaOperator.Or(cirtera, DevExpress.Data.Filtering.CriteriaOperator.Parse("InActive = False"));
                }
                Module.Helpers.XafXpoHelper.PopupDialogControllerListView(this, dc, systemType, View.ObjectSpace, "FilterByCurrentUser", cirtera, false, null, true, true);
            }
            #endregion WorkRelativeObjectImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
     }
}