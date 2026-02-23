using System;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;

namespace ENTOS.Module.Services
{
    public partial class AccountEntryService
    {
        private static System.Collections.Generic.IList<AccountingTemplate> LoadAccountingTemplates(IObjectSpace objectSpace, Type objectType)
        {
            return objectSpace.GetObjects<AccountingTemplate>(new BinaryOperator("ObjectType", objectType));
        }

        private static DevExpress.ExpressApp.Actions.ChoiceActionItem CreateEntryTemplateItem(EntryTemplate entryTemplate)
        {
            return new DevExpress.ExpressApp.Actions.ChoiceActionItem(entryTemplate.EntryFolder.Name, entryTemplate.Oid);
        }
    }
}
