namespace ENTOS.Module.Services
{
    public partial class FolderService
    {
        #region MemberFolderLoad Helpers

        private static bool MemberFolderLoad_TryParseGuid(string[] parts, out System.Guid oid)
        {
            oid = System.Guid.Empty;
            if (parts.Length > 1)
            {
                return System.Guid.TryParse(parts[1], out oid);
            }
            return false;
        }

        private static string[] MemberFolderLoad_SplitData(string data)
        {
            return data.Split(new[] { '{', '}' }, System.StringSplitOptions.RemoveEmptyEntries);
        }

        private static bool MemberFolderLoad_IsAccountingType(string folderType)
        {
            return folderType == "Accounting";
        }

        #endregion

        #region CreateDefaultFilter Helpers

        private static bool CreateDefaultFilter_ShouldRemoveItem(string itemId, System.Collections.Generic.IEnumerable<Folder> listFolder)
        {
            foreach (var folder in listFolder)
            {
                if (itemId.Equals(folder.Oid.ToString()))
                {
                    return false;
                }
            }
            return true;
        }

        #endregion

        #region FindItemByID Helpers

        private static bool FindItemByID_IsMatch(string itemId, string targetId)
        {
            return itemId.Equals(targetId);
        }

        #endregion

        #region AddAllChildCriteriaOperator Helpers

        private static bool AddAllChildCriteriaOperator_IsAlreadyProcessed(System.Collections.Generic.IList<System.Guid> existedCriteria, System.Guid currentItem)
        {
            return existedCriteria.Contains(currentItem);
        }

        private static bool AddAllChildCriteriaOperator_HasChildren(System.Collections.Generic.IEnumerable<Folder> lowerFolder)
        {
            return lowerFolder != null && lowerFolder.Count() > 0;
        }

        #endregion

        #region CreateTreeSource Helpers

        private static string CreateTreeSource_BuildPrefix(string currentPrefix, string itemName)
        {
            if (string.IsNullOrEmpty(currentPrefix))
            {
                return itemName;
            }
            return currentPrefix + " > " + itemName;
        }

        private static bool CreateTreeSource_ItemExists(DevExpress.ExpressApp.Actions.ChoiceActionItem foundItem)
        {
            return foundItem != null;
        }

        #endregion

        #region Folder Type Helpers

        private static bool IsMemberFolderType(string folderType)
        {
            return folderType == "Member";
        }

        private static bool IsNotMemberFolderType(string folderType)
        {
            return folderType != "Member";
        }

        #endregion
    }
}
