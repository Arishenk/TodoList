namespace Notes.Models.Converters.TodoItems
{
    using System;
    using Client = global::Notes.Client.TodoItems;
    using Model = global::Notes.Models.TodoItems;

    /// <summary>
    /// Предоставляет методы конвертирования запроса на изменение задачи между клиентской и серверной моделями
    /// </summary>
    public static class TodoItemPathcInfoConverter
    {
        /// <summary>
        /// Переводит запрос на изменение задачи из клиентсокой модели в серверную
        /// </summary>
        /// <param name="itemId">Идентификатор задачи</param>
        /// <param name="clientPatchInfo">Запрос на изменение задачи в клиентской модели</param>
        /// <returns>Запрос на изменение задачи в серверной модели</returns>
        public static Model.TodoItemPatchInfo Convert(Guid itemId, Client.TodoItemPatchInfo clientPatchInfo)
        {
            if (clientPatchInfo == null)
            {
                throw new ArgumentNullException(nameof(clientPatchInfo));
            }

            var modelPatchInfo = new Model.TodoItemPatchInfo(itemId, clientPatchInfo.DeadLine)
            {
                IsComplited = clientPatchInfo.IsComplited,
                Text = clientPatchInfo.Text,
                Title = clientPatchInfo.Title,
                DeadLine = clientPatchInfo.DeadLine
            };

            return modelPatchInfo;
        }
    }
}
