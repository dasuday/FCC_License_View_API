using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace FCCapi
{
    class Program
    {

        static void Main(string[] args)
        {
            APIthread a1 = new APIthread(10103, 11000, 1);
            APIthread a2 = new APIthread(11104, 12000, 2);
            APIthread a3 = new APIthread(12102, 13000, 3);
            APIthread a4 = new APIthread(13104, 14000, 4);
            APIthread a5 = new APIthread(14104, 15000, 5);

            APIthread a6 = new APIthread(20103, 21000, 6);
            APIthread a7 = new APIthread(21102, 22000, 7);
            APIthread a8 = new APIthread(22102, 23000, 8);
            APIthread a9 = new APIthread(23103, 24000, 9);
            APIthread a10 = new APIthread(24102, 25000, 10);

            Thread t1 = new Thread(new ThreadStart(a1.callAPI));
            Thread t2 = new Thread(new ThreadStart(a2.callAPI));
            Thread t3 = new Thread(new ThreadStart(a3.callAPI));
            Thread t4 = new Thread(new ThreadStart(a4.callAPI));
            Thread t5 = new Thread(new ThreadStart(a5.callAPI));

            Thread t6 = new Thread(new ThreadStart(a6.callAPI));
            Thread t7 = new Thread(new ThreadStart(a7.callAPI));
            Thread t8 = new Thread(new ThreadStart(a8.callAPI));
            Thread t9 = new Thread(new ThreadStart(a9.callAPI));
            Thread t10 = new Thread(new ThreadStart(a10.callAPI));

            t1.Start();
            t2.Start();
            t3.Start();
            t4.Start();
            t5.Start();

            t6.Start();
            t7.Start();
            t8.Start();
            t9.Start();
            t10.Start();

            //Thread t2 = new Thread(method1);
        }

        /*public static void method1()
        {
            Console.WriteLine("Hello");
        }*/
    }

    class APIthread
    {
        private long lowNum;
        private long highNum;
        private int threadID;

        public APIthread(long lNum, long hNum, int tid)
        {
            this.lowNum = lNum;
            this.highNum = hNum;
            this.threadID = tid;
        }

        public void callAPI()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            HttpClient httpClient = new HttpClient();
            for (long i = lowNum; i < highNum; i++)
            {
                System.IO.StreamWriter sw = File.CreateText(@"C:\Users\crazy\OneDrive\Workspace-VS\FCCapi\Results\FRNsearch\foundFRNs\" + "foundFRNs-" + i + ".txt");
                System.IO.StreamWriter fa = File.CreateText(@"C:\Users\crazy\OneDrive\Workspace-VS\FCCapi\Results\FRNsearch\failedSearches\" + "failedSearches-" + i + ".txt");
                long intFRN = i * 100;
                while (intFRN < i * 100 + 100)
                {
                    //int intFRN = 116;
                    string searchFRN = intFRN.ToString();
                    searchFRN = searchFRN.PadLeft(10, '0');
                    Console.WriteLine(threadID + ": " + searchFRN);
                        //string searchFRN = "0001688472";
                    while (true)
                    {
                        int attempts = 0;
                        var httpResponse = httpClient.GetAsync("https://data.fcc.gov/api/license-view/basicSearch/getLicenses?searchValue=" + searchFRN).Result;
                        if (httpResponse.IsSuccessStatusCode)
                        {
                            //Console.WriteLine(httpResponse)
                            if (!httpResponse.ToString().Contains("209"))
                            {
                                string apiResult = httpResponse.Content.ReadAsStringAsync().Result;
                                //Console.WriteLine(apiResult);
                                Console.WriteLine(threadID + ": FRN Exists: " + searchFRN + "    ...saving XML response.");
                                sw.WriteLine(searchFRN);
                                System.IO.File.WriteAllText(@"C:\Users\crazy\OneDrive\Workspace-VS\FCCapi\Results\FRNsearch\FRNresponses\response_" + searchFRN + ".xml", apiResult);
                            }
                            break;
                        }
                        if (attempts >= 9)
                        {
                            fa.WriteLine(searchFRN);
                            break;
                        }
                        attempts++;
                    }
                    intFRN++;
                }
                sw.Close();
                Console.WriteLine(threadID + ": " + $"Execution Time: {watch.Elapsed}");
                fa.WriteLine(threadID + ": " + $"Execution Time: {watch.Elapsed}");
                fa.Close();
            }
            Console.WriteLine(threadID + ": " + $"Execution Time: {watch.Elapsed}");
        }
    }
}



