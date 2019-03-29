namespace Notes.Models.Converters.TodoItems
{
    using System;
    using Client = global::Notes.Client.TodoItems;
    using Model = global::Notes.Models.TodoItems;

    /// <summary>
    /// Предоставляет методы конвертирования запроса на создание задачи между клиентской и серверной моделями
    /// </summary>
    public static class TodoItemBuildInfoConverter
    {
        /// <summary>
        /// Переводит запрос на создание задачи из клиентсокой модели в серверную
        /// </summary>
        /// <param name="clientUserId">Идентификатор пользователя в клиентской модели</param>
        /// <param name="clientBuildInfo">Запрос на создание задачи в клиентской модели</param>
        /// <returns>Запрос на создание задачи в серверной модели</returns>
        public static Model.TodoItemCreationInfo Convert(string clientUserId, Client.TodoItemBuildInfo clientBuildInfo)
        {
            if (clientUserId == null)
            {
                throw new ArgumentNullException(nameof(clientUserId));
            }

            if (clientBuildInfo == null)
            {
                throw new ArgumentNullException(nameof(clientBuildInfo));
            }

            if (!Guid.TryParse(clientUserId, out var modelUserId))
            {
                throw new ArgumentException($"The client user id \"{clientUserId}\" is invalid.", nameof(clientUserId));
            }

            var modelCreationInfo = new Model.TodoItemCreationInfo(
                modelUserId,
                clientBuildInfo.Title,
                clientBuildInfo.Text,
                clientBuildInfo.DeadLine);

            return modelCreationInfo;
        }
    }
}

