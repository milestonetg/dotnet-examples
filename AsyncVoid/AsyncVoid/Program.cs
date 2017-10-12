// 2017 Milestone Technology Group, LLC., MIT License
// This code is intended for demonstration purposes only and should not be considered production-ready.
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AsyncVoid
{
    // Async/Await - Best Practices in Asynchronous Programming, By Stephen Cleary
    // https://msdn.microsoft.com/en-us/magazine/jj991977.aspx

    // SO Post
    // https://stackoverflow.com/questions/9343594/how-to-call-asynchronous-method-from-synchronous-method-in-c

    class Program
    {
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += (s,e) =>
            {
                Exception ex = e.ExceptionObject as Exception;
                Console.WriteLine($"{DateTime.Now.ToLongTimeString()} Unhandled: {ex.Message}. Terminating Process: {e.IsTerminating}");
            };

            try
            {
                Console.WriteLine($"{DateTime.Now.ToLongTimeString()} Main: Calling Foo");
                Foo(5000);
                Console.WriteLine($"{DateTime.Now.ToLongTimeString()} Main: returned from Foo");

                Console.WriteLine($"{DateTime.Now.ToLongTimeString()} Main: Calling Bar");
                Bar(3000);
                Console.WriteLine($"{DateTime.Now.ToLongTimeString()} Main: returned from Bar");
                
            }
            catch(Exception ex)
            {
                Console.WriteLine($"{DateTime.Now.ToLongTimeString()} Main: {ex.Message}");
            }
            Console.WriteLine($"{DateTime.Now.ToLongTimeString()} Done!");
            Console.ReadLine();
        }

        static async void Foo(int work)
        {
            try
            {
                await BarAsync(work);
                Console.WriteLine($"{DateTime.Now.ToLongTimeString()} Foo more work");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Foo: {ex.Message}");
            }
        }

        static async Task BarAsync(int work, [CallerMemberName] string caller = null)
        {
            await Task.Delay(work);
            Console.WriteLine($"{DateTime.Now.ToLongTimeString()} BarAsync: Did work");
            //throw new ApplicationException($"BarAsync failed for {caller}");
        }

        static void Bar(int work)
        {
            // BarAsync(work)
            //     .ConfigureAwait(false)
            //     .GetAwaiter()
            //     .GetResult();

            Task.Run(()=>BarAsync(work))
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();
        }
    }
}
