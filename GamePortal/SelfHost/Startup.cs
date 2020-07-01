﻿using IdentityServer3.Core.Configuration;
using Owin;
using SelfHost.Config;

namespace SelfHost
{
    internal class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            var factory = new IdentityServerServiceFactory();

            var options = new IdentityServerOptions
            {
                SiteName = "IdentityServer3 (self host)",

                SigningCertificate = Certificate.Get(),
                Factory = factory,
            };

            appBuilder.UseIdentityServer(options);
        }
    }
}