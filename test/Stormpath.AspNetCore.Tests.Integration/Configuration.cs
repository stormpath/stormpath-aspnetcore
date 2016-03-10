// <copyright file="Configuration.cs" company="Stormpath, Inc.">
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

using System;
using FluentAssertions;
using Xunit;

namespace Stormpath.AspNetCore.Tests.Integration
{
    public class Configuration
    {
        /// <summary>
        /// Issue stormpath/stormpath-framework-tck#20
        /// </summary>
        [Fact]
        [Obsolete("Move to TCK after #3")]
        public void Raise_exception_if_application_cannot_be_found_by_href()
        {
            Action creatingClient = () =>
            {
                var client = TestClient.CreateWithConfiguration(options: new
                {
                    application = new
                    {
                        href = "https://api.stormpath.com/v1/applications/foo" // invalid
                    }
                });
            };

            creatingClient.ShouldThrow<InitializationException>();
        }

        /// <summary>
        /// Issue stormpath/stormpath-framework-tck#21
        /// </summary>
        [Fact]
        [Obsolete("Move to TCK after #3")]
        public void Raise_exception_if_application_cannot_be_found_by_name()
        {
            Action creatingClient = () =>
            {
                var client = TestClient.CreateWithConfiguration(options: new
                {
                    application = new
                    {
                        name = "Foobar" + Guid.NewGuid().ToString()
                    }
                });
            };

            creatingClient.ShouldThrow<InitializationException>();
        }
    }
}
