using Agenda.Models;
using Agenda.Repository.Context;
using Microsoft.EntityFrameworkCore;

namespace AgendaTest.Context
{
    public class TempContextTests
    {
        [Fact]
        public void Medicos_SetAndGet_ReturnsExpectedValue()
        {
            // Arrange
            var expectedMedicos = new[]
            {
            new Medico { Id = 1, Nome = "Medico 1" },
            new Medico { Id = 2, Nome = "Medico 2" },
            new Medico { Id = 3, Nome = "Medico 3" }
        };
            var context = new TempContext(CreateInMemoryDatabaseOptions());

            // Act
            context.Medicos.AddRange(expectedMedicos);
            context.SaveChanges();
            var medicos = context.Medicos.ToArray();

            // Assert
            Assert.Equal(expectedMedicos, medicos);
        }

        [Fact]
        public void Consultas_SetAndGet_ReturnsExpectedValue()
        {
            // Arrange
            var expectedConsultas = new[]
            {
            new Consulta { Id = 1 },
            new Consulta { Id = 2 },
            new Consulta { Id = 3 }
        };
            var context = new TempContext(CreateInMemoryDatabaseOptions());

            // Act
            context.Consultas.AddRange(expectedConsultas);
            context.SaveChanges();
            var consultas = context.Consultas.ToArray();

            // Assert
            Assert.Equal(expectedConsultas, consultas);
        }

        [Fact]
        public void Pacientes_SetAndGet_ReturnsExpectedValue()
        {
            // Arrange
            var expectedPacientes = new[]
            {
            new Paciente { Id = 2, Nome = "Paciente 2", Email = "teste2@teste.com", Endereco = "Teste endereço1" },
            new Paciente { Id = 3, Nome = "Paciente 3", Email = "teste3@teste.com", Endereco = "Teste endereço2" },
            new Paciente { Id = 1, Nome = "Paciente 1", Email = "teste1@teste.com", Endereco = "Teste endereço3" },
            };
            var context = new TempContext(CreateInMemoryDatabaseOptions());

            // Act
            context.Pacientes.AddRange(expectedPacientes);
            context.SaveChanges();
            var pacientes = context.Pacientes.ToArray();

            // Assert
            Assert.Equal(expectedPacientes, pacientes);
        }

        private DbContextOptions<TempContext> CreateInMemoryDatabaseOptions()
        {
            return new DbContextOptionsBuilder<TempContext>()
                .UseInMemoryDatabase(databaseName: "temp_test_db")
                .Options;
        }
    }
}
