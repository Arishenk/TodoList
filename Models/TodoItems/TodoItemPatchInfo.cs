namespace Notes.Models.TodoItems
{
    using System;

    /// <summary>
    /// Информация для изменения задачи
    /// </summary>
    public class TodoItemPatchInfo
    {
        /// <summary>
        /// Создает новый экземпляр объекта, описывающего изменение задачи
        /// </summary>
        /// <param name="itemId">Идентификатор задачи, которую нужно изменить</param>
        /// <param name="title">Новый заголовок задачи</param>
        /// <param name="text">Новый текст задачи</param>
        /// <param name="isComplited">Флаг, указывающий, что задача выполнена</param>
        /// <param name="deadLine"> Дата завершения задачи</param>
        public TodoItemPatchInfo(Guid itemId, DateTime deadLine, bool? isComplited = null, string title = null, string text = null)
        {
            this.ItemId = itemId;
            this.IsComplited = isComplited;
            this.Title = title;
            this.Text = text;
            this.DeadLine = deadLine;
        }

        /// <summary>
        /// Идентификатор задачи, которую нужно изменить
        /// </summary>
        public Guid ItemId { get; }

        /// <summary>
        /// Флаг, указывающий, что задача выполнена
        /// </summary>
        public bool? IsComplited { get; set; }

        /// <summary>
        /// Новый заголовок задачи
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Новый текст задачи
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Дата завершения задачи
        /// </summary>
        public DateTime DeadLine { get; set; }
    }
}
