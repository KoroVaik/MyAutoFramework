using System.Net;
using System.Text;
using System.Text.Json;

namespace Core.Logger.Allure
{
    public static class Log
    {

        public static void Init()
        {
        }

        public static void WriteLine(string message)
        {
            Console.WriteLine(message);
        }

        public static void WriteConsoleAllureLine(string message)
        {
            Console.WriteLine(message);
            //AllureApi.Step(message);
        }

        public static void AddVideoUrl(string name, string url)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(url))
                WriteLine("Video name and url nust not be empty or null");

            //AllureLifecycle.Instance.UpdateTestCase(testCase =>
            //{
            //    testCase.links.Add(new Link
            //    {
            //        name = name,
            //        url = url,
            //        type = "link"
            //    });
            //});
        }

        public static void AddScreenshotPng(byte[] screenshotData)
        {
            try
            {
                //AllureApi.AddAttachment("Screenshot", "image/png", screenshotData, ".png");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Screenshot has not been added. Exception: {ex.Message}");
            }
        }

        public static void AddHtmlAttachment(string html, string attachmentName)
        {
            try
            {
                byte[] buffer = Encoding.ASCII.GetBytes(html);
                //AllureApi.AddAttachment(attachmentName, "text/plain", buffer, ".html");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Html attachment has not been added. Exception: {ex.Message}");
            }
        }

        public static void AddLogsAsAttachment(string logs, string attachmentName)
        {
            try
            {
                byte[] buffer = Encoding.ASCII.GetBytes(logs);
                //AllureApi.AddAttachment(attachmentName, "text/plain", buffer, ".txt");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Attachment has not been added. Exception: {ex.Message}");
            }
        }

        public static void AddRequestLogsAsAttachment(string? reguestBody)
        {
            try
            {
                byte[] buffer = Encoding.ASCII.GetBytes("Request body: " + reguestBody);
                //AllureApi.AddAttachment("Request body", "application/json", buffer, ".txt");
            }
            catch (Exception ex)
            {
                WriteConsoleAllureLine($"Request attachment has not been added. Exception: {ex.Message}");
            }
        }

        public static void WriteRequestLogsToConsole(string requestMethodAndUrl)
        {
            WriteLine($"{DateTime.Now}:\tRequest: {requestMethodAndUrl}");
        }

        public static void WriteRequestLogsToConsole(string requestMethodAndUrl, string? requestBody)
        {
            WriteLine($"{DateTime.Now}:\tRequest: {requestMethodAndUrl}");
            WriteLine($"Request body: {requestBody}");
        }

        public static void AddResponseLogsAsAttachment(HttpStatusCode statusCode, string responseBody)
        {
            try
            {
                byte[] buffer = Encoding.ASCII.GetBytes("Response status: " + (int)statusCode + " " + statusCode + "\nResponse body: " + responseBody);
                //AllureApi.AddAttachment("Response", "application/json", buffer, ".txt");
            }
            catch (Exception ex)
            {
                WriteConsoleAllureLine($"Response attachment has not been added. Exception: {ex.Message}");
            }
        }

        public static void AddTestDataAsAttachment(Dictionary<string, string> testData)
        {
            var serializedJson = JsonSerializer.Serialize(testData, new JsonSerializerOptions { WriteIndented = true });

            try
            {
                byte[] buffer = Encoding.ASCII.GetBytes("Test data: " + serializedJson);
                //AllureApi.AddAttachment("Test data", "application/json", buffer, ".txt");
            }
            catch (Exception ex)
            {
                WriteConsoleAllureLine($"Test data has not been added. Exception: {ex.Message}");
            }
        }

        public static void WriteResponseLogsToConsole(HttpStatusCode statusCode, string responseBody)
        {
            WriteLine($"{DateTime.Now}:\tResponse status: {(int)statusCode} {statusCode}");
            WriteLine($"Response body: {responseBody}");
        }
    }
}
