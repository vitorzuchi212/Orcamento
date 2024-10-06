using Microsoft.EntityFrameworkCore;

namespace Orcamento.Models
{
    public class OrcamentoGeral
    {
        public Guid Id { get; set; }
        public List<OrcamentoP> Gastos;
        public List<OrcamentoRec> Receitas;

        }
}
