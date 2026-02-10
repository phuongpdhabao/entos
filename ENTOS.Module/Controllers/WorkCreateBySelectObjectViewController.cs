using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;


namespace ENTOS.Module.Controllers;

public class WorkCreateBySelectObjectViewController : ViewController<ListView>
{
    public WorkCreateBySelectObjectViewController()
    {
       
        TargetObjectType = typeof(Module.BusinessObjects.Work);
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
        if (View?.CurrentObject is Module.BusinessObjects.Work currentWork)
        {
            if (e.CreatedObject is Module.BusinessObjects.Work newWork)
            {
                if (currentWork.Member != null)
                    newWork.Member = newWork.Session.GetObjectByKey<Module.BusinessObjects.Work>(currentWork.Oid)?.Member;
                newWork.Reference = currentWork.Reference;
            }
        }
    }

    

    protected override void OnDeactivated()
    {
        base.OnDeactivated();
        Frame.GetController<NewObjectViewController>().ObjectCreated -= NewObjectViewController_ObjectCreated;
    }
}
