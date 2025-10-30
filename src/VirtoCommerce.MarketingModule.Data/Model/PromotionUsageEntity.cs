using System;
using System.ComponentModel.DataAnnotations;
using VirtoCommerce.MarketingModule.Core.Model.Promotions;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Domain;

namespace VirtoCommerce.MarketingModule.Data.Model;

public class PromotionUsageEntity : AuditableEntity, IDataEntity<PromotionUsageEntity, PromotionUsage>
{
    [StringLength(128)]
    public string ObjectId { get; set; }

    [StringLength(128)]
    public string ObjectType { get; set; }

    [StringLength(64)]
    public string CouponCode { get; set; }

    [StringLength(128)]
    public string UserId { get; set; }

    [StringLength(128)]
    public string UserName { get; set; }

    #region Navigation Properties

    [StringLength(128)]
    public string PromotionId { get; set; }
    public virtual PromotionEntity Promotion { get; set; }

    #endregion

    public virtual PromotionUsage ToModel(PromotionUsage model)
    {
        ArgumentNullException.ThrowIfNull(model);

        model.Id = Id;
        model.CreatedBy = CreatedBy;
        model.CreatedDate = CreatedDate;
        model.ModifiedBy = ModifiedBy;
        model.ModifiedDate = ModifiedDate;

        model.CouponCode = CouponCode;
        model.ObjectId = ObjectId;
        model.ObjectType = ObjectType;
        model.PromotionId = PromotionId;
        model.UserId = UserId;
        model.UserName = UserName;

        return model;
    }

    public virtual PromotionUsageEntity FromModel(PromotionUsage model, PrimaryKeyResolvingMap pkMap)
    {
        ArgumentNullException.ThrowIfNull(model);

        pkMap.AddPair(model, this);

        Id = model.Id;
        CreatedBy = model.CreatedBy;
        CreatedDate = model.CreatedDate;
        ModifiedBy = model.ModifiedBy;
        ModifiedDate = model.ModifiedDate;

        CouponCode = model.CouponCode;
        PromotionId = model.PromotionId;
        ObjectId = model.ObjectId;
        ObjectType = model.ObjectType;
        UserId = model.UserId;
        UserName = model.UserName;

        return this;
    }

    public virtual void Patch(PromotionUsageEntity target)
    {
        ArgumentNullException.ThrowIfNull(target);

        target.ObjectId = ObjectId;
        target.ObjectType = ObjectType;
    }
}
