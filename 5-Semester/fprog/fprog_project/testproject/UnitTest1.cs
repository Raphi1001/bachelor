namespace testproject
{
    public class WordCountProgramIntegrationTests
    {
        [Fact]
        public void GivenValidDirectoryAndFileExtension()
        {
            // Arrange
            string[] args = { "..\\..\\..\\data", ".lll" };
            var expectedOutput = "test1 -> 1\r\ntest2 -> 2\r\ntest3 -> 3\r\ntest4 -> 4\r\ntest7 -> 7\r\n";

            // Redirect console output to a string
            using StringWriter stringWriter = new();
            Console.SetOut(stringWriter);

            // Act
            Program.Main(args);
            string output = stringWriter.ToString();

            // Assert
            Assert.Equal(expectedOutput, output);
        }

        [Fact]
        public void GivenInvalidDirectoryAndFileExtension()
        {
            // Arrange
            string[] args = Array.Empty<string>();
            var expectedOutput = "Missing command line arguments: directory path and file extension\r\n";

            // Redirect console output to a string
            using StringWriter stringWriter = new();
            Console.SetOut(stringWriter);

            // Act
            Program.Main(args);
            string output = stringWriter.ToString();

            // Assert
            Assert.Equal(expectedOutput, output);
        }
    }
}