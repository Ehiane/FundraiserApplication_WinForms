// <copyright file="Donation.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WSUFundraiserEngine
{
    /// <summary>
    /// This class represents a donation.
    /// </summary>
    public class Donation
    {
        /// <summary>
        /// Gets the donor of the donation.
        /// </summary>
        public Donor Donor { get; private set; }

        /// <summary>
        /// Gets the project of the donation.
        /// </summary>
        public Project Project { get; private set; }

        /// <summary>
        /// Gets the amount of the donation.
        /// </summary>
        public decimal Amount { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the donation is anonymous.
        /// </summary>
        public bool IsAnonymous { get; private set; }

        /// <summary>
        /// Gets the date of the donation.
        /// </summary>
        public DateTime Date { get; private set; } = DateTime.Now;

        /// <summary>
        /// Initializes a new instance of the <see cref="Donation"/> class.
        /// </summary>
        /// <param name="donor">Donor making the donation.</param>
        /// <param name="project">Project benefiting from the donation.</param>
        /// <param name="amount">The amount to be donated.</param>
        /// <param name="anonymous">Indicator to be anonymous or not.</param>
        public Donation(Donor donor, Project project, decimal amount, bool anonymous)
        {
            this.Donor = donor;
            this.Project = project;
            this.Amount = amount;
            this.IsAnonymous = anonymous;
        }
    }
}
