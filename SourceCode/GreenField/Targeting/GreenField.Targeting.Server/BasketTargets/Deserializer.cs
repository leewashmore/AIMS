﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Core = TopDown.Core.ManagingBpst;
using TopDown.Core.Persisting;

namespace GreenField.Targeting.Server.BasketTargets
{
    public class Deserializer
    {
        private Server.Deserializer deserializer;
        private Core.ModelBuilder modelBuilder;
        private Core.BenchmarkInitializer benchmarkInitializer;

        [DebuggerStepThrough]
        public Deserializer(
            Server.Deserializer deserializer,
            Core.ModelBuilder modelBuilder,
            Core.BenchmarkInitializer benchmarkInitializer
        )
        {
            this.deserializer = deserializer;
            this.modelBuilder = modelBuilder;
            this.benchmarkInitializer = benchmarkInitializer;
        }

        public Core.RootModel DeserializeRoot(RootModel model, DateTime benchmarkDate)
        {
            var targetingTypeGroup = this.deserializer.DeserializeTargetingTypeGroup(model.TargetingTypeGroup);
            var benchmarkRepository = this.deserializer.ClaimBenchmarkRepository(benchmarkDate);

            var securities = this.DeserializeSecurities(model.Securities).ToList();
            if (model.SecurityToBeAddedOpt != null)
            {
                var security = this.DeserializeAdditionalSecurity(
                    model.SecurityToBeAddedOpt,
                    targetingTypeGroup,
                    benchmarkRepository
                );
                securities.Add(security);
            }
            var baseTotalExpression = this.modelBuilder.CreateBaseTotalExpression(securities);

            var core = new Core.CoreModel(
                targetingTypeGroup,
                this.deserializer.DeserializeBasket(model.Basket.Id),
                this.DeserializePortfolios(model.Portfolios, securities),
                securities,
                baseTotalExpression
            );

            var result = new Core.RootModel(
                this.DeserializeTargetingTypeGroupBasketSecurityBaseValueChangeset(model.LatestBaseChangeset),
                this.DeserializeBasketPortfolioSecurityTargetChangesetInfo(model.LatestPortfolioTargetChangeset),
                core
            );
            return result;
        }

        private Core.SecurityModel DeserializeAdditionalSecurity(
            Server.SecurityModel securityModel,
            TopDown.Core.ManagingTargetingTypes.TargetingTypeGroup targetingTypeGroup,
            TopDown.Core.ManagingBenchmarks.BenchmarkRepository benchmarkRepository
        )
        {
                var baseExpression = this.modelBuilder.CreateBaseExpression();
                var benchmarkExpression = this.modelBuilder.CreateBenchmarkExpression();
                var result = new Core.SecurityModel(
                    this.deserializer.DeserializeSecurity(securityModel),
                    baseExpression,
                    benchmarkExpression,
                    targetingTypeGroup.GetBgaPortfolios().Select(bgaPortfolio => new Core.PortfolioTargetModel(
                        bgaPortfolio,
                        this.modelBuilder.CreatePortfolioTargetExpression(bgaPortfolio.Name))
                    ).ToList()
                );
                this.benchmarkInitializer.InitializeSecurity(result, benchmarkRepository);
            return result;
        }

        protected IEnumerable<Core.PortfolioModel> DeserializePortfolios(IEnumerable<PortfolioModel> models, IEnumerable<Core.SecurityModel> securities)
        {
            var result = models.Select(x => this.DeserializePortfolio(x, securities)).ToList();
            return result;
        }

        protected Core.PortfolioModel DeserializePortfolio(PortfolioModel model, IEnumerable<Core.SecurityModel> securities)
        {
            var broadGlobalActiveProfolio = this.deserializer.DeserializeBroadGlobalActivePorfolio(model.BroadGlobalActivePortfolio);
            var portfolioTargetTotalExpression = this.modelBuilder.CreatePortfolioTargetTotalExpression(broadGlobalActiveProfolio, securities);
            var result = new Core.PortfolioModel(
                broadGlobalActiveProfolio,
                portfolioTargetTotalExpression
            );
            return result;
        }

        protected IEnumerable<Core.SecurityModel> DeserializeSecurities(IEnumerable<SecurityModel> models)
        {
            var result = models.Select(x => this.DeserializeSecurity(x)).ToList();
            return result;
        }

        protected Core.SecurityModel DeserializeSecurity(SecurityModel model)
        {
            var baseExpression = this.modelBuilder.CreateBaseExpression();
            this.deserializer.PopulateEditableExpression(baseExpression, model.Base);
            var benchmarkExpression = this.modelBuilder.CreateBenchmarkExpression();
            this.deserializer.PopulateUnchangableExpression(benchmarkExpression, model.Benchmark);

            var portfolioTargets = this.DeserializePortfolioTargets(model.PortfolioTargets);
            var result = new Core.SecurityModel(
                this.deserializer.DeserializeSecurity(model.Security),
                baseExpression,
                benchmarkExpression,
                portfolioTargets
            );
            return result;
        }

        protected IEnumerable<Core.PortfolioTargetModel> DeserializePortfolioTargets(IEnumerable<PortfolioTargetModel> models)
        {
            var result = models.Select(x => this.DeserializePortfolioTarget(x)).ToList();
            return result;
        }

        private Core.PortfolioTargetModel DeserializePortfolioTarget(PortfolioTargetModel model)
        {
            var broadGlobalActivePorfolio = this.deserializer.DeserializeBroadGlobalActivePorfolio(model.BroadGlobalActivePortfolio);
            var porfolioTargetExpression = this.modelBuilder.CreatePortfolioTargetExpression(broadGlobalActivePorfolio.Name);
            this.deserializer.PopulateEditableExpression(porfolioTargetExpression, model.PortfolioTarget);
            var result = new Core.PortfolioTargetModel(broadGlobalActivePorfolio, porfolioTargetExpression);
            return result;
        }

        protected BasketPortfolioSecurityTargetChangesetInfo DeserializeBasketPortfolioSecurityTargetChangesetInfo(ChangesetModel model)
        {
            var result = new BasketPortfolioSecurityTargetChangesetInfo(
                model.Id,
                model.Username,
                model.Timestamp,
                model.CalculationId
            );
            return result;
        }

        protected TargetingTypeGroupBasketSecurityBaseValueChangesetInfo DeserializeTargetingTypeGroupBasketSecurityBaseValueChangeset(ChangesetModel model)
        {
            var result = new TargetingTypeGroupBasketSecurityBaseValueChangesetInfo(
                model.Id,
                model.Username,
                model.Timestamp,
                model.CalculationId
            );
            return result;
        }
    }
}