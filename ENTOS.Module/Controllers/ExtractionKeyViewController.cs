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
    public partial class ExtractionKeyViewController: BaseViewController<Module.BusinessObjects.ExtractionKey>
    {      
        
        public ExtractionKeyViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(Module.BusinessObjects.ExtractionKey);    
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


        
        //Code: 3880            Oid: d5cddfbf-4919-4315-85c1-77b790316113
		private void ExtractionKeyImport_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(ExtractionKeyImport), "Nạp khóa");              
      
            #region ExtractionKeyImportImportCode
            // --- Lấy ObjectSpace hiện tại ---
            var objectSpace = View.ObjectSpace;

            // --- Lấy OcrDocumentType hiện tại ---
            var currentDocType = Module.Helpers.XafXpoHelper
                .GetMasterObjectFromView<Module.BusinessObjects.ExtractionTemplate>(this.View);

            if (currentDocType == null || currentDocType.SystemType == null)
                return;

            // --- Xác định SystemType ---
            var systemType = currentDocType.SystemType;
            if (e.SelectedChoiceActionItem.Id.Equals("TableObject"))
                systemType = currentDocType.TableSystemType;
            if (e.SelectedChoiceActionItem.Id.Equals("Table2Object"))
                systemType = currentDocType.Table2SystemType;


            if (systemType == null) return;

            // --- Tạo danh sách FieldNonPersistent ---
            var typeInfo = XafTypesInfo.Instance.FindTypeInfo(systemType);
            if (typeInfo == null) return;

            var importList = new List<FieldNonPersistent>();

            //var newObjectSpace = Application.CreateObjectSpace(typeof(FieldNonPersistent));
            //var session = (newObjectSpace as XPObjectSpace)?.Session; // lấy Session
            foreach (var member in typeInfo.Members)
            {
                if (member.IsList && member.MemberType != typeof(string))
                    continue;

                if (!member.IsReadOnly &&
                    (member.IsVisible || member.FindAttribute<DevExpress.ExpressApp.Security.SecurityBrowsableAttribute>() != null))
                {
                    // ⚡ Tạo trực tiếp bằng constructor, không dùng ObjectSpace
                    var importItem = objectSpace.CreateObject<FieldNonPersistent>();
                    importItem.Name = DevExpress.ExpressApp.Utils.CaptionHelper.GetMemberCaption(member);
                    importItem.Code = member.Name;

                    // --- Lookup DataType ---
                    Type type = member.MemberType;
                    // --- Lookup DataType ---
                    var mapped = objectSpace.FindObject<Module.BusinessObjects.DataType>(
                        DevExpress.Data.Filtering.CriteriaOperator.Parse(
                            "[Code] = ?", type.Name
                        )
                    );
                    if (mapped != null)
                        importItem.DataType = mapped;
                    else
                    {
                        var fullName = Module.Helpers.ReflectionHelper.SimplifyTypeName(type.FullName);
                        mapped = objectSpace.FindObject<Module.BusinessObjects.DataType>(
                            DevExpress.Data.Filtering.CriteriaOperator.Parse(
                                "[FullName] = ?", fullName
                            )
                        );
                    }
                    if (mapped != null)
                        importItem.DataType = mapped;
                    else
                    {
                        var fullName = Module.Helpers.ReflectionHelper.SimplifyTypeName(type.FullName);

                        string pattern = @"\[\[(.*?)\]\]";
                        var match = System.Text.RegularExpressions.Regex.Match(fullName, pattern);

                        // Lấy tên kiểu rút gọn (vd: Int32, String, DateTime)
                        var shortTypeName = match.Success
                            ? match.Groups[1].Value.Split('.').Last()
                            : fullName.Split('.').Last();

                        // Nếu fullName có Nullable thì thêm ?
                        var trueTypeName = fullName.StartsWith("System.Nullable`1")
                            ? shortTypeName + "?"
                            : shortTypeName;

                        var criteria = DevExpress.Data.Filtering.CriteriaOperator.Parse("[Code] = ?", trueTypeName);
                        mapped = objectSpace.FindObject<Module.BusinessObjects.DataType>(criteria);
                    }
                    if (mapped != null)
                        importItem.DataType = mapped;

                    importList.Add(importItem);
                }
            }

            // --- CollectionSource
            var collectionSource = new CollectionSource(objectSpace, typeof(FieldNonPersistent));
            foreach (var item in importList)
                collectionSource.Add(item);

            // --- ListView không phải root
            var listViewId = Application.FindLookupListViewId(typeof(FieldNonPersistent));
            var listView = Application.CreateListView(listViewId, collectionSource, false); // false = non-root

            // --- DialogController
            var dc = Application.CreateController<DevExpress.ExpressApp.SystemModule.DialogController>();
            dc.SaveOnAccept = false;
            dc.Accepting += (s, args) =>
            {
                var selectedObjects = listView.SelectedObjects.Cast<FieldNonPersistent>().ToList();
                if (selectedObjects.Any())
                {
                    var docType = objectSpace.GetObject(currentDocType);

                    foreach (var sel in selectedObjects)
                    {
                        var newKey = objectSpace.CreateObject<ExtractionKey>();
                        newKey.Name = sel.Name;
                        newKey.Code = sel.Code;
                        newKey.DataType = sel.DataType != null ? objectSpace.GetObject(sel.DataType) : null;
                        newKey.SystemTypeCode = systemType.Name;
                        newKey.DataTypeCategory = newKey.DataType?.DataTypeCategory;

                        if (e.SelectedChoiceActionItem.Id.Equals("TableObject"))
                            newKey.DataLayout = DataLayout.Table;
                        else if (e.SelectedChoiceActionItem.Id.Equals("Table2Object"))
                            newKey.DataLayout = DataLayout.Table2;

                        docType.ExtractionKeyList.Add(newKey);
                    }
                    //os.CommitChanges();
                }
            };

            // --- Show popup
            var svp = new ShowViewParameters(listView)
            {
                TargetWindow = TargetWindow.NewModalWindow,
                CreatedView = listView
            };
            svp.Controllers.Add(dc);
            Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(Frame, null));

            #endregion ExtractionKeyImportImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
     }
}