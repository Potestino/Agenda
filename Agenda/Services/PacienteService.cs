using Agenda.Middleware;
using Agenda.Models;
using Agenda.Repository.Interfaces;
using Agenda.Services.Interfaces;

namespace Agenda.Services
{
    public class PacienteService : IPacienteService
    {

        private readonly IPacienteRepository _pacienteRepository;
        private readonly IMedicoService _medicoService;

        public PacienteService(IPacienteRepository pacienteRepository, IMedicoService medicoService)
        {
            _pacienteRepository = pacienteRepository;
            _medicoService = medicoService;
        }

        public IEnumerable<Consulta> GetConsultas(int pacienteId)
        {
            Paciente paciente = _pacienteRepository.GetPaciente(pacienteId);
            if (paciente == null)
                throw new NotFoundException($"Paciente com ID {pacienteId} não encontrado");

            return _pacienteRepository.GetConsultas(paciente);
        }

        public IEnumerable<ConsultaPaciente> GetAgendaMedico(int medicoId)
        {
            var consultas = _medicoService.GetConsultas(medicoId);

            if (consultas == null)
                throw new NotFoundException($"Não foram encontradas consulta para o medico com ID {medicoId}");

            var consultaPaciente = consultas.Select(c => new ConsultaPaciente
            {
                Id = c.Id,
                DataHora = c.DataHora,
                Medico = c.Medico
            });

            return consultaPaciente;
        }
    }
}
