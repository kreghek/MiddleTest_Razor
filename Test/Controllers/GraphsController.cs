using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Test;
using Test.BLL;

namespace Test.Controllers
{
    public class GraphsController : Controller
    {
        private readonly TestDbContext _context;

        public GraphsController(TestDbContext context)
        {
            _context = context;
        }

        // GET: Graphs
        public async Task<IActionResult> Index()
        {
            return View(await _context.Graphs.ToListAsync());
        }

        // GET: Graphs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var graph = await _context.Graphs
                .SingleOrDefaultAsync(m => m.Id == id);
            if (graph == null)
            {
                return NotFound();
            }

            return View(graph);
        }

        // GET: Graphs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Graphs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Graph graph)
        {
            if (ModelState.IsValid)
            {
                _context.Add(graph);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(graph);
        }

        // GET: Graphs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var graph = await _context.Graphs.SingleOrDefaultAsync(m => m.Id == id);
            if (graph == null)
            {
                return NotFound();
            }
            return View(graph);
        }

        // POST: Graphs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Graph graph)
        {
            if (id != graph.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(graph);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GraphExists(graph.Id))
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
            return View(graph);
        }

        // GET: Graphs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var graph = await _context.Graphs
                .SingleOrDefaultAsync(m => m.Id == id);
            if (graph == null)
            {
                return NotFound();
            }

            return View(graph);
        }

        // POST: Graphs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var graph = await _context.Graphs.SingleOrDefaultAsync(m => m.Id == id);
            _context.Graphs.Remove(graph);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> TopoAsync(int id)
        {
            var graph = await _context.Graphs.SingleOrDefaultAsync(m => m.Id == id);
            if (graph == null)
            {
                return NotFound();
            }

            var topoSorter = new TopoSorter();
            var nodes = topoSorter.Sort(graph);

            return View(nodes);
        }

        private bool GraphExists(int id)
        {
            return _context.Graphs.Any(e => e.Id == id);
        }
    }
}
