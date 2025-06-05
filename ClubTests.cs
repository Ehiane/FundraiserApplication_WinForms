// <copyright file="ClubTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WSUFundraiserTests
{
    using System;
    using System.Collections.Generic;
    using WSUFundraiserEngine;

    /// <summary>
    /// This class contains tests for the Club class.
    /// </summary>
    [TestFixture]
    public class ClubTests
    {
        private Club club;
        private Student student;

        /// <summary>
        /// This method is called before each test.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            XMLDatabase.CleanDatabase();
            this.club = new Club("Art Club", "A club for art enthusiasts");
            this.student = new Student("John Doe", "john@example.com", "johndoe", "password123", "CS", "WSU123", "2025");
            XMLDatabase.SaveUsers(new List<User> { this.student });
            XMLDatabase.SaveClubs(new List<Club> { this.club });
        }

        /// <summary>
        /// This method is called after each test.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            XMLDatabase.CleanDatabase();
            this.club = null;
            this.student = null;
        }

        /// <summary>
        /// This method tests the normal case of adding a member to a club.
        /// </summary>
        [Test]
        public void Club_NormalCase_AddMember()
        {
            this.club.AddMember(this.student);
            XMLDatabase.SaveClubs(new List<Club> { this.club });
            var loadedClubs = XMLDatabase.LoadClubs();
            Assert.IsTrue(loadedClubs[0].Members.Exists(m => m.Username == this.student.Username));
        }

        /// <summary>
        /// This method tests the boundary case of removing the last member from a club.
        /// </summary>
        [Test]
        public void Club_BoundaryCase_RemoveMemberLastMember()
        {
           this.club.AddMember(this.student);
           XMLDatabase.SaveClubs(new List<Club> { this.club });
           this.club.RemoveMember(this.student);
           XMLDatabase.SaveClubs(new List<Club> { this.club });
           var loadedClubs = XMLDatabase.LoadClubs();
           Assert.IsFalse(loadedClubs[0].Members.Exists(m => m.Username == this.student.Username), "Last member was not removed properly");
        }

        /// <summary>
        /// This method tests the exceptional case of adding a duplicate member to a club.
        /// </summary>
        [Test]
        public void Club_ExceptionalCase_AddDuplicateMember()
        {
            this.club.AddMember(this.student);
            Assert.Throws<ArgumentException>(() => this.club.AddMember(this.student));
        }
    }
}