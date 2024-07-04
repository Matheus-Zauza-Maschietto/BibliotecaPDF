using bibliotecaPDF.DTOs;
using bibliotecaPDF.Exceptions;
using bibliotecaPDF.Models;
using bibliotecaPDF.Services;
using bibliotecaPDF.Services.Interfaces;
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
    private readonly IFileService _fileService;
    public FilesController(IFileService fileService)
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
        catch (BussinessException ex)
        {
            return Problem(ex.Message);
        }
        catch (Exception ex)
        {
            return Problem("An error was ocurred.");
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
        catch (BussinessException ex)
        {
            return Problem(ex.Message);
        }
        catch (Exception ex)
        {
            return Problem("An error was ocurred.");
        }
    }

    [HttpGet("{pdfName}")]
    public async Task<IActionResult> GetPDFFileByName([FromRoute] string pdfName)
    {
        try
        {
            string? userEmailClaim = User.FindFirstValue(ClaimTypes.Email);
            PdfFile? pdfFile = await _fileService.GetFileByName(pdfName, userEmailClaim);
            return Ok(new PdfFileDTO(pdfFile.FileName, pdfFile.IsFavorite));
        }
        catch (BussinessException ex)
        {
            return Problem(ex.Message);
        }
        catch (Exception ex)
        {
            return Problem("An error was ocurred.");
        }
    }

    [HttpGet("{pdfName}/file")]
    public async Task<IActionResult> GetFileFromPDFFileByName([FromRoute]string pdfName)
    {
        try
        {
            string? userEmailClaim = User.FindFirstValue(ClaimTypes.Email);
            PdfFile pdfFile = await _fileService.GetFileByName(pdfName, userEmailClaim);
            return File(pdfFile.Content, "application/octet-stream", pdfFile.FileName);
        }
        catch (BussinessException ex)
        {
            return Problem(ex.Message);
        }
        catch (Exception ex)
        {
            return Problem("An error was ocurred.");
        }
    }

    [HttpDelete("{pdfName}")]
    public async Task<IActionResult> DeletePDFByName([FromRoute] string pdfName)
    {
        try
        {
            string? userEmailClaim = User.FindFirstValue(ClaimTypes.Email);
            await _fileService.DeleteFileByName(pdfName, userEmailClaim);
            return Ok("PDF unfavorited");
        }
        catch (BussinessException ex)
        {
            return Problem(ex.Message);
        }
        catch (Exception ex)
        {
            return Problem("An error was ocurred.");
        }
    }

    [HttpPatch("favorite/{pdfName}")]
    public async Task<IActionResult> FavoritePDFByName([FromRoute]string pdfName)
    {
        try
        {
            string? userEmailClaim = User.FindFirstValue(ClaimTypes.Email);
            await _fileService.FavoriteFileByName(pdfName, userEmailClaim);
            return Ok("PDF Favorited");
        }
        catch (BussinessException ex)
        {
            return Problem(ex.Message);
        }
        catch (Exception ex)
        {
            return Problem("An error was ocurred.");
        }
    }

    [HttpPatch("unfavorite/{pdfName}")]
    public async Task<IActionResult> UnfavoritePDFByName([FromRoute] string pdfName)
    {
        try
        {
            string? userEmailClaim = User.FindFirstValue(ClaimTypes.Email);
            await _fileService.UnfavoriteFileByName(pdfName, userEmailClaim);
            return Ok("PDF unfavorited");
        }
        catch (BussinessException ex)
        {
            return Problem(ex.Message);
        }
        catch (Exception ex)
        {
            return Problem("An error was ocurred.");
        }
    }
}

