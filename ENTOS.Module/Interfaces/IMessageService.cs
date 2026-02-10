using System;

namespace ENTOS.Module.Interfaces
{
    /// <summary>
    /// Defines the types of messages that can be displayed.
    /// </summary>
    public enum MessageType
    {
        Information,
        Warning,
        Error,
        Success
    }

    /// <summary>
    /// Represents the possible user responses to a question dialog.
    /// </summary>
    public enum QuestionResult
    {
        Ok,
        Cancel,
        Yes,
        No
    }

    /// <summary>
    /// Provides an abstraction for showing messages and asking questions to the user,
    /// allowing for different implementations in different UI frameworks (WinForms, Blazor, etc.).
    /// </summary>
    public interface IMessageService
    {
        /// <summary>
        /// Displays a message to the user.
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <param name="caption">The caption for the message box.</param>
        /// <param name="type">The type of message to display (e.g., Information, Warning).</param>
        void ShowMessage(string message, string caption, MessageType type = MessageType.Information);

        /// <summary>
        /// Asks a question to the user and returns their response.
        /// </summary>
        /// <param name="question">The question to ask.</param>
        /// <param name="caption">The caption for the question dialog.</param>
        /// <returns>The user's response (e.g., Yes, No).</returns>
        QuestionResult AskQuestion(string question, string caption);
    }
} 