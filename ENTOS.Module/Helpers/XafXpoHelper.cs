using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo;
using ENTOS.Module.SystemObjects;
using Microsoft.AspNetCore.Http;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;

namespace ENTOS.Module.Helpers
{
    /// <summary>
    /// Lớp hỗ trợ các thao tác chung giữa UI (XAF) và Service cho tất cả đối tượng XAF
    /// </summary>
    public static class XafXpoHelper
    {
        /// <summary>
        /// Hiển thị dialog chọn đối tượng bất kỳ (object XAF) theo tiêu chí, trả về object được chọn hoặc null nếu không chọn
        /// </summary>
        /// <typeparam name="T">Kiểu đối tượng XAF</typeparam>
        /// <param name="viewController">ViewController hiện tại</param>
        /// <param name="criteria">Tiêu chí lọc đối tượng</param>
        /// <returns>Đối tượng được chọn hoặc null</returns>
        public static T ShowSelectObjectDialog<T>(ViewController viewController, CriteriaOperator criteria) where T : class
        {
            T selectedObject = null;
            using (DialogController dc = viewController.Application.CreateController<DialogController>())
            {
                dc.Accepting += (s, e) =>
                {
                    selectedObject = e?.AcceptActionArgs?.CurrentObject as T;
                };
                var objectType = typeof(T);
                var objectSpace = viewController.View?.ObjectSpace;
                // Sử dụng PopupDialogControllerListView để hiển thị dialog chọn object                
                PopupDialogControllerListView(
                    viewController,
                    dc,
                    objectType,
                    objectSpace,
                    $"{objectType.Name}PopupDialogCriteria",
                    criteria,
                    false,
                    null,
                    true,
                    true
                );
            }
            return selectedObject;
        }

        public static DevExpress.ExpressApp.DC.IMemberInfo GetMemberInfo(PersistentBase persistentBase, string memberName)
        {
            var objectTypeInfo = XafTypesInfo.Instance.FindTypeInfo(persistentBase.GetType());
            return objectTypeInfo.FindMember(memberName);
        }

        public static bool IsVersionEqual(PersistentBase persistentBase)
        {
            if (persistentBase.Session.IsNewObject(persistentBase))
                return true;
            var objectTypeInfo = XafTypesInfo.Instance.FindTypeInfo(persistentBase.GetType());
            var memberInfo = objectTypeInfo.FindMember("OptimisticLockField");
            if (memberInfo != null)
            {
                var optimisticLockField = memberInfo.GetValue(persistentBase);
                if (optimisticLockField != null)
                {
                    using (var checkSession = new Session(persistentBase.Session.DataLayer))
                    {
                        var oKey = persistentBase.Session.GetKeyValue(persistentBase);
                        if (oKey != null)
                        {
                            var dbObject = checkSession.GetObjectByKey(objectTypeInfo.Type, oKey);
                            if (dbObject != null)
                            {
                                var dbOptimisticLockField = memberInfo.GetValue(dbObject);
                                return optimisticLockField.Equals(dbOptimisticLockField);
                            }
                        }

                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Lấy master object từ View (nếu là ListView có PropertyCollectionSource), trả về null nếu không có
        /// </summary>
        /// <param name="view">View hiện tại</param>
        /// <returns>Master object hoặc null</returns>
        public static T GetMasterObjectFromView<T>(DevExpress.ExpressApp.View view) where T : class
        {
            if (view is ListView listView && listView.CollectionSource is PropertyCollectionSource pcs)
            {
                return pcs.MasterObject as T;
            }
            return null;
        }

        /// <summary>
        /// Tạo ObjectSpace không phân quyền (bỏ qua security)
        /// </summary>
        public static IObjectSpace CreateNonSecuredObjectSpace(XafApplication application)
        {
            return ((INonsecuredObjectSpaceProvider)application.ObjectSpaceProvider).CreateNonsecuredObjectSpace();
        }

        /// <summary>
        /// Tạo DialogController cho DetailView, hiển thị DetailView dưới dạng dialog, trả về DialogController đã tạo
        /// </summary>
        /// <param name="controller">Controller hiện tại</param>
        /// <param name="dc">DialogController (có thể null)</param>
        /// <param name="currentObject">Đối tượng cần hiển thị DetailView</param>
        /// <param name="objectSpace">ObjectSpace sử dụng</param>
        /// <param name="saveOnAccept">Tự động lưu khi Accept</param>
        /// <param name="showViewParameters">Tham số hiển thị view (tùy chọn)</param>
        /// <returns>DialogController đã tạo</returns>
        public static DevExpress.ExpressApp.SystemModule.DialogController CreateDialogControllerDetailView(
            DevExpress.ExpressApp.Controller controller,
            DevExpress.ExpressApp.SystemModule.DialogController dc,
            object currentObject,
            IObjectSpace objectSpace,
            bool saveOnAccept = true,
            ShowViewParameters showViewParameters = null)
        {
            if (dc is null)
                dc = controller.Application.CreateController<DevExpress.ExpressApp.SystemModule.DialogController>();
            dc.SaveOnAccept = saveOnAccept;
            if (showViewParameters is null)
            {
                showViewParameters = new ShowViewParameters
                {
                    TargetWindow = TargetWindow.NewModalWindow,
                    CreateAllControllers = true,
                    NewWindowTarget = NewWindowTarget.Separate
                };
            }
            showViewParameters.Controllers.Add(dc);
            showViewParameters.CreatedView = controller.Application.CreateDetailView(objectSpace, currentObject, true);
            showViewParameters.Context = TemplateContext.View;
            controller.Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(controller.Frame, dc.AcceptAction));
            return dc;
        }

        /// <summary>
        /// Hiển thị dialog chọn ListView với DialogController, hỗ trợ lọc, tìm kiếm, trả về DialogController đã tạo
        /// </summary>
        /// <param name="controller">Controller hiện tại</param>
        /// <param name="dc">DialogController (có thể null)</param>
        /// <param name="objectType">Kiểu đối tượng cần chọn</param>
        /// <param name="objectSpace">ObjectSpace sử dụng</param>
        /// <param name="criteriaName">Tên tiêu chí lọc (tùy chọn)</param>
        /// <param name="criteriaOperator">Tiêu chí lọc (tùy chọn)</param>
        /// <param name="saveOnAccept">Tự động lưu khi Accept</param>
        /// <param name="showViewParameters">Tham số hiển thị view (tùy chọn)</param>
        /// <param name="showFind">Hiển thị ô tìm kiếm</param>
        /// <param name="lookupView">Dùng LookupListView</param>
        /// <param name="collectionSourceDataAccessMode">Chế độ truy xuất dữ liệu (tùy chọn)</param>
        /// <returns>DialogController đã tạo</returns>
        public static DevExpress.ExpressApp.SystemModule.DialogController PopupDialogControllerListView(
            DevExpress.ExpressApp.Controller controller,
            DevExpress.ExpressApp.SystemModule.DialogController dc,
            Type objectType,
            IObjectSpace objectSpace,
            string criteriaName = null,
            DevExpress.Data.Filtering.CriteriaOperator criteriaOperator = null,
            bool saveOnAccept = true,
            ShowViewParameters showViewParameters = null,
            bool showFind = true,
            bool lookupView = false,
            DevExpress.ExpressApp.CollectionSourceDataAccessMode? collectionSourceDataAccessMode = null)
        {
            if (dc is null)
                dc = controller.Application.CreateController<DevExpress.ExpressApp.SystemModule.DialogController>();
            dc.SaveOnAccept = saveOnAccept;
            if (showViewParameters is null)
            {
                showViewParameters = new ShowViewParameters
                {
                    TargetWindow = TargetWindow.NewModalWindow,
                    CreateAllControllers = true,
                    NewWindowTarget = NewWindowTarget.Separate,
                    Context = TemplateContext.LookupWindow,
                };
            }
            if (showFind)
            {
                dc.WindowTemplateChanged += delegate (object o, EventArgs args)
                {
                    if (o is DevExpress.ExpressApp.Controller c && c.Frame != null &&
                        c.Frame.Template is DevExpress.ExpressApp.Editors.ILookupPopupFrameTemplate template)
                    {
                        template.IsSearchEnabled = true;
                    }
                };
            }

            showViewParameters.Controllers.Add(dc);
            if (showViewParameters.CreatedView is null)
            {
                string viewId = !lookupView ? controller.Application.FindListViewId(objectType) : controller.Application.FindLookupListViewId(objectType);
                if (!string.IsNullOrEmpty(viewId))
                {
                    var modelListView = controller.Application.FindModelView(viewId) as DevExpress.ExpressApp.Model.IModelListView;
                    if (modelListView != null)
                    {
                        if (collectionSourceDataAccessMode is null)
                            collectionSourceDataAccessMode = modelListView.DataAccessMode;
                        //Fix lỗi TreeListEditor không hỗ trợ chế độ server
                        if (collectionSourceDataAccessMode.Value == DevExpress.ExpressApp.CollectionSourceDataAccessMode.Server &&
                            modelListView.EditorType != null && modelListView.EditorType.Name == "TreeListEditor")
                        {
                            collectionSourceDataAccessMode = DevExpress.ExpressApp.CollectionSourceDataAccessMode.Client;
                        }
                        CollectionSourceBase collectionSource = controller.Application.CreateCollectionSource(objectSpace,
                            objectType, viewId, collectionSourceDataAccessMode.Value, CollectionSourceMode.Normal);
                        if (!string.IsNullOrEmpty(criteriaName) && !(criteriaOperator is null))
                        {
                            collectionSource.BeginUpdateCriteria();
                            collectionSource.Criteria[criteriaName] = criteriaOperator;
                            collectionSource.EndUpdateCriteria();
                        }
                        var listView = controller.Application.CreateListView(viewId, collectionSource, saveOnAccept);
                        showViewParameters.CreatedView = listView;
                    }
                }
            }

            controller.Application.ShowViewStrategy.ShowView(showViewParameters,
                new ShowViewSource(controller.Frame, dc.AcceptAction));
            return dc;
        }

        public static T GetSingleObjectByCriteria<T>(ViewController view, DevExpress.Data.Filtering.CriteriaOperator criteriaOperator)
        {
            var objectType = typeof(T);
            var objectList = view.View.ObjectSpace.GetObjects(objectType, criteriaOperator);
            if (objectList != null && objectList.Count == 1)
                return (T)objectList[0];
            T result = default(T);

            using (DevExpress.ExpressApp.SystemModule.DialogController dc =
                            view.Application.CreateController<DevExpress.ExpressApp.SystemModule.DialogController>())
            {
                dc.Accepting += delegate (object o, DevExpress.ExpressApp.SystemModule.DialogControllerAcceptingEventArgs args)
                {
                    result = (T)args?.AcceptActionArgs?.CurrentObject;
                };
                PopupDialogControllerListView(view, dc, objectType, view.View.ObjectSpace, "SingleObjectByCriteria", criteriaOperator, false, null, false, true);
            }
            return result;
        }

        /// <summary>
        /// Lấy tên thuộc tính (MemberName) của cột đang được focus trong ListView
        /// </summary>
        /// <param name="view">View hiện tại (ListView)</param>
        /// <returns>Tên thuộc tính của cột đang focus hoặc null nếu không xác định</returns>
        public static string GetFocusedColumnMemberName(DevExpress.ExpressApp.View view)
        {
            if (view is DevExpress.ExpressApp.ListView listView && listView.Editor != null)
            {
                var focusedColumnMemberName = listView.Editor.GetPropertyValue("FocusedColumnMemberName") as string;
                return focusedColumnMemberName;
            }
            return null;
        }

        /// <summary>
        /// Đảm bảo property kiểu Aggregated (liên kết 1-1) không bị null bằng cách tự động khởi tạo nếu cần.
        /// Phương thức này thường được sử dụng cho các property được đánh dấu [Aggregated] trong XAF/XPO,
        /// giúp tự động tạo instance của đối tượng con khi property chưa có giá trị.
        ///
        /// </summary>
        /// <typeparam name="T">
        /// Kiểu của property Aggregated, phải kế thừa từ <see cref="XPBaseObject"/>.
        /// </typeparam>
        /// <param name="owner">
        /// Đối tượng cha chứa property Aggregated.
        /// </param>
        /// <param name="propertyName">
        /// Tên của property Aggregated cần đảm bảo không null.
        /// </param>
        /// <returns>
        /// Giá trị hiện tại của property nếu đã có, hoặc instance mới nếu vừa được khởi tạo.
        /// </returns>
        public static T EnsureAggregatedProperty<T>(XPBaseObject owner, string propertyName) where T : XPBaseObject
        {
            var result = (T)owner.GetMemberValue(propertyName);
            if (result == null && !owner.IsLoading && owner.Session != null && !owner.IsDeleted)
            {
                result = (T)Activator.CreateInstance(typeof(T), owner.Session);
                owner.SetMemberValue(propertyName, result);
            }
            return result;
        }
        /// Hiển thị một thông báo trên giao diện người dùng trong ứng dụng XAF (WinForms hoặc Blazor).
        /// </summary>
        /// <param name="application">Thể hiện của XafApplication hiện tại, được dùng để truy cập ShowViewStrategy.</param>
        /// <param name="caption">Tiêu đề của thông báo (áp dụng cho giao diện WinForms).</param>
        /// <param name="message">Nội dung chính của thông báo.</param>
        /// <param name="informationType">
        /// Loại thông báo (Success, Info, Warning, Error). Mặc định là Success.
        /// Ảnh hưởng đến kiểu hiển thị (màu sắc và biểu tượng) của thông báo.
        /// </param>
        /// <param name="duration">
        /// Thời gian hiển thị thông báo (tính bằng mili giây). Mặc định là 5000ms (5 giây).
        /// Sau thời gian này, thông báo sẽ tự động biến mất.
        /// </param>
        public static void ShowMessage(XafApplication application, string caption, string message,
         InformationType informationType = InformationType.Success, int duration = 5000)
        {
            var messageOptions = new MessageOptions();
            messageOptions.Duration = duration;
            messageOptions.Message = message;
            messageOptions.Web.Position = InformationPosition.Right;
            messageOptions.Win.Caption = caption;
            messageOptions.Win.Type = WinMessageType.Alert;
            messageOptions.Type = informationType;

            application.ShowViewStrategy.ShowMessage(messageOptions);
        }

        /// <summary>
        /// Phải dùng final trong try catch nếu không sẽ lỗi Appearance
        /// </summary>
        public static DevExpress.ExpressApp.ConditionalAppearance.AppearanceController AppearanceBeginUpdate(ViewController view)
        {
            var appearanceController = view?.Frame?.GetController<DevExpress.ExpressApp.ConditionalAppearance.AppearanceController>();
            if (appearanceController != null)
                appearanceController.AppearanceBeginUpdate();
            return appearanceController;
        }

        /// <summary>
        /// Phải dùng final trong try catch nếu không sẽ lỗi Appearance
        /// </summary>
        public static void AppearanceEndUpdate(DevExpress.ExpressApp.ConditionalAppearance.AppearanceController appearanceController)
        {
            appearanceController?.AppearanceEndUpdate();
        }

        public static string GetCaptionRecursive(ChoiceActionItem item, string separator = " > ")
        {
            var sb = new StringBuilder();
            BuildCaption(item, sb, separator);
            return sb.ToString();
        }

        private static void BuildCaption(ChoiceActionItem item, StringBuilder sb, string separator)
        {
            if (item == null)
                return;

            if (item.ParentItem != null)
            {
                BuildCaption(item.ParentItem, sb, separator);
                sb.Append(separator);
            }

            sb.Append(item.Caption);
        }

        public static string GetCaptionObject(ViewController view, PersistentBase baseObject)
        {
            var itemCaption = CaptionHelper.GetDisplayText(baseObject);
            var classCaption = CaptionHelper.GetClassCaption(baseObject.GetType().FullName);
            return $"{itemCaption} - {classCaption}";
        }

        /// <summary>
        /// Gọi phương thức SetDefault cho thuộc tính của BaseObject.
        /// </summary>
        /// <param name="obj">BaseObject cần thiết lập giá trị mặc định</param>
        /// <param name="fieldName">Tên thuộc tính cần thiết lập mặc định</param>
        public static void SetDefaultControlOnValue(object obj, string fieldName)
        {
            if (obj != null & obj is XPBaseObject)
            {
                XPBaseObject currentRow = (XPBaseObject)obj;
                if (!currentRow.IsLoading)
                {
                    Type thisType = currentRow.GetType();
                    System.Reflection.MethodInfo theMethod = thisType.GetMethod("SetDefault" + fieldName);
                    if (theMethod != null)
                    {
                        theMethod.Invoke(currentRow, null);
                    }
                }
            }
        }


        /// <summary>
        /// Thêm FilterCriteria mới vào ObjectSpace.
        /// </summary>
        /// <param name="objectSpace">ObjectSpace để thêm FilterCriteria</param>
        /// <param name="currentType">Kiểu dữ liệu</param>
        /// <param name="field">Tên field (tùy chọn)</param>
        /// <param name="viewId">ID của view (tùy chọn)</param>
        /// <param name="value">Điều kiện lọc</param>
        /// <param name="allowInherit">Cho phép kế thừa</param>
        public static void AddFilterCriteria(IObjectSpace objectSpace, Type currentType, string field, string viewId, string value, bool allowInherit)
        {
            if (!(objectSpace is XPObjectSpace))
                return;
            var criteria = CriteriaOperator.Parse("ObjectType = ?", currentType);
            if (field != null)
            {
                criteria = CriteriaOperator.And(criteria,
                    CriteriaOperator.Parse("IsListView = False and EndsWith([Field], ?)",
                        System.Environment.NewLine + field));
            }
            else if (!string.IsNullOrEmpty(viewId))
            {
                criteria = CriteriaOperator.And(criteria, CriteriaOperator.Parse("IsListView and ViewId = ?", viewId));
            }
            if (objectSpace.FindObject<Module.SystemObjects.FilterCriteria>(criteria) == null)
            {
                Module.SystemObjects.FilterCriteria filterCriteria = objectSpace.CreateObject<Module.SystemObjects.FilterCriteria>();
                filterCriteria.ObjectType = currentType;
                if (!string.IsNullOrEmpty(field))
                {
                    filterCriteria.Field = new Module.SystemObjects.StringLookup(GetCaptionFromField(currentType, field), field);
                    filterCriteria.IsListView = false;
                }
                else if (!string.IsNullOrEmpty(viewId))
                {
                    filterCriteria.ViewId = viewId;
                    filterCriteria.IsListView = true;
                }

                filterCriteria.Condition = value;
                filterCriteria.AllowInherit = allowInherit;
            }
        }

        /// <summary>
        /// Lấy caption của field từ TypeInfo và các attribute.
        /// </summary>
        /// <param name="currentType">Kiểu dữ liệu</param>
        /// <param name="field">Tên field</param>
        /// <returns>Caption của field hoặc tên field nếu không tìm thấy</returns>
        public static string GetCaptionFromField(Type currentType, string field)
        {
            if (currentType != null && !string.IsNullOrEmpty(field))
            {
                var member = XafTypesInfo.Instance.FindTypeInfo(currentType).FindMember(field);
                if (member != null)
                {
                    var displayNameAttribute = member.FindAttribute<DevExpress.Xpo.DisplayNameAttribute>();
                    if (displayNameAttribute != null)
                    {
                        return displayNameAttribute.DisplayName;
                    }
                    var xafDisplayNameAttribute = member.FindAttribute<DevExpress.ExpressApp.DC.XafDisplayNameAttribute>();
                    if (xafDisplayNameAttribute != null)
                    {
                        return xafDisplayNameAttribute.DisplayName;
                    }

                    var defaultAttributes = member.FindAttributes<ModelDefaultAttribute>();
                    if (defaultAttributes != null)
                    {
                        foreach (var defaultAttribute in defaultAttributes)
                        {
                            if (defaultAttribute.PropertyName.Equals("Caption"))
                            {
                                return defaultAttribute.PropertyValue;
                            }
                        }
                    }
                }
            }
            return field;
        }


        /// <summary>
        /// Lấy caption của field từ ModelView.
        /// </summary>
        /// <param name="view">ModelView chứa field</param>
        /// <param name="field">Tên field</param>
        /// <returns>Caption của field hoặc tên field nếu không tìm thấy</returns>
        public static string GetCaptionFromModel(IModelView view, string field)
        {
            if (view != null && !string.IsNullOrEmpty(field))
            {
                if (view is IModelDetailView)
                {
                    var modelDetailView = (IModelDetailView)view;
                    var nodeField = modelDetailView.Items.GetNode(field);
                    //var result = nodeField.GetValue<string>("Caption");
                    if (nodeField != null)
                        return nodeField.GetValue<string>("Caption");
                }
                else if (view is IModelListView)
                {
                    var modelListView = (IModelListView)view;
                    var nodeField = modelListView.Columns.GetNode(field);
                    //var result = nodeField.GetValue<string>("Caption");
                    if (nodeField != null)
                        return nodeField.GetValue<string>("Caption");
                }
            }
            return field;
        }


        /// <summary>
        /// Lấy tooltip text cho object từ PropertyEditor.
        /// </summary>
        /// <param name="obj">Object cần lấy tooltip</param>
        /// <param name="editor">PropertyEditor của object</param>
        /// <returns>Tooltip text hoặc null nếu không tìm thấy</returns>
        public static string GetTooltipControllerTextForObject(object obj, PropertyEditor editor)
        {
            Type thisType = obj.GetType();
            System.Reflection.MethodInfo theMethod =
                thisType.GetMethod(editor != null ? editor.PropertyName : "" + "ToolTipControllerText");
            if (theMethod != null)
            {
                var resultToolTipControllerText = theMethod.Invoke(obj, null) as string;
                if (!string.IsNullOrEmpty(resultToolTipControllerText))
                    return resultToolTipControllerText;
            }
            else if (editor != null && editor.ControlValue is PersistentBase)
            {
                return GetTooltipControllerTextForObject(editor.ControlValue, null);
            }
            return null;
        }



        /// <summary>
        /// Lấy object cuối cùng theo điều kiện và sắp xếp.
        /// </summary>
        /// <param name="session">Session để thao tác database</param>
        /// <param name="type">Kiểu dữ liệu</param>
        /// <param name="criteria">Điều kiện lọc</param>
        /// <param name="sort">Thuộc tính sắp xếp</param>
        /// <param name="inTransaction">Có trong transaction không</param>
        /// <returns>Object cuối cùng hoặc null nếu không tìm thấy</returns>
        public static object GetLastedBySort(
            Session session,
            Type type,
            CriteriaOperator criteria,
            SortProperty sort, bool inTransaction = false)
        {
            XPCollection xpCollection1 = new XPCollection(inTransaction ?
                PersistentCriteriaEvaluationBehavior.InTransaction : PersistentCriteriaEvaluationBehavior.BeforeTransaction, session, type, criteria);
            xpCollection1.Sorting.Add(sort);
            xpCollection1.TopReturnedObjects = 1;
            XPCollection xpCollection2 = xpCollection1;
            if (xpCollection2.Count > 0)
                return xpCollection2[0];
            return (object)null;
        }

        /// <summary>
        /// Lấy giá trị lớn nhất của field theo điều kiện.
        /// </summary>
        /// <param name="session">Session để thao tác database</param>
        /// <param name="type">Kiểu dữ liệu</param>
        /// <param name="criteria">Điều kiện lọc</param>
        /// <param name="field">Tên field cần lấy giá trị lớn nhất</param>
        /// <returns>Giá trị lớn nhất của field</returns>
        public static object GetMaxValueFromParent(
            Session session,
            Type type,
            CriteriaOperator criteria,
            string field)
        {
            return session.Evaluate(type, CriteriaOperator.Parse(string.Format("Max({0})", (object)field)), criteria);
        }

        /// <summary>
        /// Lấy giá trị nhỏ nhất của field theo điều kiện.
        /// </summary>
        /// <param name="session">Session để thao tác database</param>
        /// <param name="type">Kiểu dữ liệu</param>
        /// <param name="criteria">Điều kiện lọc</param>
        /// <param name="field">Tên field cần lấy giá trị nhỏ nhất</param>
        /// <returns>Giá trị nhỏ nhất của field</returns>
        public static object GetMinValueFromParent(
            Session session,
            Type type,
            CriteriaOperator criteria,
            string field)
        {
            return session.Evaluate(type, CriteriaOperator.Parse(string.Format("Min({0})", (object)field)), criteria);
        }

        #region Device & Platform Detection

        /// <summary>
        /// Kiểm tra xem frame hiện tại có phải là Mobile frame không.
        /// Hữu ích để tùy chỉnh UI/UX cho mobile platforms.
        /// </summary>
        /// <param name="currentFrame">Frame cần kiểm tra</param>
        /// <returns>True nếu là Mobile frame, False nếu không phải hoặc frame là null</returns>
        public static bool IsMobileFrame(DevExpress.ExpressApp.Frame currentFrame)
        {
            if (currentFrame == null)
                return false;

            var currentType = currentFrame.GetType();
            return !string.IsNullOrEmpty(currentType.FullName) &&
                   currentType.FullName.Equals("DevExpress.ExpressApp.Mobile.MobileWindow");
        }

        #endregion


        /// <summary>
        /// Lấy CriteriaOperator cho FilterCriteria theo type và field hoặc viewId.
        /// </summary>
        /// <param name="currentType">Kiểu dữ liệu cần lọc</param>
        /// <param name="fieldName">Tên field (tùy chọn)</param>
        /// <param name="viewId">ID của view (tùy chọn)</param>
        /// <param name="session">Session để thao tác database</param>
        /// <returns>CriteriaOperator kết hợp từ các FilterCriteria</returns>
        public static CriteriaOperator GetCriteriaOperator(Type currentType, string fieldName = null, string viewId = null, Session session = null)
        {
            if (currentType != null)
            {
                var criteria = CriteriaOperator.Parse("Active and ObjectType = ?", currentType);
                if (currentType.BaseType != null)
                {
                    criteria = CriteriaOperator.Or(criteria,
                        CriteriaOperator.Parse("ObjectType = ? and AllowInherit",
                            currentType.BaseType));
                }
                if (!string.IsNullOrEmpty(fieldName))
                {
                    criteria = CriteriaOperator.And(criteria,
                        CriteriaOperator.Parse("IsListView = False and EndsWith([Field], ?)",
                            System.Environment.NewLine + fieldName));
                }
                else if (!string.IsNullOrEmpty(viewId))
                {
                    criteria = CriteriaOperator.And(criteria, CriteriaOperator.Parse("IsListView and ViewId = ?", viewId));
                }
                else
                {
                    return null;
                }

                IList<FilterCriteria> filtersCriteria = null;
                if (session != null)
                {
                    filtersCriteria = new XPCollection<FilterCriteria>(session, criteria).ToList();
                }
                if (filtersCriteria == null)
                {
                    IObjectSpace objectSpace = XPObjectSpace.FindObjectSpaceByObject(SecuritySystem.CurrentUser);
                    if (session == null)
                    {
                        session = ((XPObjectSpace)objectSpace).Session;
                    }
                    filtersCriteria = objectSpace.GetObjects<FilterCriteria>(criteria);
                }
                if (filtersCriteria != null && filtersCriteria.Count > 0)
                {
                    CriteriaOperator result = null;
                    foreach (var filterCriteria in filtersCriteria)
                    {
                        if (!string.IsNullOrEmpty(filterCriteria.Condition))
                        {
                            result = CriteriaOperator.And(result, session.ParseCriteria(filterCriteria.Condition));
                        }
                    }
                    //if (!(result is null) && session != null)
                    //    return session.ParseCriteria(result.LegacyToString());
                    return result;
                }
            }

            return null;
        }



        /// <summary>
        /// Lấy caption của giá trị enum.
        /// </summary>
        /// <param name="type">Kiểu enum</param>
        /// <param name="obj">Giá trị enum</param>
        /// <returns>Caption của enum hoặc null nếu không tìm thấy</returns>
        public static string GetCaptionEnum(Type type, object obj)
        {
            if (obj != null)
            {
                if (type != null)
                {
                    EnumDescriptor myDescriptor = new EnumDescriptor(type);
                    foreach (object enumValue in myDescriptor.Values)
                    {
                        if (obj.Equals(enumValue))
                            return myDescriptor.GetCaption(enumValue);
                    }
                }
                return obj.ToString();
            }
            return null;
        }


        public static object GetSimpleObjectByCriteria(ViewController view, Type type, DevExpress.Data.Filtering.CriteriaOperator criteriaOperator)
        {
            var objectList = view.View.ObjectSpace.GetObjects(type, criteriaOperator);
            if (objectList != null && objectList.Count == 1)
                return objectList[0];
            object result = null;

            using (DevExpress.ExpressApp.SystemModule.DialogController dc =
                            view.Application.CreateController<DevExpress.ExpressApp.SystemModule.DialogController>())
            {
                dc.Accepting += delegate (object o, DevExpress.ExpressApp.SystemModule.DialogControllerAcceptingEventArgs args)
                {
                    result = args?.AcceptActionArgs?.CurrentObject;
                };
                Module.SystemObjects.Tools.PopupDialogControllerListView(view, dc, type, view.View.ObjectSpace, "GetSimpleObjectBy", criteriaOperator, false, null, false, true);
            }
            return result;
        }

        public static T GetSimpleObjectByCriteria<T>(ViewController view, DevExpress.Data.Filtering.CriteriaOperator criteriaOperator)
        {
            var objectType = typeof(T);
            var objectList = view.View.ObjectSpace.GetObjects(objectType, criteriaOperator);
            if (objectList != null && objectList.Count == 1)
                return (T)objectList[0];
            T result = default(T);

            using (DevExpress.ExpressApp.SystemModule.DialogController dc =
                            view.Application.CreateController<DevExpress.ExpressApp.SystemModule.DialogController>())
            {
                dc.Accepting += delegate (object o, DevExpress.ExpressApp.SystemModule.DialogControllerAcceptingEventArgs args)
                {
                    result = (T)args?.AcceptActionArgs?.CurrentObject;
                };
                Module.SystemObjects.Tools.PopupDialogControllerListView(view, dc, objectType, view.View.ObjectSpace, "GetSimpleObjectBy", criteriaOperator, false, null, false, true);
            }
            return result;
        }

        public static void ShowOrCloseDefaultWaitForm(string caption, string description = null, System.TimeSpan? currentTimeSpan = null, bool defaultSplashScreenManager = false)
        {
            try
            {
                var type = GetSplashScreenManager();
                if (type != null)
                {
                    if (string.IsNullOrEmpty(caption) && string.IsNullOrEmpty(description))
                    {

                        var methodCloseForm = type.GetMethod("CloseForm", BindingFlags.Public | BindingFlags.Static, new Type[] { });
                        if (methodCloseForm != null)
                            methodCloseForm.Invoke(null, null);
                    }
                    else
                    {

                        if (!string.IsNullOrEmpty(description) && currentTimeSpan != null)
                            description += " - ";
                        if (currentTimeSpan != null)
                        {
                            description += String.Format("{0:00}:{1:00}:{2:00}",
                                    currentTimeSpan.Value.Hours, currentTimeSpan.Value.Minutes, currentTimeSpan.Value.Seconds);
                        }
                        if (defaultSplashScreenManager)
                        {
                            var property = type.GetProperty("Default");
                            if (property != null)
                            {
                                var propertyValue = property.GetValue(null);
                                if (propertyValue is null)
                                {
                                    ShowOrCloseWaitFormWithCancelButton();
                                    propertyValue = property.GetValue(null);
                                }
                                if (propertyValue != null)
                                {
                                    if (!string.IsNullOrEmpty(caption))
                                    {
                                        var method = property.PropertyType.GetMethod("SetWaitFormCaption");
                                        if (method != null)
                                            method.Invoke(propertyValue, new object[] { caption });
                                    }
                                    else if (!string.IsNullOrEmpty(description))
                                    {
                                        var method = property.PropertyType.GetMethod("SetWaitFormDescription");
                                        if (method != null)
                                            method.Invoke(propertyValue, new object[] { description });
                                    }
                                }
                                else
                                {
                                    ShowOrCloseDefaultWaitForm(caption, description, currentTimeSpan, false);
                                }
                            }
                            else
                            {
                                //Form đã bị đóng
                                var method = type.GetMethod("ShowDefaultWaitForm", BindingFlags.Public | BindingFlags.Static, new Type[] { typeof(string), typeof(string) });
                                if (method != null)
                                    method.Invoke(null, new object[] { caption, description });
                            }

                        }
                        else
                        {
                            var method = type.GetMethod("ShowDefaultWaitForm", BindingFlags.Public | BindingFlags.Static, new Type[] { typeof(string), typeof(string) });
                            if (method != null)
                                method.Invoke(null, new object[] { caption, description });
                        }

                    }
                }
            }
            catch (System.Exception) { }

        }

        public static void ShowOrCloseWaitFormWithCancelButton()
        {
            try
            {
                var type = GetSplashScreenManager();
                if (type is null)
                    return;
                var formType = DevExpress.Persistent.Base.ReflectionHelper.FindType("System.Windows.Forms.Form");
                if (formType is null)
                    return;
                var method = type.GetMethod("ShowForm", BindingFlags.Public | BindingFlags.Static, new Type[] { formType, typeof(Type), typeof(bool), typeof(bool), typeof(bool) });
                if (method is null)
                    return;
                var waitType = DevExpress.Persistent.Base.ReflectionHelper.FindType("WaitFormWithCancelButton");
                if (waitType is null)
                    return;
                method.Invoke(null, new object[] { null, waitType, true, true, false });
            }
            catch (System.Exception) { }

        }
        private static PropertyInfo defaultSplashScreenManagerProperty = null;
        private static Type defaultSplashScreenManagerType = null;
        public static object DefaultSplashScreenManager
        {
            get
            {
                if (defaultSplashScreenManagerProperty is null)
                {
                    if (defaultSplashScreenManagerType is null)
                        defaultSplashScreenManagerType = GetSplashScreenManager();
                    if (defaultSplashScreenManagerType != null)
                    {
                        defaultSplashScreenManagerProperty = defaultSplashScreenManagerType.GetProperty("Default");
                    }
                }
                if (defaultSplashScreenManagerProperty != null)
                    return defaultSplashScreenManagerProperty.GetValue(null);
                return null;
            }
        }

        private static Type GetSplashScreenManager()
        {
            string typeName = "DevExpress.XtraSplashScreen.SplashScreenManager";
            //var objectTypeInfo = XafTypesInfo.Instance.FindTypeInfo("DevExpress.XtraSplashScreen.SplashScreenManager");
            return DevExpress.Persistent.Base.ReflectionHelper.FindType(typeName);
        }

        public static T CopyObject<T>(XPBaseObject source, Session targetSession, IEnumerable<string> ignoreProperties = null) where T : XPBaseObject
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (targetSession == null) throw new ArgumentNullException(nameof(targetSession));

            // Danh sách field mặc định bỏ qua
            var defaultIgnores = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                nameof(XPObject.Oid)
            };

            if (ignoreProperties != null)
                defaultIgnores.UnionWith(ignoreProperties);

            // Tạo đối tượng mới trong session mục tiêu
            T target = (T)Activator.CreateInstance(typeof(T), targetSession);

            // Duyệt qua tất cả property public
            foreach (var prop in source.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!prop.CanRead || !prop.CanWrite) continue;
                if (defaultIgnores.Contains(prop.Name)) continue; // bỏ qua trong danh sách ignore

                // Bỏ qua reference object (chỉ copy scalar)
                if (typeof(XPBaseObject).IsAssignableFrom(prop.PropertyType)) continue;

                object value = prop.GetValue(source);
                prop.SetValue(target, value);
            }

            return target;
        }


        /// <summary>
        /// Lấy thời gian hiện tại từ server database.
        /// </summary>
        /// <param name="session">Session để thao tác database</param>
        /// <returns>Thời gian hiện tại từ server</returns>
        public static DateTime GetDateTimeNowFromServer(Session session)
        {
            return (DateTime)session.Evaluate(typeof(XPObjectType),
                (CriteriaOperator)new FunctionOperator(FunctionOperatorType.Now, new CriteriaOperator[0]),
                (CriteriaOperator)null);
        }

        public static T GetCurrentUser<T>(DevExpress.Xpo.Session session) where T : DevExpress.Persistent.BaseImpl.PermissionPolicy.PermissionPolicyUser
        {
            try
            {
                //DevExpress.Persistent.Base.ValueManager.ValueManagerType
                //Dùng SecuritySystem nhanh nhưng bị lổi trên web api
                if (SecuritySystem.Instance != null)
                {
                    return session.GetObjectByKey<T>(SecuritySystem.CurrentUserId);
                }
            }
            catch (Exception)
            {
                //Lỗi truy cập đến SecuritySystem.CurrentUserId
            }
            IHttpContextAccessor httpContextAccessor = session.ServiceProvider.GetService(typeof(IHttpContextAccessor)) as IHttpContextAccessor;
            if (!string.IsNullOrEmpty(httpContextAccessor?.HttpContext?.User?.Identity?.Name))
            {
                //Trường hơp dùng web api
                var user = session.FindObject<T>(new DevExpress.Data.Filtering.BinaryOperator(nameof(DevExpress.Persistent.BaseImpl.PermissionPolicy.PermissionPolicyUser.UserName), httpContextAccessor.HttpContext.User.Identity.Name));
                if (user != null)
                    return user;
            }
            var securityProvider = session.ServiceProvider.GetService(typeof(ISecurityProvider)) as ISecurityProvider;
            if (securityProvider != null)
            {
                //Chạy cả trên win, blazor và web api
                var security = securityProvider.GetSecurity();
                if (security != null)
                    return session.GetObjectByKey<T>(security.UserId);
            }
            return null;
        }
    }
}