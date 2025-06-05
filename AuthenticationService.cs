// <copyright file="AuthenticationService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WSUFundraiserEngine
{
    /// <summary>
    /// This class represents an authentication service to manage user registration, login and logout.
    /// </summary>
    public static class AuthenticationService
    {
        /// <summary>
        /// This field stores the current user.
        /// </summary>
        private static User currentUser;

        /// <summary>
        /// This field stores the registered users.
        /// </summary>
        private static List<User> registeredUsers = new List<User>();

        /// <summary>
        /// This method Registers a non-existent user.
        /// </summary>
        /// <param name="user">The user to be registerd.</param>
        /// <exception cref="ArgumentException">Occurs when the user with that username exists</exception>
        public static void Register(User user)
        {
            // Check if username already exists
            if (registeredUsers.Any(u => u.Username == user.Username))
            {
                throw new ArgumentException("Username already exists");
            }

            registeredUsers.Add(user);
            currentUser = user;

            // Save to XML database
            XMLDatabase.SaveUsers(registeredUsers);
            Console.WriteLine("User registered successfully");
        }

        /// <summary>
        /// This method logs in a user.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>The user with the matching credentials.</returns>
        /// <exception cref="ArgumentException">Occurs when the credentials are wrong.</exception>
        public static User Login(string username, string password)
        {
            // load users from XML database if not already loaded
            if (registeredUsers.Count == 0)
            {
                registeredUsers = XMLDatabase.LoadUsers();
            }

            // find user with matching username and password
            User user = registeredUsers.FirstOrDefault(u => u.Username == username && u.Password == password); // LINQ expression for finding user with matching username and password

            if (user == null)
            {
                throw new ArgumentException("Invalid username or password");
            }

            currentUser = user;

            Console.WriteLine($"User '{username}' logged in successfully");
            return currentUser;
        }

        /// <summary>
        /// This method logs out a user.
        /// </summary>
        /// <param name="user">The user to be logged out.</param>
        public static void Logout(User user)
        {
            if ((user == currentUser)
                && (currentUser != null)
                && registeredUsers.Contains(currentUser))
            {
                Console.WriteLine($"User '{user.Username}' logged out successfully");
                currentUser = null;
            }
        }

        /// <summary>
        /// This method gets the current user.
        /// </summary>
        /// <returns>The current user at the time.</returns>
        public static User GetCurrentUser()
        {
            return currentUser;
        }
    }
}
