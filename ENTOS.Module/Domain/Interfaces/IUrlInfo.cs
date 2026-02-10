using DevExpress.ExpressApp.DC;
using ENTOS.Module.BusinessObjects;

namespace ENTOS.Module.BusinessObjects
{
	public interface IUrlInfo
    {
		string URL
		{get;set;}
		string Name
		{get;set;}
		byte[] Image
		{get;set;}

    }

}