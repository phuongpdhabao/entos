using DevExpress.ExpressApp;
using ENTOS.Module.Helpers;
using ENTOS.Module.SystemObjects;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.ComponentModel;

namespace ENTOS.Module.Controllers
{
    [ToolboxItem(false)]
    public abstract class CommonViewController<T> : ViewController
    {
        //public CommonViewController()
        //{
        //    InitializeComponent();
        //    TargetObjectType = typeof(T);
        //    //TargetViewNesting = Nesting.Nested;
        //}
        //protected abstract void InitializeComponent();

        //Cách chuẩn bị lỗi trong blazor
        //private readonly AutoMapper.IMapper mapper;
        //private readonly Module.Interfaces.ILogService logger;
        //private readonly Module.Interfaces.INotificationService notificationService;
        //private readonly Module.Interfaces.IUserInteractionService userInteractionService;

        //[Microsoft.Extensions.DependencyInjection.ActivatorUtilitiesConstructor]
        //public TermLocationViewController(AutoMapper.IMapper mapper, Module.Interfaces.ILogService logger, Module.Interfaces.INotificationService notificationService, Module.Interfaces.IUserInteractionService userInteractionService) : this()
        //{
        //    this.mapper = mapper;
        //    this.logger = logger;
        //    this.notificationService = notificationService;
        //    this.userInteractionService = userInteractionService;          
        //}
        //Các fix không dùng DI


        protected override void OnViewControlsCreated()
        {
            Module.Helpers.LogHelper.Info($"Tạo controler {this.Name} : {this.View?.Id}");
            base.OnViewControlsCreated();
        }

        protected void LogActionStart(string actionName, string actionCaption)
        {
#if !DEBUG
            string logMessage = $"{this.View?.Id} -> ({actionCaption}) {actionName}: {View.GetCurrentObjectCaption()} - {View.SelectedObjects?.Count} items";
            using (Module.Helpers.LogHelper.PushProperty("SelectedObjects", GetKeySelectedObjects()))
                Module.Helpers.LogHelper.Info(logMessage + " - Begin");   
#endif
        }

        protected T GetCurrentObject()
        {
            return (T)View.CurrentObject;
        }
        protected IEnumerable<T> GetSelectedObjects()
        {
            return View.SelectedObjects.Cast<T>();
        }

        protected IEnumerable<object> GetKeySelectedObjects()
        {
            return GetSelectedObjects().Select(x => View.ObjectTypeInfo.KeyMember.GetValue(x));
        }

        protected M GetMasterObject<M>() where M : class
        {
            return GetMasterObject() as M;
        }

        protected object GetMasterObject()
        {
            if (View is ListView listView && listView.CollectionSource is PropertyCollectionSource pcs)
            {
                return pcs.MasterObject;
            }
            return null;
        }

        protected System.Collections.IList GetDisplayObjects()
        {
            return ((ListView)View).CollectionSource.List;        
        }

        #region Method dùng chung cho Service và ViewController

        private AutoMapper.IMapper _mapper;
        private Module.Interfaces.ILogService _logger;
        private Module.Interfaces.INotificationService _notificationService;
        private Module.Interfaces.IUserInteractionService _userInteractionService;


        protected Module.Interfaces.ILogService logger => _logger ??= Application.ServiceProvider.GetRequiredService<Module.Interfaces.ILogService>();
        protected Module.Interfaces.INotificationService notificationService => _notificationService ??= Application.ServiceProvider.GetRequiredService<Module.Interfaces.INotificationService>();
        protected AutoMapper.IMapper mapper => _mapper ??= Application.ServiceProvider.GetRequiredService<AutoMapper.IMapper>();
        protected Module.Interfaces.IUserInteractionService userInteractionService => _userInteractionService ??= Application.ServiceProvider.GetRequiredService<Module.Interfaces.IUserInteractionService>();

        protected string GetLocalizedText(string key)
        {
            return DevExpress.ExpressApp.Utils.CaptionHelper.GetLocalizedText("Messages", key);
        }

        protected void ShowWaitForm(string caption, string message = null, TimeSpan? elapsed = null, bool defaultSplashScreenManager = false)
        {
            //Module.Helpers.XafXpoHelper.ShowOrCloseWaitFormWithCancelButton();
            Module.Helpers.XafXpoHelper.ShowOrCloseDefaultWaitForm(caption, message, elapsed, defaultSplashScreenManager);
        }
        protected void CloseWaitForm()
        {
            Module.Helpers.XafXpoHelper.ShowOrCloseDefaultWaitForm(null, null);
        }

        protected IQueryable<T> GetObjectsQuery<T>(bool inTransaction = false)
        {
            return ObjectSpace.GetObjectsQuery<T>(inTransaction);
        }

        #endregion

        //protected override void OnActivated()
        //{
        //    base.OnActivated();
        //    if (View is DetailView)
        //    {
        //        //if (Frame is WinWindow)
        //        //((WinWindow) Frame).KeyDown += WindowController1_KeyDown;
        //        //if (Frame is WebWindow)
        //        //((WebWindow) Frame).PagePreRender += CurrentRequestWindow_PagePreRender;           
        //    }
        //    else if (View is ListView)
        //    {
        //        //var parent = View.ObjectSpace.Owner as DetailView;
        //    }
        //}
    }
    [ToolboxItem(false)]
    public abstract class BaseViewController<T> : CommonViewController<T>
    {
        
        private void OnViewObjectSpaceCommitted(object sender, EventArgs e)
        {
            if (View.CurrentObject is IOnViewObjectSpaceCommitted currentObject)
            {
                currentObject.OnViewObjectSpaceCommitted(View);
            }
        }
        protected override void OnDeactivated()
        {
            if (View is DetailView)
                View.ObjectSpace.Committed -= OnViewObjectSpaceCommitted;
            base.OnDeactivated();
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            if (View is DetailView)
                View.ObjectSpace.Committed += OnViewObjectSpaceCommitted;

        }
    }
}
