using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AseguradoraSeguritas.Models;
using System.IO;
using System.Reflection;
using log4net;
using log4net.Config;
namespace AseguradoraSeguritas.Controllers
{
    public class PlanesController : Controller
    {
        private readonly MyDBContex _context;

        public PlanesController(MyDBContex context)
        {
            _context = context;
        }

        // GET: Plans
        public async Task<IActionResult> Index()
        {
            var myDBContex = _context.Plan.Include(p => p.Cliente);
            return View(await myDBContex.ToListAsync());
        }

        // GET: Plans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plan = await _context.Plan
                .Include(p => p.Cliente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (plan == null)
            {
                return NotFound();
            }

            return View(plan);
        }

        // GET: Plans/Create
        public IActionResult Create()
        {
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "Id", "Id");
            return View();
        }

        // POST: Plans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Descripcion,FechaModificacion,ClienteId")] Plan plan)
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());

            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            var logger = LogManager.GetLogger(typeof(ClientesController));

            logger.Info("Se Agrego El Plan :" + plan.Descripcion.ToString());

            if (ModelState.IsValid)
            {
                _context.Add(plan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "Id", "Id", plan.ClienteId);
            return View(plan);
        }

        // GET: Plans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plan = await _context.Plan.FindAsync(id);
            if (plan == null)
            {
                return NotFound();
            }
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "Id", "Id", plan.ClienteId);
            return View(plan);
        }

        // POST: Plans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descripcion,FechaModificacion,ClienteId")] Plan plan)
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());

            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            var logger = LogManager.GetLogger(typeof(ClientesController));

            if (id != plan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(plan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlanExists(plan.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                logger.Info("Se Modifico El Plan: " + id);
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "Id", "Id", plan.ClienteId);
            return View(plan);
        }

        // GET: Plans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plan = await _context.Plan
                .Include(p => p.Cliente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (plan == null)
            {
                return NotFound();
            }

            return View(plan);
        }

        // POST: Plans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());

            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            var logger = LogManager.GetLogger(typeof(ClientesController));

            var plan = await _context.Plan.FindAsync(id);
            _context.Plan.Remove(plan);
            await _context.SaveChangesAsync();
            logger.Warn("Se Elimino El Plan: " + id);
            return RedirectToAction(nameof(Index));
        }

        private bool PlanExists(int id)
        {
            return _context.Plan.Any(e => e.Id == id);
        }

    }
}
