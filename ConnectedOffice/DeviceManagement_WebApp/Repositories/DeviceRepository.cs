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

namespace DeviceManagement_WebApp.Repositories
{
    public class DeviceRepository
    {
        private readonly ConnectedOfficeContext _context;

        public DeviceRepository(ConnectedOfficeContext context)
        {
            _context = context;
        }

        public async Task<List<Device>> GetDeviceListAsync()
        {
            return await _context.Device.ToListAsync();
        }

        public async Task<Device> GetDetailAsync(Guid? id)
        {
            return await _context.Device
                .Include(d => d.Category)
                .Include(d => d.Zone)
                .FirstOrDefaultAsync(m => m.DeviceId == id);
        }

        public async Task CreateDeviceAsync(Device device)
        {
            device.DeviceId = Guid.NewGuid();
            _context.Add(device);
            await _context.SaveChangesAsync();
        }

        public async Task EditAsync(Guid id, Device device)
        {
            try
            {
                _context.Update(device);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var device = await _context.Device.FindAsync(id);
            _context.Device.Remove(device);
            await _context.SaveChangesAsync();
        }

    }
}