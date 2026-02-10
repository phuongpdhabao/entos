﻿using System;
using System.ComponentModel;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.AuditTrail;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace ENTOS.Module.SystemObjects
{
    //Cross Module
    [NonPersistent]
    [LookupEditorMode(LookupEditorMode.AllItemsWithSearch)]
    [ModelDefault("ShowCaption", "False")]
    [Appearance("Hide PermissionPolicyUser", TargetItems = "AuditTrail",
        Criteria = "!DisplayAuditTrail", Visibility = ViewItemVisibility.Hide,
        Context = "DetailView")]
    public abstract class HbBaseObject : DevExpress.Persistent.BaseImpl.BaseObject
    {
        public HbBaseObject(Session session)
            : base(session)
        {
        }

        public override void AfterConstruction()
        {
            //if (SecuritySystem.CurrentUser is DevExpress.Persistent.BaseImpl.PermissionPolicy.PermissionPolicyUser)
            //{
            //    Owner =
            //        ((DevExpress.Persistent.BaseImpl.PermissionPolicy.PermissionPolicyUser)SecuritySystem.CurrentUser)
            //        .UserName;
            //}
        }

        protected override void OnSaving()
        {
            ////Time Edit
            //CriteriaOperator funcNow = new FunctionOperator(FunctionOperatorType.Now);
            //UpdateDateTime = (DateTime)Session.Evaluate(typeof(XPObjectType), funcNow, null);
            ////User Edit
            //if (SecuritySystem.CurrentUser is DevExpress.Persistent.BaseImpl.PermissionPolicy.PermissionPolicyUser)
            //{
            //    Editor =
            //        ((DevExpress.Persistent.BaseImpl.PermissionPolicy.PermissionPolicyUser)SecuritySystem.CurrentUser)
            //        .UserName;
            //}
            base.OnSaving();
        }

        public void OnChangedField(string field)
        {
            OnChanged(field);
        }

        //UpdateUser
        //[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        //[ModelDefault("Caption", "Cập nhật")]
        //public DateTime UpdateDateTime { get; set; }

        //[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        //[ModelDefault("Caption", "Chỉnh sửa bởi")]
        //public string Editor { get; set; }

        //[Browsable(false)]
        //public string Owner { get; set; }

        [Browsable(false)]
        public static bool IsSleepOnLoad { get; set; }

        private XPCollection<AuditDataItemPersistent> auditTrail;

        [DetailViewLayoutAttribute("Tabs", LayoutGroupType.TabbedGroup, 5)]
        [VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
        [ModelDefault("Caption", "Log")]
        public XPCollection<AuditDataItemPersistent> AuditTrail
        {
            get
            {
                if (auditTrail == null)
                {
                    auditTrail = AuditedObjectWeakReference.GetAuditTrail(Session, this);
                }
                return auditTrail;
            }
        }
        private bool _displayAuditTrail;
        [Browsable(false)]
        [NonPersistent]
        public bool DisplayAuditTrail
        {
            get { return _displayAuditTrail; }
            set { SetPropertyValue("DisplayAuditTrail", ref _displayAuditTrail, value); }
        }

    }

    public class MSSqlServerTimestampStrategy : IAuditTimestampStrategy
    {
        DateTime cachedTimeStamp;

        #region IAuditTimestampStrategy Members

        public DateTime GetTimestamp(AuditDataItem auditDataItem)
        {
            return cachedTimeStamp;
        }

        public void OnBeginSaveTransaction(Session session)
        {
            cachedTimeStamp = (DateTime) session.ExecuteScalar("select getdate()");
        }

        #endregion
    }
    

    public class ContactsObject
    {
        public string title { get; set; }
        public string email { get; set; }
        public string im { get; set; }
    }

}
