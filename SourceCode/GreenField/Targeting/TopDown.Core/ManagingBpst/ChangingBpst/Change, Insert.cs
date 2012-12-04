﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace TopDown.Core.ManagingBpst.ChangingBpst
{
    public class InsertChange : IChange
    {
        [DebuggerStepThrough]
        public InsertChange(
			String portfolioId,
			String securityId,
			Decimal targetAfter,
			String comment
		)
        {
            this.PortfolioId = portfolioId;
            this.SecurityId = securityId;
            this.TargetAfter = targetAfter;
			this.Comment = comment;
        }

        public String PortfolioId { get; private set; }
        public String SecurityId { get; private set; }
        public Decimal TargetAfter { get; private set; }
		public String Comment { get; private set; }

        [DebuggerStepThrough]
        public void Accept(IChangeResolver resolver)
        {
            resolver.Resolve(this);
        }

		
	}
}
