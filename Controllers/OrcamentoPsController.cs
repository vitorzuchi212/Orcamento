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
        public async Task<IActionResult> Index()
        {
            return View(await _context.OrcamentoP.ToListAsync());
        }

        // GET: OrcamentoPs/Details/5
        public async Task<IActionResult> Details(Guid? id)
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
        public IActionResult Create()
        {
            return View();
        }

        // POST: OrcamentoPs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,Value,ActionCreate")] OrcamentoP orcamentoP)
        {
            if (ModelState.IsValid)
            {
                orcamentoP.Id = Guid.NewGuid();
                _context.Add(orcamentoP);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(orcamentoP);
        }

        // GET: OrcamentoPs/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Description,Value,ActionCreate")] OrcamentoP orcamentoP)
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
                return RedirectToAction(nameof(Index));
            }
            return View(orcamentoP);
        }

        // GET: OrcamentoPs/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var orcamentoP = await _context.OrcamentoP.FindAsync(id);
            if (orcamentoP != null)
            {
                _context.OrcamentoP.Remove(orcamentoP);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrcamentoPExists(Guid id)
        {
            return _context.OrcamentoP.Any(e => e.Id == id);
        }
    }
}
