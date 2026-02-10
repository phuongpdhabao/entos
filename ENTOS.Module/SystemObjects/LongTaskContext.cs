using System.Threading; // For SynchronizationContext
using ENTOS.Module.Interfaces;

namespace ENTOS.Module.SystemObjects
{
    public class LongTaskContext
    {
        public ITaskProgress Progress { get; private set; }
        public ITaskControl Control { get; private set; }
        public StepProgressConfig StepProgressConfig { get; set; }
        public SynchronizationContext UiContext { get; private set; }
        public LongTaskContext(ITaskProgress progress, ITaskControl control, StepProgressConfig stepProgressConfig, SynchronizationContext uiContext)
        {
            Progress = progress;
            Control = control;
            StepProgressConfig = stepProgressConfig;
            UiContext = uiContext;
        }
    }
}