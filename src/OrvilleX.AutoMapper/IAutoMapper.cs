using OrvilleX.Dependency;

namespace OrvilleX.AutoMapper
{
    /// <summary>
    /// 实体映射接口
    /// </summary>
    public interface IAutoMapper : ISingletonDependency
    {
        /// <summary>
        /// 将source映射为指定类型
        /// </summary>
        /// <typeparam name="TDestination">需要映射成的类型</typeparam>
        /// <param name="source">需要被映射的实体</param>
        TDestination Map<TDestination>(object source);

        TDestination Map<TSource, TDestination>(TSource source);

        TDestination Map<TSource, TDestination>(TSource source, TDestination destination);
    }
}
