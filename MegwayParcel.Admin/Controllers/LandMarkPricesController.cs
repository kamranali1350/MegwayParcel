using MegwayParcel.Admin.Models.LandMarkViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel;
using System.IO;
using MegwayParcel.Common.Data;
using OfficeOpenXml;

namespace MegwayParcel.Admin.Controllers
{
    [Authorize]
    [DisplayName("Prices")]
    public class LandMarkPricesController : Controller
    {
        private readonly LogisticERPContext _context;

        public LandMarkPricesController(LogisticERPContext context)
        {
            _context = context;
        }
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
                            double weight = Convert.ToDouble(worksheet.Cells[row, 2].Value?.ToString().Trim());
                            double price = Convert.ToDouble(worksheet.Cells[row, 3].Value?.ToString().Trim());
                            int countryId = Convert.ToInt32(worksheet.Cells[row, 4].Value?.ToString().Trim());
                            int sortOrder = Convert.ToInt32(worksheet.Cells[row, 5].Value?.ToString().Trim());
                            bool isActive = Convert.ToBoolean(worksheet.Cells[row, 6].Value?.ToString().Trim());
                            DateTime createdAt = DateTime.Parse(worksheet.Cells[row, 7].Value?.ToString().Trim());

                            // Check for duplication based on unique fields (e.g., Weight and CountryId)
                            bool exists = _context.LandMarkPrices.Any(l => l.Weight == weight && l.CountryId == countryId);

                            if (!exists)
                            {
                                var newLandMarkPrice = new LandMarkPrices
                                {
                                    PriceId=id,
                                    Weight = weight,
                                    Price = price,
                                    CountryId = countryId,
                                    SortOrder = sortOrder,
                                    IsActive = isActive,
                                    CreatedAt = DateTime.Now
                                };

                                _context.LandMarkPrices.Add(newLandMarkPrice);
                            }
                            else
                            {
                                // Optionally, log or handle duplicates as needed
                            }
                        }
                        catch (Exception ex)
                        {
                            // Handle parsing errors
                            ModelState.AddModelError("", $"Error parsing data at row {row}: {ex.Message}");
                            return View();
                        }
                    }

                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction(nameof(Index));
        }
        // GET: LandMarkPrices
        public async Task<IActionResult> Index()
        {
            var landMarkPrices = _context.LandMarkPrices.Include(l => l.LandMarkCountries);
            return View(await landMarkPrices.ToListAsync());
        }
        // GET: LandMarkPrices/Create
        public IActionResult Create()
        {
            var countries = _context.LandMarkCountries
                .Select(c => new SelectListItem
                {
                    Value = c.CountryId.ToString(),
                    Text = c.Name
                }).ToList();

            var viewModel = new LandMarkPriceViewModel
            {
                Countries = countries
            };

            return View(viewModel);
        }

        // POST: LandMarkPrices/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LandMarkPriceViewModel viewModel)
        {
            if (_context.LandMarkPrices.Any(p => p.SortOrder == viewModel.SortOrder))
            {
                ModelState.AddModelError("SortOrder", "Sort Order already exists.");
            }

            if (ModelState.IsValid)
            {
                int newPriceId = _context.LandMarkPrices.Any() ? _context.LandMarkPrices.Max(p => p.PriceId) + 1 : 1;

                var landMarkPrices = new LandMarkPrices
                {
                    PriceId = newPriceId,
                    Weight = viewModel.Weight,
                    Price = viewModel.Price,
                    IsActive = viewModel.IsActive,
                    CreatedAt = DateTime.Now,
                    CountryId = viewModel.CountryId,
                    SortOrder = viewModel.SortOrder // Add SortOrder here
                };

                _context.Add(landMarkPrices);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            viewModel.Countries = _context.LandMarkCountries
                .Select(c => new SelectListItem
                {
                    Value = c.CountryId.ToString(),
                    Text = c.Name
                }).ToList();

            return View(viewModel);
        }

        // GET: LandMarkPrices/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var landMarkPrices = await _context.LandMarkPrices.FindAsync(id);
            if (landMarkPrices == null)
            {
                return NotFound();
            }

            var viewModel = new LandMarkPriceViewModel
            {
                PriceId = landMarkPrices.PriceId,
                Weight = (double)landMarkPrices.Weight,
                Price = (double)landMarkPrices.Price,
                IsActive = landMarkPrices.IsActive,
                CreatedAt = landMarkPrices.CreatedAt,
                CountryId = landMarkPrices.CountryId,
                SortOrder=landMarkPrices.SortOrder,
                Countries = _context.LandMarkCountries
                    .Select(c => new SelectListItem
                    {
                        Value = c.CountryId.ToString(),
                        Text = c.Name
                    }).ToList()
            };

            return View(viewModel);
        }

        // POST: LandMarkPrices/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LandMarkPriceViewModel viewModel)
        {
            if (_context.LandMarkPrices.Any(p => p.SortOrder == viewModel.SortOrder && p.PriceId != id))
            {
                ModelState.AddModelError("SortOrder", "Sort Order already exists.");
            }

            if (id != viewModel.PriceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var landMarkPrices = new LandMarkPrices
                    {
                        PriceId = (int)viewModel.PriceId,
                        Weight = viewModel.Weight,
                        Price = viewModel.Price,
                        IsActive = viewModel.IsActive,
                        CreatedAt = DateTime.Now,
                        CountryId = viewModel.CountryId,
                        SortOrder = viewModel.SortOrder // Add SortOrder here
                    };

                    _context.Update(landMarkPrices);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LandMarkPricesExists((int)viewModel.PriceId))
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

            viewModel.Countries = _context.LandMarkCountries
                .Select(c => new SelectListItem
                {
                    Value = c.CountryId.ToString(),
                    Text = c.Name
                }).ToList();

            return View(viewModel);
        }
        // GET: LandMarkPrices/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var landMarkPrices = await _context.LandMarkPrices
                .Include(l => l.LandMarkCountries)
                .FirstOrDefaultAsync(m => m.PriceId == id);
            if (landMarkPrices == null)
            {
                return NotFound();
            }

            return View(landMarkPrices);
        }

        // POST: LandMarkPrices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var landMarkPrices = await _context.LandMarkPrices.FindAsync(id);
            _context.LandMarkPrices.Remove(landMarkPrices);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LandMarkPricesExists(int id)
        {
            return _context.LandMarkPrices.Any(e => e.PriceId == id);
        }
    }

}
