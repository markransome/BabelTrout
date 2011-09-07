// Copyright 2007-2008 The Apache Software Foundation.
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed 
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace dropkick
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Magnum.CommandLineParser;

    public static class Extensions
    {
        public static string FormatWith(this string input, params object[] args)
        {
            return string.Format(input, args);
        }

        public static bool EqualsIgnoreCase(this string input, string other)
        {
            return input.Equals(other, StringComparison.InvariantCultureIgnoreCase);
        }

        public static T ToEnum<T>(this string input)
        {
            return (T) Enum.Parse(typeof(T), input, true);
        }

        public static string GetDefinition(this IEnumerable<ICommandLineElement> elements, string key)
        {
            return elements
                .Where(x => typeof(IDefinitionElement).IsAssignableFrom(x.GetType()))
                .Select(x => x as IDefinitionElement)
                .Where(x => x.Key == key || x.Key[0] == key[0])
                .Select(x => x.Value)
                .Single();
        }

        public static string GetDefinition(this IEnumerable<ICommandLineElement> elements, string key, string defaultValue)
        {
            return elements
                .Where(x => typeof(IDefinitionElement).IsAssignableFrom(x.GetType()))
                .Select(x => x as IDefinitionElement)
                .Where(x => x.Key == key || x.Key[0] == key[0])
                .Select(x => x.Value)
                .DefaultIfEmpty(defaultValue)
                .Single();
        }

        public static T GetDefinition<T>(this IEnumerable<ICommandLineElement> elements, string key,
                                         Func<string, T> converter)
        {
            return elements
                .Where(x => typeof(IDefinitionElement).IsAssignableFrom(x.GetType()))
                .Select(x => x as IDefinitionElement)
                .Where(x => x.Key == key)
                .Select(x => converter(x.Value))
                .Single();
        }


        public static bool GetSwitch(this IEnumerable<ICommandLineElement> elements, string key)
        {
            return elements
                .Where(x => typeof(ISwitchElement).IsAssignableFrom(x.GetType()))
                .Select(x => x as ISwitchElement)
                .Where(x => x.Key == key || x.Key[0] == key[0])
                .Select(x => x.Value)
                .DefaultIfEmpty(false)
                .Single();
        }

    }
}