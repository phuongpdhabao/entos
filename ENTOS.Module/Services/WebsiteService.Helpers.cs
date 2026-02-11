namespace ENTOS.Module.Services
{
    public partial class WebsiteService
    {
        #region URL Helpers

        private static string BuildAdminMenuUrl(string baseUrl, int menuId)
        {
            string url = baseUrl;
            if (!url.EndsWith("/"))
            {
                url += "/";
            }
            url += "wp-admin/nav-menus.php?action=edit&menu=" + menuId.ToString("D");
            return url;
        }

        private static bool IsValidUrl(string url)
        {
            return !string.IsNullOrEmpty(url);
        }

        #endregion

        #region Validation Helpers

        private static bool ValidateWordPressCredentials(string url, string user, string key)
        {
            if (string.IsNullOrEmpty(url))
            {
                return false;
            }
            if (string.IsNullOrEmpty(user))
            {
                return false;
            }
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }
            return true;
        }

        private static bool ValidateWooCommerceCredentials(string user, string key, bool useWooCommerce)
        {
            if (!useWooCommerce)
            {
                return true;
            }
            if (string.IsNullOrEmpty(user))
            {
                return false;
            }
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }
            return true;
        }

        #endregion

        #region Menu Helpers

        private static string GetMenuType(string objType)
        {
            return objType == "page" ? "post_type" : "taxonomy";
        }

        private static string BuildMenuJson(string menuName, string menuType, int parentId, int menuOrder, string objType, int objectId, int menuId, int? mediaItemId)
        {
            var builder = new System.Text.StringBuilder("{");
            builder.Append("\"title\":{\"rendered\": \"" + menuName + "\"}");
            builder.Append(",\"type\":\"" + menuType + "\"");
            builder.Append(",\"parent\":" + parentId.ToString("D"));
            builder.Append(",\"menu_order\":" + menuOrder.ToString("D"));
            builder.Append(",\"object\":\"" + objType + "\"");
            builder.Append(",\"object_id\":" + objectId.ToString("D"));
            builder.Append(",\"menus\":" + menuId.ToString("D"));
            builder.Append("}");
            return builder.ToString();
        }

        #endregion

        #region Progress Helpers

        private static string FormatProgress(int count, int total)
        {
            return (System.Convert.ToDecimal(count) / total).ToString("p0");
        }

        #endregion

        #region Choice Action Helpers

        private static bool ChoiceContainsProduct(string choiceId)
        {
            return choiceId.Contains("product");
        }

        private static bool ChoiceContainsCategory(string choiceId)
        {
            return choiceId.Contains("category");
        }

        private static bool ChoiceContainsProductCat(string choiceId)
        {
            return choiceId.Contains("product_cat");
        }

        private static bool ChoiceContainsPage(string choiceId)
        {
            return choiceId.Contains("page");
        }

        private static bool ChoiceContainsMenu(string choiceId)
        {
            return choiceId.Contains("menu");
        }

        private static bool IsEditMenuChoice(string choiceId)
        {
            return choiceId.Equals("Edit_menu");
        }

        #endregion

        #region Image Helpers

        private static bool CheckEqualMedia_CompareSize(int imageByteLength, int mediaSize)
        {
            return imageByteLength == mediaSize;
        }

        #endregion

        #region JSON Helpers

        private static int GetIntFromJObject(Newtonsoft.Json.Linq.JObject obj, string key)
        {
            var value = obj.GetValue(key);
            if (value != null)
            {
                return System.Convert.ToInt32(value);
            }
            return 0;
        }

        private static string GetStringFromJObject(Newtonsoft.Json.Linq.JObject obj, string key)
        {
            return obj.GetValue(key)?.ToString();
        }

        #endregion
    }
}
