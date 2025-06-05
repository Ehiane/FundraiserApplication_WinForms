// <copyright file="Student.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WSUFundraiserEngine
{
    /// <summary>
    /// This class represents a student.
    /// </summary>
    public class Student : User
    {
        /// <summary>
        /// Gets or sets the major of the student.
        /// </summary>
        public string Major { get; set; }

        /// <summary>
        /// gets the clubs that the student is in.
        /// </summary>
        public List<Club> Clubs { get; private set; }

        /// <summary>
        /// Gets or sets the graduation year of the student.
        /// </summary>
        public string GraduationYear { get; set; }

        /// <summary>
        /// Gets the WSU ID of the student.
        /// </summary>
        public string WsuId { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Student"/> class.
        /// </summary>
        /// <param name="name">The name of the Student.</param>
        /// <param name="email">Email of the Student.</param>
        /// <param name="username">Username of the Student.</param>
        /// <param name="password">Password of the Student.</param>
        /// <param name="major">Major of the Student.</param>
        /// <param name="wsuId">Id of the student.</param>
        /// <param name="graduationYear">The Graduation year of the student.</param>
        public Student(string name, string email, string username, string password, string major, string wsuId, string graduationYear)
            : base(name, email, username, password)
        {
            this.Major = major;
            this.WsuId = wsuId;
            this.GraduationYear = graduationYear;
            this.Clubs = new List<Club>();
            this.GraduationYear = graduationYear;
        }

        /// <summary>
        /// This method allows a student to join a club.
        /// </summary>
        /// <param name="club">The name of the club to join.</param>
        public void JoinClub(Club club)
        {
            if (!this.Clubs.Contains(club))
            {
                this.Clubs.Add(club);
                Console.WriteLine($"Student {this.Name} joined {club.Name}");
            }
        }

        /// <summary>
        /// This method allows a student to leave a club.
        /// </summary>
        /// <param name="club"> The name of the club to leave</param>
        public void LeaveClub(Club club)
        {
            if (this.Clubs.Contains(club))
            {
                this.Clubs.Remove(club);
                Console.WriteLine($"Student {this.Name} left {club.Name}");
            }
            else
            {
                Console.WriteLine($"Student {this.Name} is not in {club.Name}");
                return;
            }
        }

        /// <summary>
        /// This updates the WSU ID of the student.
        /// </summary>
        /// <param name="wsuId">the new wsuId.</param>
        public void UpdateWsuId(string wsuId)
        {
            this.WsuId = wsuId;
        }

        /// <summary>
        /// To string method for a Student.
        /// </summary>
        /// <returns> The stringified version od this student instance. </returns>
        public override string ToString()
        {
            return $"Student:\n Name: {this.Name}, WSU ID: {this.WsuId}, Major: {this.Major}, Graduation Year: {this.GraduationYear}, Username: {this.Username}, Email: {this.Email}";
        }

        /// <summary>
        /// Customm equals method for a student.
        /// </summary>
        /// <param name="obj">The student in question.</param>
        /// <returns>True if student matches.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Student other)
            {
                return this.WsuId == other.WsuId; // Compare based on WSU ID
            }

            return false;
        }
    }
}
