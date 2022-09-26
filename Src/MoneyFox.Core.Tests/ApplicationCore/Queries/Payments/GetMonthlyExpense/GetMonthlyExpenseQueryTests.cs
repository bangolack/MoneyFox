﻿namespace MoneyFox.Tests.Core.ApplicationCore.Queries.Payments.GetMonthlyExpense
{

    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using FluentAssertions;
    using MoneyFox.Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
    using MoneyFox.Core.ApplicationCore.Queries;
    using MoneyFox.Core.Common.Helpers;
    using MoneyFox.Infrastructure.Persistence;
    using NSubstitute;
    using TestFramework;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class GetMonthlyExpenseQueryTests
    {
        private readonly AppDbContext context;
        private readonly GetMonthlyExpenseQuery.Handler handler;
        private readonly ISystemDateHelper systemDateHelper;

        public GetMonthlyExpenseQueryTests()
        {
            context = InMemoryAppDbContextFactory.Create();
            systemDateHelper = Substitute.For<ISystemDateHelper>();
            handler = new GetMonthlyExpenseQuery.Handler(appDbContext: context, systemDateHelper: systemDateHelper);
        }

        [Fact]
        public async Task ReturnCorrectAmount()
        {
            // Arrange
            systemDateHelper.Today.Returns(new DateTime(year: 2020, month: 09, day: 05));
            var account = new Account(name: "test", initialBalance: 80);
            var payment1 = new Payment(date: new DateTime(year: 2020, month: 09, day: 03), amount: 50, type: PaymentType.Expense, chargedAccount: account);
            var payment2 = new Payment(date: new DateTime(year: 2020, month: 09, day: 04), amount: 20, type: PaymentType.Expense, chargedAccount: account);
            var payment3 = new Payment(date: new DateTime(year: 2020, month: 09, day: 04), amount: 30, type: PaymentType.Income, chargedAccount: account);
            await context.AddAsync(payment1);
            await context.AddAsync(payment2);
            await context.AddAsync(payment3);
            await context.SaveChangesAsync();

            // Act
            var sum = await handler.Handle(request: new GetMonthlyExpenseQuery(), cancellationToken: default);

            // Assert
            sum.Should().Be(70);
        }
    }

}