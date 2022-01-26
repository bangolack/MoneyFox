﻿using FluentAssertions;
using MoneyFox.Core._Pending_.Common.Interfaces;
using MoneyFox.Core.Aggregates;
using MoneyFox.Core.Aggregates.Payments;
using MoneyFox.Core.Queries.Statistics;
using MoneyFox.Core.Queries.Statistics.Queries;
using MoneyFox.Core.Tests.Infrastructure;
using MoneyFox.Infrastructure.Persistence;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Xunit;

namespace MoneyFox.Core.Tests.Queries.Statistics
{
    [ExcludeFromCodeCoverage]
    [Collection("CultureCollection")]
    public class GetAccountProgressionHandlerTests : IDisposable
    {
        private readonly AppDbContext context;
        private readonly Mock<IContextAdapter> contextAdapterMock;

        public GetAccountProgressionHandlerTests()
        {
            context = InMemoryEfCoreContextFactory.Create();

            contextAdapterMock = new Mock<IContextAdapter>();
            contextAdapterMock.SetupGet(x => x.Context).Returns(context);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) => InMemoryEfCoreContextFactory.Destroy(context);

        [Fact]
        public async Task CalculateCorrectSums()
        {
            // Arrange
            var account = new Account("Foo1");
            context.AddRange(
                new List<Payment>
                {
                    new Payment(DateTime.Today, 60, PaymentType.Income, account),
                    new Payment(DateTime.Today, 20, PaymentType.Expense, account),
                    new Payment(DateTime.Today.AddMonths(-1), 50, PaymentType.Expense, account),
                    new Payment(DateTime.Today.AddMonths(-2), 40, PaymentType.Expense, account)
                });
            context.Add(account);
            context.SaveChanges();

            // Act
            List<StatisticEntry> result = await new GetAccountProgressionHandler(contextAdapterMock.Object).Handle(
                new GetAccountProgressionQuery(
                    account.Id,
                    DateTime.Today.AddYears(-1),
                    DateTime.Today.AddDays(3)),
                default);

            // Assert
            result[0].Value.Should().Be(40);
            result[1].Value.Should().Be(-50);
            result[2].Value.Should().Be(-40);
        }

        [Fact]
        public async Task GetValues_CorrectSums()
        {
            // Arrange
            var account = new Account("Foo1");
            context.AddRange(
                new List<Payment>
                {
                    new Payment(DateTime.Today, 60, PaymentType.Income, account),
                    new Payment(DateTime.Today, 20, PaymentType.Expense, account),
                    new Payment(DateTime.Today.AddMonths(-1), 50, PaymentType.Expense, account),
                    new Payment(DateTime.Today.AddMonths(-2), 40, PaymentType.Expense, account)
                });
            context.Add(account);
            context.SaveChanges();

            // Act
            List<StatisticEntry> result = await new GetAccountProgressionHandler(contextAdapterMock.Object).Handle(
                new GetAccountProgressionQuery(
                    account.Id,
                    DateTime.Today.AddYears(-1),
                    DateTime.Today.AddDays(3)),
                default);

            // Assert
            result[0].Color.Should().Be("#87cefa");
            result[1].Color.Should().Be("#cd3700");
            result[2].Color.Should().Be("#cd3700");
        }
    }
}