using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace GoogleMusic.Api.Helpers
{
    public class Client
    {
        public string Auth { get; set; }
        public string Lsid { get; set; }
        public string Sid { get; set; }

        private string Xt { get; set; }
        private string Sjsaid { get; set; }
        
        public async Task<string> UploadDataAsync(Uri address, FormBuilder form)
        {
            var request = SetupRequest(address);

            if (!String.IsNullOrEmpty(form.ContentType))
                request.ContentType = form.ContentType;

            request.Method = "POST";
            var requestStream = await request.GetRequestStreamAsync();
            WriteToStream(requestStream, form.GetBytes());
            var response = await request.GetResponseAsync();
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
            {
                var url = address.OriginalString;
                if (string.IsNullOrEmpty(address.Query) || 
                    !(address.Query.IndexOf("?u=0", StringComparison.Ordinal) < 0 || 
                    address.Query.IndexOf("&u=0", StringComparison.Ordinal) < 0))
                {
                    url += "?u=0";
                }
                if (!string.IsNullOrEmpty(Xt)) url += String.Format("&xt={0}", Xt);
                address = new Uri(url);
            }

            var request = (HttpWebRequest)WebRequest.Create(address);
            request.CookieContainer = new CookieContainer();
            // if we know the cookie values then we will set them on each request
            if (!string.IsNullOrEmpty(Sid)) request.CookieContainer.Add(new Uri(MusicManager.BaseUrl), new Cookie("SID", Sid));
            if (!string.IsNullOrEmpty(Lsid)) request.CookieContainer.Add(new Uri(MusicManager.BaseUrl), new Cookie("LSID", Lsid));
            if (!string.IsNullOrEmpty(Sjsaid)) request.CookieContainer.Add(new Uri(MusicManager.BaseUrl), new Cookie("sjsaid", Sjsaid));
            // if we know the authorization token then we will set it on each request
            if (!string.IsNullOrEmpty(Auth))
                request.Headers[HttpRequestHeader.Authorization] = String.Format("GoogleLogin auth={0}", Auth);
            
            return request;
        }

        private void WriteToStream(Stream stream, byte[] data)
        {
            using (stream)
            {
                var buffer = new Byte[checked((uint) Math.Min(1024, data.Length))];
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
            // if we get cookies from the response then we'll update the values for xt en sjsaid.
            foreach (Cookie cookie in response.Cookies)
            {
                if (cookie.Name == "xt") Xt = cookie.Value;
                if (cookie.Name == "sjsaid") Sjsaid = cookie.Value;
            }
            using (var responseStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(responseStream);
                result = reader.ReadToEnd();
            }
            return result;
        }
    }
}
