namespace Notes.Models.TodoItems
{
    using System;

    /// <summary>
    /// Исключение, которое возникает при попытке получить несуществующую задачу
    /// </summary>
    public class TodoItemNotFoundExcepction : Exception
    {
        /// <summary>
        /// Создает новый экземпляр исключения о том, что задача не найдена
        /// </summary>
        /// <param name="message">Сообщение об ошибке</param>
        public TodoItemNotFoundExcepction(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Создает новый экземпляр исключения о том, что задача не найдена
        /// </summary>
        /// <param name="itemId">Идентификатор задачи, которая не найдена</param>
        public TodoItemNotFoundExcepction(Guid itemId)
            : base($"Note \"{itemId}\" is not found.")
        {
        }
    }
}

