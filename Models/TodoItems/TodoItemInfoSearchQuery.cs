namespace Notes.Models.TodoItems
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Параметры поиска задач
    /// </summary>
    public class TodoItemInfoSearchQuery
    {
        /// <summary>
        /// Позиция, начиная с которой нужно производить поиск
        /// </summary>
        public int? Offset { get; set; }

        /// <summary>
        /// Максимальеное количество задач, которое нужно вернуть
        /// </summary>
        public int? Limit { get; set; }

        /// <summary>
        /// Пользователь, которому принадлежит задача
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// Минимальная дата создания задачи
        /// </summary>
        public DateTime? CreatedFrom { get; set; }

        /// <summary>
        /// Максимальная дата создания задачи
        /// </summary>
        public DateTime? CreatedTo { get; set; }

        /// <summary>
        /// Искать по параметру "выполнена"
        /// </summary>
        public bool? IsComplited { get; set; }

        /// <summary>
        /// Тип сортировки
        /// </summary>
        public SortType? Sort { get; set; }

    }
}
