using Agenda.Middleware;
using Agenda.Models;
using Agenda.Repository.Interfaces;
using Agenda.Services;
using Moq;

namespace AgendaTest.Services
{
    public class MedicoServiceTest
    {
        private readonly Mock<IMedicoRepository> _medicoRepositoryMock;
        private readonly Mock<IPacienteRepository> _pacienteRepositoryMock;
        private readonly MedicoService _medicoService;

        public MedicoServiceTest()
        {
            _medicoRepositoryMock = new Mock<IMedicoRepository>();
            _pacienteRepositoryMock = new Mock<IPacienteRepository>();
            _medicoService = new MedicoService(_medicoRepositoryMock.Object, _pacienteRepositoryMock.Object);
        }

        [Fact]
        public void GetConsultas_WithExistingMedico_ReturnsConsultas()
        {
            // Arrange
            var medicoId = 1;
            var medico = new Medico { Id = medicoId };
            var consultas = new List<Consulta> { new Consulta { Id = 1, DataHora = DateTime.Now } };
            _medicoRepositoryMock.Setup(m => m.GetMedico(medicoId)).Returns(medico);
            _medicoRepositoryMock.Setup(m => m.GetConsultas(medico)).Returns(consultas);

            // Act
            var result = _medicoService.GetConsultas(medicoId);

            // Assert
            Assert.Equal(consultas, result);
        }

        [Fact]
        public void MarcarConsulta_WithValidData_ReturnsConsulta()
        {
            // Arrange
            var medicoId = 1;
            var pacienteId = 1;
            var dataHora = DateTime.Now;
            var medico = new Medico { Id = medicoId };
            var paciente = new Paciente { Id = pacienteId };
            var consulta = new Consulta { Id = 1, Medico = medico, Paciente = paciente, DataHora = dataHora };
            _medicoRepositoryMock.Setup(m => m.GetMedico(medicoId)).Returns(medico);
            _pacienteRepositoryMock.Setup(p => p.GetPaciente(pacienteId)).Returns(paciente);
            _medicoRepositoryMock.Setup(m => m.MarcarConsulta(medico, paciente, dataHora)).Returns(consulta);

            // Act
            var result = _medicoService.MarcarConsulta(medicoId, pacienteId, dataHora);

            // Assert
            Assert.Equal(consulta, result);
        }

        [Fact]
        public void MarcarConsulta_WithConflictData_ThrowsConflictException()
        {
            // Arrange
            var medicoId = 1;
            var pacienteId = 1;
            var dataHora = DateTime.Now;
            var medico = new Medico { Id = medicoId };
            var paciente = new Paciente { Id = pacienteId };
            var consultaExistente = new Consulta { Id = 1, Medico = medico, Paciente = paciente, DataHora = dataHora };
            _medicoRepositoryMock.Setup(m => m.GetMedico(medicoId)).Returns(medico);
            _pacienteRepositoryMock.Setup(p => p.GetPaciente(pacienteId)).Returns(paciente);
            _medicoRepositoryMock.Setup(m => m.GetConsultas(medico)).Returns(new List<Consulta> { consultaExistente });
            _pacienteRepositoryMock.Setup(p => p.GetConsultas(paciente)).Returns(new List<Consulta> { consultaExistente });

            // Act & Assert
            Assert.Throws<ConflictException>(() => _medicoService.MarcarConsulta(medicoId, pacienteId, dataHora));
        }

        [Fact]
        public void GetConsultas_InvalidMedicoId_ThrowsNotFoundException()
        {
            // Arrange
            int medicoId = 1;
            Medico medico = null;

            var medicoRepositoryMock = new Mock<IMedicoRepository>();
            medicoRepositoryMock.Setup(repo => repo.GetMedico(medicoId)).Returns(medico);

            // Act & Assert
            Assert.Throws<NotFoundException>(() => _medicoService.GetConsultas(medicoId));
        }
        [Fact]
        public void MarcarConsulta_InvalidMedicoId_ThrowsNotFoundException()
        {
            // Arrange
            int medicoId = 1;
            int pacienteId = 1;
            DateTime dataHora = DateTime.Now;

            // Act & Assert
            Assert.Throws<NotFoundException>(() => _medicoService.MarcarConsulta(medicoId, pacienteId, dataHora));
        }

        [Fact]
        public void MarcarConsulta_InvalidPacienteId_ThrowsNotFoundException()
        {
            // Arrange
            int medicoId = 1;
            int pacienteId = 1;
            DateTime dataHora = DateTime.Now;
            var medico = new Medico { Id = medicoId };
            _medicoRepositoryMock.Setup(m => m.GetMedico(medicoId)).Returns(medico);

            // Act & Assert
            Assert.Throws<NotFoundException>(() => _medicoService.MarcarConsulta(medicoId, pacienteId, dataHora));
        }

        [Fact]
        public void MarcarConsulta_ConsultaExistenteMedico_ThrowsConflictException()
        {
            // Arrange
            int medicoId = 1;
            int pacienteId = 1;
            DateTime dataHora = DateTime.Now;

            var medico = new Medico { Id = medicoId };
            var paciente = new Paciente { Id = pacienteId };
            var consultaExistente = new Consulta { Id = 1, Medico = medico, Paciente = paciente, DataHora = dataHora };
            _medicoRepositoryMock.Setup(m => m.GetMedico(medicoId)).Returns(medico);
            _pacienteRepositoryMock.Setup(p => p.GetPaciente(pacienteId)).Returns(paciente);
            _medicoRepositoryMock.Setup(m => m.GetConsultas(medico)).Returns(new List<Consulta> { consultaExistente });

            // Act & Assert
            Assert.Throws<ConflictException>(() => _medicoService.MarcarConsulta(medicoId, pacienteId, dataHora));
        }

        [Fact]
        public void MarcarConsulta_ConsultaExistentePaciente_ThrowsConflictException()
        {
            // Arrange
            int medicoId = 1;
            int pacienteId = 1;
            DateTime dataHora = DateTime.Now;
            var medico = new Medico { Id = medicoId };
            var paciente = new Paciente { Id = pacienteId };
            var consultaExistente = new Consulta { Id = 1, Medico = medico, Paciente = paciente, DataHora = dataHora };
            _medicoRepositoryMock.Setup(m => m.GetMedico(medicoId)).Returns(medico);
            _pacienteRepositoryMock.Setup(p => p.GetPaciente(pacienteId)).Returns(paciente);
            _pacienteRepositoryMock.Setup(p => p.GetConsultas(paciente)).Returns(new List<Consulta> { consultaExistente });

            // Act & Assert
            Assert.Throws<ConflictException>(() => _medicoService.MarcarConsulta(medicoId, pacienteId, dataHora));
        }
    }
}
