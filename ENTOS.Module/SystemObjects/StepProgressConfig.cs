using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTOS.Module.SystemObjects
{
    public class StepProgressConfig
    {
        public List<StepInfo> Steps { get; set; } = new();

        public double MapStepProgress(double stepProgress)
        {
            var indexStep = CurrentStepIndex - 1;
            double startPercent = Steps.Take(indexStep).Sum(s => s.Weight);
            double totalProgress = startPercent + stepProgress * Steps[indexStep].Weight;
            return totalProgress;
        }

        public double MapStepProgress(int currentIndex, int total)
        {
            double percentCompleteStep = (double)currentIndex / total;
            return MapStepProgress(percentCompleteStep);
        }

        public int MapStepProgressPercent(int currentIndex, int total)
        {
            return (int)(MapStepProgress(currentIndex, total) * 100);
        }
        public int CurrentStepIndex { get; set; } = 1;

        public string CurrentStepName => Steps.Count > 0 ? Steps[CurrentStepIndex - 1].Name : "Bắt đầu";
    }

    public class StepInfo
    {
        public string Name { get; set; }
        public double Weight { get; set; }
    }
}
