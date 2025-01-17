﻿namespace MoneyFox.Tests.Core.ApplicationCore.Queries.Categories.GetCategoryBySearchTerm
{

    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using FluentAssertions;
    using MoneyFox.Core.ApplicationCore.Domain.Aggregates.CategoryAggregate;
    using MoneyFox.Core.ApplicationCore.Queries;
    using MoneyFox.Infrastructure.Persistence;
    using TestFramework;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class GetCategoryBySearchTermQueryTests
    {
        private readonly AppDbContext context;
        private readonly GetCategoryBySearchTermQuery.Handler handler;

        public GetCategoryBySearchTermQueryTests()
        {
            context = InMemoryAppDbContextFactory.Create();
            handler = new GetCategoryBySearchTermQuery.Handler(context);
        }

        [Fact]
        public async Task GetExcludedAccountQuery_WithoutFilter_CorrectNumberLoaded()
        {
            // Arrange
            var category1 = new Category("test");
            var category2 = new Category("test2");
            await context.AddAsync(category1);
            await context.AddAsync(category2);
            await context.SaveChangesAsync();

            // Act
            var result = await handler.Handle(request: new GetCategoryBySearchTermQuery(), cancellationToken: default);

            // Assert
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetExcludedAccountQuery_CorrectNumberLoaded()
        {
            // Arrange
            var category1 = new Category("this is a guid");
            var category2 = new Category("test2");
            await context.AddAsync(category1);
            await context.AddAsync(category2);
            await context.SaveChangesAsync();

            // Act
            var result = await handler.Handle(request: new GetCategoryBySearchTermQuery("guid"), cancellationToken: default);

            // Assert
            Assert.Single(result);
        }
    }

}
