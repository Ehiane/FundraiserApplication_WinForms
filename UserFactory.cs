// <copyright file="UserFactory.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WSUFundraiserEngine
{
    using System;

    /// <summary>
    /// This class represents a user factory.
    /// </summary>
    public class UserFactory
    {
        /// <summary>
        /// This method creates a user based on the user type.
        /// </summary>
        /// <param name="userType">The type of user that is about to be created.</param>
        /// <param name="name">The user's name.</param>
        /// <param name="email">The user's email.</param>
        /// <param name="username">The user's username.</param>
        /// <param name="password">The user's password.</param>
        /// <param name="extraInfo">The user specif details based on the user type.</param>
        /// <returns>The intended user.</returns>
        /// <exception cref="ArgumentException">Occurs when an invalid user type is being referenced.</exception>
        public static User CreateUser(string userType, string name, string email, string username, string password, string extraInfo = null)
        {
            switch (userType.ToLower())
            {
                case "student":
                    if (!extraInfo.Contains("WSU ID"))
                    {
                        throw new ArgumentException("Student must have a WSU ID");
                    }
                    string[] studentLabels = { "Major:", "WSU ID:", "Graduation Year:" };
                    string[] studentInfo = ExtractExtraInfo(extraInfo, studentLabels);

                    if (studentInfo.Length < 3)
                    {
                        throw new ArgumentException("Incomplete student information provided");
                    }

                    // Create and return the Student object
                    // where,  0 : Major, 1: WSU ID, 2: Graduation Year
                    return new Student(name, email, username, password, studentInfo[0].Trim(), studentInfo[1].Trim(), studentInfo[2].Trim());

                case "donor":
                    if (!extraInfo.Contains("Donor ID"))
                    {
                        throw new ArgumentException("Donor must have a Donor ID");
                    }

                    string[] donorLabels = { "Donor ID:" };
                    string[] donorInfo = ExtractExtraInfo(extraInfo, donorLabels);

                    if (donorInfo.Length < 1)
                    {
                        throw new ArgumentException("Incomplete donor information provided");
                    }

                    // where, 0 : Donor ID
                    return new Donor(name, email, username, password, donorInfo[0].Trim());

                case "guest":
                    return new Guest(name, email, username, password);
                default:
                    throw new ArgumentException("Invalid user type");
            }
        }

        /// <summary>
        /// This method extracts the extra information from the user's details.
        /// </summary>
        /// <param name="extraInfo">The user specific info</param>
        /// <param name="labelsToRemove">Labels to remove.</param>
        /// <returns></returns>
        private static string[] ExtractExtraInfo(string extraInfo, string[] labelsToRemove)
        {
            foreach (string label in labelsToRemove)
            {
                extraInfo = extraInfo.Replace(label, string.Empty);
            }

            return extraInfo.Split(',');
        }
    }
}
