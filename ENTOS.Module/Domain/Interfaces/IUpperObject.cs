using DevExpress.ExpressApp.DC;
using ENTOS.Module.BusinessObjects;

namespace ENTOS.Module.BusinessObjects
{
	public interface IUpperObject
    {
		System.Type SystemType
		{get;set;}
		System.Guid? ObjectID
		{get;set;}

    }

}