using MegwayParcel.Admin.Models.LandMarkViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel;
using OfficeOpenXml;
using System.IO;
using MegwayParcel.Common.Data;

namespace MegwayParcel.Admin.Controllers
{
    [Authorize]
    [DisplayName("Countries")]
    public class LandMarkCountriesController : Controller
    {
        private readonly LogisticERPContext _context;

        public LandMarkCountriesController(LogisticERPContext context)
        {
            _context = context;
        }
        [AcceptVerbs("Get", "Post")]
        public IActionResult VerifySortOrder(int sortOrder, string Name)
        {
            var exists = _context.LandMarkCountries.Any(c => c.SortOrder == sortOrder && c.Name != Name);

            if (exists)
            {
                return Json($"SortOrder {sortOrder} is already in use.");
            }

            return Json(true);
        }

        // GET: LandMarkCountries/Import
        public IActionResult Import()
        {
            return View();
        }

        // POST: LandMarkCountries/Import
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile excelFile)
        {
            if (excelFile == null || excelFile.Length == 0)
            {
                ModelState.AddModelError("", "Please select a valid Excel file.");
                ViewBag.MessageType = "error";
                ViewBag.Message = "Please select a valid Excel file.";
                return View();
            }
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            using (var stream = new MemoryStream())
            {
                await excelFile.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    int rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++) // Assuming first row has headers
                    {
                        try
                        {
                            int id = Convert.ToInt32(worksheet.Cells[row, 1].Value?.ToString().Trim());
                            string name = worksheet.Cells[row, 2].Value?.ToString().Trim();
                            string iso = worksheet.Cells[row, 3].Value?.ToString().Trim();
                            int sortOrder = Convert.ToInt32(worksheet.Cells[row, 4].Value?.ToString().Trim());
                            bool isActive = Convert.ToBoolean(worksheet.Cells[row, 5].Value?.ToString().Trim());
                            DateTime createdAt = DateTime.Parse(worksheet.Cells[row, 6].Value?.ToString().Trim());
                            // Check for duplication based on unique fields (e.g., Id or Name)
                            bool exists = _context.LandMarkCountries.Any(c => c.CountryId == id || c.Name == name);

                            if (!exists)
                            {
                                var newLandMarkCountry = new LandMarkCountries
                                {
                                    CountryId = id,
                                    Name = name,
                                    ISO = iso,
                                    IsActive = isActive,
                                    SortOrder = sortOrder,
                                    CreatedAt = createdAt
                                };

                                _context.LandMarkCountries.Add(newLandMarkCountry);
                            }
                        }
                        catch (Exception ex)
                        {
                            // Handle parsing errors
                            ModelState.AddModelError("", $"Error parsing data at row {row}: {ex.Message}");
                            ViewBag.MessageType = "error";
                            ViewBag.Message = $"Error parsing data at row {row}: {ex.Message}";
                            return View();
                        }
                    }

                    await _context.SaveChangesAsync();
                }
            }
            ViewBag.MessageType = "success";
            ViewBag.Message = "Data imported successfully!";
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Index()
        {
            var countries = await _context.LandMarkCountries.ToListAsync();
            return View(countries);
        }
        // GET: LandMarkCountries/Create
        public IActionResult Create()
        {
            return View();
        }
        // POST: LandMarkCountries/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LandMarkCountryViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Generate new ID
                int newCountryId = _context.LandMarkCountries.Any() ? _context.LandMarkCountries.Max(c => c.CountryId) + 1 : 1;

                var landMarkCountries = new LandMarkCountries
                {
                    CountryId = newCountryId,
                    Name = viewModel.Name,
                    ISO = viewModel.ISO,
                    IsActive = viewModel.IsActive,
                    CreatedAt = DateTime.Now
                };

                _context.Add(landMarkCountries);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }
        // GET: LandMarkCountries/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var landMarkCountries = await _context.LandMarkCountries.FindAsync(id);
            if (landMarkCountries == null)
            {
                return NotFound();
            }

            var viewModel = new LandMarkCountryViewModel
            {
                CountryId = landMarkCountries.CountryId,
                Name = landMarkCountries.Name,
                ISO = landMarkCountries.ISO,
                SortOrder=landMarkCountries.SortOrder,
                IsActive = landMarkCountries.IsActive,
                CreatedAt = landMarkCountries.CreatedAt
            };

            return View(viewModel);
        }
        // POST: LandMarkCountries/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LandMarkCountryViewModel viewModel)
        {
            if (id != viewModel.CountryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var landMarkCountries = new LandMarkCountries
                    {
                        CountryId = (int)viewModel.CountryId,
                        Name = viewModel.Name,
                        ISO = viewModel.ISO,
                        IsActive = viewModel.IsActive,
                        SortOrder = viewModel.SortOrder,
                        CreatedAt = DateTime.Now
                    };

                    _context.Update(landMarkCountries);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LandMarkCountriesExists((int)viewModel.CountryId))
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

            return View(viewModel);
        }
        // GET: LandMarkCountries/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var landMarkCountries = await _context.LandMarkCountries
                .FirstOrDefaultAsync(m => m.CountryId == id);
            if (landMarkCountries == null)
            {
                return NotFound();
            }

            return View(landMarkCountries);
        }
        // POST: LandMarkCountries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var landMarkCountries = await _context.LandMarkCountries.FindAsync(id);
            _context.LandMarkCountries.Remove(landMarkCountries);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool LandMarkCountriesExists(int id)
        {
            return _context.LandMarkCountries.Any(e => e.CountryId == id);
        }
    }
}