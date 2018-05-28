using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace TestClient
{
    class Program
    {
        private static long[] timeForRequests;
        private static int requestCount;
        private static int taskNum;
        private static string href;
        private static long globalAvg;

        private static readonly ManualResetEvent resetEvent = new ManualResetEvent(false);

        static void Main(string[] args)
        {
            Console.WriteLine("Input href:");
            href = Console.ReadLine();
            Console.WriteLine("Input request count:");
            requestCount = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Input number of tasks:");
            taskNum = Convert.ToInt32(Console.ReadLine());

            Task.Run(async () =>
            {
                var httpClient = new HttpClient();
                for (int i = 0; i < 10; i++)
                {
                    await httpClient.GetAsync(href);
                }

                resetEvent.Set();
            });
            resetEvent.WaitOne();

            timeForRequests = new long[taskNum];
            for (int i = 0;  i < taskNum; ++i)
            {
                int index = i;
                Task.Run(async () => { await SendRequests(index); });
            }

            Console.WriteLine("Put any string to calculate global time:");
            Console.ReadLine();

            for (int i = 0; i < taskNum; i++)
            {
                Console.WriteLine("Time for requests: " + timeForRequests[i] + "ms");
            }

            Console.WriteLine("Requests query avg: " + (timeForRequests.Sum() / (double)timeForRequests.Length));
            Console.WriteLine("Global avg: " + (globalAvg / (double)(taskNum * requestCount)) + "ms");
            Console.WriteLine("Global time: " + timeForRequests.Sum() + "ms");

            Console.ReadLine();
        }

        private static async Task SendRequests(int index)
        {
            var httpClient = new HttpClient();
            var timeForRequestsBuf = DateTime.Now;
            for (int j = 0; j < requestCount; j++)
            {
                var timeForRequest = DateTime.Now;
                var response = await httpClient.GetAsync(href);
                long currTime = (long)(DateTime.Now - timeForRequest).TotalMilliseconds;
                globalAvg += currTime;
                //string content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(index + ") " + currTime + "ms" + " sum: " + globalAvg + "ms");
            }

            timeForRequests[index] = (long)(DateTime.Now - timeForRequestsBuf).TotalMilliseconds;
            Console.WriteLine("Time for requests[" + index + "] = " + timeForRequests[index]);
        }
    }
}
