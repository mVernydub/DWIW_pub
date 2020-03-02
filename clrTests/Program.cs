using System;
using System.Threading;
using System.Reflection;
using System.Runtime.Remoting;

namespace clrTests
{
    class Example: MarshalByRefObject {
        public static int StaticVal = 5;
        private int val2;
        public int Value {get;set;}

        public int defineVa2l{set {val2 = value;}}

        public int IncPrivate() {
            val2++;
            return val2;
        }

        public void LoopInf() {
            int counter = 0;
            while (counter<10) {
                counter++;
                Value = counter;
                defineVa2l = 10* counter;
                Thread.Sleep(2000);
                Console.WriteLine($"Value = {Value}; StaticVal = {Example.StaticVal}");
            }
        }
    }
    
    class Program
    {
        private static void Marshalling() {
            AppDomain adCallingThreadDomain = Thread.GetDomain();

            String callingDomainName = adCallingThreadDomain.FriendlyName;
            Console.WriteLine(
                "Default AppDomain's friendly name={0}", callingDomainName);

            String exeAssembly = Assembly.GetEntryAssembly().FullName;
            Console.WriteLine("Main assembly={0}", exeAssembly);

            AppDomain ad2 = AppDomain.CreateDomain("AD #2");
           
            Example exampleType = null;

            exampleType = (Example)ad2.CreateInstanceAndUnwrap(exeAssembly, "Example");

            Console.WriteLine("Type={0}", exampleType.GetType());

            // Console.WriteLine("Is proxy={0}", RemotingServices.IsTransparentProxy(exampleType));
            exampleType.IncPrivate();

            AppDomain.Unload(ad2);
        }

        static void Main(string[] args)
        {
            var obj = new Example();
            Console.WriteLine("Started");
            Marshalling();
            obj.LoopInf();
            Marshalling();

        }
    }
}
