using Agenda.Models;
using Microsoft.EntityFrameworkCore;

namespace Agenda.Repository.Context
{
    public class TempContext : DbContext, ITempContext
    {
        public TempContext(DbContextOptions<TempContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Medico> Medicos { get; set; }
        public virtual DbSet<Consulta> Consultas { get; set; }
        public virtual DbSet<Paciente> Pacientes { get; set; }
    }
}
