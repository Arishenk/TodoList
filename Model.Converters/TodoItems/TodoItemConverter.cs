namespace Notes.Models.Converters.TodoItems
{
    using System;
    using System.Linq;
    using Client = global::Notes.Client.TodoItems;
    using Model = global::Notes.Models.TodoItems;

    /// <summary>
    /// Предоставляет методы конвертирования задачи между серверной и клиентской моделями
    /// </summary>
    public static class TodoItemConverter
    {
        /// <summary>
        /// Переводит задачу из серверной модели в клиентскую
        /// </summary>
        /// <param name="modelItem">Задача в серверной модели</param>
        /// <returns>Задача в клиентской модели</returns>
        public static Client.TodoItem Convert(Model.TodoItem modelItem)
        {
            if (modelItem == null)
            {
                throw new ArgumentNullException(nameof(modelItem));
            }

            var clientItem = new Client.TodoItem
            {
                Id = modelItem.Id.ToString(),
                UserId = modelItem.UserId.ToString(),
                CreatedAt = modelItem.CreatedAt,
                LastUpdateAt = modelItem.LastUpdatedAt,
                IsComplited = modelItem.IsComplited,
                Title = modelItem.Title,
                Text = modelItem.Text,
                DeadLine = modelItem.DeadLine
            };

            return clientItem;
        }
    }
}
