/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Blazr.Auth
{
    public class SessionTokenManagement
    {
        /// <summary>
        /// Method to issue a new SessionToken
        /// </summary>
        /// <param name="userClaims"></param>
        /// <param name="jwtTokenSetup"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        public static SessionToken GetNewSessionToken(Claim[] userClaims, JwtTokenSetup jwtTokenSetup)
        {
            var jwtToken = GetNewJwtToken(userClaims, jwtTokenSetup);
            return new SessionToken() { JwtToken = jwtToken, IsAuthenticated = true };
        }

        /// <summary>
        /// Method to check if a Session Token has expired
        /// </summary>
        /// <param name="token">Issued Session Token</param>
        /// <returns></returns>
        public static bool HasTokenExpired(SessionToken sessionToken)
        {
            var isValidToken = sessionToken?.JwtToken is not null;
            if (isValidToken && TryGetJwtTokenOnject(sessionToken.JwtToken, out JwtSecurityToken jwtToken))
                return jwtToken.ValidTo < DateTime.UtcNow;
            return true;
        }

        /// <summary>
        /// Method to Validate a Session Token
        /// </summary>
        /// <param name="sessionToken">Issued Session Token</param>
        /// <param name="jwtTokenSetup">Token setup</param>
        /// <param name="claimsPrincipal">Claims principal from the Session Token</param>
        /// <returns></returns>
        public static bool TryValidateToken(SessionToken sessionToken, JwtTokenSetup jwtTokenSetup, out ClaimsPrincipal claimsPrincipal)
        {
            claimsPrincipal = null;
            bool isValid;
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenValidationParameters = GetValidationParameters(jwtTokenSetup);
            try
            {
                claimsPrincipal = tokenHandler.ValidateToken(sessionToken.JwtToken, tokenValidationParameters, out SecurityToken validatedToken);
                JwtSecurityToken jwtToken = validatedToken as JwtSecurityToken;
                isValid = jwtToken is not null;
            }
            catch (SecurityTokenException)
            {
                isValid = false;
            }
            catch (Exception)
            {
                throw;
            }
            return isValid;
        }

        /// <summary>
        /// Expiration Validation Method for a Seciurity Token
        /// Matches the Delegate pattern required for TokenValidationParameters.LiftimeValidator
        /// </summary>
        /// <param name="notBefore"></param>
        /// <param name="expires"></param>
        /// <param name="token"></param>
        /// <param name="params"></param>
        /// <returns></returns>
        public static bool JwtTokenLifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken token, TokenValidationParameters @params)
        {
            if (expires != null)
                return expires > DateTime.Now;
            return false;
        }


        /// <summary>
        /// Method to Retrieve the Claims from a JwtToken
        /// </summary>
        /// <param name="jwt"></param>
        /// <returns></returns>
        public static IEnumerable<Claim> ParseClaimsFromJwt(string jwtToken)
        {
            var claims = new List<Claim>();
            if (!string.IsNullOrEmpty(jwtToken))
            {
                var payload = jwtToken.Split('.')[1];
                if (!string.IsNullOrEmpty(payload))
                {
                    try
                    {
                        var jsonBytes = ParseBase64WithoutPadding(payload);
                        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
                        if (claims is not null)
                            claims
                                .AddRange(keyValuePairs
                                .Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));
                    }
                    catch { }
                }
            }
            return claims;
        }

        /// <summary>
        /// Method to get a JwtSecurityToken fom a SessionToken
        /// </summary>
        /// <param name="sessionToken"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static bool TryGetJwtSecurityTokenOnject(SessionToken sessionToken, out JwtSecurityToken token)
        {
            token = null;
            if (sessionToken?.JwtToken is null)
                return false;

            var handler = new JwtSecurityTokenHandler();
            try
            {
                token = handler.ReadJwtToken(sessionToken.JwtToken);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return false;
        }

        /// <summary>
        /// Tries to get a ClaimsPrincipal from a JwtToken
        /// </summary>
        /// <param name="jwtToken">Jwt Token to use</param>
        /// <param name="authenticationType">Authentication Type to use in ClaimsPrincipal constructor</param>
        /// <param name="id">returned ClaimsPrincipal </param>
        /// <returns>True if successful</returns>
        public static bool TryGetFromJwt(string jwtToken, string authenticationType, out ClaimsPrincipal claimsPrincipal)
        {
            var claims = SessionTokenManagement.ParseClaimsFromJwt(jwtToken);
            var isIdentity = claims.Any();
            claimsPrincipal = isIdentity
                ? new ClaimsPrincipal(new ClaimsIdentity(SessionTokenManagement.ParseClaimsFromJwt(jwtToken), authenticationType))
                : AnonymousClaimsPrincipal;

            return isIdentity;
        }

        /// <summary>
        /// Tries to get a ClaimsPrincipal from a SessionToken
        /// </summary>
        /// <param name="sessionToken">Session Token to use</param>
        /// <param name="authenticationType">Authentication Type to use in ClaimsPrincipal constructor</param>
        /// <param name="id">returned ClaimsPrincipal </param>
        /// <returns>True if successful</returns>
        public static bool TryGetFromSessionToken(SessionToken sessionToken, string authenticationType, out ClaimsPrincipal claimsPrincipal)
        {
            var isIdentity = sessionToken is not null && !sessionToken.IsEmpty;
            IEnumerable<Claim> claims = null;
            if (isIdentity)
            {
                claims = SessionTokenManagement.ParseClaimsFromJwt(sessionToken.JwtToken);
                isIdentity = claims.Any();
            }
            claimsPrincipal = isIdentity
                ? new ClaimsPrincipal(new ClaimsIdentity(claims, authenticationType))
                : AnonymousClaimsPrincipal;

            return isIdentity;
        }

        public static ClaimsPrincipal AnonymousClaimsPrincipal
            => new ClaimsPrincipal(new ClaimsIdentity(null, ""));

        /// <summary>
        /// Method to get and the Claims from a set of User Credentials
        /// Looks up the Specific Identity from the TestClaimsPrincipals class
        /// </summary>
        /// <param name="userCredentials"></param>
        /// <param name="claims"></param>
        /// <returns></returns>
        // TODO - check Dumb system and remove
        //public static bool TryGetIdentity(IdentityLoginCredentials userCredentials, out Claim[] claims)
        //{
        //    claims = null;
        //    var isValid = false;
        //    if (Enum.TryParse(userCredentials.UserName, out TestUserType userType))
        //    {
        //        if (userType is not TestUserType.None)
        //        {
        //            claims = userType switch
        //            {
        //                TestUserType.Admin => TestClaimsPrincipals.AdminClaims,
        //                TestUserType.User => TestClaimsPrincipals.UserClaims,
        //                TestUserType.Visitor => TestClaimsPrincipals.VisitorClaims,
        //                _ => TestClaimsPrincipals.AnonymousClaims
        //            };
        //            isValid = true;
        //        }
        //    }
        //    return isValid;
        //}

        private static TokenValidationParameters GetValidationParameters(JwtTokenSetup jwtTokenSetup)
        {
            return new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = true,
                ValidIssuer = jwtTokenSetup.Issuer,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtTokenSetup.Key)),
                ValidateLifetime = true,
                LifetimeValidator = JwtTokenLifetimeValidator,
            };
        }

        private static string GetNewJwtToken(Claim[] userClaims, JwtTokenSetup jwtTokenSetup)
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtTokenSetup.Key));
            var credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecToken = new JwtSecurityToken(
                issuer: jwtTokenSetup.Issuer,
                expires: DateTime.UtcNow.AddSeconds(jwtTokenSetup.ExpireSeconds),
                claims: userClaims,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtSecToken);
        }

        /// <summary>
        /// Method to generate a Refresh Token
        /// </summary>
        /// <returns></returns>
        private static string GetRefreshToken()
        {
            var key = new Byte[32];
            using var refreshTokenGenerator = RandomNumberGenerator.Create();
            refreshTokenGenerator.GetBytes(key);
            return Convert.ToBase64String(key);
        }

        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }

        private static bool TryGetJwtTokenOnject(string jwtToken, out JwtSecurityToken token)
        {
            token = null;
            var handler = new JwtSecurityTokenHandler();
            try
            {
                token = handler.ReadJwtToken(jwtToken);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}

