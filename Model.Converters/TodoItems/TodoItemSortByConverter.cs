namespace Notes.Models.Converters.TodoItems
{
    using System;
    using Client = global::Notes.Client.TodoItems;
    using Model = global::Notes.Models.TodoItems;

    /// <summary>
    /// Предоставляет методы конвертирования аспекта сортировки задач между клиентской и серверной моделями
    /// </summary>
    public static class TodoItemSortByConverter
    {
        /// <summary>
        /// Переводит аспект сортировки задач из клиентской модели в серверную
        /// </summary>
        /// <param name="clientSortBy">Аспект сортировки задач в клиентской модели</param>
        /// <returns>Аспект сортировки задач в серверной модели</returns>
        public static Model.TodoItemSortBy Convert(Client.TodoItemSortBy clientSortBy)
        {
            switch (clientSortBy)
            {
                case Client.TodoItemSortBy.Creation:
                    return Model.TodoItemSortBy.Creation;

                case Client.TodoItemSortBy.LastUpdate:
                    return Model.TodoItemSortBy.LastUpdate;

                default:
                    throw new ArgumentException($"Unknown sort by value \"{clientSortBy}\".", nameof(clientSortBy));
            }
        }
    }
}