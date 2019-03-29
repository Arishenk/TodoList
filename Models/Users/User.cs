namespace Notes.Models.Users
{
    using System;

    /// <summary>
    /// Пользователь
    /// </summary>
    public class User1
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Логин пользователя
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Хэш пароля
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// Дата регистрации пользователя
        /// </summary>
        public DateTime RegisteredAt { get; set; }

        /// <summary>
        /// Prava пользователя
        /// </summary>
        public string Role { get; set; }
    }
}
