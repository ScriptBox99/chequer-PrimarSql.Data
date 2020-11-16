using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Amazon.DynamoDBv2.Model;

namespace PrimarSql.Data.Extensions
{
    internal static class AttributeValueExtension
    {
        public static AttributeValue ToAttributeValue(this double value)
        {
            return new AttributeValue
            {
                N = value.ToString(CultureInfo.CurrentCulture)
            };
        }

        public static AttributeValue ToAttributeValue(this float value)
        {
            return new AttributeValue
            {
                N = value.ToString(CultureInfo.CurrentCulture)
            };
        }

        public static AttributeValue ToAttributeValue(this int value)
        {
            return new AttributeValue
            {
                N = value.ToString()
            };
        }

        public static AttributeValue ToAttributeValue(this uint value)
        {
            return new AttributeValue
            {
                N = value.ToString()
            };
        }

        public static AttributeValue ToAttributeValue(this long value)
        {
            return new AttributeValue
            {
                N = value.ToString()
            };
        }

        public static AttributeValue ToAttributeValue(this ulong value)
        {
            return new AttributeValue
            {
                N = value.ToString()
            };
        }

        public static AttributeValue ToAttributeValue(this short value)
        {
            return new AttributeValue
            {
                N = value.ToString()
            };
        }

        public static AttributeValue ToAttributeValue(this ushort value)
        {
            return new AttributeValue
            {
                N = value.ToString()
            };
        }

        public static AttributeValue ToAttributeValue(this bool value)
        {
            return new AttributeValue
            {
                BOOL = value
            };
        }

        public static AttributeValue ToAttributeValue(this string value)
        {
            return new AttributeValue
            {
                S = value
            };
        }

        public static AttributeValue ToAttributeValue(this IEnumerable<int> values)
        {
            return new AttributeValue
            {
                NS = values.Select(value => value.ToString()).ToList()
            };
        }
        
        public static AttributeValue ToAttributeValue(this IEnumerable<string> values)
        {
            return new AttributeValue
            {
                SS = values.ToList()
            };
        }
    }
}
