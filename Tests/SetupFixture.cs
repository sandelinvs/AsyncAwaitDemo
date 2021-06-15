using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class SetupFixture
    {
        [SetUp]
        public virtual void BeforeEach()
        {
            TestContext.WriteLine("BeforeEach");
        }

        [TearDown]
        public virtual void AfterEach()
        {
            TestContext.WriteLine("AfterEach");
        }

        [OneTimeSetUp]
        public virtual void BeforeAll()
        {
            TestContext.WriteLine("BeforeAll");
        }

        [OneTimeTearDown]
        public virtual void AfterAll()
        {
            TestContext.WriteLine("AfterAll");
        }
    }
}