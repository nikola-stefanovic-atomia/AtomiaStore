﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atomia.Store.Core;

namespace Atomia.Store.Services.Fakes
{
    public class FakePricingProvider : ICartPricingService
    {
        public Cart CalculatePricing(Cart cart)
        {
            return cart;
        }
    }
}
