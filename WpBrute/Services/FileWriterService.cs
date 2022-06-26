using System.Collections.Generic;
using System.IO;
using WpBrute.Entities;

namespace WpBrute.Services
{
    public class FileWriterService
    {
        public void save(List<WpData> data, string dir)
        {
            dir += "/";

            string filename = dir + System.DateTime.Now.ToString("ddMMyyyyHHmmss") + ".csv";

            if (!File.Exists(filename))
            {
                File.Create(filename).Dispose();
            }

            using (TextWriter tw = new StreamWriter(filename))
            {
                foreach(var row in data)
                {
                    tw.WriteLine(string.Format("{0},{1}|{2}", row.Url, row.Login, row.Password));
                }
            }
        }
    }
}
