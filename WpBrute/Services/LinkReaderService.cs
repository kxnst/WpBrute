using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using WpBrute.Entities;

namespace WpBrute.Services
{
    public class LinkReaderService
    {
        public ObservableCollection<WpData> getLinks(string path)
        {
            var links = new ObservableCollection<WpData>();

            foreach (string row in System.IO.File.ReadAllLines(path))
            {
                string url = row.Substring(0, row.IndexOf(","));
                string loginData = row.Substring(row.IndexOf(",") + 1);

                var rx = new System.Text.RegularExpressions.Regex("\\[NEXT_PAIR\\]");
                var data = rx.Split(loginData);

                foreach (string pair in data)
                {
                    var d = new WpData();
                    d.Url = url;
                    d.StatusCode = WpData.STATUS_NEW;
                    d.Login = pair.Substring(0, pair.IndexOf("|"));
                    d.Password = pair.Substring(pair.IndexOf("|") + 1);
                    links.Add(d);
                }
            }

            return links;
        }
    }
}
