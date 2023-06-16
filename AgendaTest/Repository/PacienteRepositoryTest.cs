using Agenda.Models;
using Agenda.Repository;
using Agenda.Repository.Context;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace AgendaTest.Repository
{
    public class PacienteRepositoryTest
    {
        private readonly PacienteRepository _pacienteRepository;
        private readonly Mock<ITempContext> _mockContext;
        private readonly Mock<DbSet<Paciente>> _mockPacientes;
        private readonly Mock<DbSet<Consulta>> _mockConsultas;
        private readonly List<Consulta> _consultas;
        private readonly IQueryable<Paciente> _pacientes;

        public PacienteRepositoryTest()
        {
            _pacientes = new List<Paciente>
            {
                new Paciente { Id = 1, Nome = "João", Endereco = "Rua da Alegria, 123", Email = "joao@gmail.com" },
                new Paciente { Id = 2, Nome = "Maria", Endereco = "Avenida do Sol, 456", Email = "maria@gmail.com" },
                new Paciente { Id = 3, Nome = "Carlos", Endereco = "Praça da Estrela, 789", Email = "carlos@gmail.com" },
            }.AsQueryable();

            _mockPacientes = new Mock<DbSet<Paciente>>();
            _mockPacientes.As<IQueryable<Paciente>>().Setup(m => m.Provider).Returns(_pacientes.Provider);
            _mockPacientes.As<IQueryable<Paciente>>().Setup(m => m.Expression).Returns(_pacientes.Expression);
            _mockPacientes.As<IQueryable<Paciente>>().Setup(m => m.ElementType).Returns(_pacientes.ElementType);
            _mockPacientes.As<IQueryable<Paciente>>().Setup(m => m.GetEnumerator()).Returns(() => _pacientes.GetEnumerator());

            _consultas = new List<Consulta>();
            _mockConsultas = new Mock<DbSet<Consulta>>();
            _mockConsultas.As<IQueryable<Consulta>>().Setup(m => m.Provider).Returns(_consultas.AsQueryable().Provider);
            _mockConsultas.As<IQueryable<Consulta>>().Setup(m => m.Expression).Returns(_consultas.AsQueryable().Expression);
            _mockConsultas.As<IQueryable<Consulta>>().Setup(m => m.ElementType).Returns(_consultas.AsQueryable().ElementType);
            _mockConsultas.As<IQueryable<Consulta>>().Setup(m => m.GetEnumerator()).Returns(() => _consultas.GetEnumerator());

            _mockContext = new Mock<ITempContext>();
            _mockContext.Setup(m => m.Pacientes).Returns(_mockPacientes.Object);
            _mockContext.Setup(m => m.Consultas).Returns(_mockConsultas.Object);
            _mockContext.Setup(m => m.SaveChanges()).Callback(() => _consultas.AddRange(_consultas));

            _pacienteRepository = new PacienteRepository(_mockContext.Object);
        }

        [Fact]
        public void GetPaciente_WithExistingId_ReturnsPaciente()
        {
            // Act
            var result = _pacienteRepository.GetPaciente(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public void GetConsultas_WithExistingPaciente_ReturnsConsultas()
        {
            // Arrange
            var paciente = _pacientes.First();
            var medico = new Medico { Id = 1, Nome = "Dr. João" };
            _consultas.Add(new Consulta { Id = 1, Paciente = paciente, Medico = medico, DataHora = DateTime.Now });

            // Act
            var result = _pacienteRepository.GetConsultas(paciente);

            // Assert
            Assert.NotEmpty(result);
            Assert.All(result, c => Assert.Equal(paciente.Id, c.Paciente.Id));
        }
    }
}
