namespace Notes.Client.TodoItems
{
    using System.Collections.Generic;

    /// <summary>
    ///  Список c описанием задач
    /// </summary>
    public class NoteList
    {
        /// <summary>
        /// Краткая информация о задачах
        /// </summary>
        public IReadOnlyCollection<TodoItemInfo> TodoItems { get; set; }
    }
}