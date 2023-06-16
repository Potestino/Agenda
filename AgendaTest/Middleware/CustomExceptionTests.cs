using Agenda.Middleware;

namespace AgendaTest.Middleware
{
    public class CustomExceptionTests
    {
        [Fact]
        public void NotFoundException_CreatesExceptionWithCorrectMessage()
        {
            // Arrange
            string expectedMessage = "Test not found exception";

            // Act
            NotFoundException exception = new NotFoundException(expectedMessage);

            // Assert
            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void BadRequestException_CreatesExceptionWithCorrectMessage()
        {
            // Arrange
            string expectedMessage = "Test bad request exception";

            // Act
            BadRequestException exception = new BadRequestException(expectedMessage);

            // Assert
            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void UnauthorizedException_CreatesExceptionWithCorrectMessage()
        {
            // Arrange
            string expectedMessage = "Test unauthorized exception";

            // Act
            UnauthorizedException exception = new UnauthorizedException(expectedMessage);

            // Assert
            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void ConflictException_CreatesExceptionWithCorrectMessage()
        {
            // Arrange
            string expectedMessage = "Test conflict exception";

            // Act
            ConflictException exception = new ConflictException(expectedMessage);

            // Assert
            Assert.Equal(expectedMessage, exception.Message);
        }
    }

}
