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
                // Переходим на тестовую страницу 
                driver.Navigate().GoToUrl(url);
                 // Получаем список элементов а
                var links = driver.FindElements(By.CssSelector("a"));

                using (var client = new HttpClient())
                {
                    foreach (var link in links)
                    {
                        // Получаем ссылку 
                        var href = link.GetAttribute("href");
                        try
                        {
                           // по ссылке делаем запрос 
                            var response = client.GetAsync(href).Result;

                            // 
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
                            //если ссылка некорректная, проверяем, является ли ссылка почтой
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
