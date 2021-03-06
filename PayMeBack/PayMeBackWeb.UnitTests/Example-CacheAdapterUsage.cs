﻿#region Using Statements
using System;
using Glav.CacheAdapter.Core.DependencyInjection;

#endregion

namespace Glav.CacheAdapter.ExampleUsage
{
	/// <summary>
	/// This class is simply a very basic demonstration of the use of the CacheAdapter
	/// in an application. This class is not required in any way so feel free to delete or modify
	/// this class as you see fit.
	/// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            ExampleAddAndRetrieveFromCache();

            Console.WriteLine("Done.");
            Console.ReadLine();

        }

        private static void ExampleAddAndRetrieveFromCache()
        {
        	var cacheProvider = AppServices.Cache;

            // First try and get some data. It wont be in the cache, so the anonymous function is executed,
            // the item is automatically added to the cache and returned.

            Console.WriteLine("Getting Some Data.");
            var data1 = cacheProvider.Get<SomeData>("cache-key", DateTime.Now.AddSeconds(3), () =>
            {
                // This is the anonymous function which gets called if the data is not in the cache.
                // This method is executed and whatever is returned, is added to the cache with the
                // passed in expiry time.
                Console.WriteLine("... => Adding data to the cache... 1st call");
                var someData = new SomeData() { SomeText = "cache example1", SomeNumber = 1 };
                return someData;
            });
            Console.WriteLine("... => SomeData values: SomeText=[{0}], SomeNumber={1}\n", data1.SomeText, data1.SomeNumber);

            // Now try and get some data using the same cache-key and before the cached data expiry time elapses.
            // The data will be located in the cache and returned, and the anonymous function is NOT executed.

            Console.WriteLine("Getting Some More Data which should now be cached.");
            var data2 = cacheProvider.Get<SomeData>("cache-key", DateTime.Now.AddSeconds(5), () =>
            {
                // This is the anonymous function which gets called if the data is not in the cache.
                // This method is executed and whatever is returned, is added to the cache with the
                // passed in expiry time.
                Console.WriteLine("... => Adding data to the cache...2nd call");
                var someData = new SomeData() { SomeText = "cache example2", SomeNumber = 2 };
                return someData;
            });
            Console.WriteLine("... => SomeData values: SomeText=[{0}], SomeNumber={1}\n", data2.SomeText, data2.SomeNumber);

            // Here we wait a period of time so that the cached data expiry time has passed and the data is
            // removed from the cache. We make the same call to retrieve the data using the same cache key, the data
            // is not found in the cache, so once again the anonymous function is executed, whatever is returned is
            // added to the cache, and then returned to the caller.

            System.Threading.Thread.Sleep(4000);
            Console.WriteLine("Getting Some More Data which should not be cached.");
            var data3 = cacheProvider.Get<SomeData>("cache-key", DateTime.Now.AddSeconds(5), () =>
            {
                // This is the anonymous function which gets called if the data is not in the cache.
                // This method is executed and whatever is returned, is added to the cache with the
                // passed in expiry time.
                Console.WriteLine("... => Adding data to the cache...3rd call");
                var someData = new SomeData() { SomeText = "cache example3", SomeNumber = 3 };
                return someData;
            });
            Console.WriteLine("... => SomeData values: SomeText=[{0}], SomeNumber={1}\n", data3.SomeText, data3.SomeNumber);


        	Func<SomeData> getCacheData = new Func<SomeData>(() =>
        	                                                 	{
																	// This is the anonymous function which gets called if the data is not in the cache.
																	// This method is executed and whatever is returned, is added to the cache with the
																	// passed in expiry time.
																	Console.WriteLine("... => Adding data to the cache...4th call, with generated cache key - should only see this msg twice, NOT 3 times");
																	var someData = new SomeData() { SomeText = "cache example4 - generated cache key", SomeNumber = 4 };
																	return someData;

        	                                                 	});
			// Here we use the really simple API call to get an item from the cache without a cache key specified.
			// The cache key is generated from the function we pass in as the delegate used to retrieve the data
			Console.WriteLine("Getting Some Data which should NOT BE cached.");
			var data4 = cacheProvider.Get<SomeData>(DateTime.Now.AddSeconds(2), getCacheData );

        	System.Threading.Thread.Sleep(1000);
			Console.WriteLine("Getting Some More Data which should BE cached.");
			var data5 = cacheProvider.Get<SomeData>(DateTime.Now.AddSeconds(2), getCacheData );

			System.Threading.Thread.Sleep(1000);
			Console.WriteLine("Getting Some More Data which should NOT be cached.");
			var data6 = cacheProvider.Get<SomeData>(DateTime.Now.AddSeconds(2), getCacheData );
		}

    }

	[Serializable]
	public class SomeData
	{
		public string SomeText { get; set; }
		public int SomeNumber { get; set; }
	}

}
