using Atomia.Store.AspNetMvc.Ports;
using System;
using System.Web.Mvc;

namespace Atomia.Store.AspNetMvc.Infrastructure
{
    /// <summary>
    /// Model binder that can get and bind to concrete implementations of abstract classes and interfaces via the Asp.Net DependencyResolver.
    /// </summary>
    public sealed class AbstractModelBinder : DefaultModelBinder
    {
        /// <summary>
        /// Creates and binds to a concrete implementation of abstract model.
        /// </summary>
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var model = DependencyResolver.Current.GetService(bindingContext.ModelType);

            if (model != null)
            {
                var metaData = ModelMetadataProviders.Current.GetMetadataForType(null, model.GetType());
                bindingContext.ModelMetadata = metaData;
            }

            var boundModel = base.BindModel(controllerContext, bindingContext);

            return boundModel;
        }
    }

    /// <summary>
    /// Model binder for <see cref="PaymentMethodForm"/>.
    /// </summary>
    /// <remarks>
    /// It is expected that any <see cref="PaymentMethodForm"/> will be instantiated by the selected <see cref="Atomia.Store.AspNetMvc.Ports.PaymentMethodGuiPlugin" />,
    /// so we don't want the <see cref="AbstractModelBinder"/> to override the already instantiated <see cref="PaymentMethodForm"/> with a new instance.
    /// </remarks>
    public sealed class PaymentMethodFormBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var model = bindingContext.Model;

            if (model != null)
            {
                var metaData = ModelMetadataProviders.Current.GetMetadataForType(null, model.GetType());
                bindingContext.ModelMetadata = metaData;

                var boundModel = base.BindModel(controllerContext, bindingContext);

                return boundModel;
            }

            return null;
        }
    }

    /// <summary>
    /// Select between <see cref="AbstractModelBinder"/> and <see cref="PaymentMethodFormBinder"/>
    /// </summary>
    public sealed class ModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(Type modelType)
        {
            if (modelType == typeof(PaymentMethodForm))
            {
                return new PaymentMethodFormBinder();
            }
            else if (modelType.IsAbstract || modelType.IsInterface)
            {
                return new AbstractModelBinder();
            }

            return null;
        }
    }
}
