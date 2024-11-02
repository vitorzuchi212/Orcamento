using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing.Printing;
using System;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Orcamento.InfraStructure.Data.Context;
using Orcamento.Models;
using Org.BouncyCastle.Ocsp;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Org.BouncyCastle.Asn1.Pkcs;

namespace Orcamento.Controllers
{
    public class OrcamentoRecController : Controller
    {
        private readonly ApplicationDataContext _context;

        public OrcamentoRecController(ApplicationDataContext context)
        {
            _context = context;
        }



        public async Task<IActionResult> DownloadRelatorio(Guid id, OrcamentoRec orcamentoR)
        {

            var receita = _context.OrcamentoRec.ToList();


            var orderedOrcamentosRec = receita.OrderByDescending(o => o.ActionCreateR).ToList();


            var orcamentos = new OrcamentoRec
            {
                Receita = orderedOrcamentosRec

            };
            if (orcamentos == null)
            {
                return NotFound();

            }



            using (var memoryStream = new MemoryStream())
            {
                using (var doc = new Document(iTextSharp.text.PageSize.A4))
                {
                    iTextSharp.text.pdf.PdfWriter.GetInstance(doc, memoryStream);

            
                    doc.Open();

                    doc.Add(new iTextSharp.text.Paragraph("Relatório Mensal de Orçamento"));
                    doc.Add(new iTextSharp.text.Paragraph(" "));

                   
                    var tabela = new PdfPTable(5);

                 
                    tabela.AddCell("Descrição");
                    tabela.AddCell("Valor");
                    tabela.AddCell("Data");
                    tabela.AddCell("Tipo de Operação");
                    tabela.AddCell("Forma de Pagamento");
                    tabela.AddCell(" ");
                    tabela.AddCell(" ");
                    tabela.AddCell(" ");
                    tabela.AddCell(" ");
                    tabela.AddCell(" ");

                    for (int i = 0; i < orderedOrcamentosRec.Count; i++)
                    {
                        tabela.AddCell(orderedOrcamentosRec[i].DescriptionR);
                        tabela.AddCell(orderedOrcamentosRec[i].ValueR.ToString("F2"));
                        tabela.AddCell(orderedOrcamentosRec[i].ActionCreateR.ToString("dd/MM/yyyy"));
                        tabela.AddCell(orderedOrcamentosRec[i].TipoOperacao);

                       
                        if (i == orderedOrcamentosRec.Count - 1)
                        {
                            var totalValue = orderedOrcamentosRec.Sum(item => item.ValueR);
                            tabela.AddCell(" ");
                            tabela.AddCell(" ");
                            tabela.AddCell(" ");
                            tabela.AddCell(" ");
                            tabela.AddCell(" ");
                            tabela.AddCell("Total no fim do mês");
                            tabela.AddCell(totalValue.ToString("F2"));
                      
                            tabela.AddCell(""); 
                            tabela.AddCell(""); 
                            tabela.AddCell(""); 
                        }
                    }


                    doc.Add(tabela);

                    doc.Close();
                }

                var pdfBytes = memoryStream.ToArray();
                return File(pdfBytes, "application/pdf", "RelatorioMensal.pdf");
            }
        }




        public async Task<IActionResult> DashboardRec(string trimestre, int? ano)
        {
            var orcamentos = _context.OrcamentoRec.AsQueryable();

            if (ano.HasValue)
            {
                orcamentos = orcamentos.Where(o => o.ActionCreateR.Year == ano.Value);
            }

            if (!string.IsNullOrEmpty(trimestre))
            {
                int startMonth = trimestre switch
                {
                    "Trimestre1" => 1,
                    "Trimestre2" => 4,
                    "Trimestre3" => 7,
                    "Trimestre4" => 10,
                    _ => 1
                };
                int endMonth = startMonth + 2;

                orcamentos = orcamentos.Where(o => o.ActionCreateR.Month >= startMonth && o.ActionCreateR.Month <= endMonth);
            }

            var orderedOrcamentos = await orcamentos.OrderByDescending(o => o.ActionCreateR).ToListAsync();

            ViewBag.Anos = _context.OrcamentoRec.Select(o => o.ActionCreateR.Year).Distinct().OrderByDescending(y => y).ToList();

            return View(orderedOrcamentos);
        }

        public async Task<IActionResult> Pesquisa(string txt_pesq)
        {
            if (!string.IsNullOrEmpty(txt_pesq))
            {
                var pesquisa = string.IsNullOrWhiteSpace(txt_pesq)
                ? new List<OrcamentoRec>()
                : _context.OrcamentoRec
                    .Where(c => c.DescriptionR.Contains(txt_pesq))
                    .OrderByDescending(c => c.ActionCreateR)
                    .ToList();

                ViewData["txt_pesq"] = txt_pesq; 
                return View(pesquisa); 
            }
            else
            {

                return RedirectToAction(nameof(Index));
            }
        }


        // GET: OrcamentoPs
        public async Task<IActionResult> Index(string trimestre, int? ano)
        {
            var orcamentos = _context.OrcamentoRec.AsQueryable();

            if (ano.HasValue)
            {
                orcamentos = orcamentos.Where(o => o.ActionCreateR.Year == ano.Value);
            }

            if (!string.IsNullOrEmpty(trimestre))
            {
                int startMonth = trimestre switch
                {
                    "Trimestre1" => 1,
                    "Trimestre2" => 4,
                    "Trimestre3" => 7,
                    "Trimestre4" => 10,
                    _ => 1
                };
                int endMonth = startMonth + 2;

                orcamentos = orcamentos.Where(o => o.ActionCreateR.Month >= startMonth && o.ActionCreateR.Month <= endMonth);


            }
  

            var orderedOrcamentos = orcamentos.OrderByDescending(o => o.ActionCreateR).ToList();
           
            ViewBag.Anos = _context.OrcamentoRec.Select(o => o.ActionCreateR.Year).Distinct().OrderByDescending(y => y).ToList();

            return View(orderedOrcamentos);
        }


        // GET: OrcamentoPs/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orcamentoRec = await _context.OrcamentoRec.FirstOrDefaultAsync(m => m.Id == id);
            if (orcamentoRec == null)
            {
                return NotFound();
            }

            return View(orcamentoRec);
        }

        // GET: OrcamentoPs/Create
        public IActionResult Create_()
        {
            return View();
        }

        // POST: OrcamentoPs/Create
        [HttpPost]

        public async Task<IActionResult> Create(OrcamentoRec orcamentoR)
        {

            if (orcamentoR.TipoOperacao == "Receita")
            {
                orcamentoR.Id = Guid.NewGuid();
                _context.Add(orcamentoR);
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "OrcamentoRec");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Ocorreu um erro ao salvar a despesa: " + ex.Message);
                }
            }
            else if (orcamentoR.TipoOperacao == "Despesa")
            {
                var orcamentoP = new OrcamentoP
                {
                    Id = Guid.NewGuid(),
                    Description = orcamentoR.DescriptionR,
                    Value = orcamentoR.ValueR,
                    ActionCreate = orcamentoR.ActionCreateR,
                    TipoOperacao = orcamentoR.TipoOperacao
                };

                _context.Add(orcamentoR);
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index__G", "OrcamentoPs");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Ocorreu um erro ao salvar a receita: " + ex.Message);
                }
            }

            return View(orcamentoR);
        }

        [HttpPost]
        public async Task<IActionResult> Edit__G(Guid id, OrcamentoRec orcamentoR)
        {
            if (id != orcamentoR.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orcamentoR);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrcamentoPExists(orcamentoR.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(orcamentoR);
        }

        // GET: OrcamentoRec/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orcamentoR = await _context.OrcamentoRec
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orcamentoR == null)
            {
                return NotFound();
            }

            return View(orcamentoR);
        }

        // POST: OrcamentoRec/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> Delete__GConfirmed(Guid id)
        {
            var orcamentoR = await _context.OrcamentoRec.FindAsync(id);
            if (orcamentoR != null)
            {
                _context.OrcamentoRec.Remove(orcamentoR);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrcamentoPExists(Guid id)
        {
            return _context.OrcamentoRec.Any(e => e.Id == id);
        }
    }
}
