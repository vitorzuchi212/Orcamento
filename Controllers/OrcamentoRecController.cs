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
    public class OrcamentoRecController : Controller
    {
        private readonly ApplicationDataContext _context;

        public OrcamentoRecController(ApplicationDataContext context)
        {
            _context = context;
        }

        // GET: OrcamentoRec
        public async Task<IActionResult> Index()
        {
            var orcamentosR = _context.OrcamentoRec.ToList();
            var orderedOrcamentosR = orcamentosR.OrderByDescending(o => o.ActionCreateR).ToList();
            return View(orderedOrcamentosR);
        }

        // GET: OrcamentoRec/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orcamentoRec = await _context.OrcamentoRec
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orcamentoRec == null)
            {
                return NotFound();
            }

            return View(orcamentoRec);
        }

        // GET: OrcamentoRec/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: OrcamentoRec/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DescriptionR,ValueR,ActionCreateR")] OrcamentoRec orcamentoRec)
        {
            if (ModelState.IsValid)
            {
                orcamentoRec.Id = Guid.NewGuid();
                _context.Add(orcamentoRec);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(orcamentoRec);
        }

        // GET: OrcamentoRec/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orcamentoRec = await _context.OrcamentoRec.FindAsync(id);
            if (orcamentoRec == null)
            {
                return NotFound();
            }
            return View(orcamentoRec);
        }

        // POST: OrcamentoRec/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,DescriptionR,ValueR,ActionCreateR")] OrcamentoRec orcamentoRec)
        {
            if (id != orcamentoRec.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orcamentoRec);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrcamentoRecExists(orcamentoRec.Id))
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
            return View(orcamentoRec);
        }

        // GET: OrcamentoRec/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orcamentoRec = await _context.OrcamentoRec
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orcamentoRec == null)
            {
                return NotFound();
            }

            return View(orcamentoRec);
        }

        // POST: OrcamentoRec/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var orcamentoRec = await _context.OrcamentoRec.FindAsync(id);
            if (orcamentoRec != null)
            {
                _context.OrcamentoRec.Remove(orcamentoRec);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrcamentoRecExists(Guid id)
        {
            return _context.OrcamentoRec.Any(e => e.Id == id);
        }
    }
}
