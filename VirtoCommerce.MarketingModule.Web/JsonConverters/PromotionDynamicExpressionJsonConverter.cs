﻿using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Marketing.Services;
using VirtoCommerce.MarketingModule.Data.Promotions;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Serialization;
using coreModel = VirtoCommerce.Domain.Marketing.Model;


namespace VirtoCommerce.MarketingModule.Web.JsonConverters
{
    public class PromotionDynamicExpressionJsonConverter : JsonConverter
    {
        private static Type[] _knowTypes = new[] {typeof(coreModel.Promotion), typeof(DynamicPromotion)};

        private readonly IMarketingExtensionManager _marketingExtensionManager;

        private readonly IExpressionSerializer _expressionSerializer;

        public PromotionDynamicExpressionJsonConverter(IMarketingExtensionManager marketingExtensionManager, IExpressionSerializer expressionSerializer)
        {
            _marketingExtensionManager = marketingExtensionManager;
            _expressionSerializer = expressionSerializer;
        }

        #region Overrides of JsonConverter

        public override bool CanWrite => true;
        public override bool CanRead => true;

        public override bool CanConvert(Type objectType)
        {
            return _knowTypes.Any(x => x.IsAssignableFrom(objectType));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var serializerClone = CloneSerializerSettings(serializer);
            var jo = JObject.FromObject(value, serializerClone);

            // Workaround for UI: DynamicPromotion type is hardcoded in HTML template
            Type promotionType = value.GetType();
            var dynamicPromotionType = typeof(DynamicPromotion);
            var typeName = dynamicPromotionType.IsAssignableFrom(promotionType)
                ? dynamicPromotionType.Name
                : promotionType.Name;
            jo.Add("type", typeName);

            coreModel.PromoDynamicExpressionTree expressionTree = GetDynamicPromotion(value);
            if (expressionTree != null)
            {
                jo.Add("dynamicExpression", JToken.FromObject(expressionTree, serializer));
            }

            jo.WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            object result;
            var jObj = JObject.Load(reader);

            var promoType = jObj["type"].Value<string>();
            if (promoType.EqualsInvariant(typeof(DynamicPromotion).Name))
            {
                result = AbstractTypeFactory<DynamicPromotion>.TryCreateInstance();                
            }
            else
            {
                var tryCreateInstance = typeof(AbstractTypeFactory<>).MakeGenericType(objectType).GetMethods().FirstOrDefault(x => x.Name.EqualsInvariant("TryCreateInstance") && !x.GetParameters().Any());
                result = tryCreateInstance?.Invoke(null, null);
            }

            serializer.Populate(jObj.CreateReader(), result);

            var dynamicPromotion = result as DynamicPromotion;
            if (dynamicPromotion != null)
            {
                PopulateDynamicExpression(dynamicPromotion, jObj);
            }

            return result;
        }

        #endregion

        private void PopulateDynamicExpression(DynamicPromotion dynamicPromotion, JObject jObj)
        {
            var dynamicExpressionToken = jObj["dynamicExpression"];
            var dynamicExpression = dynamicExpressionToken?.ToObject<coreModel.PromoDynamicExpressionTree>();
            if (dynamicExpression?.Children != null)
            {
                var conditionExpression = dynamicExpression.GetConditionExpression();
                dynamicPromotion.PredicateSerialized = _expressionSerializer.SerializeExpression(conditionExpression);

                var rewards = dynamicExpression.GetRewards();
                dynamicPromotion.RewardsSerialized = JsonConvert.SerializeObject(rewards, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });

                // Clear availableElements in expression to decrease size
                dynamicExpression.AvailableChildren = null;
                var allBlocks = ((DynamicExpression)dynamicExpression).Traverse(x => x.Children);
                foreach (var block in allBlocks)
                {
                    block.AvailableChildren = null;
                }

                dynamicPromotion.PredicateVisualTreeSerialized = JsonConvert.SerializeObject(dynamicExpression);
            }
        }

        private coreModel.PromoDynamicExpressionTree GetDynamicPromotion(object value)
        {
            coreModel.PromoDynamicExpressionTree result = null;

            var dynamicPromotion = value as DynamicPromotion;
            if (dynamicPromotion != null && dynamicPromotion.PredicateVisualTreeSerialized == null && dynamicPromotion.PredicateSerialized == null 
                && dynamicPromotion.RewardsSerialized == null && !dynamicPromotion.IsTransient())
            {
                return result;
            }

            var etalonEpressionTree = _marketingExtensionManager.PromotionDynamicExpressionTree;
            if (etalonEpressionTree != null)
            {
                result = etalonEpressionTree;

                if (!string.IsNullOrEmpty(dynamicPromotion?.PredicateVisualTreeSerialized))
                {
                    result = JsonConvert.DeserializeObject<coreModel.PromoDynamicExpressionTree>(dynamicPromotion.PredicateVisualTreeSerialized);

                    // Copy available elements from etalon because they not persisted
                    var sourceBlocks = ((DynamicExpression)etalonEpressionTree).Traverse(x => x.Children);
                    var targetBlocks = ((DynamicExpression)result).Traverse(x => x.Children).ToList();

                    foreach (var sourceBlock in sourceBlocks)
                    {
                        foreach (var targetBlock in targetBlocks.Where(x => x.Id == sourceBlock.Id))
                        {
                            targetBlock.AvailableChildren = sourceBlock.AvailableChildren;
                        }
                    }

                    // Copy available elements from etalon
                    result.AvailableChildren = etalonEpressionTree.AvailableChildren;
                }
            }
            return result;
        }

        private JsonSerializer CloneSerializerSettings(JsonSerializer jsonSettings)
        {
            var copySettings = new JsonSerializerSettings
            {
                Context = jsonSettings.Context,
                Culture = jsonSettings.Culture,
                ContractResolver = jsonSettings.ContractResolver,
                ConstructorHandling = jsonSettings.ConstructorHandling,
                CheckAdditionalContent = jsonSettings.CheckAdditionalContent,
                DateFormatHandling = jsonSettings.DateFormatHandling,
                DateFormatString = jsonSettings.DateFormatString,
                DateParseHandling = jsonSettings.DateParseHandling,
                DateTimeZoneHandling = jsonSettings.DateTimeZoneHandling,
                DefaultValueHandling = jsonSettings.DefaultValueHandling,
                EqualityComparer = jsonSettings.EqualityComparer,
                FloatFormatHandling = jsonSettings.FloatFormatHandling,
                Formatting = jsonSettings.Formatting,
                FloatParseHandling = jsonSettings.FloatParseHandling,
                MaxDepth = jsonSettings.MaxDepth,
                MetadataPropertyHandling = jsonSettings.MetadataPropertyHandling,
                MissingMemberHandling = jsonSettings.MissingMemberHandling,
                NullValueHandling = jsonSettings.NullValueHandling,
                ObjectCreationHandling = jsonSettings.ObjectCreationHandling,
                PreserveReferencesHandling = jsonSettings.PreserveReferencesHandling,
                ReferenceLoopHandling = jsonSettings.ReferenceLoopHandling,
                StringEscapeHandling = jsonSettings.StringEscapeHandling,
                TraceWriter = jsonSettings.TraceWriter,
                TypeNameHandling = jsonSettings.TypeNameHandling,
                SerializationBinder = jsonSettings.SerializationBinder,
                TypeNameAssemblyFormatHandling = jsonSettings.TypeNameAssemblyFormatHandling,
                Converters = null
            };
            return JsonSerializer.Create(copySettings);
        }
    }
}