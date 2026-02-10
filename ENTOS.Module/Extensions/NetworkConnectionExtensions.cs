using ENTOS.Module.SystemObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace ENTOS.Module.Extensions
{
    public class NetworkConnectionExtensions : NetworkConnection
    {
        public NetworkConnectionExtensions(string networkName, string username, string password) : base(networkName, new NetworkCredential(username, password))
        {
        }

        public NetworkConnectionExtensions(string networkName, DevExpress.ExpressApp.IObjectSpace objectSpace) : base(networkName, new NetworkCredential(Module.Helpers.ParameterHelper.GetParameterValueOrDefault(objectSpace, "FileServerUser", "null").Value, Module.Helpers.ParameterHelper.GetParameterValueOrDefault(objectSpace, "FileServerPassword", "null").Value))
        {
        }
    }
}
