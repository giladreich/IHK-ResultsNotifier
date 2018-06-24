using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Security.Authentication;
using System.Threading.Tasks;


namespace IHK.ResultsNotifier.Utils
{
    /// <summary>
    /// Project specific object.
    /// </summary>
    public sealed class WebClientIHK : IDisposable
    {
        private const string MOZILLA              = "Mozilla/5.0 (Windows NT 10.0; Win64; x64)";
        private const string APPLE                = "AppleWebKit/537.36 (KHTML, like Gecko)";
        private const string CHROME               = "Chrome/66.0.3359.181 Safari/537.36";
        private static readonly string USER_AGENT = $"{MOZILLA} {APPLE} {CHROME}";

        private const string COOKIE_URL1        = "https://apps.ihk-berlin.de/tibrosBB/BB_auszubildende.jsp";
        private const string COOKIE_URL2        = "https://apps.ihk-berlin.de/favicon.ico";
        private const string COOKIE_PATH        = "https://apps.ihk-berlin.de/tibrosBB";

        private const string EXAMS_PAGE         = "https://apps.ihk-berlin.de/tibrosBB/azubiPruef.jsp";
        private const string EXAMS_RESULTS_PAGE = "https://apps.ihk-berlin.de/tibrosBB/azubiErgebnisse.jsp?id=1";
        private const string LOGIN_PAGE         = "https://apps.ihk-berlin.de/tibrosBB/azubiHome.jsp";

        private const string PARM_USER    = "login";
        private const string PARM_PASS    = "pass";
        private const string PARM_NOTUSED = "anmelden";

        private CookieContainer cookieJar;
        private HttpClientHandler clientHandler;
        private HttpClient client;

        public bool IsAuthenticated { get; private set; }


        public WebClientIHK()
        {
            cookieJar = new CookieContainer();
            clientHandler = new HttpClientHandler
            {
                UseCookies               = true,
                CookieContainer          = cookieJar,
                ClientCertificateOptions = ClientCertificateOption.Automatic
            };

            client = new HttpClient(clientHandler);
        }


        /// <returns>Whether the user is successfully autenticated.</returns>
        public async Task<bool> AuthenticateUser(User user)
        {
            List<Cookie> collectedCookies = await CollectSomeCookies();
            HttpContent payload = BuildPayloadData(user);

            // NOTE: If the server accepted our login credentials, it will replace us a new cookie 
            // with the first cookie from the cookie jar and much tastier! nom nom...
            HttpResponseMessage loginResp = await SendRequestAsync(() => client.PostAsync(LOGIN_PAGE, payload));
            List<Cookie> currentCookies = GetCookies(COOKIE_PATH);

            bool isNewCookieReceived = !currentCookies.SequenceEqual(collectedCookies);
            if (loginResp.StatusCode == HttpStatusCode.OK && isNewCookieReceived)
                return IsAuthenticated = true;

            return IsAuthenticated = false;
        }

        /// <returns>The payload body to be sent in the post request header.</returns>
        private HttpContent BuildPayloadData(User user)
        {
            List<KeyValuePair<string, string>> userCredentials = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(PARM_USER, user.Username),
                new KeyValuePair<string, string>(PARM_PASS, user.Password),
                new KeyValuePair<string, string>(PARM_NOTUSED, "")
            };

            return new FormUrlEncodedContent(userCredentials);
        }

        /// <summarry>
        /// Simulate a website visit before authenticating a user in order to collect some session keys.
        /// NOTE: The order is important, otherwise the server will send us the same session key.
        /// </summarry>
        /// <returns>List of the collected <see cref="Cookie"/>s.</returns>
        private async Task<List<Cookie>> CollectSomeCookies()
        {
            HttpResponseMessage jspResp = await SendRequestAsync(() => client.GetAsync(COOKIE_URL1));
            HttpResponseMessage icoResp = await SendRequestAsync(() => client.GetAsync(COOKIE_URL2));

            return GetCookies(COOKIE_PATH);
        }

        /// <param name="uri">The full domain name path including the protocol(HTTPS/HTTP).</param>
        /// <returns>List of the requested <see cref="Cookie"/>s.</returns>
        private List<Cookie> GetCookies(string uri)
        {
            Uri cookiesPath = new Uri(uri);
            List<Cookie> cookiesList = cookieJar.GetCookies(cookiesPath).Cast<Cookie>().ToList();
#if DEBUG
            cookiesList.ForEach(cookie => Console.WriteLine($"{cookie.Name}: {cookie.Value}"));
#endif
            return cookiesList;
        }

        /// <summarry>
        /// NOTE: We must call them in this order.
        /// The server is pretty smart and will check if we jump to the EXAMS_RESULTS_PAGE link from the EXAMS_PAGE.
        /// If we jump to the EXAMS_RESULTS_PAGE first, it will destroy our session and will ask us to login again.
        /// </summarry>
        /// <returns>The HTML content of the exams page.</returns>
        public async Task<string> GetExamResultsDocument()
        {
            await ValidateAuthentication();

            if (!IsAuthenticated)
                throw new AuthenticationException("Cannot get exam results before the user is authenticated.");

            HttpResponseMessage resultsResp = await SendRequestAsync(() => client.GetAsync(EXAMS_RESULTS_PAGE));

            return await resultsResp.Content.ReadAsStringAsync();
        }

        /// <returns>True if the user is autenticated and has access.</returns>
        public async Task<bool> ValidateAuthentication()
        {
            HttpResponseMessage response = await SendRequestAsync(() => client.GetAsync(EXAMS_PAGE));
            IsAuthenticated = response.StatusCode == HttpStatusCode.OK;

            return IsAuthenticated;
        }

        /// <param name="requestCallback">Request delegate.</param>
        /// <param name="callerMethod">Default parameter for the caller method.</param>
        /// <returns><see cref="HttpResponseMessage"/>.</returns>
        public HttpResponseMessage SendRequest(
            Func<HttpResponseMessage> requestCallback, 
            [CallerMemberName] string callerMethod = "")
        {
            HttpResponseMessage response;

            try
            {
                response = requestCallback.Invoke();
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException(
                    $"[{callerMethod}] - HttpRequestException thrown while sending a request -> "
                    + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"[{callerMethod}] - Exception thrown while sending a request -> "
                    + ex.Message);
            }

            return response;
        }

        /// <param name="requestCallback">Awaitable Request delegate.</param>
        /// <param name="callerMethod">Default parameter for the caller method.</param>
        /// <returns>A <see cref="Task"/> of <see cref="HttpResponseMessage"/>.</returns>
        public async Task<HttpResponseMessage> SendRequestAsync(
            Func<Task<HttpResponseMessage>> requestCallback,
            [CallerMemberName] string callerMethod = "")
        {
            HttpResponseMessage response;

            try
            {
                response = await requestCallback.Invoke();
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException(
                    $"[{callerMethod}] - HttpRequestException thrown while sending a async-request -> " 
                    + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"[{callerMethod}] - Exception thrown while sending a async-request -> "
                    + ex.Message);
            }

            return response;
        }

        public void Dispose()
        {
            clientHandler?.Dispose();
            client?.Dispose();

            clientHandler = null;
            client = null;
            cookieJar = null;
        }

    }
}
