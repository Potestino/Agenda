using Agenda.Controllers;
using Agenda.Models;
using Agenda.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AgendaTest.Controllers
{
    public class PacienteControllerTest
    {
        private readonly Mock<IPacienteService> _pacienteServiceMock;
        private readonly Mock<IMedicoService> _medicoServiceMock;
        private readonly PacienteController _pacienteController;

        public PacienteControllerTest()
        {
            _pacienteServiceMock = new Mock<IPacienteService>();
            _medicoServiceMock = new Mock<IMedicoService>();
            _pacienteController = new PacienteController(_pacienteServiceMock.Object, _medicoServiceMock.Object);
        }

        [Fact]
        public void GetConsultas_WithExistingPaciente_ReturnsOkResult()
        {
            // Arrange
            var pacienteId = 1;
            var consultas = new List<Consulta> { new Consulta { Id = 1, DataHora = DateTime.Now } };
            _pacienteServiceMock.Setup(m => m.GetConsultas(pacienteId)).Returns(consultas);

            // Act
            var okResult = _pacienteController.GetConsultas(pacienteId);

            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
            var resultValue = (okResult.Result as OkObjectResult).Value as IEnumerable<Consulta>;
            Assert.Equal(consultas, resultValue);
        }

        [Fact]
        public void GetAgendaMedico_WithExistingMedico_ReturnsOkResult()
        {
            // Arrange
            var medicoId = 1;
            var consultas = new List<ConsultaPaciente> { new ConsultaPaciente { Id = 1, DataHora = DateTime.Now } };
            _pacienteServiceMock.Setup(m => m.GetAgendaMedico(medicoId)).Returns(consultas);

            // Act
            var okResult = _pacienteController.GetAgendaMedico(medicoId);

            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
            var resultValue = (okResult.Result as OkObjectResult).Value as IEnumerable<ConsultaPaciente>;
            Assert.Equal(consultas, resultValue);
        }

        [Fact]
        public void MarcarConsulta_WithValidData_ReturnsCreatedResult()
        {
            // Arrange
            var pacienteId = 1;
            var medicoId = 1;
            var dataHora = DateTime.Now;
            var consulta = new Consulta { Id = 1, Medico = new Medico { Id = medicoId }, Paciente = new Paciente { Id = pacienteId }, DataHora = dataHora };
            _medicoServiceMock.Setup(m => m.MarcarConsulta(medicoId, pacienteId, dataHora)).Returns(consulta);

            // Act
            var createdResult = _pacienteController.MarcarConsulta(pacienteId, medicoId, dataHora);

            // Assert
            Assert.IsType<CreatedAtActionResult>(createdResult.Result);
            var resultValue = (createdResult.Result as CreatedAtActionResult).Value as Consulta;
            Assert.Equal(consulta, resultValue);
        }

        [Fact]
        public void MarcarConsulta_WithInvalidData_ReturnsBadRequestResult()
        {
            // Arrange
            var pacienteId = 1;
            var medicoId = 1;
            var dataHora = DateTime.Now;
            _medicoServiceMock.Setup(m => m.MarcarConsulta(medicoId, pacienteId, dataHora)).Returns((Consulta)null);

            // Act
            var badRequestResult = _pacienteController.MarcarConsulta(pacienteId, medicoId, dataHora);

            // Assert
            Assert.IsType<BadRequestObjectResult>(badRequestResult.Result);
        }
    }
}
