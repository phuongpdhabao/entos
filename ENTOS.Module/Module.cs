using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ApplicationBuilder;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.DC.Xpo;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.Adapters;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace ENTOS.Module {
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppModuleBasetopic.aspx.
    public sealed partial class ENTOSModule : ModuleBase {
        public ENTOSModule() {
            InitializeComponent();
			BaseObject.OidInitializationMode = OidInitializationMode.AfterConstruction;
        }
        public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB) {
            ModuleUpdater updater = new DatabaseUpdate.Updater(objectSpace, versionFromDB);
            return new ModuleUpdater[] { updater };
        }
        public override void Setup(XafApplication application) {
            base.Setup(application);
            //application.CreateCustomObjectSpaceProvider += application_CreateCustomObjectSpaceProvider;
            //(application.Security as SecurityStrategy)?.RegisterXPOAdapterProviders(new SecurityPermissionsProviderDefault(application));
            //application.ObjectSpaceCreated += Application_ObjectSpaceCreated;
            // Manage various aspects of the application UI and behavior at the module level.
            application.LoggedOn += application_LoggedOn;
  
        }
        //Notifications
        private void application_LoggedOn(object sender, LogonEventArgs e)
        {
            try
            {
                var notificationsModule = Application.Modules.FindModule<DevExpress.ExpressApp.Notifications.NotificationsModule>();
                if (notificationsModule != null)
                {
                    //notificationsModule.NotificationsRefreshInterval = TimeSpan.FromMinutes(5);
                    //notificationsModule.NotificationsStartDelay = TimeSpan.FromMinutes(5);
                    //notificationsModule.ShowRefreshAction = true;  
                    //notificationsModule.ShowNotificationsWindow = false;
                    notificationsModule.ShowDismissAllAction = true;
                    //notificationsModule.ShowNotificationsWindow = false;
                    var notificationsProvider = notificationsModule.DefaultNotificationsProvider;
                    notificationsProvider.CustomizeNotificationCollectionCriteria +=
                        notificationsProvider_CustomizeNotificationCollectionCriteria;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void notificationsProvider_CustomizeNotificationCollectionCriteria(
            object sender, DevExpress.Persistent.Base.General.CustomizeCollectionCriteriaEventArgs e)
        {
            if (e.Type == typeof(Module.SystemObjects.UserNotifications))
            {
                var criteriaOperator =
                    DevExpress.Data.Filtering.CriteriaOperator.Parse("CurrentUserId = CurrentUserId()");
                e.Criteria = criteriaOperator;
            }
        }
  
        private void Application_ObjectSpaceCreated(object sender, ObjectSpaceCreatedEventArgs e)
        {
            (e.ObjectSpace as CompositeObjectSpace)?.PopulateAdditionalObjectSpaces((XafApplication)sender);
        }

        //public override void CustomizeTypesInfo(ITypesInfo typesInfo) {
        //    base.CustomizeTypesInfo(typesInfo);
        //    CalculatedPersistentAliasHelper.CustomizeTypesInfo(typesInfo);
        //}
        public override void CustomizeTypesInfo(ITypesInfo typesInfo)
        {
            base.CustomizeTypesInfo(typesInfo);

              
            CalculatedPersistentAliasHelper.CustomizeTypesInfo(typesInfo);
        }

 
    }
}
