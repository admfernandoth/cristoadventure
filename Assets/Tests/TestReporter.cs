using NUnit.Framework;
using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;

public class TestReporter
{
    private const string REPORT_PATH = "TestResults_Report.html";
    private const string SCREENSHOT_PATH = "TestScreenshots";
    private List<TestResult> allResults = new List<TestResult>();
    private Dictionary<string, List<string>> categoryResults = new Dictionary<string, List<string>>();

    [Serializable]
    public class TestResult
    {
        public string testName;
        public string category;
        public bool passed;
        public string message;
        public string duration;
        public string screenshotPath;
    }

    [Serializable]
    public class TestReport
    {
        public DateTime testDate;
        public int totalTests;
        public int passedTests;
        public int failedTests;
        public float totalDuration;
        public List<TestResult> testResults;
    }

    public void RunAllTests()
    {
        // Create screenshots directory
        if (!Directory.Exists(SCREENSHOT_PATH))
        {
            Directory.CreateDirectory(SCREENSHOT_PATH);
        }

        // Clear previous results
        allResults.Clear();
        categoryResults.Clear();

        // Log test start
        Debug.Log("=== STARTING TEST SUITE ===");
        Debug.Log($"Test run started at: {DateTime.Now}");

        // Define test categories and their methods
        var testCategories = new Dictionary<string, Type>
        {
            { "GameplayTests", typeof(GameplayTests) },
            { "InteractionTests", typeof(InteractionTests) },
            { "PuzzleTests", typeof(PuzzleTests) },
            { "UISystemTests", typeof(UISystemTests) },
            { "SaveLoadTests", typeof(SaveLoadTests) },
            { "LocalizationTests", typeof(LocalizationTests) }
        };

        // Run each test category
        foreach (var category in testCategories)
        {
            Debug.Log($"\n=== RUNNING {category.Key} ===");
            List<string> categoryResultsList = new List<string>();
            categoryResults[category.Key] = categoryResultsList;

            RunTestCategory(category.Value, category.Key);
        }

        // Generate and display report
        GenerateHTMLReport();
        DisplayConsoleResults();
    }

    private void RunTestCategory(Type testType, string categoryName)
    {
        var testInstance = Activator.CreateInstance(testType);
        var methods = testType.GetMethods().Where(m =>
            m.GetCustomAttributes(typeof(TestAttribute), false).Length > 0 ||
            m.GetCustomAttributes(typeof(UnityTestAttribute), false).Length > 0);

        foreach (var method in methods)
        {
            var testResult = new TestResult
            {
                testName = $"{categoryName}.{method.Name}",
                category = categoryName,
                passed = true,
                message = "",
                duration = "0ms",
                screenshotPath = ""
            };

            try
            {
                Debug.Log($"Running test: {testResult.testName}");

                // Start timer
                var startTime = DateTime.Now;

                // Run test
                method.Invoke(testInstance, null);

                // End timer
                var duration = DateTime.Now - startTime;
                testResult.duration = duration.TotalMilliseconds.ToString("F2") + "ms";

                Debug.Log($"✓ PASS: {testResult.testName} ({testResult.duration})");
                categoryResults[categoryName].Add($"✓ PASS: {testResult.testName} ({testResult.duration})");

            }
            catch (Exception ex)
            {
                // Test failed
                testResult.passed = false;
                testResult.message = ex.Message;
                testResult.duration = "0ms";

                // Take screenshot on failure
                string screenshotFile = $"{SCREENSHOT_PATH}/{categoryName}_{method.Name}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
                ScreenCapture.CaptureScreenshot(screenshotFile);
                testResult.screenshotPath = screenshotFile;

                Debug.LogError($"✗ FAIL: {testResult.testName}");
                Debug.LogError($"  Error: {testResult.message}");
                Debug.LogError($"  Screenshot saved to: {screenshotFile}");

                categoryResults[categoryName].Add($"✗ FAIL: {testResult.testName}");
                categoryResults[categoryName].Add($"  Error: {testResult.message}");
                categoryResults[categoryName].Add($"  Screenshot: {screenshotFile}");
            }

            allResults.Add(testResult);
        }
    }

    private void GenerateHTMLReport()
    {
        var report = new TestReport
        {
            testDate = DateTime.Now,
            totalTests = allResults.Count,
            passedTests = allResults.Count(r => r.passed),
            failedTests = allResults.Count(r => !r.passed),
            totalDuration = allResults.Sum(r => double.Parse(r.duration.Replace("ms", ""))) / 1000f,
            testResults = allResults
        };

        var htmlBuilder = new StringBuilder();
        htmlBuilder.AppendLine("<!DOCTYPE html>");
        htmlBuilder.AppendLine("<html>");
        htmlBuilder.AppendLine("<head>");
        htmlBuilder.AppendLine("<title>Test Report - Cristo Adventure</title>");
        htmlBuilder.AppendLine("<style>");
        htmlBuilder.AppendLine("body { font-family: Arial, sans-serif; margin: 20px; }");
        htmlBuilder.AppendLine(".header { background: #2c3e50; color: white; padding: 20px; border-radius: 5px; }");
        htmlBuilder.AppendLine(".summary { display: flex; gap: 20px; margin: 20px 0; }");
        htmlBuilder.AppendLine(".summary-item { background: #ecf0f1; padding: 15px; border-radius: 5px; flex: 1; text-align: center; }");
        htmlBuilder.AppendLine(".pass { color: #27ae60; }");
        htmlBuilder.AppendLine(".fail { color: #e74c3c; }");
        htmlBuilder.AppendLine(".test-item { border: 1px solid #bdc3c7; padding: 10px; margin: 10px 0; border-radius: 5px; }");
        htmlBuilder.AppendLine(".screenshot { margin-top: 10px; }");
        htmlBuilder.AppendLine(".screenshot img { max-width: 200px; border: 1px solid #bdc3c7; }");
        htmlBuilder.AppendLine("</style>");
        htmlBuilder.AppendLine("</head>");
        htmlBuilder.AppendLine("<body>");

        // Header
        htmlBuilder.AppendLine("<div class='header'>");
        htmlBuilder.AppendLine("<h1>Cristo Adventure - Test Report</h1>");
        htmlBuilder.AppendLine($"<p>Test Date: {report.testDate:yyyy-MM-dd HH:mm:ss}</p>");
        htmlBuilder.AppendLine("</div>");

        // Summary
        htmlBuilder.AppendLine("<div class='summary'>");
        htmlBuilder.AppendLine($"<div class='summary-item'><h3>Total Tests</h3><p>{report.totalTests}</p></div>");
        htmlBuilder.AppendLine($"<div class='summary-item pass'><h3>Passed</h3><p>{report.passedTests}</p></div>");
        htmlBuilder.AppendLine($"<div class='summary-item fail'><h3>Failed</h3><p>{report.failedTests}</p></div>");
        htmlBuilder.AppendLine($"<div class='summary-item'><h3>Total Duration</h3><p>{report.totalDuration:F2}s</p></div>");
        htmlBuilder.AppendLine("</div>");

        // Test Results by Category
        var categories = allResults.GroupBy(r => r.category).ToList();
        foreach (var category in categories)
        {
            htmlBuilder.AppendLine($"<h2>Category: {category.Key}</h2>");
            htmlBuilder.AppendLine("<ul>");

            foreach (var test in category)
            {
                htmlBuilder.AppendLine($"<li class='test-item {(test.passed ? "pass" : "fail")}'>");
                htmlBuilder.AppendLine($"<strong>{test.testName}</strong> - {test.duration}");
                if (!test.passed)
                {
                    htmlBuilder.AppendLine($"<br><strong>Error:</strong> {test.message}");
                    if (!string.IsNullOrEmpty(test.screenshotPath))
                    {
                        htmlBuilder.AppendLine("<div class='screenshot'>");
                        htmlBuilder.AppendLine($"<img src='{test.screenshotPath}' alt='Test Screenshot'>");
                        htmlBuilder.AppendLine($"<p>Screenshot: {test.screenshotPath}</p>");
                        htmlBuilder.AppendLine("</div>");
                    }
                }
                htmlBuilder.AppendLine("</li>");
            }

            htmlBuilder.AppendLine("</ul>");
        }

        htmlBuilder.AppendLine("</body>");
        htmlBuilder.AppendLine("</html>");

        // Save report
        File.WriteAllText(REPORT_PATH, htmlBuilder.ToString());
        Debug.Log($"HTML Report saved to: {REPORT_PATH}");
    }

    private void DisplayConsoleResults()
    {
        Debug.Log("\n=== TEST RESULTS SUMMARY ===");
        Debug.Log($"Total Tests: {allResults.Count}");
        Debug.Log($"Passed: {allResults.Count(r => r.passed)}");
        Debug.Log($"Failed: {allResults.Count(r => !r.passed)}");

        foreach (var category in categoryResults)
        {
            Debug.Log($"\n--- {category.Key} ---");
            foreach (var result in category.Value)
            {
                Debug.Log(result);
            }
        }

        // Save detailed log
        string logPath = "TestResults_Log.txt";
        File.WriteAllText(logPath, string.Join("\n", categoryResults.Values.SelectMany(r => r)));
        Debug.Log($"Detailed log saved to: {logPath}");
    }
}

// Add to Test Runner
public class TestRunner
{
    [Test]
    public void RunTestSuite()
    {
        var reporter = new TestReporter();
        reporter.RunAllTests();
    }
}