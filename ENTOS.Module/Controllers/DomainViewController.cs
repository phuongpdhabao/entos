using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;

namespace ENTOS.Module.Controllers;

public class DomainCreateDomainTypeViewController : ViewController<ListView>
{
    public DomainCreateDomainTypeViewController()
    {

        TargetObjectType = typeof(Module.BusinessObjects.Domain);
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
            foreach (var criteria in View.CollectionSource.Criteria.Values)
            {
                var legacyToString = criteria.LegacyToString();
                if (legacyToString.StartsWith("[DomainType]"))
                {
                    var rightOperator = legacyToString.Substring(legacyToString.IndexOf('=') + 1).Replace("'", "").Trim();
                    var domainType = System.Enum.Parse(typeof(Module.BusinessObjects.DomainType), rightOperator);
                    if (domainType != null)
                    {
                        if (e.CreatedObject is Module.BusinessObjects.Domain domain)
                        {
                            domain.DomainType = (Module.BusinessObjects.DomainType)domainType ;
                            break;
                        }
                    }
                }
            }
        }
    }



    protected override void OnDeactivated()
    {
        base.OnDeactivated();
        Frame.GetController<NewObjectViewController>().ObjectCreated -= NewObjectViewController_ObjectCreated;
    }
}

