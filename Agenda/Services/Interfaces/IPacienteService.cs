using Agenda.Models;

namespace Agenda.Services.Interfaces
{
    public interface IPacienteService
    {
        IEnumerable<ConsultaPaciente> GetAgendaMedico(int medicoId);
        IEnumerable<Consulta> GetConsultas(int pacienteId);
    }
}
