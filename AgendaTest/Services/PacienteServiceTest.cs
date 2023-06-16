using Agenda.Middleware;
using Agenda.Models;
using Agenda.Repository.Interfaces;
using Agenda.Services;
using Agenda.Services.Interfaces;
using Moq;

namespace AgendaTest.Services
{
    public class PacienteServiceTest
    {
        private readonly Mock<IPacienteRepository> _pacienteRepositoryMock;
        private readonly Mock<IMedicoService> _medicoServiceMock;
        private readonly PacienteService _pacienteService;

        public PacienteServiceTest()
        {
            _pacienteRepositoryMock = new Mock<IPacienteRepository>();
            _medicoServiceMock = new Mock<IMedicoService>();
            _pacienteService = new PacienteService(_pacienteRepositoryMock.Object, _medicoServiceMock.Object);
        }

        [Fact]
        public void GetConsultas_WithExistingPaciente_ReturnsConsultas()
        {
            // Arrange
            var pacienteId = 1;
            var paciente = new Paciente { Id = pacienteId };
            var consultas = new List<Consulta> { new Consulta { Id = 1, DataHora = DateTime.Parse("2023-06-15T12:30:00") } };
            _pacienteRepositoryMock.Setup(m => m.GetPaciente(pacienteId)).Returns(paciente);
            _pacienteRepositoryMock.Setup(m => m.GetConsultas(paciente)).Returns(consultas);

            // Act
            var result = _pacienteService.GetConsultas(pacienteId);

            // Assert
            Assert.Equal(consultas, result);
        }

        [Fact]
        public void GetAgendaMedico_ExistingMedicoId_ReturnsConsultas()
        {
            // Arrange
            var medicoId = 1;

            var consultas = new List<Consulta>
            {
                new Consulta { Id = 1, DataHora = new DateTime(2023, 6, 15, 12, 30, 0), Medico = new Medico { Id = 1, Nome = "Dr. João" } },
                new Consulta { Id = 2, DataHora = new DateTime(2023, 6, 16, 12, 30, 0), Medico = new Medico { Id = 1, Nome = "Dr. João" } }
            };

            _medicoServiceMock.Setup(m => m.GetConsultas(It.IsAny<int>())).Returns(consultas);

            // Act
            var result = _pacienteService.GetAgendaMedico(medicoId);

            // Assert
            Assert.Equal(consultas.Count, result.Count());
            Assert.All(result, c => Assert.Equal(medicoId, c.Medico.Id));
        }

        [Fact]
        public void GetConsultas_InvalidPacienteId_ThrowsNotFoundException()
        {
            // Arrange
            int pacienteId = 1;

            Paciente paciente = null;

            var pacienteRepositoryMock = new Mock<IPacienteRepository>();
            pacienteRepositoryMock.Setup(repo => repo.GetPaciente(pacienteId)).Returns(paciente);

            // Act & Assert
            Assert.Throws<NotFoundException>(() => _pacienteService.GetConsultas(pacienteId));
        }

        [Fact]
        public void GetAgendaMedico_NonExistingMedicoId_ThrowsNotFoundException()
        {
            // Arrange
            var medicoId = 2;

            _medicoServiceMock.Setup(m => m.GetConsultas(It.IsAny<int>())).Returns((List<Consulta>)null);

            // Act and Assert
            Assert.Throws<NotFoundException>(() => _pacienteService.GetAgendaMedico(medicoId));
        }
    }
}
