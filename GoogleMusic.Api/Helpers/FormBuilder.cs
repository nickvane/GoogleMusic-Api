using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GoogleMusic.Api.Helpers
{

    public class FormBuilder
    {
        private readonly string boundary = "----------" + DateTime.Now.Ticks.ToString("x");
        private readonly MemoryStream ms;

        public static FormBuilder Empty
        {
            get
            {
                var b = new FormBuilder();
                b.Close();
                return b;
            }
        }

        public string ContentType
        {
            get { return "multipart/form-data; boundary=" + boundary; }
        }

        public FormBuilder()
        {
            ms = new MemoryStream();
        }

        public FormBuilder(Dictionary<string, string> fields, bool shouldClose) : this()
        {
            AddFields(fields);
            if (shouldClose) Close();
        }

        public void AddFields(Dictionary<string, string> fields)
        {
            foreach (var key in fields)
                AddField(key.Key, key.Value);
        }

        public void AddField(string key, string value)
        {
            var sb = new StringBuilder();

            sb.AppendFormat("\r\n--{0}\r\n", boundary);
            sb.AppendFormat("Content-Disposition: form-data; name=\"{0}\";\r\n\r\n{1}", key, value);

            byte[] sbData = Encoding.UTF8.GetBytes(sb.ToString());

            ms.Write(sbData, 0, sbData.Length);
        }

        public void AddFile(string name, string fileName, byte[] file)
        {
            var sb = new StringBuilder();

            sb.AppendFormat("\r\n--{0}\r\n", boundary);
            sb.AppendFormat("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n", name, fileName);
            sb.AppendFormat("Content-Type: {0}\r\n\r\n", "application/octet-stream");

            byte[] sbData = Encoding.UTF8.GetBytes(sb.ToString());
            ms.Write(sbData, 0, sbData.Length);

            ms.Write(file, 0, file.Length);
        }

        public void Close()
        {
            var sb = new StringBuilder();

            sb.AppendLine("\r\n--" + boundary + "--\r\n");

            byte[] sbData = Encoding.UTF8.GetBytes(sb.ToString());
            ms.Write(sbData, 0, sbData.Length);
        }

        public byte[] GetBytes()
        {
            return ms.ToArray();
        }

        public string GetString()
        {
            byte[] bytes = GetBytes();
            return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        }
    }
}
