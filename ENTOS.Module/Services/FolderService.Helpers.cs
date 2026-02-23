using System;
using ENTOS.Module.SystemObjects;

namespace ENTOS.Module.Services
{
    public partial class FolderService
    {
        private static bool TryGetFolderOid(string data, out Guid oid)
        {
            string[] parts = data.Split(new[] { '{', '}' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length > 0)
            {
                string oidString = parts[1];
                return Guid.TryParse(oidString, out oid);
            }
            oid = Guid.Empty;
            return false;
        }

        private static string ResolveFolderCriteria(Type objectType)
        {
            string data = "MemberFolder.Oid = ?";
            var listAttribute = objectType.GetCustomAttributes(typeof(CustomFilter), true);
            foreach (CustomFilter customAttribute in listAttribute)
            {
                if (customAttribute.Name.Equals("IFolder"))
                {
                    data = customAttribute.Criteria;
                    break;
                }
            }
            return data;
        }
    }
}
