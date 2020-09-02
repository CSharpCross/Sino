using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;

namespace Sino.Web.Validation
{
    /// <summary>
    /// 参数验证模块配置
    /// </summary>
    public class ValidationMvcConfiguration
    {
        /// <summary>
        /// 需要自动扫描的程序集
        /// </summary>
        public List<Assembly> AssembliesToRegister { get; private set; } = new List<Assembly>();

        /// <summary>
        /// 过滤器，用于过滤不需要注入的对象
        /// </summary>
        public Func<AssemblyScanner.AssemblyScanResult, bool> TypeFilter { get; private set; }

        /// <summary>
        /// 注入的生命周期
        /// </summary>
        public ServiceLifetime ServiceLifetime { get; private set; } = ServiceLifetime.Transient;

        /// <summary>
        /// Fluent验证类库配置
        /// </summary>
        public ValidatorConfiguration ValidatorOptions { get; private set; }

        /// <summary>
        /// 验证生成工厂对象类型
        /// </summary>
        public Type ValidatorFactoryType { get; set; }

        /// <summary>
        /// 验证生成工厂
        /// </summary>
        public IValidatorFactory ValidatorFactory { get; set; }

        /// <summary>
        /// 是否在本验证执行后执行Mvc默认的模型验证，默认为启用
        /// </summary>
        public bool RunDefaultMvcValidation { get; set; } = true;

        /// <summary>
        /// 是否启用服务端自动校验，默认为启用
        /// </summary>
        public bool AutomaticValidationEnabled { get; set; } = true;

        /// <summary>
        /// 是否开启多语言支持
        /// </summary>
        public bool LocalizationEnabled
        {
            get => ValidatorOptions.LanguageManager.Enabled;
            set => ValidatorOptions.LanguageManager.Enabled = value;
        }

        /// <summary>
        /// 如果找到匹配的验证器，是否隐式验证子属性，默认为Flase
        /// </summary>
        public bool ImplicitylyValidateChildProperties { get; set; }

        public ValidationMvcConfiguration(ValidatorConfiguration validatorConfiguration)
        {
            ValidatorOptions = validatorConfiguration;
        }

        /// <summary>
        /// 自动从程序集注册模型验证对象
        /// </summary>
        /// <typeparam name="T">验证模型</typeparam>
        /// <param name="filter">过滤器</param>
        /// <param name="lifetime">注册生命周期</param>
        public ValidationMvcConfiguration RegisterValidatorsFromAssemblyContaining<T>(Func<AssemblyScanner.AssemblyScanResult, bool> filter = null, 
            ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            return RegisterValidatorsFromAssemblyContaining(typeof(T), filter, lifetime);
        }

        public ValidationMvcConfiguration RegisterValidatorsFromAssemblyContaining(Type type, Func<AssemblyScanner.AssemblyScanResult, bool> filter = null,
            ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            return RegisterValidatorsFromAssemblyContaining(type.GetTypeInfo().Assembly, filter, lifetime);
        }

        public ValidationMvcConfiguration RegisterValidatorsFromAssemblyContaining(Assembly assembly, Func<AssemblyScanner.AssemblyScanResult, bool> filter = null,
            ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            ValidatorFactoryType = typeof(ServiceProviderValidatorFactory);
            AssembliesToRegister.Add(assembly);
            TypeFilter = filter;
            ServiceLifetime = lifetime;
            return this;
        }

        public ValidationMvcConfiguration RegisterValidatorsFromAssemblies(IEnumerable<Assembly> assemblies, Func<AssemblyScanner.AssemblyScanResult, bool> filter = null,
            ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            ValidatorFactoryType = typeof(ServiceProviderValidatorFactory);
            AssembliesToRegister.AddRange(assemblies);
            TypeFilter = filter;
            ServiceLifetime = lifetime;
            return this;
        }
    }
}
