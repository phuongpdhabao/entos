using DevExpress.ExpressApp;
using ENTOS.Module.SystemObjects;



namespace ENTOS.Module.Helpers
{
    public static partial class ImageHelper
    {
        public static byte[] RemoveBackgroundApi(byte[] images, string api = null, IObjectSpace objectSpace = null)
        {
            if (string.IsNullOrEmpty(api))
            {
                api = Module.Helpers.ParameterHelper.GetValueOrDefault(objectSpace, "ApiKeyRemove.bg", "thdpo4psPsDX4LCEYk9171SD");
            }
            if (string.IsNullOrEmpty(api))
                return null;
            using (var client = new System.Net.Http.HttpClient())
            using (var formData = new System.Net.Http.MultipartFormDataContent())
            {
                formData.Headers.Add("X-Api-Key", api);
                formData.Add(new System.Net.Http.ByteArrayContent(images), "image_file", "file.png");
                formData.Add(new System.Net.Http.StringContent("auto"), "size");
                var response = client.PostAsync("https://api.remove.bg/v1.0/removebg", formData).Result;

                if (response.IsSuccessStatusCode)
                {
                    //FileStream fileStream = new FileStream("no-bg.png", FileMode.Create, FileAccess.Write, FileShare.None);                    
                    //response.Content.CopyToAsync(fileStream).ContinueWith((copyTask) => { fileStream.Close(); });
                    using (MemoryStream stream = new MemoryStream())
                    {
                        byte[] result = null;
                        var task = response.Content.CopyToAsync(stream);
                        int wait = 0;
                        while (!task.IsCompleted && wait < 10)
                        {
                            task.Wait(1000);
                            wait++;
                        }
                        if (task.IsCompleted)
                        {
                            result = stream.ToArray();
                        }
                        return result;
                    }
                }
                else
                {
                    Console.WriteLine("Error: " + response.Content.ReadAsStringAsync().Result);
                    return null;
                }
            }
            return null;
        }
    }
}
