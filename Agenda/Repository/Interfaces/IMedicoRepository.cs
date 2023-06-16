using Agenda.Models;

namespace Agenda.Repository.Interfaces
{
    public interface IMedicoRepository
    {
        public IEnumerable<Consulta> GetConsultas(Medico medico);
        public Medico GetMedico(int medicoId);
        public Consulta MarcarConsulta(Medico medico, Paciente paciente, DateTime dataHora);
    }
}