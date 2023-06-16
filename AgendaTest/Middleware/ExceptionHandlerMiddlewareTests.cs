using System.Net;
using Agenda.Middleware;
using Agenda.Models;
using Microsoft.AspNetCore.Http;
using Moq;
using Newtonsoft.Json;

namespace AgendaTests.Middleware
{
    public class ExceptionHandlerMiddlewareTests
    {
        private HttpContext _httpContext;

        public ExceptionHandlerMiddlewareTests()
        {
            _httpContext = new DefaultHttpContext();
            _httpContext.Response.Body = new MemoryStream();
        }
        [Fact]
        public async Task InvokeAsync_OK_ReturnsOKResponse()
        {
            // Arrange
            var middleware = new ExceptionHandlerMiddleware(Ok);

            // Act
            await middleware.InvokeAsync(_httpContext);

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, _httpContext.Response.StatusCode);
        }

        [Fact]
        public async Task InvokeAsync_NotFoundException_ReturnsNotFoundResponse()
        {
            // Arrange
            var middleware = new ExceptionHandlerMiddleware(NotFoundException);

            // Act
            await middleware.InvokeAsync(_httpContext);

            // Assert
            Assert.Equal((int)HttpStatusCode.NotFound, _httpContext.Response.StatusCode);
            Assert.Equal("application/json", _httpContext.Response.ContentType);

            var responseString = await GetResponseBodyAsString(_httpContext.Response.Body);
            var errorDetails = JsonConvert.DeserializeObject<ErrorDetails>(responseString);

            Assert.Equal("O recurso solicitado não foi encontrado.", errorDetails.Message);
            Assert.Equal((int)HttpStatusCode.NotFound, errorDetails.StatusCode);
        }

        [Fact]
        public async Task InvokeAsync_BadRequestException_ReturnsBadRequestResponse()
        {
            // Arrange
            var middleware = new ExceptionHandlerMiddleware(BadRequestException);

            // Act
            await middleware.InvokeAsync(_httpContext);

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, _httpContext.Response.StatusCode);
            Assert.Equal("application/json", _httpContext.Response.ContentType);

            var responseString = await GetResponseBodyAsString(_httpContext.Response.Body);
            var errorDetails = JsonConvert.DeserializeObject<ErrorDetails>(responseString);

            Assert.Equal("A solicitação foi malformada ou inválida.", errorDetails.Message);
            Assert.Equal((int)HttpStatusCode.BadRequest, errorDetails.StatusCode);
        }

        [Fact]
        public async Task InvokeAsync_UnauthorizedException_ReturnsUnauthorizedResponse()
        {
            // Arrange
            var middleware = new ExceptionHandlerMiddleware(UnauthorizedException);

            // Act
            await middleware.InvokeAsync(_httpContext);

            // Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, _httpContext.Response.StatusCode);
            Assert.Equal("application/json", _httpContext.Response.ContentType);

            var responseString = await GetResponseBodyAsString(_httpContext.Response.Body);
            var errorDetails = JsonConvert.DeserializeObject<ErrorDetails>(responseString);

            Assert.Equal("Você não tem autorização para realizar essa operação.", errorDetails.Message);
            Assert.Equal((int)HttpStatusCode.Unauthorized, errorDetails.StatusCode);
        }


        [Fact]
        public async Task InvokeAsync_ConflictException_ReturnsConflictResponse()
        {
            // Arrange
            var middleware = new ExceptionHandlerMiddleware(ConflictException);

            // Act
            await middleware.InvokeAsync(_httpContext);

            // Assert
            Assert.Equal((int)HttpStatusCode.Conflict, _httpContext.Response.StatusCode);
            Assert.Equal("application/json", _httpContext.Response.ContentType);

            var responseString = await GetResponseBodyAsString(_httpContext.Response.Body);
            var errorDetails = JsonConvert.DeserializeObject<ErrorDetails>(responseString);

            Assert.Equal("Um conflito foi indentificado.", errorDetails.Message);
            Assert.Equal((int)HttpStatusCode.Conflict, errorDetails.StatusCode);
        }

        [Fact]
        public async Task InvokeAsync_InternalServerErrorException_ReturnsInternalServerErrorResponse()
        {
            // Arrange
            var middleware = new ExceptionHandlerMiddleware(Exception);

            // Act
            await middleware.InvokeAsync(_httpContext);

            // Assert
            Assert.Equal((int)HttpStatusCode.InternalServerError, _httpContext.Response.StatusCode);
            Assert.Equal("application/json", _httpContext.Response.ContentType);

            var responseString = await GetResponseBodyAsString(_httpContext.Response.Body);
            var errorDetails = JsonConvert.DeserializeObject<ErrorDetails>(responseString);

            Assert.Equal("Ocorreu um erro interno no servidor.", errorDetails.Message);
            Assert.Equal((int)HttpStatusCode.InternalServerError, errorDetails.StatusCode);
        }
        private Task Ok(HttpContext context)
        {
            return Task.CompletedTask;
        }
        private Task NotFoundException(HttpContext context)
        {
            throw new NotFoundException("NotFoundException");
        }
        private Task BadRequestException(HttpContext context)
        {
            throw new BadRequestException("BadRequest");
        }       
        private Task UnauthorizedException(HttpContext context)
        {
            throw new UnauthorizedException("Unauthorized");
        }
        private Task ConflictException(HttpContext context)
        {
            throw new ConflictException("Conflict");
        }
        private Task Exception(HttpContext context)
        {
            throw new Exception("Exception");
        }

        private async Task<string> GetResponseBodyAsString(Stream bodyStream)
        {
            bodyStream.Seek(0, SeekOrigin.Begin);
            using (var reader = new StreamReader(bodyStream))
            {
                return await reader.ReadToEndAsync();
            }
        }

    }
}
