﻿using FluentAssertions;
using MoneyFox.Core._Pending_.Common.Interfaces;
using MoneyFox.Core.Aggregates;
using MoneyFox.Core.Commands.Accounts.UpdateAccount;
using MoneyFox.Core.Tests.Infrastructure;
using MoneyFox.Infrastructure.Persistence;
using Moq;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Xunit;

namespace MoneyFox.Core.Tests.Commands.Accounts.UpdateAccount
{
    [ExcludeFromCodeCoverage]
    public class UpdateCategoryCommandTests : IDisposable
    {
        private readonly AppDbContext context;
        private readonly Mock<IContextAdapter> contextAdapterMock;

        public UpdateCategoryCommandTests()
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
        public async Task UpdateCategoryCommand_CorrectNumberLoaded()
        {
            // Arrange
            var account = new Account("test", 80);
            await context.AddAsync(account);
            await context.SaveChangesAsync();

            // Act
            account.UpdateAccount("foo");
            await new UpdateAccountCommand.Handler(
                    contextAdapterMock.Object)
                .Handle(new UpdateAccountCommand(account), default);

            Account loadedAccount = await context.Accounts.FindAsync(account.Id);

            // Assert
            loadedAccount.Name.Should().Be("foo");
        }
    }
}