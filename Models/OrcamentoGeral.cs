using Microsoft.EntityFrameworkCore;

namespace Orcamento.Models
{
    public class OrcamentoGeral
    {
   
        public Guid Id { get; set; }
        public List<OrcamentoP> Gastos { get; set; }
        public List<OrcamentoRec> Receitas { get; set; }



    }




}

