using Agenda.Models;

namespace Agenda.Services.Interfaces
{
    public interface IMedicoService
    {
        public IEnumerable<Consulta> GetConsultas(int medicoId);
        public Consulta MarcarConsulta(int medicoId, int pacienteId, DateTime dataHora);
    }
}
