// <copyright file="Guest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WSUFundraiserEngine
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// This class represents a guest user.
    /// </summary>
    public class Guest : User
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Guest"/> class.
        /// </summary>
        /// <param name="name">The name of the Guest.</param>
        /// <param name="email">Guest's email.</param>
        /// <param name="username">Guest's username.</param>
        /// <param name="password">Guest's password.</param>
        public Guest(string name, string email, string username, string password)
            : base(name, email, username, password)
        {
        }

        /// <summary>
        /// This method allows a guest to view projects.
        /// </summary>
        /// <param name="projects">avialble projects.</param>
        public void ViewProjects(List<Project> projects)
        {
            foreach (var project in projects)
            {
                Console.WriteLine($"Project: {project.Name}, Target: ");
            }
        }

        /// <summary>
        /// This method allows a guest to view donations.
        /// </summary>
        /// <param name="donations">avialble donations.</param>
        public void ViewDonations(List<Donation> donations)
        {
            foreach (var donation in donations)
            {
                Console.WriteLine($"Donor: {donation.Donor.Name}, Project: {donation.Project.Name}, Amount: {donation.Amount}");
            }
        }

        /// <summary>
        /// This method allows a guest to view clubs.
        /// </summary>
        /// <param name="clubs">avialble clubs.</param>
        public void ViewClubs(List<Club> clubs)
        {
            foreach (var club in clubs)
            {
                Console.WriteLine($"Club: {club.Name}");
            }
        }

        /// <summary>
        /// this method allows a guest to view scholarships.
        /// </summary>
        /// <param name="scholarships">avialble scholarships.</param>
        public void ViewScholarships(List<Scholarship> scholarships)
        {
            foreach (var scholarship in scholarships)
            {
                Console.WriteLine($"Scholarship: {scholarship.Name}");
            }
        }
    }
}
