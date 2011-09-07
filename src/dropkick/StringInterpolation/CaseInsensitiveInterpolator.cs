﻿// Copyright 2007-2010 The Apache Software Foundation.
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
namespace dropkick.StringInterpolation
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using Exceptions;
    using Magnum.Reflection;

    public class CaseInsensitiveInterpolator :
        Interpolator
    {
        readonly Regex _pattern = new Regex("{{(?<key>\\w+)}}");
        readonly Dictionary<Type, Dictionary<string, FastProperty>>  _properties = new Dictionary<Type, Dictionary<string, FastProperty>>();


        public string ReplaceTokens(object settings, string input)
        {
            PrepareDictionary(settings.GetType());
            string output = _pattern.Replace(input, m =>
            {
                string key = "";
                try
                {
                    key = m.Groups["key"].Value;
                    var setting = _properties[settings.GetType()];
                    var pi = setting[key];

                    var value = (string) pi.Get(settings);
                    return value;
                }
                catch (KeyNotFoundException kex)
                {
                    throw new DeploymentConfigurationException("Couldn't find key '{0}' from settings type '{1}'".FormatWith(key, settings.GetType()),kex);
                }
            });

            return output;
        }

        void PrepareDictionary(Type settingsType)
        {
            if (!_properties.ContainsKey(settingsType))
            {
                var dict = new Dictionary<string, FastProperty>(StringComparer.InvariantCultureIgnoreCase);
                _properties.Add(settingsType, dict);
                foreach (var propertyInfo in settingsType.GetProperties())
                {
                    dict.Add(propertyInfo.Name, new FastProperty(propertyInfo));
                }
            }
        }
    }
}