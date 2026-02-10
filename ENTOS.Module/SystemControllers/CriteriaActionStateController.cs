using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.StateMachine;
using DevExpress.ExpressApp.StateMachine.Xpo;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Validation;

namespace ENTOS.Module.SystemControllers
{
    public class ViewHideInactiveStateMachineActionsController : ViewController<ObjectView>
    {
        private StateMachineCacheController StateMachineCacheController;
        private StateMachineController StateMachineController;

        protected override void OnActivated()
        {
            base.OnActivated();

            StateMachineCacheController = Frame.GetController<StateMachineCacheController>();

            StateMachineController = Frame.GetController<StateMachineController>();

            if (StateMachineController != null)
            {
                UpdateActionItems(StateMachineController.ChangeStateAction);

                StateMachineController.ChangeStateAction.ItemsChanged += ChangeStateAction_ItemsChanged;
            }

            ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;

            ObjectSpace.Reloaded += ObjectSpace_Reloaded;
            ObjectSpace.Committed += ObjectSpace_Reloaded;
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();

            if (StateMachineController != null)
                StateMachineController.ChangeStateAction.ItemsChanged -= ChangeStateAction_ItemsChanged;

            ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;

            ObjectSpace.Reloaded -= ObjectSpace_Reloaded;
            ObjectSpace.Committed -= ObjectSpace_Reloaded;
        }

        private void ChangeStateAction_ItemsChanged(object sender, ItemsChangedEventArgs itemsChangedEventArgs)
        {
            foreach (
                var Item in
                itemsChangedEventArgs.ChangedItemsInfo.Where(Item => Item.Value == ChoiceActionItemChangesType.Add)
                    .Select(Item => Item.Key)
                    .OfType<ChoiceActionItem>())
                UpdateActionItems(Item);
        }

        private void ObjectSpace_ObjectChanged(object sender, ObjectChangedEventArgs objectChangedEventArgs)
        {
            ObjectSpace_Reloaded(sender, EventArgs.Empty);
        }

        private void ObjectSpace_Reloaded(object sender, EventArgs e)
        {
            if (StateMachineCacheController != null)
            {
                StateMachineController.GetType()
                    .GetMethod(@"UpdateActionState",
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    .Invoke(StateMachineController, null);

                foreach (var Item in StateMachineController.ChangeStateAction.Items)
                    UpdateActionItems(Item);
            }
        }

        private void UpdateActionItems(SingleChoiceAction Action)
        {
            if (Action == null) return;

            foreach (var Item in Action.Items)
                UpdateActionItems(Item);
        }

        private void UpdateActionItems(ChoiceActionItem Action)
        {
            if (Action == null) return;

            var IsActive = this.IsActive(Action);

            Action.Active[GetType().Name] = IsActive;

            UpdatePanelActions(Action.Id, IsActive);
        }

        private void UpdatePanelActions(string itemId, bool isActive)
        {
            var DetailView = View as DetailView;

            if (DetailView != null)
            {
                var PanelActions =
                    StateMachineController.GetType()
                        .GetField(@"panelActions", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                        .GetValue(StateMachineController) as Dictionary<object, List<SimpleAction>>;

                foreach (string Key in PanelActions.Keys)
                {
                    var ActionContainerViewItem =
                        DetailView.FindItem(Key) as ActionContainerViewItem;

                    if (ActionContainerViewItem != null)
                    {
                        var Action =
                            ActionContainerViewItem.Actions.FirstOrDefault(@base => @base.Caption == itemId);

                        if (Action != null) Action.Active[GetType().Name] = isActive;
                    }
                }
            }
        }

        private bool IsActive(ChoiceActionItem choiceActionItem)
        {
            var StateMachine = choiceActionItem.Data as IStateMachine;

            if (StateMachine != null)
            {
                var BoolList = new BoolList(true, BoolListOperatorType.Or);

                BoolList.BeginUpdate();

                foreach (var Item in choiceActionItem.Items)
                {
                    var Transition = Item.Data as ITransition;

                    Item.Active[GetType().Name] = IsActive(Transition);

                    BoolList.SetItemValue(Transition.Caption, Item.Active.ResultValue);
                }

                BoolList.EndUpdate();

                return BoolList.ResultValue;
            }

            {
                var Transition = choiceActionItem.Data as ITransition;

                if (Transition != null)
                    return IsActive(Transition);
            }

            return true;
        }

        private bool IsActive(ITransition Transition)
        {           
            if (View != null && View.CurrentObject != null)
            {
                var targetObject = View.ObjectSpace.GetObject(View.CurrentObject);
                if (targetObject != null)
                {
                    var stateMachineLogic = new StateMachineLogic(ObjectSpace);
                    var resultValidate = stateMachineLogic.ValidateTransition(Transition.TargetState, targetObject
                    );
                    var result = resultValidate.State != ValidationState.Invalid;
                    return result;
                }                
            }
            return false;
        }
    }
}