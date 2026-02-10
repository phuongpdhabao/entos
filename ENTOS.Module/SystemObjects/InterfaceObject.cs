﻿using System;
using System.ComponentModel;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.AuditTrail;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace ENTOS.Module.SystemObjects
{
    
    public interface IFindImageBaseObject
    {
        void SaveImage(byte[] img);
        string SearchUrl();

        string Caption();
    }

    public interface INoIndexColumn
    {

    }

    public interface ICustomHideToolbar
    {

    }
    public interface IImportFromExcel
    {

    }

    public interface INoSortingIndex
    {
        string No { get; set; }
    }
    public interface IFilterYearView
    {
        
    }
    public interface IFilterMyCompany
    {

    }

    public interface IFilterMyObject
    {

    }

    public interface IReSortItem
    { 
        void SetNoIndex(int noIndex);
    }
    public interface ILinkUnLink
    {
    }
	
	public interface IDisplay
    {
        bool Display { get; set; }
    }

    public interface IOnViewObjectSpaceCommitted
    {
        void OnViewObjectSpaceCommitted(DevExpress.ExpressApp.View view);
    }
}
