using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;

/// <summary>
/// Test runner for Cristo Adventure automated test suite
/// This class can be added to the Unity Test Runner to execute all tests
/// </summary>
[TestFixture]
public class RunAllTests : MonoBehaviour
{
    [UnityTest]
    [Order(1)]
    public IEnumerator RunGameplayTests()
    {
        yield return new RunTestSuite<GameplayTests>();
    }

    [UnityTest]
    [Order(2)]
    public IEnumerator RunInteractionTests()
    {
        yield return new RunTestSuite<InteractionTests>();
    }

    [UnityTest]
    [Order(3)]
    public IEnumerator RunPuzzleTests()
    {
        yield return new RunTestSuite<PuzzleTests>();
    }

    [UnityTest]
    [Order(4)]
    public IEnumerator RunUISystemTests()
    {
        yield return new RunTestSuite<UISystemTests>();
    }

    [UnityTest]
    [Order(5)]
    public IEnumerator RunSaveLoadTests()
    {
        yield return new RunTestSuite<SaveLoadTests>();
    }

    [UnityTest]
    [Order(6)]
    public IEnumerator RunLocalizationTests()
    {
        yield return new RunTestSuite<LocalizationTests>();
    }

    [UnityTest]
    [Order(7)]
    public IEnumerator GenerateReport()
    {
        // Generate comprehensive report after all tests
        var reporter = new TestReporter();
        reporter.RunAllTests();
        yield return null;
    }
}

/// <summary>
/// Helper class to run test suites in Unity Test Runner
/// </summary>
public class RunTestSuite<T> where T : new()
{
    public IEnumerator Execute()
    {
        var testSuite = new T();
        var methods = typeof(T).GetMethods();

        foreach (var method in methods)
        {
            // Skip non-test methods
            if (method.GetCustomAttributes(typeof(TestAttribute), false).Length > 0 ||
                method.GetCustomAttributes(typeof(UnityTestAttribute), false).Length > 0)
            {
                yield return ExecuteTest(testSuite, method);
            }
        }
    }

    private IEnumerator ExecuteTest(object testInstance, System.Reflection.MethodInfo method)
    {
        Debug.Log($"Running test: {method.Name}");

        try
        {
            // Start timer
            var startTime = System.DateTime.Now;

            // Execute the test
            if (method.GetCustomAttributes(typeof(UnityTestAttribute), false).Length > 0)
            {
                var enumerator = (IEnumerator)method.Invoke(testInstance, null);
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current;
                }
            }
            else
            {
                method.Invoke(testInstance, null);
            }

            // End timer
            var duration = System.DateTime.Now - startTime;
            Debug.Log($"✓ PASS: {method.Name} ({duration.TotalMilliseconds:F2}ms)");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"✗ FAIL: {method.Name}");
            Debug.LogError($"  Error: {ex.Message}");

            // Take screenshot on failure
            string screenshotPath = $"TestScreenshots/{testInstance.GetType().Name}_{method.Name}_{System.DateTime.Now:yyyyMMdd_HHmmss}.png";
            ScreenCapture.CaptureScreenshot(screenshotPath);
            Debug.LogError($"  Screenshot saved: {screenshotPath}");

            yield return null;
        }
    }
}