using Agenda.Models;
using Agenda.Repository.Context;
using Agenda.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Agenda.Repository
{
    public class PacienteRepository : IPacienteRepository
    {
        private readonly ITempContext _context;
       
        public PacienteRepository(ITempContext context)
        {
            _context = context;

            
            if (_context.Pacientes.Count() == 0)//apenas para o exemplo por que não existe cadastro de pacientes no flow
            {
                _context.Pacientes.Add(new Paciente
                {
                    Nome = "João",
                    Endereco = "Rua da Alegria, 123",
                    Email = "joao@gmail.com"
                });

                _context.Pacientes.Add(new Paciente
                {
                    Nome = "Maria",
                    Endereco = "Avenida do Sol, 456",
                    Email = "maria@gmail.com"
                });

                _context.Pacientes.Add(new Paciente
                {
                    Nome = "Carlos",
                    Endereco = "Praça da Estrela, 789",
                    Email = "carlos@gmail.com"
                });

                _context.SaveChanges();
            }
        }

        public Paciente GetPaciente(int pacienteId)
        {
            return _context.Pacientes.FirstOrDefault(p => p.Id == pacienteId);
        }

        public IEnumerable<Consulta> GetConsultas(Paciente paciente)
        {
            return _context.Consultas.Include(c => c.Medico).Where(c => c.Paciente.Id == paciente.Id).ToList();
        }
    }
}
