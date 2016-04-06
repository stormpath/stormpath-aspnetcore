// <copyright file="OwinKeys.cs" company="Stormpath, Inc.">
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

namespace Stormpath.AspNetCore.Owin
{
    /// <summary>
    /// OWIN keys, as defined in <see href="http://owin.org/html/spec/owin-1.0.html"/>.
    /// </summary>
    public static class OwinKeys
    {
        public static readonly string RequestBody = "owin.RequestBody";

        public static readonly string RequestHeaders = "owin.RequestHeaders";

        public static readonly string RequestMethod = "owin.RequestMethod";

        public static readonly string RequestPath = "owin.RequestPath";

        public static readonly string RequestPathBase = "owin.RequestPathBase";

        public static readonly string RequestProtocol = "owin.RequestProtocol";

        public static readonly string RequestQueryString = "owin.RequestQueryString";

        public static readonly string RequestScheme = "owin.RequestScheme";

        public static readonly string RequestUser = "owin.RequestUser";

        public static readonly string RequestUserOld = "server.User";

        public static readonly string ResponseBody = "owin.ResponseBody";

        public static readonly string ResponseHeaders = "owin.ResponseHeaders";

        public static readonly string ResponseStatusCode = "owin.ResponseStatusCode";

        public static readonly string ResponseReasonPhrase = "owin.ResponseReasonPhrase";

        public static readonly string ResponseProtocol = "owin.ResponseProtocol";

        public static readonly string CallCancelled = "owin.CallCancelled";
    }
}
