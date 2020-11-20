using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.Context;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using System;

namespace OrvilleX.Dependency.Resolvers
{
    /// <summary>
    /// 当没有指定处理器能够处理<see name="IKernel.ResolveAll" />的迭代服务时使用
    /// </summary>
    public class RegisteredCollectionResolver : CollectionResolver
    {
        public RegisteredCollectionResolver(IKernel kernel, bool allowEmptyCollections = true)
            : base(kernel, allowEmptyCollections) { }

        public override bool CanResolve(CreationContext context, ISubDependencyResolver contextHandlerResolver, ComponentModel model, DependencyModel dependency)
        {
            if (kernel.HasComponent(dependency.TargetItemType))
            {
                return false;
            }
            return base.CanResolve(context, contextHandlerResolver, model, dependency);
        }

        public override object Resolve(CreationContext context, ISubDependencyResolver contextHandlerResolver, ComponentModel model, DependencyModel dependency)
        {
            return base.Resolve(context, contextHandlerResolver, model, dependency) as Array;
        }
    }
}
