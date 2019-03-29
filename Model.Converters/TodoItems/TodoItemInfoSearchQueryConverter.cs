namespace Notes.Models.Converters.TodoItems
{
    using System;
    using System.Linq;
    using Client = global::Notes.Client.TodoItems;
    using Model = global::Notes.Models.TodoItems;

    /// <summary>
    /// Предоставляет методы конвертирования запроса за задачами между клиентской и серверной моделями
    /// </summary>
    public static class TodoItemInfoSearchQueryConverter
    {
        /// <summary>
        /// Переводит запрос за задачами из клиентсокой модели в серверную
        /// </summary>
        /// <param name="clientQuery">Запрос за задачами в клиентской модели</param>
        /// <returns>Запрос за задачами в серверной модели</returns>
        public static Model.TodoItemInfoSearchQuery Convert(Client.TodoItemInfoSearchQuery clientQuery)
        {
            if (clientQuery == null)
            {
                throw new ArgumentNullException(nameof(clientQuery));
            }

            var modelUserId = (Guid?)null;

            if (clientQuery.UserId != null)
            {
                if (!Guid.TryParse(clientQuery.UserId, out var userId))
                {
                    throw new ArgumentException($"The user id \"{clientQuery.UserId}\" is invalid.", nameof(clientQuery));
                }

                modelUserId = userId;
            }

            var modelSort = clientQuery.Sort.HasValue ?
                SortTypeConverter.Convert(clientQuery.Sort.Value) :
                (Models.SortType?)null;

            var modelSortBy = clientQuery.SortBy.HasValue ?
                TodoItemSortByConverter.Convert(clientQuery.SortBy.Value) :
                (Model.TodoItemSortBy?)null;

            var modelQuery = new Model.TodoItemInfoSearchQuery
            {
                CreatedFrom = clientQuery.CreatedFrom,
                CreatedTo = clientQuery.CreatedTo,
                UserId = modelUserId,
                IsComplited = clientQuery.IsComplited,
                Limit = clientQuery.Limit,
                Offset = clientQuery.Offset,
                Sort = modelSort,
            };

            return modelQuery;
        }
    }
}

