using Agenda.Models;
using Agenda.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Agenda.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PacienteController : ControllerBase
    {

        private readonly IPacienteService _pacienteService;
        private readonly IMedicoService _medicoService;

        public PacienteController(IPacienteService pacienteService, IMedicoService medicoService)
        {
            _pacienteService = pacienteService;
            _medicoService = medicoService;
        }

        /// <summary>
        /// Obtém todas as consultas de um paciente específico.
        /// </summary>
        /// <param name="id">O ID do paciente.</param>
        /// <returns>Lista de consultas do paciente.</returns>
        /// <response code="200">Retorna as consultas do paciente.</response>
        /// <response code="404">Se o paciente não for encontrado.</response>
        [HttpGet("{id}/consultas")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<Consulta>> GetConsultas(int id)
        {
            var consultas = _pacienteService.GetConsultas(id);
            return Ok(consultas);
        }

        /// <summary>
        /// Obter a agenda de um médico
        /// </summary>
        /// <param name="id">O ID do médico.</param>
        /// <returns>Lista de consultas do médico.</returns>
        /// <response code="200">Retorna as consultas do médico.</response>
        /// <response code="404">Se o médico não for encontrado.</response>
        [HttpGet("medicos/{medicoId}/consultas")]
        public ActionResult<IEnumerable<ConsultaPaciente>> GetAgendaMedico(int medicoId)
        {
            var consultas = _pacienteService.GetAgendaMedico(medicoId);          

            return Ok(consultas);
        }

        /// <summary>
        /// Marca uma nova consulta para um médico específico.
        /// </summary>
        /// <param name="id">O ID do médico.</param>
        /// <param name="pacienteId">O ID do paciente.</param>
        /// <param name="dataHora">A data e hora da consulta.</param>
        /// <returns>A consulta marcada.</returns>
        /// <response code="201">Retorna a consulta marcada.</response>
        /// <response code="400">Se a consulta não puder ser marcada.</response>
        /// <response code="404">Se o médico ou paciente não forem encontrados.</response>
        /// <response code="409">Se a consulta tiver conflito de horario.</response>
        [HttpPost("{id}/medicos/{medicoId}/consultas")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult<Consulta> MarcarConsulta(int id, int medicoId, DateTime dataHora)
        {
            Consulta consulta = _medicoService.MarcarConsulta(medicoId, id, dataHora);
            if (consulta == null)
                return BadRequest("Não é possível marcar consulta com o mesmo médico no mesmo horário.");

            return CreatedAtAction(nameof(GetConsultas), new { id = id, consultaId = consulta.Id }, consulta);
        }
    }
}
