using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public abstract class SetupFixture
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
            TestContext.WriteLine("BeforeAll"); // does not print to console but does run.
        }

        [OneTimeTearDown]
        public virtual void AfterAll()
        {
            TestContext.WriteLine("AfterAll"); // same as above
        }
    }
}