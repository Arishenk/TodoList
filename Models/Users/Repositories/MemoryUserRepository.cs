namespace Notes.Models.Users.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Data.SQLite;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Хранилище пользователей в памяти
    /// 
    /// Потоконебезопасен
    /// </summary>
    public class MemoryUserRepository : IUserRepository
    {
        private readonly Dictionary<Guid, User1> primaryKeyIndex;
        private readonly Dictionary<string, User1> loginIndex;

        /// <summary>
        /// Инициализирует новый эксземлпляр хранилища пользователей в памяти
        /// </summary>
        public MemoryUserRepository()
        {
            this.primaryKeyIndex = new Dictionary<Guid, User1>();
            this.loginIndex = new Dictionary<string, User1>(StringComparer.InvariantCultureIgnoreCase);
            string CurrentDir = @"D:\univer\6 sem\tok\TokenApp\TokenApp\todoList.db";

            using (SQLiteConnection connect = new SQLiteConnection(string.Format("Data Source={0}", CurrentDir)))
            {
                using (SQLiteCommand command = connect.CreateCommand())
                {
                    connect.Open();
                    command.CommandText = @"CREATE TABLE IF NOT EXISTS 'users' (id INTEGER PRIMARY KEY, userid TEXT, login TEXT, password TEXT, registeredat TEXT)";
                    command.ExecuteNonQuery();
                    connect.Close();
                }
            }
        }

        /// <summary>
        /// Создать нового пользователя
        /// </summary>
        /// <param name="creationInfo">Данные для создания нового пользователя</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Созданный пользователь</returns>
        public User1 CreateAsync(UserCreationInfo creationInfo)
        {
            if (creationInfo == null)
            {
                throw new ArgumentNullException(nameof(creationInfo));
            }

            //cancellationToken.ThrowIfCancellationRequested();

            //if (this.loginIndex.ContainsKey(creationInfo.Login))
            //{
            //    throw new UserDuplicationException(creationInfo.Login);
            //}

            var id = Guid.NewGuid();
            var now = DateTime.UtcNow;

            var user = new User1
            {
                Id = id,
                Login = creationInfo.Login,
                PasswordHash = creationInfo.PasswodHash,
                RegisteredAt = now
            };

            string CurrentDir = @"D:\univer\6 sem\tok\TokenApp\TokenApp\todoList.db";
            using (SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};", CurrentDir)))
            {
                connection.Open();
                using (var command2 = new SQLiteCommand("INSERT INTO 'users' ('userid', 'login', 'password', 'registeredat') VALUES ('" + user.Id.ToString() + "', '" + user.Login + "', '" 
                    + user.PasswordHash + "', '" + user.RegisteredAt.ToString() + "');", connection))
                {
                    var a = command2.ExecuteNonQuery();
                    connection.Close();
                }
            }

            //this.primaryKeyIndex.Add(id, user);
            //this.loginIndex.Add(user.Login, user);

            return user;
        }

        /// <summary>
        /// Получить пользователя по идентификатору
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Пользователь</returns>
        public User1 GetAsync(Guid userId)
        {
            //cancellationToken.ThrowIfCancellationRequested();

            //if (!this.primaryKeyIndex.TryGetValue(userId, out var user))
            //{
            //    throw new UserNotFoundException(userId);
            //}

            string CurrentDir = @"D:\univer\6 sem\tok\TokenApp\TokenApp\todoList.db";
            SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};", CurrentDir));
            connection.Open();
            using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM 'users' WHERE userid = '" + userId.ToString() + "';", connection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    foreach (DbDataRecord record in reader)
                    {
                        var log = record["login"].ToString();
                        var Password = record["password"].ToString();
                        var date = record["registeredat"].ToString();
                        DateTime.TryParse(date, out DateTime dateTime);
                        // var hashPassword = GetHashString(password.Text);
                        return new User1 { Login = log, PasswordHash = Password, Id = userId, RegisteredAt = dateTime };
                    }
                }
            }

            throw new UserNotFoundException(userId);
        }

        /// <summary>
        /// Получить пользователя по логину
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Пользователь</returns>
        public User1 GetAsync(string login)
        {
            if (login == null)
            {
                throw new ArgumentNullException(nameof(login));
            }

            //cancellationToken.ThrowIfCancellationRequested();

            string CurrentDir = @"D:\univer\6 sem\tok\TokenApp\TokenApp\todoList.db";
            
            SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};", CurrentDir));
            connection.Open();
            using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM 'users';", connection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    foreach (DbDataRecord record in reader)
                    {
                       
                        var log = record["login"].ToString();
                       
                        bool loginIsFound = login.Equals(log);
                        if (loginIsFound)
                        {
                            var id = Guid.Parse(record["userid"].ToString());
                            var Password = record["password"].ToString();
                            var date = record["registeredat"].ToString();
                            DateTime.TryParse(date, out DateTime dateTime);
                            return new User1 { Login = login, PasswordHash = Password, Id = id, RegisteredAt = dateTime };
                        }

                    }
                }
            }
            throw new UserNotFoundException(login);

        }
    }
}
