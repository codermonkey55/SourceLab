using DryIoc;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DIWebAppSample.IoC_Core.Mvc
{
    public class DryIocFilterAttributeFilterProvider : FilterAttributeFilterProvider
    {
        private readonly IContainer container;

        public DryIocFilterAttributeFilterProvider(IContainer container)
        {
            this.container = container;
        }

        public override IEnumerable<Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            var filters = base.GetFilters(controllerContext, actionDescriptor).ToArray();
            for (var i = 0; i < filters.Length; i++)
            {
                this.container.InjectPropertiesAndFields(filters[i].Instance);
            }

            return filters;
        }
    }
}