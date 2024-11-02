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
    public class OrcamentoGeralController : Controller
    {
        private readonly ApplicationDataContext _context;


        public OrcamentoGeralController(ApplicationDataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> DownloadRelatorio()
        {

            var gastos = _context.OrcamentoP.ToList();
            var receita = _context.OrcamentoRec.ToList();


            var orderedOrcamentosG = gastos.OrderByDescending(o => o.ActionCreate).ToList();
            var orderedOrcamentosR = receita.OrderByDescending(o => o.ActionCreateR).ToList();


            var orcamentos = new OrcamentoP
            {
                Gastos = orderedOrcamentosG

            };
            if (orcamentos == null)
            {
                return NotFound();


            }

            var orcamentosR = new OrcamentoRec
            {
                Receita = orderedOrcamentosR

            };
            if (orcamentosR == null)
            {
                return NotFound();

            }


            using (var memoryStream = new MemoryStream())
            {
                using (var doc = new Document(iTextSharp.text.PageSize.A4))
                {
                    iTextSharp.text.pdf.PdfWriter.GetInstance(doc, memoryStream);

          
                    doc.Open();

               
                    doc.Add(new iTextSharp.text.Paragraph("Relatório Geral "));
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

                    for (int i = 0; i < orderedOrcamentosG.Count; i++)
                    {
                        tabela.AddCell(orderedOrcamentosG[i].Description);
                        tabela.AddCell(orderedOrcamentosG[i].Value.ToString("F2"));
                        tabela.AddCell(orderedOrcamentosG[i].ActionCreate.ToString("dd/MM/yyyy"));
                        tabela.AddCell(orderedOrcamentosG[i].TipoOperacao);
                        tabela.AddCell(orderedOrcamentosG[i].FormaPag);

                       
                        if (i == orderedOrcamentosG.Count - 1)
                        {
                            tabela.AddCell(" ");
                            tabela.AddCell(" ");
                            tabela.AddCell(" ");
                            tabela.AddCell(" ");
                            tabela.AddCell(" ");

                        }
                    }
                

                    for (int i = 0; i < orderedOrcamentosR.Count; i++)
                    {
                        tabela.AddCell(orderedOrcamentosR[i].DescriptionR);
                        tabela.AddCell(orderedOrcamentosR[i].ValueR.ToString("F2"));
                        tabela.AddCell(orderedOrcamentosR[i].ActionCreateR.ToString("dd/MM/yyyy"));
                        tabela.AddCell(orderedOrcamentosR[i].TipoOperacao);
                        tabela.AddCell(" ");

             
                        if (i == orderedOrcamentosR.Count - 1)
                        {
                            var totalValueg = orderedOrcamentosG.Sum(item => item.Value);
                            var totalValueRec = orderedOrcamentosR.Sum(item => item.ValueR);
                            var TotalGeral = totalValueRec - totalValueg;


                            tabela.AddCell(" ");
                            tabela.AddCell(" ");
                            tabela.AddCell(" ");
                            tabela.AddCell(" ");
                            tabela.AddCell(" ");

                            tabela.AddCell("Total do Orçamento");
                            tabela.AddCell(TotalGeral.ToString("F2"));
                            tabela.AddCell(" ");
                            tabela.AddCell(" ");
                            tabela.AddCell(" ");

                        }
                    }
                    var totalValue = orderedOrcamentosG.Sum(item => item.Value);
                    var totalValueR = orderedOrcamentosR.Sum(item => item.ValueR);
                    var totalGeral = totalValueR - totalValue;
                    tabela.AddCell("Total no fim do mês");
                    tabela.AddCell(totalGeral.ToString("F2"));
                 
                    doc.Add(tabela);

                 
                    doc.Close();
                }

               
                var pdfBytes = memoryStream.ToArray();
                return File(pdfBytes, "application/pdf", "RelatorioMensal.pdf");
            }
        }

        // GET: OrcamentoGeral
        public IActionResult DashboardGe()
        {

            var gastos = _context.OrcamentoP.ToList();
            var receita = _context.OrcamentoRec.ToList();

            var orderedOrcamentosG = gastos.OrderByDescending(o => o.ActionCreate).ToList();
            var orderedOrcamentosR = receita.OrderByDescending(o => o.ActionCreateR).ToList();

            var orcamentoGeral = new OrcamentoGeral
            {
                Gastos = orderedOrcamentosG,
                Receitas = orderedOrcamentosR
            };

            return View(orcamentoGeral);
        }

        public IActionResult Index()
        {
      
            var gastos = _context.OrcamentoP.ToList();
            var receita = _context.OrcamentoRec.ToList();

            var orderedOrcamentosG = gastos.OrderByDescending(o => o.ActionCreate).ToList();
            var orderedOrcamentosR = receita.OrderByDescending(o => o.ActionCreateR).ToList();

            var orcamentoGeral = new OrcamentoGeral
            {
                Gastos = orderedOrcamentosG,
                Receitas = orderedOrcamentosR
            };
      
            return View(orcamentoGeral);
        }


        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orcamentoGeral = await _context.OrcamentoGeral
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orcamentoGeral == null)
            {
                return NotFound();
            }

            return View(orcamentoGeral);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrcamentoGeral orcamentoGeral)
        {
            if (ModelState.IsValid)
            {
                orcamentoGeral.Id = Guid.NewGuid();
                _context.Add(orcamentoGeral);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(orcamentoGeral);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orcamentoGeral = await _context.OrcamentoGeral.FindAsync(id);
            if (orcamentoGeral == null)
            {
                return NotFound();
            }
            return View(orcamentoGeral);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, OrcamentoGeral orcamentoGeral)
        {
            if (id != orcamentoGeral.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orcamentoGeral);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrcamentoGeralExists(orcamentoGeral.Id))
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
            return View(orcamentoGeral);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orcamentoGeral = await _context.OrcamentoGeral
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orcamentoGeral == null)
            {
                return NotFound();
            }

            return View(orcamentoGeral);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var orcamentoGeral = await _context.OrcamentoGeral.FindAsync(id);
            if (orcamentoGeral != null)
            {
                _context.OrcamentoGeral.Remove(orcamentoGeral);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrcamentoGeralExists(Guid id)
        {
            return _context.OrcamentoGeral.Any(e => e.Id == id);
        }
    }
}
