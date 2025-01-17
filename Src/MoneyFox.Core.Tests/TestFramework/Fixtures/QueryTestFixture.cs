﻿namespace MoneyFox.Tests.TestFramework.Fixtures
{

    using System;
    using System.Diagnostics.CodeAnalysis;
    using MoneyFox.Infrastructure.Persistence;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class QueryTestFixture : IDisposable
    {
        public QueryTestFixture()
        {
            Context = InMemoryAppDbContextFactory.Create();
        }

        public AppDbContext Context { get; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            InMemoryAppDbContextFactory.Destroy(Context);
        }
    }

    [CollectionDefinition("QueryCollection")]
    public class QueryCollection : ICollectionFixture<QueryTestFixture> { }

}
