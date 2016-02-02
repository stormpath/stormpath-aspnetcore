// <copyright file="DefaultConfigurationBuilder.cs" company="Stormpath, Inc.">
// Copyright (c) 2016 Stormpath, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>

//using System;
//using FlexibleConfiguration;
//using Stormpath.SDK.Impl.Introspection;
//using Stormpath.SDK.Impl.Utility;

//namespace Stormpath.SDK.Impl.Config
//{
//    internal sealed class DefaultConfigurationBuilder
//    {
//        private static readonly string DefaultConfiguration = @"
//client.cacheManager.defaultTtl = 300
//client.cacheManager.defaultTti = 300
//client.baseUrl = https://api.stormpath.com/v1
//client.connectionTimeout = 30
//client.authenticationScheme = SAUTHC1
//";

//        private readonly IEnvironment env;
//        private readonly Platform platform;

//        public DefaultConfigurationBuilder(IEnvironment environment)
//        {
//            this.env = environment;

//            this.platform = Platform.Analyze();
//        }

//        public SdkConfiguration GetConfiguration()
//        {
//            var builder = new FlexibleConfiguration<SdkConfiguration>();

//            builder.Add(DefaultConfiguration);
//            builder.AddFile(this.GetPath("~", ".stormpath", "apiKey.properties"), required: false, root: "client");
//            builder.AddJson(this.GetPath("~", ".stormpath", "stormpath.json"), required: false);
//            builder.AddYaml(this.GetPath("~", ".stormpath", "stormpath.yaml"), required: false);
//            builder.AddJson(this.GetPath(AppDomain.CurrentDomain.BaseDirectory, "stormpath.json"), required: false);
//            builder.AddYaml(this.GetPath(AppDomain.CurrentDomain.BaseDirectory, "stormpath.yaml"), required: false);
//        }

//        private string GetHomePath()
//        {
//            if (this.platform.IsPlatformUnix)
//            {
//                return this.env.GetEnvironmentVariable("HOME");
//            }
//            else
//            {
//                return this.env.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");
//            }
//        }
//    }
//}
