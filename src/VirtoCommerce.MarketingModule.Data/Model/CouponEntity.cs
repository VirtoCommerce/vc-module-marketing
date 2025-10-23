using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VirtoCommerce.MarketingModule.Core.Model.Promotions;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Domain;

namespace VirtoCommerce.MarketingModule.Data.Model;

public class CouponEntity : AuditableEntity, IHasOuterId, IDataEntity<CouponEntity, Coupon>
{
    [Required]
    [StringLength(64)]
    public string Code { get; set; }

    public int MaxUsesNumber { get; set; }

    public int MaxUsesPerUser { get; set; }

    public DateTime? ExpirationDate { get; set; }

    [NotMapped]
    public long TotalUsesCount { get; set; }

    [StringLength(128)]
    public string OuterId { get; set; }

    [StringLength(128)]
    public string MemberId { get; set; }

    #region Navigation Properties

    [StringLength(128)]
    public string PromotionId { get; set; }
    public virtual PromotionEntity Promotion { get; set; }

    #endregion

    public virtual Coupon ToModel(Coupon model)
    {
        ArgumentNullException.ThrowIfNull(model);

        model.Code = Code;
        model.CreatedBy = CreatedBy;
        model.CreatedDate = CreatedDate;
        model.ModifiedBy = ModifiedBy;
        model.ModifiedDate = ModifiedDate;
        model.OuterId = OuterId;

        model.ExpirationDate = ExpirationDate;
        model.Id = Id;
        model.MaxUsesNumber = MaxUsesNumber;
        model.MaxUsesNumber = MaxUsesNumber;
        model.PromotionId = PromotionId;
        model.TotalUsesCount = TotalUsesCount;
        model.MaxUsesPerUser = MaxUsesPerUser;
        model.MemberId = MemberId;

        return model;
    }

    public virtual CouponEntity FromModel(Coupon model, PrimaryKeyResolvingMap pkMap)
    {
        ArgumentNullException.ThrowIfNull(model);

        pkMap.AddPair(model, this);

        Code = model.Code;
        CreatedBy = model.CreatedBy;
        CreatedDate = model.CreatedDate;
        ModifiedBy = model.ModifiedBy;
        ModifiedDate = model.ModifiedDate;
        OuterId = model.OuterId;

        ExpirationDate = model.ExpirationDate;
        Id = model.Id;
        MaxUsesNumber = model.MaxUsesNumber;
        MaxUsesPerUser = model.MaxUsesPerUser;
        MaxUsesNumber = model.MaxUsesNumber;
        PromotionId = model.PromotionId;
        TotalUsesCount = model.TotalUsesCount;
        MemberId = model.MemberId;

        return this;
    }

    public virtual void Patch(CouponEntity target)
    {
        ArgumentNullException.ThrowIfNull(target);

        target.Code = Code;
        target.ExpirationDate = ExpirationDate;
        target.MaxUsesNumber = MaxUsesNumber;
        target.MaxUsesPerUser = MaxUsesPerUser;
        target.MemberId = MemberId;
    }
}
