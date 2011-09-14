using System;
using System.Linq;
using SimpleSpeedTester.Core;

namespace PerfTest
{
    class Program
    {
        private const int IterationCount = 10000;
        private const int TestRuns = 100;

        static void Main(string[] args)
        {
            Action doNothing = () => { };
            Action throwEx = () => { throw new Exception(); };

            var testGroup = new TestGroup("MyTestGroup");

            var noTryCatchResult = testGroup.PlanAndExecute(
                "NoTryCatch", () => NoTryCatch(doNothing, IterationCount), TestRuns);
            var tryCatchNoExceptionResult = testGroup.PlanAndExecute(
                "TryCatchNoException", () => WithTryCatch(doNothing, IterationCount), TestRuns);
            var tryCatchWithExceptionResult = testGroup.PlanAndExecute(
                "TryCatchWithException", () => WithTryCatch(throwEx, IterationCount), TestRuns);

            Console.WriteLine(noTryCatchResult);
            Console.WriteLine(tryCatchNoExceptionResult);
            Console.WriteLine(tryCatchWithExceptionResult);

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
    }
}