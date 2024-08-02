﻿using Autofac.Extras.DynamicProxy;
using EasyCaching.Core.Interceptor;
using EasyCaching.Interceptor.Castle;
using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Options;
using EasyCaching.Core.Configurations;

namespace Autofac.Extensions.DependencyInjection
{
    public static class CastleInterceptorAutofacExtensions
    {
        /// <summary>
        /// Add the castle interceptor.
        /// </summary>
        public static void AddCastleInterceptor(this ContainerBuilder builder, Action<EasyCachingInterceptorOptions> action)
        {
            builder.RegisterType<DefaultEasyCachingKeyGenerator>().As<IEasyCachingKeyGenerator>().InstancePerLifetimeScope();

            builder.RegisterType<EasyCachingInterceptor>();

            var config = new EasyCachingInterceptorOptions();

            action(config);

            var options = Options.Create(config);

            builder.Register(x => options);

            var assembly = Assembly.GetCallingAssembly();

            builder.RegisterAssemblyTypes(assembly)
                .Where(t => !t.IsAbstract && t.GetInterfaces().SelectMany(x => x.GetMethods()).Any(
                   y => y.CustomAttributes.Any(data =>
                                    typeof(EasyCachingInterceptorAttribute).GetTypeInfo().IsAssignableFrom(data.AttributeType)
                              )))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(EasyCachingInterceptor));

        }

        /// <summary>
        /// Add the castle interceptor.
        /// </summary>
        public static void AddCastleInterceptor(this ContainerBuilder builder, Assembly[] assemblies)
        {
            builder.RegisterType<DefaultEasyCachingKeyGenerator>().As<IEasyCachingKeyGenerator>().InstancePerLifetimeScope();

            builder.RegisterType<EasyCachingInterceptor>();

            builder.RegisterAssemblyTypes(assemblies)
                .Where(t => !t.IsAbstract && t.GetInterfaces().SelectMany(x => x.GetMethods()).Any(
                   y => y.CustomAttributes.Any(data =>
                                    typeof(EasyCachingInterceptorAttribute).GetTypeInfo().IsAssignableFrom(data.AttributeType)
                              )))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(EasyCachingInterceptor));

        }


    }
}
