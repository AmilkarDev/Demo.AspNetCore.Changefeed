﻿using System;
using Microsoft.Extensions.Options;

namespace Demo.AspNetCore.RethinkDB.Services
{
    internal class RethinkDbSingletonProvider : IRethinkDbSingletonProvider
    {
        public RethinkDb.Driver.RethinkDB RethinkDbSingleton { get; }

        public RethinkDb.Driver.Net.Connection RethinkDbConnection { get; }

        public RethinkDbSingletonProvider(IOptions<RethinkDbOptions> options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (String.IsNullOrWhiteSpace(options.Value.HostnameOrIp))
            {
                throw new ArgumentNullException(nameof(RethinkDbOptions.HostnameOrIp));
            }

            var rethinkDbSingleton = RethinkDb.Driver.RethinkDB.R;

            var rethinkDbConnection = rethinkDbSingleton.Connection().Hostname(options.Value.HostnameOrIp);

            if (options.Value.DriverPort.HasValue)
            {
                rethinkDbConnection.Port(options.Value.DriverPort.Value);
            }

            if (options.Value.Timeout.HasValue)
            {
                rethinkDbConnection.Timeout(options.Value.Timeout.Value);
            }

            RethinkDbConnection = rethinkDbConnection.Connect();

            RethinkDbSingleton = rethinkDbSingleton;
        }
    }
}
