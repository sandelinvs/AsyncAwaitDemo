using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public abstract class SetupFixture
    {
        [SetUp]
        public virtual void BeforeEach()
        {
            TestContext.WriteLine("BeforeEach"); // Does not print wihout latest updates from nuget
        }

        [TearDown]
        public virtual void AfterEach()
        {
            TestContext.WriteLine("AfterEach"); // Does not print wihout latest updates from nuget
        }

        [OneTimeSetUp]
        public virtual void BeforeAll()
        {
            TestContext.WriteLine("BeforeAll"); // Is called but does not print to console.
        }

        [OneTimeTearDown]
        public virtual void AfterAll()
        {
            TestContext.WriteLine("AfterAll"); // Is called but does not print to console.
        }
    }
}