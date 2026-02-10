using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Net;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace ENTOS.Module.SystemObjects
{

    public class NetworkConnection : IDisposable
    {
        string _networkName;

        public NetworkConnection(string networkName, NetworkCredential credentials, bool tryForceDisconnectOnConflict = true)
        {
            _networkName = networkName;

            string serverName = GetServerName(networkName);
            if (!string.IsNullOrEmpty(serverName))
            {
                // Hủy kết nối tới \\server nếu có
                WNetCancelConnection2(serverName, 0, true);
            }
            WNetCancelConnection2(serverName, 0, true);

            var netResource = new NetResource
            {
                Scope = ResourceScope.GlobalNetwork,
                ResourceType = ResourceType.Disk,
                DisplayType = ResourceDisplaytype.Share,
                RemoteName = networkName
            };

            var result = WNetAddConnection2(
                netResource,
                credentials.Password,
                credentials.UserName,
                0);

            if (result == 1219 && tryForceDisconnectOnConflict)
            {
                // Hủy toàn bộ kết nối mạng đang mở
                ForceDisconnectAll();

                // Thử lại
                result = WNetAddConnection2(
                    netResource,
                    credentials.Password,
                    credentials.UserName,
                    0);
            }

            if (result == 1219)
            {
                throw new InvalidOperationException(
                    $"Lỗi 1219: Đã có kết nối tới server '{serverName}' bằng tài khoản khác.\n" +
                    $"→ Hãy đóng Explorer hoặc dùng cùng tài khoản.");
            }

            if (result != 0)
            {
                throw new Win32Exception(result);
            }
        }

        private static string GetServerName(string uncPath)
        {
            var match = Regex.Match(uncPath, @"^\\\\[^\\]+");
            return match.Success ? match.Value : null;
        }

        private static void ForceDisconnectAll()
        {
            try
            {
                using (var proc = Process.Start(new ProcessStartInfo
                {
                    FileName = "net",
                    Arguments = @"use * /delete /y",
                    UseShellExecute = false,
                    CreateNoWindow = true
                }))
                {
                    proc?.WaitForExit();
                    Debug.WriteLine("[NetworkConnection] Đã hủy tất cả kết nối mạng hiện có.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[NetworkConnection] Lỗi khi hủy toàn bộ kết nối mạng: " + ex.Message);
            }
        }


        ~NetworkConnection()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            WNetCancelConnection2(_networkName, 0, true);
        }

        [DllImport("mpr.dll")]
        private static extern int WNetAddConnection2(NetResource netResource,
            string password, string username, int flags);

        [DllImport("mpr.dll")]
        private static extern int WNetCancelConnection2(string name, int flags,
            bool force);

        [StructLayout(LayoutKind.Sequential)]
        public class NetResource
        {
            public ResourceScope Scope;
            public ResourceType ResourceType;
            public ResourceDisplaytype DisplayType;
            public int Usage;
            public string LocalName;
            public string RemoteName;
            public string Comment;
            public string Provider;
        }

        public enum ResourceScope : int
        {
            Connected = 1,
            GlobalNetwork,
            Remembered,
            Recent,
            Context
        }

        public enum ResourceType : int
        {
            Any = 0,
            Disk = 1,
            Print = 2,
            Reserved = 8
        }

        public enum ResourceDisplaytype : int
        {
            Generic = 0x0,
            Domain = 0x01,
            Server = 0x02,
            Share = 0x03,
            File = 0x04,
            Group = 0x05
        }
    }
}
