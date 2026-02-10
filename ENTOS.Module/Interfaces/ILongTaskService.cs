using System;
using System.Threading;
using System.Threading.Tasks;

namespace ENTOS.Module.Interfaces
{
    /// <summary>
    /// Represents the state and progress of a background task.
    /// </summary>
    public interface ITaskProgress
    {
        /// <summary>
        /// Gets or sets the progress percentage (0-100).
        /// </summary>
        int PercentComplete { get; set; }

        /// <summary>
        /// Gets or sets the message describing the current progress.
        /// </summary>
        string ProgressMessage { get; set; }

        /// <summary>
        /// Gets the elapsed time in seconds since the task started.
        /// </summary>
        double ElapsedSeconds { get; }
    }

    /// <summary>
    /// Provides options for controlling a background task.
    /// </summary>
    public interface ITaskControl
    {
        /// <summary>
        /// Gets the CancellationToken to monitor for cancellation requests.
        /// </summary>
        CancellationToken CancellationToken { get; }

        /// <summary>
        /// Gets a value indicating whether the task has been minimized to run in the background.
        /// </summary>
        bool IsMinimized { get; }
    }

    /// <summary>
    /// Service for running long-running operations with a user interface for progress, cancellation, and backgrounding.
    /// </summary>
    public interface ILongTaskService
    {
        /// <summary>
        /// Executes a long-running operation with a progress indicator.
        /// </summary>
        /// <param name="title">The title to display for the task.</param>
        /// <param name="taskToRun">The function to execute. It receives ITaskProgress and ITaskControl objects to report progress and check for cancellation.</param>
        /// <param name="canCancel">A value indicating whether the user can cancel the task.</param>
        /// <param name="canMinimize">A value indicating whether the user can minimize the task to run in the background.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task ExecuteTaskAsync(string title, Func<ITaskProgress, ITaskControl, Task> taskToRun, bool canCancel, bool canMinimize);
    }


    // Pattern 1: UI Thread (for DevExpress objects)
    //await backgroundTaskService.ExecuteTaskAsync(title, BusinessLogicTask, true, true);

    // Pattern 2: Background Thread (for CPU-intensive work)
    //await Task.Run(() => backgroundTaskService.ExecuteTaskAsync(title, CpuIntensiveTask, true, true));

    // Pattern 3: ConfigureAwait(false) for library code
    //await backgroundTaskService.ExecuteTaskAsync(title, LibraryTask, true, true).ConfigureAwait(false);
}