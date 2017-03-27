using System;
using System.Linq;
using VirtoCommerce.Domain.Commerce.Model.Search;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Domain.Marketing.Model.Promotions;
using VirtoCommerce.Domain.Marketing.Model.Promotions.Search;
using VirtoCommerce.Domain.Marketing.Services;
using VirtoCommerce.MarketingModule.Data.Converters;
using VirtoCommerce.MarketingModule.Data.Repositories;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.MarketingModule.Data.Services
{
    public class CouponService : ServiceBase, ICouponService
    {
        private readonly Func<IMarketingRepository> _repositoryFactory;

        public CouponService(Func<IMarketingRepository> repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }

        public GenericSearchResult<Coupon> SearchCoupons(CouponSearchCriteria criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException("criteria");
            }

            using (var repository = _repositoryFactory())
            {
                var query = repository.Coupons;

                if (!string.IsNullOrEmpty(criteria.PromotionId))
                {
                    query = query.Where(c => c.PromotionId == criteria.PromotionId);
                }
                if (!string.IsNullOrEmpty(criteria.Keyword))
                {
                    query = query.Where(c => c.Code.Contains(criteria.Keyword));
                }

                var sortInfos = criteria.SortInfos;
                if (sortInfos.IsNullOrEmpty())
                {
                    sortInfos = new[] { new SortInfo { SortColumn = ReflectionUtility.GetPropertyName<Coupon>(x => x.ModifiedDate), SortDirection = SortDirection.Descending } };
                }
                query = query.OrderBySortInfos(sortInfos);

                var searchResult = new GenericSearchResult<Coupon> { TotalCount = query.Count() };

                var coupons = query.Skip(criteria.Skip).Take(criteria.Take).ToList();
                searchResult.Results = coupons.Select(c => c.ToCoreModel()).ToList();

                return searchResult;
            }
        }

        public Coupon GetByCode(string promotionId, string code)
        {
            using (var repository = _repositoryFactory())
            {
                var coupon = repository.Coupons.FirstOrDefault(c => c.PromotionId == promotionId && c.Code == code);

                return coupon?.ToCoreModel();
            }
        }

        public Coupon GetById(string id)
        {
            using (var repository = _repositoryFactory())
            {
                var coupon = repository.Coupons.FirstOrDefault(c => c.Id == id);

                return coupon?.ToCoreModel();
            }
        }

        public void SaveCoupons(Coupon[] coupons)
        {
            if (coupons == null)
            {
                throw new ArgumentNullException("coupons");
            }

            using (var repository = _repositoryFactory())
            {
                foreach (var coupon in coupons)
                {
                    var entity = coupon.ToDataModel();
                    if (coupon.IsTransient())
                    {
                        repository.Add(entity);
                    }
                    else
                    {
                        repository.Update(entity);
                    }
                }

                CommitChanges(repository);
            }
        }

        public void DeleteCoupons(string[] ids)
        {
            if (ids == null)
            {
                throw new ArgumentNullException("ids");
            }

            using (var repository = _repositoryFactory())
            {
                foreach (var id in ids)
                {
                    var coupon = repository.Coupons.FirstOrDefault(c => c.Id == id);
                    if (coupon != null)
                    {
                        repository.Remove(coupon);
                    }
                }

                CommitChanges(repository);
            }
        }

        public void ClearCoupons(string promotionId)
        {
            using (var repository = _repositoryFactory())
            {
                var coupons = repository.Coupons;
                if (!string.IsNullOrEmpty(promotionId))
                {
                    coupons = coupons.Where(c => c.PromotionId == promotionId);
                }

                foreach (var coupon in coupons)
                {
                    repository.Remove(coupon);
                }

                CommitChanges(repository);
            }
        }

        public bool CheckCoupon(string couponCode, string promotionId)
        {
            using (var repository = _repositoryFactory())
            {
                var isValid = false;

                var coupon = repository.Coupons.FirstOrDefault(c => c.PromotionId == promotionId && c.Code == couponCode);
                if (coupon != null)
                {
                    var couponUsageCount = repository.PromotionUsages.Where(pu =>
                        pu.PromotionId == coupon.PromotionId && pu.CouponCode == coupon.Code &&
                        (!coupon.ExpirationDate.HasValue || coupon.ExpirationDate.Value > DateTime.UtcNow)).Count();
                    if (coupon.MaxUsesNumber == 0 || coupon.MaxUsesNumber >= couponUsageCount)
                    {
                        isValid = true;
                    }
                }

                return isValid;
            }
        }

        public void ApplyCouponUsage(ApplyCouponRequest request)
        {
            using (var repository = _repositoryFactory())
            {
                var couponPromotionIds = repository.Coupons.Where(c => c.Code == request.CouponCode).Select(c => c.PromotionId);

                var couponPromotion = repository.Promotions.FirstOrDefault(p => couponPromotionIds.Contains(p.Id));
                if (couponPromotion != null)
                {
                    var promotionUsage = new Model.PromotionUsage
                    {
                        CouponCode = request.CouponCode,
                        MemberId = request.MemberId,
                        PromotionId = couponPromotion.Id,
                        UsageDate = DateTime.UtcNow
                    };

                    if (!string.IsNullOrEmpty(request.OrderId))
                    {
                        RemoveCouponUsage(request);
                        promotionUsage.OrderId = request.OrderId;
                    }

                    repository.Add(promotionUsage);

                    CommitChanges(repository);
                }
            }
        }

        public void RemoveCouponUsage(ApplyCouponRequest request)
        {
            using (var repository = _repositoryFactory())
            {
                var couponUsage = repository.PromotionUsages.FirstOrDefault(pu =>
                    pu.PromotionId == request.PromotionId && pu.CouponCode == request.CouponCode && pu.MemberId == request.MemberId && string.IsNullOrEmpty(pu.OrderId));
                if (couponUsage != null)
                {
                    repository.Remove(couponUsage);

                    CommitChanges(repository);
                }
            }
        }
    }
}