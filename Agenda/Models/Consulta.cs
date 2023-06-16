namespace Agenda.Models
{
    public class Consulta
    {
        public int Id { get; set; }
        public DateTime DataHora { get; set; }
        public Medico Medico { get; set; }
        public Paciente Paciente { get; set; }
    }
}
