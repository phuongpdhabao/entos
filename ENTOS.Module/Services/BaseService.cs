using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo;
using ENTOS.Module.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Globalization;


namespace ENTOS.Module.Services
{
    public class BaseService
    {
        public BaseService() 
        {
        }

        private ViewController _viewController;


        public BaseService(ViewController viewController) 
        {
            _viewController = viewController;
        }

        protected ViewController ViewController => _viewController;

        [Browsable(false)]
        protected View View => _viewController.View;

        [Browsable(false)]
        protected IObjectSpace ObjectSpace => View.ObjectSpace;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected XafApplication Application => _viewController.Application;

        private IDictionary<string, string>? _parameterCache;

        private IDictionary<string, string> ParameterCache
            => _parameterCache ??= new Dictionary<string, string>();

        protected string GetValueOrDefault(string key, string defaultValue = null)
        {
            if(ParameterCache.TryGetValue(key, out var cachedValue))
            {
                return cachedValue;
            }
            var result = ParameterHelper.GetValueOrDefault(_viewController.View.ObjectSpace, key, defaultValue);
            ParameterCache.Add(key, result);
            return result;
        }
        protected string GetValueOrDefaultForCurrent(string key, string defaultValue = null)
        {
            if (ParameterCache.TryGetValue(key, out var cachedValue))
            {
                return cachedValue;
            }
            var result = ParameterHelper.GetParameterValueOrDefault(_viewController.View.ObjectSpace, key, defaultValue, SecuritySystem.CurrentUserId).Value;
            ParameterCache.Add(key, result);
            return result;
        }


        protected T GetValueOrDefault<T>(string key, T defaultValue) where T : struct, IConvertible
        {
            var enCul = new CultureInfo("en-US");
            if (ParameterCache.TryGetValue(key, out var cachedValue))
            {
                if (!string.IsNullOrEmpty(cachedValue))
                    return ChangeType<T>(cachedValue, enCul);
                else
                    throw new UserFriendlyException($"Key {key} is null");

            }         
            var result = GetValueOrDefault(key, defaultValue.ToString(enCul));
            return ChangeType<T>(result, enCul);
        }
        protected T GetValueOrDefaultForCurrent<T>(string key, T defaultValue) where T : struct, IConvertible
        {
            var enCul = new CultureInfo("en-US");
            if (ParameterCache.TryGetValue(key, out var cachedValue))
            {
                if (!string.IsNullOrEmpty(cachedValue))
                    return ChangeType<T>(cachedValue, enCul);
                else
                    throw new UserFriendlyException($"Key {key} is null");

            }
            var result = GetValueOrDefaultForCurrent(key, defaultValue.ToString(enCul));
            return ChangeType<T>(result, enCul);
        }

        private T ChangeType<T>(string value, CultureInfo cultureInfo)
        {
            return (T)Convert.ChangeType(value, typeof(T), cultureInfo);
        }
        protected  T CreateObject<T>() where T : XPBaseObject
        {
            return _viewController.View.ObjectSpace.CreateObject<T>();
        }
        protected object CreateObject(Type objectType)
        {
            return _viewController.View.ObjectSpace.CreateObject(objectType);
        }

        protected void Delete(XPBaseObject deleteObject)
        {
            _viewController.View.ObjectSpace.Delete(deleteObject);
        }

        protected static T CreateObjectFromObject<T>(XPBaseObject source) where T : XPBaseObject
        {          
            var objectSpace = XPObjectSpace.FindObjectSpaceByObject(source);
            if(objectSpace != null)
                return objectSpace.CreateObject<T>();
            else
                return (T)CreateObjectFromObject(typeof(T), source);
        }
        protected static object CreateObjectFromObject(Type objectType, XPBaseObject source)
        {
            return Activator.CreateInstance(objectType, new object[] { source.Session })!;
        }

        protected static void DeleteObjectFromObject(XPBaseObject deleteObject, XPBaseObject source)
        {
            source.Session.Delete(deleteObject);
        }

        protected T Map<T>(object source)
        {
            return _mapper.Map<T>(source);
        }

        protected DevExpress.ExpressApp.DC.ITypeInfo FindTypeInfo(Type type)
        {
            return XafTypesInfo.Instance.FindTypeInfo(type);
        }

        #region Method dùng chung cho Service và ViewController

        private AutoMapper.IMapper mapper;
        private Module.Interfaces.ILogService logger;
        private Module.Interfaces.INotificationService notificationService;
        private Module.Interfaces.IUserInteractionService userInteractionService;


        protected Module.Interfaces.ILogService _logger => logger ??= Application.ServiceProvider.GetRequiredService<Module.Interfaces.ILogService>();
        protected Module.Interfaces.INotificationService _notificationService => notificationService ??= Application.ServiceProvider.GetRequiredService<Module.Interfaces.INotificationService>();
        protected AutoMapper.IMapper _mapper => mapper ??= Application.ServiceProvider.GetRequiredService<AutoMapper.IMapper>();
        protected Module.Interfaces.IUserInteractionService _userInteractionService => userInteractionService ??= Application.ServiceProvider.GetRequiredService<Module.Interfaces.IUserInteractionService>();

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
    }
}
