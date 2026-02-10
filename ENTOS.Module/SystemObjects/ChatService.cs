using DevExpress.ExpressApp;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ENTOS.Module.SystemObjects
{
    public class ChatService
    {
        private HubConnection _hubConnection;
        private readonly string _serverUrl;
        private string _apiKey;
        private XafApplication _application;

        public event Action<string> AllReceiveMessage;
        public event Action<string, string> AllReceiveMessageFromUser;
        public event Action<string, string, string> PrivateMessageReceived;
        public event Action<string, string, string> GroupReceiveMessage;

        public ChatService(DevExpress.Xpo.Session session, XafApplication application)
        {
            _serverUrl = Module.Helpers.ParameterHelper.GetValueOrDefault(session, "ChatServer", "https://entos.habao.vn/chathub");
            _apiKey = Module.Helpers.ParameterHelper.GetValueOrDefault(session, "ChatApiKey", "");
            _application = application;
        }
    
        //public async Task<bool> LoginAsync(string username, string password)
        //{
        //    //Hiện tại cấu trúc từ client không hỗ rtợ
        //    try
        //    {
        //        var loginData = new
        //        {
        //            Username = username,
        //            Password = password
        //        };

        //        var content = new StringContent(
        //        JsonConvert.SerializeObject(loginData),
        //        Encoding.UTF8,
        //            "application/json");

        //        var response = await _httpClient.PostAsync($"{_serverUrl}/api/Token/get", content);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            var result = await response.Content.ReadAsStringAsync();
        //            var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(result);
        //            _token = tokenResponse.Token;
        //            return true;
        //        }
        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Login error: {ex.Message}");
        //        return false;
        //    }
        //}

        public async Task StartConnectionAsync()
        {

            _hubConnection = new HubConnectionBuilder()
                .WithUrl(_serverUrl, options =>
                {
                    options.Headers.Add("X-Api-Key", _apiKey);
                    options.Headers.Add("X-User-Id", SecuritySystem.CurrentUserName);
                })
                .WithAutomaticReconnect()
                .Build();

            // Đăng ký các sự kiện nhận tin nhắn
            _hubConnection.On<string>("AllReceiveMessage", message =>
            {
                AllReceiveMessage?.Invoke(message);
            });

            _hubConnection.On<string, string>("AllReceiveMessageFromUser", (fromUser, message) =>
            {
                AllReceiveMessageFromUser?.Invoke(fromUser, message);
            });

            _hubConnection.On<string, string, string>("PrivateMessageReceived", (toUserId, fromUser, message) =>
            {
                PrivateMessageReceived?.Invoke(toUserId, fromUser, message);
            });
            _hubConnection.On<string, string, string>("GroupReceiveMessage", (groupId, fromUser, message) =>
            {
                GroupReceiveMessage?.Invoke(groupId, fromUser, message);
            });

            // Đăng ký nhiều event handler tùy nhu cầu

            await _hubConnection.StartAsync();
        }

        public async Task SendMessageToAllAsync(string message)
        {
            if (_hubConnection?.State == HubConnectionState.Connected)
            {
                await _hubConnection.InvokeAsync("SendMessageToAll", message);
            }
        }

        public async Task UserSendMessageToAllAsync(string fromUser, string message)
        {
            if (_hubConnection?.State == HubConnectionState.Connected)
            {
                await _hubConnection.InvokeAsync("UserSendMessageToAll", fromUser, message);
            }
        }
        public async Task SendPrivateMessageAsync(string toUserId, string fromUser, string messageText)
        {
            if (_hubConnection?.State == HubConnectionState.Connected)
            {
                await _hubConnection.InvokeAsync("SendPrivateMessage", toUserId, fromUser, messageText);
            }
        }
        public async Task SendGroupMessageAsync(string groupId, string fromUser, string messageText)
        {
            if (_hubConnection?.State == HubConnectionState.Connected)
            {
                await _hubConnection.InvokeAsync("SendGroupMessage", groupId, fromUser, messageText);
            }
        }

        public async Task JoinGroupAsync(string groupId)
        {
            if (_hubConnection?.State == HubConnectionState.Connected)
            {
                await _hubConnection.InvokeAsync("JoinGroup", groupId);
            }
        }

        public async Task LeaveGroupAsync(string sessionId)
        {
            if (_hubConnection?.State == HubConnectionState.Connected)
            {
                await _hubConnection.InvokeAsync("LeaveGroup", sessionId);
            }
        }

        public async Task StopConnectionAsync()
        {
            if (_hubConnection != null)
            {
                await _hubConnection.StopAsync();
                await _hubConnection.DisposeAsync();
            }
        }

        public bool IsConnected => _hubConnection?.State == HubConnectionState.Connected;
    }

    //public class TokenResponse
    //{
    //    public string Token { get; set; }
    //}

}
