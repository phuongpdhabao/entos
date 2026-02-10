using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using System.ComponentModel;

namespace ENTOS.Module.Services
{
    public class ParallelActionManager
    {
        private static readonly Dictionary<string, ActionInfo> _runningActions = new Dictionary<string, ActionInfo>();
        private static readonly object _lockObject = new object();

        public ParallelActionManager(ViewController controller)
        {
            Controller = controller;
        }

        public ViewController Controller { get; }

        public List<ActionInfo> GetRunningActionsDisplay()
        {
            lock (_lockObject)
            {
                return _runningActions.Values.ToList();
            }
        }

        public void CancelAction(string actionId)
        {
            lock (_lockObject)
            {
                if (_runningActions.TryGetValue(actionId, out var action))
                {
                    action.CancellationTokenSource?.Cancel();
                }
            }
        }

        public async Task RunActionAsync(ActionInfo actionInfo, ActionBaseEventArgs e,
            Func<ActionInfo, IProgress<int>, CancellationToken, Task> executeAction)
        {
            if (actionInfo == null)
            {
                throw new ArgumentNullException(nameof(actionInfo), "Thông tin chức năng không được để trống");
            }


            // Kiểm tra xem có action nào đang chạy trong cùng ObjectSpace không
            lock (_lockObject)
            {
                var runningActionsInSameObjectSpace = _runningActions.Values
                    .Where(a => a.Status == "Đang chạy" && a.ObjectSpace == actionInfo.ObjectSpace)
                    .ToList();

                if (runningActionsInSameObjectSpace.Any())
                {
                    var runningAction = runningActionsInSameObjectSpace.First();
                    throw new InvalidOperationException(
                        $"Không thể thực thi chức năng mới. Chức năng '{runningAction.Action}' đang chạy trong cùng ObjectSpace.");
                }

                actionInfo.Id = Guid.NewGuid().ToString();
                actionInfo.Status = "Đang chạy";
                actionInfo.Progress = 0;
                actionInfo.StartTime = DateTime.Now;
                actionInfo.EndTime = null;
                actionInfo.CancellationTokenSource = new CancellationTokenSource();
                if (e is SingleChoiceActionExecuteEventArgs singleChoiceActionExecuteEventArgs)
                    actionInfo.ChoiceActionItem = singleChoiceActionExecuteEventArgs.SelectedChoiceActionItem;

                _runningActions[actionInfo.Id] = actionInfo;
            }

            try
            {
                var progress = new Progress<int>(value =>
                {
                    actionInfo.Progress = value;
                    UpdateProgress(actionInfo);
                });

                await executeAction(actionInfo, progress, actionInfo.CancellationTokenSource.Token);
                actionInfo.Status = "Hoàn thành";
                actionInfo.Progress = 100;
            }
            catch (OperationCanceledException)
            {
                actionInfo.Status = "Đã hủy";
            }
            catch (Exception)
            {
                actionInfo.Status = "Lỗi";
            }
            finally
            {
                actionInfo.EndTime = DateTime.Now;
                actionInfo.CancellationTokenSource.Dispose();
                actionInfo.CancellationTokenSource = null;

                // Xóa action khỏi danh sách sau khi hoàn thành
                lock (_lockObject)
                {
                    _runningActions.Remove(actionInfo.Id);
                }
            }
        }

        private void UpdateProgress(ActionInfo actionInfo)
        {
            lock (_lockObject)
            {
                if (_runningActions.TryGetValue(actionInfo.Id, out var info))
                {
                    info.Progress = actionInfo.Progress;
                }
            }
        }

        public class ActionInfo
        {
            [Browsable(false)]
            public string Id { get; set; }

            [DevExpress.Xpo.DisplayName("Chức năng")]
            public string Action { get; set; }
            [DevExpress.Xpo.DisplayName("Lựa chọn")]
            public ChoiceActionItem ChoiceActionItem { get; set; }

            [DevExpress.Xpo.DisplayName("Đối tượng")]
            public object TargetObject { get; set; }

            [DevExpress.Xpo.DisplayName("Trạng thái")]
            public string Status { get; set; }

            [DevExpress.Xpo.DisplayName("Hoàn thành")]
            public int Progress { get; set; }

            [DevExpress.Xpo.DisplayName("Bắt đầu")]
            public DateTime StartTime { get; set; }

            [DevExpress.Xpo.DisplayName("Kết thúc")]
            public DateTime? EndTime { get; set; }

            [Browsable(false)]
            public CancellationTokenSource CancellationTokenSource { get; set; }
            [Browsable(false)]
            public IObjectSpace ObjectSpace { get; set; }


            public string GetDisplayCaption()
            {
                return ChoiceActionItem is null ? Action : Module.Helpers.XafXpoHelper.GetCaptionRecursive(ChoiceActionItem);
            }
        }
    }
}