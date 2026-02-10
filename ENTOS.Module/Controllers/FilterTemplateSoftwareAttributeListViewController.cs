using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Data.Filtering;

namespace ENTOS.Module.Controllers {
    public partial class TemplateSoftwareAttributeListViewController : ViewController<ListView> {
        public TemplateSoftwareAttributeListViewController() {
            // Gắn controller vào đúng ListView collection editor
            TargetViewId = "DataType_TemplateSoftwareAttributeList_ListView";
        }

        protected override void OnActivated() {
            base.OnActivated();

            // Đảm bảo View là CollectionSource
            if (View.CollectionSource != null) {
                // Đặt tiêu chí lọc
                CriteriaOperator criteria = CriteriaOperator.Parse(
                    "[DataType] Is Null And [DataTypeMember] Is Null And [Field] Is Null And [DataTypeDefault] Is Null"
                );

                // Áp dụng criteria vào CollectionSource
                View.CollectionSource.Criteria["TemplateFilter"] = criteria;
            }
        }

        protected override void OnDeactivated() {
            // Xoá filter khi controller deactivated (nếu cần)
            if (View.CollectionSource != null) {
                View.CollectionSource.Criteria.Remove("TemplateFilter");
            }

            base.OnDeactivated();
        }
    }
}
