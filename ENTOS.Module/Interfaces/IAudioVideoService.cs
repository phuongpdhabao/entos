namespace ENTOS.Module.Interfaces
{
    /// <summary>
    /// Interface xử lý audio/video: tách, ghép file.
    /// </summary>
    public interface IAudioVideoService
    {
        /// <summary>
        /// Tách audio.
        /// </summary>
        void SplitAudio(string inputPath, string outputPath, TimeSpan start, TimeSpan duration);
        /// <summary>
        /// Ghép nhiều file audio.
        /// </summary>
        void MergeAudio(IEnumerable<string> inputPaths, string outputPath);
        /// <summary>
        /// Tách video.
        /// </summary>
        void SplitVideo(string inputPath, string outputPath, TimeSpan start, TimeSpan duration);
        /// <summary>
        /// Ghép nhiều file video.
        /// </summary>
        void MergeVideo(IEnumerable<string> inputPaths, string outputPath);
    }
} 