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

    public partial class FolderService : BaseService
    {

        public FolderService() : base()
        {
        }
        #region DependencyInjection
  
  
        #endregion DependencyInjection

        public FolderService(ViewController viewController) : base(viewController)
        {

        }
        //public event EventHandler<string>? OnError;
        //public event EventHandler<string>? OnSuccess;
        //public event EventHandler<string>? OnResponse; 
        
        #region SourceCode4513ImportCode
                
        public void MemberFolderLoad(string data, Folder folder)
        {
            string[] parts = data.Split(new[] { '{', '}' }, System.StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length > 0)
            {
                string oidString = parts[1]; // Extracting directly from the first split
                if (System.Guid.TryParse(oidString, out System.Guid oid))
                {
                    var tempFolder = ObjectSpace.FindObject<Folder>(
                        DevExpress.Data.Filtering.CriteriaOperator.Parse("Oid = ? and FolderType = 'Member'", oid));
                    if (folder.FolderType.ToString() == "Accounting")
                    {
                        folder.MemberFolder = tempFolder;
                    }
                    foreach (var childFolder in folder.LowerFolder)
                    {
                        MemberFolderLoad( data, childFolder);
                    }
                }
            }
        }

        private System.Collections.Generic.IList<Folder> allFolders = null;
        public void CreateDefaultFilter(DevExpress.Data.Filtering.CriteriaOperator criteria, DevExpress.ExpressApp.Actions.SingleChoiceAction singleChoiceAction, ViewController viewController)
        {
            if (allFolders == null)
                allFolders = viewController.View.ObjectSpace.GetObjects<Folder>(new DevExpress.Data.Filtering.BinaryOperator("FolderType", "Member"), new System.Collections.Generic.List<DevExpress.Xpo.SortProperty> { new DevExpress.Xpo.SortProperty { PropertyName = "Order" } }, false);
            var listFolder = allFolders.Where(x => x.UpperFolder.FolderType.ToString() != "Member");
            if (listFolder.Count() == 0)
                return;
            existed = new System.Collections.Generic.List<System.Guid>();
            System.Collections.Generic.IList<DevExpress.ExpressApp.Actions.ChoiceActionItem> listRemove = new System.Collections.Generic.List<DevExpress.ExpressApp.Actions.ChoiceActionItem>();
            foreach (var item in singleChoiceAction.Items)
            {
                if (item.Data != null)
                {
                    bool remove = true;
                    foreach (var folder in listFolder)
                    {
                        if (item.Id.Equals(folder.Oid.ToString()))
                        {
                            remove = false;
                            break;
                        }
                    }

                    if (remove)
                    {
                        listRemove.Add(item);
                    }
                }
            }

            foreach (var item in listRemove)
            {
                singleChoiceAction.Items.Remove(item);
            }

            foreach (var folder in listFolder)
            {
                if (singleChoiceAction.Items.FindItemByID(folder.Oid.ToString()) == null)
                {
                    string data = "MemberFolder.Oid = ?";
                    //System.Reflection.Cu
                    var listAttribute = viewController.View.ObjectTypeInfo.Type.GetCustomAttributes(typeof(CustomFilter), true);
                    foreach (CustomFilter customAttribute in listAttribute)
                    {

                        if (customAttribute.Name.Equals("IFolder"))
                        {
                            data = customAttribute.Criteria;
                            break;
                        }
                    }
                    CreateTreeSource(singleChoiceAction, null, folder, data, "");
                }

            }
            if (singleChoiceAction.SelectedItem == null && !string.IsNullOrEmpty(SavedChoiceActionItem(viewController, singleChoiceAction).ToolTip))
            {
                var choiceItem = FindItemByID(singleChoiceAction.Items, SavedChoiceActionItem(viewController, singleChoiceAction).ToolTip);
                if (choiceItem != null)
                    singleChoiceAction.SelectedItem = choiceItem;
                //foreach (var choiceItem in filteringCriterionAction.Items)
                //{
                //    if (choiceItem.Id.Equals(SavedChoiceActionItem.ToolTip))
                //    {
                //        filteringCriterionAction.SelectedItem = choiceItem;
                //        break;
                //    }
                //}
            }
            if (singleChoiceAction.SelectedItem == null)
            {
                singleChoiceAction.SelectedIndex = 0;
            }
        }
        private DevExpress.ExpressApp.Actions.ChoiceActionItem FindItemByID(DevExpress.ExpressApp.Actions.ChoiceActionItemCollection items, string id)
        {
            var result = items.FindItemByID(id);
            if (result != null)
                return result;
            foreach (var childItem in items)
            {
                result = FindItemByID(childItem.Items, id);
                if (result != null)
                    return result;
            }
            return null;
        }
        internal System.Collections.Generic.IList<System.Guid> existedCriteria = null;

        internal DevExpress.Data.Filtering.CriteriaOperator AddAllChildCriteriaOperator(System.Guid currentItem, DevExpress.Data.Filtering.CriteriaOperator currentCriteriaOperator, string data)
        {
            if (!existedCriteria.Contains(currentItem))
            {
                existedCriteria.Add(currentItem);
                var lowerFolder = allFolders?.Where(x => x.UpperFolder != null && x.UpperFolder?.Oid == currentItem);
                if (lowerFolder != null && lowerFolder.Count() > 0)
                {
                    foreach (var childGroup in lowerFolder)
                    {
                        var childGroupParse = DevExpress.Data.Filtering.CriteriaOperator.Parse(data, childGroup.Oid, childGroup.Oid,
                            childGroup.Oid, childGroup.Oid, childGroup.Oid, childGroup.Oid, childGroup.Oid, childGroup.Oid,
                            childGroup.Oid, childGroup.Oid);
                        currentCriteriaOperator = DevExpress.Data.Filtering.CriteriaOperator.Or(currentCriteriaOperator, childGroupParse);
                        currentCriteriaOperator = AddAllChildCriteriaOperator(currentItem, currentCriteriaOperator, data);
                    }
                }
            }
            else
            {
                //Bị trùng
            }
            return currentCriteriaOperator;
        }


        private System.Collections.Generic.IList<System.Guid> existed = null;
        private void CreateTreeSource(DevExpress.ExpressApp.Actions.SingleChoiceAction singleChoiceAction, DevExpress.ExpressApp.Actions.ChoiceActionItem parentItem, Folder currentItem, string data, string prefix)
        {
            if (currentItem != null && !string.IsNullOrEmpty(data))
            {
                var foundItem = singleChoiceAction.Items.FindItemByID(currentItem.Oid.ToString());
                if (foundItem == null && parentItem != null)
                {
                    foundItem = parentItem.Items.FindItemByID(currentItem.Oid.ToString());
                }
                if (foundItem == null)
                {
                    if (currentItem.Oid == System.Guid.Parse("d5b71606-ffb4-434c-a975-52a5741bfd24"))
                    {

                    }
                    if (existed.Contains(currentItem.Oid))
                        return;
                    existed.Add(currentItem.Oid);
                    //var criteriaParse = CriteriaOperator.Parse(data, currentItem.Oid, currentItem.Oid,
                    //    currentItem.Oid,
                    //    currentItem.Oid, currentItem.Oid, currentItem.Oid, currentItem.Oid,
                    //    currentItem.Oid,
                    //    currentItem.Oid,
                    //    currentItem.Oid);
                    //criteriaParse = AddAllChildCriteriaOperator(currentItem, criteriaParse, data);

                    //string parser = criteriaParse.LegacyToString();
                    //var choiceAction = new ChoiceActionItem(currentItem.Oid.ToString(), currentItem.Name, parser);
                    var choiceAction = new DevExpress.ExpressApp.Actions.ChoiceActionItem(currentItem.Oid.ToString(), currentItem.Name, data);
                    if (parentItem == null)
                    {
                        //Thêm vào gốc
                        singleChoiceAction.Items.Add(choiceAction);
                    }
                    else
                    {
                        //Thêm vào cành
                        //parentItem.Items.Add(choiceAction); 
                        if (singleChoiceAction.ItemType == DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsMode)
                        {
                            prefix += "    ";
                            choiceAction.Caption = prefix + choiceAction.Caption;
                            singleChoiceAction.Items.Add(choiceAction);
                        }
                        else
                            parentItem.Items.Add(choiceAction);

                    }
                    var lowerFolder = allFolders?.Where(x => x.UpperFolder != null && x.UpperFolder?.Oid == currentItem.Oid);
                    //foreach (var child in currentItem.LowerFolder.OrderBy(m => m.Order))
                    //Dùng lazy load
                    foreach (var child in lowerFolder)
                    {
                        CreateTreeSource(singleChoiceAction, choiceAction, child, data, prefix);
                    }
                }
                else if (parentItem != null)
                {
                    singleChoiceAction.Items.Remove(foundItem);
                    if (singleChoiceAction.ItemType == DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsMode)
                    {
                        foundItem.Caption = prefix + foundItem.Caption;
                        singleChoiceAction.Items.Add(foundItem);
                    }
                    else
                        parentItem.Items.Add(foundItem);

                }
            }
        }
        private DevExpress.ExpressApp.Model.IModelChoiceActionItem _savedChoiceActionItem;
        private DevExpress.ExpressApp.Model.IModelChoiceActionItem SavedChoiceActionItem(ViewController viewController, DevExpress.ExpressApp.Actions.SingleChoiceAction singleChoiceAction)
        {
            if (_savedChoiceActionItem == null)
            {
                if (viewController.Application.Model.ActionDesign.Actions[singleChoiceAction.Id]
                        .ChoiceActionItems.GetNode(viewController.View.Id) == null)
                {
                    _savedChoiceActionItem = viewController.Application.Model.ActionDesign.Actions[singleChoiceAction.Id]
                        .ChoiceActionItems.AddNode<DevExpress.ExpressApp.Model.IModelChoiceActionItem>(viewController.View.Id);
                }
                else
                {
                    _savedChoiceActionItem = viewController.Application.Model.ActionDesign.Actions[singleChoiceAction.Id]
                        .ChoiceActionItems[viewController.View.Id];
                }
            }
            return _savedChoiceActionItem;
        }



        #endregion SourceCode4513ImportCode

        #region SourceCode4551ImportCode
        
        public DevExpress.Data.Filtering.GroupOperator GetMemberFolderCriteria(Module.BusinessObjects.Folder folder)
        {
            if (folder == null) return null;

            // Tạo điều kiện cho Oid của folder cha
            var criteriaList = new System.Collections.Generic.List<DevExpress.Data.Filtering.CriteriaOperator>
     {
         new DevExpress.Data.Filtering.BinaryOperator("MemberFolder.Oid", folder.Oid)
     };

            // Đệ quy cho các folder con
            foreach (var childFolder in folder.LowerFolder)
            {
                var childCriteria = GetMemberFolderCriteria(childFolder);
                if (childCriteria != null)
                {
                    criteriaList.Add(childCriteria);
                }
            }

            // Kết hợp các điều kiện thành GroupOperator
            return new DevExpress.Data.Filtering.GroupOperator(DevExpress.Data.Filtering.GroupOperatorType.Or, criteriaList.ToArray());
        }


        public void UpdateCodeFolderAndChild(Folder folder, SortProperty sortProperty, ref int total, ref int change)
        {
            UpdateParentCode(folder);
            if (folder.Order != null)
            {
                folder.Code = folder.GetDefaultCode();
            }
            if (folder.LowerFolder != null && folder.LowerFolder.Count > 0)
            {
                DevExpress.Xpo.SortingCollection sortCollection = new DevExpress.Xpo.SortingCollection();
                sortCollection.Add(sortProperty);
                folder.LowerFolder.Sorting = sortCollection;
                foreach (var childFolder in folder.LowerFolder)
                    UpdateCodeFolderAndChild(childFolder, sortProperty, ref total, ref change);
            }
            //if (LowerFolder != null && LowerFolder.Count > 0 && Order != null)
            //{
            //    LowerFolder.Sorting.Clear();
            //    LowerFolder.Sorting.Add(sortProperty);
            //    XPCollection<Folder> xpCollection2 = LowerFolder;               
            //    var lastOrder = xpCollection2[0].Order.Value;               
            //    var countString = lastOrder.ToString("D");
            //    foreach (var childFolder in xpCollection2)
            //    {
            //        total++;                      
            //        var indexCode = Order.Value.ToString("D");
            //        for (int i = indexCode.Length; i < countString.Length; i++)
            //            indexCode = "0" + indexCode;
            //        if (!string.IsNullOrEmpty(Code))
            //            indexCode = Code + indexCode;
            //        if(!indexCode.Equals(childFolder.Code))
            //        {
            //            childFolder.Code = indexCode;
            //            change++;
            //        }
            //        childFolder.UpdateCodeFolderAndChild(sortProperty, ref total, ref change);                  
            //    }
            //}            
        }

        protected void UpdateParentCode(Folder folder)
        {
            if (string.IsNullOrEmpty(folder.Code))
            {
                if (folder.UpperFolder != null)
                    UpdateParentCode(folder.UpperFolder);
                folder.Code = folder.GetDefaultCode();
            }
        }

        public void ImportFilesFromMethod(Folder folder, object masterObject, string masterTypeName)
        {
            //102: Folder: Nạp các đường link thuộc Folder và Folder con cháu (popup Folder và nạp bookmark trong vào video)
            //Nạp hết bookmark của folder này
            foreach (var bookMark in folder.BookMarkList)
            {
                var newBookMark = new BookMark(folder.Session);
                if (View is ListView)
                    ((ListView)View).CollectionSource.Add(newBookMark);
                else if (masterTypeName == "Video")
                    newBookMark.SetMemberValue("Video", masterObject);
                else if (masterTypeName == "Folder")
                    newBookMark.SetMemberValue("Folder", masterObject);
                newBookMark.URL = bookMark.URL;
                newBookMark.Name = bookMark.Name;
            }
            //Nạp hết bookmark của con cháu
            foreach (var child in folder.LowerFolder)
                ImportFilesFromMethod(child, masterObject, masterTypeName);
        }

        public void ImportFolderFromMethod(Folder folder, Folder currentFolder, Folder upperFolder)
        {
            //102: Copy thì chỉ cần hiện cây Thư mục để multi Select mà khi chọn 1 thư mục thì sẽ copy cả con cháu
            //
            var newFolder = new Module.BusinessObjects.Folder(folder.Session);
            //bookmark.Video = video;                    
            newFolder.URL = currentFolder.URL;
            newFolder.Name = currentFolder.Name;
            newFolder.FolderType = currentFolder.FolderType;
            newFolder.Member = currentFolder.Member;
            newFolder.Image = currentFolder.Image;
            //if (view is ListView)
            //    ((ListView)view).CollectionSource.Add(newFolder);
            //else if (upperFolder != null)
            //    newFolder.UpperFolder = upperFolder;
            newFolder.UpperFolder = upperFolder;
            foreach (var child in folder.LowerFolder)
                ImportFolderFromMethod(child, child, newFolder);
        }

        public bool ExportFolderComputer(Folder folder, ref int result, ref int total, string parentPath = null)
        {
            total++;
            var currentPath = folder.URL;
            try
            {
                if (string.IsNullOrEmpty(currentPath) && !string.IsNullOrEmpty(parentPath) && !string.IsNullOrEmpty(folder.Name))
                {
                    if (parentPath.Contains("/"))
                        currentPath = parentPath + "/" + folder.Name;
                    else
                        currentPath = parentPath + "\\" + folder.Name;
                }
                if (!string.IsNullOrEmpty(currentPath))
                {
                    if (!System.IO.Directory.Exists(currentPath))
                    {
                        System.IO.Directory.CreateDirectory(currentPath);
                        result++;
                    }
                }
                //Tạo thư mục con
                foreach (var childFolder in folder.LowerFolder)
                    ExportFolderComputer(childFolder, ref result, ref total, currentPath);

            }
            catch (System.Exception ex)
            {
               throw new UserFriendlyException("Không tạo được thư mục: " + currentPath);
                return false;
            }
            return true;
        }


        #endregion SourceCode4551ImportCode

  
  
        #region Base Object Service
	    		//public string ToolTipControllerText(View view, Module.BusinessObjects.Folder folder)
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
		//public object NameToolTipControllerText(View view, Module.BusinessObjects.Folder folder)
        //{
        //    if (Name != null) 
		//			return Name;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object FolderTypeToolTipControllerText(View view, Module.BusinessObjects.Folder folder)
        //{
        //    if (FolderType != null) 
		//			return FolderType;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object UpperFolderToolTipControllerText(View view, Module.BusinessObjects.Folder folder)
        //{
        //    if (UpperFolder != null) 
		//			return UpperFolder;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object OrderToolTipControllerText(View view, Module.BusinessObjects.Folder folder)
        //{
        //    if (Order != null) 
		//			return Order;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object URLToolTipControllerText(View view, Module.BusinessObjects.Folder folder)
        //{
        //    if (URL != null) 
		//			return URL;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ImageToolTipControllerText(View view, Module.BusinessObjects.Folder folder)
        //{
        //    if (Image != null) 
		//			return Image;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object MemberToolTipControllerText(View view, Module.BusinessObjects.Folder folder)
        //{
        //    if (Member != null) 
		//			return Member;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object PermissionPolicyRoleToolTipControllerText(View view, Module.BusinessObjects.Folder folder)
        //{
        //    if (PermissionPolicyRole != null) 
		//			return PermissionPolicyRole;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object OpenToolTipControllerText(View view, Module.BusinessObjects.Folder folder)
        //{
        //    if (Open != null) 
		//			return Open;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ConsumeListToolTipControllerText(View view, Module.BusinessObjects.Folder folder)
        //{
        //    if (ConsumeList != null) 
		//			return ConsumeList;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object PostListToolTipControllerText(View view, Module.BusinessObjects.Folder folder)
        //{
        //    if (PostList != null) 
		//			return PostList;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ProductTypeListToolTipControllerText(View view, Module.BusinessObjects.Folder folder)
        //{
        //    if (ProductTypeList != null) 
		//			return ProductTypeList;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object OrgListToolTipControllerText(View view, Module.BusinessObjects.Folder folder)
        //{
        //    if (OrgList != null) 
		//			return OrgList;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ContactListToolTipControllerText(View view, Module.BusinessObjects.Folder folder)
        //{
        //    if (ContactList != null) 
		//			return ContactList;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object WorkTypeListToolTipControllerText(View view, Module.BusinessObjects.Folder folder)
        //{
        //    if (WorkTypeList != null) 
		//			return WorkTypeList;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object WorkListToolTipControllerText(View view, Module.BusinessObjects.Folder folder)
        //{
        //    if (WorkList != null) 
		//			return WorkList;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object CalendarListToolTipControllerText(View view, Module.BusinessObjects.Folder folder)
        //{
        //    if (CalendarList != null) 
		//			return CalendarList;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ProductListingListToolTipControllerText(View view, Module.BusinessObjects.Folder folder)
        //{
        //    if (ProductListingList != null) 
		//			return ProductListingList;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ContentToolTipControllerText(View view, Module.BusinessObjects.Folder folder)
        //{
        //    if (Content != null) 
		//			return Content;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object LowerFolderToolTipControllerText(View view, Module.BusinessObjects.Folder folder)
        //{
        //    if (LowerFolder != null) 
		//			return LowerFolder;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object AppListToolTipControllerText(View view, Module.BusinessObjects.Folder folder)
        //{
        //    if (AppList != null) 
		//			return AppList;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object LoginAccountListToolTipControllerText(View view, Module.BusinessObjects.Folder folder)
        //{
        //    if (LoginAccountList != null) 
		//			return LoginAccountList;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object VideoListToolTipControllerText(View view, Module.BusinessObjects.Folder folder)
        //{
        //    if (VideoList != null) 
		//			return VideoList;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ProductListToolTipControllerText(View view, Module.BusinessObjects.Folder folder)
        //{
        //    if (ProductList != null) 
		//			return ProductList;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object GroupProductListToolTipControllerText(View view, Module.BusinessObjects.Folder folder)
        //{
        //    if (GroupProductList != null) 
		//			return GroupProductList;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object AssetListToolTipControllerText(View view, Module.BusinessObjects.Folder folder)
        //{
        //    if (AssetList != null) 
		//			return AssetList;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object EquipmentListToolTipControllerText(View view, Module.BusinessObjects.Folder folder)
        //{
        //    if (EquipmentList != null) 
		//			return EquipmentList;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object IntegrationSystemListToolTipControllerText(View view, Module.BusinessObjects.Folder folder)
        //{
        //    if (IntegrationSystemList != null) 
		//			return IntegrationSystemList;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object BookMarkListToolTipControllerText(View view, Module.BusinessObjects.Folder folder)
        //{
        //    if (BookMarkList != null) 
		//			return BookMarkList;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ObjectRelationListToolTipControllerText(View view, Module.BusinessObjects.Folder folder)
        //{
        //    if (ObjectRelationList != null) 
		//			return ObjectRelationList;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object InvesterListToolTipControllerText(View view, Module.BusinessObjects.Folder folder)
        //{
        //    if (InvesterList != null) 
		//			return InvesterList;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object CompanyListToolTipControllerText(View view, Module.BusinessObjects.Folder folder)
        //{
        //    if (CompanyList != null) 
		//			return CompanyList;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ParentToolTipControllerText(View view, Module.BusinessObjects.Folder folder)
        //{
        //    if (Parent != null) 
		//			return Parent;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object UpdateToolTipControllerText(View view, Module.BusinessObjects.Folder folder)
        //{
        //    if (Update != null) 
		//			return Update;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object CreatedDateToolTipControllerText(View view, Module.BusinessObjects.Folder folder)
        //{
        //    if (CreatedDate != null) 
		//			return CreatedDate;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object ChildrenToolTipControllerText(View view, Module.BusinessObjects.Folder folder)
        //{
        //    if (Children != null) 
		//			return Children;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object QuantityToolTipControllerText(View view, Module.BusinessObjects.Folder folder)
        //{
            
        //    return null;
        //}
    

		//Tooltip for Object
		//public object CodeToolTipControllerText(View view, Module.BusinessObjects.Folder folder)
        //{
        //    if (Code != null) 
		//			return Code;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object SystemTypeToolTipControllerText(View view, Module.BusinessObjects.Folder folder)
        //{
        //    if (SystemType != null) 
		//			return SystemType;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object CloseToolTipControllerText(View view, Module.BusinessObjects.Folder folder)
        //{
        //    if (Close != null) 
		//			return Close;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object InActiveToolTipControllerText(View view, Module.BusinessObjects.Folder folder)
        //{
        //    if (InActive != null) 
		//			return InActive;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object MemberFolderToolTipControllerText(View view, Module.BusinessObjects.Folder folder)
        //{
        //    if (MemberFolder != null) 
		//			return MemberFolder;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object FlagToolTipControllerText(View view, Module.BusinessObjects.Folder folder)
        //{
        //    if (Flag != null) 
		//			return Flag;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object Flag2ToolTipControllerText(View view, Module.BusinessObjects.Folder folder)
        //{
        //    if (Flag2 != null) 
		//			return Flag2;
        //    return null;
        //}
    

	    #endregion
  

    }
}
