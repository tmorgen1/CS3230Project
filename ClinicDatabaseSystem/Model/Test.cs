namespace ClinicDatabaseSystem.Model
{
    /// <summary>
    /// Holds information regarding the types of tests and their names.
    /// </summary>
    public class Test
    {
        /// <summary>
        /// Gets or sets the test identifier.
        /// </summary>
        /// <value>
        /// The test identifier.
        /// </value>
        public int TestId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Test"/> class.
        /// </summary>
        /// <param name="testId">The test identifier.</param>
        /// <param name="name">The name.</param>
        public Test(int testId, string name)
        {
            this.TestId = testId;
            this.Name = name;
        }
    }
}
