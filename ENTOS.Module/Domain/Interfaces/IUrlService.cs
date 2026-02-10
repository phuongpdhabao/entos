using DevExpress.ExpressApp.DC;
using ENTOS.Module.BusinessObjects;

namespace ENTOS.Module.BusinessObjects
{
	public interface IUrlService
    {

		
        void OpenUrl(string url);

        Task OpenUrlAsync(string url);
    }

}