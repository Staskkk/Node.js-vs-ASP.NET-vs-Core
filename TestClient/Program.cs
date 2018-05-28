using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace TestClient
{
    class Program
    {
        private const int waitDelay = 30000;

        private static volatile int requestCount;
        private static volatile int taskNum;
        private static volatile string href;

        private static long globalSuccessCount;
        private static double globalSum;
        private static double globalMax;
        private static double globalMin;

        private static double allTestsGlobalAvg = 0;
        private static double allTestsGlobalTime = 0;
        private static double allTestsMin = 0;
        private static double allTestsMax = 0;
        private static long allTestsSuccessCount = 0;
        private static long allTestsFailedCount = 0;
        private static long allTestsCount = 0;

        private static readonly object globalLockObject = new object();

        private static readonly ManualResetEvent initResetEvent = new ManualResetEvent(false);

        private static readonly ManualResetEvent globalResetEvent = new ManualResetEvent(false);

        private static readonly List<Task> workingTasks = new List<Task>();

        static void Main(string[] args)
        {
            Console.WriteLine("Input url:");
            href = Console.ReadLine();
            Console.WriteLine("Input request count:");
            requestCount = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Input number of tasks:");
            taskNum = Convert.ToInt32(Console.ReadLine());

            long globalRequestsCount = requestCount * taskNum;

            Task.Run(async () =>
            {
                var httpClient = new HttpClient();
                for (int i = 0; i < 10; i++)
                {
                    await httpClient.GetAsync(href);
                }

                initResetEvent.Set();
            });
            initResetEvent.WaitOne();

            do
            {
                initResetEvent.Reset();
                globalResetEvent.Reset();
                workingTasks.Clear();
                globalSuccessCount = 0;
                globalSum = 0;
                globalMax = 0;
                globalMin = double.MaxValue;

                for (int i = 0; i < taskNum; ++i)
                {
                    workingTasks.Add(Task.Run(SendRequests));
                }

                Task.Run(async () =>
                {
                    await Task.WhenAll(workingTasks);
                    globalResetEvent.Set();
                });

                Console.WriteLine("Wait...");
                Console.WriteLine();

                globalResetEvent.WaitOne();

                double globalAvg = globalSum / globalSuccessCount;
                long globalFailedCount = globalRequestsCount - globalSuccessCount;

                allTestsCount++;

                Console.WriteLine("Test " + allTestsCount + ":");
                Console.WriteLine("Success count: " + globalSuccessCount + "; " + 
                    (globalSuccessCount / (double)globalRequestsCount * 100) + "%");
                Console.WriteLine("Failed count: " + globalFailedCount + "; " + 
                    (globalFailedCount / (double)globalRequestsCount * 100) + "%");
                Console.WriteLine("Global min request time: " + globalMin + " ms");
                Console.WriteLine("Global max request time: " + globalMax + " ms");
                Console.WriteLine("Global avg: " + globalAvg + " ms");
                Console.WriteLine("Global time: " + globalSum + " ms");

                allTestsGlobalAvg += globalAvg;
                allTestsGlobalTime += globalSum;
                allTestsMin += globalMin;
                allTestsMax += globalMax;
                allTestsSuccessCount += globalSuccessCount;
                allTestsFailedCount += globalFailedCount;
            } while (Console.ReadLine() != "exit");

            Console.WriteLine();
            Console.WriteLine("Tests count: " + allTestsCount);
            long allTestsRequestsCount = allTestsSuccessCount + allTestsFailedCount;
            Console.WriteLine("Average success count: " + (allTestsSuccessCount / (double)allTestsCount) + "; " + 
                (allTestsSuccessCount / (double)allTestsRequestsCount * 100) + "%");
            Console.WriteLine("Average failed count: " + (allTestsFailedCount / (double)allTestsCount) + "; " +
                (allTestsFailedCount / (double)allTestsRequestsCount * 100) + "%");
            Console.WriteLine("Average min request time: " + (allTestsMin / allTestsCount) + " ms");
            Console.WriteLine("Average max request time: " + (allTestsMax / allTestsCount) + " ms");
            Console.WriteLine("Average global avg: " + (allTestsGlobalAvg / allTestsCount) + " ms");
            Console.WriteLine("Average global time: " + (allTestsGlobalTime / allTestsCount) + " ms");

            Console.ReadLine();
        }

        private static async Task SendRequests()
        {
            var httpClient = new HttpClient();
            for (int j = 0; j < requestCount; j++)
            {
                try
                {
                    var requestCancelTokenSource = new CancellationTokenSource();
                    Task requestTask = httpClient.GetAsync(href, requestCancelTokenSource.Token);
                    var timeForRequest = DateTime.Now;
                    if (await Task.WhenAny(requestTask, Task.Delay(waitDelay)) == requestTask)
                    {
                        double currTime = (DateTime.Now - timeForRequest).TotalMilliseconds;
                        lock (globalLockObject)
                        {
                            globalSum += currTime;
                            globalSuccessCount++;
                            if (globalMin > currTime)
                            {
                                globalMin = currTime;
                            }

                            if (globalMax < currTime)
                            {
                                globalMax = currTime;
                            }
                        }
                    }
                    else
                    {
                        requestCancelTokenSource.Cancel();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                }
            }
        }
    }
}
