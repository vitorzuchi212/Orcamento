using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace Orcamento.Models
{
    public class OrcamentoRec
    {
        public Guid Id { get; set; }
        public string DescriptionR { get; set; } = string.Empty;
        public double ValueR { get; set; }
        public DateTime ActionCreateR { get; set; }
    }
}
