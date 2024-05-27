#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebCRUDMVCSQL.Models;

namespace WebCRUDMVCSQL.Controllers
{
    [Authorize]
    public class PessoassController : Controller
    {
        private readonly Contexto _context;

        public PessoassController(Contexto context)
        {
            _context = context;
        }

        // GET: Pessoass
        public async Task<IActionResult> Index()
        {
            return View(await _context.Pessoas.ToListAsync());
        }

        // GET: Pessoass/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Pessoas = await _context.Pessoas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Pessoas == null)
            {
                return NotFound();
            }

            return View(Pessoas);
        }

        // GET: Pessoass/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Pessoass/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome")] Pessoas Pessoas)
        {
            if (ModelState.IsValid)
            {
                _context.Add(Pessoas);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(Pessoas);
        }

        // GET: Pessoass/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Pessoas = await _context.Pessoas.FindAsync(id);
            if (Pessoas == null)
            {
                return NotFound();
            }
            return View(Pessoas);
        }

        // POST: Pessoass/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome")] Pessoas Pessoas)
        {
            if (id != Pessoas.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(Pessoas);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PessoasExists(Pessoas.Id))
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
            return View(Pessoas);
        }

        // GET: Pessoass/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Pessoas = await _context.Pessoas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Pessoas == null)
            {
                return NotFound();
            }

            return View(Pessoas);
        }

        // POST: Pessoass/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var Pessoas = await _context.Pessoas.FindAsync(id);
            _context.Pessoas.Remove(Pessoas);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PessoasExists(int id)
        {
            return _context.Pessoas.Any(e => e.Id == id);
        }
    }
}
