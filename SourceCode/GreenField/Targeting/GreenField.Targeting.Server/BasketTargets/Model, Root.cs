﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Diagnostics;

namespace GreenField.Targeting.Server.BasketTargets
{
    [DataContract(Name = "BtRootModel")]
    public class RootModel
    {
        [DebuggerStepThrough]
        public RootModel()
        {
            this.Portfolios = new List<PortfolioModel>();
            this.Securities = new List<SecurityModel>();
        }

        [DebuggerStepThrough]
        public RootModel(
            ChangesetModel latestBaseChangeset,
            ChangesetModel latestPortfolioTargetChangeset,
            TargetingTypeGroupModel targetingTypeGroup,
            BasketModel basket,
            IEnumerable<PortfolioModel> portfolios,
            IEnumerable<SecurityModel> securities,
            NullableExpressionModel baseTotalExpression,
            Boolean isModified
        )
            : this()
        {
            this.LatestBaseChangeset = latestBaseChangeset;
            this.LatestPortfolioTargetChangeset = latestPortfolioTargetChangeset;
            this.TargetingTypeGroup = targetingTypeGroup;
            this.Basket = basket;
            this.Portfolios.AddRange(portfolios);
            this.Securities.AddRange(securities);
            this.BaseTotal = baseTotalExpression;
            this.IsModified = isModified;
        }

        [DataMember]
        public ChangesetModel LatestBaseChangeset { get; set; }
        [DataMember]
        public ChangesetModel LatestPortfolioTargetChangeset { get; set; }
        [DataMember]
        public TargetingTypeGroupModel TargetingTypeGroup { get; set; }
        [DataMember]
        public BasketModel Basket { get; set; }
        [DataMember]
        public List<PortfolioModel> Portfolios { get; set; }
        [DataMember]
        public List<SecurityModel> Securities { get; set; }
        [DataMember]
        public NullableExpressionModel BaseTotal { get; set; }
        [DataMember]
        public Boolean IsModified { get; set; }
        [DataMember]
        public Server.SecurityModel SecurityToBeAddedOpt { get; set; }
    }
}