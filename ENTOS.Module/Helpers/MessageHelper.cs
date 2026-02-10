using ENTOS.Module.Interfaces;
using System;

namespace ENTOS.Module.Helpers
{
    /// <summary>
    /// A static helper class for showing messages and asking questions.
    /// This class must be initialized with a concrete IMessageService implementation at application startup.
    /// </summary>
    public static class MessageHelper
    {
        private static IMessageService _messageService;
        private static readonly object _lock = new object();

        /// <summary>
        /// Initializes the MessageHelper with a specific message service implementation.
        /// This should be called once at application startup.
        /// </summary>
        /// <param name="service">The IMessageService implementation to use.</param>
        public static void Initialize(IMessageService service)
        {
            if (_messageService != null)
            {
                // Optionally, log a warning if it's being initialized more than once.
                // LogHelper.Warn("MessageHelper is already initialized.");
                return;
            }
            lock (_lock)
            {
                if (_messageService == null)
                {
                    _messageService = service ?? throw new ArgumentNullException(nameof(service));
                }
            }
        }

        private static void EnsureInitialized()
        {
            if (_messageService == null)
            {
                throw new InvalidOperationException("MessageHelper has not been initialized. Please call MessageHelper.Initialize() at application startup.");
            }
        }

        #region Show Messages

        /// <summary>
        /// Shows an informational message.
        /// </summary>
        public static void ShowInfo(string message, string caption = "Information")
        {
            EnsureInitialized();
            _messageService.ShowMessage(message, caption, MessageType.Information);
        }

        /// <summary>
        /// Shows a warning message.
        /// </summary>
        public static void ShowWarning(string message, string caption = "Warning")
        {
            EnsureInitialized();
            _messageService.ShowMessage(message, caption, MessageType.Warning);
        }

        /// <summary>
        /// Shows an error message.
        /// </summary>
        public static void ShowError(string message, string caption = "Error")
        {
            EnsureInitialized();
            _messageService.ShowMessage(message, caption, MessageType.Error);
        }

        /// <summary>
        /// Shows a success message.
        /// </summary>
        public static void ShowSuccess(string message, string caption = "Success")
        {
            EnsureInitialized();
            _messageService.ShowMessage(message, caption, MessageType.Success);
        }

        #endregion

        #region Ask Questions

        /// <summary>
        /// Asks a Yes/No question.
        /// </summary>
        /// <returns>True if the user clicks Yes, false otherwise.</returns>
        public static bool AskYesNo(string question, string caption = "Question")
        {
            EnsureInitialized();
            return _messageService.AskQuestion(question, caption) == QuestionResult.Yes;
        }

        /// <summary>
        /// Asks a question and returns the user's choice.
        /// </summary>
        public static QuestionResult Ask(string question, string caption = "Question")
        {
            EnsureInitialized();
            return _messageService.AskQuestion(question, caption);
        }

        #endregion
    }
} 