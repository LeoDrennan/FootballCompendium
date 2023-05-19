using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FootballCompendium.Data;
using FootballCompendium.Models;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Reflection;
using Microsoft.DotNet.MSIdentity.Shared;
using static FootballCompendium.Models.WeatherReport;
using Microsoft.AspNetCore.Authorization;

namespace FootballCompendium.Controllers
{
    public class StadiaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StadiaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Stadia
        public async Task<IActionResult> Index()
        {
              return _context.Stadium != null ? 
                          View(await _context.Stadium.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Stadium'  is null.");
        }

        // GET
        public async Task<IActionResult> Search()
        {
            return _context.Stadium != null ?
                        View() :
                        Problem("Entity set 'ApplicationDbContext.Stadium'  is null.");
        }

        // POST
        public async Task<IActionResult> ShowSearch(String Search)
        {
            return View("Index", await _context.Stadium.Where( j => j.stadium_name.Contains(Search) ).ToListAsync());
        }

        // GET: Stadia/Details/5
        public async Task<IActionResult> WeatherReport(int? id)
        {
            if (id == null || _context.Stadium == null)
            {
                return NotFound();
            }

            var stadium = await _context.Stadium
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stadium == null)
            {
                return NotFound();
            }

            string stadiumLocation = stadium.stadium_location;
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://weatherapi-com.p.rapidapi.com/current.json?q=" + stadiumLocation),
                Headers =
            {
                { "X-RapidAPI-Key", "e14ca1b418msh3c83736abd46698p1ffdf9jsna5702b44ac85" },
                { "X-RapidAPI-Host", "weatherapi-com.p.rapidapi.com" },
            },
            };

            WeatherReport newWeatherReport = new WeatherReport();
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                string body = await response.Content.ReadAsStringAsync();
                newWeatherReport = JsonConvert.DeserializeObject<WeatherReport>(body);
                newWeatherReport.stadium_name = stadium.stadium_name;
                newWeatherReport.stadium_location = stadium.stadium_location;
                newWeatherReport.stadium_capacity = stadium.capacity;
                newWeatherReport.description = stadium.description;
            }

            return View(newWeatherReport);
        }

        // GET: Stadia/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Stadia/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,stadium_name,stadium_location,capacity,description")] Stadium stadium)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stadium);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(stadium);
        }

        // GET: Stadia/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Stadium == null)
            {
                return NotFound();
            }

            var stadium = await _context.Stadium.FindAsync(id);
            if (stadium == null)
            {
                return NotFound();
            }
            return View(stadium);
        }

        // POST: Stadia/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,stadium_name,stadium_location,capacity,description")] Stadium stadium)
        {
            if (id != stadium.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stadium);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StadiumExists(stadium.Id))
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
            return View(stadium);
        }

        // GET: Stadia/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Stadium == null)
            {
                return NotFound();
            }

            var stadium = await _context.Stadium
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stadium == null)
            {
                return NotFound();
            }

            return View(stadium);
        }

        // POST: Stadia/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Stadium == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Stadium'  is null.");
            }
            var stadium = await _context.Stadium.FindAsync(id);
            if (stadium != null)
            {
                _context.Stadium.Remove(stadium);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StadiumExists(int id)
        {
          return (_context.Stadium?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
