using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Orcamento.InfraStructure.Data.Context;
using Orcamento.Models;

namespace Orcamento.Controllers
{
    public class OrcamentoPsController : Controller
    {
        private readonly ApplicationDataContext _context;

        public OrcamentoPsController(ApplicationDataContext context)
        {
            _context = context;
        }

        // GET: OrcamentoPs
        public IActionResult Index__G()
        {
            var orcamentos = _context.OrcamentoP.ToList();
            var orderedOrcamentos = orcamentos.OrderBy(o => o.ActionCreate).ToList();
            return View(orderedOrcamentos);
        }

        // GET: OrcamentoPs/Details/5
        public async Task<IActionResult> Details__G(Guid? id)
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

        // GET: OrcamentoPs/Create
        public IActionResult Create__G()
        {
            return View();
        }

        // POST: OrcamentoPs/Create
        [HttpPost]
        public async Task<IActionResult> Create__G( OrcamentoP orcamentoP)
        {
            if (ModelState.IsValid)
            {
                orcamentoP.Id = Guid.NewGuid();
                _context.Add(orcamentoP);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index__G));
            }
            return View(orcamentoP);
        }

        // GET: OrcamentoPs/Edit/5
        public async Task<IActionResult> Edit__G(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orcamentoP = await _context.OrcamentoP.FindAsync(id);
            if (orcamentoP == null)
            {
                return NotFound();
            }
            return View(orcamentoP);
        }

        // POST: OrcamentoPs/Edit/5
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

        // GET: OrcamentoPs/Delete/5
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

        // POST: OrcamentoPs/Delete/5
        [HttpPost, ActionName("Delete__G")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
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
