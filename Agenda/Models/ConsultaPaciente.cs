namespace Agenda.Models
{
    public class ConsultaPaciente
    {
        public int Id { get; set; }
        public DateTime DataHora { get; set; }
        public Medico Medico { get; set; }
    }
}
