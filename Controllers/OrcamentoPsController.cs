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
    public class OrcamentoPsController : Controller
    {
        private readonly ApplicationDataContext _context;

        public OrcamentoPsController(ApplicationDataContext context)
        {
            _context = context;
        }



        public async Task<IActionResult> DownloadRelatorio(Guid id, OrcamentoP orcamento)
        {

            var gastos = _context.OrcamentoP.ToList();


            var orderedOrcamentosG = gastos.OrderByDescending(o => o.ActionCreate).ToList();


            var orcamentos = new OrcamentoP{ Gastos = orderedOrcamentosG };
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

                    for (int i = 0; i < orderedOrcamentosG.Count; i++)
                    {
                        tabela.AddCell(orderedOrcamentosG[i].Description);
                        tabela.AddCell(orderedOrcamentosG[i].Value.ToString("F2"));
                        tabela.AddCell(orderedOrcamentosG[i].ActionCreate.ToString("dd/MM/yyyy"));
                        tabela.AddCell(orderedOrcamentosG[i].TipoOperacao);
                        tabela.AddCell(orderedOrcamentosG[i].FormaPag);

                       
                        if (i == orderedOrcamentosG.Count - 1)
                        {
                            var totalValue = orderedOrcamentosG.Sum(item => item.Value);
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



        public async Task<IActionResult> DashboardD(string trimestre, int? ano)
        {

            var orcamentos = _context.OrcamentoP.AsQueryable();
       
            if (ano.HasValue)
            {
                orcamentos = orcamentos.Where(o => o.ActionCreate.Year == ano.Value);
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

                orcamentos = orcamentos.Where(o => o.ActionCreate.Month >= startMonth && o.ActionCreate.Month <= endMonth);


            }


            var orderedOrcamentos = orcamentos.OrderByDescending(o => o.ActionCreate).ToList();
           
            ViewBag.Anos = _context.OrcamentoRec.Select(o => o.ActionCreateR.Year).Distinct().OrderByDescending(y => y).ToList();

            return View(orderedOrcamentos);
        }


        // GET: OrcamentoPs
        public async Task<IActionResult> Index__G(string trimestre, int? ano, string formPag)
        {
           
            var orcamentos = _context.OrcamentoP.AsQueryable();

         
            if (ano.HasValue)
            {
                orcamentos = orcamentos.Where(o => o.ActionCreate.Year == ano.Value);
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

                orcamentos = orcamentos.Where(o => o.ActionCreate.Month >= startMonth && o.ActionCreate.Month <= endMonth);


            }
            if (!string.IsNullOrEmpty(formPag))
            {
                orcamentos = orcamentos.Where(o => o.FormaPag.Trim().ToUpper() == formPag.Trim().ToUpper());
            }

            var orderedOrcamentos = orcamentos.OrderByDescending(o => o.ActionCreate).ToList();
   
            ViewBag.Anos = _context.OrcamentoP.Select(o => o.ActionCreate.Year).Distinct().OrderByDescending(y => y).ToList();

            return View(orderedOrcamentos);
        }


        // GET: OrcamentoPs/Details/5
        public async Task<IActionResult> Details__G(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orcamentoP = await _context.OrcamentoP.FirstOrDefaultAsync(m => m.Id == id);
            if (orcamentoP == null)
            {
                return NotFound();
            }

            return View(orcamentoP);
        }

        // GET: OrcamentoPs/Create
        public IActionResult Create__G()
        {
            return View();
        }

        // POST: OrcamentoPs/Create
        [HttpPost]

        public async Task<IActionResult> Create__G(OrcamentoP orcamentoP)
        {
            
                if (orcamentoP.TipoOperacao == "Despesa")
                {
                    orcamentoP.Id = Guid.NewGuid();
                    _context.Add(orcamentoP);
                    try
                    {
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Index__G", "OrcamentoPs");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, "Ocorreu um erro ao salvar a despesa: " + ex.Message);
                    }
                }
                else if (orcamentoP.TipoOperacao == "Receita")
                {
                    var orcamentoRec = new OrcamentoRec
                    {
                        Id = Guid.NewGuid(),
                        DescriptionR = orcamentoP.Description,
                        ValueR = orcamentoP.Value,
                        ActionCreateR = orcamentoP.ActionCreate,
                        TipoOperacao = orcamentoP.TipoOperacao
                    };

                    _context.Add(orcamentoRec);
                    try
                    {
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Index", "OrcamentoRec");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, "Ocorreu um erro ao salvar a receita: " + ex.Message);
                    }
                }
            
            return View(orcamentoP);
        }

        [HttpPost]
        public async Task<IActionResult> Edit__G(Guid id, OrcamentoP orcamentoP)
        {
            if (id != orcamentoP.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orcamentoP);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrcamentoPExists(orcamentoP.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index__G));
            }
            return View(orcamentoP);
        }

        // GET: OrcamentoRec/Delete/5
        public async Task<IActionResult> Delete__G(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orcamentoP = await _context.OrcamentoP
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orcamentoP == null)
            {
                return NotFound();
            }

            return View(orcamentoP);
        }

        // POST: OrcamentoRec/Delete/5
        [HttpPost, ActionName("Delete__G")]
        public async Task<IActionResult> Delete__GConfirmed(Guid id)
        {
            var orcamentoP = await _context.OrcamentoP.FindAsync(id);
            if (orcamentoP != null)
            {
                _context.OrcamentoP.Remove(orcamentoP);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index__G));
        }

        private bool OrcamentoPExists(Guid id)
        {
            return _context.OrcamentoP.Any(e => e.Id == id);
        }
    }
}
