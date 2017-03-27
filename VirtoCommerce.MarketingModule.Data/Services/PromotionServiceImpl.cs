using System;
using System.Collections.Generic;
using System.Linq;
using CacheManager.Core;
using Omu.ValueInjecter;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Domain.Marketing.Services;
using VirtoCommerce.MarketingModule.Data.Model;
using VirtoCommerce.MarketingModule.Data.Promotions;
using VirtoCommerce.MarketingModule.Data.Repositories;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Serialization;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.MarketingModule.Data.Services
{
    public class PromotionServiceImpl : ServiceBase, IPromotionService
    {
        private readonly Func<IMarketingRepository> _repositoryFactory;

        public PromotionServiceImpl(Func<IMarketingRepository> repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }

        #region IMarketingService Members       

        public Promotion[] GetPromotionsByIds(string[] ids)
        {
            using (var repository = _repositoryFactory())
            {
                return repository.GetPromotionsByIds(ids).Select(x => x.ToModel(AbstractTypeFactory<DynamicPromotion>.TryCreateInstance())).ToArray();
            }
        }

        public  void SavePromotions(Promotion[] promotions)
        {
            var pkMap = new PrimaryKeyResolvingMap();
            using (var repository = _repositoryFactory())
            using (var changeTracker = GetChangeTracker(repository))
            {
                var existEntities = repository.GetPromotionsByIds(promotions.Where(x => !x.IsTransient()).Select(x => x.Id).ToArray());
                foreach (var promotion in promotions.OfType<DynamicPromotion>())
                {
                    var sourceEntity = AbstractTypeFactory<PromotionEntity>.TryCreateInstance();
                    if (sourceEntity != null)
                    {
                        sourceEntity = sourceEntity.FromModel(promotion, pkMap);
                        var targetEntity = existEntities.FirstOrDefault(x => x.Id == promotion.Id);
                        if (targetEntity != null)
                        {
                            changeTracker.Attach(targetEntity);
                            sourceEntity.Patch(targetEntity);
                        }
                        else
                        {
                            repository.Add(sourceEntity);
                        }
                    }
                }
                CommitChanges(repository);
                pkMap.ResolvePrimaryKeys();
            }
        }     

        public void DeletePromotions(string[] ids)
        {
            using (var repository = _repositoryFactory())
            {
                repository.RemovePromotions(ids);
                CommitChanges(repository);
            }
        }

        #endregion
    }
}