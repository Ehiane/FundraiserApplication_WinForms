// <copyright file="UserTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WSUFundraiserTests
{
    using System;
    using System.Collections.Generic;
    using WSUFundraiserEngine;

    /// <summary>
    /// This class contains tests for the User class.
    /// </summary>
    [TestFixture]
    public class UserTests
    {
        private Student student;
        private Donor donor;

        /// <summary>
        /// This method is called before each test.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            XMLDatabase.CleanDatabase();
            this.student = new Student("John Doe", "john@example.com", "johndoe", "password123", "CS", "WSU123", "2025");
            this.donor = new Donor("Jane Smith", "jane@example.com", "janesmith", "password456", "D12345");
            XMLDatabase.SaveUsers(new List<User> { this.student, this.donor });
        }

        /// <summary>
        /// This method is called after each test.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            XMLDatabase.CleanDatabase();
            this.student = null;
            this.donor = null;
        }

        /// <summary>
        /// This method tests the normal case of a student joining a club.
        /// </summary>
        [Test]
        public void Student_NormalCase_JoinClub()
        {
            var club = new Club("Art Club", "A club for art enthusiasts");
            this.student.JoinClub(club);
            XMLDatabase.SaveClubs(new List<Club> { club });
            var loadedClubs = XMLDatabase.LoadClubs();
            Assert.IsTrue(loadedClubs.Exists(c => c.Name == "Art Club"));
        }

        /// <summary>
        /// This method tests the boundary case of a donor making a donation of the exact amount needed to reach the target amount of a project.
        /// </summary>
        [Test]
        public void Donor_BoundaryCase_MakeDonationZeroAmount()
        {
            var project = new Project("Community Garden", "Help fund a local garden.", 1000);
            XMLDatabase.SaveProjects(new List<Project> { project });
            this.donor.MakeDonation(project, 0);
            XMLDatabase.SaveProjects(new List<Project> { project });
            var loadedProjects = XMLDatabase.LoadProjects();
            Assert.That(loadedProjects.Find(p => p.Name == "Community Garden").RaisedAmount, Is.EqualTo(0));
        }

        /// <summary>
        /// This method tests the exceptional case of a donor withdrawing more funds than are available in their account.
        /// </summary>
        [Test]
        public void Donor_ExceptionalCase_WithdrawInsufficientFinds()
        {
            Assert.Throws<ArgumentException>(() => this.donor.Withdraw(100));
        }
    }
}