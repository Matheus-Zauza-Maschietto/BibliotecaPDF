using bibliotecaPDF.Models;
using bibliotecaPDF.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace bibliotecaPDF.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class FilesController : ControllerBase
{
    private readonly FileService _fileService;
    public FilesController(FileService fileService)
    {
        _fileService = fileService;
    }

    [HttpPost]
    public async Task<IActionResult> UploadPDF(IFormFile formFile)
    {
        try
        {
            string? userEmailClaim = User.FindFirstValue(ClaimTypes.Email);
            await _fileService.CreateFile(formFile, userEmailClaim ?? "");
            return Ok("PDF Adicionado com sucesso");
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetUserPDFs()
    {
        try
        {
            string? userEmailClaim = User.FindFirstValue(ClaimTypes.Email);
            return Ok(await _fileService.GetFilesList(userEmailClaim ?? ""));
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet("{pdfName}")]
    public async Task<IActionResult> GetPDFByName([FromRoute]string pdfName)
    {
        try
        {
            string? userEmailClaim = User.FindFirstValue(ClaimTypes.Email);
            PdfFile pdfFile = await _fileService.GetFileByName(pdfName, userEmailClaim);
            return File(pdfFile.Content, "application/octet-stream", pdfFile.FileName);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }
}

