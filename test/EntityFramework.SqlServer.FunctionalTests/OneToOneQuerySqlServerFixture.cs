// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.Data.Entity.FunctionalTests;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Relational.FunctionalTests;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;

namespace Microsoft.Data.Entity.SqlServer.FunctionalTests
{
    public class OneToOneQuerySqlServerFixture : OneToOneQueryFixtureBase
    {
        private readonly DbContextOptions _options;
        private readonly IServiceProvider _serviceProvider;

        public OneToOneQuerySqlServerFixture()
        {
            _serviceProvider
                = new ServiceCollection()
                    .AddEntityFramework()
                    .AddSqlServer()
                    .ServiceCollection()
                    .AddInstance<ILoggerFactory>(new TestSqlLoggerFactory())
                    .BuildServiceProvider();

            var model = CreateModel();
            var database = SqlServerTestStore.CreateScratch();

            var optionsBuilder = new DbContextOptionsBuilder().UseModel(model);
            optionsBuilder.UseSqlServer(database.Connection.ConnectionString);
            _options = optionsBuilder.Options;

            using (var context = new DbContext(_serviceProvider, _options))
            {
                context.Database.EnsureCreated();

                AddTestData(context);
            }
        }

        public DbContext CreateContext()
        {
            return new DbContext(_serviceProvider, _options);
        }
    }
}
