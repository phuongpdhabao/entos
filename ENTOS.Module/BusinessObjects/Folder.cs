using System;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Drawing;
using System.IO;
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
using ENTOS.Module.SystemObjects;
using ENTOS.Module;
using ENTOS.Domain.Abstractions;
using ENTOS.Module.FilterControllers;


namespace ENTOS.Module.BusinessObjects 
{
	[NavigationItem("Common")] 
	[DefaultClassOptions]
    [ModelDefault("Caption", "Thư mục"), ImageName("Folder")]
    [ModelDefault("DefaultLookupEditorMode", "Auto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
	[Appearance("Folder ObjectRelationList Hide_None__" , TargetItems = "ObjectRelationList" , Criteria = "[FolderType] <> ##ToString#Product# And [FolderType] <> ##ToString#Post#",AppearanceItemType = "ViewItem", Visibility = ViewItemVisibility.Hide )]
	[Appearance("Folder OrgList Hide_None__" , TargetItems = "OrgList" , Criteria = "[FolderType] <> ##ToString#Org# And [FolderType] <> ##ToString#Org#",AppearanceItemType = "ViewItem", Visibility = ViewItemVisibility.Hide )]
	[Appearance("Folder ConsumeList Hide_None__" , TargetItems = "ConsumeList" , Criteria = "[FolderType] <> ##ToString#Consume#",AppearanceItemType = "ViewItem", Visibility = ViewItemVisibility.Hide )]
	[Appearance("Folder ProductTypeList Hide_None__" , TargetItems = "ProductTypeList" , Criteria = "[FolderType] <> ##ToString#ProductType#",AppearanceItemType = "ViewItem", Visibility = ViewItemVisibility.Hide )]
	[Appearance("Folder ProductListingList Hide_None__" , TargetItems = "ProductListingList" , Criteria = "[FolderType] <> ##ToString#ProductListing#",AppearanceItemType = "ViewItem", Visibility = ViewItemVisibility.Hide )]
	[Appearance("Folder WorkTypeList Hide_None__" , TargetItems = "WorkTypeList" , Criteria = "[FolderType] <> ##ToString#WorkType#",AppearanceItemType = "ViewItem", Visibility = ViewItemVisibility.Hide )]
	[Appearance("Folder Image, BookMarkList, Content Hide_None__" , TargetItems = "Image, BookMarkList, Content" , Criteria = "[FolderType] = ##ToString#ProductType# Or [FolderType] = ##ToString#Calendar# Or [FolderType] = ##ToString#Consume# Or [FolderType] = ##ToString#Accounting# Or [FolderType] = ##ToString#Member# Or [FolderType] = ##ToString#SoftwareIcon#",AppearanceItemType = "ViewItem", Visibility = ViewItemVisibility.Hide )]
	[Appearance("Folder ProductList Hide_None__" , TargetItems = "ProductList" , Criteria = "[FolderType] <> ##ToString#Product#",AppearanceItemType = "ViewItem", Visibility = ViewItemVisibility.Hide )]
	[Appearance("Folder LoginAccountList, AppList Hide_None__" , TargetItems = "LoginAccountList, AppList" , Criteria = "[FolderType] <> ##ToString#App#",AppearanceItemType = "ViewItem", Visibility = ViewItemVisibility.Hide )]
	[Appearance("Folder AccountEntryList, AllAccountEntryList, PartyAccountEntryList Hide_None__" , TargetItems = "AccountEntryList, AllAccountEntryList, PartyAccountEntryList" , Criteria = "[FolderType] <> ##ToString#Accounting#",AppearanceItemType = "ViewItem", Visibility = ViewItemVisibility.Hide )]
	[Appearance("Folder CompanyList, InvesterList Hide_None__" , TargetItems = "CompanyList, InvesterList" , Criteria = "[FolderType] <> ##ToString#Company#",AppearanceItemType = "ViewItem", Visibility = ViewItemVisibility.Hide )]
	[Appearance("Folder ContactList Hide_None__" , TargetItems = "ContactList" , Criteria = "[FolderType] <> ##ToString#Contact# And [FolderType] <> ##ToString#Org#",AppearanceItemType = "ViewItem", Visibility = ViewItemVisibility.Hide )]
	[Appearance("Folder VideoList Hide_None__" , TargetItems = "VideoList" , Criteria = "[FolderType] <> ##ToString#Video#",AppearanceItemType = "ViewItem", Visibility = ViewItemVisibility.Hide )]
	[Appearance("Folder PostList Hide_None__" , TargetItems = "PostList" , Criteria = "[FolderType] <> ##ToString#Post#",AppearanceItemType = "ViewItem", Visibility = ViewItemVisibility.Hide )]
	[Appearance("Folder MemberList Hide_None__" , TargetItems = "MemberList" , Criteria = "[FolderType] <> ##ToString#Member#",AppearanceItemType = "ViewItem", Visibility = ViewItemVisibility.Hide )]
	[Appearance("Folder WorkList Hide_None__" , TargetItems = "WorkList" , Criteria = "[FolderType] <> ##ToString#Work#",AppearanceItemType = "ViewItem", Visibility = ViewItemVisibility.Hide )]
	[Appearance("Folder IntegrationSystemList, AssetList, EquipmentList Hide_None__" , TargetItems = "IntegrationSystemList, AssetList, EquipmentList" , Criteria = "[FolderType] <> ##ToString#Asset#",AppearanceItemType = "ViewItem", Visibility = ViewItemVisibility.Hide )]
	[Appearance("Folder CalendarList Hide_None__" , TargetItems = "CalendarList" , Criteria = "[FolderType] <> ##ToString#Calendar#",AppearanceItemType = "ViewItem", Visibility = ViewItemVisibility.Hide )]
	[Appearance("Folder GroupProductList Hide_None__" , TargetItems = "GroupProductList" , Criteria = "[FolderType] <> ##ToString#Product#",AppearanceItemType = "ViewItem", Visibility = ViewItemVisibility.Hide )]
	[Appearance("Folder FolderRelationList Hide_None__" , TargetItems = "FolderRelationList" , Criteria = "[FolderType] <> ##ToString#Work# And [FolderType] <> ##ToString#Product#",AppearanceItemType = "ViewItem", Visibility = ViewItemVisibility.Hide )]
    //[Appearance("Hide Employee", TargetItems = "Birthday, BankAccount", Criteria = "!IsEmployee", Visibility = ViewItemVisibility.Hide)]
    //[Appearance("Is Not Validated " + nameof(xxx), TargetItems = nameof(xxx), Criteria = nameof(xxxIsNotValidate), FontColor = "Red", Context = "DetailView")]	
    //[Appearance("Disable Delete", Criteria = "AppearanceDisableDelete", AppearanceItemType = "Action", TargetItems = "Delete", Enabled = false)] // Hủy vì bị chậm
 
    [Appearance("Hide Non Display", TargetItems = nameof(FolderType)+ "," + nameof(UpperFolder)+ "," + nameof(Order)+ "," + nameof(Image)+ "," + nameof(Member)+ "," + nameof(PermissionPolicyRole)+ "," + nameof(Open)+ "," + nameof(Content)+ "," + nameof(LowerFolder), Criteria = "!Display", Visibility = ViewItemVisibility.Hide, Context = "DetailView")]
 
    [ShowToolTipAttribute(TargetItems = nameof(Quantity))]
 
    [AllowSetDefaultAttribute(TargetItems = nameof(Order)+ "," + nameof(Member)+ "," + nameof(Update)+ "," + nameof(CreatedDate)+ "," + nameof(Quantity)+ "," + nameof(Code)+ "," + nameof(MemberFolder))]
 
	[MobileColumnAttribute(Context = "Folder_ListView", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "Folder_ListView_SoftwareIcon", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "ProductListing_FolderList_ListView", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "Folder_ListView_App", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "Product_FolderList_ListView", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "Folder_ListView_Org", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "Folder_ListView_Document", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "Folder_ListView_Asset", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "Folder_ListView_ProductType", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "Folder_ListView_Contact", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "Folder_ListView_Member", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "Folder_ListView_Communication", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "Folder_ListView_Accounting", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "Folder_ListView_ProductListing", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "Folder_ListView_Product", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "Folder_ListView_Folder", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "Folder_LowerFolder_ListView", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "Folder_ListView_WorkType", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "Folder_ListView_Post", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "Folder_ListView_Work", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "Folder_ListView_Consume", TargetItems = nameof(Name))]
	[MobileColumnAttribute(Context = "Folder_LookupListView", TargetItems = nameof(Name))]
	[DefaultProperty("Name")]
 
	[UpDownTopBottomOrder(Criteria = "[<Folder>][^.UpperFolder = UpperFolder and Oid = ?] or (UpperFolder is null and [<Folder>][UpperFolder is null and Oid = ?])", AscSort = true, ChangeBetweenRow = false, AutoSave = false)]
	[RuleCombinationOfPropertiesIsUnique("UniqueRule.Folder2", DefaultContexts.Save, "UpperFolder, Name, PermissionPolicyRole")]
	[RuleCriteria("Messenger.Folder.Order", DefaultContexts.Save, "ShowWarningFolderFolder", "Phải đặt lại mã cho toàn bộ Thư mục cùng Cấp trên và toàn thể con cháu", SkipNullOrEmptyValues = true, ResultType = ValidationResultType.Warning, InvertResult = true)]
	[RuleCombinationOfPropertiesIsUnique("UniqueRule.Folder", DefaultContexts.Save, "UpperFolder, Code")]
[OptimisticLocking(true)]
    public partial class Folder:  DevExpress.Xpo.XPLiteObject , IWebData, IReOrder, DevExpress.Persistent.Base.General.ITreeNode, IUrlInfo , INoIndexColumn, IOnViewObjectSpaceCommitted, IDisplay      //, HbBaseObject
    {
        public Folder(Session session)
            : base(session) {              
        }

				public string ToolTipControllerText(View view)
        {
            var result = "";
            //if (this.Name != null)
            //{
            //    if (!string.IsNullOrEmpty(result))
            //        result += "\r\n";
            //    result += "Tiêu đề:" + Name;
            //}            
            return result;
        }
		        private System.Collections.Generic.Dictionary<string, bool> _cacheAppearanceDisableDelete;
		[Browsable(false)]
        public bool AppearanceDisableDelete
        {
            get
            {

                if (Session.IsNewObject(this))
                    return false;
				if (ConsumeList.IsLoaded)
                {
                    if (ConsumeList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(ConsumeList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(ConsumeList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.Consume>(CriteriaOperator.Parse("[Folder.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool consumelist = Session.Query<Module.BusinessObjects.Consume>().Where(x => x.Folder.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(ConsumeList), consumelist);
                        if (consumelist)
                            return true;

                    }                    
                }				
				if (PostList.IsLoaded)
                {
                    if (PostList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(PostList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(PostList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.Post>(CriteriaOperator.Parse("[Folder.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool postlist = Session.Query<Module.BusinessObjects.Post>().Where(x => x.Folder.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(PostList), postlist);
                        if (postlist)
                            return true;

                    }                    
                }				
				if (ProductTypeList.IsLoaded)
                {
                    if (ProductTypeList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(ProductTypeList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(ProductTypeList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.ProductType>(CriteriaOperator.Parse("[Folder.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool producttypelist = Session.Query<Module.BusinessObjects.ProductType>().Where(x => x.Folder.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(ProductTypeList), producttypelist);
                        if (producttypelist)
                            return true;

                    }                    
                }				
				if (OrgList.IsLoaded)
                {
                    if (OrgList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(OrgList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(OrgList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.Org>(CriteriaOperator.Parse("[Folder.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool orglist = Session.Query<Module.BusinessObjects.Org>().Where(x => x.Folder.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(OrgList), orglist);
                        if (orglist)
                            return true;

                    }                    
                }				
				if (ContactList.IsLoaded)
                {
                    if (ContactList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(ContactList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(ContactList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.Contact>(CriteriaOperator.Parse("[Folder.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool contactlist = Session.Query<Module.BusinessObjects.Contact>().Where(x => x.Folder.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(ContactList), contactlist);
                        if (contactlist)
                            return true;

                    }                    
                }				
				if (WorkTypeList.IsLoaded)
                {
                    if (WorkTypeList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(WorkTypeList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(WorkTypeList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.WorkType>(CriteriaOperator.Parse("[Folder.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool worktypelist = Session.Query<Module.BusinessObjects.WorkType>().Where(x => x.Folder.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(WorkTypeList), worktypelist);
                        if (worktypelist)
                            return true;

                    }                    
                }				
				if (WorkList.IsLoaded)
                {
                    if (WorkList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(WorkList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(WorkList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.Work>(CriteriaOperator.Parse("[Folder.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool worklist = Session.Query<Module.BusinessObjects.Work>().Where(x => x.Folder.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(WorkList), worklist);
                        if (worklist)
                            return true;

                    }                    
                }				
				if (CalendarList.IsLoaded)
                {
                    if (CalendarList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(CalendarList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(CalendarList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.Calendar>(CriteriaOperator.Parse("[Folder.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool calendarlist = Session.Query<Module.BusinessObjects.Calendar>().Where(x => x.Folder.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(CalendarList), calendarlist);
                        if (calendarlist)
                            return true;

                    }                    
                }				
				if (LowerFolder.IsLoaded)
                {
                    if (LowerFolder.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(LowerFolder)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(LowerFolder)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.Folder>(CriteriaOperator.Parse("[UpperFolder.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool lowerfolder = Session.Query<Module.BusinessObjects.Folder>().Where(x => x.UpperFolder.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(LowerFolder), lowerfolder);
                        if (lowerfolder)
                            return true;

                    }                    
                }				
				if (AppList.IsLoaded)
                {
                    if (AppList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(AppList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(AppList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.App>(CriteriaOperator.Parse("[Folder.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool applist = Session.Query<Module.BusinessObjects.App>().Where(x => x.Folder.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(AppList), applist);
                        if (applist)
                            return true;

                    }                    
                }				
				if (LoginAccountList.IsLoaded)
                {
                    if (LoginAccountList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(LoginAccountList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(LoginAccountList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.LoginAccount>(CriteriaOperator.Parse("[Folder.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool loginaccountlist = Session.Query<Module.BusinessObjects.LoginAccount>().Where(x => x.Folder.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(LoginAccountList), loginaccountlist);
                        if (loginaccountlist)
                            return true;

                    }                    
                }				
				if (VideoList.IsLoaded)
                {
                    if (VideoList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(VideoList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(VideoList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.Video>(CriteriaOperator.Parse("[Folder.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool videolist = Session.Query<Module.BusinessObjects.Video>().Where(x => x.Folder.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(VideoList), videolist);
                        if (videolist)
                            return true;

                    }                    
                }				
				if (GroupProductList.IsLoaded)
                {
                    if (GroupProductList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(GroupProductList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(GroupProductList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.Product>(CriteriaOperator.Parse("[Folder.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool groupproductlist = Session.Query<Module.BusinessObjects.Product>().Where(x => x.Folder.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(GroupProductList), groupproductlist);
                        if (groupproductlist)
                            return true;

                    }                    
                }				
				if (AssetList.IsLoaded)
                {
                    if (AssetList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(AssetList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(AssetList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.Asset>(CriteriaOperator.Parse("[Folder.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool assetlist = Session.Query<Module.BusinessObjects.Asset>().Where(x => x.Folder.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(AssetList), assetlist);
                        if (assetlist)
                            return true;

                    }                    
                }				
				if (EquipmentList.IsLoaded)
                {
                    if (EquipmentList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(EquipmentList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(EquipmentList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.Equipment>(CriteriaOperator.Parse("[Folder.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool equipmentlist = Session.Query<Module.BusinessObjects.Equipment>().Where(x => x.Folder.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(EquipmentList), equipmentlist);
                        if (equipmentlist)
                            return true;

                    }                    
                }				
				if (IntegrationSystemList.IsLoaded)
                {
                    if (IntegrationSystemList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(IntegrationSystemList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(IntegrationSystemList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.IntegrationSystem>(CriteriaOperator.Parse("[Folder.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool integrationsystemlist = Session.Query<Module.BusinessObjects.IntegrationSystem>().Where(x => x.Folder.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(IntegrationSystemList), integrationsystemlist);
                        if (integrationsystemlist)
                            return true;

                    }                    
                }				
				if (BookMarkList.IsLoaded)
                {
                    if (BookMarkList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(BookMarkList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(BookMarkList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.BookMark>(CriteriaOperator.Parse("[Folder.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool bookmarklist = Session.Query<Module.BusinessObjects.BookMark>().Where(x => x.Folder.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(BookMarkList), bookmarklist);
                        if (bookmarklist)
                            return true;

                    }                    
                }				
				if (ObjectRelationList.IsLoaded)
                {
                    if (ObjectRelationList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(ObjectRelationList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(ObjectRelationList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.ObjectRelation>(CriteriaOperator.Parse("[Folder.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool objectrelationlist = Session.Query<Module.BusinessObjects.ObjectRelation>().Where(x => x.Folder.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(ObjectRelationList), objectrelationlist);
                        if (objectrelationlist)
                            return true;

                    }                    
                }				
				if (InvesterList.IsLoaded)
                {
                    if (InvesterList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(InvesterList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(InvesterList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.Invester>(CriteriaOperator.Parse("[Folder.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool investerlist = Session.Query<Module.BusinessObjects.Invester>().Where(x => x.Folder.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(InvesterList), investerlist);
                        if (investerlist)
                            return true;

                    }                    
                }				
				if (CompanyList.IsLoaded)
                {
                    if (CompanyList.Any())
                            return true;
                }else
                {
                    if (_cacheAppearanceDisableDelete is null)
                        _cacheAppearanceDisableDelete = new Dictionary<string, bool>();
                    if (_cacheAppearanceDisableDelete.ContainsKey(nameof(CompanyList)))
                    {
                        if (_cacheAppearanceDisableDelete[nameof(CompanyList)])
                            return true;
                    }
                    else
                    {
                        //if (Session.FindObject<Module.BusinessObjects.Company>(CriteriaOperator.Parse("[Folder.Oid] = ?", Oid)) != null) //Loại này sẽ lấy cá tham chiếu
                        bool companylist = Session.Query<Module.BusinessObjects.Company>().Where(x => x.Folder.Oid == Oid).Take(1).Any(); //Tạo truy vấn top 1
                        _cacheAppearanceDisableDelete.Add(nameof(CompanyList), companylist);
                        if (companylist)
                            return true;

                    }                    
                }				
                                
                return false;
            }
        }

        public void OnViewObjectSpaceCommitted(View view)
        {

           
        }
//Kết thúc khổi phải là đối tượng NonPersistent (không lưu CSDL)

		[Key(true)]
		[VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]     
        public Guid Oid { get; set; }
               

		//private string _name;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Tên")]
        [ToolTip("Tên")]
		//[Index(0)]		

 		[Size(250)]
		public string Name
        { 
		    get => GetPropertyValue<string>("Name");                         
			set => SetPropertyValue<string>("Name", value); 
			
        }
		//Tooltip for Object
		public object NameToolTipControllerText(View view)
        {
        //    if (Name != null) 
		//			return Name;
            return null;
        }
		//Get Default Value
        public string GetDefaultName(View view = null)
        { 
			return Name;
        }
		//Set Default Value
		public void SetDefaultName(View view = null)
        {
            //if (Name is null){
            //    var result = GetDefaultName(view);
            //    if (result != null && result != Name){
			//          Name = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool NameIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultName();
				//if (result != null && Name != null){
				//	return !Name.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private SoftwareObjectType _foldertype;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
		[DevExpress.Xpo.DisplayName("Loại")]
        [ToolTip("Loại")]
		//[Index(1)]		
	    [Indexed()]
		public SoftwareObjectType FolderType
        { 
		    get => GetPropertyValue<SoftwareObjectType>("FolderType");                         
			set => SetPropertyValue<SoftwareObjectType>("FolderType", value); 
			
        }
		//Tooltip for Object
		public object FolderTypeToolTipControllerText(View view)
        {
        //    if (FolderType != null) 
		//			return FolderType;
            return null;
        }
		//Get Default Value
        public SoftwareObjectType GetDefaultFolderType(View view = null)
        { 
			return FolderType;
        }
		//Set Default Value
		public void SetDefaultFolderType(View view = null)
        {
            //if (FolderType is null){
            //    var result = GetDefaultFolderType(view);
            //    if (result != null && result != FolderType){
			//          FolderType = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool FolderTypeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultFolderType();
				//if (result != null && FolderType != null){
				//	return !FolderType.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Folder _upperfolder;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cấp trên")]
        [ToolTip("Cấp trên")]
		//[Index(2)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(UpperFolderCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		[DevExpress.Xpo.Association("UpperFolder-LowerFolder")]
	 
		public Module.BusinessObjects.Folder UpperFolder
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Folder>("UpperFolder");                         
			set => SetPropertyValue<Module.BusinessObjects.Folder>("UpperFolder", value); 
			
        }
		//Tooltip for Object
		public object UpperFolderToolTipControllerText(View view)
        {
        //    if (UpperFolder != null) 
		//			return UpperFolder;
            return null;
        }
		//Get Default Value
        public Module.BusinessObjects.Folder GetDefaultUpperFolder(View view = null)
        { 
			return UpperFolder;
        }
		//Set Default Value
		public void SetDefaultUpperFolder(View view = null)
        {
            //if (UpperFolder is null){
            //    var result = GetDefaultUpperFolder(view);
            //    if (result != null && result != UpperFolder){
			//          UpperFolder = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool UpperFolderIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultUpperFolder();
				//if (result != null && UpperFolder != null){
				//	return !UpperFolder.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator UpperFolderCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(UpperFolder));
            }
        }
	
       
		//private int? _order;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Left, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thứ tự")]
        [ToolTip("Thứ tự")]
		//[Index(3)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n0")]
		public int? Order
        { 
		    get => GetPropertyValue<int?>("Order");                         
			set => SetPropertyValue<int?>("Order", value); 
			
        }
		//Tooltip for Object
		public object OrderToolTipControllerText(View view)
        {
        //    if (Order != null) 
		//			return Order;
            return null;
        }
		//Get Default Value
		//Set Default Value

		//Check Not Validate
		protected bool OrderIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultOrder();
				//if (result != null && Order != null){
				//	return !Order.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _url;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Đường dẫn")]
        [ToolTip("Đường dẫn")]
		//[Index(4)]		

 		[Size(1000)]
	    [ModelDefault("RowCount","1")]
	    [EditorAlias("FileBrowserPropertyEditor")]
		public string URL
        { 
		    get => GetPropertyValue<string>("URL");                         
			set => SetPropertyValue<string>("URL", value); 
			
        }
		//Tooltip for Object
		public object URLToolTipControllerText(View view)
        {
        //    if (URL != null) 
		//			return URL;
            return null;
        }
		//Get Default Value
        public string GetDefaultURL(View view = null)
        { 
			return URL;
        }
		//Set Default Value
		public void SetDefaultURL(View view = null)
        {
            //if (URL is null){
            //    var result = GetDefaultURL(view);
            //    if (result != null && result != URL){
			//          URL = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool URLIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultURL();
				//if (result != null && URL != null){
				//	return !URL.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private byte[] _image;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Biểu tượng")]
        [ToolTip("Biểu tượng")]
		//[Index(5)]		
		[Appearance("Biểu tượngBackground", BackColor = "Transparent")]
	
        [ImageEditor(ListViewImageEditorCustomHeight = 24,DetailViewImageEditorMode = ImageEditorMode.DropDownPictureEdit, DetailViewImageEditorFixedHeight = 64)] 
	
		public byte[] Image
        { 
		    get => GetPropertyValue<byte[]>("Image");                         
			set => SetPropertyValue<byte[]>("Image", value); 
			
        }
		//Tooltip for Object
		public object ImageToolTipControllerText(View view)
        {
        //    if (Image != null) 
		//			return Image;
            return null;
        }
		//Get Default Value
        public byte[] GetDefaultImage(View view = null)
        { 
			return Image;
        }
		//Set Default Value
		public void SetDefaultImage(View view = null)
        {
            //if (Image is null){
            //    var result = GetDefaultImage(view);
            //    if (result != null && result != Image){
			//          Image = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ImageIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultImage();
				//if (result != null && Image != null){
				//	return !Image.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Member _member;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Quản lý")]
        [ToolTip("Quản lý")]
		//[Index(6)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(MemberCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public Module.BusinessObjects.Member Member
        { 
		    get => GetPropertyValue<Module.BusinessObjects.Member>("Member");                         
			set => SetPropertyValue<Module.BusinessObjects.Member>("Member", value); 
			
        }
		//Tooltip for Object
		public object MemberToolTipControllerText(View view)
        {
        //    if (Member != null) 
		//			return Member;
            return null;
        }
		//Get Default Value
		//Set Default Value

		//Check Not Validate
		protected bool MemberIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultMember();
				//if (result != null && Member != null){
				//	return !Member.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator MemberCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(Member));
            }
        }
	
       
		//private DevExpress.Persistent.BaseImpl.PermissionPolicy.PermissionPolicyRole _permissionpolicyrole;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Nhóm")]
        [ToolTip("Nhóm")]
		//[Index(7)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(PermissionPolicyRoleCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
		public DevExpress.Persistent.BaseImpl.PermissionPolicy.PermissionPolicyRole PermissionPolicyRole
        { 
		    get => GetPropertyValue<DevExpress.Persistent.BaseImpl.PermissionPolicy.PermissionPolicyRole>("PermissionPolicyRole");                         
			set => SetPropertyValue<DevExpress.Persistent.BaseImpl.PermissionPolicy.PermissionPolicyRole>("PermissionPolicyRole", value); 
			
        }
		//Tooltip for Object
		public object PermissionPolicyRoleToolTipControllerText(View view)
        {
        //    if (PermissionPolicyRole != null) 
		//			return PermissionPolicyRole;
            return null;
        }
		//Get Default Value
        public DevExpress.Persistent.BaseImpl.PermissionPolicy.PermissionPolicyRole GetDefaultPermissionPolicyRole(View view = null)
        { 
			return PermissionPolicyRole;
        }
		//Set Default Value
		public void SetDefaultPermissionPolicyRole(View view = null)
        {
            //if (PermissionPolicyRole is null){
            //    var result = GetDefaultPermissionPolicyRole(view);
            //    if (result != null && result != PermissionPolicyRole){
			//          PermissionPolicyRole = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool PermissionPolicyRoleIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultPermissionPolicyRole();
				//if (result != null && PermissionPolicyRole != null){
				//	return !PermissionPolicyRole.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator PermissionPolicyRoleCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(PermissionPolicyRole));
            }
        }
	
       
		//private bool? _open;
		[DetailViewLayoutAttribute(LayoutColumnPosition.Right, " ", 1)]
 
		[VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Công khai")]
        [ToolTip("Công khai")]
		//[Index(8)]		
		public bool? Open
        { 
		    get => GetPropertyValue<bool?>("Open");                         
			set => SetPropertyValue<bool?>("Open", value); 
			
        }
		//Tooltip for Object
		public object OpenToolTipControllerText(View view)
        {
        //    if (Open != null) 
		//			return Open;
            return null;
        }
		//Get Default Value
        public bool? GetDefaultOpen(View view = null)
        { 
			return Open;
        }
		//Set Default Value
		public void SetDefaultOpen(View view = null)
        {
            //if (Open is null){
            //    var result = GetDefaultOpen(view);
            //    if (result != null && result != Open){
			//          Open = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool OpenIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultOpen();
				//if (result != null && Open != null){
				//	return !Open.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tiêu dùng")]
		//[Index(9)]
		[DevExpress.Xpo.Association("Folder-ConsumeList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.Consume> ConsumeList
        {      
		    get => GetCollection<Module.BusinessObjects.Consume>("ConsumeList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Bài viết")]
		//[Index(10)]
		[DevExpress.Xpo.Association("Folder-PostList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.Post> PostList
        {      
		    get => GetCollection<Module.BusinessObjects.Post>("PostList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Loại sản phẩm")]
		//[Index(11)]
		[DevExpress.Xpo.Association("Folder-ProductTypeList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.ProductType> ProductTypeList
        {      
		    get => GetCollection<Module.BusinessObjects.ProductType>("ProductTypeList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tổ chức")]
		//[Index(12)]
		[DevExpress.Xpo.Association("Folder-OrgList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.Org> OrgList
        {      
		    get => GetCollection<Module.BusinessObjects.Org>("OrgList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Liên hệ")]
		//[Index(13)]
		[DevExpress.Xpo.Association("Folder-ContactList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.Contact> ContactList
        {      
		    get => GetCollection<Module.BusinessObjects.Contact>("ContactList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Loại công việc")]
		//[Index(14)]
		[DevExpress.Xpo.Association("Folder-WorkTypeList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.WorkType> WorkTypeList
        {      
		    get => GetCollection<Module.BusinessObjects.WorkType>("WorkTypeList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Công việc")]
		//[Index(15)]
		[DevExpress.Xpo.Association("Folder-WorkList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.Work> WorkList
        {      
		    get => GetCollection<Module.BusinessObjects.Work>("WorkList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Lịch")]
		//[Index(16)]
		[DevExpress.Xpo.Association("Folder-CalendarList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.Calendar> CalendarList
        {      
		    get => GetCollection<Module.BusinessObjects.Calendar>("CalendarList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Niêm yết")]
		//[Index(17)]
		[DataSourceCriteria("Not FolderList[Oid = '@This.Oid']")]
		[DevExpress.Xpo.Association("FolderList-ProductListingList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.ProductListing> ProductListingList
        {      
		    get => GetCollection<Module.BusinessObjects.ProductListing>("ProductListingList"); 
			
        }
       
		//private string _content;
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
		//[EditorAlias(EditorAliases.RichTextPropertyEditor)][ModelDefault("DocumentStorageFormat", "Html")]//[EditorAlias(EditorAliases.HtmlPropertyEditor)]
		//[Delayed]
 
		[VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Nội dung")]
        [ToolTip("Nội dung")]
		//[Index(18)]		

 		[Size(SizeAttribute.Unlimited)]
	    [ModelDefault("DocumentStorageFormat", "Html")]
	    [EditorAlias(EditorAliases.RichTextPropertyEditor)]
		public string Content
        { 
		    get => GetPropertyValue<string>("Content");                         
			set => SetPropertyValue<string>("Content", value); 
			
        }
		//Tooltip for Object
		public object ContentToolTipControllerText(View view)
        {
        //    if (Content != null) 
		//			return Content;
            return null;
        }
		//Get Default Value
        public string GetDefaultContent(View view = null)
        { 
			return Content;
        }
		//Set Default Value
		public void SetDefaultContent(View view = null)
        {
            //if (Content is null){
            //    var result = GetDefaultContent(view);
            //    if (result != null && result != Content){
			//          Content = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ContentIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultContent();
				//if (result != null && Content != null){
				//	return !Content.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cấp dưới")]
		//[Index(19)]
		[DevExpress.Xpo.Association("UpperFolder-LowerFolder")]
	    [RuleCombinationOfPropertiesIsUnique("UniqueRule.Folder3", DefaultContexts.Save, "Order")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.Folder> LowerFolder
        {      
		    get => GetCollection<Module.BusinessObjects.Folder>("LowerFolder"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ứng dụng")]
		//[Index(20)]
		[DevExpress.Xpo.Association("Folder-AppList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.App> AppList
        {      
		    get => GetCollection<Module.BusinessObjects.App>("AppList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Đăng nhập")]
		//[Index(21)]
		[DevExpress.Xpo.Association("Folder-LoginAccountList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.LoginAccount> LoginAccountList
        {      
		    get => GetCollection<Module.BusinessObjects.LoginAccount>("LoginAccountList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tư liệu")]
		//[Index(22)]
		[DevExpress.Xpo.Association("Folder-VideoList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.Video> VideoList
        {      
		    get => GetCollection<Module.BusinessObjects.Video>("VideoList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	

	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Sản phẩm n-n")]
		//[Index(23)]
		[DataSourceCriteria("Not FolderList[Oid = '@This.Oid']")]
		[DevExpress.Xpo.Association("FolderList-ProductList")]
	    [VisibleInDetailView(false)]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.Product> ProductList
        {      
		    get => GetCollection<Module.BusinessObjects.Product>("ProductList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Sản phẩm")]
		//[Index(24)]
		[DevExpress.Xpo.Association("Folder-GroupProductList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.Product> GroupProductList
        {      
		    get => GetCollection<Module.BusinessObjects.Product>("GroupProductList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tài sản")]
		//[Index(25)]
		[DevExpress.Xpo.Association("Folder-AssetList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.Asset> AssetList
        {      
		    get => GetCollection<Module.BusinessObjects.Asset>("AssetList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Thiết bị")]
		//[Index(26)]
		[DevExpress.Xpo.Association("Folder-EquipmentList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.Equipment> EquipmentList
        {      
		    get => GetCollection<Module.BusinessObjects.Equipment>("EquipmentList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Hệ thống")]
		//[Index(27)]
		[DevExpress.Xpo.Association("Folder-IntegrationSystemList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.IntegrationSystem> IntegrationSystemList
        {      
		    get => GetCollection<Module.BusinessObjects.IntegrationSystem>("IntegrationSystemList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Liên kết")]
		//[Index(28)]
		[DevExpress.Xpo.Association("Folder-BookMarkList")]
	    [DevExpress.Xpo.Aggregated()]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.BookMark> BookMarkList
        {      
		    get => GetCollection<Module.BusinessObjects.BookMark>("BookMarkList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Quan hệ")]
		//[Index(29)]
		[DevExpress.Xpo.Association("Folder-ObjectRelationList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.ObjectRelation> ObjectRelationList
        {      
		    get => GetCollection<Module.BusinessObjects.ObjectRelation>("ObjectRelationList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Nhà đầu tư")]
		//[Index(30)]
		[DevExpress.Xpo.Association("Folder-InvesterList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.Invester> InvesterList
        {      
		    get => GetCollection<Module.BusinessObjects.Invester>("InvesterList"); 
			
        }
       
		[DetailViewLayoutAttribute("Tab", LayoutGroupType.TabbedGroup, 5)]
	
        [VisibleInDetailView(true)]
	
        [VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Công ty")]
		//[Index(31)]
		[DevExpress.Xpo.Association("Folder-CompanyList")]
		//[CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
		public XPCollection <Module.BusinessObjects.Company> CompanyList
        {      
		    get => GetCollection<Module.BusinessObjects.Company>("CompanyList"); 
			
        }
       
		//private DevExpress.Persistent.Base.General.ITreeNode _parent;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Parent")]
        [ToolTip("Parent")]
		//[Index(33)]		
	    [Browsable(false)]
		public DevExpress.Persistent.Base.General.ITreeNode Parent
        { 
		    #region 0277ImportCode 
get => UpperFolder;
#endregion 0277ImportCode
			
        }
		//Tooltip for Object
		public object ParentToolTipControllerText(View view)
        {
        //    if (Parent != null) 
		//			return Parent;
            return null;
        }
		//Get Default Value
        public DevExpress.Persistent.Base.General.ITreeNode GetDefaultParent(View view = null)
        { 
			return Parent;
        }
		//Set Default Value
		public void SetDefaultParent(View view = null)
        {
            //if (Parent is null){
            //    var result = GetDefaultParent(view);
            //    if (result != null && result != Parent){
			//          Parent = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ParentIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultParent();
				//if (result != null && Parent != null){
				//	return !Parent.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private DateTime? _update;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cập nhật")]
        [ToolTip("Cập nhật")]
		//[Index(34)]		
		[ModelDefault("DisplayFormat", "d/M/yyyy")]
		[ModelDefault("EditMask", "d/M/yyyy")]
	    [ModelDefault("AllowEdit", "False")]
		public DateTime? Update
        { 
		    get => GetPropertyValue<DateTime?>("Update");                         
			set => SetPropertyValue<DateTime?>("Update", value); 
			
        }
		//Tooltip for Object
		public object UpdateToolTipControllerText(View view)
        {
        //    if (Update != null) 
		//			return Update;
            return null;
        }
		//Get Default Value
		//Set Default Value

		//Check Not Validate
		protected bool UpdateIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultUpdate();
				//if (result != null && Update != null){
				//	return !Update.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private DateTime? _createddate;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ngày")]
        [ToolTip("Ngày")]
		//[Index(35)]		
		[ModelDefault("DisplayFormat", "d/M/yyyy")]
		[ModelDefault("EditMask", "d/M/yyyy H:mm")]
	    [ModelDefault("AllowEdit", "False")]
		public DateTime? CreatedDate
        { 
		    get => GetPropertyValue<DateTime?>("CreatedDate");                         
			set => SetPropertyValue<DateTime?>("CreatedDate", value); 
			
        }
		//Tooltip for Object
		public object CreatedDateToolTipControllerText(View view)
        {
        //    if (CreatedDate != null) 
		//			return CreatedDate;
            return null;
        }
		//Get Default Value
		//Set Default Value

		//Check Not Validate
		protected bool CreatedDateIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCreatedDate();
				//if (result != null && CreatedDate != null){
				//	return !CreatedDate.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private System.ComponentModel.IBindingList _children;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Children")]
        [ToolTip("Children")]
		//[Index(36)]		
	    [Browsable(false)]
		public System.ComponentModel.IBindingList Children
        { 
		    #region 0303ImportCode 
get => LowerFolder;
#endregion 0303ImportCode
			
        }
		//Tooltip for Object
		public object ChildrenToolTipControllerText(View view)
        {
        //    if (Children != null) 
		//			return Children;
            return null;
        }
		//Get Default Value
        public System.ComponentModel.IBindingList GetDefaultChildren(View view = null)
        { 
			return Children;
        }
		//Set Default Value
		public void SetDefaultChildren(View view = null)
        {
            //if (Children is null){
            //    var result = GetDefaultChildren(view);
            //    if (result != null && result != Children){
			//          Children = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool ChildrenIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultChildren();
				//if (result != null && Children != null){
				//	return !Children.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private decimal? _quantity;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Số lượng")]
        [ToolTip("Số lượng")]
		//[Index(37)]		
		[ModelDefault("DisplayFormat", "{0:n0}")]
		[ModelDefault("EditMask", "n2")]
	    [NonPersistent()]
	    [NotMapped()]
		public decimal? Quantity
        { 
		    #region 1563ImportCode 
get;set; //Tạo mã GetSet để không bắt sự kiện onchange, tránh lỗi xung đột sửa nhiều session
#endregion 1563ImportCode
			
        }
		//Tooltip for Object
		public object QuantityToolTipControllerText(View view)
        {
            #region 1095ImportCode 
var defaultQuantity = GetDefaultQuantity();
var result = string.Format("{0:n2}", defaultQuantity);
if(defaultQuantity != Quantity)
{
    result = "<color=red>" + result + "</color>";
}            
return result;
#endregion 1095ImportCode
            return null;
        }
		//Get Default Value
		//Set Default Value

		//Check Not Validate
		protected bool QuantityIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultQuantity();
				//if (result != null && Quantity != null){
				//	return !Quantity.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private string _code;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Mã")]
        [ToolTip("Mã")]
		//[Index(38)]		

 		[Size(20)]
		public string Code
        { 
		    get => GetPropertyValue<string>("Code");                         
			set => SetPropertyValue<string>("Code", value); 
			
        }
		//Tooltip for Object
		public object CodeToolTipControllerText(View view)
        {
        //    if (Code != null) 
		//			return Code;
            return null;
        }
		//Get Default Value
		//Set Default Value

		//Check Not Validate
		protected bool CodeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultCode();
				//if (result != null && Code != null){
				//	return !Code.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private System.Type _systemtype;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Kiểu hệ thống")]
        [ToolTip("Kiểu hệ thống")]
		//[Index(39)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteriaProperty(nameof(SystemTypeCriteria))]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
	    [TypeConverter(typeof(DevExpress.Persistent.Base.Security.SecurityTargetTypeConverter))]
	    [ValueConverter(typeof(DevExpress.ExpressApp.Utils.TypeToStringConverter))]
		public System.Type SystemType
        { 
		    get => GetPropertyValue<System.Type>("SystemType");                         
			set => SetPropertyValue<System.Type>("SystemType", value); 
			
        }
		//Tooltip for Object
		public object SystemTypeToolTipControllerText(View view)
        {
        //    if (SystemType != null) 
		//			return SystemType;
            return null;
        }
		//Get Default Value
        public System.Type GetDefaultSystemType(View view = null)
        { 
			return SystemType;
        }
		//Set Default Value
		public void SetDefaultSystemType(View view = null)
        {
            //if (SystemType is null){
            //    var result = GetDefaultSystemType(view);
            //    if (result != null && result != SystemType){
			//          SystemType = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool SystemTypeIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultSystemType();
				//if (result != null && SystemType != null){
				//	return !SystemType.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator SystemTypeCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(SystemType));
            }
        }
	
       
		//private bool _close;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Riêng tư")]
        [ToolTip("Riêng tư")]
		//[Index(40)]		
		public bool Close
        { 
		    get => GetPropertyValue<bool>("Close");                         
			set => SetPropertyValue<bool>("Close", value); 
			
        }
		//Tooltip for Object
		public object CloseToolTipControllerText(View view)
        {
        //    if (Close != null) 
		//			return Close;
            return null;
        }
		//Get Default Value
        public bool GetDefaultClose(View view = null)
        { 
			return Close;
        }
		//Set Default Value
		public void SetDefaultClose(View view = null)
        {
            //if (Close is null){
            //    var result = GetDefaultClose(view);
            //    if (result != null && result != Close){
			//          Close = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool CloseIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultClose();
				//if (result != null && Close != null){
				//	return !Close.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _inactive;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Ngừng")]
        [ToolTip("Ngừng")]
		//[Index(41)]		
		public bool InActive
        { 
		    get => GetPropertyValue<bool>("InActive");                         
			set => SetPropertyValue<bool>("InActive", value); 
			
        }
		//Tooltip for Object
		public object InActiveToolTipControllerText(View view)
        {
        //    if (InActive != null) 
		//			return InActive;
            return null;
        }
		//Get Default Value
        public bool GetDefaultInActive(View view = null)
        { 
			return InActive;
        }
		//Set Default Value
		public void SetDefaultInActive(View view = null)
        {
            //if (InActive is null){
            //    var result = GetDefaultInActive(view);
            //    if (result != null && result != InActive){
			//          InActive = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool InActiveIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultInActive();
				//if (result != null && InActive != null){
				//	return !InActive.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private Module.BusinessObjects.Folder _memberfolder;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Tập thể")]
        [ToolTip("Tập thể")]
		//[Index(42)]		
		[LookupEditorMode(LookupEditorMode.Auto)]
		//[ModelDefault("LookupProperty", "")]
		[DataSourceCriteria("[FolderType] = ##ToString#Member# And [InActive] = False")]
		//[DataSourceProperty("",DataSourcePropertyIsNullMode.SelectAll)] 
		//[ModelDefault("PropertyEditorType", "TooltipLookupPropertyEditor")]
		//[DevExpress.Xpo.Association]
		//[NoForeignKey]
	 
	    [NotMapped()]
	    [ImmediatePostData()]
	    [NonPersistent()]
		public Module.BusinessObjects.Folder MemberFolder
        { 
		    #region 1564ImportCode 
get;set;//Chống xung đột Session của hàm onload
#endregion 1564ImportCode
			
        }
		//Tooltip for Object
		public object MemberFolderToolTipControllerText(View view)
        {
        //    if (MemberFolder != null) 
		//			return MemberFolder;
            return null;
        }
		//Get Default Value
		//Set Default Value

		//Check Not Validate
		protected bool MemberFolderIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultMemberFolder();
				//if (result != null && MemberFolder != null){
				//	return !MemberFolder.Equals(result);
				//} 
   
                return false;
            }
        }

		private CriteriaOperator MemberFolderCriteria
        {
            get
            {
                return Module.Helpers.XafXpoHelper.GetCriteriaOperator(this.GetType(), nameof(MemberFolder));
            }
        }
	
       
		//private bool _flag;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cờ")]
        [ToolTip("Cờ")]
		//[Index(43)]		
	    [NotMapped()]
	    [NonPersistent()]
		public bool Flag
        { 
		    get => GetPropertyValue<bool>("Flag");                         
			set => SetPropertyValue<bool>("Flag", value); 
			
        }
		//Tooltip for Object
		public object FlagToolTipControllerText(View view)
        {
        //    if (Flag != null) 
		//			return Flag;
            return null;
        }
		//Get Default Value
        public bool GetDefaultFlag(View view = null)
        { 
			return Flag;
        }
		//Set Default Value
		public void SetDefaultFlag(View view = null)
        {
            //if (Flag is null){
            //    var result = GetDefaultFlag(view);
            //    if (result != null && result != Flag){
			//          Flag = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool FlagIsNotValidate
        {
            get
            {
                
				//var result = GetDefaultFlag();
				//if (result != null && Flag != null){
				//	return !Flag.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
		//private bool _flag2;
		//[Browsable(false)] 
 
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		[DevExpress.Xpo.DisplayName("Cờ 2")]
        [ToolTip("Cờ 2")]
		//[Index(44)]		
		public bool Flag2
        { 
		    get => GetPropertyValue<bool>("Flag2");                         
			set => SetPropertyValue<bool>("Flag2", value); 
			
        }
		//Tooltip for Object
		public object Flag2ToolTipControllerText(View view)
        {
        //    if (Flag2 != null) 
		//			return Flag2;
            return null;
        }
		//Get Default Value
        public bool GetDefaultFlag2(View view = null)
        { 
			return Flag2;
        }
		//Set Default Value
		public void SetDefaultFlag2(View view = null)
        {
            //if (Flag2 is null){
            //    var result = GetDefaultFlag2(view);
            //    if (result != null && result != Flag2){
			//          Flag2 = result;
            //	  }
            //}
        }

		//Check Not Validate
		protected bool Flag2IsNotValidate
        {
            get
            {
                
				//var result = GetDefaultFlag2();
				//if (result != null && Flag2 != null){
				//	return !Flag2.Equals(result);
				//} 
   
                return false;
            }
        }

	
       
        private bool _display;
        [Browsable(false)]
        [NonPersistent]
        [ImmediatePostData]
        public bool Display
        {
            get { return _display; }
            set { SetPropertyValue("Display", ref _display, value); }
        }
 


		public override void AfterConstruction()
        {
            Oid = Guid.NewGuid();
 
            #region 1076ImportCode
            base.AfterConstruction();
Open = null;
SetDefaultMember();
SetDefaultCreatedDate();
            #endregion 1076ImportCode
            Display = true;
 
        //SetDefaultName(View view = null);
        //SetDefaultFolderType(View view = null);
        //SetDefaultUpperFolder(View view = null);
        //SetDefaultOrder(View view = null);
        //SetDefaultURL(View view = null);
        //SetDefaultImage(View view = null);
        //SetDefaultMember(View view = null);
        //SetDefaultPermissionPolicyRole(View view = null);
        //SetDefaultOpen(View view = null);
        //SetDefaultParent(View view = null);
        //SetDefaultUpdate(View view = null);
        //SetDefaultCreatedDate(View view = null);
        //SetDefaultChildren(View view = null);
        //SetDefaultQuantity(View view = null);
        //SetDefaultCode(View view = null);
        //SetDefaultSystemType(View view = null);
        //SetDefaultClose(View view = null);
        //SetDefaultInActive(View view = null);
        //SetDefaultMemberFolder(View view = null);
        //SetDefaultFlag(View view = null);
        //SetDefaultFlag2(View view = null);
			
        }
        
        protected override void OnLoading()
        {
            base.OnLoading();
        }
        
        protected override void OnLoaded()
        {
            base.OnLoaded();
        }

        private bool alreadySaving = false;        
        protected override void OnSaving()
        {
            #region 1067ImportCode
            base.OnSaving();
SetDefaultUpdate();
            #endregion 1067ImportCode
//            Update = (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
    		if (!(Session is NestedUnitOfWork)&& (Session.DataLayer != null))
            {
   //             if (Session.ObjectLayer is DevExpress.ExpressApp.Security.ClientServer.SecuredSessionObjectLayer)
   //             {
   //                 //Khi đang mở Object
   //             }
   //             else if ((Session.ObjectLayer is DevExpress.Xpo.SimpleObjectLayer))
   //             {
   //                 //Từ popup form con về form chính
   //             }
             }
        }
        
        protected override void OnSaved()
        {
             base.OnSaved();
        }

        protected override void OnDeleting()
        {
             base.OnDeleting();
  
        }

        protected override void OnDeleted()
        {
             base.OnDeleted();
            
        }

		protected override void OnChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnChanged(propertyName, oldValue, newValue);
            if (!IsLoading && !IsSaving)
            {

                switch (propertyName)
                {       
				
                    case nameof(UpperFolder):
                        OnChangedUpperFolder(oldValue, newValue);
                        break;
				
                    case nameof(Name):
                        OnChangedName(oldValue, newValue);
                        break;
				
                    case nameof(URL):
                        OnChangedURL(oldValue, newValue);
                        break;
				
                    case nameof(MemberFolder):
                        OnChangedMemberFolder(oldValue, newValue);
                        break;
 						
                }
                  
            }
        }

        private void OnChangedUpperFolder(object oldValue, object newValue)
        {
            #region 0334ImportCode
            if (newValue is null) return;
SetDefaultOrder();
FolderType = UpperFolder.FolderType;
PermissionPolicyRole = UpperFolder.PermissionPolicyRole;
MemberFolder = UpperFolder.MemberFolder;
Open = UpperFolder.Open;
Close = UpperFolder.Close;
SystemType = UpperFolder.SystemType;
if (newValue is Folder folder)
{
    if (folder.UpperFolder != null)
        FolderType = folder.FolderType;
}            
            #endregion 0334ImportCode
        }               
        private void OnChangedName(object oldValue, object newValue)
        {
            #region 1370ImportCode
                                if (string.IsNullOrEmpty(Name))
                        return;
                    var newName = System.Web.HttpUtility.HtmlDecode(Name);
                    //Xử lý ký tự đặc biệt mã ASCII 160 giống dấu cách
                    newName = newName.Replace(" ", " "); 
                    if (newName != Name)
                        Name = newName;
            
            #endregion 1370ImportCode
        }               
        private void OnChangedURL(object oldValue, object newValue)
        {
            #region 1371ImportCode
                                if (string.IsNullOrEmpty(URL)) return;
                    var newUrl = System.Web.HttpUtility.HtmlDecode(URL);
                    if (newUrl != URL)
                        URL = newUrl;
                    SetDefaultName();
            
            #endregion 1371ImportCode
        }               
        private void OnChangedMemberFolder(object oldValue, object newValue)
        {
            #region 1538ImportCode
            if (newValue is null) return;
SetDefaultQuantity();            
            #endregion 1538ImportCode
        }               
   


		//protected override XPCollection<T> CreateCollection<T>(DevExpress.Xpo.Metadata.XPMemberInfo property)
        //{
        //    var collection = base.CreateCollection<T>(property);
        //    collection.ListChanged += OnItemListChanged;
        //    return collection;
        //}

        //private void OnItemListChanged(object sender, ListChangedEventArgs e)
        //{            
            //if (e.ListChangedType == ListChangedType.ItemAdded)
            //{
			//	SetDefaultConsumeList();
			//	SetDefaultPostList();
			//	SetDefaultProductTypeList();
			//	SetDefaultOrgList();
			//	SetDefaultContactList();
			//	SetDefaultWorkTypeList();
			//	SetDefaultWorkList();
			//	SetDefaultCalendarList();
			//	SetDefaultProductListingList();
			//	SetDefaultContent();
			//	SetDefaultLowerFolder();
			//	SetDefaultAppList();
			//	SetDefaultLoginAccountList();
			//	SetDefaultVideoList();
			//	SetDefaultProductList();
			//	SetDefaultGroupProductList();
			//	SetDefaultAssetList();
			//	SetDefaultEquipmentList();
			//	SetDefaultIntegrationSystemList();
			//	SetDefaultBookMarkList();
			//	SetDefaultObjectRelationList();
			//	SetDefaultInvesterList();
			//	SetDefaultCompanyList();
			//	SetDefaultSoftwareIconList();
            //}
            //else if (e.ListChangedType == ListChangedType.ItemDeleted)
            //{
            //    
            //}
        //}
//Kết thúc khổi phải là đối tượng NonPersistent (không lưu CSDL)
        #region Các phương thức code gen từ Software Task
#region 1110ImportCode
		public string GetDefaultCode(View view = null)
        {
            //Code: 1110            Oid: 5ff83860-eaed-4f09-953e-5e33d2d0ddf7
                        string result = "";
            var sort = new DevExpress.Xpo.SortProperty(nameof(Order), DevExpress.Xpo.DB.SortingDirection.Descending);
            var criteria = DevExpress.Data.Filtering.CriteriaOperator.Parse("UpperFolder is null");
            int count = 0;
            Folder lasted = null;
            if (UpperFolder != null)
            {
                result = UpperFolder.Code;
                criteria = DevExpress.Data.Filtering.CriteriaOperator.Parse("UpperFolder.Oid = ?", UpperFolder.Oid);
                count = UpperFolder.LowerFolder.Count;
                lasted = UpperFolder.LowerFolder.OrderByDescending(x => x?.Order).FirstOrDefault();
            }
            else
            {
                count = Convert.ToInt32(Session.Evaluate<Folder>(CriteriaOperator.Parse("Count()"), criteria));
                //Tìm ra số thứ tự lớn nhất
                lasted = Module.Helpers.XafXpoHelper.GetLastedBySort(Session, this.GetType(), criteria, sort) as Folder;
                var lastedTransaction = Module.Helpers.XafXpoHelper.GetLastedBySort(Session, this.GetType(), criteria, sort, true) as Folder;
                if (lasted is null)
                    lasted = lastedTransaction;
                else if (lastedTransaction != null && lasted.Order < lastedTransaction.Order)
                    lasted = lastedTransaction;
                else if (lastedTransaction != null && !string.IsNullOrEmpty(lastedTransaction.Code) &&
                    !string.IsNullOrEmpty(lasted.Code) && lasted.Code.CompareTo(lastedTransaction.Code) < 0)
                    lasted = lastedTransaction;
            }

            if (lasted != null && lasted.Order != null && lasted.Order >= count)
                count = lasted.Order.Value;
            if (Order != null)
            {
                var countString = count.ToString("D");
                var orderString = Order.Value.ToString("D");
                for (int i = orderString.Length; i < countString.Length; i++)
                    result += "0";
                result += orderString;
            }
            else
            {
                if (lasted is null || string.IsNullOrEmpty(lasted.Code))
                {
                    var countString = count.ToString("D");
                    for (int i = 0; i < countString.Length - 1; i++)
                        result += "0";
                    result += "1";
                }
                else
                {
                    //Lấy số không ở đầu
                    foreach (var c in lasted.Code)
                    {
                        if (c.Equals('0'))
                            result += "0";
                        else
                            break;
                    }
                    var number = Convert.ToInt64(lasted.Code);
                    number++;
                    result = number.ToString("D");

                }
            }

            return result;
        }
#endregion 1110ImportCode
#region 1075ImportCode
		public void SetDefaultMember(View view = null)
        {
            //Code: 1075            Oid: 628eab1d-492e-40d7-be90-00a0bfa3b020
            if(Member == null) Member = GetDefaultMember();
        }
#endregion 1075ImportCode
#region 1111ImportCode
		public void SetDefaultCode(View view = null)
        {
            //Code: 1111            Oid: 54df7743-29af-4bba-a705-1b381e970860
            if (string.IsNullOrEmpty(Code))
{
    var result = GetDefaultCode();
    if (result != null && result != Code)
    {
        Code = result;
    }
}
        }
#endregion 1111ImportCode
#region 1089ImportCode
		public void SetDefaultQuantity(View view = null)
        {
            //Code: 1089            Oid: 4bfc8b87-4764-43b6-b834-679ef61cd93e
              if (BookMarkList.Count > 0)
  {
      var quantity = GetDefaultQuantity();
      if (Quantity != quantity)
          Quantity = quantity;
  }
        }
#endregion 1089ImportCode
#region 1068ImportCode
		public DateTime? GetDefaultUpdate(View view = null)
        {
            //Code: 1068            Oid: c5bb4fa9-78a1-4c0c-9815-08ef98f12bef
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 1068ImportCode
#region 1088ImportCode
		public decimal? GetDefaultQuantity(View view = null)
        {
            //Code: 1088            Oid: 92958384-7030-40b3-991e-06094dc85ed3
                            //Code: 1336            Oid: 92958384-7030-40b3-991e-06094dc85ed3
                decimal total = 0;
                if (BookMarkList != null)
                    total += BookMarkList.Count;
                foreach (var childFolder in LowerFolder)
                {
                    var childFolderQuantity = childFolder.GetDefaultQuantity();
                    if (childFolderQuantity != null)
                        total += childFolderQuantity.Value;
                }
                if (total > 0)
                    return total;
                return null;
            
        


        }
#endregion 1088ImportCode
#region 0322ImportCode
		public int? GetDefaultOrder(View view = null)
        {
            //Code: 0322            Oid: 2de6efe7-4f4f-444b-9eb4-4158bbe4701d
            if (UpperFolder != null && UpperFolder.LowerFolder != null)
{
    var lasted = UpperFolder.LowerFolder.Where(m => m.Order != null).OrderByDescending(m => m.Order).FirstOrDefault();
    if (lasted != null)
        return lasted.Order + 1;
    return 1;
}
return null;
        }
#endregion 0322ImportCode
#region 1077ImportCode
		public Module.BusinessObjects.Member GetDefaultMember(View view = null)
        {
            //Code: 1077            Oid: ee4fc3a4-3c4a-4b39-b901-eff6ae43e6f7
            return Module.Helpers.XafXpoHelper.GetCurrentUser<Member>(Session);
        }
#endregion 1077ImportCode
#region 1368ImportCode
		public DateTime? GetDefaultCreatedDate(View view = null)
        {
            //Code: 1368            Oid: 4c59f4b9-9e90-438d-8319-eee99215c9d3
            return (DateTime)Session.Evaluate(typeof(XPObjectType), new FunctionOperator(FunctionOperatorType.Now), null);
        }
#endregion 1368ImportCode
#region 1369ImportCode
		public void SetDefaultCreatedDate(View view = null)
        {
            //Code: 1369            Oid: 59305813-54c2-4d9b-a3c6-061cec78583c
            if(CreatedDate == null) CreatedDate = GetDefaultCreatedDate();
        }
#endregion 1369ImportCode
#region 1066ImportCode
		public void SetDefaultUpdate(View view = null)
        {
            //Code: 1066            Oid: dbf70a98-7000-4b69-83dd-addd66d1f765
            Update = GetDefaultUpdate();
        }
#endregion 1066ImportCode
#region 0323ImportCode
		public void SetDefaultOrder(View view = null)
        {
            //Code: 0323            Oid: 47c6a978-a138-4035-9595-683e536c290b
            Order= GetDefaultOrder();
        }
#endregion 0323ImportCode
        #endregion
//Mã nguồn bổ sung
#region FolderImportCode

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
		
		
        public void UpdateCodeFolderAndChild(SortProperty sortProperty,ref int total, ref int change)
        {
            UpdateParentCode();
            if(Order != null)
            {
                Code = GetDefaultCode();
            }
            if (LowerFolder != null && LowerFolder.Count > 0)
            {
                DevExpress.Xpo.SortingCollection sortCollection = new DevExpress.Xpo.SortingCollection();
                sortCollection.Add(sortProperty);
                LowerFolder.Sorting = sortCollection;
                foreach(var childFolder in LowerFolder)
                    childFolder.UpdateCodeFolderAndChild(sortProperty, ref total, ref change);
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

        protected void UpdateParentCode()
        {
            if (string.IsNullOrEmpty(Code))
            {
                if (UpperFolder != null)
                    UpperFolder.UpdateParentCode();
                Code = GetDefaultCode();
            }
        }

        public void ImportFilesFromMethod(XafApplication xafApplication, View view, object masterObject, string masterTypeName)
        {
            //102: Folder: Nạp các đường link thuộc Folder và Folder con cháu (popup Folder và nạp bookmark trong vào video)
            //Nạp hết bookmark của folder này
            foreach (var bookMark in BookMarkList)
            {
                var newBookMark = new BookMark(Session);
                if (view is ListView)
                    ((ListView)view).CollectionSource.Add(newBookMark);
                else if (masterTypeName == "Video")
                    newBookMark.SetMemberValue("Video", masterObject);
                else if (masterTypeName == "Folder")
                    newBookMark.SetMemberValue("Folder", masterObject);
                newBookMark.URL = bookMark.URL;
                newBookMark.Name = bookMark.Name;
            }
            //Nạp hết bookmark của con cháu
            foreach (var child in LowerFolder)
                child.ImportFilesFromMethod(xafApplication, view, masterObject, masterTypeName);
        }

        public void ImportFolderFromMethod(XafApplication xafApplication, View view, Folder currentFolder, Folder upperFolder)
        {
            //102: Copy thì chỉ cần hiện cây Thư mục để multi Select mà khi chọn 1 thư mục thì sẽ copy cả con cháu
            //
            var newFolder = new Module.BusinessObjects.Folder(Session);
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
            foreach (var child in LowerFolder)
                child.ImportFolderFromMethod(xafApplication, view, child, newFolder);
        }

                protected bool ShowWarningFolderFolder
        {
            get
            {
                if (Session.IsNewObject(this) && Order != null && (Order % 10) == 0)
                    return true;                
                return false;
            }
        }

     			
		



#endregion FolderImportCode
		 		 
    }
}
