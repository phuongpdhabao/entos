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

    public partial class WebsiteService : BaseService
    {

        public WebsiteService() : base()
        {
        }
        #region DependencyInjection
        private IClipboardService clipboardService;
        protected IClipboardService _clipboardService => clipboardService ??= Application.ServiceProvider.GetRequiredService<IClipboardService>();        
        private IProcessManagementService processManagementService;
        protected IProcessManagementService _processManagementService => processManagementService ??= Application.ServiceProvider.GetRequiredService<IProcessManagementService>();        
        private IMySqlManagementService mySqlManagementService;
        protected IMySqlManagementService _mySqlManagementService => mySqlManagementService ??= Application.ServiceProvider.GetRequiredService<IMySqlManagementService>();        
        private IUrlService urlService;
        protected IUrlService _urlService => urlService ??= Application.ServiceProvider.GetRequiredService<IUrlService>();        
  
  
        #endregion DependencyInjection

        public WebsiteService(ViewController viewController) : base(viewController)
        {

        }
        //public event EventHandler<string>? OnError;
        //public event EventHandler<string>? OnSuccess;
        //public event EventHandler<string>? OnResponse; 
        
        #region SourceCode4510ImportCode
                        public void FolderToWordpress(DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventArgs e, Website currentWebsite)
        {
            var wordPressClient = GetWordPressClient(currentWebsite, e.SelectedChoiceActionItem.Id.Contains("product"));
            if (wordPressClient is null)
                return;
            replaceAll = false;
            WooCommerceNET.RestAPI rest = new WooCommerceNET.RestAPI($"{currentWebsite.URL}/wp-json/wc/v3/", currentWebsite.WooCommerceUser, currentWebsite.WooCommerceKey);
            WooCommerceNET.WooCommerce.v3.WCObject wcObject = new WooCommerceNET.WooCommerce.v3.WCObject(rest);

            if (currentWebsite.Folder is null)
                return;
            int count = 1;
            int menu_other_id = 0;
            int? menuid = null;
            int maxCategory = 10;
            var categoryList = e.SelectedChoiceActionItem.Id.Contains("category") ? wordPressClient.Categories.GetAllAsync().Result?.ToList() : null;
            var productCategoryList = e.SelectedChoiceActionItem.Id.Contains("product_cat") ? GetProductCategoryList(wcObject) : null;
            var pageList = e.SelectedChoiceActionItem.Id.Contains("page") ? wordPressClient.Pages.GetAllAsync().Result?.ToList() : null;
            if (e.SelectedChoiceActionItem.Id.Contains("menu"))
            {
                maxCategory = Module.Helpers.ParameterHelper.GetIntOrDefault(View.ObjectSpace, "WordpressMaxCategoriesDisplay", 5);
                var otherCategoryCaption = Module.Helpers.ParameterHelper.GetValueOrDefault(View.ObjectSpace, "ExportOtherCategoryCaption", "Loại khác");
                var mainMenu = GetMainMenu(currentWebsite);
                if (mainMenu is null)
                {
                    Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Không tìm thấy menu chính", InformationType.Error);
                    return;
                }
                if (e.SelectedChoiceActionItem.Id.Equals("Edit_menu"))
                {
                    string url = currentWebsite.URL;
                    if (!url.EndsWith("/"))
                        url += "/";
                    url += "wp-admin/nav-menus.php?action=edit&menu=" + mainMenu.Id.ToString("D");
                    _urlService.OpenUrl(url);
                
                    return;
                }
                menuid = mainMenu.Id;
                var menuItems = GetMenuItem(currentWebsite, menuid.Value);
                if (menuItems is null)
                    return;
                foreach (Newtonsoft.Json.Linq.JObject menuItem in menuItems)
                {
                    var menuTitle = menuItem.GetValue("title") as Newtonsoft.Json.Linq.JObject;
                    if (menuTitle != null)
                    {
                        var menuCaption = menuTitle.GetValue("rendered")?.ToString();
                        if (!string.IsNullOrEmpty(menuCaption) && menuCaption == otherCategoryCaption)
                        {
                            var parent = System.Convert.ToInt32(menuItem.GetValue("parent"));
                            if (parent == 0)
                            {
                                menu_other_id = System.Convert.ToInt32(menuItem.GetValue("id"));
                                break;
                            }
                            //var object_id = menuItem.GetValue("object_id");
                        }
                    }
                }
                //Tạo menu Other Id khi số thư mục của menu vượt quá maxCategory
                if (menu_other_id == 0 && currentWebsite.Folder.LowerFolder.Count > maxCategory + 1)
                {
                    //Trường hợp trang
                    WordPressPCL.Models.MediaItem mediaItem = null;
                    int otherId = 0;
                    //Tạo loại khác
                    if (e.SelectedChoiceActionItem.Id.Contains("category"))
                    {
                        //chuyên mục tin tức
                        var existedCategory = categoryList.FirstOrDefault(m => m.Name == otherCategoryCaption && m.Parent == 0);
                        if (existedCategory is null)
                        {
                            var newCategory = new WordPressPCL.Models.Category(otherCategoryCaption);
                            newCategory.Taxonomy = "category";
                            var resultCategory = wordPressClient.Categories.CreateAsync(newCategory).Result;
                            if (resultCategory != null)
                            {
                                categoryList.Add(resultCategory);
                                otherId = resultCategory.Id;
                            }
                        }
                        else
                        {
                            otherId = existedCategory.Id;
                        }
                    }
                    else if (e.SelectedChoiceActionItem.Id.Contains("product_cat"))
                    {
                        //Danh mục sản phẩm                                                     
                        var existCategory = productCategoryList.FirstOrDefault(m => m.name == otherCategoryCaption && (m.parent is null || m.parent == 0));
                        if (existCategory is null)
                        {
                            var newCategory = new WooCommerceNET.WooCommerce.v3.ProductCategory();
                            newCategory.name = otherCategoryCaption;
                            mediaItem = ImportProductCategoryImage(wordPressClient, currentWebsite.Folder, newCategory);
                            var resultCategory = wcObject.Category.Add(newCategory).Result;
                            if (resultCategory != null)
                            {
                                productCategoryList.Add(resultCategory);
                                otherId = Convert.ToInt32(resultCategory.id);
                            }
                        }
                        else
                        {
                            otherId = System.Convert.ToInt32(existCategory.id);
                        }
                    }
                    else if (e.SelectedChoiceActionItem.Id.Contains("page"))
                    {
                        //Đối với Page thì không tạo loại khác
                        ////var newTitle = new WordPressPCL.Models.Title(otherCategoryCaption);
                        ////var existPage = pageList?.FirstOrDefault(m => m.Title != null && (m.Title.Rendered == otherCategoryCaption || m.Title.Raw == otherCategoryCaption));
                        ////if (existPage is null)
                        ////{
                        ////    var newPage = new WordPressPCL.Models.Page();
                        ////    newPage.Title = new WordPressPCL.Models.Title(otherCategoryCaption);
                        ////    newPage.Date = System.DateTime.Now;
                        ////    var resultPage = wordPressClient.Pages.CreateAsync(newPage).Result;
                        ////    if (resultPage != null)
                        ////    {
                        ////        pageList?.Add(resultPage);
                        ////        otherId = resultPage.Id;
                        ////    }
                        ////}
                        ////else
                        ////{
                        ////    otherId = System.Convert.ToInt32(existPage.Id);
                        ////}
                    }
                    if (!e.SelectedChoiceActionItem.Id.Contains("page"))
                    {
                        //Đối với Page thì không tạo loại khác
                        string objType = "category";
                        if (e.SelectedChoiceActionItem.Id.Contains("product_cat")) objType = "product_cat";
                        if (e.SelectedChoiceActionItem.Id.Contains("page")) objType = "page";
                        var otherMenu = CreateMenu(menuid.Value, otherId, menu_other_id, currentWebsite, otherCategoryCaption, mediaItem, objType);
                        if (otherMenu != null)
                        {
                            var menu_id = otherMenu.GetValue("id");
                            //var object_id = currentMenu.GetValue("object_id");
                            if (menu_id != null)
                            {
                                menu_other_id = System.Convert.ToInt32(menu_id);
                            }
                            if (_menuItemList != null)
                            {
                                _menuItemList.Add(otherMenu);
                            }
                        }
                    }
                }

            }
            int total = currentWebsite.Folder.LowerFolder.Count();
            foreach (var childFolder in currentWebsite.Folder.LowerFolder.OrderBy(x => x.Order))
            {
                Tools.ShowOrCloseDefaultWaitForm((System.Convert.ToDecimal(count) / total).ToString("p0"), " ");
                SyncCategory(wordPressClient, wcObject, e.SelectedChoiceActionItem.Id, currentWebsite, childFolder, categoryList, productCategoryList, pageList, 0, count > maxCategory ? menu_other_id : 0, menuid);
                count++;
            }
            Tools.ShowOrCloseDefaultWaitForm(null, null);
            _menuItemList = null;
            if (e.SelectedChoiceActionItem.Id.Contains("menu") && menuid != null)
            {
                //Update menu tin tức và loại khác
                var menuItems = GetMenuItem(currentWebsite, menuid.Value);
                if (menuItems is null)
                    return;
                if (menu_other_id > 0)
                {
                    //otherMenu.SetP
                    UpdateMenuItem(currentWebsite, menu_other_id, menuItems.Count - 1);
                }
                if (e.SelectedChoiceActionItem.Id.Contains("product_cat"))
                {
                    int otherIndex = menuItems.Count;
                    int index = 0;
                    foreach (Newtonsoft.Json.Linq.JObject menuItem in menuItems)
                    {
                        index++;
                        if (index <= 1)
                            continue;
                        var objectText = menuItem.GetValue("object")?.ToString();
                        if (objectText != "product_cat")
                        {
                            var id = menuItem.GetValue("id");
                            if (id != null)
                            {
                                UpdateMenuItem(currentWebsite, System.Convert.ToInt32(id), otherIndex);
                                otherIndex++;
                            }

                        }
                    }
                }
            }

        }
        private bool? replaceAll = false;

        public WordPressPCL.WordPressClient GetWordPressClient(Website currentWebsite, bool useWooCommerce)
        {
            if (string.IsNullOrEmpty(currentWebsite.URL))
            {
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Chưa có địa chỉ", InformationType.Error);
                return null;
            }
            if (string.IsNullOrEmpty(currentWebsite.WordpressUser))
            {
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Chưa có Wordpress User", InformationType.Error);
                return null;
            }
            if (string.IsNullOrEmpty(currentWebsite.WordpressKey))
            {
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Chưa có Wordpress Key", InformationType.Error);
                return null;
            }
            if (useWooCommerce && string.IsNullOrEmpty(currentWebsite.WooCommerceUser))
            {
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Chưa có WooCommerce User", InformationType.Error);
                return null;
            }
            if (useWooCommerce && string.IsNullOrEmpty(currentWebsite.WooCommerceKey))
            {
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Chưa có WooCommerce Key", InformationType.Error);
                return null;
            }
            var wordPressClient = new WordPressPCL.WordPressClient($"{currentWebsite.URL}/wp-json/");
            wordPressClient.Auth.UseBasicAuth(currentWebsite.WordpressUser, currentWebsite.WordpressKey);
            return wordPressClient;
        }
        private List<WooCommerceNET.WooCommerce.v3.ProductCategory> GetProductCategoryList(WooCommerceNET.WooCommerce.v3.WCObject wc)
        {
            Dictionary<string, string> cDic = new Dictionary<string, string>();
            cDic.Add("per_page", "100");
            var productCategoryList = wc.Category.GetAll(cDic).Result;
            int count = 1;
            while (productCategoryList.Count == count * 100)
            {
                count++;
                if (cDic.ContainsKey("page"))
                    cDic["page"] = count.ToString();
                else
                    cDic.Add("page", count.ToString());
                var result = wc.Category.GetAll(cDic).Result;
                if (result.Count > 0)
                {
                    productCategoryList.AddRange(result);
                }
            }
            return productCategoryList;
        }

        private Newtonsoft.Json.Linq.JObject CreateMenu(int menuId, int objectId, int parentId, Website website, string menuName, WordPressPCL.Models.MediaItem mediaItem, string objType = "category")
        {
            try
            {
                int menuOrder = _menuItemList != null ? _menuItemList.Count : 0;
                menuOrder++;
                string menuType = objType == "page" ? "post_type" : "taxonomy";
                var builder = new System.Text.StringBuilder("{");
                builder.Append("\"title\":{\"rendered\": \"" + menuName + "\"}");
                builder.Append(",\"type\":\"" + menuType + "\"");
                builder.Append(",\"parent\":" + parentId.ToString("D"));
                builder.Append(",\"menu_order\":" + menuOrder.ToString("D"));
                builder.Append(",\"object\":\"" + objType + "\"");
                builder.Append(",\"object_id\":" + objectId.ToString("D"));
                builder.Append(",\"menus\":" + menuId.ToString("D"));
                if (mediaItem != null)
                {
                    builder.Append(",\"meta\":" + GetMetaMenuItemIcon(mediaItem.Id));
                }
                builder.Append("}");
                string content = builder.ToString();
                //string content = string.Format("{\"title\":{\"rendered\": \"{0}\"},\"type\":\"{1}\",\"parent\":{2},\"menu_order\":{3},\"object\":\"{4}\",\"object_id\":{5},\"menus\":{6}}",
                //                                folder.Name, menuType, parentId.ToString("D"), menuOrder.ToString("D"), objType, objectId.ToString("D"), menuId.ToString("D"));
                using var postBody = new System.Net.Http.StringContent(content, System.Text.Encoding.UTF8, "application/json");
                {
                    var resultString = GetResponseString(website, "wp/v2/menu-items", postBody).Result;
                    if (!string.IsNullOrEmpty(resultString))
                    {
                        var result = Newtonsoft.Json.JsonConvert.DeserializeObject(resultString) as Newtonsoft.Json.Linq.JObject;
                        //if (result != null && _menuItemList != null)
                        //    _menuItemList.Add(result);
                        return result;
                    }
                }
            }
            catch (System.Exception)
            {
            }

            return null;
        }

        private WordPressPCL.Models.MediaItem ImportProductCategoryImage(WordPressPCL.WordPressClient wordPressClient, Folder folder, WooCommerceNET.WooCommerce.v3.ProductCategory category)
        {
            if (folder.Image != null)
            {
                var media = UploadImage(wordPressClient, folder.Image, folder.Oid.ToString());
                if (media != null)
                {
                    if (category != null)
                    {
                        category.image = new WooCommerceNET.WooCommerce.v2.ProductCategoryImage();
                        category.image.id = Convert.ToUInt64(media.Id);
                        category.image.src = media.SourceUrl;
                    }
                    return media;
                }
                //string tempPath = System.IO.Path.GetTempPath();
                //string fileName = folder.Oid.ToString();
                //foreach (char c in System.IO.Path.GetInvalidFileNameChars())
                //{
                //    fileName = fileName.Replace(c, '_');
                //}
                //fileName = fileName.Replace("&amp;", "").Replace("&", "").Replace("  ", " ").Replace(",", "");
                //string fullFileName = tempPath;
                //try
                //{
                //    if (Module.Helpers.ImageHelper.IsSvgFile(folder.Image))
                //    {
                //        fileName += ".svg";
                //        fullFileName += fileName;
                //        System.IO.File.WriteAllBytes(fullFileName, folder.Image);
                //    }
                //    else
                //    {
                //        using (var ms = new System.IO.MemoryStream(folder.Image))
                //        {
                //            System.Drawing.Image image = System.Drawing.Image.FromStream(ms);
                //            fileName += Module.Helpers.ImageHelper.GetFileExtension(image.RawFormat);
                //            fullFileName += fileName;
                //            image.Save(fullFileName, image.RawFormat);
                //        }
                //    }

                //    var media = wordPressClient.Media.CreateAsync(fullFileName, fileName).Result;
                //    if (media != null)
                //    {
                //        if (category != null)
                //        {
                //            category.image = new WooCommerceNET.WooCommerce.v2.ProductCategoryImage();
                //            category.image.id = Convert.ToUInt64(media.Id);
                //            category.image.src = media.SourceUrl;
                //        }
                //        return media;
                //    }
                //}
                //catch (System.Exception)
                //{

                //}
            }
            return null;
        }

        public WordPressPCL.Models.MediaItem UploadImage(WordPressPCL.WordPressClient wordPressClient, byte[] imageArray, string name)
        {
            if (imageArray != null)
            {
                string tempPath = System.IO.Path.GetTempPath();
                string fileName = name;
                foreach (char c in System.IO.Path.GetInvalidFileNameChars())
                {
                    fileName = fileName.Replace(c, '_');
                }
                fileName = fileName.Replace("&amp;", "").Replace("&", "").Replace("  ", " ").Replace(",", "");
                string fullFileName = tempPath;
                try
                {
                    if (Module.Helpers.ImageHelper.IsSvgFile(imageArray))
                    {
                        fileName += ".svg";
                        fullFileName += fileName;
                        System.IO.File.WriteAllBytes(fullFileName, imageArray);
                    }
                    else
                    {
                        using (var ms = new System.IO.MemoryStream(imageArray))
                        {
                            System.Drawing.Image image = System.Drawing.Image.FromStream(ms);
                            fileName += Module.Helpers.ImageHelper.GetFileExtension(image.RawFormat);
                            fullFileName += fileName;
                            image.Save(fullFileName, image.RawFormat);
                            //using (var content = new System.Net.Http.StreamContent(ms))
                            //{
                            //    content.Headers.TryAddWithoutValidation("Content-Type", WordPressPCL.Utility.MimeTypeHelper.GetMIMETypeFromExtension(Module.Helpers.ImageHelper.GetFileExtension(image.RawFormat)));

                            //    content.Headers.TryAddWithoutValidation("Content-Disposition", "attachment; filename=" + fileName);
                            //    var result = wordPressClient.CustomRequest.CreateAsync<object, WordPressPCL.Models.MediaItem>("media", content).Result;
                            //}
                        }
                    }

                    return wordPressClient.Media.CreateAsync(fullFileName, fileName).Result;
                }
                catch (System.Exception)
                {

                }
            }
            return null;
        }

        private async void SyncCategory(WordPressPCL.WordPressClient wordPressClient, WooCommerceNET.WooCommerce.v3.WCObject wc,
                string choice, Website currentWebsite, Folder folder,
                System.Collections.Generic.List<WordPressPCL.Models.Category> categoryList,
                System.Collections.Generic.List<WooCommerceNET.WooCommerce.v3.ProductCategory> productCategoryList,
                System.Collections.Generic.List<WordPressPCL.Models.Page> pageList,
                int parent_id = 0, int menu_parent_id = 0, int? menuid = null)
        {
            int currentId = parent_id;
            int currentMenuId = menu_parent_id;
            bool createNew = false;

            WordPressPCL.Models.MediaItem mediaItem = null;
            if (choice.Contains("category"))
            {
                var existedCategory = categoryList?.FirstOrDefault(m => m.Name == folder.Name && m.Parent == parent_id);
                if (existedCategory is null)
                {
                    createNew = true;
                    var newCategory = new WordPressPCL.Models.Category(folder.Name);
                    newCategory.Parent = parent_id;
                    newCategory.Taxonomy = "category";
                    var resultCategory = wordPressClient.Categories.CreateAsync(newCategory).Result;
                    if (resultCategory != null)
                    {
                        categoryList?.Add(resultCategory);
                        currentId = resultCategory.Id;
                    }
                }
                else
                {
                    currentId = existedCategory.Id;
                }
            }
            else if (choice.Contains("product_cat"))
            {
                var parentProductCategoryId = Convert.ToUInt64(parent_id);
                var encodeName = System.Security.SecurityElement.Escape(folder.Name);
                var existCategory = productCategoryList?.FirstOrDefault(m => (m.name == folder.Name || m.name == encodeName) && ((m.parent is null && parent_id == 0) || (m.parent == parentProductCategoryId)));
                if (existCategory is null)
                {
                    createNew = true;
                    var newCategory = new WooCommerceNET.WooCommerce.v3.ProductCategory();
                    newCategory.parent = parentProductCategoryId;
                    newCategory.name = folder.Name;
                    mediaItem = ImportProductCategoryImage(wordPressClient, folder, newCategory);
                    var resultCategory = wc.Category.Add(newCategory).Result;
                    if (resultCategory != null)
                    {
                        productCategoryList?.Add(resultCategory);
                        currentId = Convert.ToInt32(resultCategory.id);
                    }
                }
                else
                {
                    currentId = System.Convert.ToInt32(existCategory.id);
                    if (existCategory.image is null)
                    {
                        mediaItem = ImportProductCategoryImage(wordPressClient, folder, existCategory);
                        if (existCategory.image != null)
                        {
                            var resultCategory = wc.Category.Update(existCategory.id.Value, existCategory).Result;
                        }
                    }
                    else
                    {
                        mediaItem = new WordPressPCL.Models.MediaItem();
                        mediaItem.Id = Convert.ToInt32(existCategory.image.id);
                        mediaItem.SourceUrl = existCategory.image.src;
                    }

                }
            }
            else if (choice.Contains("page"))
            {
                var title = new WordPressPCL.Models.Title(folder.Name);
                var existedPage = pageList?.FirstOrDefault(m => m.Title != null && (m.Title.Rendered == folder.Name || m.Title.Raw == folder.Name || m.Title.Rendered == title.Rendered));
                if (existedPage is null)
                {
                    createNew = true;
                    var newPage = new WordPressPCL.Models.Page();
                    newPage.Title = new WordPressPCL.Models.Title(folder.Name);
                    if (folder.CreatedDate != null)
                        newPage.Date = folder.CreatedDate.Value;
                    else
                        newPage.Date = System.DateTime.Now;
                    if (choice.Contains("_tabs") && folder.LowerFolder?.Count > 0)
                    {
                        AddTabsContent(newPage, folder);
                    }
                    if (!string.IsNullOrEmpty(folder.Content) && (newPage.Content is null || string.IsNullOrEmpty(newPage.Content.Raw)))
                    {
                        newPage.Content = new WordPressPCL.Models.Content(folder.Content);
                    }
                    var resultPage = wordPressClient.Pages.CreateAsync(newPage).Result;
                    if (resultPage != null)
                    {
                        pageList?.Add(resultPage);
                        currentId = resultPage.Id;
                    }
                }
                else
                {
                    if (replaceAll != null && !string.IsNullOrEmpty(folder.Content))
                    {
                        
                        bool? question = true;
                        if (replaceAll != true)
                            question = _userInteractionService.ShowYesNoCancel("Bạn có muốn ghi đè nội dung các trang đã tồn tại không?\r\nCó: Ghi đè tất cả các trang\r\nKhông: Chỉ ghi đè trang này\r\nBỏ quả: Không ghi đè", folder.Name + " đã có");
                        if (question != false)
                        {
                            if (question == true)
                                replaceAll = true;
                            else replaceAll = null;
                            if (choice.Contains("_tabs") && folder.LowerFolder?.Count > 0)
                            {
                                AddTabsContent(existedPage, folder);
                            }
                            else if (!string.IsNullOrEmpty(folder.Content))
                            {
                                existedPage.Content = new WordPressPCL.Models.Content(folder.Content);
                            }
                            var resultPage = wordPressClient.Pages.UpdateAsync(existedPage).Result;
                        }
                    }
                    currentId = existedPage.Id;
                }
            }
            if (choice.Contains("menu"))
            {
                if (menuid is null)
                {
                    var mainMenu = GetMainMenu(currentWebsite);
                    if (mainMenu is null)
                    {
                        Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Không tìm thấy menu chính", InformationType.Error);
                        return;
                    }
                    menuid = mainMenu.Id;
                }

                var menuItems = GetMenuItem(currentWebsite, menuid.Value);
                if (menuItems is null)
                    return;
                Newtonsoft.Json.Linq.JObject currentMenu = null;
                foreach (Newtonsoft.Json.Linq.JObject menuItem in menuItems)
                {
                    var menuTitle = menuItem.GetValue("title") as Newtonsoft.Json.Linq.JObject;
                    if (menuTitle != null)
                    {
                        var menuCaption = menuTitle.GetValue("rendered")?.ToString();
                        if (!string.IsNullOrEmpty(menuCaption) && menuCaption == folder.Name)
                        {
                            var parent = System.Convert.ToInt32(menuItem.GetValue("parent"));
                            if (parent == currentMenuId)
                            {
                                currentMenu = menuItem;
                            }
                            //var object_id = menuItem.GetValue("object_id");
                        }
                    }

                }
                if (currentMenu is null)
                {
                    if (mediaItem is null)
                        mediaItem = ImportProductCategoryImage(wordPressClient, folder, null);
                    string objType = "category";
                    if (choice.Contains("product_cat")) objType = "product_cat";
                    if (choice.Contains("page")) objType = "page";
                    currentMenu = CreateMenu(menuid.Value, currentId, menu_parent_id, currentWebsite, folder.Name, mediaItem, objType);
                }
                if (currentMenu != null)
                {
                    var menu_id = currentMenu.GetValue("id");
                    //var object_id = currentMenu.GetValue("object_id");
                    if (menu_id != null)
                    {
                        currentMenuId = System.Convert.ToInt32(menu_id);
                    }
                    if (_menuItemList != null)
                    {
                        _menuItemList.Add(currentMenu);
                    }
                    var meta = currentMenu.GetValue("meta") as Newtonsoft.Json.Linq.JObject;
                    if (meta != null)
                    {
                        string metaKey = "_menu_item_icon-id";
                        var menu_item_icon_id = meta.GetValue(metaKey);
                        bool addImage = false;
                        if (menu_item_icon_id is null)
                            addImage = true;
                        else if (replaceAll != null)
                        {
                            var menu_item_icon_id_text = Tools.GetNumberInText(menu_item_icon_id.ToString());
                            if (menu_item_icon_id_text != null)
                            {
                                var currentMediaItem = wordPressClient.Media.GetByIDAsync(menu_item_icon_id_text.Value).Result;
                                if (!CheckEqualMedia(folder.Image, currentMediaItem))
                                    addImage = true;
                            }
                        }
                        if (addImage)
                        {
                            //Upload ảnh
                            if (mediaItem is null)
                                mediaItem = ImportProductCategoryImage(wordPressClient, folder, null);
                            if (mediaItem != null)
                            {
                                string metaValue = GetMetaMenuItemIcon(mediaItem.Id);
                                UpdateMenuItem(currentWebsite, currentMenuId, null, metaValue);
                            }
                        }
                    }
                    //var metaType = meta.GetType();
                }
            }
            //Trường hợp nội dung có tab thì không làm cấu trúc cây
            if (!choice.Contains("_tabs"))
                foreach (var childFolder in folder.LowerFolder.OrderBy(x => x.Order))
                    SyncCategory(wordPressClient, wc, choice, currentWebsite, childFolder, categoryList, productCategoryList, pageList, currentId, currentMenuId, menuid);
        }
        private string tabConfig = "],\"tabPosition\":\"flex\",\"tabPadding\":{\"desktop\":{\"top\":\"8px\",\"right\":\"30px\",\"bottom\":\"8px\",\"left\":\"30px\"}},\"tabBorder\":{\"normal\":{\"border\":{\"color\":\"#272a411a\",\"style\":\"solid\",\"width\":\"1px\"}},\"hover\":{\"border\":{\"style\":\"solid\"}},\"active\":{\"border\":{\"top\":{\"style\":\"solid\",\"color\":\"#272a411a\",\"width\":\"1px\"},\"right\":{\"style\":\"solid\",\"color\":\"#272a411a\",\"width\":\"1px\"},\"bottom\":{\"style\":\"solid\",\"color\":\"#272a411a\"},\"left\":{\"style\":\"solid\",\"color\":\"#272a411a\",\"width\":\"1px\"}}}},\"tabSpacing\":\"0px\",\"tabAfterGap\":\"0px\",\"tabColors\":{\"normal\":{\"text\":\"#272A41\"},\"active\":{\"text\":\"#0DA88C\"},\"hover\":{\"text\":\"#0DA88C\"}},\"tabIcon\":false,\"tabIconPosition\":\"top\",\"tabIconSize\":32,\"tabContainerBorder\":{\"normal\":{\"border\":{\"top\":{\"color\":\"#272a411a\",\"style\":\"solid\"},\"right\":{\"color\":\"#272a411a\",\"style\":\"solid\",\"width\":\"1px\"},\"bottom\":{\"color\":\"#272a411a\",\"style\":\"solid\",\"width\":\"1px\"},\"left\":{\"color\":\"#272a411a\",\"style\":\"solid\",\"width\":\"1px\"}}},\"hover\":{\"border\":{\"color\":\"#9d9d9d\",\"style\":\"solid\",\"width\":\"1px\"}}},\"blockStyles\":{\"\\u002d\\u002dgutena\\u002d\\u002dtabs-tab-min-width\":\"40px\",\"\\u002d\\u002dgutena\\u002d\\u002dtabs-tab-spacing\":\"0px\",\"\\u002d\\u002dgutena\\u002d\\u002dtabs-tab-after-gap\":\"0px\",\"\\u002d\\u002dgutena\\u002d\\u002dtabs-tab-font-size\":\"16px\",\"\\u002d\\u002dgutena\\u002d\\u002dtabs-tab-padding\":\"8px 30px 8px 30px\",\"\\u002d\\u002dgutena\\u002d\\u002dtabs-tab-border-top\":\"1px solid #272a411a\",\"\\u002d\\u002dgutena\\u002d\\u002dtabs-tab-border-right\":\"1px solid #272a411a\",\"\\u002d\\u002dgutena\\u002d\\u002dtabs-tab-border-bottom\":\"1px solid #272a411a\",\"\\u002d\\u002dgutena\\u002d\\u002dtabs-tab-border-left\":\"1px solid #272a411a\",\"\\u002d\\u002dgutena\\u002d\\u002dtabs-tab-active-border-top\":\"1px solid #272a411a\",\"\\u002d\\u002dgutena\\u002d\\u002dtabs-tab-active-border-right\":\"1px solid #272a411a\",\"\\u002d\\u002dgutena\\u002d\\u002dtabs-tab-active-border-left\":\"1px solid #272a411a\",\"\\u002d\\u002dgutena\\u002d\\u002dtabs-tab-text-color\":\"#272A41\",\"\\u002d\\u002dgutena\\u002d\\u002dtabs-tab-hover-text-color\":\"#0DA88C\",\"\\u002d\\u002dgutena\\u002d\\u002dtabs-tab-active-text-color\":\"#0DA88C\",\"\\u002d\\u002dgutena\\u002d\\u002dtabs-tab-icon-spacing\":5,\"\\u002d\\u002dgutena\\u002d\\u002dtabs-tab-container-padding\":\"20px 20px 20px 20px\",\"\\u002d\\u002dgutena\\u002d\\u002dtabs-tab-container-border-right\":\"1px solid #272a411a\",\"\\u002d\\u002dgutena\\u002d\\u002dtabs-tab-container-border-bottom\":\"1px solid #272a411a\",\"\\u002d\\u002dgutena\\u002d\\u002dtabs-tab-container-border-left\":\"1px solid #272a411a\",\"\\u002d\\u002dgutena\\u002d\\u002dtabs-tab-container-hover-border-top\":\"1px solid #9d9d9d\",\"\\u002d\\u002dgutena\\u002d\\u002dtabs-tab-container-hover-border-right\":\"1px solid #9d9d9d\",\"\\u002d\\u002dgutena\\u002d\\u002dtabs-tab-container-hover-border-bottom\":\"1px solid #9d9d9d\",\"\\u002d\\u002dgutena\\u002d\\u002dtabs-tab-container-hover-border-left\":\"1px solid #9d9d9d\"}} -->";
        bool showQuestion = true;
        private void AddTabsContent(WordPressPCL.Models.Page newPage, Folder folder)
        {
            //Trang và Thực đơn có tab
            var lowerList = folder.LowerFolder.OrderBy(x => x.Order).ToList();
            //string content = "<!-- wp:html -->\r\n<!-- /wp:html -->\r\n\r\n";
            string content = "";
            var tabsId = folder.Oid.ToString().Substring(2, 9);
            content += "<!-- wp:gutena/tabs {\"uniqueId\":\"" + tabsId + "\",\"tabCount\":" + folder.LowerFolder.Count.ToString("D") + ",\"titleTabs\":[";
            for (int i = 0; i < lowerList.Count; i++)
            {
                if (i > 0)
                    content += ",";
                content += "{\"text\":\"" + lowerList[i].Name + "\",\"icon\":\"\",\"iconSize\":22,\"iconPosition\":\"left\"}";
                //content += string.Format(tabContentTemplate, lowerList[i].Oid.ToString().Substring(2, 9), i + 1, i != 0 ? "inactive" : "active"); 
            }
            if (showQuestion)
            {
                bool useClipboard = false;
              
                string startText = "],\"tabPosition\":\"";
                string endText = "-->";
                if (_clipboardService.GetDataPresent("Text"))
                {
                    var clipboardText = _clipboardService.GetData("Text") as string;

                    if (!string.IsNullOrEmpty(clipboardText) && clipboardText.Contains("wp:gutena/tabs") && clipboardText.Contains(startText))
                    {
                        if (_userInteractionService.ShowConfirmation("Bạn có muốn dùng dữ liệu cấu hình tab trong Clipboard ?", "Thông báo"))
                        {
                            clipboardText = clipboardText.Substring(clipboardText.IndexOf(startText));
                            var endTextIndex = clipboardText.IndexOf(endText);
                            if (endTextIndex > 0)
                            {
                                tabConfig = clipboardText.Substring(0, endTextIndex + endText.Length);
                                useClipboard = true;
                            }
                        }
                    }
                }
                if (!useClipboard && folder.UpperFolder != null && !string.IsNullOrEmpty(folder.UpperFolder.Content) && folder.UpperFolder.Content.Contains(startText))
                {
                    var contentText = folder.UpperFolder.Content.Substring(folder.UpperFolder.Content.IndexOf(startText));
                    var endTextIndex = contentText.IndexOf(endText);
                    if (endTextIndex > 0)
                    {
                        tabConfig = contentText.Substring(0, endTextIndex + endText.Length);
                    }
                }
                showQuestion = false;
            }
            content += tabConfig;
            content += "\r\n<div class=\"wp-block-gutena-tabs gutena-tabs-block gutena-tabs-block-" + tabsId + " tabs-block-" + Module.Helpers.TextHelper.GetSlug(folder.Name) + "\"><ul class=\"gutena-tabs-tab tab-flex\">";
            for (int i = 0; i < lowerList.Count; i++)
            {
                string activeText = i != 0 ? "inactive" : "active";
                string tabIndex = (i + 1).ToString("D");
                content += "<li class=\"gutena-tab-title " + activeText + "\" data-tab=\"" + tabIndex + "\"><div class=\"gutena-tab-title-content icon-top\"><div class=\"gutena-tab-title-text\"><div>" + lowerList[i].Name + "</div></div></div></li>";
                //content += string.Format(tabContentTemplate, lowerList[i].Oid.ToString().Substring(2, 9), i + 1, i != 0 ? "inactive" : "active"); 
            }
            content += "</ul><div class=\"gutena-tabs-content\">";
            //string tabContentTemplate = "\r\n<!-- wp:gutena/tab {\"uniqueId\":\"{0}\",\"tabId\":{1},\"tabBorder\":{\"enable\":true,\"normal\":{\"border\":{\"top\":{\"color\":\"#272a411a\",\"style\":\"solid\",\"width\":\"1px\"},\"right\":{\"color\":\"#FFFFFF\",\"style\":\"solid\",\"width\":\"1px\"},\"bottom\":{\"color\":\"#272a411a\",\"style\":\"solid\",\"width\":\"1px\"},\"left\":{\"color\":\"#272a411a\",\"style\":\"solid\",\"width\":\"1px\"}}},\"active\":{\"border\":{\"top\":{\"color\":\"#272a411a\",\"style\":\"solid\",\"width\":\"1px\"},\"right\":{\"color\":\"#FFFFFF\",\"style\":\"solid\",\"width\":\"1px\"},\"bottom\":{\"color\":\"#FFFFFF\",\"style\":\"solid\",\"width\":\"1px\"},\"left\":{\"color\":\"#272a411a\",\"style\":\"solid\",\"width\":\"1px\"}}}},\"parentUniqueId\":\"4f2ed9-d7\",\"blockStyles\":{\"\\u002d\\u002dgutena\\u002d\\u002dtabs-tab-border-top\":\"1px solid #272a411a\",\"\\u002d\\u002dgutena\\u002d\\u002dtabs-tab-border-right\":\"1px solid #FFFFFF\",\"\\u002d\\u002dgutena\\u002d\\u002dtabs-tab-border-bottom\":\"1px solid #272a411a\",\"\\u002d\\u002dgutena\\u002d\\u002dtabs-tab-border-left\":\"1px solid #272a411a\",\"\\u002d\\u002dgutena\\u002d\\u002dtabs-tab-active-border-top\":\"1px solid #272a411a\",\"\\u002d\\u002dgutena\\u002d\\u002dtabs-tab-active-border-right\":\"1px solid #FFFFFF\",\"\\u002d\\u002dgutena\\u002d\\u002dtabs-tab-active-border-bottom\":\"1px solid #FFFFFF\",\"\\u002d\\u002dgutena\\u002d\\u002dtabs-tab-active-border-left\":\"1px solid #272a411a\"}} -->\r\n<div class=\"wp-block-gutena-tab gutena-tab-block gutena-tab-block-{0} {2}\" data-tab=\"{1}\"><!-- wp:paragraph {\"style\":{\"spacing\":{\"margin\":{\"top\":\"0\",\"bottom\":\"0\"}}}} -->\r\n<p style=\"margin-top:0;margin-bottom:0\">4</p>\r\n<!-- /wp:paragraph --></div>\r\n<!-- /wp:gutena/tab -->";
            //string tabContentTemplate = "\r\n\r\n<!-- wp:gutena/tab {\"uniqueId\":\"{tabId}\",\"tabId\":{tabIndex},\"tabBorder\":{\"enable\":true,\"normal\":{\"border\":{\"top\":{\"color\":\"#272a411a\",\"style\":\"solid\",\"width\":\"1px\"},\"right\":{\"color\":\"#FFFFFF\",\"style\":\"solid\",\"width\":\"1px\"},\"bottom\":{\"color\":\"#272a411a\",\"style\":\"solid\",\"width\":\"1px\"},\"left\":{\"color\":\"#272a411a\",\"style\":\"solid\",\"width\":\"1px\"}}},\"active\":{\"border\":{\"top\":{\"color\":\"#272a411a\",\"style\":\"solid\",\"width\":\"1px\"},\"right\":{\"color\":\"#FFFFFF\",\"style\":\"solid\",\"width\":\"1px\"},\"bottom\":{\"color\":\"#FFFFFF\",\"style\":\"solid\",\"width\":\"1px\"},\"left\":{\"color\":\"#272a411a\",\"style\":\"solid\",\"width\":\"1px\"}}}},\"parentUniqueId\":\"4f2ed9-d7\",\"blockStyles\":{\"\\u002d\\u002dgutena\\u002d\\u002dtabs-tab-border-top\":\"1px solid #272a411a\",\"\\u002d\\u002dgutena\\u002d\\u002dtabs-tab-border-right\":\"1px solid #FFFFFF\",\"\\u002d\\u002dgutena\\u002d\\u002dtabs-tab-border-bottom\":\"1px solid #272a411a\",\"\\u002d\\u002dgutena\\u002d\\u002dtabs-tab-border-left\":\"1px solid #272a411a\",\"\\u002d\\u002dgutena\\u002d\\u002dtabs-tab-active-border-top\":\"1px solid #272a411a\",\"\\u002d\\u002dgutena\\u002d\\u002dtabs-tab-active-border-right\":\"1px solid #FFFFFF\",\"\\u002d\\u002dgutena\\u002d\\u002dtabs-tab-active-border-bottom\":\"1px solid #FFFFFF\",\"\\u002d\\u002dgutena\\u002d\\u002dtabs-tab-active-border-left\":\"1px solid #272a411a\"}} -->\r\n<div class=\"wp-block-gutena-tab gutena-tab-block gutena-tab-block-{tabId} {activeText}\" data-tab=\"{tabIndex}\"><!-- wp:paragraph {\"style\":{\"spacing\":{\"margin\":{\"top\":\"0\",\"bottom\":\"0\"}}}} -->\r\n<p style=\"margin-top:0;margin-bottom:0\">Nội dung tab</p>\r\n<!-- /wp:paragraph --></div>\r\n<!-- /wp:gutena/tab -->";
            string tabContentTemplate = "\r\n\r\n<!-- wp:gutena/tab {\"uniqueId\":\"{tabId}\",\"tabId\":{tabIndex},\"tabBorder\":{\"enable\":true,\"normal\":{\"border\":{\"top\":{\"color\":\"#272a411a\",\"style\":\"solid\",\"width\":\"1px\"},\"right\":{\"color\":\"#FFFFFF\",\"style\":\"solid\",\"width\":\"1px\"},\"bottom\":{\"color\":\"#272a411a\",\"style\":\"solid\",\"width\":\"1px\"},\"left\":{\"color\":\"#272a411a\",\"style\":\"solid\",\"width\":\"1px\"}}},\"active\":{\"border\":{\"top\":{\"color\":\"#272a411a\",\"style\":\"solid\",\"width\":\"1px\"},\"right\":{\"color\":\"#FFFFFF\",\"style\":\"solid\",\"width\":\"1px\"},\"bottom\":{\"color\":\"#FFFFFF\",\"style\":\"solid\",\"width\":\"1px\"},\"left\":{\"color\":\"#272a411a\",\"style\":\"solid\",\"width\":\"1px\"}}}},\"parentUniqueId\":\"4f2ed9-d7\",\"blockStyles\":{\"\\u002d\\u002dgutena\\u002d\\u002dtabs-tab-border-top\":\"1px solid #272a411a\",\"\\u002d\\u002dgutena\\u002d\\u002dtabs-tab-border-right\":\"1px solid #FFFFFF\",\"\\u002d\\u002dgutena\\u002d\\u002dtabs-tab-border-bottom\":\"1px solid #272a411a\",\"\\u002d\\u002dgutena\\u002d\\u002dtabs-tab-border-left\":\"1px solid #272a411a\",\"\\u002d\\u002dgutena\\u002d\\u002dtabs-tab-active-border-top\":\"1px solid #272a411a\",\"\\u002d\\u002dgutena\\u002d\\u002dtabs-tab-active-border-right\":\"1px solid #FFFFFF\",\"\\u002d\\u002dgutena\\u002d\\u002dtabs-tab-active-border-bottom\":\"1px solid #FFFFFF\",\"\\u002d\\u002dgutena\\u002d\\u002dtabs-tab-active-border-left\":\"1px solid #272a411a\"}} -->\r\n<div class=\"wp-block-gutena-tab gutena-tab-block gutena-tab-block-{tabId} {activeText}\" data-tab=\"{tabIndex}\">\r\n<!-- wp:html -->\r\n{tabContent}<!-- /wp:html -->\r\n</div>\r\n\r\n<!-- /wp:gutena/tab -->";
            string folderContent = folder.Content;
            try
            {
                var htmlDoc = new HtmlAgilityPack.HtmlDocument();
                htmlDoc.LoadHtml(folderContent);
                var deleteNodes = new System.Collections.Generic.List<HtmlAgilityPack.HtmlNode>();
                for (int i = 0; i < lowerList.Count; i++)
                {
                    string tabId = lowerList[i].Oid.ToString().Substring(2, 9);
                    string tabIndex = (i + 1).ToString("D");
                    string activeText = i != 0 ? "inactive" : "active";
                    string tabContent = "";

                    if (!string.IsNullOrEmpty(folderContent) && !string.IsNullOrEmpty(lowerList[i].URL) && lowerList[i].URL.StartsWith('/'))
                    {
                        var tabsNode = htmlDoc.DocumentNode.SelectNodes(lowerList[i].URL);
                        //Hỗ trợ multi xpath
                        foreach (var tabNode in tabsNode)
                        {
                            tabContent += tabNode.OuterHtml;
                            deleteNodes.Add(tabNode);
                        }
                        //Cắt gọn file html cho dễ nhìn
                        tabContent = tabContent.Replace("\n\n", "\n").Replace("\n\n", "\n").Replace("\t\t", "\t").Replace("\t\t", "\t").Replace("\t\t", "\t").Replace("\t\t", "\t").Replace("\n\t\n\t", "\n\t").Replace("\t\n\t\n", "\t\n");
                        //var tabNode = htmlDoc.DocumentNode.SelectSingleNode(lowerList[i].URL);
                        //if (tabNode != null)
                        //{
                        //    tabContent = tabNode.OuterHtml;
                        //    //Cắt gọn file html cho dễ nhìn
                        //    tabContent = tabContent.Replace("\n\n", "\n").Replace("\n\n", "\n").Replace("\t\t", "\t").Replace("\t\t", "\t").Replace("\t\t", "\t").Replace("\t\t", "\t").Replace("\n\t\n\t", "\n\t").Replace("\t\n\t\n", "\t\n");
                        //    deleteNodes.Add(tabNode);
                        //}
                        //else
                        //{

                        //}
                    }
                    else if (!string.IsNullOrEmpty(lowerList[i].Content))
                    {
                        //Lấy nội dung của tab con đẻ làm nội dung tab
                        tabContent = lowerList[i].Content;
                    }
                    if (string.IsNullOrEmpty(lowerList[i].Content) && !string.IsNullOrEmpty(tabContent))
                        lowerList[i].Content = tabContent;
                    if (string.IsNullOrEmpty(tabContent))
                        tabContent = "<p style=\"margin-top:0;margin-bottom:0\"></p>";
                    content += tabContentTemplate.Replace("{tabId}", tabId).Replace("{tabIndex}", tabIndex).Replace("{activeText}", activeText).Replace("{tabContent}", tabContent);
                    //content += string.Format(tabContentTemplate, lowerList[i].Oid.ToString().Substring(2, 9), i + 1, i != 0 ? "inactive" : "active"); 
                }
                //Xóa các node đã chuyển sang tab
                for (int i = deleteNodes.Count - 1; i >= 0; i--)
                    deleteNodes[i].Remove();
                folderContent = htmlDoc.DocumentNode.OuterHtml;

            }
            catch (System.Exception) { }

            content += "\r\n</div>\r\n</div>\r\n<!-- /wp:gutena/tabs -->";
            content = "<!-- wp:html -->" + folderContent + "\r\n<!-- /wp:html -->\r\n\r\n" + content;
            newPage.Content = new WordPressPCL.Models.Content(content);
        }

        private Newtonsoft.Json.Linq.JObject UpdateMenuItem(Website currentWebsite, int menuItemId, int? menuOrder = null, string meta = null)
        {
            try
            {
                var builder = new System.Text.StringBuilder("{");
                builder.Append("\"id\":" + menuItemId.ToString("D"));
                if (menuOrder != null)
                    builder.Append(",\"menu_order\":" + menuOrder.Value.ToString("D"));
                if (!string.IsNullOrEmpty(meta))
                {
                    builder.Append(",\"meta\":" + meta);
                }
                builder.Append("}");
                string content = builder.ToString();
                using var postBody = new System.Net.Http.StringContent(content, System.Text.Encoding.UTF8, "application/json");
                {
                    var resultString = GetResponseString(currentWebsite, "wp/v2/menu-items/" + menuItemId.ToString("D"), postBody).Result;
                    //if (!string.IsNullOrEmpty(resultString))
                    //{
                    //    return Newtonsoft.Json.JsonConvert.DeserializeObject(resultString) as Newtonsoft.Json.Linq.JObject;
                    //}
                }
            }
            catch (System.Exception)
            {
            }

            return null;
        }
        private bool CheckEqualMedia(byte[] imagesByte, WordPressPCL.Models.MediaItem mediaItem)
        {
            var image = Module.Helpers.ImageHelper.ConvertArrayToBitmap(imagesByte);
            if (image != null && image.Size != null)
            {
                if (mediaItem.MediaDetails != null && mediaItem.MediaDetails.Width == image.Size.Width &&
                        mediaItem.MediaDetails.Height == image.Size.Height)
                {
                    return true;
                }
            }
            return false;
        }

        private string GetMetaMenuItemIcon(int mediaItemId)
        {
            string metaValue = "{";
            metaValue += "\"_menu_item_icon-id\":  \"" + mediaItemId.ToString("D") + "\",";
            metaValue += "\"_menu_item_icon-type\":  \"media\",";
            metaValue += "\"_menu_item_icon-width\":  \"" + GetDefaultImageSise()[0] + "\",";
            metaValue += "\"_menu_item_icon-height\":  \"" + GetDefaultImageSise()[1] + "\"";
            metaValue += "}";
            return metaValue;
        }

        private string[] defaultImageSise = null;
        private string[] GetDefaultImageSise()
        {
            if (defaultImageSise is null)
                defaultImageSise = Module.Helpers.ParameterHelper.GetValueOrDefault(View.ObjectSpace, "WordpressMenuIconSize", "40x40").Split('x', System.StringSplitOptions.RemoveEmptyEntries);
            return defaultImageSise;
        }

        public async void SyncMenuAndHomepage(Website currentWebsite)
        {
            try
            {
                var wordPressClient = GetWordPressClient(currentWebsite, true);
                if (wordPressClient is null)
                    return;
                var settingString = GetResponseString(currentWebsite, "wp/v2/settings").Result;
                if (string.IsNullOrEmpty(settingString))
                    return;
                var setting = Newtonsoft.Json.JsonConvert.DeserializeObject(settingString) as Newtonsoft.Json.Linq.JObject;
                if (setting is null)
                    return;
                var page_on_front = setting.GetValue("page_on_front");
                if (page_on_front is null)
                    return;
                //Vì api bản mới không hỗ trợ raw
                string pathSlug = "wp/v2/pages/" + page_on_front;
                var homePageString = GetResponseString(currentWebsite, pathSlug).Result;
                if (string.IsNullOrEmpty(homePageString))
                    return;
                var homePageObject = Newtonsoft.Json.JsonConvert.DeserializeObject(homePageString) as Newtonsoft.Json.Linq.JObject;
                if (homePageObject is null)
                    return;
                //var homePagePost = GetWordPressClient(sites).Pages.GetByIDAsync(page_on_front).Result;
                //if (homePagePost is null)
                //    return;
                var content_raw = homePageObject.GetValue("content_raw");
                if (content_raw is null)
                    return;
                string content_raw_string = (string)content_raw;
                var homePagePost = Newtonsoft.Json.JsonConvert.DeserializeObject<WordPressPCL.Models.Page>(homePageString);

                if (homePagePost != null && homePagePost.Content != null)
                {
                    if (string.IsNullOrEmpty(homePagePost.Content.Raw))
                        homePagePost.Content.Raw = content_raw_string;
                    //var ux_slider_Index = homePagePost.Post_Content.IndexOf("[/ux_slider]");
                    //var category = homePagePost.Post_Content.Substring(0, ux_slider_Index);
                    var rowArray = homePagePost.Content.Raw.Split("[ux_html label=\"Import Category\"]");
                    if (rowArray.Length == 3)
                    {
                        string rowContentFormat = "[row_inner class=\"category-title\"]\r\n\r\n[col_inner span=\"1\" span__sm=\"12\"]\r\n\r\n[ux_image id=\"{4}\" image_size=\"thumbnail\" height=\"28px\" class=\"img-{2}\"]\r\n\r\n\r\n[/col_inner]\r\n[col_inner span=\"11\" span__sm=\"12\"]\r\n\r\n[button text=\"{0}\" letter_case=\"lowercase\" color=\"secondary\" expand=\"0\" link=\"{1}\" class=\"products-title {2}-title\"]\r\n\r\n[/col_inner]\r\n\r\n[/row_inner]\r\n[ux_products slider_bullets=\"true\" auto_slide=\"3000\" infinitive=\"false\" cat=\"{3}\" products=\"-1\" orderby=\"date\" products=\"{5}\"]\r\n";
                        string postContent = rowArray[0] + "[ux_html label=\"Import Category\"]\r\n[/ux_html]\r\n";
                        int index = 0;
                        var mainMenu = GetMainMenu(currentWebsite);
                        if (mainMenu is null)
                        {
                            Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Không tìm thấy menu chính", InformationType.Error);
                            return;
                        }
                        var menuItems = GetMenuItem(currentWebsite, mainMenu.Id);
                        if (menuItems is null)
                            return;
                        var otherCategoryCaption = Module.Helpers.ParameterHelper.GetValueOrDefault(View.ObjectSpace, "WordpressOtherCategoryCaption", "Loại khác");
                        var wordpressHomepageDisplayMaxItem = Module.Helpers.ParameterHelper.GetIntOrDefault(View.ObjectSpace, "WordpressHomepageDisplayMaxItem", -1);
                        for (int i = 0; i < menuItems.Count; i++)
                        {
                            var menuItem = ((Newtonsoft.Json.Linq.JObject)menuItems[i]);
                            var parent = menuItem.Value<string>("parent");
                            if (parent != "0")
                                continue;
                            var objectType = menuItem.GetValue("object");
                            if (objectType != null && objectType.ToObject<string>() == "product_cat")
                            {
                                var metas = menuItem.GetValue("meta") as Newtonsoft.Json.Linq.JObject;
                                if (metas is null)
                                    continue;
                                var _menu_item_object_id = GetMetaKey(metas, "_menu_item_object_id");
                                if (string.IsNullOrEmpty(_menu_item_object_id))
                                    continue;
                                var caption = menuItem.GetValue("title") as Newtonsoft.Json.Linq.JObject;
                                if (caption is null)
                                    continue;
                                var title = caption.GetValue("rendered")?.ToObject<string>();
                                if (string.IsNullOrEmpty(title))
                                    continue;
                                index = i;
                                var _menu_item_icon_id = GetMetaKey(metas, "_menu_item_icon-id");

                                //var menuObject = rootList[i].Object_Id.Menu;
                                //var imageMeta = rootList[i].Object_Id.GetPostMetaByKey("_menu_item_icon-id");
                                //var imageId = imageMeta != null ? imageMeta.Meta_Value : "";
                                var urlObject = menuItem.Value<string>("url");
                                if (string.IsNullOrEmpty(urlObject))
                                    continue;
                                var uri = new System.Uri(urlObject);
                                var css = Module.Helpers.TextHelper.GetSlug(title);
                                if (title == otherCategoryCaption)
                                {
                                    var currentId = menuItem.Value<string>("id");
                                    if (currentId is null)
                                        continue;
                                    var listId = new System.Collections.Generic.List<string>();
                                    foreach (Newtonsoft.Json.Linq.JObject childMenuItem in menuItems)
                                    {
                                        var itemParent = menuItem.Value<string>("parent");
                                        if (itemParent == currentId)
                                        {
                                            var childMetas = childMenuItem.GetValue("meta") as Newtonsoft.Json.Linq.JObject;
                                            if (childMetas is null)
                                                continue;
                                            var childObjectId = childMetas.Value<string>("_menu_item_object_id");
                                            if (childObjectId != null)
                                            {
                                                listId.Add(childObjectId);
                                            }
                                        }
                                    }
                                    if (!listId.Contains(_menu_item_object_id.ToString()))
                                        listId.Add(_menu_item_object_id.ToString());
                                    string listIdText = string.Join(",", listId.ToArray());

                                    var rowContent = string.Format(rowContentFormat, title, uri.LocalPath, css, listIdText, _menu_item_icon_id, wordpressHomepageDisplayMaxItem.ToString("D"));
                                    postContent += rowContent;
                                    //if (menuObject != null)
                                    //{
                                    //    string url = "/" + wooOptions["category_base"] + "/" + menuObject.Slug;
                                    //    var rowContent = string.Format(rowContentFormat, rootList[i].Object_Id.MenuName, url, menuObject.Slug, listIdText, imageId);
                                    //    postContent += rowContent;
                                    //}
                                    //else if (rootList[i].Object_Id.Post_Type == "nav_menu_item")
                                    //{
                                    //    var _menu_item_object_id = rootList[i].Object_Id.Postmeta.FirstOrDefault(m => m.Meta_Key == "_menu_item_object_id");
                                    //    if (_menu_item_object_id != null && !string.IsNullOrEmpty(_menu_item_object_id.Meta_Value))
                                    //    {
                                    //        var _menu_item_type = rootList[i].Object_Id.Postmeta.FirstOrDefault(m => m.Meta_Key == "_menu_item_type");
                                    //        if (_menu_item_type != null && _menu_item_type.Meta_Value == "post_type")
                                    //        {
                                    //            var postMenu = rootList[i].Object_Id.Session.GetObjectByKey<Posts>(System.Convert.ToInt64(_menu_item_object_id.Meta_Value));
                                    //            if (postMenu != null)
                                    //            {
                                    //                string url = "/" + wooOptions["category_base"] + "/" + postMenu.Post_Name;
                                    //                var rowContent = string.Format(rowContentFormat, rootList[i].Object_Id.MenuName, url, postMenu.Post_Name, listIdText, imageId);
                                    //                postContent += rowContent;
                                    //            }
                                    //        }
                                    //    }
                                    //}

                                    break;
                                }
                                else
                                {

                                    //string url = "/" + wooOptions["category_base"] + "/" + menuObject.Slug;
                                    var rowContent = string.Format(rowContentFormat, title, uri.LocalPath, css, _menu_item_object_id, _menu_item_icon_id, wordpressHomepageDisplayMaxItem.ToString("D"));
                                    postContent += rowContent;
                                    //}
                                }
                            }
                        }
                        //Số 0 là trang chủ
                        index++;

                        postContent += "\r\n[ux_html label=\"Import Category\"]" + rowArray[2];
                        homePagePost.Content.Raw = postContent;
                        var result = wordPressClient.Pages.UpdateAsync(homePagePost);
                        if (result != null)
                            Module.Helpers.XafXpoHelper.ShowMessage(Application, "Kết quả", "Đồng bộ thành công " + (index), InformationType.Info);

                    }
                }
            }
            catch (System.Exception ex)
            {

            }

        }

        private Newtonsoft.Json.Linq.JArray _menuItemList;
        private Newtonsoft.Json.Linq.JArray GetMenuItem(Website website, int menuId)
        {
            if (_menuItemList is null)
            {
                var nav_Menu_Items_string = GetResponseString(website, "wp/v2/menu-items?menus=" + menuId.ToString("D")).Result;
                if (string.IsNullOrEmpty(nav_Menu_Items_string))
                    return null;
                _menuItemList = Newtonsoft.Json.JsonConvert.DeserializeObject(nav_Menu_Items_string) as Newtonsoft.Json.Linq.JArray;
            }
            return _menuItemList;
        }
        private WordPressPCL.Models.Category GetMainMenu(Website website)
        {
            var categoriesString = GetResponseString(website, "wp/v2/menus").Result;
            if (string.IsNullOrEmpty(categoriesString))
                return null;
            categoriesString = categoriesString.Replace("\"meta\":", "\"meta2\":").Replace("\"locations\":", "\"meta\":");
            var categories = Newtonsoft.Json.JsonConvert.DeserializeObject<System.Collections.Generic.List<WordPressPCL.Models.Category>>(categoriesString);
            if (categories is null)
            {
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Không tìm thấy menu", InformationType.Error);
                return null;
            }
            for (int i = 0; i < categories.Count; i++)
            {
                var categoryMeta = Convert.ToString(categories[i].Meta);
                if (!string.IsNullOrEmpty(categoryMeta) && categoryMeta.Contains("primary"))
                {
                    return categories[i];
                }
            }
            return null;
        }

        private string GetMetaKey(Newtonsoft.Json.Linq.JObject meta, string key)
        {
            try
            {
                var metaValueObject = meta.GetValue(key);
                if (metaValueObject is Newtonsoft.Json.Linq.JObject)
                {
                    return metaValueObject.ToObject<string>();
                }
                else if (metaValueObject is Newtonsoft.Json.Linq.JArray)
                {
                    var menu_object_list = (Newtonsoft.Json.Linq.JArray)metaValueObject;
                    if (menu_object_list != null && menu_object_list.Count > 0)
                    {
                        var metaValue = menu_object_list[0].ToObject<string>();
                        return metaValue;
                    }
                }

            }
            catch (System.Exception)
            {

            }

            return null;
        }
        public async Task<string> GetResponseString(Website website, string route, System.Net.Http.HttpContent postBody = null)
        {
            if (!route.Contains("wp-json"))
                route = "wp-json/" + route;
            using (var requestMessage = new System.Net.Http.HttpRequestMessage(postBody is null ? System.Net.Http.HttpMethod.Get : System.Net.Http.HttpMethod.Post, route))
            {
                byte[] bytes = System.Text.Encoding.ASCII.GetBytes(website.WordpressUser + ":" + website.WordpressKey);
                requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(bytes));
                var httpClient = new System.Net.Http.HttpClient();
                httpClient.BaseAddress = new System.Uri($"{website.URL}");
                if (postBody != null)
                {
                    requestMessage.Content = postBody;
                }
                var response = await httpClient.SendAsync(requestMessage).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                }
            }
            ;
            return null;
        }
        public bool SetupWebsiteApplication(Website currentWebsite)
        {
            if (string.IsNullOrEmpty(currentWebsite.Path))
            {
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Chưa có đường dẫn", InformationType.Error);
                return false;
            }
            if (string.IsNullOrEmpty(currentWebsite.URL))
            {
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Chưa có địa chỉ", InformationType.Error);
                return false;
            }
            string name = "Web Server";
            if (currentWebsite.LoginAccountWebServer is null)
            {
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", $"Chưa có tài khoản {name}", InformationType.Error);
                return true;
            }
            if (string.IsNullOrEmpty(currentWebsite.LoginAccountWebServer.Name))
            {
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", $"{name} chưa có tên", InformationType.Error);
                return true;
            }
            if (string.IsNullOrEmpty(currentWebsite.LoginAccountWebServer.Password))
            {
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", $"{name} chưa có mật khẩu", InformationType.Error);
                return true;
            }
            if (string.IsNullOrEmpty(currentWebsite.LoginAccountWebServer.AppName))
            {
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", $"{name} chưa có ứng dụng", InformationType.Error);
                return true;
            }
            var appDB = currentWebsite.Session.FindObject<App>(DevExpress.Data.Filtering.CriteriaOperator.Parse("Name = ?", currentWebsite.LoginAccountWebServer.AppName));
            if (appDB is null)
            {
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", $"{name} không tìm thấy ứng dụng", InformationType.Error);
                return true;
            }
            if (string.IsNullOrEmpty(appDB.HomePage))
            {
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", $"Ứng dụng {name} không có địa chỉ", InformationType.Error);
                return true;
            }
            var currentWebsiteDirectory = new System.IO.DirectoryInfo(currentWebsite.Path);
            var siteUri = new System.Uri(currentWebsite.URL);
            int port = siteUri.Port;
            if (port == 443)
                port = 80;
            string localPublicFolder = string.Format("{0}\\{1}", WebsitesDirectoryServer(), currentWebsiteDirectory.Name);
            string logFile = "log_siteId.txt";
            string deployContent = string.Format("%systemroot%\\system32\\inetsrv\\APPCMD add site /name:{0} /bindings:http://{0}:{1} /physicalPath:{2}", siteUri.Host, port, localPublicFolder);
            deployContent += string.Format("\r\n%systemroot%\\system32\\inetsrv\\APPCMD add apppool /name:{0}", siteUri.Host);
            deployContent += string.Format("\r\n%systemroot%\\system32\\inetsrv\\APPCMD set app \"{0}/\" /applicationPool:{0}", siteUri.Host);
            deployContent += string.Format("\r\n%systemroot%\\system32\\inetsrv\\APPCMD  list site {0} >  {1}\\{2}", siteUri.Host, localPublicFolder, logFile);
            string fileName = string.Format("{0}\\{1}.Deploy.cmd", currentWebsite.Path, currentWebsiteDirectory.Name);
            System.IO.File.WriteAllText(fileName, deployContent);
            string command = string.Format("{0}\\{1}.Deploy.cmd", localPublicFolder, currentWebsiteDirectory.Name);
            if (System.Environment.MachineName.ToLower().Equals(WebsitesDirectoryServer().ToLower()))
            {
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.UseShellExecute = true;
                startInfo.FileName = command;
                startInfo.Verb = "runas";
                startInfo.ErrorDialog = true;
                System.Diagnostics.Process process = System.Diagnostics.Process.Start(startInfo);
                process.WaitForExit();
                //return true;
            }
            else if (_processManagementService.RunCommandFromOtherComputer(command, appDB.HomePage, currentWebsite.LoginAccountWebServer.Name, currentWebsite.LoginAccountWebServer.Password))
            {

            }
            else
            {
                return false;
            }

            var publicLogFile = currentWebsiteDirectory.FullName + "\\" + logFile;
            if (System.IO.File.Exists(publicLogFile))
            {
                var publicLogFileText = System.IO.File.ReadAllText(publicLogFile);
                System.IO.File.Delete(publicLogFile);
                if (!string.IsNullOrEmpty(publicLogFileText))
                {
                    var hostId = publicLogFileText.Split(new char[] { ':', ',' })[1];
                    deployContent = string.Format("{0} --source iis --host {1} --force --installation iis --installationsiteid {2}", SslWinAcmeFile, siteUri.Host, hostId);
                    System.IO.File.WriteAllText(fileName, deployContent);
                    if (System.Environment.MachineName.ToLower().Equals(WebsitesDirectoryServer().ToLower()))
                    {
                        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                        startInfo.UseShellExecute = true;
                        startInfo.FileName = command;
                        startInfo.Verb = "runas";
                        startInfo.ErrorDialog = true;
                        System.Diagnostics.Process process = System.Diagnostics.Process.Start(startInfo);
                        process.WaitForExit();
                        return true;
                    }
                    else if (_processManagementService.RunCommandFromOtherComputer(command, appDB.HomePage, currentWebsite.LoginAccountWebServer.Name, currentWebsite.LoginAccountWebServer.Password))
                    {
                        //System.IO.File.Delete(fileName);
                        return true;
                    }
                    else
                    {
                        //System.IO.File.Delete(fileName);
                        return false;
                    }
                }
                System.IO.File.Delete(publicLogFile);
            }
            //System.IO.File.Delete(fileName);
            return false;
        }

        private string _websitesDirectoryServer;
        private string WebsitesDirectoryServer()
        {
            if (string.IsNullOrEmpty(_websitesDirectoryServer))
            {
                _websitesDirectoryServer = Module.Helpers.ParameterHelper.GetValueOrDefault(View.ObjectSpace, "WebsitesDirectoryServer", "D:\\Websites");
            }
            return _websitesDirectoryServer;
        }

        private string _ssl_win_acme_file;
        private string SslWinAcmeFile()
        {
            if (string.IsNullOrEmpty(_ssl_win_acme_file))
            {
                _ssl_win_acme_file = Module.Helpers.ParameterHelper.GetValueOrDefault(View.ObjectSpace, "WebsitesSslWinAcmeFile", "E:\\Setup\\Tools\\LetsEncrypt\\win-acme.v2.1.23.1315.x64.pluggable\\wacs.exe");
            }
            return _ssl_win_acme_file;
        }

        public bool DeleteWebsiteOldData(Website currentWebsite)
        {
            if (string.IsNullOrEmpty(currentWebsite.URL))
            {
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Chưa có địa chỉ", InformationType.Error);
                return false;
            }
            if (string.IsNullOrEmpty(currentWebsite.WordpressUser))
            {
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Chưa có Wordpress User", InformationType.Error);
                return false;
            }
            if (string.IsNullOrEmpty(currentWebsite.WordpressKey))
            {
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Chưa có Wordpress Key", InformationType.Error);
                return false;
            }
            if (string.IsNullOrEmpty(currentWebsite.WooCommerceUser))
            {
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Chưa có WooCommerce User", InformationType.Error);
                return false;
            }
            if (string.IsNullOrEmpty(currentWebsite.WooCommerceKey))
            {
                Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Chưa có WooCommerce Key", InformationType.Error);
                return false;
            }
            if (currentWebsite.URL.EndsWith('/'))
                currentWebsite.URL = currentWebsite.URL.Substring(0, currentWebsite.URL.Length - 1);
            var wordPressClient = new WordPressPCL.WordPressClient($"{currentWebsite.URL}/wp-json/");
            wordPressClient.Auth.UseBasicAuth(currentWebsite.WordpressUser, currentWebsite.WordpressKey);
            var r = wordPressClient.Posts.GetAllAsync().Result;
            WooCommerceNET.RestAPI rest = new WooCommerceNET.RestAPI($"{currentWebsite.URL}/wp-json/wc/v3/", currentWebsite.WooCommerceUser, currentWebsite.WooCommerceKey);
            WooCommerceNET.WooCommerce.v3.WCObject wcObject = new WooCommerceNET.WooCommerce.v3.WCObject(rest);

            var postsList = Module.Helpers.ParameterHelper.GetValueOrDefault(View.ObjectSpace, "WebsitesDeletePostId", "8288,8283").Split(',', StringSplitOptions.RemoveEmptyEntries);
            var postCategoryList = Module.Helpers.ParameterHelper.GetValueOrDefault(View.ObjectSpace, "WebsitesDeletePostCategoryId", "279,294,293,295").Split(',', StringSplitOptions.RemoveEmptyEntries);
            var productsList = Module.Helpers.ParameterHelper.GetValueOrDefault(View.ObjectSpace, "WebsitesDeleteProductId", "7882,7878,7829,407").Split(',', StringSplitOptions.RemoveEmptyEntries);
            var productCategoryList = Module.Helpers.ParameterHelper.GetValueOrDefault(View.ObjectSpace, "WebsitesDeleteProductCategoryId", "297,289,239,242").Split(',', StringSplitOptions.RemoveEmptyEntries);
            var menuItemList = Module.Helpers.ParameterHelper.GetValueOrDefault(View.ObjectSpace, "WebsitesDeleteMenuItemId", "8436,8438,8440,8442").Split(',', StringSplitOptions.RemoveEmptyEntries);
            int result = 0;
            foreach (var postId in postsList)
            {
                try
                {
                    if (wordPressClient.Posts.DeleteAsync(System.Convert.ToInt32(postId), true).Result)
                        result++;
                }
                catch (System.Exception) { }
            }
            foreach (var postCategoryId in postCategoryList)
            {
                try
                {
                    if (wordPressClient.Categories.DeleteAsync(System.Convert.ToInt32(postCategoryId)).Result)
                        result++;
                }
                catch (System.Exception) { }
            }
            foreach (var productId in productsList)
            {
                try
                {
                    if (wcObject.Product.Delete(System.Convert.ToUInt64(productId)).Result != null)
                        result++;
                }
                catch (System.Exception) { }
            }
            foreach (var productCategoryId in productCategoryList)
            {
                try
                {
                    if (wcObject.Category.Delete(System.Convert.ToUInt64(productCategoryId), true).Result != null)
                        result++;
                }
                catch (System.Exception)
                {
                }
            }
            var route = "wp/v2/menu-items/";
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(currentWebsite.WordpressUser + ":" + currentWebsite.WordpressKey);
            foreach (var menuItemId in menuItemList)
            {
                try
                {
                    using (var requestMessage = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Delete, route + menuItemId + "?force=true"))
                    {
                        requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(bytes));
                        var httpClient = new System.Net.Http.HttpClient();
                        httpClient.BaseAddress = new System.Uri($"{currentWebsite.URL}/wp-json/");
                        var response = httpClient.SendAsync(requestMessage).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            var mn = response.Content.ReadAsStringAsync().Result;
                            result++;
                        }
                    }

                }
                catch (System.Exception) { }
            }
            return true;
        }

    

        #endregion SourceCode4510ImportCode

        #region SourceCode4511ImportCode
                public static void CheckValidDB(Website currentWebsite, string name = "Cơ sở dữ liệu")
        {
            if (string.IsNullOrEmpty(currentWebsite.DatabaseName))
            {
                throw new ArgumentNullException($"Chưa có {name}");
            }
            if (currentWebsite.LoginAccountDB is null)
            {
                throw new ArgumentNullException($"Chưa có tài khoản {name}");
            }
            if (string.IsNullOrEmpty(currentWebsite.LoginAccountDB.Name))
            {
                throw new ArgumentNullException($"{name} chưa có tên");
            }
            if (string.IsNullOrEmpty(currentWebsite.LoginAccountDB.Password))
            {
                throw new ArgumentNullException($"{name} chưa có mật khẩu");
            }
            if (string.IsNullOrEmpty(currentWebsite.LoginAccountDB.AppName))
            {
                throw new ArgumentNullException($"{name} chưa có ứng dụng");
            }
        }
        //public static void CheckValidDB(App appDB, string name = "Cơ sở dữ liệu")
        //{            
        //    if (appDB is null)
        //    {
        //        throw new ArgumentNullException($"{name} không tìm thấy ứng dụng");

        //    }
        //    if (string.IsNullOrEmpty(appDB.HomePage))
        //    {
        //        throw new ArgumentNullException($"Ứng dụng {name} không có địa chỉ");
        //    }

        //}

        #endregion SourceCode4511ImportCode

  
  
        #region Base Object Service
	    		//public string ToolTipControllerText(View view, Module.BusinessObjects.Website website)
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
		//public object NameToolTipControllerText(View view, Module.BusinessObjects.Website website)
        //{
        //    if (Name != null) 
		//			return Name;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object TitleToolTipControllerText(View view, Module.BusinessObjects.Website website)
        //{
        //    if (Title != null) 
		//			return Title;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object URLToolTipControllerText(View view, Module.BusinessObjects.Website website)
        //{
        //    if (URL != null) 
		//			return URL;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object PathToolTipControllerText(View view, Module.BusinessObjects.Website website)
        //{
        //    if (Path != null) 
		//			return Path;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object MemberToolTipControllerText(View view, Module.BusinessObjects.Website website)
        //{
        //    if (Member != null) 
		//			return Member;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object FolderToolTipControllerText(View view, Module.BusinessObjects.Website website)
        //{
        //    if (Folder != null) 
		//			return Folder;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object TemplateWebsiteToolTipControllerText(View view, Module.BusinessObjects.Website website)
        //{
        //    if (TemplateWebsite != null) 
		//			return TemplateWebsite;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object IconToolTipControllerText(View view, Module.BusinessObjects.Website website)
        //{
        //    if (Icon != null) 
		//			return Icon;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object DatabaseNameToolTipControllerText(View view, Module.BusinessObjects.Website website)
        //{
        //    if (DatabaseName != null) 
		//			return DatabaseName;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object LoginAccountDBToolTipControllerText(View view, Module.BusinessObjects.Website website)
        //{
        //    if (LoginAccountDB != null) 
		//			return LoginAccountDB;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object LoginAccountWebServerToolTipControllerText(View view, Module.BusinessObjects.Website website)
        //{
        //    if (LoginAccountWebServer != null) 
		//			return LoginAccountWebServer;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object WordpressUserToolTipControllerText(View view, Module.BusinessObjects.Website website)
        //{
        //    if (WordpressUser != null) 
		//			return WordpressUser;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object WordpressKeyToolTipControllerText(View view, Module.BusinessObjects.Website website)
        //{
        //    if (WordpressKey != null) 
		//			return WordpressKey;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object WooCommerceUserToolTipControllerText(View view, Module.BusinessObjects.Website website)
        //{
        //    if (WooCommerceUser != null) 
		//			return WooCommerceUser;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object WooCommerceKeyToolTipControllerText(View view, Module.BusinessObjects.Website website)
        //{
        //    if (WooCommerceKey != null) 
		//			return WooCommerceKey;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object UpdateToolTipControllerText(View view, Module.BusinessObjects.Website website)
        //{
        //    if (Update != null) 
		//			return Update;
        //    return null;
        //}
    

		//Tooltip for Object
		//public object WebsiteTemplateToolTipControllerText(View view, Module.BusinessObjects.Website website)
        //{
        //    if (WebsiteTemplate != null) 
		//			return WebsiteTemplate;
        //    return null;
        //}
    

	    #endregion
  

    }
}
