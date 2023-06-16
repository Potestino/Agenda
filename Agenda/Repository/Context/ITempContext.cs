using Agenda.Models;
using Microsoft.EntityFrameworkCore;

namespace Agenda.Repository.Context
{
    public interface ITempContext
    {
        DbSet<Paciente> Pacientes { get; set; }
        DbSet<Medico> Medicos { get; set; }
        DbSet<Consulta> Consultas { get; set; }

        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
