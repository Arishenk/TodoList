namespace Notes.Models.Converters.TodoItems
{
    using System;
    using System.Linq;
    using Client = global::Notes.Client.TodoItems;
    using Model = global::Notes.Models.TodoItems;

    /// <summary>
    /// Предоставляет методы конвертирования информации о задаче между серверной и клиентской моделями
    /// </summary>
    public static class TodoItemInfoConverter
    {
        /// <summary>
        /// Переводит информацию о задаче из серверной модели в клиентскую
        /// </summary>
        /// <param name="modelTodoItemInfo">Информация о задаче в серверной модели</param>
        /// <returns>Информация о задаче в клиентской модели</returns>
        public static Client.TodoItemInfo Convert(Model.TodoItemInfo modelItemInfo)
        {
            if (modelItemInfo == null)
            {
                throw new ArgumentNullException(nameof(modelItemInfo));
            }

            var clientItemInfo = new Client.TodoItemInfo
            {
                Id = modelItemInfo.Id.ToString(),
                UserId = modelItemInfo.UserId.ToString(),
                CreatedAt = modelItemInfo.CreatedAt,
                LastUpdateAt = modelItemInfo.LastUpdatedAt,
                IsComplited = modelItemInfo.IsComplited,
                Title = modelItemInfo.Title,
                DeadLine = modelItemInfo.DeadLine
            };

            return clientItemInfo;
        }
    }
}

