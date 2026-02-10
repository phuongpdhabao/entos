using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace ENTOS.Module.Helpers
{
    /// <summary>
    /// Helper mạng: ping, IP, port, email, tải file, kiểm tra mạng, ... Chỉ dùng .NET chuẩn.\nCác hàm đều có mô tả tiếng Việt cho dễ bảo trì.
    /// </summary>
    public static class NetworkHelper
    {
        /// <summary>
        /// Ping một host (trả về true nếu thành công).
        /// </summary>
        public static bool PingHost(string host, int timeout = 1000)
        {
            using var ping = new Ping();
            try { return ping.Send(host, timeout).Status == IPStatus.Success; }
            catch { return false; }
        }
        /// <summary>
        /// Ping nhiều host, trả về danh sách host thành công.
        /// </summary>
        public static System.Collections.Generic.List<string> PingMany(System.Collections.Generic.IEnumerable<string> hosts, int timeout = 1000)
        {
            return hosts.Where(h => PingHost(h, timeout)).ToList();
        }
        /// <summary>
        /// Lấy IP local đầu tiên (IPv4).
        /// </summary>
        public static string GetLocalIP()
        {
            return Dns.GetHostAddresses(Dns.GetHostName())
                .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork)?.ToString();
        }
        /// <summary>
        /// Lấy tất cả IP local (IPv4).
        /// </summary>
        public static System.Collections.Generic.List<string> GetAllLocalIPs()
        {
            return Dns.GetHostAddresses(Dns.GetHostName())
                .Where(ip => ip.AddressFamily == AddressFamily.InterNetwork)
                .Select(ip => ip.ToString()).ToList();
        }
        /// <summary>
        /// Lấy IP public (qua api ipify.org).
        /// </summary>
        public static string GetPublicIP()
        {
            try
            {
                using var wc = new WebClient();
                return wc.DownloadString("https://api.ipify.org");
            }
            catch { return null; }
        }
        /// <summary>
        /// Lấy tên máy tính (host name).
        /// </summary>
        public static string GetHostName() => Dns.GetHostName();
        /// <summary>
        /// Lấy danh sách DNS của máy tính.
        /// </summary>
        public static System.Collections.Generic.List<string> GetDnsAddresses()
        {
            return NetworkInterface.GetAllNetworkInterfaces()
                .SelectMany(nic => nic.GetIPProperties().DnsAddresses)
                .Select(ip => ip.ToString())
                .Distinct().ToList();
        }
        /// <summary>
        /// Kiểm tra port có mở không (TCP).
        /// </summary>
        public static bool IsPortOpen(string host, int port, int timeout = 1000)
        {
            try
            {
                using var client = new TcpClient();
                var result = client.BeginConnect(host, port, null, null);
                var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromMilliseconds(timeout));
                return success && client.Connected;
            }
            catch { return false; }
        }
        /// <summary>
        /// Lấy danh sách port đang mở trên host (TCP, trong dải chỉ định).
        /// </summary>
        public static System.Collections.Generic.List<int> GetOpenPorts(string host, int startPort, int endPort, int timeout = 200)
        {
            var open = new System.Collections.Generic.List<int>();
            for (int port = startPort; port <= endPort; port++)
                if (IsPortOpen(host, port, timeout)) open.Add(port);
            return open;
        }
        /// <summary>
        /// Gửi email SMTP đơn giản.
        /// </summary>
        public static void SendEmail(string smtpHost, int port, string from, string to, string subject, string body, string user = null, string pass = null, bool enableSsl = true)
        {
            using var client = new SmtpClient(smtpHost, port) { EnableSsl = enableSsl };
            if (!string.IsNullOrEmpty(user)) client.Credentials = new NetworkCredential(user, pass);
            var mail = new MailMessage(from, to, subject, body);
            client.Send(mail);
        }
        /// <summary>
        /// Download string từ url.
        /// </summary>
        public static string DownloadString(string url)
        {
            using var wc = new WebClient();
            return wc.DownloadString(url);
        }
        /// <summary>
        /// Upload string lên url (POST).
        /// </summary>
        public static string UploadString(string url, string data)
        {
            using var wc = new WebClient();
            wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            return wc.UploadString(url, data);
        }
        /// <summary>
        /// Download file từ url về local.
        /// </summary>
        public static void DownloadFile(string url, string localPath)
        {
            using var wc = new WebClient();
            wc.DownloadFile(url, localPath);
        }
        /// <summary>
        /// Upload file lên url (POST multipart).
        /// </summary>
        public static string UploadFile(string url, string filePath, string paramName = "file")
        {
            using var wc = new WebClient();
            var boundary = "----WebKitFormBoundary" + DateTime.Now.Ticks.ToString("x");
            wc.Headers[HttpRequestHeader.ContentType] = "multipart/form-data; boundary=" + boundary;
            var fileData = File.ReadAllBytes(filePath);
            var sb = new StringBuilder();
            sb.AppendLine($"--{boundary}");
            sb.AppendLine($"Content-Disposition: form-data; name=\"{paramName}\"; filename=\"{Path.GetFileName(filePath)}\"");
            sb.AppendLine("Content-Type: application/octet-stream\r\n");
            var header = Encoding.UTF8.GetBytes(sb.ToString());
            var footer = Encoding.UTF8.GetBytes($"\r\n--{boundary}--\r\n");
            var body = new byte[header.Length + fileData.Length + footer.Length];
            Buffer.BlockCopy(header, 0, body, 0, header.Length);
            Buffer.BlockCopy(fileData, 0, body, header.Length, fileData.Length);
            Buffer.BlockCopy(footer, 0, body, header.Length + fileData.Length, footer.Length);
            return Encoding.UTF8.GetString(wc.UploadData(url, body));
        }
        /// <summary>
        /// Kiểm tra có internet không (ping google).
        /// </summary>
        public static bool IsInternetAvailable()
        {
            return PingHost("8.8.8.8");
        }
        /// <summary>
        /// Lấy danh sách network interface (card mạng) trên máy.
        /// </summary>
        public static System.Collections.Generic.List<string> GetNetworkInterfaces()
        {
            return NetworkInterface.GetAllNetworkInterfaces()
                .Select(nic => nic.Name + " - " + nic.Description)
                .ToList();
        }
        /// <summary>
        /// Lấy địa chỉ MAC đầu tiên.
        /// </summary>
        public static string GetMacAddress()
        {
            return NetworkInterface.GetAllNetworkInterfaces()
                .Where(nic => nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .Select(nic => nic.GetPhysicalAddress().ToString())
                .FirstOrDefault();
        }
        /// <summary>
        /// Lấy subnet mask của IP local đầu tiên.
        /// </summary>
        public static string GetSubnetMask()
        {
            var ip = GetLocalIP();
            foreach (var nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                foreach (var ua in nic.GetIPProperties().UnicastAddresses)
                {
                    if (ua.Address.AddressFamily == AddressFamily.InterNetwork && ua.Address.ToString() == ip)
                        return ua.IPv4Mask.ToString();
                }
            }
            return null;
        }
        /// <summary>
        /// Lấy gateway mặc định của máy.
        /// </summary>
        public static string GetGateway()
        {
            foreach (var nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                var gw = nic.GetIPProperties().GatewayAddresses.FirstOrDefault();
                if (gw != null) return gw.Address.ToString();
            }
            return null;
        }
        /// <summary>
        /// Lấy danh sách DNS server.
        /// </summary>
        public static System.Collections.Generic.List<string> GetDnsServers()
        {
            return NetworkInterface.GetAllNetworkInterfaces()
                .SelectMany(nic => nic.GetIPProperties().DnsAddresses)
                .Select(ip => ip.ToString())
                .Distinct().ToList();
        }
    }
} 