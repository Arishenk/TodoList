namespace Notes.Models.TodoItems.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Data.SQLite;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Хранилище заметок в памяти
    /// 
    /// Потоконебезопасно
    /// </summary>
    public sealed class DatabaseItemRepository : ITodoItemRepository
    {
        private readonly Dictionary<Guid, TodoItem> primaryKeyIndex;
        private readonly List<TodoItemInfo> sequenceIndex;
        private readonly List<TodoItem> removedNotes;

        /// <summary>
        /// Создает новый экземпляр хранилища заметок в памяти
        /// </summary>
        public DatabaseItemRepository()
        {
            this.primaryKeyIndex = new Dictionary<Guid, TodoItem>();
            this.sequenceIndex = new List<TodoItemInfo>();
            this.removedNotes = new List<TodoItem>();
            string CurrentDir = @"D:\univer\6 sem\tok\TokenApp\TokenApp\todoList.db";

            using (SQLiteConnection connect = new SQLiteConnection(string.Format("Data Source={0}", CurrentDir)))
            {
                using (SQLiteCommand command = connect.CreateCommand())
                {
                    connect.Open();
                    command.CommandText = @"CREATE TABLE IF NOT EXISTS 'items' (id INTEGER PRIMARY KEY, itemid TEXT, userid TEXT, createdat TEXT, lastupdatedat TEXT, iscomplited TEXT, title TEXT, 
                                    deadline TEXT, text TEXT)";
                    command.ExecuteNonQuery();
                    connect.Close();
                }
            }
        }

        /// <summary>
        /// Создать новую задачу
        /// </summary>
        /// <param name="creationInfo">Информация для создания задачи</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Задача, представляющая асинхронное создание задачи. Результат выполнения операции - информация о созданной задаче</returns>
        public Task<TodoItemInfo> CreateAsync(TodoItemCreationInfo creationInfo, CancellationToken cancellationToken)
        {
            if (creationInfo == null)
            {
                throw new ArgumentNullException(nameof(creationInfo));
            }

            cancellationToken.ThrowIfCancellationRequested();

            var id = Guid.NewGuid();
            var now = DateTime.UtcNow;

            var item = new TodoItem
            {
                Id = id,
                UserId = creationInfo.UserId,
                CreatedAt = now,
                LastUpdatedAt = now,
                IsComplited = false,
                Title = creationInfo.Title,
                Text = creationInfo.Text,
                DeadLine = creationInfo.DeadLine
            };

            this.primaryKeyIndex.Add(id, item);
            this.sequenceIndex.Add(item);

            string CurrentDir = @"D:\univer\6 sem\tok\TokenApp\TokenApp\todoList.db";
            using (SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};", CurrentDir)))
            {
                connection.Open();
                using (var command = new SQLiteCommand("INSERT INTO 'items' ('itemid', 'userid', 'createdat', 'lastupdatedat', 'iscomplited', 'title', 'text', 'deadline') VALUES ('" + item.Id.ToString()
                    + "', '" + item.UserId.ToString() + "','" + item.CreatedAt.ToString() + "', '" + item.LastUpdatedAt.ToString() + "', '" + item.IsComplited.ToString() + "', '" + item.Title + "', '" 
                    + item.Text + "', '" + item.DeadLine.ToString() + "');", connection))
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }

            return Task.FromResult<TodoItemInfo>(item);
        }

        /// <summary>
        /// Запросить задачу
        /// </summary>
        /// <param name="itemId">Идентификатор задачи</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Задача, представлящая асинхронный запрос задачи. Результат выполнения - задача</returns>
        public TodoItem GetAsync(Guid itemId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            string CurrentDir = @"D:\univer\6 sem\tok\TokenApp\TokenApp\todoList.db";
            SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};", CurrentDir));
            connection.Open();
            using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM 'items' WHERE itemid = '" + itemId.ToString() + "';", connection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    foreach (DbDataRecord record in reader)
                    {
                        Guid.TryParse(record["userid"].ToString(), out var usrId);
                        DateTime.TryParse(record["createdat"].ToString(), out var date);
                        DateTime.TryParse(record["deadline"].ToString(), out var deadLine);
                        bool.TryParse(record["iscomplited"].ToString(), out var isComplited);
                        DateTime.TryParse(record["lastupdatedat"].ToString(), out var update);
                        return new TodoItem
                        {
                            Id = itemId,
                            UserId = usrId,
                            CreatedAt = date,
                            DeadLine = deadLine,
                            IsComplited = isComplited,
                            LastUpdatedAt = update,
                            Text = record["text"].ToString(),
                            Title = record["title"].ToString()
                        };
                     }
                }
            }

            throw new TodoItemNotFoundExcepction(itemId);
        }

        /// <summary>
        /// Изменить заметку
        /// </summary>
        /// <param name="patchInfo">Описание изменений заметки</param>
        /// <param name="cancelltionToken">Токен отмены операции</param>
        /// <returns>Задача, представляющая асинхронный запрос на изменение заметки. Результат выполнения - актуальное состояние заметки</returns>
        public Task<TodoItem> PatchAsync(TodoItemPatchInfo patchInfo, CancellationToken cancelltionToken)
        {
            if (patchInfo == null)
            {
                throw new ArgumentNullException(nameof(patchInfo));
            }

            cancelltionToken.ThrowIfCancellationRequested();

            //if (!this.primaryKeyIndex.TryGetValue(patchInfo.ItemId, out var item))
            //{
            //    throw new TodoItemNotFoundExcepction(patchInfo.ItemId);
            //}

            var item = GetAsync(patchInfo.ItemId, cancelltionToken);
           
            var updated = false;

            string CurrentDir = @"D:\univer\6 sem\tok\TokenApp\TokenApp\todoList.db";

            if (patchInfo.Title != null)
            {
                using (SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};", CurrentDir)))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(string.Format("UPDATE 'items' SET title = '{0}' WHERE itemid = '{1}';", patchInfo.Title, patchInfo.ItemId.ToString()), connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    connection.Close();
                }

                item.Title = patchInfo.Title;
                updated = true;
            }

            if (patchInfo.Text != null)
            {
                using (SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};", CurrentDir)))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(string.Format("UPDATE 'items' SET text = '{0}' WHERE itemid = '{1}';", patchInfo.Text, patchInfo.ItemId.ToString()), connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    connection.Close();
                }
                item.Text = patchInfo.Text;
                updated = true;
            }

            if (patchInfo.IsComplited != null)
            {
                using (SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};", CurrentDir)))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(string.Format("UPDATE 'items' SET 'iscomplited' = '{0}' WHERE itemid = '{1}';", patchInfo.IsComplited.ToString(), patchInfo.ItemId.ToString()), connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    connection.Close();
                }
                item.IsComplited = patchInfo.IsComplited.Value;
                updated = true;
            }

            if (updated)
            {
                item.LastUpdatedAt = DateTime.UtcNow;
                using (SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};", CurrentDir)))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(string.Format("UPDATE 'items' SET 'lastupdatedat' = '{0}' WHERE itemid = '{1}';", item.LastUpdatedAt.ToString(), patchInfo.ItemId.ToString()), connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    connection.Close();
                }
            }

            return Task.FromResult(item);
        }

        /// <summary>
        /// Удалить задачу
        /// </summary>
        /// <param name="itemId">Идентификатор задачи</param>
        /// <param name="cancelltionToken">Токен отмены операции</param>
        /// <returns>Задача, представляющая асинхронный запрос на удаление задачи</returns>
        public Task RemoveAsync(Guid itemId, CancellationToken cancelltionToken)
        {
            cancelltionToken.ThrowIfCancellationRequested();

            string CurrentDir = @"D:\univer\6 sem\tok\TokenApp\TokenApp\todoList.db";

            using (SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};", CurrentDir)))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(string.Format("DELETE FROM 'items' WHERE itemid = '{0}';", itemId.ToString()), connection))
                {
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Найти задачи
        /// </summary>
        /// <param name="query">Поисковый запрос</param>
        /// <param name="cancelltionToken">Токен отмены операции</param>
        /// <returns>Задача, представляющая асинхронный поиск задач. Результат выполнения - список найденных задач</returns>
        public Task<IReadOnlyList<TodoItemInfo>> SearchAsync(TodoItemInfoSearchQuery query, CancellationToken cancelltionToken)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            cancelltionToken.ThrowIfCancellationRequested();

            List<TodoItem> search = new List<TodoItem>();
            string CurrentDir = @"D:\univer\6 sem\tok\TokenApp\TokenApp\todoList.db";

            SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};", CurrentDir));
            connection.Open();
            using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM 'items';", connection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    foreach (DbDataRecord record in reader)
                    {
                        Guid.TryParse(record["userid"].ToString(), out var usrId);
                        Guid.TryParse(record["itemid"].ToString(), out var id);
                        DateTime.TryParse(record["createdat"].ToString(), out var date);
                        DateTime.TryParse(record["deadline"].ToString(), out var deadLine);
                        bool.TryParse(record["iscomplited"].ToString(), out var isComplited);
                        DateTime.TryParse(record["lastupdatedat"].ToString(), out var update);
                        search.Add(new TodoItem
                        {
                            Id = id,
                            UserId = usrId,
                            CreatedAt = date,
                            DeadLine = deadLine,
                            IsComplited = isComplited,
                            LastUpdatedAt = update,
                            Text = record["text"].ToString(),
                            Title = record["title"].ToString()
                        });
                    }
                }
            }

            IEnumerable<TodoItem> search1 = search;

            if (query.CreatedFrom != null)
            {
                search1 = search1.Where(note => note.CreatedAt >= query.CreatedFrom.Value);
            }

            if (query.CreatedTo != null)
            {
                search1 = search1.Where(note => note.CreatedAt <= query.CreatedTo.Value);
            }

            if (query.UserId != null)
            {
                search1 = search1.Where(note => note.UserId == query.UserId.Value);
            }

            if (query.IsComplited != null)
            {
                search1 = search1.Where(note => note.IsComplited = query.IsComplited.Value);
            }

           
            if (query.Offset != null)
            {
                search1 = search1.Take(query.Offset.Value);
            }

            if (query.Limit != null)
            {
                search1 = search1.Take(query.Limit.Value);
            }

            var result = search1.ToList();

            return Task.FromResult<IReadOnlyList<TodoItemInfo>>(result);
        }
    }
}

