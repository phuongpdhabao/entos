namespace ENTOS.Module.Services
{
    public partial class RecognitionService
    {
        private static int ComputeFrameDigits(int totalFrameCount)
        {
            return System.Math.Max(3, totalFrameCount.ToString().Length);
        }

        private static int ComputeFrameNumber(double ptsTime, double fps)
        {
            if (ptsTime % 1 == 0)
                return (int)ptsTime;

            return (int)System.Math.Round(ptsTime * fps);
        }

        private static System.Collections.Generic.List<int> ComputeExtraFrames(int start, int end, int gapFrame)
        {
            var extraFrames = new System.Collections.Generic.List<int>();
            int distance = end - start;
            if (distance <= gapFrame)
                return extraFrames;

            int numExtra = distance / gapFrame;
            int remainder = distance % (numExtra + 1);
            double extraGap = (double)distance / (numExtra + 1);

            for (int j = 1; j <= numExtra; j++)
            {
                int extraFrame = (int)(start + j * extraGap);
                if (remainder > 0)
                {
                    extraFrame += 1;
                    remainder--;
                }
                extraFrames.Add(extraFrame);
            }

            return extraFrames;
        }
    }
}
