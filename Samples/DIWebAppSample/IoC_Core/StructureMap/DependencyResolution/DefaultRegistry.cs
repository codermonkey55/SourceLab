// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultRegistry.cs" company="Web Advanced">
// Copyright 2012 Web Advanced (www.webadvanced.com)
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Specialized;
using System.Runtime.Caching;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using CodeLabs.Web.WebForms.IoC_Integration.Services;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace CodeLabs.Web.WebForms.IoC_Integration.IoC_Core.StructureMap.DependencyResolution
{
    public class DefaultRegistry : Registry
    {
        #region Constructors and Destructors

        public DefaultRegistry()
        {
            Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
                scan.With(new ControllerConvention());
            });

            //For<HomeController>().HttpContextScoped();

            For<RouteCollection>().Use(_ => RouteTable.Routes);
            //For<HttpContextBase>().Use(_ => new HttpContextWrapper(HttpContext.Current));
            For<RequestContext>().Use(_ => ((MvcHandler)HttpContext.Current.Handler).RequestContext);
            //For<RouteData>().Use(_ => RouteTable.Routes.GetRouteData(_.GetInstance<HttpContextBase>()));
            For<MemoryCache>().Use(_ => new MemoryCache("StructureMapRegistration_Default", new NameValueCollection()));
            //For<UrlHelper>().Use(_ => new UrlHelper(_.GetInstance<RequestContext>(), _.GetInstance<RouteCollection>()));

            For<IBrowserConfigService>().Use<BrowserConfigService>();
            For<ICacheService>().Use<CacheService>();
            For<IFeedService>().Use<FeedService>();
            For<ILoggingService>().Use<LoggingService>().Singleton();
            For<IManifestService>().Use<ManifestService>();
            For<IOpenSearchService>().Use<OpenSearchService>();
            For<IRobotsService>().Use<RobotsService>();
            For<ISitemapService>().Use<SitemapService>();
            For<ISitemapPingerService>().Use<SitemapPingerService>();
        }

        #endregion
    }
}