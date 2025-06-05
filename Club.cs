// <copyright file="Club.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WSUFundraiserEngine
{
    /// <summary>
    /// This class represents a club.
    /// </summary>
    public class Club
    {
        /// <summary>
        /// Gets or Sets the name of the club.
        /// </summary>
        public string Name { get; set; } // Name of the club

        /// <summary>
        /// Gets or Sets the description of the club.
        /// </summary>
        public string Description { get; set; } // Description of the club

        /// <summary>
        /// Gets the members of the club.
        /// </summary>
        public List <Student> Members { get; private set; } // List of members in the club

        /// <summary>
        /// Gets the projects of the club.
        /// </summary>
        public List <Project> Projects { get; private set; }// List of projects in the club

        /// <summary>
        /// Gets or Sets the start date of the club.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or Sets the end date of the club.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Club"/> class.
        /// </summary>
        /// <param name="name">Name of the club.</param>
        /// <param name="description">Description of the club.</param>
        public Club(string name, string description)
        {
            this.Name = name;
            this.Description = description;
            this.Members = new List<Student>();
            this.Projects = new List<Project>();
        }

        /// <summary>
        /// This method allows a student to join a club.
        /// </summary>
        /// <param name="student">The student to join the club.</param>
        /// <exception cref="ArgumentException">Occurs when the student is already a member.</exception>
        public void AddMember(Student student)
        {
            if (!this.Members.Contains(student))
            {
                this.Members.Add(student);
            }
            else
            {
                throw new ArgumentException("Student is already a member of this club.");
            }
        }

        /// <summary>
        /// This method allows a student to leave a club.
        /// </summary>
        /// <param name="student">The student to join the club.</param>
        /// <exception cref="ArgumentException">Occurs when the student is not a member.</exception>
        public void RemoveMember(Student student)
        {
            if (this.Members.Contains(student))
            {
                this.Members.Remove(student);
            }
            else
            {
                throw new ArgumentException("Student is not a member of this club.");
            }
        }

        /// <summary>
        /// This method allows a project to be added to the club.
        /// </summary>
        /// <param name="project"></param>
        public void AddProject(Project project)
        {
            if (!this.Projects.Contains(project))
            {
                this.Projects.Add(project);
            }
        }

        /// <summary>
        /// This method allows the start date of the club to be updated.
        /// </summary>
        /// <param name="startDate">The start date of a club.</param>
        public void UpdateStartDate(DateTime startDate)
        {
            this.StartDate = startDate;
        }

        /// <summary>
        /// This method allows the end date of the club to be updated.
        /// </summary>
        /// <param name="endDate">The end date of the club</param>
        public void UpdateEndDate(DateTime endDate)
        {
            this.EndDate = endDate;
        }

        /// <summary>
        /// This method allows a project to be removed from the club.
        /// </summary>
        /// <param name="project">The project being referenced.</param>
        /// <exception cref="ArgumentException">Occurs when the project has no relation to the club.</exception>
        public void RemoveProject(Project project)
        {
            if (this.Projects.Contains(project))
            {
                this.Projects.Remove(project);
            }
            else
            {
                throw new ArgumentException("This project has no relation with this club.");
            }
        }

        /// <summary>
        /// This method allows the description of the club to be updated.
        /// </summary>
        /// <returns>The stringified version of the entity.</returns>
        public override string ToString()
        {
            //return $"Club: {this.Name}, Description: {this.Description}, Members: {string.Join(",", this.Members)}, Projects:{string.Join(",", this.Projects)} ";
            return $"{this.Name} ";
        }
    }
}
