using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using ENTOS.Module.SystemObjects;


namespace ENTOS.Module.SystemControllers
{
    public partial class StatusColorViewController : ViewController
    {
        private AppearanceController appearanceController;
        private string[] statusColorProperties = null;
        public StatusColorViewController()
        {
            //TargetObjectType = typeof(ENTOS.Module.BusinessObjects.SoftwareStatus);    
            //TargetViewNesting = Nesting.Nested;
        }
		
		protected override void OnActivated()
        {
            appearanceController = Frame.GetController<AppearanceController>();
            if (appearanceController != null)
            {
                appearanceController.AppearanceApplied +=
                    new EventHandler<ApplyAppearanceEventArgs>(
                        appearanceController_AppearanceApplied);
                if (View != null && View.ObjectTypeInfo != null)
                {
                    var allowStatusColorProperties = View.ObjectTypeInfo.FindAttribute<StatusColorAttribute>();
                    if (allowStatusColorProperties != null && !string.IsNullOrEmpty(allowStatusColorProperties.TargetItems) && CheckContextIsValidated(allowStatusColorProperties.Context))
                    {
                        statusColorProperties = allowStatusColorProperties.TargetItems.Split(',');
                    }
                }
            }
        }

        void appearanceController_AppearanceApplied(
            object sender, ApplyAppearanceEventArgs e)
        {
            if (statusColorProperties != null && statusColorProperties.Length > 0 && e.ItemType == AppearanceItemType.ViewItem.ToString() && e.ContextObjects.Length > 0 && e.Item is IAppearanceFormat)
            {
                //if (e.Item is PropertyEditor)
                //{
                //    if (((PropertyEditor)e.Item).Control != null &&  ((PropertyEditor) e.Item).ControlValue is Status)
                //    {
                //        var status = (Status)((PropertyEditor)e.Item).ControlValue;
                //        if (status.Color != null)
                //            ((IAppearanceFormat)e.Item).FontColor = status.Color.Value;
                //        else ((IAppearanceFormat)e.Item).ResetFontColor();
                //    }
                //}
                //(e.ItemName == "Status")
                foreach (var statusColorProperty in statusColorProperties)
                {
                    if (statusColorProperty.Trim() == e.ItemName)
                    {
                        var status = e.ContextObjects[0].GetPropertyValue(e.ItemName) as Status;
                        if (status != null)
                        {
                            IAppearanceFormat formattedItem = e.Item as IAppearanceFormat;
                            if (formattedItem != null)
                            {
                                if (status.Color != null)
                                    formattedItem.FontColor = status.Color.Value;
                                else formattedItem.ResetFontColor();
                            }
                        }
                        break;
                    }
                }
                
            }
        }

        protected override void OnDeactivated()
        {
            if (appearanceController != null)
            {
                appearanceController.AppearanceApplied -=
                    new EventHandler<ApplyAppearanceEventArgs>(
                        appearanceController_AppearanceApplied);
            }
            base.OnDeactivated();
        }

        private bool CheckContextIsValidated(string context)
        {
            if (View is null)
                return false;
            if (string.IsNullOrEmpty(context))
                return true;
            if (View is DetailView && context.Equals("DetailView"))
                return true;
            var viewIds = context.Split(',');
            foreach (var viewId in viewIds)
            {
                if (viewId.Trim().Equals(View.Id))
                    return true;
            }
            return false;
        }

    }   
}