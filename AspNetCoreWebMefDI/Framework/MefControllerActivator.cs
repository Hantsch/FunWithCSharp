using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Internal;

namespace AspNetCoreWebMefDI.Framework
{
    public class MefControllerActivator : DefaultControllerActivator, IControllerActivator
    {
        private readonly MefServiceProvider MefServiceProvider;

        public MefControllerActivator(MefServiceProvider mefServiceProvider, ITypeActivatorCache typeActivatorCache)
            : base(typeActivatorCache)
        {
            this.MefServiceProvider = mefServiceProvider;
        }

        public object Create(ControllerContext context)
        {
            var controller = base.Create(context);
            this.MefServiceProvider.SatisfyImports(controller);
            return controller;
        }

        public void Release(ControllerContext context, object controller)
        {
            base.Release(context, controller);
        }
    }
}
