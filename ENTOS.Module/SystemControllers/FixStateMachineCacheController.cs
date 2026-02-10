using System;
using System.Collections;
using System.Collections.Generic;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.StateMachine;

namespace ENTOS.Module.SystemControllers
{
    public class FixStateMachineCacheController : StateMachineCacheController
    {
        public override IList<IStateMachine> GetStateMachinesByType(Type targetObjectType)
        {
            this.EnsureCache();
            List<IStateMachine> stateMachineList = new List<IStateMachine>();
            foreach (IStateMachine stateMachine in this.cache)
            {
                if (stateMachine.Active && stateMachine.TargetObjectType.IsAssignableFrom(targetObjectType))
                    stateMachineList.Add(stateMachine);
            }

            return (IList<IStateMachine>) stateMachineList;
        }

        private bool isLoading;

        private void EnsureCache()
        {
            if (this.isLoading)
                return;
            this.isLoading = true;
            try
            {
                if (this.isCompleteCache)
                    return;
                IObjectSpace objectSpaceForCache = this.GetObjectSpaceForCache();
                if (!objectSpaceForCache.CanInstantiate(this.StateMachineStorageType))
                    return;
                IList objects = objectSpaceForCache.GetObjects(this.StateMachineStorageType, (CriteriaOperator) null);
                if (objects == null)
                    return;
                foreach (IStateMachine stateMachine in (IEnumerable) objects)
                {
                    if (stateMachine.TargetObjectType != null)
                        this.cache.Add(stateMachine);
                }

                this.isCompleteCache = true;
            }
            finally
            {
                this.isLoading = false;
            }
        }
    }
}