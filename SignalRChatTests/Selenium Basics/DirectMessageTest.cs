using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SignalRChatDBCon;

namespace SignalRChatTests.Selenium_Basics
{
    class DirectMessageTest
    {
        IWebDriver driver = new ChromeDriver();

        [SetUp]
        public void Initialize()
        {
            driver.Navigate().GoToUrl("https://localhost:44350/");
            driver.Manage().Window.Maximize();
            ((IJavaScriptExecutor)driver).ExecuteScript("window.open();");
            driver.SwitchTo().Window(driver.WindowHandles.Last());
            driver.Navigate().GoToUrl("https://localhost:44350/");
        }

        [Test]
        public void ExecuteTest()
        {
            // expected values
            var expectedUser1 = "Pawel";
            var expectedUser2 = "Hanna";
            var expectedText = "Testowa wiadomość bezpośrednia";
            var expectedSendDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

            // set users
            Program.SendKeyControl(driver, "#userInput", expectedUser2);
            Program.ClickControl(driver, "#setUserButton");
            driver.SwitchTo().Window(driver.WindowHandles.First());
            Program.SendKeyControl(driver, "#userInput", expectedUser1);
            Program.ClickControl(driver, "#setUserButton");

            // send message
            Program.ClickControl(driver, "a[href='#']");
            Program.SendKeyControl(driver, "#messagesInput" + expectedUser2, expectedText);
            Program.ClickControl(driver, "#sendDirectMessage" + expectedUser2);

            // check what's in database
            string query = "Select top 1 * from Message order by Id desc";
            var result = GetQueryResult(query);

            // received values
            string receivedUser = result.Rows[0]["User"].ToString();
            string receivedText = result.Rows[0]["Text"].ToString();
            string receivedSendDate = result.Rows[0]["SendDate"].ToString();
            receivedSendDate = receivedSendDate.Substring(0, 16);   // cut out seconds, because of delay

            Assert.AreEqual(expectedUser1, receivedUser);
            Assert.AreEqual(expectedText, receivedText);
            Assert.AreEqual(expectedSendDate, receivedSendDate);

        }

        [TearDown]
        public void CleanUp()
        {
            driver.Close();
        }


        public DataTable GetQueryResult(String query)
        {
            //SqlConnection Connection;  // It is for SQL connection
            DataSet ds = new DataSet();  // it is for store query result

            try
            {
                //Connection = new SqlConnection(connectionString);  // Declare SQL connection with connection string 
                //Connection.Open();  // Connect to Database
                DbCon Connection = new DbCon();
                Connection.Connect();
                Console.WriteLine("Connection with database is done.");

                SqlDataAdapter adp = new SqlDataAdapter(query, Connection.Connection);  // Execute query on database 
                adp.Fill(ds);  // Store query result into DataSet object   
                //Connection.Close();  // Close connection 
                //Connection.Dispose();   // Dispose connection
                Connection.Disconnect();
            }
            catch (Exception E)
            {
                Console.WriteLine("Error in getting result of query.");
                Console.WriteLine(E.Message);
                return new DataTable();
            }
            return ds.Tables[0];
        }

        public string GetQueryResultString(String connectionString, String query, String column)
        {
            var user = String.Empty;
            SqlConnection Connection;  // It is for SQL connection
            DataSet ds = new DataSet();  // it is for store query result

            try
            {
                Connection = new SqlConnection(connectionString);  // Declare SQL connection with connection string 
                Connection.Open();  // Connect to Database
                SqlCommand oCmd = new SqlCommand(query, Connection);
                using (SqlDataReader oReader = oCmd.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        user = oReader[column].ToString();
                    }
                    Connection.Close();
                }
            }
            catch (Exception E)
            {
                Console.WriteLine("Error in getting result of query.");
                Console.WriteLine(E.Message);
            }
            return user;
        }

    }
}
