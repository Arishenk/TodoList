namespace Notes.Client.TodoItems
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Информация о задаче
    /// </summary>
    public class TodoItemInfo
    {
        /// <summary>
        /// Идентификатор задачи
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Идентификатор пользователя, которому принадлежит задача
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Дата создания задачи
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Дата последнего изменения задачи
        /// </summary>
        public DateTime LastUpdateAt { get; set; }

        /// <summary>
        /// Дата завершения задачи
        /// </summary>
        public DateTime DeadLine { get; set; }

        /// <summary>
        /// Флаг, указывающий, завершена ли задача
        /// </summary>
        public bool IsComplited { get; set; }

        /// <summary>
        /// Название задачи
        /// </summary>
        public string Title { get; set; }

     }
}
