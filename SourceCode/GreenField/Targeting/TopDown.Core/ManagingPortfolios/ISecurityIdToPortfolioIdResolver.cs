﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TopDown.Core.ManagingPortfolios
{
	public interface ISecurityIdToPortfolioIdResolver
	{
		String TryResolveToPortfolioId(String securityId);
	}
}
