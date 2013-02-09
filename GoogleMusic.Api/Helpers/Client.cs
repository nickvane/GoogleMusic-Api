using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace GoogleMusic.Api.Helpers
{
    public class Client
    {
        private static string AuthorizationToken;
        private static string Sid;
        private static string Lsid;
        private static CookieContainer AuthorizationCookieContainer = new CookieContainer();
        private static CookieCollection AuthorizationCookies = new CookieCollection();

        public void SetAuthorizationToken(string token)
        {
            AuthorizationToken = token;
        }

        public void SetLsidToken(string token)
        {
            Lsid = token;
        }

        public void SetSidToken(string token)
        {
            Sid = token;
        }

        public async Task<string> UploadDataAsync(Uri address, FormBuilder form, bool saveCookies = false)
        {
            var request = SetupRequest(address);

            if (!String.IsNullOrEmpty(form.ContentType))
                request.ContentType = form.ContentType;

            request.Method = "POST";
            var requestStream = await request.GetRequestStreamAsync();
            WriteToStream(requestStream, form.GetBytes());
            var response = await request.GetResponseAsync();
            if (saveCookies)
            {
                AuthorizationCookieContainer = request.CookieContainer;
                AuthorizationCookies = ((HttpWebResponse)response).Cookies;
            }
            return GetResponseContent((HttpWebResponse)response);
        }

        public async Task<string> DownloadDataAsync(Uri address)
        {
            var request = SetupRequest(address);
            request.Method = "GET";
            var response = await request.GetResponseAsync();
            return GetResponseContent((HttpWebResponse)response);
        }

        private HttpWebRequest SetupRequest(Uri address)
        {
            if (address == null)
                throw new ArgumentNullException("address");

            if (address.ToString().StartsWith(MusicManager.BaseUrl))
                address = new Uri(address.OriginalString + String.Format("?u=0&xt={0}", GetCookieValue("xt")));

            var request = (HttpWebRequest)WebRequest.Create(address);

            request.CookieContainer = AuthorizationCookieContainer;
            if (!string.IsNullOrEmpty(Sid)) AuthorizationCookieContainer.Add(new Uri(MusicManager.BaseUrl), new Cookie("SID", Sid));
            if (!string.IsNullOrEmpty(Lsid)) AuthorizationCookieContainer.Add(new Uri(MusicManager.BaseUrl), new Cookie("LSID", Lsid));

            if (AuthorizationToken != null)
                request.Headers[HttpRequestHeader.Authorization] = String.Format("GoogleLogin auth={0}", AuthorizationToken);
            
            return request;
        }

        public string GetCookieValue(String cookieName)
        {
            return (from Cookie cookie in AuthorizationCookies where cookie.Name.Equals(cookieName) select cookie.Value).FirstOrDefault();
        }

        private void WriteToStream(Stream stream, byte[] data)
        {
            using (stream)
            {
                var buffer = new Byte[checked((uint) Math.Min(1024, (int) data.Length))];
                var ms = new MemoryStream(data);
                int bytesRead;
                while ((bytesRead = ms.Read(buffer, 0, buffer.Length)) != 0)
                {
                    stream.Write(buffer, 0, bytesRead);
                }
            }
        }

        private string GetResponseContent(HttpWebResponse response)
        {
            string result = string.Empty;
            using (var responseStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(responseStream);
                result = reader.ReadToEnd();
            }
            return result;
        }
    }
}
