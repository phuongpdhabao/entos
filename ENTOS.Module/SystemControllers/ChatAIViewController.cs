using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Xpo;
using OpenAI.Chat;
using static System.Net.Mime.MediaTypeNames;
using ListView = DevExpress.ExpressApp.ListView;


namespace ENTOS.Module.SystemControllers
{
    public partial class ChatAIViewController : ViewController<ListView>
    {

        public ChatAIViewController()
        {
            InitializeComponent();
            //TargetViewType = ViewType.ListView;
        }
        protected override void OnActivated()
        {
            base.OnActivated();
        }


        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.         
            base.OnDeactivated();
        }


        private IDictionary<string, string> conditionsDictionary = null;
        private IObjectSpace _ChatAIObjectObjectSpace = null;
        private IObjectSpace GetChatAIObjectObjectSpace()
        {
            if (_ChatAIObjectObjectSpace is null)
                _ChatAIObjectObjectSpace = Application.CreateObjectSpace(typeof(Module.SystemObjects.ChatAI));
            return _ChatAIObjectObjectSpace;
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            if (ActionChatAIData != null && ActionChatAIData.Items.Count == 0 &&
                View is ListView && View.ObjectTypeInfo != null && View.ObjectTypeInfo.Type.IsSubclassOf(typeof(PersistentBase)))
            {
                var ChatAIObjects = GetChatAIObjectObjectSpace().GetObjects<Module.SystemObjects.ChatAI>(CriteriaOperator.Parse(
                    "Active = True and ObjectType = ? and (IsNullOrEmpty(Trim(ViewId)) or ViewId = ?) ",
                    View.ObjectTypeInfo.Type, View.Id));
                if (ChatAIObjects.Count > 0)
                {
                    var items = ChatAIObjects.OrderBy(m => m.Name);
                    ActionChatAIData.Items.Clear();
                    if (conditionsDictionary == null)
                    {
                        conditionsDictionary = new Dictionary<string, string>();
                    }
                    foreach (var ChatAIObject in items)
                    {
                        if ((View.AllowEdit || View.Id.Equals(ChatAIObject.ViewId) || string.IsNullOrEmpty(ChatAIObject.ViewId) || ChatAIObject.AutoSave) && ChatAIObject.InputField1 != null &&
                            ChatAIObject.InputField1.Value is string && !string.IsNullOrEmpty((string)ChatAIObject.InputField1.Value)
                            && ChatAIObject.ResultField != null &&
                            ChatAIObject.ResultField.Value is string && !string.IsNullOrEmpty((string)ChatAIObject.ResultField.Value)
                             && !string.IsNullOrEmpty((string)ChatAIObject.Content))
                        {
                            ActionChatAIData.Items.Add(new ChoiceActionItem(ChatAIObject.Oid.ToString(),
                                           ChatAIObject.Name, ChatAIObject.Oid));
                        }
                    }
                }
            }
        }

        private void ActionChatAIData_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if (View is null)
                return;
            //if (View.ObjectTypeInfo != null && (e.SelectedChoiceActionItem.Caption.EndsWith(defaultText) || e.SelectedChoiceActionItem.Caption.EndsWith(setNullText) || e.SelectedChoiceActionItem.Caption.EndsWith(setNullAndSetDefaultText)))
            var ChatAIObject = GetChatAIObjectObjectSpace().FindObject<Module.SystemObjects.ChatAI>(CriteriaOperator.Parse("Oid =?", Guid.Parse(e.SelectedChoiceActionItem.Id)));
            if (ChatAIObject is null)
                return;
            OpenAI.Chat.ChatClient chatClient = null;
            if (View.ObjectTypeInfo != null)
            {
                var member = View.ObjectTypeInfo.FindMember(ChatAIObject.ResultField.Value as string);
                if (member is null)
                {
                    Module.Helpers.XafXpoHelper.ShowMessage(Application, "Lỗi", "Không tìm thấy trường kết quả", InformationType.Error);
                    return;
                }
                foreach (var selectedObject in View.SelectedObjects)
                {
                    var field1Value = Module.Helpers.ReflectionHelper.GetPropertyValueInObject(selectedObject, ChatAIObject.InputField1.Value as string) as string;
                    if (!string.IsNullOrEmpty((string)field1Value))
                    {
                        string field2Value = "";
                        if (ChatAIObject.InputField2 != null && ChatAIObject.InputField2.Value is string && !string.IsNullOrEmpty((string)ChatAIObject.InputField2.Value))
                            field2Value = Module.Helpers.ReflectionHelper.GetPropertyValueInObject(selectedObject, ChatAIObject.InputField2.Value as string) as string;
                        var message = string.Format(ChatAIObject.Content, field1Value, field2Value);
                        string resultText = "";
                        if (ChatAIObject.AIType == SystemObjects.AIType.Gemini)
                        {
                            //resultText = GetChatSession().SendMessageAsync(message).Result;
                            var logContent = "Chat AI (Gemini): " + message;
                            resultText = SendToGoogleGemini(message, logContent);
                        }
                        else if (ChatAIObject.AIType == SystemObjects.AIType.GoolgeSearch)
                        {
                            resultText = SendToCustomSearchAPI(message);
                        }
                        else if (ChatAIObject.AIType == SystemObjects.AIType.ChatGPT)
                        {
                            //resultText = SendToChatGpt4o(message);
                            var logContent = "Chat AI (ChatGPT): " + message;
                            OpenAI.Chat.ChatMessage[] chatMessages = new OpenAI.Chat.ChatMessage[] { OpenAI.Chat.ChatMessage.CreateUserMessage(message) };
                            resultText = Module.Utils.OpenAiUtils.OpenAITranslate(View.ObjectSpace, Application, chatMessages, logContent, ref chatClient);
                        }
                        else
                        {
                            //Chức năng này chưa hỗ trợ
                        }
                        if (!string.IsNullOrEmpty(resultText))
                        {
                            Module.Helpers.ReflectionHelper.SetPropertyValue(selectedObject, ChatAIObject.ResultField.Value as string, resultText);
                        }
                    }
                }

                CallAutoSave();
                return;
            }
        }


        private string _geminioUrl = null;
        private string GetGeminiUrl()
        {
            if (string.IsNullOrEmpty(_geminioUrl))
                _geminioUrl = Module.Helpers.ParameterHelper.GetValueOrDefault(View.ObjectSpace, "GoogleGeminiUrl", "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash-latest:generateContent?key=****");
            return _geminioUrl;
        }
        private string SendToGoogleGemini(string inputText, string logContent)
        {
            string url = GetGeminiUrl();
            inputText = inputText.Replace("\"", "\\\"");
            string content = $@"{{
                ""contents"": [
                    {{
                        ""role"": """",
                        ""parts"": [
                            {{
                                ""text"": ""{inputText}""
                            }}
                        ]
                    }}
                ],
                ""generationConfig"": {{
                    ""temperature"": 0.9,
                    ""topK"": 50,
                    ""topP"": 0.95,
                    ""maxOutputTokens"": 4096,
                    ""stopSequences"": []
                }},
                ""safetySettings"": [

                ]
            }}";
            string translation = "";
            using var postBody = new System.Net.Http.StringContent(content, System.Text.Encoding.UTF8, "application/json");
            {
                postBody.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                var resultString = GetResponseString(url, postBody).Result;
                if (!string.IsNullOrEmpty(resultString))
                {
                    var result = Newtonsoft.Json.JsonConvert.DeserializeObject(resultString) as Newtonsoft.Json.Linq.JObject;
                    var usageMetadata = result.GetValue("usageMetadata") as Newtonsoft.Json.Linq.JObject;
                    if (usageMetadata != null)
                    {
                        //Log message 
                        var promptTokenCount = usageMetadata.GetValue("promptTokenCount") as Newtonsoft.Json.Linq.JValue;
                        var candidatesTokenCount = usageMetadata.GetValue("candidatesTokenCount") as Newtonsoft.Json.Linq.JValue;

                        var objectSpace = Application.CreateObjectSpace(typeof(Module.SystemObjects.ChatGPTTokenUsage));
                        var chatGPTTokenUsage = objectSpace.CreateObject<Module.SystemObjects.ChatGPTTokenUsage>();
                        chatGPTTokenUsage.InputTokens = System.Convert.ToInt32(promptTokenCount.Value);
                        chatGPTTokenUsage.OutputTokens = System.Convert.ToInt32(candidatesTokenCount.Value);
                        if (logContent.Length > 200)
                            logContent = logContent.Substring(0, 200);
                        chatGPTTokenUsage.Content = logContent;
                        var model = url.Substring(url.IndexOf("models/") + 7);
                        if (!string.IsNullOrEmpty(model))
                            model = model.Substring(0, model.IndexOf(":"));
                        chatGPTTokenUsage.AIModel = model;
                        chatGPTTokenUsage.Session.CommitTransaction();
                    }
                    var candidates = result.GetValue("candidates");
                    if (candidates != null)
                    {
                        var candidatesObj = candidates as Newtonsoft.Json.Linq.JArray;
                        var contentText = ((Newtonsoft.Json.Linq.JObject)candidatesObj[0]).GetValue("content");
                        var contentObj = contentText as Newtonsoft.Json.Linq.JObject;
                        var partsText = contentObj.GetValue("parts");
                        var partsObj = partsText as Newtonsoft.Json.Linq.JArray;
                        var textText = ((Newtonsoft.Json.Linq.JObject)partsObj[0]).GetValue("text");
                        //var re = textText.GetType();
                        var textObj = textText as Newtonsoft.Json.Linq.JValue;
                        return textObj.Value as string;
                    }

                    //if (result != null && _menuItemList != null)
                    //    _menuItemList.Add(result);
                    //return result;
                }
            }
            return translation;
        }

        private string _customSearchUrl = null;
        private string GetCustomSearchUrl()
        {
            if (string.IsNullOrEmpty(_customSearchUrl))
                _customSearchUrl = Module.Helpers.ParameterHelper.GetValueOrDefault(View.ObjectSpace, "GoogleCustomSearchUrl", "https://www.googleapis.com/customsearch/v1?key=AI***_eE&cx=b4196acc9f8ac42bd&q={0}");
            return _customSearchUrl;
        }
        private string SendToCustomSearchAPI(string inputText)
        {
            string googleAPIKey = "AI***_eE";
            string url = string.Format(GetCustomSearchUrl(), inputText);
            string translation = "";
            var resultString = GetResponseString(url, null).Result;
            var dicResult = Newtonsoft.Json.JsonConvert.DeserializeObject<System.Collections.Generic.Dictionary<string, object>>(resultString);
            if (dicResult.ContainsKey("items"))
            {
                var items = dicResult["items"] as System.Collections.IEnumerable;
                foreach (Newtonsoft.Json.Linq.JObject item in items)
                {
                    if (item.ContainsKey("snippet"))
                    {
                        var objectSnippet = item.GetValue("snippet") as Newtonsoft.Json.Linq.JValue;
                        if (objectSnippet != null) return objectSnippet.Value as string;
                    }
                }
            }
            return translation;
        }

        private async System.Threading.Tasks.Task<string> GetResponseString(string route, System.Net.Http.HttpContent postBody = null)
        {
            using (var requestMessage = new System.Net.Http.HttpRequestMessage(postBody is null ? System.Net.Http.HttpMethod.Get : System.Net.Http.HttpMethod.Post, route))
            {
                var httpClient = new System.Net.Http.HttpClient();
                //httpClient.BaseAddress = new System.Uri(route);
                if (postBody != null)
                {
                    requestMessage.Content = postBody;
                }
                var response = await httpClient.SendAsync(requestMessage).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    //string responseBody = await response.Content.ReadAsStringAsync();
                    //return responseBody.Substring(responseBody.IndexOf("\"text\": \"") + 9, responseBody.IndexOf("\"", responseBody.IndexOf("\"text\": \"") + 10) - responseBody.IndexOf("\"text\": \"") - 9);
                }
            };
            return null;
        }

        //private GenerativeAI.Methods.ChatSession _chatSession = null;
        //private GenerativeAI.Methods.ChatSession GetChatSession()
        //{
        //    if(_chatSession is null)
        //    {
        //        var geminiKey = Module.Helpers.ParameterHelper.GetValueOrDefault(View.ObjectSpace, "GoogleGeminiKey", "AI****8");
        //        var generativeModel = new GenerativeAI.Models.GenerativeModel(geminiKey);
        //        _chatSession = generativeModel.StartChat(new GenerativeAI.Types.StartChatParams());
        //    }            
        //    return _chatSession;
        //}


        private string _chatGpt4oNonces = null;
        private string GetChatGpt4oNonces()
        {
            if (string.IsNullOrEmpty(_chatGpt4oNonces))
                _chatGpt4oNonces = Module.Helpers.ParameterHelper.GetValueOrDefault(View.ObjectSpace, "ChatGpt4oNonces", "c996ba75ad");
            return _chatGpt4oNonces;
        }

        private string _chatGpt4oUrl = null;
        private string GetChatGpt4oUrl()
        {
            if (string.IsNullOrEmpty(_chatGpt4oUrl))
                _chatGpt4oUrl = Module.Helpers.ParameterHelper.GetValueOrDefault(View.ObjectSpace, "ChatGpt4oUrl", "https://chatgpt4o.one/wp-admin/admin-ajax.php?_wpnonce={0}&post_id=11&action=wpaicg_chat_shortcode_message&message={1}");
            return _chatGpt4oUrl;
        }
        private string SendToChatGpt4o(string inputText)
        {
            var nonces = GetChatGpt4oNonces();
            string url = string.Format(GetChatGpt4oUrl(), GetChatGpt4oNonces(), System.Web.HttpUtility.UrlEncodeUnicode(inputText));
            string translation = "";
            using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
            {
                //HttpClient httpClient = new HttpClient();
                string result = client.GetStringAsync(url).Result;
                //var jsonData = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<List<dynamic>>(result);
                var jsonData = Newtonsoft.Json.JsonConvert.DeserializeObject<System.Collections.Generic.List<dynamic>>(result);
                // Extract just the first array element (This is the only data we are interested in)
                if (jsonData[0] is null)
                    return null;
                var translationItems = jsonData[0];

                foreach (object item in translationItems)
                {
                    System.Collections.IEnumerable translationLineObject = item as System.Collections.IEnumerable;
                    if (translationLineObject != null)
                    {
                        System.Collections.IEnumerator translationLineString = translationLineObject.GetEnumerator();
                        translationLineString.MoveNext();
                        translation += string.Format(" {0}", System.Convert.ToString(translationLineString.Current));
                    }
                }

            }
            return translation;
        }

        private void CallAutoSave()
        {
            try
            {
                if (ActionChatAIData.SelectedItem != null && ActionChatAIData.SelectedItem.Id != null)
                {
                    var ChatAIObject = GetChatAIObjectObjectSpace().GetObjectByKey<Module.SystemObjects.ChatAI>(Guid.Parse(ActionChatAIData.SelectedItem.Id));
                    if (ChatAIObject != null && ChatAIObject.AutoSave)
                    {
                        ObjectSpace.CommitChanges();
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.ActionChatAIData = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            // 
            // ActionChatAIData
            // 
            this.ActionChatAIData.Caption = "ChatAI";
            this.ActionChatAIData.Category = "Edit";
            this.ActionChatAIData.ConfirmationMessage = null;
            this.ActionChatAIData.Id = "ChatAI";
            this.ActionChatAIData.ImageName = "ActionChatAIData";
            this.ActionChatAIData.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.ActionChatAIData.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;
            this.ActionChatAIData.TargetObjectsCriteria = "";
            this.ActionChatAIData.TargetViewId = "";
            this.ActionChatAIData.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.ActionChatAIData.ToolTip = null;
            this.ActionChatAIData.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.ActionChatAIData.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.ActionChatAIData_Execute);
            // 
            // PopupControlEditMultiViewController
            // 
            this.Actions.Add(this.ActionChatAIData);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SingleChoiceAction ActionChatAIData;
    }
}