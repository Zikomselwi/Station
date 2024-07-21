using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Station.Data;
using Station.dto;
using Station.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Station.Controllers
{
    [Authorize(Roles = clsRole.roleauser)]

    [Route("api/[controller]")]
    [ApiController]
    public class ReaderController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ReaderController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] updateReadings dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var reading = await _context.Readings.FindAsync(id);
            if (reading == null)
            {
                return NotFound(new { message = "Reading not found" });
            }

            // Check if the current reading is greater than the previous reading
            var latestReading = await _context.Readings
                .Where(r => r.MeterId == reading.MeterId && r.Id != id) // Exclude current reading
                .OrderByDescending(r => r.dateTime)
                .FirstOrDefaultAsync();

            if (latestReading != null && dto.readcurrent <= latestReading.CurrentRead)
            {
                return BadRequest(new { message = "Current reading must be greater than the previous reading" });
            }

            // Update only the CurrentRead property of the existing reading with new data
            reading.CurrentRead = (float)dto.readcurrent;
            reading.dateTime = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Reading updated successfully" });
        }

        // GET api/Reader/Get/{meterNumber}
        [HttpGet("Get/{meterNumber}")]
        public async Task<IActionResult> Get(int meterNumber)
        {
            var meter = await _context.Meters.FirstOrDefaultAsync(m => m.numberMeter == meterNumber);

            if (meter == null)
            {
                return NotFound(new { message = "Meter not found" });
            }

            var subscriber = await _context.Subscribers.FirstOrDefaultAsync(s => s.MeterId == meter.Id);

            if (subscriber == null)
            {
                return NotFound(new { message = "Subscriber not found" });
            }

            var readings = await _context.Readings.Where(r => r.MeterId == meter.Id)
                .OrderByDescending(r => r.dateTime)
                .Take(2)
                .ToListAsync();
            var previousReading = readings.Skip(1).FirstOrDefault();
            var currentReading = readings.FirstOrDefault();





            if (readings == null)
            {
                return NotFound(new { message = "No readings found" });
            }

            var result = new
            {
                MeterNumber = meter.numberMeter,
                SubscriberName = subscriber.FullName,
                currentReading = currentReading?.CurrentRead,
                PreviousReadings = previousReading?.CurrentRead
            };

            return Ok(result);
        }



        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ReadingMeter dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var meter = await _context.Meters.FirstOrDefaultAsync(m => m.Id == dto.meterId);
            if (meter == null)
            {
                return NotFound(new { message = "Meter not found" });
            }

            var subscriber = await _context.Subscribers.FirstOrDefaultAsync(s => s.MeterId == meter.Id);
            if (subscriber == null)
            {
                return NotFound(new { message = "Subscriber not found" });
            }

            var latestReading = await _context.Readings
                .Where(r => r.MeterId == meter.Id)
                .OrderByDescending(r => r.dateTime)
                .FirstOrDefaultAsync();

            if (latestReading != null && dto.readcurrent <= latestReading.CurrentRead)
            {
                return BadRequest(new { message = "Current reading must be greater than the previous reading" });
            }

            var newReading = new Reading
            {
                CurrentRead = (float)dto.readcurrent,
                dateTime = DateTime.Now,
                MeterId = meter.Id,
                ItemId = dto.pointId
            };

            _context.Readings.Add(newReading);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Reading added successfully" });
        }
    }

   
    
}

