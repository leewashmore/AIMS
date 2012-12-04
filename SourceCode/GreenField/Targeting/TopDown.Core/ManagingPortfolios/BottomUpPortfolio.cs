﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using TopDown.Core.ManagingSecurities;

namespace TopDown.Core.ManagingPortfolios
{
    public class BottomUpPortfolio : IPortfolio
    {
        [DebuggerStepThrough]
        public BottomUpPortfolio(String id, String name, Fund fund)
        {
            this.Id = id;
            this.Name = name;
            this.Fund = fund;
        }

        public String Id { get; private set; }
        public String Name { get; private set; }
        public Fund Fund { get; private set; }

        public override Boolean Equals(Object obj)
        {
            var portfolio = obj as BottomUpPortfolio;
            if (portfolio == null) return false;
            return portfolio.Id == this.Id;
        }
        public override Int32 GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
