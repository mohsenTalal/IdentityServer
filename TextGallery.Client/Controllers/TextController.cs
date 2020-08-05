using IdentityModel.Client;
using ImageGallery.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace TextGallery.Client
{
    [Authorize]
    public class TextController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public TextController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory ??
                throw new ArgumentNullException(nameof(httpClientFactory));
        }
        public async Task<IActionResult> Index()
        {
            await WriteOutIdentiotyInformation();

            return View();
        }

        public async Task<IActionResult> Index1()
        {
            var httpClient = _httpClientFactory.CreateClient("APIClient");

            var request = new HttpRequestMessage(
                HttpMethod.Get,
                "/api/images/");

            var response = await httpClient.SendAsync(
                request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            using (var responseStream = await response.Content.ReadAsStreamAsync())
            {
                return View(new GalleryIndexViewModel(
                    await JsonSerializer.DeserializeAsync<List<Image>>(responseStream)));
            }

        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> OrderAddress()
        {
            var idpClient = _httpClientFactory.CreateClient("IDPAPI");

            var metadataResponse = await idpClient.GetDiscoveryDocumentAsync();

            if (metadataResponse.IsError)
                throw new Exception("metadataResponse: ", metadataResponse.Exception);

            var accessToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

            var userinfoResponse = await idpClient.GetUserInfoAsync(
                    new UserInfoRequest
                    {
                        Address = metadataResponse.UserInfoEndpoint,
                        Token = accessToken
                    }
                );

            if (userinfoResponse.IsError)
                throw new Exception("userinfoResponse: ", userinfoResponse.Exception);

            var address = userinfoResponse.Claims.FirstOrDefault(a => a.Type == "address")?.Value;


            return View(new OrderAddressViewModel(address));
        }

        public async Task<IActionResult> EditImage(Guid id)
        {

            var httpClient = _httpClientFactory.CreateClient("APIClient");

            var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"/api/images/{id}");

            var response = await httpClient.SendAsync(
                request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            using (var responseStream = await response.Content.ReadAsStreamAsync())
            {
                var deserializedImage = await JsonSerializer.DeserializeAsync<Image>(responseStream);

                var editImageViewModel = new EditImageViewModel()
                {
                    Id = deserializedImage.Id,
                    Title = deserializedImage.Title
                };

                return View(editImageViewModel);
            }
        }

        public async Task Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);

            var idpClient = _httpClientFactory.CreateClient("IDPAPI");

            var metadataResponse = await idpClient.GetDiscoveryDocumentAsync();

            if (metadataResponse.IsError)
                throw new Exception("metadataResponse: ", metadataResponse.Exception);


            var tokenRevocationResponse = await idpClient.RevokeTokenAsync(
                    new TokenRevocationRequest
                    {
                        Address = metadataResponse.RevocationEndpoint,
                        ClientId = "textgalleryclient",
                        ClientSecret = "secret2",
                        Token = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.IdToken)
                    }
                );
            if (tokenRevocationResponse.IsError)
            {
                throw new Exception(tokenRevocationResponse.Error);
            }

            var refreshTokenRevocationResponse = await idpClient.RevokeTokenAsync(
                    new TokenRevocationRequest
                    {
                        Address = metadataResponse.RevocationEndpoint,
                        ClientId = "textgalleryclient",
                        ClientSecret = "secret2",
                        Token = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken)
                    }
                );
            if (refreshTokenRevocationResponse.IsError)
            {
                throw new Exception(refreshTokenRevocationResponse.Error);
            }
        }

        public async Task WriteOutIdentiotyInformation()
        {
            //get the saved entity
            var identityToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.IdToken);

            //write it out
            Debug.WriteLine($"Identity token: {identityToken}");

            //write out the user claims
            foreach (var claim in User.Claims)
            {
                Debug.WriteLine($"Claim type: {claim.Type} - Claim value: {claim.Value}");

            }
        }
    }
}
