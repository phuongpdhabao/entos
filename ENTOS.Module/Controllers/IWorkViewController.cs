using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.XtraGrid;
using ENTOS.Module.BusinessObjects;
using ListView = DevExpress.ExpressApp.ListView;
using ENTOS.Module.SystemObjects;
using System.Linq;

namespace ENTOS.Module.Controllers 
{
    public partial class IWorkViewController: ViewController
    {      
        
        public IWorkViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(ENTOS.Module.BusinessObjects.IWork);    
            //TargetViewNesting = Nesting.Nested;
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

        
		private void Work_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;

            #region WorkImportCode
                        if (View.SelectedObjects.Count == 0)
            {
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Vui lòng chọn đối tượng", InformationType.Error);
                return;
            }
            string name = View.ObjectTypeInfo.Name;
            var bOModel = Application.Model.BOModel.FirstOrDefault(m => m.Name.Equals(View.ObjectTypeInfo.FullName));
            if (bOModel != null)
                name = bOModel.Caption;
            var keyField = View.ObjectSpace.GetKeyPropertyName(View.ObjectTypeInfo.Type);
            if (string.IsNullOrEmpty(keyField))
            {
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Không tìm thấy khóa chính", InformationType.Error);
                return;
            }
            int created = 0;
            IObjectSpace newObjectSpace = null;
            bool isModified = View.ObjectSpace.IsModified;
            foreach (Module.BusinessObjects.IWork createWork in View.SelectedObjects)
            {
                var keyObject = createWork.GetPropertyValue(keyField) as System.Guid?;
                if (keyObject is null)
                    continue;
                if (e.SelectedChoiceActionItem.Id.Equals("Open") || e.SelectedChoiceActionItem.Id.Equals("UnLink"))
                {

                    var refWork = View.ObjectSpace.FindObject<Module.BusinessObjects.Work>(DevExpress.Data.Filtering.CriteriaOperator.Parse("SystemType = ? and ObjectID = ?", View.ObjectTypeInfo.Type, keyObject.Value), false);
                    Module.BusinessObjects.WorkDetail workDetail = null;
                    //var refWork = createWork.Work;
                    if (refWork is null)
                    {
                        if (refWork is null)
                            workDetail = View.ObjectSpace.FindObject<Module.BusinessObjects.WorkDetail>(DevExpress.Data.Filtering.CriteriaOperator.Parse("SystemType = ? and ObjectID = ?", View.ObjectTypeInfo.Type, keyObject.Value), false);
                        if (workDetail is null)
                        {
                            if (View.SelectedObjects.Count == 1)
                                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Không có công việc", InformationType.Error);
                            continue;
                        }
                        else
                        {
                            refWork = workDetail.Work;
                        }

                    }

                    //037: Chỉ có quyền Open, Link, Unlink khi LoginID = Thực hiện của Work
                    if ((refWork.Member != null && refWork.Member.Oid == SecuritySystem.CurrentUserId as System.Guid?) ||
                        (refWork.Requester != null && refWork.Requester.Oid == SecuritySystem.CurrentUserId as System.Guid?))
                    {
                        if (e.SelectedChoiceActionItem.Id.Equals("UnLink"))
                        {
                            if (workDetail is null)
                            {
                                refWork.SystemType = null;
                                refWork.ObjectID = System.Guid.Empty;
                                refWork.Reference = null;
                                //createWork.Work = null;
                                if (!isModified || (View.IsRoot && View is ListView))
                                {
                                    refWork.Session.CommitTransaction();
                                    View.ObjectSpace.CommitChanges();
                                }
                            }
                            else
                            {
                                workDetail.SystemType = null;
                                workDetail.ObjectID = System.Guid.Empty;
                                if (!isModified || (View.IsRoot && View is ListView))
                                {
                                    workDetail.Session.CommitTransaction();
                                    View.ObjectSpace.CommitChanges();
                                }
                            }

                        }
                        else
                        {
                            if (workDetail is null)
                            {
                                if (newObjectSpace == null)
                                    newObjectSpace = Application.CreateObjectSpace(typeof(Module.BusinessObjects.Work));
                                //Tìm đối tượng theo objectSpace mới để tránh xung đột
                                var workOtherObjectSpace = newObjectSpace.GetObjectByKey<Module.BusinessObjects.Work>(refWork.Oid);
                                Module.Helpers.XafXpoHelper.CreateDialogControllerDetailView(this, null, workOtherObjectSpace, newObjectSpace, false);
                            }
                            else
                            {
                                if (newObjectSpace == null)
                                    newObjectSpace = Application.CreateObjectSpace(typeof(Module.BusinessObjects.WorkDetail));
                                //Tìm đối tượng theo objectSpace mới để tránh xung đột
                                var workOtherObjectSpace = newObjectSpace.GetObjectByKey<Module.BusinessObjects.WorkDetail>(workDetail.Oid);
                                Module.Helpers.XafXpoHelper.CreateDialogControllerDetailView(this, null, workOtherObjectSpace, newObjectSpace, false);
                            }

                        }
                    }
                    else
                    {
                        Module.Helpers.XafXpoHelper.ShowMessage(Application, "Thông báo", "Bạn không phải là người thực hiện hiện công việc này", InformationType.Error);
                        continue;
                    }
                }
                else if (e.SelectedChoiceActionItem.Id.Equals("Link"))
                {
                    using (DevExpress.ExpressApp.SystemModule.DialogController dc =
                        Application.CreateController<DevExpress.ExpressApp.SystemModule.DialogController>())
                    {
                        dc.Accepting += delegate (object o, DevExpress.ExpressApp.SystemModule.DialogControllerAcceptingEventArgs args)
                        {
                            foreach (Module.BusinessObjects.Work selectWork in args?.AcceptActionArgs?.SelectedObjects)
                            {
                                //037: Chỉ có quyền Open, Link, Unlink khi LoginID = Thực hiện của Work
                                if ((selectWork.Member != null && selectWork.Member.Oid == SecuritySystem.CurrentUserId as System.Guid?) ||
                                    (selectWork.Requester != null && selectWork.Requester.Oid == SecuritySystem.CurrentUserId as System.Guid?))
                                {
                                    if (string.IsNullOrEmpty(selectWork.Name))
                                        selectWork.Name = name + " " + createWork.Code + " " + createWork.Name;
                                    selectWork.SystemType = View.ObjectTypeInfo.Type;
                                    selectWork.ObjectID = keyObject.Value;
                                    //2024: Khi tạo hay liên kết Công việc sẽ
                                    //-gán tên SystemType của đối tượng cho Reference: Ví dụ Báo giá, Tư liệu
                                    //- Lưu Oid của CV tương ứng vào WorkOid

                                    selectWork.Reference = name;
                                    //066
                                    //-Lưu Oid của CV tương ứng vào WorkOid
                                    //2025-05-13: Bỏ tham chiếu ngược
                                    //createWork.Work = selectWork;

                                    if (selectWork.Requester is null)
                                        selectWork.Requester = Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(selectWork.Session);
                                    if (createWork.Member != null)
                                        selectWork.Member = createWork.Member;
                                    else if (selectWork.Member is null)
                                        selectWork.Member = selectWork.Requester;
                                    if (View.IsRoot && View is ListView)
                                        selectWork.Session.CommitTransaction();
                                }
                                else
                                {
                                    Module.Helpers.XafXpoHelper.ShowMessage(Application, "Thông báo", "Bạn không phải là người thực hiện hiện công việc này", InformationType.Error);
                                    continue;
                                }
                            }
                        };
                        //Khi liên kết tới Công việc cần lọc các công việc: Người giao hoặc Thành viên = LoginOid & Trạng thái công việc khác Hoàn thành / Hủy
                        var criteria = DevExpress.Data.Filtering.CriteriaOperator.Parse("(Member.Oid = ? or Requester.Oid = ?) and not (Status.Code in ('Completed', 'Canceled'))", SecuritySystem.CurrentUserId, SecuritySystem.CurrentUserId);
                        Module.Helpers.XafXpoHelper.PopupDialogControllerListView(this, dc, typeof(Module.BusinessObjects.Work), View.ObjectSpace, "IWorkCurrent", criteria, false, null, true, false);
                        //Module.Helpers.XafXpoHelper.PopupDialogControllerListView(this, dc, typeof(Module.BusinessObjects.BookMark), View.ObjectSpace, null, null, false, null, false, false);
                    }
                }
                else if (e.SelectedChoiceActionItem.Id.Equals("Create"))
                {
                    //Tạo
                    //Các trường yêu cầu:
                    //-Code
                    //- Name
                    //- Member(optional)
                    //- SoftwareClass(optional)

                    //Copy 1 số dữ liệu để đưa vào công việc

                    //-Loại đối tượng +Code + Name > Tên công việc
                    //: VD: Báo giá 032 Tekcast dự án VTV
                    //-Thành viên > thực hiện
                    //- Login User > Đề xuất / Thực hiện
                    //var softwareClass = createWork.GetSoftwareClass();
                    //2023-08-18: Nếu đối tượng phần mềm trống thì tạo công việc, ngược lại tạo đối tượng phần mềm
                    //if(softwareClass is null)
                    //{

                    Module.BusinessObjects.Work work = View.ObjectSpace.CreateObject<Module.BusinessObjects.Work>();
                    //if (isModified)
                    //{
                    //    work = View.ObjectSpace.CreateObject<Module.BusinessObjects.Work>();
                    //}
                    //else
                    //{
                    //    if (newObjectSpace == null)
                    //        newObjectSpace = Application.CreateObjectSpace(typeof(Module.BusinessObjects.Work));
                    //    work = newObjectSpace.CreateObject<Module.BusinessObjects.Work>();

                    //}

                    work.Name = name + " " + createWork.Code + " " + createWork.Name;
                    //var member = createWork.GetMember();
                    //if (member != null)
                    //    work.Member = member;
                    work.SystemType = View.ObjectTypeInfo.Type;
                    work.ObjectID = keyObject.Value;
                    work.Reference = name;


                    //else
                    //    work.SetDefaultMember();                  
                    work.Requester = Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(work.Session);
                    if (createWork.Member != null)
                        work.Member = work.Session.GetObjectByKey<Member>(createWork.Member.Oid);
                    else
                        work.Member = work.Requester;
                    //if (isModified)
                    //{
                    //    createWork.Work = work;
                    //}
                    //2025-05-13: Bỏ tham chiếu ngược
                    //createWork.Work = work;
                    created++;
                    if (!isModified || (View.IsRoot && View is ListView))
                    {
                        work.Session.CommitTransaction();
                        View.ObjectSpace.CommitChanges();
                    }
                    //066
                    //-Lưu Oid của CV tương ứng vào WorkOid
                    //if (!isModified)
                    //{
                    //    work.Session.CommitTransaction();
                    //    var currentObjObjectSpace = newObjectSpace.GetObjectByKey(View.ObjectTypeInfo.Type, keyObject) as Module.BusinessObjects.IWork;
                    //    if (currentObjObjectSpace != null)
                    //    {
                    //        currentObjObjectSpace.Work = work;
                    //        newObjectSpace.CommitChanges();
                    //    }
                    //}


                    //}
                    //else
                    //{
                    //    var softwareRequirement = View.ObjectSpace.CreateObject<Module.BusinessObjects.SoftwareRequirement>();
                    //    softwareRequirement.Name = name + " " + createWork.Code + " " + createWork.Name;
                    //    var member = createWork.GetMember();
                    //    if (member != null)
                    //        softwareRequirement.Member = member;
                    //    //else
                    //    //    work.SetDefaultMember();
                    //    softwareRequirement.SoftwareClass = createWork.GetSoftwareClass();
                    //    softwareRequirement.Requester = softwareRequirement.Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(Session);
                    //    if (View is ListView && View.IsRoot)
                    //        softwareRequirement.Session.CommitTransaction();
                    //}
                }

            }
            if (created > 0)
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Kết quả", created + "/" + View.SelectedObjects.Count + " công việc được tạo");

            #endregion WorkImportCode
		}
     }   
}