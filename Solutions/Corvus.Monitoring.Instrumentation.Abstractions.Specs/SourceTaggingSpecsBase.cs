
namespace Corvus.Monitoring.Instrumentation.Abstractions.Specs
{
    using NUnit.Framework;

    /// <summary>
    /// Base class of tests for source tagging instrumentation interfaces.
    /// </summary>
    public abstract class SourceTaggingSpecsBase
    {
        /// <summary>
        /// Gets the <see cref="SourceTaggingTestContext"/>.
        /// </summary>
        private protected SourceTaggingTestContext Context { get; private set; }

        /// <summary>
        /// Creates the <see cref="SourceTaggingTestContext"/>.
        /// </summary>
        /// <remarks>
        /// Tests for source tagging need the source tagging implementations to have been
        /// registered in DI in the normal way, and we also need implementations of the
        /// non-generic interfaces that enable us to test whether the calls were forwarded and,
        /// where required, augmented.
        /// </remarks>
        [SetUp]
        public void Setup()
        {
            this.Context = new SourceTaggingTestContext();
        }

        /// <summary>
        /// Stops the activity created for the test.
        /// </summary>
        [TearDown]
        public void Teardown()
        {
            this.Context.Dispose();
        }

        public class TestType1
        {
        }

        public class TestType2
        {
        }
    }
}