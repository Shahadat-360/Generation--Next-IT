using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using MeetingMinutes.Data;
using MeetingMinutes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;

namespace MeetingMinutes.Controllers
{
    public class MeetingMinutesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly string _connectionString;

        public MeetingMinutesController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // GET: MeetingMinutes
        public async Task<IActionResult> Index()
        {
            var meetingMinutes = await _context.MeetingMinutesMasters
                .OrderByDescending(m => m.Date)
                .ToListAsync();
            return View(meetingMinutes);
        }

        // GET: MeetingMinutes/Create
        public IActionResult Create()
        {
            var viewModel = new MeetingMinutesViewModel
            {
                CorporateCustomers = _context.CorporateCustomers
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name
                    }).ToList(),
                IndividualCustomers = _context.IndividualCustomers
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name
                    }).ToList(),
                ProductServices = _context.ProductServices
                    .Select(p => new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = p.Name
                    }).ToList()
            };

            viewModel.MeetingMinutesMaster.CustomerType = "Corporate"; // Default selection
            return View(viewModel);
        }

        // POST: MeetingMinutes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MeetingMinutesViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Set customer name based on selected ID and type
                if (viewModel.MeetingMinutesMaster.CustomerType == "Corporate")
                {
                    var customer = await _context.CorporateCustomers
                        .FirstOrDefaultAsync(c => c.Id == viewModel.MeetingMinutesMaster.CustomerId);
                    viewModel.MeetingMinutesMaster.CustomerName = customer?.Name;
                }
                else
                {
                    var customer = await _context.IndividualCustomers
                        .FirstOrDefaultAsync(c => c.Id == viewModel.MeetingMinutesMaster.CustomerId);
                    viewModel.MeetingMinutesMaster.CustomerName = customer?.Name;
                }

                // Use stored procedure to save master record
                int masterId = await SaveMeetingMinutesMasterAsync(viewModel.MeetingMinutesMaster);
                
                // Save details if they exist
                if (viewModel.MeetingMinutesDetails?.Count > 0)
                {
                    foreach (var detail in viewModel.MeetingMinutesDetails)
                    {
                        detail.MeetingMinutesMasterId = masterId;
                        
                        // Get product/service name
                        var productService = await _context.ProductServices
                            .FirstOrDefaultAsync(p => p.Id == detail.ProductServiceId);
                        detail.ProductServiceName = productService?.Name;
                        
                        // Use stored procedure to save detail record
                        await SaveMeetingMinutesDetailAsync(detail);
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            // If we got this far, something failed, redisplay form
            viewModel.CorporateCustomers = _context.CorporateCustomers
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList();
            viewModel.IndividualCustomers = _context.IndividualCustomers
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList();
            viewModel.ProductServices = _context.ProductServices
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                }).ToList();

            return View(viewModel);
        }

        // GET: MeetingMinutes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meetingMinutesMaster = await _context.MeetingMinutesMasters
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (meetingMinutesMaster == null)
            {
                return NotFound();
            }

            var meetingMinutesDetails = await _context.MeetingMinutesDetails
                .Where(d => d.MeetingMinutesMasterId == id)
                .ToListAsync();

            var viewModel = new MeetingMinutesViewModel
            {
                MeetingMinutesMaster = meetingMinutesMaster,
                MeetingMinutesDetails = meetingMinutesDetails,
                CorporateCustomers = _context.CorporateCustomers
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name,
                        Selected = meetingMinutesMaster.CustomerType == "Corporate" && 
                                  c.Id == meetingMinutesMaster.CustomerId
                    }).ToList(),
                IndividualCustomers = _context.IndividualCustomers
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name,
                        Selected = meetingMinutesMaster.CustomerType == "Individual" && 
                                  c.Id == meetingMinutesMaster.CustomerId
                    }).ToList(),
                ProductServices = _context.ProductServices
                    .Select(p => new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = p.Name
                    }).ToList()
            };

            return View(viewModel);
        }

        // POST: MeetingMinutes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MeetingMinutesViewModel viewModel)
        {
            if (id != viewModel.MeetingMinutesMaster.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Update customer name
                    if (viewModel.MeetingMinutesMaster.CustomerType == "Corporate")
                    {
                        var customer = await _context.CorporateCustomers
                            .FirstOrDefaultAsync(c => c.Id == viewModel.MeetingMinutesMaster.CustomerId);
                        viewModel.MeetingMinutesMaster.CustomerName = customer?.Name;
                    }
                    else
                    {
                        var customer = await _context.IndividualCustomers
                            .FirstOrDefaultAsync(c => c.Id == viewModel.MeetingMinutesMaster.CustomerId);
                        viewModel.MeetingMinutesMaster.CustomerName = customer?.Name;
                    }

                    // Use stored procedure to update master record
                    await SaveMeetingMinutesMasterAsync(viewModel.MeetingMinutesMaster);

                    // Get existing details
                    var existingDetails = await _context.MeetingMinutesDetails
                        .Where(d => d.MeetingMinutesMasterId == id)
                        .ToListAsync();

                    // Remove details not in the updated list
                    foreach (var existing in existingDetails)
                    {
                        if (!viewModel.MeetingMinutesDetails.Any(d => d.Id == existing.Id))
                        {
                            _context.MeetingMinutesDetails.Remove(existing);
                        }
                    }
                    await _context.SaveChangesAsync(); // Delete removed records

                    // Update or add details
                    if (viewModel.MeetingMinutesDetails?.Count > 0)
                    {
                        foreach (var detail in viewModel.MeetingMinutesDetails)
                        {
                            detail.MeetingMinutesMasterId = id;
                            
                            // Get product/service name
                            var productService = await _context.ProductServices
                                .FirstOrDefaultAsync(p => p.Id == detail.ProductServiceId);
                            detail.ProductServiceName = productService?.Name;
                            
                            // Use stored procedure to save or update detail record
                            await SaveMeetingMinutesDetailAsync(detail);
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (!MeetingMinutesMasterExists(viewModel.MeetingMinutesMaster.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        ModelState.AddModelError("", "An error occurred: " + ex.Message);
                        return View(viewModel);
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            // If we got this far, something failed, redisplay form
            viewModel.CorporateCustomers = _context.CorporateCustomers
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name,
                    Selected = viewModel.MeetingMinutesMaster.CustomerType == "Corporate" && 
                              c.Id == viewModel.MeetingMinutesMaster.CustomerId
                }).ToList();
            viewModel.IndividualCustomers = _context.IndividualCustomers
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name,
                    Selected = viewModel.MeetingMinutesMaster.CustomerType == "Individual" && 
                              c.Id == viewModel.MeetingMinutesMaster.CustomerId
                }).ToList();
            viewModel.ProductServices = _context.ProductServices
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                }).ToList();

            return View(viewModel);
        }

        // AJAX endpoint to get product/service unit
        [HttpGet]
        public async Task<JsonResult> GetProductServiceUnit(int id)
        {
            var productService = await _context.ProductServices.FirstOrDefaultAsync(p => p.Id == id);
            return Json(new { unit = productService?.Unit });
        }

        private bool MeetingMinutesMasterExists(int id)
        {
            return _context.MeetingMinutesMasters.Any(e => e.Id == id);
        }

        // Helper method to save meeting minutes master using stored procedure
        private async Task<int> SaveMeetingMinutesMasterAsync(MeetingMinutesMaster master)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("Meeting_Minutes_Master_Save_SP", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Add parameters
                    command.Parameters.Add("@Id", SqlDbType.Int).Value = master.Id;
                    command.Parameters["@Id"].Direction = ParameterDirection.InputOutput;
                    command.Parameters.Add("@CustomerType", SqlDbType.NVarChar, 20).Value = master.CustomerType;
                    command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = master.CustomerId;
                    command.Parameters.Add("@CustomerName", SqlDbType.NVarChar, 200).Value = master.CustomerName;
                    command.Parameters.Add("@Date", SqlDbType.Date).Value = master.Date;
                    command.Parameters.Add("@Time", SqlDbType.Time).Value = master.Time;
                    command.Parameters.Add("@MeetingPlace", SqlDbType.NVarChar, 200).Value = master.MeetingPlace;
                    command.Parameters.Add("@AttendsFromClientSide", SqlDbType.NVarChar, 500).Value = (object)master.AttendsFromClientSide ?? DBNull.Value;
                    command.Parameters.Add("@AttendsFromHostSide", SqlDbType.NVarChar, 500).Value = (object)master.AttendsFromHostSide ?? DBNull.Value;
                    command.Parameters.Add("@MeetingAgenda", SqlDbType.NVarChar, -1).Value = master.MeetingAgenda;
                    command.Parameters.Add("@MeetingDiscussion", SqlDbType.NVarChar, -1).Value = (object)master.MeetingDiscussion ?? DBNull.Value;
                    command.Parameters.Add("@MeetingDecision", SqlDbType.NVarChar, -1).Value = (object)master.MeetingDecision ?? DBNull.Value;

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();

                    // Get the Id of the saved record
                    master.Id = (int)command.Parameters["@Id"].Value;
                    return master.Id;
                }
            }
        }

        // Helper method to save meeting minutes detail using stored procedure
        private async Task<int> SaveMeetingMinutesDetailAsync(MeetingMinutesDetail detail)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("Meeting_Minutes_Details_Save_SP", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Add parameters
                    command.Parameters.Add("@Id", SqlDbType.Int).Value = detail.Id;
                    command.Parameters["@Id"].Direction = ParameterDirection.InputOutput;
                    command.Parameters.Add("@MeetingMinutesMasterId", SqlDbType.Int).Value = detail.MeetingMinutesMasterId;
                    command.Parameters.Add("@ProductServiceId", SqlDbType.Int).Value = detail.ProductServiceId;
                    command.Parameters.Add("@ProductServiceName", SqlDbType.NVarChar, 200).Value = detail.ProductServiceName;
                    command.Parameters.Add("@Quantity", SqlDbType.Decimal).Value = detail.Quantity;
                    command.Parameters["@Quantity"].Precision = 18;
                    command.Parameters["@Quantity"].Scale = 2;
                    command.Parameters.Add("@Unit", SqlDbType.NVarChar, 50).Value = (object)detail.Unit ?? DBNull.Value;

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();

                    // Get the Id of the saved record
                    detail.Id = (int)command.Parameters["@Id"].Value;
                    return detail.Id;
                }
            }
        }
    }
} 