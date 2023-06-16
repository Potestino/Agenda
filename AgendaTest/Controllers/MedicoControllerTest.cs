using Agenda.Controllers;
using Agenda.Models;
using Agenda.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AgendaTest.Controllers
{
    public class MedicoControllerTest
    {
        private readonly Mock<IMedicoService> _medicoServiceMock;
        private readonly MedicoController _medicoController;

        public MedicoControllerTest()
        {
            _medicoServiceMock = new Mock<IMedicoService>();
            _medicoController = new MedicoController(_medicoServiceMock.Object);
        }

        [Fact]
        public void GetConsultas_WithExistingMedico_ReturnsOkResult()
        {
            // Arrange
            var medicoId = 1;
            var consultas = new List<Consulta> { new Consulta { Id = 1, DataHora = DateTime.Now } };
            _medicoServiceMock.Setup(m => m.GetConsultas(medicoId)).Returns(consultas);

            // Act
            var okResult = _medicoController.GetConsultas(medicoId);

            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
            var resultValue = (okResult.Result as OkObjectResult).Value as IEnumerable<Consulta>;
            Assert.Equal(consultas, resultValue);
        }

        [Fact]
        public void GetConsultas_WithNonExistingMedico_ReturnsNotFoundResult()
        {
            // Arrange
            var medicoId = 1;
            _medicoServiceMock.Setup(m => m.GetConsultas(medicoId)).Returns((IEnumerable<Consulta>)null);

            // Act
            var notFoundResult = _medicoController.GetConsultas(medicoId);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult.Result);
        }

        [Fact]
        public void MarcarConsulta_WithValidData_ReturnsCreatedResult()
        {
            // Arrange
            var medicoId = 1;
            var pacienteId = 1;
            var dataHora = DateTime.Now;
            var consulta = new Consulta { Id = 1, Medico = new Medico { Id = medicoId }, Paciente = new Paciente { Id = pacienteId }, DataHora = dataHora };
            _medicoServiceMock.Setup(m => m.MarcarConsulta(medicoId, pacienteId, dataHora)).Returns(consulta);

            // Act
            var createdResult = _medicoController.MarcarConsulta(medicoId, pacienteId, dataHora);

            // Assert
            Assert.IsType<CreatedAtActionResult>(createdResult.Result);
            var resultValue = (createdResult.Result as CreatedAtActionResult).Value as Consulta;
            Assert.Equal(consulta, resultValue);
        }

        [Fact]
        public void MarcarConsulta_WithInvalidData_ReturnsBadRequestResult()
        {
            // Arrange
            var medicoId = 1;
            var pacienteId = 1;
            var dataHora = DateTime.Now;
            _medicoServiceMock.Setup(m => m.MarcarConsulta(medicoId, pacienteId, dataHora)).Returns((Consulta)null);

            // Act
            var badRequestResult = _medicoController.MarcarConsulta(medicoId, pacienteId, dataHora);

            // Assert
            Assert.IsType<BadRequestObjectResult>(badRequestResult.Result);
        }
    }
}
