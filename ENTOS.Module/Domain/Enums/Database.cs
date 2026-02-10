using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum Database
    {
					[XafDisplayName("SQL Server")]
        SqlServer,
					[XafDisplayName("MySQL")]
        MySQL,
					[XafDisplayName("PostgreSQL")]
        PostgreSQL,
					[XafDisplayName("Oracle")]
        Oracle,
					[XafDisplayName("SQLite")]
        SQLite,
					[XafDisplayName("MongoDB")]
        MongoDB,
	    }

}