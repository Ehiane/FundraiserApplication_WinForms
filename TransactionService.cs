// <copyright file="TransactionService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WSUFundraiserEngine
{
    /// <summary>
    /// This class represents a transaction service to process monetary donations.
    /// </summary>
    public class TransactionService
    {
        /// <summary>
        /// This dictionary stores the account balances of donors.
        /// </summary>
        private Dictionary<string, decimal> accountBalances = new Dictionary<string, decimal>();

        /// <summary>
        /// This method initializes the account of a donor.
        /// </summary>
        /// <param name="donorId"> The Id of the donor.</param>
        /// <param name="initialBalance">The amount to be added to be deposited in the service.</param>
        public void InitializeAccount(string donorId, decimal initialBalance)
        {
            if (!this.accountBalances.ContainsKey(donorId))
            {
                this.accountBalances.Add(donorId, initialBalance);
            }
            else
            {
                throw new ArgumentException("Donor already has an account");
            }
        }

        /// <summary>
        /// This method allows a donor to withdraw money from their account.
        /// </summary>
        /// <param name="donorId"> The Id of the donor.</param>
        /// <returns>The available money remaining in the user's account.</returns>
        /// <exception cref="ArgumentException"> Occurs when there is no associateed account.</exception>
        public decimal GetBalance(string donorId)
        {
            if (this.accountBalances.ContainsKey(donorId))
            {
                return this.accountBalances[donorId];
            }
            else
            {
                throw new ArgumentException("Donor does not have an account");
            }
        }

        /// <summary>
        /// This method processes a transaction, either a withdrawl or a deposit.
        /// </summary>
        /// <param name="donorId"> The Id of the donor.</param>
        /// <param name="amount">The amount to be added to be deposited in the service.</param>
        /// <param name="transactionType">The type of transaction the user wants to perform.</param>
        public void ProcessTransaction(string donorId, decimal amount, string transactionType)
        {
            if (transactionType.ToLower().Contains("withdraw"))
            {
                this.Withdraw(donorId, amount);
            }
            else if (transactionType.ToLower().Contains("deposit"))
            {
                this.Deposit(donorId, amount);
            }
        }

        /// <summary>
        /// This method notifies the donor of a transaction.
        /// </summary>
        /// <param name="donorId"> The Id of the donor.</param>
        /// <param name="message"> the message to be notified </param>
        public void Notify(string donorId, string message)
        {
            Console.WriteLine($"Donor {donorId}: {message}");
        }

        /// <summary>
        /// This method allows a donor to deposit money to their account.
        /// </summary>
        /// <param name="donorId"> The Id of the donor.</param>
        /// <param name="amount">The amount to be added to be deposited in the service.</param>
        /// <exception cref="ArgumentException"> Occurs when there is no associated account.</exception>
        private void Deposit(string donorId, decimal amount)
        {
            if (this.accountBalances.ContainsKey(donorId))
            {
                this.accountBalances[donorId] += amount;
                this.Notify(donorId, $"Deposited ${amount}");
            }
            else
            {
                throw new ArgumentException("Donor does not have an account");
            }
        }

        /// <summary>
        /// This method allows a donor to withdraw money from their account.
        /// </summary>
        /// <param name="donorId"> The Id of the donor.</param>
        /// <param name="amount">The amount to be added to be deposited in the service.</param>
        /// <exception cref="ArgumentException"> Occurs when there is no associated account.</exception>
        private void Withdraw(string donorId, decimal amount)
        {
            if (this.accountBalances.ContainsKey(donorId))
            {
                if (this.accountBalances[donorId] >= amount)
                {
                    this.accountBalances[donorId] -= amount;
                    this.Notify(donorId, $"Withdrew ${amount}");
                }
                else
                {
                    throw new ArgumentException("Insufficient funds");
                }
            }
            else
            {
                throw new ArgumentException("Donor does not have an account");
            }
        }
    }
}
