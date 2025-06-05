// <copyright file="User.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace WSUFundraiserEngine
{
    /// <summary>
    /// This class represents a user.
    /// </summary>
    public abstract class User
    {
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the email of the user.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the username of the user.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password of the user.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="name">User's name.</param>
        /// <param name="email">User's email.</param>
        /// <param name="username">User's username.</param>
        /// <param name="password">User's password.</param>
        public User(string name, string email, string username, string password)
        {
            this.Name = name;
            this.Email = email;
            this.Username = username;
            this.Password = password;
        }
    }
}
