using Agenda.Models;

namespace Agenda.Repository.Interfaces
{
    public interface IPacienteRepository
    {
        public IEnumerable<Consulta> GetConsultas(Paciente paciente);
        public Paciente GetPaciente(int pacienteId);
    }
}