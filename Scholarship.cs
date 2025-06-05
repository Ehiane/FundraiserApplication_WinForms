// <copyright file="Scholarship.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WSUFundraiserEngine
{
    /// <summary>
    /// This class represents a scholarship.
    /// </summary>
    public class Scholarship
    {
        /// <summary>
        /// Gets or sets the name of the scholarship.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the scholarship.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the status of the scholarship is open (True) or closed (False).
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets the criteria for the scholarship.
        /// </summary>
        public List<string> Criteria { get; private set; } = new List<string>();

        /// <summary>
        /// Gets the applicants for the scholarship.
        /// </summary>
        public List<User> Applicants { get; private set; } = new List<User>();

        /// <summary>
        /// Gets the awarded students for the scholarship.
        /// </summary>
        public List<User> AwardedStudents { get; private set; } = new List<User>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Scholarship"/> class.
        /// </summary>
        /// <param name="name">The name of the scholarship.</param>
        /// <param name="description">The description of the scholarship.</param>
        /// <param name="status">The status of the scholarshinp. </param>
        /// <param name="criteria">The criteria of the scholarship.</param>
        public Scholarship(string name, string description, string status, List<string> criteria)
        {
            this.Name = name;
            this.Description = description;
            this.Status = status;
            this.Criteria = criteria;
        }

        /// <summary>
        /// This method allows a user to apply for a scholarship.
        /// </summary>
        /// <param name="user">The user to be added to the applicants.</param>
        public void Apply(User user)
        {
            if (this.Applicants.Contains(user) || user == null)
            {
                if (user == null)
                {
                    throw new ArgumentNullException("User does not exist.");
                }

                Console.WriteLine($"{user.Name} has already applied for {this.Name}");
            }
            else
            {
                this.Applicants.Add(user);
                Console.WriteLine($"{user.Name} has applied for {this.Name}");
            }
        }

        /// <summary>
        /// This method allows a user to be awarded a scholarship.
        /// </summary>
        /// <param name="user">The user to be awarded the scholarship.</param>
        public void Award(User user)
        {
            if (this.AwardedStudents.Contains(user))
            {
                Console.WriteLine($"{user.Name} has already been awarded {this.Name}");
            }
            else
            {
                this.AwardedStudents.Add(user);
                Console.WriteLine($"{user.Name} has been awarded {this.Name}");
            }
        }

        /// <summary>
        /// This method allows a user to be removed from the awarded students.
        /// </summary>
        /// <returns>The stringified verision of this entity.</returns>
        public override string ToString()
        {
            return this.Name;
        }
    }
}
