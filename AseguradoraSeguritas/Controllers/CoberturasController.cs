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
    public class CoberturasController : Controller
    {
        private readonly MyDBContex _context;

        public CoberturasController(MyDBContex context)
        {
            _context = context;
        }

        // GET: Coberturas
        public async Task<IActionResult> Index()
        {
            var myDBContex = _context.Cobertura.Include(c => c.Plan);
            return View(await myDBContex.ToListAsync());
        }

        // GET: Coberturas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cobertura = await _context.Cobertura
                .Include(c => c.Plan)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cobertura == null)
            {
                return NotFound();
            }

            return View(cobertura);
        }

        // GET: Coberturas/Create
        public IActionResult Create()
        {
            ViewData["PlanId"] = new SelectList(_context.Plan, "Id", "Id");
            return View();
        }

        // POST: Coberturas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Descripcion,FechaModificacion,PlanId")] Cobertura cobertura)
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());

            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            var logger = LogManager.GetLogger(typeof(ClientesController));

            logger.Info("Se Agrego la Cobertura :" + cobertura.Descripcion.ToString());

            if (ModelState.IsValid)
            {
                _context.Add(cobertura);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PlanId"] = new SelectList(_context.Plan, "Id", "Id", cobertura.PlanId);
            return View(cobertura);
        }

        // GET: Coberturas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cobertura = await _context.Cobertura.FindAsync(id);
            if (cobertura == null)
            {
                return NotFound();
            }
            ViewData["PlanId"] = new SelectList(_context.Plan, "Id", "Id", cobertura.PlanId);
            return View(cobertura);
        }

        // POST: Coberturas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descripcion,FechaModificacion,PlanId")] Cobertura cobertura)
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());

            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            var logger = LogManager.GetLogger(typeof(ClientesController));

            if (id != cobertura.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cobertura);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CoberturaExists(cobertura.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                logger.Info("Se Modifico la Cobertura: " + id);
                return RedirectToAction(nameof(Index));
            }
            ViewData["PlanId"] = new SelectList(_context.Plan, "Id", "Id", cobertura.PlanId);
            return View(cobertura);
        }

        // GET: Coberturas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cobertura = await _context.Cobertura
                .Include(c => c.Plan)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cobertura == null)
            {
                return NotFound();
            }

            return View(cobertura);
        }

        // POST: Coberturas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());

            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            var logger = LogManager.GetLogger(typeof(ClientesController));

            var cobertura = await _context.Cobertura.FindAsync(id);
            _context.Cobertura.Remove(cobertura);
            await _context.SaveChangesAsync();
            logger.Warn("Se Elimino La Cobertura: " + id);
            return RedirectToAction(nameof(Index));
        }

        private bool CoberturaExists(int id)
        {
            return _context.Cobertura.Any(e => e.Id == id);
        }
    }
}
