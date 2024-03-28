using DsmSuite.Common.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;

namespace DsmSuite.Analyzer.C4.Test
{
    [TestClass]
    public class SetupAssemblyInitializer
    {
        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            Logger.Init(Assembly.GetExecutingAssembly(), true);
        }
    }
}
