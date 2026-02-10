using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.XtraGrid;
using ENTOS.Module.BusinessObjects;
using ListView = DevExpress.ExpressApp.ListView;
using ENTOS.Module.SystemObjects;
using System.Linq;

namespace ENTOS.Module.Controllers 
{
    public partial class IUpDownOrderViewController: ViewController
    {      
        
        public IUpDownOrderViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(ENTOS.Module.BusinessObjects.IUpDownOrder);    
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

        
		private void UpOrder_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;

            #region UpOrderImportCode
                            SetUpDownOrder(sender, e);
}
        private void SetUpDownOrder(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if (View is DetailView)
                return;
            string criteria = null;
            bool ascSort = true;
            bool changeBetweenRow = true;
            bool autoSave = false;
            var upDownTopBottomOrderAttribute = View.ObjectTypeInfo.FindAttribute<UpDownTopBottomOrderAttribute>();
            if (upDownTopBottomOrderAttribute != null)
            {
                criteria = upDownTopBottomOrderAttribute.Criteria;
                ascSort = upDownTopBottomOrderAttribute.AscSort;
                changeBetweenRow = upDownTopBottomOrderAttribute.ChangeBetweenRow;
                autoSave = upDownTopBottomOrderAttribute.AutoSave;
            }
            //Thực hiện từ dưới lên: Down, Top
            //Thực hiện từ trên xuống: Up, Bottom
            var selectedOrders = new System.Collections.Generic.List<Module.BusinessObjects.IUpDownOrder>();
            foreach (Module.BusinessObjects.IUpDownOrder reOrder in View.SelectedObjects)
                selectedOrders.Add(reOrder);
            if (ascSort)
            {
                if (e.SelectedChoiceActionItem.Id == "Up" || e.SelectedChoiceActionItem.Id == "Bottom")
                    selectedOrders = selectedOrders.OrderBy(m => m.Order).ToList();
                else if (e.SelectedChoiceActionItem.Id == "Down" || e.SelectedChoiceActionItem.Id == "Top")
                    selectedOrders = selectedOrders.OrderByDescending(m => m.Order).ToList();
            }
            else
            {
                if (e.SelectedChoiceActionItem.Id == "Down" || e.SelectedChoiceActionItem.Id == "Top")
                    selectedOrders = selectedOrders.OrderBy(m => m.Order).ToList();
                else if (e.SelectedChoiceActionItem.Id == "Up" || e.SelectedChoiceActionItem.Id == "Bottom")
                    selectedOrders = selectedOrders.OrderByDescending(m => m.Order).ToList();
            }

            foreach (Module.BusinessObjects.IUpDownOrder reOrder in selectedOrders)
            {
                if (reOrder.Order is null)
                    continue;
                var arrayOrder = new Module.BusinessObjects.IUpDownOrder[((ListView)View).CollectionSource.List.Count];
                if (!string.IsNullOrEmpty(criteria))
                {
                    string filterKey = "UpDownTopBottomOrderAttribute";
                    try
                    {
                        var criteriaOperator = DevExpress.Data.Filtering.CriteriaOperator.Parse(criteria, reOrder, reOrder, reOrder, reOrder, reOrder);
                        ((DevExpress.ExpressApp.ListView)View).CollectionSource.BeginUpdateCriteria();

                        if (((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria.ContainsKey(filterKey))
                        {
                            ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria.Remove(filterKey);
                        }
                        ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria[filterKey] = criteriaOperator;
                        ((DevExpress.ExpressApp.ListView)View).CollectionSource.EndUpdateCriteria();
                        arrayOrder = new Module.BusinessObjects.IUpDownOrder[((ListView)View).CollectionSource.List.Count];
                        ((ListView)View).CollectionSource.List.CopyTo(arrayOrder, 0);

                        if (((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria.ContainsKey(filterKey))
                        {
                            ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria.Remove(filterKey);
                        }
                    }
                    catch (System.Exception)
                    {
                        ((DevExpress.ExpressApp.ListView)View).CollectionSource.BeginUpdateCriteria();
                        if (((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria.ContainsKey(filterKey))
                        {
                            ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria.Remove(filterKey);
                        }
                        ((DevExpress.ExpressApp.ListView)View).CollectionSource.EndUpdateCriteria();
                        //arrayOrder = new Module.BusinessObjects.IUpDownOrder[((ListView)View).CollectionSource.List.Count];
                        ((ListView)View).CollectionSource.List.CopyTo(arrayOrder, 0);
                    }
                    finally
                    {

                    }
                }
                else
                {
                    ((ListView)View).CollectionSource.List.CopyTo(arrayOrder, 0);
                }
                if ((ascSort && e.SelectedChoiceActionItem.Id == "Up") || (!ascSort && e.SelectedChoiceActionItem.Id == "Down"))
                {
                    var orderObject = arrayOrder.Where(m => m.Order != null && m.Order < reOrder.Order).OrderByDescending(m => m.Order).FirstOrDefault();
                    if (orderObject != null)
                    {
                        var tempOrder = orderObject.Order;
                        orderObject.Order = reOrder.Order;
                        reOrder.Order = tempOrder;
                    }
                }
                else if ((ascSort && e.SelectedChoiceActionItem.Id == "Down") || (!ascSort && e.SelectedChoiceActionItem.Id == "Up"))
                {
                    var orderObject = arrayOrder.OrderBy(m => m.Order).Where(m => m.Order != null && m.Order > reOrder.Order).FirstOrDefault();
                    if (orderObject != null)
                    {
                        var tempOrder = orderObject.Order;
                        orderObject.Order = reOrder.Order;
                        reOrder.Order = tempOrder;
                    }
                }
                else if ((ascSort && e.SelectedChoiceActionItem.Id == "Top") || (!ascSort && e.SelectedChoiceActionItem.Id == "Bottom"))
                {
                    var orderObjects = arrayOrder.OrderBy(m => m.Order).Where(m => m.Order != null && m.Order < reOrder.Order).ToList();
                    if (orderObjects.Count() > 0)
                    {
                        if (changeBetweenRow)
                        {
                            var tempOrder = reOrder.Order;
                            reOrder.Order = orderObjects[0].Order;
                            for (int i = 0; i < orderObjects.Count(); i++)
                            {
                                if (i == orderObjects.Count() - 1)
                                    orderObjects[i].Order = tempOrder;
                                else
                                    orderObjects[i].Order = orderObjects[i + 1].Order;
                            }
                        }
                        else
                        {
                            reOrder.Order = orderObjects[0].Order - 1;
                        }
                    }
                }
                else if ((ascSort && e.SelectedChoiceActionItem.Id == "Bottom") || (!ascSort && e.SelectedChoiceActionItem.Id == "Top"))
                {
                    var orderObjects = arrayOrder.OrderByDescending(m => m.Order).Where(m => m.Order != null && m.Order > reOrder.Order).ToList();
                    if (orderObjects.Count() > 0)
                    {
                        if (changeBetweenRow)
                        {
                            var tempOrder = reOrder.Order;
                            reOrder.Order = orderObjects[0].Order;
                            for (int i = 0; i < orderObjects.Count(); i++)
                            {
                                if (i == orderObjects.Count() - 1)
                                    orderObjects[i].Order = tempOrder;
                                else
                                    orderObjects[i].Order = orderObjects[i + 1].Order;
                            }
                        }
                        else
                        {
                            reOrder.Order = orderObjects[0].Order + 1;
                        }

                    }
                }
            }
            if (View is ListView)
            {
                var gridView =
                    ((ListView)View).Editor.GetPropertyValue("GridView");
                if (gridView != null)
                {
                    var findRow = gridView.GetType().GetMethod("FindRow");
                    var selectRow = gridView.GetType().GetMethod("SelectRow");
                    if (findRow != null && selectRow != null)
                    {
                        foreach (var selectedOrder in selectedOrders)
                        {
                            var index = findRow.Invoke(gridView, new object[] { selectedOrder });
                            if (index is int)
                            {
                                //gridListEditor.GridView.SelectRow(index);
                                selectRow.Invoke(gridView, new object[] { index });
                            }
                        }
                    }
                }
            }
            if (System.Diagnostics.Debugger.IsAttached)
                return;
            if (autoSave)
                ObjectSpace.CommitChanges();

            #endregion UpOrderImportCode
		}
		private void DownOrder_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(View is null)
                return;

            #region DownOrderImportCode
            SetUpDownOrder(sender, e);
            #endregion DownOrderImportCode
		}
     }   
}