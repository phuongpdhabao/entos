namespace ENTOS.Module.Services
{
    public partial class DataServiceService
    {
        private static string GetResultTypeValue(object result)
        {
            if (result is string stringResult)
            {
                if (stringResult.StartsWith("{") && stringResult.EndsWith("}"))
                {
                    return "json";
                }
                else if (stringResult.StartsWith("<") && stringResult.EndsWith(">"))
                {
                    return "xml";
                }
                else if (stringResult.StartsWith("1") && stringResult.Contains(" --> "))
                {
                    return "srt";
                }
                else
                {
                    return "text";
                }
            }
            else if (result is byte[])
            {
                return "file";
            }
            return null;
        }
    }
}
