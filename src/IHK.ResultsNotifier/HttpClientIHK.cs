using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IHK.ResultsNotifier
{
    public class HttpClientIHK : IDisposable
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

        private readonly CookieContainer cookieJar;
        private readonly HttpClientHandler clientHandler;
        private readonly HttpClient client;

        public bool IsAuthenticated { get; set; }

        public HttpClientIHK()
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

        public bool AuthenticateUser(string username, string password)
        {
            List<Cookie> collectedCookies = CollectSomeCookies();

            List<KeyValuePair<string, string>> userCredintials = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(PARM_USER, username),
                new KeyValuePair<string, string>(PARM_PASS, password),
                new KeyValuePair<string, string>(PARM_NOTUSED, "")
            };

            HttpContent postData = new FormUrlEncodedContent(userCredintials);

            // NOTE: If the server accepted our login credintials, it will replace us a new cookie 
            // with the first cookie from the cookie jar and much tastier! nom nom...
            HttpResponseMessage loginResp = SendRequest(() => client.PostAsync(LOGIN_PAGE, postData).Result);
            List<Cookie> cookies          = GetCookies(COOKIE_PATH);
            bool isNewCookieReceived      = !cookies.SequenceEqual(collectedCookies);
            if (isNewCookieReceived)
                return IsAuthenticated = true;

            return false;
        }

        private List<Cookie> CollectSomeCookies()
        {
            // Simulate a website visit before user authentication to collect some sessions keys
            // NOTE: The order is important, otherwise the server will send us the same session key.
            HttpResponseMessage jspResp = SendRequest(() => client.GetAsync(COOKIE_URL1).Result);
            HttpResponseMessage icoResp = SendRequest(() => client.GetAsync(COOKIE_URL2).Result);

            return GetCookies(COOKIE_PATH);
        }

        private List<Cookie> GetCookies(string uri)
        {
            Uri cookiesPath = new Uri(uri);
            List<Cookie> cookiesList = cookieJar.GetCookies(cookiesPath).Cast<Cookie>().ToList();

            cookiesList.ForEach(c => Console.WriteLine($"{c.Name}: {c.Value}"));

            return cookiesList;
        }

        public string GetExamResultsDocument()
        {
            if (!IsAuthenticated)
                throw new InvalidOperationException("Cannot get exam results before the user is authenticated.");

            // NOTE: We must call them in this order.
            // The server is pretty smart and will check if we jump to the EXAMS_RESULTS_PAGE link from the EXAMS_PAGE.
            // If we jump to the EXAMS_RESULTS_PAGE first, it will destroy our session and will ask us to login again.
            HttpResponseMessage examsResp   = SendRequest(() => client.GetAsync(EXAMS_PAGE).Result);
            HttpResponseMessage resultsResp = SendRequest(() => client.GetAsync(EXAMS_RESULTS_PAGE).Result);

            return resultsResp.Content.ReadAsStringAsync().Result;
        }

        public HttpResponseMessage SendRequest(Func<HttpResponseMessage> requestAction)
        {
            try
            {
                return requestAction.Invoke();
            }
            catch (WebException e)
            {
                MessageBox.Show("Lost connection or bad gateway -> " + e.Message, 
                                "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception e)
            {
                // TODO: Probably a fallback method would be a good idea to validate connection.
                MessageBox.Show("Unknown exception while sending GET request -> " + e.Message +
                                "\nCheck your internet connection..." + e.Message,
                                "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return null;
        }

        public Task<HttpResponseMessage> SendRequestAsync(Func<Task<HttpResponseMessage>> requestAction)
        {
            try
            {
                return requestAction.Invoke();
            }
            catch (WebException e)
            {
                MessageBox.Show("Lost connection or bad gateway -> " + e.Message,
                                "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception e)
            {
                MessageBox.Show("Unknown exception while sending GET request -> " + e.Message +
                                "\nCheck your internet connection..." + e.Message,
                                "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return null;
        }

        public void Dispose()
        {
            clientHandler?.Dispose();
            client?.Dispose();
        }

    }
}
