using HoYa.Entities;
using HoYa.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Net;
using System.Net.Http.Headers;
using HoYa.Repository;
using Newtonsoft.Json;
using System.Web.Http.Description;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Net.Mail;
using System.Text;

namespace HoYa.Controllers
{
    [RoutePrefix("auth")]
    public class AuthenticateController : ApiController
    {
        private HoYaContext db = new HoYaContext();
        private AuthRepository repository = null;
        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        public AuthenticateController()
        {
            repository = new AuthRepository();
        }

        /// <summary>  
        /// 新增使用者
        /// </summary>  
        /// <param name="register">新增使用者</param> 
        /// <response code="200">成功</response>  
        /// <response code="400">失敗</response>  
        /// <response code="500">系統內部錯誤</response>
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(RegisterModel register)
        {
            if (register.UserName == null || register.UserName == "") return Content(HttpStatusCode.BadRequest, new JObject(new JProperty("message", "無註冊資訊")));

            if (db.AspNetUsers.FirstOrDefault(apiUser => apiUser.UserName == register.UserName) != null) return Content(HttpStatusCode.BadRequest, new JObject(new JProperty("message", "帳號重複")));


            await repository.CreateUser(new AspNetUser()
            {
                Email = register.Email,
                UserName = register.UserName,
                PhoneNumber = register.PhoneNumber
            }, register.Password);
            AspNetUser user = db.AspNetUsers.FirstOrDefault(apiUser => apiUser.UserName == register.UserName);
            AspNetRole role = db.AspNetRoles.FirstOrDefault(apiRole => apiRole.Sort == 1);
            AspNetUserRole userRole = new AspNetUserRole
            {
                UserId = user.Id,
                RoleId = role.Id
            };
            db.AspNetUserRoles.Add(userRole);

            await db.SaveChangesAsync();
            return await LoginAsync(register.UserName, register.Password);
        }





        /// <summary>  
        /// 登入使用者
        /// </summary>  
        /// <param name="login">登入使用者</param> 
        /// <response code="200">成功</response>  
        /// <response code="400">失敗</response>
        /// <response code="500">系統內部錯誤</response>  
        [AllowAnonymous]
        [Route("Login")]
        public async Task<IHttpActionResult> Login(LoginModel login)
        {
            try
            {
                if (login.UserName == null || login.UserName == "") return Content(HttpStatusCode.BadRequest, new JObject(new JProperty("message", "無登入資訊")));

                return await LoginAsync(login.UserName, login.Password);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, new JObject(new JProperty("message", ex.ToString())));
            }
        }



        /// <summary>  
        /// 修改密碼
        /// </summary>  
        /// <param name="newPassword">新密碼</param> 
        /// <response code="200">成功</response>
        [AllowAnonymous]
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(string newPassword)
        {
            try
            {
                AspNetUser user = await db.AspNetUsers.FindAsync(this.getCurrentUserId());
                user.PasswordHash = repository.getHashedNewPassword(newPassword);
                AspNetUser existedUser = db.AspNetUsers.Find(user.Id);
                db.Entry(existedUser).CurrentValues.SetValues(user);
                await db.SaveChangesAsync();
                await db.Entry(existedUser).GetDatabaseValuesAsync();
                return Ok();
            }
            catch
            {
                return Content(HttpStatusCode.BadRequest, new JObject(new JProperty("message", "密碼不可包含-,_,+,=,{,[,},],',''且長度需大於等於6")));
            }
        }

        /// <summary>  
        /// 重設密碼
        /// </summary>  
        /// <param name="userEmail">重設密碼信箱</param> 
        /// <response code="200">成功</response>
        [AllowAnonymous]
        [Route("ResetPassword")]
        [ResponseType(typeof(ResetPasswordResponseModel))]
        public async Task<IHttpActionResult> ResetPassword(string userEmail)
        {
            try
            {
                AspNetUser user = db.AspNetUsers.FirstOrDefault(apiUser => apiUser.Email == userEmail);
                string tempPassword = user.PasswordHash.Substring(0, 6).Replace("+", "0").Replace("=", "1");
                user.PasswordHash = repository.getHashedNewPassword(tempPassword);
                AspNetUser existedUser = db.AspNetUsers.Find(user.Id);
                db.Entry(existedUser).CurrentValues.SetValues(user);
                await db.SaveChangesAsync();
                await db.Entry(existedUser).GetDatabaseValuesAsync();
                return Ok(new ResetPasswordResponseModel
                {
                    TempPassword = tempPassword,
                    TempPasswordSendTo = existedUser.Email
                });
            }
            catch
            {
                return Content(HttpStatusCode.BadRequest, new JObject(new JProperty("message", "密碼不可包含-,_,+,=,{,[,},],',''且長度需大於等於6")));
            }
        }


        private async Task<IHttpActionResult> LoginAsync(string userName, string password)
        {
            string app = Request.RequestUri.AbsolutePath.Split('/')[1] + "/";
            if (app != "hoya/") app = "";
            var client = new HttpClient
            {
                //BaseAddress = new Uri($"http://118.163.183.248/hoya/")
                //BaseAddress = new Uri($"http://localhost:3001/")
                //BaseAddress = new Uri($"https://hoya.azurewebsites.net/")
                BaseAddress = new Uri($"{Request.RequestUri.Scheme}://{Request.RequestUri.Authority}/{app}")
            };

            var buffer = System.Text.Encoding.UTF8.GetBytes("grant_type=password&username=" + userName + "&password=" + password);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            HttpResponseMessage result = client.PostAsync("token", byteContent).Result;

            if (result.IsSuccessStatusCode)
            {
                AuthenticationToken token = result.Content.ReadAsAsync<AuthenticationToken>().Result;
                AspNetUser user = db.AspNetUsers.FirstOrDefault(apiUser => apiUser.UserName == token.UserName);

                if (user.LockoutEnabled)
                    return Content(HttpStatusCode.BadRequest, new JObject(new JProperty("message", "帳號已被鎖定")));

                else
                {
                    AspNetRole role = await db.AspNetRoles.FindAsync(user.Roles.FirstOrDefault().RoleId);
                    LoginReturnModel loginReturn = new LoginReturnModel
                    {
                        Token = token.Value,
                        UserId = user.Id,
                        UserName = token.UserName,
                        ExpiresIn = token.ExpiresIn,
                        Start = token.Start.ToString("yyyy-MM-dd HH:mm:ss"),
                        End = token.End.ToString("yyyy-MM-dd HH:mm:ss"),
                        RoleId = role.Id
                    };
                    return Ok(loginReturn);
                }
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, new JObject(new JProperty("message", result.Content)));
            }
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repository.Dispose();
            }

            base.Dispose(disposing);
        }

        private string getCurrentUserId()
        {
            ClaimsPrincipal claimsPrincipal = User as ClaimsPrincipal;
            return claimsPrincipal.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name).Value;
        }

        internal class AuthenticationToken
        {
            [JsonProperty(".expires")]
            public DateTime End { get; set; }
            [JsonProperty(".issued")]
            public DateTime Start { get; set; }

            [JsonProperty("access_token")]
            public string Value { get; set; }

            [JsonProperty("expires_in")]
            public string ExpiresIn { get; set; }

            [JsonProperty("userName")]
            public string UserName { get; set; }
            [JsonProperty("clientId")]
            public string Client { get; set; }
        }

        #region Helpers

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        private string ValidateClientAndRedirectUri(HttpRequestMessage request, ref string redirectUriOutput)
        {

            Uri redirectUri;

            var redirectUriString = GetQueryString(Request, "redirect_uri");

            if (string.IsNullOrWhiteSpace(redirectUriString))
            {
                return "redirect_uri is required";
            }

            bool validUri = Uri.TryCreate(redirectUriString, UriKind.Absolute, out redirectUri);

            if (!validUri)
            {
                return "redirect_uri is invalid";
            }

            var clientId = GetQueryString(Request, "client_id");

            if (string.IsNullOrWhiteSpace(clientId))
            {
                return "client_Id is required";
            }
            /*
            var client = repository.FindClient(clientId);

            if (client == null)
            {
                return string.Format("Client_id '{0}' is not registered in the system.", clientId);
            }            
            if (!string.Equals(client.AllowedOrigin, redirectUri.GetLeftPart(UriPartial.Authority), StringComparison.OrdinalIgnoreCase))
            {
                return string.Format("The given URL is not allowed by Client_id '{0}' configuration.", clientId);
            }
            */
            redirectUriOutput = redirectUri.AbsoluteUri;

            return string.Empty;

        }

        private string GetQueryString(HttpRequestMessage request, string key)
        {
            var queryStrings = request.GetQueryNameValuePairs();

            if (queryStrings == null) return null;

            var match = queryStrings.FirstOrDefault(keyValue => string.Compare(keyValue.Key, key, true) == 0);

            if (string.IsNullOrEmpty(match.Value)) return null;

            return match.Value;
        }

        //產生訪問權杖給應用程式使用
        private JObject GenerateLocalAccessTokenResponse(string userName)
        {

            var tokenExpiration = TimeSpan.FromDays(1);

            ClaimsIdentity identity = new ClaimsIdentity(OAuthDefaults.AuthenticationType);

            identity.AddClaim(new Claim(ClaimTypes.Name, userName));
            identity.AddClaim(new Claim("role", "user"));

            var props = new AuthenticationProperties()
            {
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.Add(tokenExpiration),
            };

            var ticket = new AuthenticationTicket(identity, props);

            var accessToken = Startup.OAuthBearerOptions.AccessTokenFormat.Protect(ticket);

            JObject tokenResponse = new JObject(
                                        new JProperty("userName", userName),
                                        new JProperty("access_token", accessToken),
                                        new JProperty("token_type", "bearer"),
                                        new JProperty("expires_in", tokenExpiration.TotalSeconds.ToString()),
                                        new JProperty(".issued", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")),
                                        new JProperty(".expires", DateTime.UtcNow.Add(tokenExpiration).ToString("yyyy-MM-dd HH:mm:ss"))

        );

            return tokenResponse;
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string Email { get; set; }
            public string Gender { get; set; }
            public string FamilyName { get; set; }
            public string GivenName { get; set; }
            public string ExternalAccessToken { get; set; }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer) || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    Email = identity.FindFirstValue(ClaimTypes.Email),
                    Gender = identity.FindFirstValue("Gender"),
                    FamilyName = identity.FindFirstValue("FamilyName"),
                    GivenName = identity.FindFirstValue(ClaimTypes.GivenName),
                    ExternalAccessToken = identity.FindFirstValue("ExternalAccessToken"),
                };
            }
        }

        #endregion

        /* 外部註冊與登入
    // GET api/Account/ExternalLogin
    [OverrideAuthentication]
    [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
    [AllowAnonymous]
    [Route("ExternalLogin", Name = "ExternalLogin")]
    public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
    {
      string redirectUri = string.Empty;

      if (error != null)
      {
          return BadRequest(Uri.EscapeDataString(error));
      }

      if (!User.Identity.IsAuthenticated)
      {
          return new ChallengeResult(provider, this);
      }

      var redirectUriValidationResult = ValidateClientAndRedirectUri(this.Request, ref redirectUri);

      if (!string.IsNullOrWhiteSpace(redirectUriValidationResult))
      {
          return BadRequest(redirectUriValidationResult);
      }

      ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

      if (externalLogin == null)
      {
          return InternalServerError();
      }

      if (externalLogin.LoginProvider != provider)
      {
          Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
          return new ChallengeResult(provider, this);
      }

      IdentityUser user = await authRepository.FindAsync(new UserLoginInfo(externalLogin.LoginProvider, externalLogin.ProviderKey));

      bool hasRegistered = user != null;

      redirectUri = string.Format("{0}#externalAccessToken={1}&provider={2}&haslocalaccount={3}&email={4}&gender={5}&familyName={6}&givenName={7}",
                                      redirectUri,
                                      externalLogin.ExternalAccessToken,
                                      externalLogin.LoginProvider,
                                      hasRegistered.ToString(),
                                      externalLogin.Email,
                                      externalLogin.Gender,
                                      externalLogin.FamilyName,
                                      externalLogin.GivenName);

      return Redirect(redirectUri);

    }

    // POST api/Account/RegisterExternal
    [AllowAnonymous]
    [Route("RegisterExternal")]
    public async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model)
    {

      if (!ModelState.IsValid)
      {
          return BadRequest(ModelState);
      }

      var verifiedAccessToken = await VerifyExternalAccessToken(model.Provider, model.ExternalAccessToken);
      if (verifiedAccessToken == null)
      {
          return BadRequest("Invalid Provider or External Access Token");
      }

      IdentityUser user = await authRepository.FindAsync(new UserLoginInfo(model.Provider, verifiedAccessToken.user_id));

      bool hasRegistered = user != null;

      if (hasRegistered)
      {
          return BadRequest("External user is already registered");
      }

      user = new User() { Email = model.Email, UserName = model.Email
         // , Gender = model.Gender, FamilyName = model.FamilyName, GivenName = model.GivenName
      };

      IdentityResult result = await authRepository.Create(user);
      if (!result.Succeeded)
      {
          return GetErrorResult(result);
      }

      var info = new ExternalLoginInfo()
      {
          DefaultUserName = model.Email,
          Login = new UserLoginInfo(model.Provider, verifiedAccessToken.user_id),
          Email = model.Email
      };

      result = await authRepository.AddLoginAsync(user.Id, info.Login);
      if (!result.Succeeded)
      {
          return GetErrorResult(result);
      }

      //generate access token response
      var accessTokenResponse = GenerateLocalAccessTokenResponse(model.FamilyName + model.GivenName);

      return Ok(accessTokenResponse);
    }

    [AllowAnonymous]
    [HttpGet]
    [Route("ObtainLocalAccessToken")]
    public async Task<IHttpActionResult> ObtainLocalAccessToken(string provider, string externalAccessToken)
    {
      if (string.IsNullOrWhiteSpace(provider) || string.IsNullOrWhiteSpace(externalAccessToken))
      {
          return BadRequest("Provider or external access token is not sent");
      }

      var verifiedAccessToken = await VerifyExternalAccessToken(provider, externalAccessToken);
      if (verifiedAccessToken == null)
      {
          return BadRequest("Invalid Provider or External Access Token");
      }

      IdentityUser user = await authRepository.FindAsync(new UserLoginInfo(provider, verifiedAccessToken.user_id));

      bool hasRegistered = user != null;

      if (!hasRegistered)
      {
          return BadRequest("External user is not registered");
      }

      //generate access token response
      var accessTokenResponse = GenerateLocalAccessTokenResponse(user.UserName);

      return Ok(accessTokenResponse);
    }
    */
    }
}
