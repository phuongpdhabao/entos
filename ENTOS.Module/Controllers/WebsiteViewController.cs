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
    public partial class WebsiteViewController: BaseViewController<Module.BusinessObjects.Website>
    {      
        
        public WebsiteViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(Module.BusinessObjects.Website);    
            //TargetViewNesting = Nesting.Nested;
        }

        #region DependencyInjection
        private IMySqlManagementService mySqlManagementService;
        protected IMySqlManagementService _mySqlManagementService => mySqlManagementService ??= Application.ServiceProvider.GetRequiredService<IMySqlManagementService>();        
  
  
  
  
        #endregion DependencyInjection
		
		protected override void OnActivated()
        {
            base.OnActivated();
        }
        
        private Module.Services.WebsiteService websiteService;
        protected override void OnViewControlsCreated()
        {
             base.OnViewControlsCreated();
      
             websiteService = new Module.Services.WebsiteService(this);
             
        }
        
        protected override void OnDeactivated()
        {                 
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }


        
        //Code: 1387            Oid: e5c023e8-3adf-4e2b-b1b6-64a7dc149477
		private void CreateWebsite_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(CreateWebsite), "Tạo Website");              
      
            #region CreateWebsiteImportCode
            Module.BusinessObjects.App GetValidateApp(string appName) 
            {
                var query = GetObjectsQuery<Module.BusinessObjects.App>();
                var result=  query.FirstOrDefault(x => x.Name == appName);
                //var result = View.ObjectSpace.FindObject<Module.BusinessObjects.App>(DevExpress.Data.Filtering.CriteriaOperator.Parse("Name = ?", appName));
                if (result is null)
                {
                    throw new UserFriendlyException($"{appName} không tìm thấy ứng dụng");                   
                }
                if (string.IsNullOrEmpty(result.HomePage))
                {
                    throw new UserFriendlyException($"Ứng dụng {appName} không có địa chỉ");
                }
                return result;
            }
            //throw new UserFriendlyException( "Username should be unique!");
            var currentWebsite = GetCurrentObject();
            if (currentWebsite is null)
                throw new UserFriendlyException(GetLocalizedText(Website.CurrentWebsiteIsNullException));
            if (string.IsNullOrEmpty(currentWebsite.URL))
                throw new UserFriendlyException(GetLocalizedText(Website.CurrentWebsiteUrlIsEmptyException));
            var uri = new System.Uri(currentWebsite.URL);
            string siteName = uri.Host;
            //string database = siteName;
            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();
            if (e.SelectedChoiceActionItem.Id.Equals("All") || e.SelectedChoiceActionItem.Id.Contains("Database"))
            {
                //Dùng phpMyAdmin để copy database: Chọn database cần copy, Chọn tab "Thao tác", nhập "Chép cơ sở dữ liệu sang", rồi thực hiện
                //Sửa đường dẫn website mới trong table options: record siteurl và home

                Module.Services.WebsiteService.CheckValidDB(currentWebsite);
                Module.BusinessObjects.App appDB = GetValidateApp(currentWebsite.LoginAccountDB.AppName);
                if (currentWebsite.TemplateWebsite is null)
                {
                    throw new UserFriendlyException("Chưa có Website mẫu");
                    return;
                }
                if (string.IsNullOrEmpty(currentWebsite.TemplateWebsite.URL))
                {
                    throw new UserFriendlyException("Địa chỉ Website mẫu trống");
                    return;
                }
                if (string.IsNullOrEmpty(currentWebsite.TemplateWebsite.DatabaseName))
                {
                    throw new UserFriendlyException("Chưa có Cơ sở dữ liệu mẫu");
                    return;
                }
                if (currentWebsite.TemplateWebsite.LoginAccountDB is null)
                {
                    throw new UserFriendlyException("Website mẫu chưa có Cơ sở dữ liệu");
                    return;
                }
                if (currentWebsite.TemplateWebsite.LoginAccountDB is null)
                {
                    throw new UserFriendlyException("Website mẫu chưa có Cơ sở dữ liệu");
                    return;
                }
                if (string.IsNullOrEmpty(currentWebsite.TemplateWebsite.LoginAccountDB.Name))
                {
                    throw new UserFriendlyException("Cơ sở dữ liệu Website mẫu  chưa có tên");
                    return;
                }
                if (string.IsNullOrEmpty(currentWebsite.TemplateWebsite.LoginAccountDB.Password))
                {
                    throw new UserFriendlyException("Cơ sở dữ liệu Website mẫu chưa có mật khẩu");
                    return;
                }
                if (string.IsNullOrEmpty(currentWebsite.TemplateWebsite.LoginAccountDB.AppName))
                {
                    throw new UserFriendlyException("Cơ sở dữ liệu Website mẫu chưa có ứng dụng");
                    return;
                }
                var appDBTemplateWebsite = currentWebsite.Session.FindObject<Module.BusinessObjects.App>(DevExpress.Data.Filtering.CriteriaOperator.Parse("Name = ?", currentWebsite.LoginAccountDB.AppName));
                if (appDBTemplateWebsite is null)
                {
                    throw new UserFriendlyException("Cơ sở dữ liệu Website mẫu không tìm thấy ứng dụng");
                    return;
                }
                if (string.IsNullOrEmpty(appDBTemplateWebsite.HomePage))
                {
                    throw new UserFriendlyException("Ứng dụng Website mẫu không có địa chỉ");
                    return;
                }
                ShowWaitForm("Xuất dữ liệu", " ", stopWatch.Elapsed);
                //Tạo fileSql mẫu
                string constring = $"server={appDBTemplateWebsite.HomePage};user id={currentWebsite.TemplateWebsite.LoginAccountDB.Name}; password={currentWebsite.TemplateWebsite.LoginAccountDB.Password};database={currentWebsite.TemplateWebsite.DatabaseName};";
                string file = System.IO.Path.GetTempPath() + "\\" + "temp_backup.sql";
                _mySqlManagementService.ExportToFile(constring, file);
                
                ShowWaitForm("Tạo cơ sở dữ liệu", " ", stopWatch.Elapsed);
                string conStringCreate = $"server={appDB.HomePage};user id={currentWebsite.LoginAccountDB.Name}; password={currentWebsite.LoginAccountDB.Password};";
                _mySqlManagementService.CreateDatabase(conStringCreate, currentWebsite.DatabaseName);
                
                //Sửa đường dẫn website mới trong table options: record siteurl và home
                //Replace text in site
                string text = System.IO.File.ReadAllText(file);
                if (currentWebsite.TemplateWebsite.URL.EndsWith("/"))
                    currentWebsite.TemplateWebsite.URL = currentWebsite.TemplateWebsite.URL.Substring(0, currentWebsite.TemplateWebsite.URL.Length - 1);
                if (currentWebsite.URL.EndsWith("/"))
                    currentWebsite.URL = currentWebsite.URL.Substring(0, currentWebsite.URL.Length - 1);
                text = text.Replace(currentWebsite.TemplateWebsite.URL, currentWebsite.URL);
                System.IO.File.WriteAllText(file, text);
                ShowWaitForm("Nhập dữ liệu", " ", stopWatch.Elapsed);
                string constring3 = $"server={appDB.HomePage};user id={currentWebsite.LoginAccountDB.Name}; password={currentWebsite.LoginAccountDB.Password};database={currentWebsite.DatabaseName};";
                _mySqlManagementService.ImportFromFile(constring3, file);
                
                ShowWaitForm(null, null);
                notificationService.NotifySuccess("Kết quả", "Sao chép cơ sở dữ liệu thành công");

            }
            if (e.SelectedChoiceActionItem.Id.Equals("All") || e.SelectedChoiceActionItem.Id.Contains("Files"))
            {
                if (string.IsNullOrEmpty(currentWebsite.Path))
                {
                    throw new UserFriendlyException("Chưa có đường dẫn");
                    return;
                }
                if (currentWebsite.TemplateWebsite is null)
                {
                    throw new UserFriendlyException("Chưa có Website mẫu");
                    return;
                }
                if (string.IsNullOrEmpty(currentWebsite.TemplateWebsite.Path))
                {
                    throw new UserFriendlyException("Website mẫu chưa có đường dẫn");
                    return;
                }
                if (!System.IO.Directory.Exists(currentWebsite.TemplateWebsite.Path))
                {
                    throw new UserFriendlyException("Không tồn tại đường dẫn Website mẫu");
                    return;
                }

                Module.Services.WebsiteService.CheckValidDB(currentWebsite);
                Module.BusinessObjects.App appDB = GetValidateApp(currentWebsite.LoginAccountDB.AppName);
                foreach (string dirPath in System.IO.Directory.GetDirectories(currentWebsite.TemplateWebsite.Path, "*", System.IO.SearchOption.AllDirectories))
                {
                    System.IO.Directory.CreateDirectory(dirPath.Replace(currentWebsite.TemplateWebsite.Path, currentWebsite.Path));
                }
                ShowWaitForm("Đang copy", "Chuẩn bị danh sách tập tin", stopWatch.Elapsed);
                //Copy all the files & Replaces any files with the same name
                var allFiles = System.IO.Directory.GetFiles(currentWebsite.TemplateWebsite.Path, "*.*", System.IO.SearchOption.AllDirectories);
                int total = allFiles.Count();
                decimal countNumber = 0;
                //Dùng Parallel để tăng tốc copy
                System.Threading.Tasks.Parallel.ForEach(allFiles, (newPath) =>
                {
                    try
                    {
                        countNumber++;
                        string newFile = newPath.Replace(currentWebsite.TemplateWebsite.Path, currentWebsite.Path);
                        string messageName = newPath.Replace(currentWebsite.TemplateWebsite.Path, "");
                        ShowWaitForm((countNumber / total).ToString("p0") + " - Đang copy", messageName, stopWatch.Elapsed);
                        System.IO.File.Copy(newPath, newFile, true);
                    }
                    catch (Exception caughtException)
                    {
                        //TODO: Handle copy error here!
                    }
                });
                //foreach (string newPath in allFiles)
                //{
                //    countNumber++;
                //    string newFile = newPath.Replace(currentWebsite.TemplateWebsite.Path, currentWebsite.Path);
                //    string messageName = newPath.Replace(currentWebsite.TemplateWebsite.Path, "");
                //    ShowWaitForm((countNumber / total).ToString("p0") + " - Đang copy", messageName, stopWatch.Elapsed);
                //    System.IO.File.Copy(newPath, newFile, true);
                //}
                ShowWaitForm(null, null);
                //Edit file wp-config.php
                var currentWebsitePath = currentWebsite.Path;
                if (currentWebsitePath.EndsWith("\\"))
                    currentWebsitePath = currentWebsitePath.Substring(0, currentWebsitePath.Length - 1);
                var currentWebsiteDirectory = new System.IO.DirectoryInfo(currentWebsitePath);
                var templateWebsiteDirectory = new System.IO.DirectoryInfo(currentWebsite.TemplateWebsite.Path);
                var wp_config = currentWebsitePath + "\\" + "wp-config.php";
                if (System.IO.File.Exists(wp_config))
                {
                    var allLine = System.IO.File.ReadAllLines(wp_config);
                    for (int i = 0; i < allLine.Count(); i++)
                    {
                        if (string.IsNullOrEmpty(allLine[i]))
                            continue;
                        if (allLine[i].Contains("'DB_NAME'"))
                        {
                            allLine[i] = string.Format("define( 'DB_NAME', '{0}' );", currentWebsite.DatabaseName);
                            //break;                            
                        }
                        else if (allLine[i].Contains("'DB_USER'"))
                        {
                            allLine[i] = string.Format("define( 'DB_USER', '{0}' );", currentWebsite.LoginAccountDB.Name);
                            //break;                            
                        }
                        else if (allLine[i].Contains("'DB_PASSWORD'"))
                        {
                            allLine[i] = string.Format("define( 'DB_PASSWORD', '{0}' );", currentWebsite.LoginAccountDB.Password);
                            //break;                            
                        }
                        //else if (allLine[i].Contains("'DB_HOST'"))
                        //{
                        //    allLine[i] = string.Format("define( 'DB_HOST', '{0}' );", appDB.HomePage);
                        //    //break;                            
                        //}
                        else if (allLine[i].Contains(templateWebsiteDirectory.Name))
                        {
                            allLine[i] = allLine[i].Replace(templateWebsiteDirectory.Name, currentWebsiteDirectory.Name);
                            //break;                            
                        }
                    }
                    System.IO.File.WriteAllLines(wp_config, allLine);
                }
                //Edit file aios-bootstrap.php
                var aios_bootstrap = currentWebsitePath + "\\" + "aios-bootstrap.php";
                if (System.IO.File.Exists(aios_bootstrap))
                {
                    var allLine = System.IO.File.ReadAllLines(aios_bootstrap);
                    for (int i = 0; i < allLine.Count(); i++)
                    {
                        if (string.IsNullOrEmpty(allLine[i]))
                            continue;
                        if (allLine[i].Contains(templateWebsiteDirectory.Name))
                        {
                            allLine[i] = allLine[i].Replace(templateWebsiteDirectory.Name, currentWebsiteDirectory.Name);
                            //break;                            
                        }
                        System.IO.File.WriteAllLines(aios_bootstrap, allLine);
                    }
                }
                ShowWaitForm(null, null);
                notificationService.NotifySuccess("Kết quả", "Tạo dữ liệu thành công");
            }
            if (e.SelectedChoiceActionItem.Id.Equals("All") || e.SelectedChoiceActionItem.Id.Contains("Setup"))
            {
                if (websiteService.SetupWebsiteApplication(currentWebsite))
                {
                    notificationService.NotifySuccess("Kết quả", "Cài đặt thành công");
                }
                else
                {
                    throw new UserFriendlyException("Không thể cài đặt");
                }
            }
            if (e.SelectedChoiceActionItem.Id.Contains("Delete"))
            {
                if (websiteService.DeleteWebsiteOldData(currentWebsite))
                {
                    notificationService.NotifySuccess("Kết quả", "Xóa dữ liệu thành công");
                }
            }
            else if (e.SelectedChoiceActionItem.Id.Equals("Logo"))
            {
                if(currentWebsite.Icon is null)
                {
                    throw new UserFriendlyException("Chưa có biểu tượng");
                    return;
                }
                var wordPressClient = websiteService.GetWordPressClient(currentWebsite, false);
                if (wordPressClient is null)
                    return;
                var media = websiteService.UploadImage(wordPressClient, currentWebsite.Icon, currentWebsite.Name + "_Logo");
                if (media != null)
                {
                    //var settings = wordPressClient.Settings.GetSettingsAsync().Result;
                    try
                    {
                      
                        var builder = new System.Text.StringBuilder("{");
                        builder.Append("\"site_logo\":" + media.Id.ToString("D"));
                        builder.Append(",\"site_icon\":" + media.Id.ToString("D"));
                        builder.Append("}");
                        string content = builder.ToString();                        
                        using var postBody = new System.Net.Http.StringContent(content, System.Text.Encoding.UTF8, "application/json");
                        {
                            var resultString = websiteService.GetResponseString(currentWebsite, "wp/v2/settings", postBody).Result;
                            if (!string.IsNullOrEmpty(resultString))
                            {
                                //var resultObject = Newtonsoft.Json.JsonConvert.DeserializeObject(resultString) as Newtonsoft.Json.Linq.JObject;
                                notificationService.NotifySuccess("Kết quả", "Thay đổi logo thành công");
                            }
                        }
                    }
                    catch (System.Exception)
                    {
                    }
                }

            }
            else if (e.SelectedChoiceActionItem.Id.Contains("SyncMenuAndHomepage"))
            {
                websiteService.SyncMenuAndHomepage(currentWebsite);
            }
            else if (e.SelectedChoiceActionItem.Id.Contains("cat") || e.SelectedChoiceActionItem.Id.Contains("page") || e.SelectedChoiceActionItem.Id.Equals("Edit_menu"))
            {
                websiteService.FolderToWordpress(e, currentWebsite);              
            }
            stopWatch.Stop();






            #endregion CreateWebsiteImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
     }
}