using Agenda.Models;
using Agenda.Repository.Context;
using Agenda.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Agenda.Repository
{
    public class MedicoRepository : IMedicoRepository
    {
        private readonly ITempContext _context;

        public MedicoRepository(ITempContext context)
        {
            _context = context;

            if (!_context.Medicos.Any())//apenas para o exemplo por que não existe cadastro de medicos no flow
            {
                _context.Medicos.Add(new Medico { Id = 1, Nome = "Dr. João" });
                _context.Medicos.Add(new Medico { Id = 2, Nome = "Dra. Maria" });
                _context.Medicos.Add(new Medico { Id = 3, Nome = "Dr. Paulo" });

                _context.SaveChanges();
            }
        }

        public Medico GetMedico(int medicoId)
        {
            return _context.Medicos.FirstOrDefault(m => m.Id == medicoId);
        }

        public IEnumerable<Consulta> GetConsultas(Medico medico)
        {
            return _context.Consultas.Include(c => c.Paciente).Where(c => c.Medico.Id == medico.Id).ToList();
        }

        public Consulta MarcarConsulta(Medico medico,Paciente paciente, DateTime dataHora)
        {
            var consulta = new Consulta { Medico = medico, Paciente = paciente, DataHora = dataHora };
            _context.Consultas.Add(consulta);
            _context.SaveChanges();

            return consulta;
        }
    }
}
