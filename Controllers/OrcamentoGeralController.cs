using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orcamento.InfraStructure.Data.Context;
using Orcamento.Models;

namespace Orcamento.Controllers
{
    public class OrcamentoGeralController : Controller
    {
        private readonly ApplicationDataContext _context;


        public OrcamentoGeralController(ApplicationDataContext context)
        {
            _context = context;
        }


        // GET: OrcamentoGeral
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
