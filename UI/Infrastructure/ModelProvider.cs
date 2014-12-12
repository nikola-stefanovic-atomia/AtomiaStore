﻿using System.Web.Mvc;

namespace Atomia.Store.AspNetMvc.Infrastructure
{
    public sealed class ModelProvider : IModelProvider
    {
        public TViewModel Create<TViewModel>()
        {
            return DependencyResolver.Current.GetService<TViewModel>();
        }
    }
}