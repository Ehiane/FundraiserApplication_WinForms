// <copyright file="ProjectTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WSUFundraiserTests
{
    using WSUFundraiserEngine;

    /// <summary>
    /// This class contains tests for the Project class.
    /// </summary>
    public class ProjectTests
    {
        private Project project;

        /// <summary>
        /// This method is called before each test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            XMLDatabase.CleanDatabase();
            this.project = new Project("Community Garden", "Help fund a local garden.", 1000);
            XMLDatabase.SaveProjects(new List<Project> { this.project });
        }

        /// <summary>
        /// This method is called after each test.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            XMLDatabase.CleanDatabase();
            this.project = null;
        }

        /// <summary>
        /// This method tests the normal case of adding a donation to a project.
        /// </summary>
        [Test]
        public void Project_NormalCase_AddDonation()
        {
            var donor = new Donor("Jane Smith", "jane@example.com", "janesmith", "password456", "D12345");
            XMLDatabase.SaveUsers(new List<User> { donor });
            var donation = new Donation(donor, this.project, 500, false);
            this.project.AddDonation(donation);
            XMLDatabase.SaveProjects(new List<Project> { this.project });
            var loadedProjects = XMLDatabase.LoadProjects();
            Assert.That(loadedProjects[0].RaisedAmount, Is.EqualTo(500));
        }

        /// <summary>
        /// This method tests the boundary case of updating the target amount of a project to zero.
        /// </summary>
        [Test]
        public void Project_BoundaryCase_UpdateTargetAmountToZero()
        {
            this.project.UpdateTargetAmount(-1000);
            XMLDatabase.SaveProjects(new List<Project> { this.project });
            var loadedProjects = XMLDatabase.LoadProjects();
            Assert.That(loadedProjects[0].TargetAmount, Is.EqualTo(0));
        }

        /// <summary>
        /// This method tests the exceptional case of adding a donation to a project with a null donor.
        /// </summary>
        [Test]
        public void Project_ExceptionalCase_RemoveMemberNotInProject()
        {
            var student = new Student("John Doe", "john@example.com", "johndoe", "password123", "CS", "WSU123", "2025");
            Assert.Throws<ArgumentException>(() => this.project.RemoveMember(student));
        }
    }
}
