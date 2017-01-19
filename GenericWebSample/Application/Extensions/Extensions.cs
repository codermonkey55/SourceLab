using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace GenericWebSample.Application.Extensions
{
    public static class Extensions
    {
        public static T ToContent<T>(this IDictionary source, string keyValueSeparator, string sequenceSeparator) where T : class
        {
            if (source == null)
                throw new ArgumentException($"Parameter \"{nameof(source)}\" cannot be null.");

            return source.Cast<DictionaryEntry>()
                         .Aggregate(new StringBuilder(),
                                   (sb, dicEnt) => sb.Append(dicEnt.Key + keyValueSeparator + dicEnt.Value + sequenceSeparator),
                                    sb => sb.ToString(0, sb.Length - 1)) as T;
        }

        public static string ToStringContent(this IDictionary source)
        {
            if (source == null)
                throw new ArgumentException($"Parameter \"{nameof(source)}\" cannot be null.");

            return source.Cast<DictionaryEntry>().Aggregate(new StringBuilder(), (sb, dicEnt) => sb.AppendLine($"\t Key: {dicEnt.Key} | Value: {dicEnt.Value}"), sb => sb.ToString());
        }

        public static string ToStringContent(this IDictionary source, string keyValueSeparator, string sequenceSeparator)
        {
            if (source == null)
                throw new ArgumentException($"Parameter \"{nameof(source)}\" cannot be null.");

            return source.Cast<DictionaryEntry>()
                         .Aggregate(new StringBuilder(),
                                   (sb, dicEnt) => sb.Append(dicEnt.Key + keyValueSeparator + dicEnt.Value + sequenceSeparator),
                                    sb => sb.ToString(0, sb.Length - 1));
        }
    }
}