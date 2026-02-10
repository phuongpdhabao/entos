using DevExpress.ExpressApp.DC;
using ENTOS.Module.BusinessObjects;

namespace ENTOS.Module.BusinessObjects
{
	public interface IMySqlManagementService
    {

		        bool ImportFromFile(string connectString, string fileName);
        bool ExportToFile(string connectString, string fileName);  
        
        bool CreateDatabase(string connectString, string name);
        bool ExecuteNonQuery(string connectString, string commandText);
    }

}