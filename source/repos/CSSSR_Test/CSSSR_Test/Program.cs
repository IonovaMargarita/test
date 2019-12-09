using System;
using System.Net.Http;
using System.Net.Mail;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace CSSSR_Test
{
    class Program
    {
        static IWebDriver driver;

        static void Main(string[] args)
        {
            CheckBrokenLinks("http://blog.csssr.ru/qa-engineer/");           
        }

        public static void CheckBrokenLinks(string url)
        {
            using (driver = new ChromeDriver("."))
            {

                driver.Navigate().GoToUrl(url);

                var links = driver.FindElements(By.CssSelector("a"));

                using (var client = new HttpClient())
                {
                    foreach (var link in links)
                    {
                        var href = link.GetAttribute("href");
                        try
                        {
                            var response = client.GetAsync(href).Result;

                            if (!response.IsSuccessStatusCode)
                            {
                                Console.WriteLine(href + " gave a response code of: " + response.StatusCode);
                            }
                            else
                            {
                                Console.WriteLine(href + " valid link ");
                            }
                        }
                        catch
                        {
                            try
                            {
                                new MailAddress(href);
                            }
                            catch (FormatException)
                            {
                                Console.WriteLine(href + " not valid link ");
                            }

                            Console.WriteLine(href + " valid email ");
                        }
                    }
                }
            }
            Console.ReadLine();
        }    
    }
}
