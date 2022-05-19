﻿namespace MoneyFox.Tests.Presentation.Utilities
{

    using System;
    using System.Diagnostics.CodeAnalysis;
    using MoneyFox.Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
    using MoneyFox.Core.Resources;
    using MoneyFox.Utilities;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class PaymentTypeHelperTests
    {
        [Theory]
        [InlineData(PaymentType.Expense, "Expense")]
        [InlineData(PaymentType.Income, "Income")]
        [InlineData(PaymentType.Transfer, "Transfer")]
        public void GetEnumFromString_Expense_Title(PaymentType type, string typeString)
        {
            Assert.Equal(expected: type, actual: PaymentTypeHelper.GetEnumFromString(typeString));
        }

        [Theory]
        [InlineData(3)]
        [InlineData(-1)]
        public void GetTypeString_InvalidType_Exception(int enumInt)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => PaymentTypeHelper.GetTypeString(enumInt));
        }

        [Fact]
        public void GetEnumFromString_ExpenseIntEditTrue_Title()
        {
            Assert.Equal(expected: Strings.EditSpendingTitle, actual: PaymentTypeHelper.GetViewTitleForType(type: 0, isEditMode: true));
        }

        [Fact]
        public void GetEnumFromString_IncomeIntEditTrue_Title()
        {
            Assert.Equal(expected: Strings.EditIncomeTitle, actual: PaymentTypeHelper.GetViewTitleForType(type: 1, isEditMode: true));
        }

        [Fact]
        public void GetEnumFromString_TransferIntEditTrue_Title()
        {
            Assert.Equal(expected: Strings.EditTransferTitle, actual: PaymentTypeHelper.GetViewTitleForType(type: 2, isEditMode: true));
        }

        [Fact]
        public void GetEnumFromString_ExpenseIntEditFalse_Title()
        {
            Assert.Equal(expected: Strings.AddExpenseTitle, actual: PaymentTypeHelper.GetViewTitleForType(type: 0, isEditMode: false));
        }

        [Fact]
        public void GetEnumFromString_IncomeIntEditFalse_Title()
        {
            Assert.Equal(expected: Strings.AddIncomeTitle, actual: PaymentTypeHelper.GetViewTitleForType(type: 1, isEditMode: false));
        }

        [Fact]
        public void GetEnumFromString_TransferIntEditFalse_Title()
        {
            Assert.Equal(expected: Strings.AddTransferTitle, actual: PaymentTypeHelper.GetViewTitleForType(type: 2, isEditMode: false));
        }

        [Fact]
        public void GetEnumFromString_ExpenseEnumEditTrue_Title()
        {
            Assert.Equal(expected: Strings.EditSpendingTitle, actual: PaymentTypeHelper.GetViewTitleForType(type: PaymentType.Expense, isEditMode: true));
        }

        [Fact]
        public void GetEnumFromString_IncomeEnumEditTrue_Title()
        {
            Assert.Equal(expected: Strings.EditIncomeTitle, actual: PaymentTypeHelper.GetViewTitleForType(type: PaymentType.Income, isEditMode: true));
        }

        [Fact]
        public void GetEnumFromString_TransferEnumEditTrue_Title()
        {
            Assert.Equal(expected: Strings.EditTransferTitle, actual: PaymentTypeHelper.GetViewTitleForType(type: PaymentType.Transfer, isEditMode: true));
        }

        [Fact]
        public void GetEnumFromString_ExpenseEnumEditFalse_Title()
        {
            Assert.Equal(expected: Strings.AddExpenseTitle, actual: PaymentTypeHelper.GetViewTitleForType(type: PaymentType.Expense, isEditMode: false));
        }

        [Fact]
        public void GetEnumFromString_IncomeEnumEditFalse_Title()
        {
            Assert.Equal(expected: Strings.AddIncomeTitle, actual: PaymentTypeHelper.GetViewTitleForType(type: PaymentType.Income, isEditMode: false));
        }

        [Fact]
        public void GetEnumFromString_TransferEnumEditFalse_Title()
        {
            Assert.Equal(expected: Strings.AddTransferTitle, actual: PaymentTypeHelper.GetViewTitleForType(type: PaymentType.Transfer, isEditMode: false));
        }
    }

}