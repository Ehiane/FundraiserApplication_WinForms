// <copyright file="Project.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WSUFundraiserEngine
{
    /// <summary>
    /// this class represents a project.
    /// </summary>
    public class Project
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Project"/> class.
        /// </summary>
        /// <param name="name">Project's name.</param>
        /// <param name="description">Project's description.</param>
        /// <param name="targetAmount">Project's target amount.</param>
        public Project(string name, string description, decimal targetAmount)
        {
            this.Name = name;
            this.Description = description;
            this.TargetAmount = targetAmount;
        }

        /// <summary>
        /// Gets or sets the name of the project.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the project.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or Sets the target amount of the project.
        /// </summary>
        public decimal TargetAmount { get; set; }

        /// <summary>
        /// Gets or Sets the raised amount of the project.
        /// </summary>
        public decimal RaisedAmount { get; set; }

        /// <summary>
        /// Gets or Sets the club of the project.
        /// </summary>
        public Club Club { get; set; }

        /// <summary>
        /// Gets or Sets the donations for the project.
        /// </summary>
        public List<Donation> Donations { get; set; } = new List<Donation>();

        /// <summary>
        /// Gets the members of the project.
        /// </summary>
        public List<User> Members { get; private set; } = new List<User>();

        /// <summary>
        /// Gets or Sets the start date of the project.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or Sets the end date of the project.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// This method allows a donation to be added to the project.
        /// </summary>
        /// <param name="donation">The donation.</param>
        public void AddDonation(Donation donation)
        {
            this.Donations.Add(donation);
            this.RaisedAmount += donation.Amount;
        }

        /// <summary>
        /// This method allows a user to be added to the project.
        /// </summary>
        /// <param name="user">The user.</param>
        public void AddMember(User user)
        {
            this.Members.Add(user);
        }

        /// <summary>
        /// This method allows a user to be removed from the project.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <exception cref="ArgumentException">Occurs when the user is not a member.</exception>
        public void RemoveMember(User user)
        {
            if (this.Members.Contains(user))
            {
                this.Members.Remove(user);
            }
            else
            {
                throw new ArgumentException("This user is not a member");
            }
        }

        /// <summary>
        /// This method allows a donation to be removed from the project.
        /// </summary>
        /// <param name="amount">the new amount to be raised.</param>
        public void UpdateRaisedAmount(decimal amount)
        {
            this.RaisedAmount += amount;
        }

        /// <summary>
        /// This method allows the target amount to be updated.
        /// </summary>
        /// <param name="amount">The new target amount.</param>
        public void UpdateTargetAmount(decimal amount)
        {
            this.TargetAmount += amount;
        }

        /// <summary>
        /// This method allows the name of the project to be updated.
        /// </summary>
        /// <param name="description">updated description.</param>
        public void UpdateDescription(string description)
        {
            this.Description = description;
        }

        /// <summary>
        /// This method allows the club of the project to be updated.
        /// </summary>
        /// <param name="club">New club to be attached.</param>
        public void UpdateClub(Club club)
        {
            this.Club = club;
        }

        /// <summary>
        /// This method allows the start date of the project to be updated.
        /// </summary>
        /// <param name="startDate">The new start date.</param>
        public void UpdateStartDate(DateTime startDate)
        {
            this.StartDate = startDate;
        }

        /// <summary>
        /// This method allows the end date of the project to be updated.
        /// </summary>
        /// <param name="endDate">The new end date.</param>
        public void UpdateEndDate(DateTime endDate)
        {
            this.EndDate = endDate;
        }

        /// <summary>
        /// This method allows the project to be removed.
        /// </summary>
        /// <returns>Stringified version of this entity.</returns>
        public override string ToString()
        {
            return $"{this.Name}";
        }
    }
}
