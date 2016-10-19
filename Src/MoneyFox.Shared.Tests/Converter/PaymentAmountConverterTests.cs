﻿using System;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyFox.Foundation;
using MoneyFox.Foundation.DataModels;
using MoneyFox.Foundation.Interfaces.Repositories;
using MoneyFox.Shared.Converter;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Interfaces.Repositories;
using Moq;
using MvvmCross.Platform;
using MvvmCross.Test.Core;

namespace MoneyFox.Shared.Tests.Converter
{
    [TestClass]
    public class PaymentAmountConverterTests : MvxIoCSupportingTest
    {
        [TestMethod]
        public void Converter_Payment_NegativeAmountSign()
        {
            new PaymentAmountConverter().Convert(new PaymentViewModel {Amount = 80, Type = (int) PaymentType.Expense}, null,
                null, null).ShouldBe("- " + 80.ToString("C"));
        }

        [TestMethod]
        public void Converter_Payment_PositiveAmountSign()
        {
            new PaymentAmountConverter().Convert(new PaymentViewModel {Amount = 80, Type = (int) PaymentType.Income}, null,
                null, null).ShouldBe("+ " + 80.ToString("C"));
        }

        [TestMethod]
        public void Converter_TransferSameAccount_Minus()
        {
            ClearAll();
            Setup();

            var account = new AccountViewModel
            {
                Id = 4,
                CurrentBalance = 400
            };

            var mock = new Mock<IAccountRepository>();
            mock.Setup(x => x.Load(It.IsAny<Expression<Func<AccountViewModel, bool>>>()));

            Mvx.RegisterSingleton(mock.Object);

            new PaymentAmountConverter()
                .Convert(new PaymentViewModel
                {
                    Amount = 80,
                    Type = (int) PaymentType.Transfer,
                    ChargedAccountId = account.Id,
                    ChargedAccount = account,
                    CurrentAccountId = account.Id
                }, null, account, null)
                .ShouldBe("- " + 80.ToString("C"));
        }

        [TestMethod]
        public void Converter_TransferSameAccount_Plus()
        {
            ClearAll();
            Setup();
            var account = new AccountViewModel
            {
                Id = 4,
                CurrentBalance = 400
            };

            var mock = new Mock<IAccountRepository>();

            Mvx.RegisterSingleton(mock.Object);

            new PaymentAmountConverter()
                .Convert(new PaymentViewModel
                {
                    Amount = 80,
                    Type = (int) PaymentType.Transfer,
                    ChargedAccount = new AccountViewModel(),
                    CurrentAccountId = account.Id
                }, null, new AccountViewModel(), null)
                .ShouldBe("+ " + 80.ToString("C"));
        }
    }
}