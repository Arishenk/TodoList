using System;
using System.Collections.Generic;

namespace Notes.Models.TodoItems
{
    /// <summary>
    /// Информация о задаче
    /// </summary>
    public class TodoItemInfo
    {
        /// <summary>
        /// Идентификатор задачи
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Идентификатор пользователя, которому принадлежит задача
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Дата создания задачи
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Дата последнего изменения
        /// </summary>
        public DateTime LastUpdatedAt { get; set; }

        /// <summary>
        /// Флаг, указывающий, выполнена ли задача
        /// </summary>
        public bool IsComplited { get; set; }

        /// <summary>
        /// Название задачи
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Дата завершения задачи
        /// </summary>
        public DateTime DeadLine { get; set; }
    }
}
