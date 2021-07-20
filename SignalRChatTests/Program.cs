using System;
using System.Configuration;
using System.Data.SqlClient;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SignalRChatTests
{
    class Program
    {

        static void Main(string[] args)
        {
            IWebDriver driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("https://localhost:44350/");
            SendKeyControl(driver, "#userInput", "Pawel");
            SendKeyControl(driver, "#messageInput", "Wiadomosc testowa.");
            ClickControl(driver, "#sendButton");

            var text = driver.FindElement(By.CssSelector("#messagesList")).GetAttribute("value");

            Console.WriteLine(text);


            //driver.Close();
        }

        public static void SendKeyControl(IWebDriver driver, string by, string value)
        {
            IWebElement element = driver.FindElement(By.CssSelector(by));
            element.SendKeys(value);
        }

        public static void ClickControl(IWebDriver driver, string by)
        {
            IWebElement element = driver.FindElement(By.CssSelector(by));
            element.Click();
        }

        public string GetQueryResult(String vConnectionString, String vQuery)
        {
            var user = String.Empty;
            SqlConnection Connection; // It is for SQL connection

            Connection = new SqlConnection(vConnectionString); // Declare SQL connection with connection string 
            Connection.Open(); // Connect to Database
            //Console.WriteLine("Connection with database is done.");

            //SqlDataAdapter adp = new SqlDataAdapter(vQuery, Connection);  // Execute query on database 
            //adp.Fill(ds);  // Store query result into DataSet object
            //Connection.Close();  // Close connection 
            //Connection.Dispose();   // Dispose connection   
            SqlCommand oCmd = new SqlCommand(vQuery, Connection);
            using (SqlDataReader oReader = oCmd.ExecuteReader())
            {
                while (oReader.Read())
                {
                    user = oReader["User"].ToString();
                }

                Connection.Close();
            }

            return user;
        }
    }
}
