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
    public partial class WorkDetailViewController: BaseViewController<Module.BusinessObjects.WorkDetail>
    {      
        
        public WorkDetailViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(Module.BusinessObjects.WorkDetail);    
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
             
            #region WorkDetailRelativeObjectOnViewControlsCreatedCode
		    		    if (WorkDetailRelativeObject.Items.Count == 0)
            {
                WorkDetailRelativeObject.Items.Add(new ChoiceActionItem("Open", "Mở", "Open"));
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
                        WorkDetailRelativeObject.Items.Add(choiceActionItem);
                    }
                }
                WorkDetailRelativeObject.Items.Add(new ChoiceActionItem("Delete", "Xóa", "Delete"));
            }
		    #endregion WorkDetailRelativeObjectOnViewControlsCreatedCode
        }
        
        protected override void OnDeactivated()
        {                 
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }


        
        //Code: 1549            Oid: 360ced05-5c4a-4bdb-8858-cc1abe8ed965
		private void ImportWorkDetail_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(ImportWorkDetail), "Nạp");              
      
            #region ImportWorkDetailImportCode
            //Copy các bước công việc của Loại công việc tương ứng
            var work = Module.SystemObjects.Tools.GetMasterObjectFromView(View) as Work;
            if(work is null)
                return;
            if(work.WorkType is null)
            {
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Chưa chọn Loại công việc", InformationType.Error);
                return;
            }
            if (work.WorkType.WorkDetailList.Count == 0)
            {
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Loại công việc đang chọn không tồn tại chi tiết", InformationType.Error);
                return;
            }
            foreach(var refWorkDetail in work.WorkType.WorkDetailList)
            {
                var workDetail = new Module.BusinessObjects.WorkDetail(work.Session);                
                workDetail.Work = work;
                workDetail.Order = refWorkDetail.Order;
                workDetail.Name = refWorkDetail.Name;
                workDetail.Note = refWorkDetail.Note;
            }

            #endregion ImportWorkDetailImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 3195            Oid: c2fff35d-889b-4813-a913-d6bd538225ff
		private void WorkDetailRelativeObject_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(WorkDetailRelativeObject), "Đối tượng");              
      
            #region WorkDetailRelativeObjectImportCode
            if (e.SelectedChoiceActionItem.Id.Equals("Open") || e.SelectedChoiceActionItem.Id.Equals("Delete"))
            {
                int delete = 0;
                string deleteName = "";
                foreach (Module.BusinessObjects.WorkDetail workDetail in View.SelectedObjects)
                {
                    if (workDetail.SystemType is null)
                    {
                        Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Đối tượng được chọn không tồn tại kiểu dữ liệu", InformationType.Error);
                        return;
                    }
                    
                    if (e.SelectedChoiceActionItem.Id.Equals("Open"))
                    {
                        var objectSpace = Application.CreateObjectSpace();
                        var parentObject = objectSpace.GetObjectByKey(workDetail.SystemType, workDetail.ObjectID) as DevExpress.Xpo.PersistentBase;
                        if (parentObject is null)
                        {
                            Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Không tìm thấy đối tượng cấp trên", InformationType.Error);
                            return;
                        }
                        Module.Helpers.XafXpoHelper.CreateDialogControllerDetailView(this, null, parentObject, objectSpace , true);
                    }
                    else if (e.SelectedChoiceActionItem.Id.Equals("Delete"))
                    {
                        var parentObject = View.ObjectSpace.GetObjectByKey(workDetail.SystemType, workDetail.ObjectID) as DevExpress.Xpo.PersistentBase;
                        if (parentObject is null)
                        {
                            Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Không tìm thấy đối tượng cấp trên", InformationType.Error);
                            return;
                        }
                        View.ObjectSpace.Delete(parentObject);
                        workDetail.ObjectID = System.Guid.Empty;
                        workDetail.SystemType = null;
                       Module.Helpers.ReflectionHelper.SetPropertyValue(workDetail, "WorkDetail", null);
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
            var currentObject = View?.CurrentObject as Module.BusinessObjects.WorkDetail;
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

            #endregion WorkDetailRelativeObjectImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
     }
}