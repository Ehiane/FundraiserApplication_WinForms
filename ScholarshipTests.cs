// <copyright file="ScholarshipTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WSUFundraiserTests
{
    using System;
    using System.Collections.Generic;
    using WSUFundraiserEngine;

    /// <summary>
    /// This class contains tests for the Scholarship class.
    /// </summary>
    public class ScholarshipTests
    {
        private Scholarship scholarship;

        /// <summary>
        /// This method is called before each test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            XMLDatabase.CleanDatabase();
            this.scholarship = new Scholarship("STEM Scholarship", "For STEM students.", "Open", new List<string> { "GPA > 3.5" });
            XMLDatabase.SaveScholarships(new List<Scholarship> { this.scholarship });
        }

        /// <summary>
        /// This method is called after each test.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            XMLDatabase.CleanDatabase();
            this.scholarship = null;
        }

        /// <summary>
        /// This method tests the normal case of applying for a scholarship.
        /// </summary>
        [Test]
        public void Scholarship_NormalCase_Apply()
        {
            var student = new Student("John Doe", "john@example.com", "johndoe", "password123", "CS", "WSU123", "2025");
            XMLDatabase.SaveUsers(new List<User> { student });
            this.scholarship.Apply(student);
            XMLDatabase.SaveScholarships(new List<Scholarship> { this.scholarship });
            var loadedScholarships = XMLDatabase.LoadScholarships();
            Assert.IsTrue(loadedScholarships[0].Applicants.Exists(a => a.Username == student.Username));
        }

        /// <summary>
        /// This method tests the boundary case of awarding a scholarship to the same student twice.
        /// </summary>
        [Test]
        public void Scholarship_BoundaryCase_AwardSameStudentTwice()
        {
            var student = new Student("John Doe", "john@example.com", "johndoe", "password123", "CS", "WSU123", "2025");
            this.scholarship.Award(student);

            using (var consoleOutput = new System.IO.StringWriter())
            {
                Console.SetOut(consoleOutput);

                // Attempt to award the same student again
                this.scholarship.Award(student);

                // Assert
                string output = consoleOutput.ToString();
                Assert.IsTrue(output.Contains($"{student.Name} has already been awarded {this.scholarship.Name}"), "Expected message not found in the console output.");
            }
        }

        /// <summary>
        /// This method tests the exceptional case of applying for a scholarship with a null student.
        /// </summary>
        [Test]
        public void Scholarship_ExceptionalCase_ApplyNullStudent()
        {
            Assert.Throws<ArgumentNullException>(() => this.scholarship.Apply(null));
        }
    }
}
