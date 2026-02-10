using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;

namespace ENTOS.Module.Controllers;

public class FolderCreateByConditionViewController : ViewController<ListView>
{
    public FolderCreateByConditionViewController()
    {
       
        TargetObjectType = typeof(Module.BusinessObjects.Folder);
        TargetViewNesting = Nesting.Root;
    }

    protected override void OnActivated()
    {
        base.OnActivated();
        NewObjectViewController newObjectViewController = Frame.GetController<NewObjectViewController>();
        if (newObjectViewController != null)
            newObjectViewController.ObjectCreated += NewObjectViewController_ObjectCreated;
    }

    private void NewObjectViewController_ObjectCreated(object sender, ObjectCreatedEventArgs e)
    {
        if (View?.CollectionSource?.Criteria != null)
        {
            if(View.CollectionSource.Criteria.ContainsKey("ListViewCriteria"))
            {
                var criteria = View.CollectionSource.Criteria["ListViewCriteria"];
                if (criteria is GroupOperator groupOperator)
                {
                    foreach (var childCriteria in groupOperator.Operands)
                    {
                        if (SetFolderTypeByCriteria(e, childCriteria.LegacyToString()))
                            break;
                    }
                }
                else
                    SetFolderTypeByCriteria(e, criteria.LegacyToString());
            }
        }
    }

    private bool SetFolderTypeByCriteria(ObjectCreatedEventArgs e, string criteriaLegacy)
    {
        try
        {
            if (criteriaLegacy.StartsWith("[FolderType]") || criteriaLegacy.StartsWith("FolderType ="))
            {
                var rightOperator = criteriaLegacy.Substring(criteriaLegacy.IndexOf('=') + 1).Replace("'", "").Trim();
                var folderType = System.Enum.Parse(typeof(Module.BusinessObjects.SoftwareObjectType), rightOperator);
                if (folderType != null)
                {
                    if (e.CreatedObject is Module.BusinessObjects.Folder folder)
                    {
                        folder.FolderType = (Module.BusinessObjects.SoftwareObjectType)folderType;
                        return true;
                    }
                }
            }
        }
        catch (System.Exception) { }
        
        return false;
    }




    protected override void OnDeactivated()
    {
        base.OnDeactivated();
        Frame.GetController<NewObjectViewController>().ObjectCreated -= NewObjectViewController_ObjectCreated;
    }
}