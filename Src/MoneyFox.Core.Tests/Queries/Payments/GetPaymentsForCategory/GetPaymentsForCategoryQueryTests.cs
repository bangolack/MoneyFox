﻿using FluentAssertions;
using MoneyFox.Core.Queries.Payments.GetPaymentsForCategory;
using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace MoneyFox.Core.Tests.Queries.Payments.GetPaymentsForCategory
{
    [ExcludeFromCodeCoverage]
    public class GetPaymentsForCategoryQueryTests
    {
        [Fact]
        public void AssignCorrectlyInCtor()
        {
            // Arrange
            const int catId = 5234;
            DateTime dateRangeFrom = DateTime.Now.AddDays(1);
            DateTime dateRangeTo = DateTime.Now.AddDays(2);

            // Act
            var query = new GetPaymentsForCategoryQuery(catId, dateRangeFrom, dateRangeTo);

            // Assert
            query.CategoryId.Should().Be(catId);
            query.DateRangeFrom.Should().Be(dateRangeFrom);
            query.DateRangeTo.Should().Be(dateRangeTo);
        }
    }
}