using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Composition.Hosting;

namespace AspNetCoreWebMefDI.Framework
{
    public class MefServiceProvider : IServiceProvider
    {
        private readonly CompositionHost Composition;
        public IServiceProvider DefaultServiceProvider;

        public static List<Type> TypesLoadedWithDefault = new List<Type>();
        public static List<Type> TypesLoadedWithMef = new List<Type>();
        public static List<Type> TypesNotFound = new List<Type>();

        public MefServiceProvider()
        {
            var container = new ContainerConfiguration()
                .WithAssembly(typeof(Program).Assembly);

            this.Composition = container.CreateContainer();
        }

        public void SatisfyImports(object instance)
        {
            this.Composition.SatisfyImports(instance);
        }

        public object GetService(Type serviceType)
        {
            object serviceObject;
            if (!this.Composition.TryGetExport(serviceType, out serviceObject))
            {
                serviceObject = this.DefaultServiceProvider.GetService(serviceType);
                TypesLoadedWithDefault.Add(serviceType);
            }
            else
            {
                TypesLoadedWithMef.Add(serviceType);
            }

            if (serviceObject == null)
            {
                TypesNotFound.Add(serviceType);
            }

            return serviceObject;
        }
    }
}
