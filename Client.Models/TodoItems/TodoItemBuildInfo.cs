namespace Notes.Client.TodoItems
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Информация для создания задачи
    /// </summary>
    [DataContract]
    public class TodoItemBuildInfo
    {
        /// <summary>
        /// Заголовок задачи
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Title { get; set; }

        /// <summary>
        /// Текст задачи
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Text { get; set; }

        /// <summary>
        /// Дата завершения задачи
        /// </summary>
        [DataMember(IsRequired = false)]
        public DateTime DeadLine { get; set; }
    }
}
