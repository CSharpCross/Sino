using Castle.MicroKernel;
using Castle.MicroKernel.SubSystems.Naming;
using Castle.MicroKernel.Util;
using System;
using System.Collections.Generic;
using System.Linq;
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
            _ = service ?? throw new ArgumentNullException();


        }
    }
}
