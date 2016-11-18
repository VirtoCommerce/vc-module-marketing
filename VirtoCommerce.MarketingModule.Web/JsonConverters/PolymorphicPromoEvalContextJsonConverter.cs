using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VirtoCommerce.Domain.Marketing.Model;

namespace VirtoCommerce.MarketingModule.Web.JsonConverters
{
    public class PolymorphicPromoEvalContextJsonConverter : JsonConverter
    {
        private static Type[] _knowTypes = new[] { typeof(PromotionEvaluationContext) };

        public PolymorphicPromoEvalContextJsonConverter()
        {
        }

        public override bool CanWrite { get { return false; } }
        public override bool CanRead { get { return true; } }

        public override bool CanConvert(Type objectType)
        {
            return _knowTypes.Any(x => x.IsAssignableFrom(objectType));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            object retVal = null;
            var obj = JObject.Load(reader);
            if (typeof(PromotionEvaluationContext).IsAssignableFrom(objectType))
            {
                retVal = Platform.Core.Common.AbstractTypeFactory<PromotionEvaluationContext>.TryCreateInstance();
            }
            serializer.Populate(obj.CreateReader(), retVal);
            return retVal;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}