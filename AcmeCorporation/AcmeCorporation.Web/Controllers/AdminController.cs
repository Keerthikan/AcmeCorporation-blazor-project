using System.Text;
using AcmeCorporation.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcmeCorporation.Web.Controllers;

[Route("admin")]
//[Authorize(Roles = "Admin")]
public class AdminController(IAdminService adminService) : Controller
{
    [HttpGet("download-submissions-csv")]
    public async Task<IActionResult> DownloadCsv()
    {
        var submissions = await adminService.ExportSubmissionsCsvAsync();
        return File(submissions, "text/csv", "submissions.csv");
    }
}