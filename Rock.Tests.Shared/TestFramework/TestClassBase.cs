namespace Rock.Tests.Shared
{
    /// <summary>
    /// Provides base functionality for a class that defines tests for a Rock project.
    /// </summary>
    public class TestClassBase
    {
        private RockAssert _RockAssert = new RockAssert();

        /// <summary>
        /// Gets an instance of the Assert class that provides access to the assertions used for testing.
        /// </summary>
        /// <remarks>
        /// This property provides a semantic replacement for references to the standard MSTest Assert static object.
        /// It has the advantage of allowing the use of extension methods without the "Assert.This" syntax required by the standard implementation,
        /// and can therefore be more easily used as a drop-in replacement for tests ported from other testing frameworks.
        /// </remarks>
        public RockAssert Assert
        {
            get
            {
                return _RockAssert;
            }
        }
    }
}
