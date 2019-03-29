namespace Notes.Client.TodoItems
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Информация для изменения задачи
    /// </summary>
    [DataContract]
    public class TodoItemPatchInfo
    {
        /// <summary>
        /// Флаг, указывающий, что задача находится в избранном
        /// </summary>
        [DataMember(IsRequired = false)]
        public bool? IsComplited { get; set; }

        /// <summary>
        /// Новый заголовок задачи
        /// </summary>
        [DataMember(IsRequired = false)]
        public string Title { get; set; }

        /// <summary>
        /// Новый текст задачи
        /// </summary>
        [DataMember(IsRequired = false)]
        public string Text { get; set; }

        /// <summary>
        /// Новый срок сдачи задачи
        /// </summary>
        [DataMember(IsRequired = false)]
        public DateTime DeadLine { get; set; }
    }
}
