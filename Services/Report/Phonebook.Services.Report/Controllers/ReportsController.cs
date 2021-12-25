using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Phonebook.Core.ControllerBases;
using Phonebook.Services.Report.Services.Abstract;

namespace Phonebook.Services.Report.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReportsController : CustomBaseController
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet]
        public async Task<IActionResult> CreateReportRequest()
        {
            var response = await _reportService.GetInstantDataAndReportAsync();

            return CreateActionResultInstance(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllReports()
        {
            var response = await _reportService.GetLocationReportList();

            return CreateActionResultInstance(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetReportById(Guid id)
        {
            var response = await _reportService.GetLocationReportByIdAsync(id);

            return CreateActionResultInstance(response);
        }

    }
}

