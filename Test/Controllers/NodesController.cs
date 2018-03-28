using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Test;
using Test.BLL;
using Test.Models;

namespace Test.Controllers
{
    public class NodesController : Controller
    {
        private readonly TestDbContext _context;

        public NodesController(TestDbContext context)
        {
            _context = context;
        }

        // GET: Nodes
        public async Task<IActionResult> Index(int graphId)
        {
            ViewBag.GraphId = graphId;
            var graph = _context.Graphs.SingleOrDefault(x=>x.Id == graphId);

            return View(graph.Nodes);
        }

        // GET: Nodes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var node = await _context.Node
                .SingleOrDefaultAsync(m => m.Id == id);
            if (node == null)
            {
                return NotFound();
            }

            return View(node);
        }

        // GET: Nodes/Create
        public IActionResult Create(int graphId)
        {
            ViewBag.GraphId = graphId;
            return View();
        }

        // POST: Nodes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Node node, int graphId)
        {
            if (ModelState.IsValid)
            {
                var graph = _context.Graphs.SingleOrDefault(x => x.Id == graphId);
                if (graph.Nodes == null)
                {
                    graph.Nodes = new List<Node>();
                }

                graph.Nodes.Add(node);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { graphId });
            }
            return View(node);
        }

        // GET: Nodes/Edit/5
        public async Task<IActionResult> Edit(int? id, int graphId)
        {
            if (id == null)
            {
                return NotFound();
            }

            var node = await _context.Node.SingleOrDefaultAsync(m => m.Id == id);
            if (node == null)
            {
                return NotFound();
            }
            return View(node);
        }

        // POST: Nodes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int graphId, [Bind("Id,Name")] Node node)
        {
            if (id != node.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(node);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NodeExists(node.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                ViewBag.GraphId = graphId;

                return RedirectToAction(nameof(Index));
            }
            return View(node);
        }

        // GET: Nodes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var node = await _context.Node
                .SingleOrDefaultAsync(m => m.Id == id);
            if (node == null)
            {
                return NotFound();
            }

            return View(node);
        }

        // POST: Nodes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var node = await _context.Node.SingleOrDefaultAsync(m => m.Id == id);
            _context.Node.Remove(node);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Relations(int id, int graphId)
        {
            var graph = _context.Graphs.SingleOrDefault(x => x.Id == graphId);
            var node = _context.Node.SingleOrDefault(x => x.Id == id);
            var models = new List<RelatedNodeViewModel>();
            var relatedModels = node.Nodes.Select(x => new RelatedNodeViewModel {
                Node = x,
                IsRelated = true
            });
            models.AddRange(relatedModels);

            var graphModels = graph.Nodes
                .Where(x=> x != node && !node.Nodes.Contains(x))
                .Select(x=> new RelatedNodeViewModel
            {
                Node = x,
                IsRelated = false
            });
            models.AddRange(graphModels);

            ViewBag.GraphId = graphId;
            ViewBag.NodeId = id;

            return View(models);
        }

        public IActionResult SwitchRelation(int id, int graphId, int relationId)
        {
            var graph = _context.Graphs.SingleOrDefault(x => x.Id == graphId);
            var node = _context.Node.SingleOrDefault(x => x.Id == id);
            var relatedNode = node.Nodes.SingleOrDefault(x => x.Id == relationId);
            if (relatedNode == null)
            {
                relatedNode = graph.Nodes.SingleOrDefault(x => x.Id == relationId);
                node.Nodes.Add(relatedNode);
            }
            else
            {
                node.Nodes.Remove(relatedNode);
            }

            _context.SaveChanges();

            ViewBag.GraphId = graphId;
            ViewBag.NodeId = id;

            return RedirectToAction(nameof(Relations), new { id, graphId });
        }

        private bool NodeExists(int id)
        {
            return _context.Node.Any(e => e.Id == id);
        }
    }
}
