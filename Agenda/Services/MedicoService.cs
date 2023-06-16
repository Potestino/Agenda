using Agenda.Middleware;
using Agenda.Models;
using Agenda.Repository.Interfaces;
using Agenda.Services.Interfaces;

namespace Agenda.Services
{
    public class MedicoService : IMedicoService
    {

        private readonly IMedicoRepository _medicoRepository;
        private readonly IPacienteRepository _pacienteRepository;

        public MedicoService(IMedicoRepository medicoRepository, IPacienteRepository pacienteRepository)
        {
            _medicoRepository = medicoRepository;
            _pacienteRepository = pacienteRepository;
        }

        public IEnumerable<Consulta> GetConsultas(int medicoId)
        {
            Medico medico = _medicoRepository.GetMedico(medicoId);
            if (medico == null)
                throw new NotFoundException("Médico não encontrado");
            
            return _medicoRepository.GetConsultas(medico);
        }

        public Consulta MarcarConsulta(int medicoId,int pacienteId, DateTime dataHora)
        {
            Medico medico = _medicoRepository.GetMedico(medicoId);

            if (medico == null)
                throw new NotFoundException("Médico não encontrado");

            Paciente paciente = _pacienteRepository.GetPaciente(pacienteId);

            if (paciente == null)
                throw new NotFoundException("Paciente não encontrado");


            var consultaExistenteMedico = _medicoRepository.GetConsultas(medico)
                                               .FirstOrDefault(c => c.Medico.Id == medicoId && c.DataHora == dataHora);

            if (consultaExistenteMedico != null)
                throw new ConflictException("Já existe uma consulta marcada com este médico neste horário.");

            var consultaExistentePaciente = _pacienteRepository.GetConsultas(paciente)
                                                            .FirstOrDefault(c => c.Paciente.Id == pacienteId && c.DataHora == dataHora);

            if (consultaExistentePaciente != null)
                throw new ConflictException("Já existe uma consulta marcada para este paciente neste horário.");

            return _medicoRepository.MarcarConsulta(medico, paciente, dataHora);
        }
    }
}
