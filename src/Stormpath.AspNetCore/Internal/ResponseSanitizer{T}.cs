// <copyright file="ResponseSanitizer{T}.cs" company="Stormpath, Inc.">
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
using Stormpath.SDK.Account;
using Stormpath.SDK.Oauth;

namespace Stormpath.AspNetCore.Internal
{
    internal sealed class ResponseSanitizer<T>
    {
        public object Sanitize(T model)
        {
            if (typeof(T) == typeof(IAccount))
            {
                return SanitizeAccount((IAccount)model);
            }

            if (typeof(T) == typeof(IOauthGrantAuthenticationResult))
            {
                return SanitizeToken((IOauthGrantAuthenticationResult)model);
            }

            throw new NotImplementedException($"Cannot sanitize type '{typeof(T).Name}'");
        }

        private object SanitizeAccount(IAccount account)
        {
            return new
            {
                Href = account.Href,
                Username = account.Username,
                ModifiedAt = account.ModifiedAt,
                Status = account.Status.ToString(),
                CreatedAt = account.CreatedAt,
                Email = account.Email,
                MiddleName = account.MiddleName,
                Surname = account.Surname,
                GivenName = account.GivenName,
                FullName = account.FullName,
            };
        }

        private object SanitizeToken(IOauthGrantAuthenticationResult tokenResult)
        {
            return new
            {
                access_token = tokenResult.AccessTokenString,
                expires_in = tokenResult.ExpiresIn,
                refresh_token = tokenResult.RefreshTokenString,
                token_type = tokenResult.TokenType
            };
        }
    }
}
