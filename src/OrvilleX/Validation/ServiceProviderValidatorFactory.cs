using FluentValidation;
using System;

namespace OrvilleX.Validation
{
    /// <summary>
    /// 利用IOC获取模型验证对象
    /// </summary>
    public class ServiceProviderValidatorFactory : ValidatorFactoryBase
    {
        private readonly IServiceProvider _serviceProvider;

        public ServiceProviderValidatorFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override IValidator CreateInstance(Type validatorType)
        {
            return _serviceProvider.GetService(validatorType) as IValidator;
        }
    }
}
