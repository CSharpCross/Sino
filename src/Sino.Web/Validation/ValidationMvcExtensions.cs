using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Options;
using Sino.Web.Validation;
using System;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ValidationMvcExtensions
    {
        public static IMvcBuilder AddValidation(this IMvcBuilder mvcBuilder, Action<ValidationMvcConfiguration> configurationExpression = null)
        {
            var config = new ValidationMvcConfiguration(ValidatorOptions.Global);
            configurationExpression?.Invoke(config);

            RegisterServices(mvcBuilder.Services, config);

            mvcBuilder.AddMvcOptions(options =>
            {
                if (!options.ModelMetadataDetailsProviders.Any(x => x is ValidationBindingMetadataProvider))
                {
                    options.ModelMetadataDetailsProviders.Add(new ValidationBindingMetadataProvider());
                }

                if (!options.ModelValidatorProviders.Any(x => x is ValidationModelValidatorProvider))
                {
                    options.ModelValidatorProviders.Insert(0, new ValidationModelValidatorProvider(config.ImplicitylyValidateChildProperties));
                }
            });

            return mvcBuilder;
        }

        private static void RegisterServices(IServiceCollection services, ValidationMvcConfiguration config)
        {
            services.AddValidatorsFromAssemblies(config.AssembliesToRegister, config.ServiceLifetime, config.TypeFilter);
            services.AddSingleton(config.ValidatorOptions);

            if (config.ValidatorFactory != null)
            {
                var factory = config.ValidatorFactory;
                services.Add(ServiceDescriptor.Transient(s => factory));
            }
            else
            {
                services.Add(ServiceDescriptor.Transient(typeof(IValidatorFactory), config.ValidatorFactoryType ?? typeof(ServiceProviderValidatorFactory)));
            }

            if (config.AutomaticValidationEnabled)
            {
                services.Add(ServiceDescriptor.Singleton<IObjectModelValidator, ValidationObjectModelValidator>(s =>
                {
                    var options = s.GetRequiredService<IOptions<MvcOptions>>().Value;
                    var metadataProvider = s.GetRequiredService<IModelMetadataProvider>();
                    return new ValidationObjectModelValidator(metadataProvider, options.ModelValidatorProviders, config.RunDefaultMvcValidation);
                }));
            }
        }
    }
}
