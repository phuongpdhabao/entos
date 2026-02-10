using DevExpress.ExpressApp.DC;
using ENTOS.Module.BusinessObjects;

namespace ENTOS.Module.BusinessObjects
{
	public interface IProcessManagementService
    {

		        bool OpenFile(string path);
        bool OpenFile(string path, string arguments);
        bool OpenFolder(string filePath);
        bool RunCommandFromOtherComputer(string command, string server, string username, string password);
    }

}