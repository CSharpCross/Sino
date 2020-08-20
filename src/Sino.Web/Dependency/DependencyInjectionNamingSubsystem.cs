using Castle.MicroKernel;
using Castle.MicroKernel.SubSystems.Naming;
using Castle.MicroKernel.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;

namespace Sino.Web.Dependency
{
    /// <summary>
    /// 基于DefaultNamingSubSystem的命名子系统
    /// </summary>
    public class DependencyInjectionNamingSubsystem : DefaultNamingSubSystem
    {
        private readonly IDictionary<Type, IHandler[]> _handlerListsRegistrationOrderByTypeCache =
            new Dictionary<Type, IHandler[]>(SimpleTypeEqualityComparer.Instance);

        private IHandler[] GetHandlersInRegisterOrderNoLock(Type service)
        {
            var handlers = name2Handler.Values.Where(x => x.Supports(service) == true);
            return handlers.ToArray();
        }

        public override IHandler[] GetHandlers(Type service)
        {
            _ = service ?? throw new ArgumentNullException(nameof(service));

            if (filters != null)
            {
                var filtersOpinion = GetFiltersOpinion(service);
                if (filtersOpinion != null)
                {
                    return filtersOpinion;
                }
            }

            IHandler[] result;
            using (var locker = @lock.ForReadingUpgradeable())
            {
                if (_handlerListsRegistrationOrderByTypeCache.TryGetValue(service, out result))
                {
                    return result;
                }
                result = GetHandlersInRegisterOrderNoLock(service);

                locker.Upgrade();
                _handlerListsRegistrationOrderByTypeCache[service] = result;
            }

            return result;
        }

        public override IHandler GetHandler(Type service)
        {
            _ = service ?? throw new ArgumentNullException(nameof(service));

            if (selectors != null)
            {
                var selectorsOpinion = GetSelectorsOpinion(null, service);
                if (selectorsOpinion != null)
                {
                    return selectorsOpinion;
                }
            }
            if (HandlerByServiceCache.TryGetValue(service, out var handler))
            {
                return handler;
            }
            
            if (service.GetTypeInfo().IsGenericType && service.GetTypeInfo().IsGenericTypeDefinition == false)
            {
                var openService = service.GetGenericTypeDefinition();
                if (HandlerByServiceCache.TryGetValue(openService, out handler) && handler.Supports(service))
                {
                    return handler;
                }

                var handlerCandidates = base.GetHandlers(openService);
                return handlerCandidates.FirstOrDefault(x => x.Supports(service));
            }

            return null;
        }
    }
}
