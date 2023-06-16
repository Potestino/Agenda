using Agenda.Models;
using Agenda.Repository;
using Agenda.Repository.Context;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace AgendaTest.Repository
{
    public class MedicoRepositoryTest
    {
        private readonly MedicoRepository _medicoRepository;
        private readonly Mock<ITempContext> _mockContext;
        private readonly Mock<DbSet<Medico>> _mockMedicos;
        private readonly Mock<DbSet<Consulta>> _mockConsultas;
        private readonly Mock<DbSet<Paciente>> _mockPacientes;
        private readonly List<Consulta> _consultas = new List<Consulta>();

        public MedicoRepositoryTest()
        {
            _mockMedicos = new Mock<DbSet<Medico>>();
            _mockConsultas = new Mock<DbSet<Consulta>>();
            _mockContext = new Mock<ITempContext>();

            var medicos = new List<Medico>
            {
                new Medico { Id = 1, Nome = "Dr. João" },
                new Medico { Id = 2, Nome = "Dra. Maria" },
                new Medico { Id = 3, Nome = "Dr. Paulo" }
            }.AsQueryable();

            _mockMedicos.As<IQueryable<Medico>>().Setup(m => m.Provider).Returns(medicos.Provider);
            _mockMedicos.As<IQueryable<Medico>>().Setup(m => m.Expression).Returns(medicos.Expression);
            _mockMedicos.As<IQueryable<Medico>>().Setup(m => m.ElementType).Returns(medicos.ElementType);
            _mockMedicos.As<IQueryable<Medico>>().Setup(m => m.GetEnumerator()).Returns(() => medicos.GetEnumerator());

            _mockContext.Setup(m => m.Medicos).Returns(_mockMedicos.Object);


            var pacientes = new List<Paciente>
            {
                new Paciente { Id = 1, Nome = "João", Endereco = "Rua da Alegria, 123", Email = "joao@gmail.com" },
                new Paciente { Id = 2, Nome = "Maria", Endereco = "Avenida do Sol, 456", Email = "maria@gmail.com" },
                new Paciente { Id = 3, Nome = "Carlos", Endereco = "Praça da Estrela, 789", Email = "carlos@gmail.com" },
            }.AsQueryable();

            _mockPacientes = new Mock<DbSet<Paciente>>();
            _mockPacientes.As<IQueryable<Paciente>>().Setup(m => m.Provider).Returns(pacientes.Provider);
            _mockPacientes.As<IQueryable<Paciente>>().Setup(m => m.Expression).Returns(pacientes.Expression);
            _mockPacientes.As<IQueryable<Paciente>>().Setup(m => m.ElementType).Returns(pacientes.ElementType);
            _mockPacientes.As<IQueryable<Paciente>>().Setup(m => m.GetEnumerator()).Returns(() => pacientes.GetEnumerator());

            _mockConsultas.As<IQueryable<Consulta>>().Setup(m => m.Provider).Returns(_consultas.AsQueryable().Provider);
            _mockConsultas.As<IQueryable<Consulta>>().Setup(m => m.Expression).Returns(_consultas.AsQueryable().Expression);
            _mockConsultas.As<IQueryable<Consulta>>().Setup(m => m.ElementType).Returns(_consultas.AsQueryable().ElementType);
            _mockConsultas.As<IQueryable<Consulta>>().Setup(m => m.GetEnumerator()).Returns(() => _consultas.GetEnumerator());

            _mockContext.Setup(m => m.Pacientes).Returns(_mockPacientes.Object);

            _mockContext.Setup(m => m.Consultas).Returns(_mockConsultas.Object);

            _mockContext.Setup(m => m.SaveChanges()).Callback(() => _mockConsultas.Object.ToList().AddRange(_consultas));

            _medicoRepository = new MedicoRepository(_mockContext.Object);
        }

        [Fact]
        public void GetMedico_WithExistingId_ReturnsMedico()
        {
            // Act
            var result = _medicoRepository.GetMedico(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public void GetConsultas_WithExistingMedico_ReturnsConsultas()
        {
            // Arrange
            var medico = _mockMedicos.Object.First();
            var paciente = _mockPacientes.Object.First();
            _consultas.Add(new Consulta { Id = 1, Paciente = paciente, Medico = medico, DataHora = DateTime.Now });

            // Act
            var result = _medicoRepository.GetConsultas(medico);

            // Assert
            Assert.NotEmpty(result);
            Assert.All(result, c => Assert.Equal(medico.Id, c.Medico.Id));
        }

        [Fact]
        public void MarcarConsulta_ValidInput_CreatesConsulta()
        {
            // Arrange
            var medico = _mockMedicos.Object.First();
            var paciente = _mockPacientes.Object.First();
            var dataHora = DateTime.Now;
            // Act
            var result = _medicoRepository.MarcarConsulta(medico, paciente, dataHora);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(medico.Id, result.Medico.Id);
            Assert.Equal(paciente.Id, result.Paciente.Id);
            Assert.Equal(dataHora, result.DataHora);
        }
    }
}
