using System;
using System.Runtime.Loader;
using System.Threading;

namespace sigterm
{
	public static class Program
	{
		private static ManualResetEvent _shutdownEvent = new ManualResetEvent(false);
		private static ManualResetEventSlim _completeEvent = new ManualResetEventSlim();

		public static int Main(string[] args)
		{
			try
			{
				// Capture SIGTERM
				AssemblyLoadContext.Default.Unloading += OnAssemblyLoadContextUnloading;

				Console.WriteLine("Application running...");

				// << Do some stuff here >>

				// Wait for a signal
				_shutdownEvent.WaitOne();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			finally
			{
				Console.WriteLine("Cleaning up resources");
			}

			Console.WriteLine("Exiting...");

			_completeEvent.Set();

			return 0;
		}

		private static void OnAssemblyLoadContextUnloading(AssemblyLoadContext obj)
		{
			Console.Write($"Shutting down in response to SIGTERM.");

			_shutdownEvent.Set();

			_completeEvent.Wait();
		}
	}
}
