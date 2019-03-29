namespace Notes.Models.TodoItems
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Информация для создания задачи
    /// </summary>
    public class TodoItemCreationInfo
    {
        /// <summary>
        /// Создает задачу
        /// </summary>
        /// <param name="userId">Идентификатор пользователя, который хочет создать задачу</param>
        /// <param name="title">Заголовок задачи</param>
        /// <param name="text">Текст задачи</param>
        /// <param name="deadLine">Дата завершения задачи</param>
        public TodoItemCreationInfo(Guid userId, string title, string text, DateTime deadLine)
        {
            if (title == null)
            {
                throw new ArgumentNullException(nameof(title));
            }

            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            this.UserId = userId;
            this.Title = title;
            this.Text = text;
            this.DeadLine = deadLine;
        }

        /// <summary>
        /// Идентификатор пользователя, который хочет создать задачу
        /// </summary>
        public Guid UserId { get; }

        /// <summary>
        /// Заголовок задачи
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Текст задачи
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Дата завершения задачи
        /// </summary>
        public DateTime DeadLine { get; }
    }
}
