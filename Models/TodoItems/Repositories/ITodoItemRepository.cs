namespace Notes.Models.TodoItems.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Интерфейс хранилища задач
    /// </summary>
    public interface ITodoItemRepository
    {
        /// <summary>
        /// Создать новую задачу
        /// </summary>
        /// <param name="creationInfo">Информация для создания задачи</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Задача, представляющая асинхронное создание задач. Результат выполнения операции - информация о созданной задаче</returns>
        Task<TodoItemInfo> CreateAsync(TodoItemCreationInfo creationInfo, CancellationToken cancellationToken);

        /// <summary>
        /// Найти задачи
        /// </summary>
        /// <param name="query">Поисковый запрос</param>
        /// <param name="cancelltionToken">Токен отмены операции</param>
        /// <returns>Задача, представляющая асинхронный поиск задач. Результат выполнения - список найденных задач</returns>
        Task<IReadOnlyList<TodoItemInfo>> SearchAsync(TodoItemInfoSearchQuery query, CancellationToken cancelltionToken);

        /// <summary>
        /// Запросить задачу
        /// </summary>
        /// <param name="itemId">Идентификатор задачи</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Задача, представлящая асинхронный запрос задачи. Результат выполнения - задача</returns>
        TodoItem GetAsync(Guid itemId, CancellationToken cancellationToken);

        /// <summary>
        /// Изменить задачу
        /// </summary>
        /// <param name="patchInfo">Описание изменений задачи</param>
        /// <param name="cancelltionToken">Токен отмены операции</param>
        /// <returns>Задача, представляющая асинхронный запрос на изменение задачи. Результат выполнения - актуальное состояние задачи</returns>
        Task<TodoItem> PatchAsync(TodoItemPatchInfo patchInfo, CancellationToken cancelltionToken);

        /// <summary>
        /// Удалить задачу
        /// </summary>
        /// <param name="itemId">Идентификатор задачи</param>
        /// <param name="cancelltionToken">Токен отмены операции</param>
        /// <returns>Задача, представляющая асинхронный запрос на удаление задачи</returns>
        Task RemoveAsync(Guid itemId, CancellationToken cancelltionToken);
    }
}
