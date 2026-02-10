using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace ENTOS.Module.BusinessObjects 
{
    
public enum WebsiteElementType
    {
					[XafDisplayName("Thực đơn")]
        Menu,
					[XafDisplayName("Chuyên mục")]
        PostCategory,
					[XafDisplayName("Bài viết")]
        Post,
					[XafDisplayName("Danh mục SP")]
        ProductCategory,
					[XafDisplayName("Sản phẩm")]
        Product,
					[XafDisplayName("Trang")]
        Page,
					[XafDisplayName("Media")]
        Media,
	    }

}