using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DeviceManagement_WebApp.Data;
using DeviceManagement_WebApp.Models;
using DeviceManagement_WebApp.Repositories;

namespace DeviceManagement_WebApp.Controllers
{
    public class DevicesController : Controller
    {
        private readonly ConnectedOfficeContext _context;
        private readonly DeviceRepository _deviceRepository;
        private readonly ZoneRepository _zoneRepository;
        private readonly CategoryRepository _categoryRepository;

        public DevicesController(ConnectedOfficeContext context, DeviceRepository deviceRepository, ZoneRepository zoneRepository, CategoryRepository categoryRepository)
        {
            _context = context;
            _deviceRepository = deviceRepository;
            _zoneRepository = zoneRepository;
            _categoryRepository = categoryRepository;
        }

        // GET: Devices
        public async Task<IActionResult> Index()
        {
            return View(await _deviceRepository.GetDeviceListAsync());
        }

        // GET: Devices/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var device = await _deviceRepository.GetDetailAsync(id);

            if (device == null)
            {
                return NotFound();
            }

            return View(device);
        }

        // GET: Devices/Create
        public async Task<IActionResult> Create()
        {
            var categoryList = await _categoryRepository.GetCategoryListAsync();
            var zoneList = await _zoneRepository.GetZoneListAsync();

            ViewData["CategoryId"] = new SelectList(categoryList, "CategoryId", "CategoryName");
            ViewData["ZoneId"] = new SelectList(zoneList, "ZoneId", "ZoneName");

            return View();
        }

        // POST: Devices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DeviceId,DeviceName,CategoryId,ZoneId,Status,IsActvie,DateCreated")] Device device)
        {
            await _deviceRepository.CreateDeviceAsync(device);
            return RedirectToAction(nameof(Index));
        }

        // GET: Devices/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoryList = await _categoryRepository.GetCategoryListAsync();
            var zoneList = await _zoneRepository.GetZoneListAsync();

            ViewData["CategoryId"] = new SelectList(categoryList, "CategoryId", "CategoryName");
            ViewData["ZoneId"] = new SelectList(zoneList, "ZoneId", "ZoneName");

            var device = await _context.Device.FindAsync(id);

            if (device == null)
            {
                return NotFound();
            }

            return View(device);
        }

        // POST: Devices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("DeviceId,DeviceName,CategoryId,ZoneId,Status,IsActvie,DateCreated")] Device device)
        {
            if (id != device.DeviceId)
            {
                return NotFound();
            }
            try
            {
                await _deviceRepository.EditAsync(id, device);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeviceExists(device.DeviceId))
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

        // GET: Devices/Delete/5
        public async Task<IActionResult> DeleteAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var device = await _deviceRepository.GetDetailAsync(id);

            if (device == null)
            {
                return NotFound();
            }

            return View(device);
        }

        // POST: Devices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _deviceRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private bool DeviceExists(Guid id)
        {
            return _context.Device.Any(e => e.DeviceId == id);
        }
    }
}