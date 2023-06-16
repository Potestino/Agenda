using Agenda.Models;

namespace AgendaTest.Models
{
    public class ErrorDetailsTests
    {
        [Fact]
        public void ErrorDetails_CreatesObjectWithCorrectProperties()
        {
            // Arrange
            int expectedStatusCode = 404;
            string expectedMessage = "Not Found";
            string expectedDetails = "Details about the error";

            // Act
            ErrorDetails errorDetails = new ErrorDetails
            {
                StatusCode = expectedStatusCode,
                Message = expectedMessage,
                Details = expectedDetails
            };

            // Assert
            Assert.Equal(expectedStatusCode, errorDetails.StatusCode);
            Assert.Equal(expectedMessage, errorDetails.Message);
            Assert.Equal(expectedDetails, errorDetails.Details);
        }
    }


}
