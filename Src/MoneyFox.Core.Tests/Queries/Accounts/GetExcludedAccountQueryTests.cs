﻿using FluentAssertions;
using MoneyFox.Core._Pending_.Common.Interfaces;
using MoneyFox.Core.Aggregates;
using MoneyFox.Core.Queries.Accounts.GetExcludedAccount;
using MoneyFox.Core.Tests.Infrastructure;
using MoneyFox.Infrastructure.Persistence;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Xunit;

namespace MoneyFox.Core.Tests.Queries.Accounts
{
    [ExcludeFromCodeCoverage]
    public class GetExcludedAccountQueryTests : IDisposable
    {
        private readonly AppDbContext context;
        private readonly Mock<IContextAdapter> contextAdapterMock;

        public GetExcludedAccountQueryTests()
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
        public async Task GetExcludedAccountQuery_CorrectNumberLoaded()
        {
            // Arrange
            var accountExcluded = new Account("test", 80, isExcluded: true);
            var accountIncluded = new Account("test", 80);
            await context.AddAsync(accountExcluded);
            await context.AddAsync(accountIncluded);
            await context.SaveChangesAsync();

            // Act
            List<Account> resultList =
                await new GetExcludedAccountQuery.Handler(contextAdapterMock.Object)
                    .Handle(new GetExcludedAccountQuery(), default);

            // Assert
            resultList.Should().ContainSingle();
            resultList[0].CurrentBalance.Should().Be(80);
        }

        [Fact]
        public async Task DontLoadDeactivatedAccounts()
        {
            // Arrange
            var accountExcluded = new Account("test", 80, isExcluded: true);
            var accountIncluded = new Account("test", 80);
            var accountDeactivated = new Account("test", 80);
            accountDeactivated.Deactivate();

            await context.AddAsync(accountExcluded);
            await context.AddAsync(accountIncluded);
            await context.AddAsync(accountDeactivated);
            await context.SaveChangesAsync();

            // Act
            List<Account> resultList =
                await new GetExcludedAccountQuery.Handler(contextAdapterMock.Object)
                    .Handle(new GetExcludedAccountQuery(), default);

            // Assert
            resultList.Should().ContainSingle();
            resultList[0].CurrentBalance.Should().Be(80);
        }
    }
}