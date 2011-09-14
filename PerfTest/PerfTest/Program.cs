using System;
using System.Linq;
using SimpleSpeedTester.Core;

namespace PerfTest
{
    class Program
    {
        static void Main(string[] args)
        {
            // uncomment the test you want to run
            //PerformSimpleTest(10000, 100);
            //PerformStackTraceDepthTest(1000, 100);
        }

        /// <summary>
        /// Performs simple test to measure the difference in execution time when:
        /// - executing code in try-catch
        /// - throwing exception in try-catch
        /// compared to just running the code
        /// </summary>
        private static void PerformSimpleTest(int iterationCount, int testRuns)
        {
            Action doNothing = () => { };
            Action throwEx = () => { throw new Exception(); };

            var testGroup = new TestGroup("SimpleTest");

            var noTryCatchResult = testGroup.PlanAndExecute(
                "NoTryCatch", () => NoTryCatch(doNothing, iterationCount), testRuns);
            var tryCatchNoExceptionResult = testGroup.PlanAndExecute(
                "TryCatchNoException", () => WithTryCatch(doNothing, iterationCount), testRuns);
            var tryCatchWithExceptionResult = testGroup.PlanAndExecute(
                "TryCatchWithException", () => WithTryCatch(throwEx, iterationCount), testRuns);

            Console.WriteLine(noTryCatchResult);
            Console.WriteLine(tryCatchNoExceptionResult);
            Console.WriteLine(tryCatchWithExceptionResult);

            Console.WriteLine("All done...");
            Console.ReadKey();
        }

        /// <summary>
        /// Performs test to measure the difference in execution time when:
        /// - stack trace is shallow
        /// - stack trace is deeper (50 levels of recursion)
        /// - stack trace os deeeper (100 levels of recursion)
        /// </summary>
        private static void PerformStackTraceDepthTest(int iterationCount, int testRuns)
        {
            var testGroup = new TestGroup("StackTraceDepthTest");

            var shallowTestResult = testGroup.PlanAndExecute(
                "Shallow (1)", () => TestRecurseThenException(iterationCount, 1), testRuns);
            var deeperTestResult = testGroup.PlanAndExecute(
                "Deeper (50)", () => TestRecurseThenException(iterationCount, 50), testRuns);
            var deepestTestResult = testGroup.PlanAndExecute(
                "Deepest (100)", () => TestRecurseThenException(iterationCount, 100), testRuns);

            Console.WriteLine(shallowTestResult);
            Console.WriteLine(deeperTestResult);
            Console.WriteLine(deepestTestResult);

            Console.WriteLine("All done...");
            Console.ReadKey();
        }

        private static void NoTryCatch(Action action, int iterations)
        {
            foreach (var i in Enumerable.Range(1, iterations))
            {
                action();
            }
        }

        private static void WithTryCatch(Action action, int iterations)
        {
            foreach (var i in Enumerable.Range(1, iterations))
            {
                try
                {
                    action();
                }
                catch (Exception)
                {
                    // ignore exceptions
                }
            }
        }

        private static void TestRecurseThenException(int iterations, int depth)
        {
            foreach (var i in Enumerable.Range(1, iterations))
            {
                try
                {
                    RecurseThenExcept(0, depth);
                }
                catch (Exception)
                {
                    // ignore exceptions
                }
            }
        }

        private static void RecurseThenExcept(int current, int max)
        {
            if (current >= max)
            {
                throw new Exception();
            }

            RecurseThenExcept(current+1, max);            
        }
    }
}