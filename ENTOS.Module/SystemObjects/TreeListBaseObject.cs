﻿using System.ComponentModel;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace ENTOS.Module.SystemObjects
{
    public abstract class TreeListBaseObject : BaseObject, ITreeNode
    {

        private string name;
        protected abstract ITreeNode Parent
        {
            get;
        }
        protected abstract IBindingList Children
        {
            get;
        }

        protected abstract string Name
        {
            get;
        }
        public TreeListBaseObject(Session session) : base(session) { }


        #region ITreeNode
        IBindingList ITreeNode.Children
        {
            get
            {
                return Children;
            }
        }
        string ITreeNode.Name
        {
            get
            {
                return name;
            }
        }
        ITreeNode ITreeNode.Parent
        {
            get
            {
                return Parent;
            }
        }
        #endregion
    }
}
