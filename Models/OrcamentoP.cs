using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace Orcamento.Models
{
    public class OrcamentoP
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public double Value { get; set; }
        public DateTime ActionCreate { get; set; }
        public string TipoOperacao { get; set; }
        public string FormaPag { get; set; }
        public List<OrcamentoP> Gastos { get; set; }
     
    }
}
