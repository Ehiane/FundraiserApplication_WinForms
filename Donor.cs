// <copyright file="Donor.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WSUFundraiserEngine
{
    /// <summary>
    /// This class represents a donor.
    /// </summary>
    public class Donor : User
    {
        /// <summary>
        /// Gets or sets the donation transaction service.
        /// </summary>
        private readonly TransactionService donationTransactionService;

        /// <summary>
        /// Gets or sets the donations made by the donor.
        /// </summary>
        public List<Donation> Donations { get; set; } = new List<Donation>();

        /// <summary>
        /// Gets the donor ID.
        /// </summary>
        public string DonorId { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Donor"/> class.
        /// </summary>
        /// <param name="name">The name of the Donor.</param>
        /// <param name="email">Email of the Donor.</param>
        /// <param name="username">Username of the Donor.</param>
        /// <param name="password">Password of the Donor.</param>
        /// <param name="donorId">Id of the Donor.</param>
        public Donor(string name, string email, string username, string password, string donorId)
            : base(name, email, username, password)
        {
            this.DonorId = donorId;
            this.donationTransactionService = new TransactionService();
            this.donationTransactionService.InitializeAccount(this.DonorId, 0);
        }

        /// <summary>
        /// This method allows a donor to make a donation.
        /// </summary>
        /// <param name="project"> The details of the project.</param>
        /// <param name="amount">The amount to donate.</param>
        /// <param name="anonymous">Determins whether to make the donation anonymous or not.</param>
        public void MakeDonation(Project project, decimal amount, bool anonymous = false)
        {
            try
            {
                this.donationTransactionService.ProcessTransaction(this.DonorId, amount, "deposit");
                var donation = new Donation(this, project, amount, anonymous);
                this.Donations.Add(donation);
                project.AddDonation(donation);
                Console.WriteLine($"Donation of ${amount} made to {project.Name} Successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// This method allows a donor to view their donations.
        /// </summary>
        public void ViewDonations()
        {
            foreach (var donation in this.Donations)
            {
                Console.WriteLine($"Donation: {donation.Amount}, Project: {donation.Project.Name}");
            }
        }

        /// <summary>
        /// This method allows a donor to view their balance.
        /// </summary>
        /// <returns>The donor's available balance.</returns>
        public decimal GetDonorBalance()
        {
            return this.donationTransactionService.GetBalance(this.DonorId);
        }

        /// <summary>
        /// This method allows a donor to withdraw money from their account.
        /// </summary>
        /// <param name="amount">The amount to be withdrawn.</param>
        public void Withdraw(decimal amount)
        {
            this.donationTransactionService.ProcessTransaction(this.DonorId, amount, "withdraw");
        }

        /// <summary>
        /// This method allows a donor to update their donor ID.
        /// </summary>
        /// <param name="donorId">The donor's id.</param>
        public void UpdateDonorId(string donorId)
        {
            this.DonorId = donorId;
        }

        /// <summary>
        /// This method allows a donor to deposit money into their account.
        /// </summary>
        /// <param name="amount">The amount to be deposited.</param>
        public void Deposit(decimal amount)
        {
            this.donationTransactionService.ProcessTransaction(this.DonorId, amount, "deposit");
        }

        /// <summary>
        /// This method allows a donor to view the projects they have donated to.
        /// </summary>
        /// <returns>The strinfigied version of this donor instance.</returns>
        public override string ToString()
        {
            return $"Donor:\n Name: {this.Name},  Email: {this.Email}, Username: {this.Username}, Donor ID: {this.DonorId}\n";
        }
    }
}
