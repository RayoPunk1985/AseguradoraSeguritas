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

    public class ClientesController : Controller
    {

        private readonly MyDBContex _context;
      

        public ClientesController(MyDBContex context)
        {
            _context = context;
        
        }

        // GET: Clientes
        public async Task<IActionResult> Index()
        {
          

            return View(await _context.Cliente.ToListAsync());
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Cliente
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
         
            return View();
        }

        // POST: Clientes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,FechaModificacion")] Cliente cliente)
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());

            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            var logger = LogManager.GetLogger(typeof(ClientesController));

           
            if (!ClienteNombreExists(cliente.Nombre))
            {
                if (ModelState.IsValid)
                {
                    _context.Add(cliente);
                    logger.Info("Se Agrego El Cliente :" + cliente.Nombre.ToString());
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                   
                }
                
            }
            else
            {
                logger.Error("Se Quizo agregar El Cliente :" + cliente.Nombre.ToString());
                return View(cliente);
            }
            
            return View(cliente);
        }

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            
            if (id == null)
            {
                return NotFound();
            }
           
            var cliente = await _context.Cliente.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
           
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,FechaModificacion")] Cliente cliente)
        {

            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());

            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            var logger = LogManager.GetLogger(typeof(ClientesController));
            if (id != cliente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.Id)) 
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }

                   
                }
                logger.Info("Se Modifico El Cliente: " + id);
                return RedirectToAction(nameof(Index));
            }
         
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Cliente
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }
         
            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());

            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            var logger = LogManager.GetLogger(typeof(ClientesController));

            var cliente = await _context.Cliente.FindAsync(id);
            _context.Cliente.Remove(cliente);
            await _context.SaveChangesAsync();
            logger.Warn("Se Elimino El Cliente: " + id);
            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(int id)
        {
            return _context.Cliente.Any(e => e.Id == id );
        }

        private bool ClienteNombreExists(string nombre)
        {
            return _context.Cliente.Any(e => e.Nombre == nombre);
        }
    }
}
