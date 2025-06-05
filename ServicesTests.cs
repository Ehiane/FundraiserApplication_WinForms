// <copyright file="ServicesTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WSUFundraiserTests
{
    using System;
    using WSUFundraiserEngine;

    /// <summary>
    /// This class contains tests for the Services class.
    /// </summary>
    public class ServicesTests
    {
        private TransactionService transactionService;

        /// <summary>
        /// This method is called before each test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            XMLDatabase.CleanDatabase();
            this.transactionService = new TransactionService();
            this.transactionService.InitializeAccount("D12345", 100);
        }


        /// <summary>
        /// This method is called after each test.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            XMLDatabase.CleanDatabase();
            this.transactionService = null;
        }

        /// <summary>
        /// This method tests the normal case of depositing funds into an account.
        /// </summary>
        [Test]
        public void TransactionService_NormalCase_Deposit()
        {
            this.transactionService.ProcessTransaction("D12345", 50, "deposit");
            Assert.That(this.transactionService.GetBalance("D12345"), Is.EqualTo(150));
        }

        /// <summary>
        /// This method tests the boundary case of withdrawing all funds from an account.
        /// </summary>
        [Test]
        public void TransactionService_BoundaryCase_WithdrawAllFunds()
        {
            this.transactionService.ProcessTransaction("D12345", 100, "withdraw");
            Assert.That(this.transactionService.GetBalance("D12345"), Is.EqualTo(0));
        }

        /// <summary>
        /// This method tests the exceptional case of withdrawing more funds than are available in an account.
        /// </summary>
        [Test]
        public void TransactionService_ExceptionalCase_WithdrawInsufficientFunds()
        {
            Assert.Throws<ArgumentException>(() => this.transactionService.ProcessTransaction("D12345", 200, "withdraw"));
        }
    }
}
