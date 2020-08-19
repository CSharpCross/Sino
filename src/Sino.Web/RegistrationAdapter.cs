using Castle.MicroKernel.Registration;
using Microsoft.Extensions.DependencyInjection;
using Sino.Web.Dependency.Scope;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Sino.Web
{
    public class RegistrationAdapter
    {
        public static string OriginalComponentName(string uniqueComponentName)
        {
            if (uniqueComponentName == null)
            {
                return null;
            }
            if (!uniqueComponentName.Contains("@"))
            {
                return uniqueComponentName;
            }
            return uniqueComponentName.Split('@')[0];
        }

        public static IRegistration FromOpenGenericServiceDescriptor(Microsoft.Extensions.DependencyInjection.ServiceDescriptor service)
        {
            ComponentRegistration<object> registration = Component.For(service.ServiceType).NamedAutomatically(UniqueComponentName(service));

            if (service.ImplementationType != null)
            {
                registration = UsingImplementation(registration, service);
            }
            else
            {
                throw new ArgumentException("Unsupported ServiceDescriptor");
            }

            return ResolveLifestyle(registration, service).IsDefault();
        }

        public static string UniqueComponentName(Microsoft.Extensions.DependencyInjection.ServiceDescriptor service)
        {
            var result = "";
            if (service.ImplementationType != null)
            {
                result = service.ImplementationType.FullName;
            }
            else if (service.ImplementationInstance != null)
            {
                result = service.ImplementationInstance.GetType().FullName;
            }
            else
            {
                result = service.ImplementationFactory.GetType().FullName;
            }
            result = result + "@" + Guid.NewGuid().ToString();

            return result;
        }

        private static ComponentRegistration<TService> UsingImplementation<TService>(ComponentRegistration<TService> registration, Microsoft.Extensions.DependencyInjection.ServiceDescriptor service) where TService : class
        {
            return registration.ImplementedBy(service.ImplementationType);
        }

        private static ComponentRegistration<TService> ResolveLifestyle<TService>(ComponentRegistration<TService> registration, Microsoft.Extensions.DependencyInjection.ServiceDescriptor service) where TService : class
        {
            switch (service.Lifetime)
            {
                case ServiceLifetime.Singleton:
                    return registration.LifeStyle.Scoped<ExtensionContainerRootScopeAccessor>();
                case ServiceLifetime.Scoped:
                    return registration.LifeStyle.Scoped<ExtensionContainerScopeAccessor>();
                case ServiceLifetime.Transient:
                    return registration.Attribute(ExtensionContainerScope.TransientMarker).Eq(Boolean.TrueString).LifeStyle.Scoped<ExtensionContainerScopeAccessor>();
                default:
                    throw new ArgumentException($"Invalid lifetime {service.Lifetime}");
            }
        }
    }
}
