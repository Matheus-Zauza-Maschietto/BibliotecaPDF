using bibliotecaPDF.DTOs;
using bibliotecaPDF.Models;
using bibliotecaPDF.Services;
using bibliotecaPDF.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using bibliotecaPDF.Enums;
using bibliotecaPDF.Models.Exceptions;

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
    public async Task<IActionResult> UploadPDF(ICollection<IFormFile> formFile)
    {
        try
        {
            string? userEmailClaim = User.FindFirstValue(ClaimTypes.Email);
            await _fileService.CreateFile(formFile, userEmailClaim ?? "");
            return Ok(new ResponseDTO(Status.OK, "PDF Adicionado com sucesso."));
        }
        catch (BussinesException ex)
        {
            return BadRequest(new ResponseDTO(Status.ERROR, ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ResponseDTO(Status.ERROR, ex.Message));
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetUserPDFs()
    {
        try
        {
            string? userEmailClaim = User.FindFirstValue(ClaimTypes.Email);
            var filesList = await _fileService.GetFilesList(userEmailClaim ?? "");
            return Ok(new ResponseDTO(Status.OK, string.Empty, filesList));
        }
        catch (BussinesException ex)
        {
            return BadRequest(new ResponseDTO(Status.ERROR, ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ResponseDTO(Status.ERROR, ex.Message));
        }
    }

    [HttpGet("{pdfName}")]
    public async Task<IActionResult> GetPDFFileByName([FromRoute] string pdfName)
    {
        try
        {
            string? userEmailClaim = User.FindFirstValue(ClaimTypes.Email);
            PdfFile? pdfFile = await _fileService.GetFileByName(pdfName, userEmailClaim);
            return Ok(new ResponseDTO(Status.OK, string.Empty, new PdfFileDTO(pdfFile.FileName, pdfFile.IsFavorite, pdfFile.FileSize)));
        }
        catch (BussinesException ex)
        {
            return BadRequest(new ResponseDTO(Status.ERROR, ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ResponseDTO(Status.ERROR, ex.Message));
        }
    }

    [HttpGet("{pdfName}/file")]
    public async Task<IActionResult> GetFileFromPDFFileByName([FromRoute]string pdfName)
    {
        try
        {
            string? userEmailClaim = User.FindFirstValue(ClaimTypes.Email);
            GetPdfFileDTO pdfFile = await _fileService.GetFileContentByName(pdfName, userEmailClaim);
            return File(pdfFile.FileContent, "application/octet-stream", pdfFile.FileName);
        }
        catch (BussinesException ex)
        {
            return BadRequest(new ResponseDTO(Status.ERROR, ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ResponseDTO(Status.ERROR, ex.Message));
        }
    }

    [HttpDelete("{pdfName}")]
    public async Task<IActionResult> DeletePDFByName([FromRoute] string pdfName)
    {
        try
        {
            string? userEmailClaim = User.FindFirstValue(ClaimTypes.Email);
            await _fileService.DeleteFileByName(pdfName, userEmailClaim);
            return Ok(new ResponseDTO(Status.OK, "PDF Deletado com sucesso."));
        }
        catch (BussinesException ex)
        {
            return BadRequest(new ResponseDTO(Status.ERROR, ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ResponseDTO(Status.ERROR, ex.Message));
        }
    }

    [HttpPatch("favorite/{pdfName}")]
    public async Task<IActionResult> FavoritePDFByName([FromRoute]string pdfName)
    {
        try
        {
            string? userEmailClaim = User.FindFirstValue(ClaimTypes.Email);
            await _fileService.FavoriteFileByName(pdfName, userEmailClaim);
            return Ok(new ResponseDTO(Status.OK, "PDF favoritado."));
        }
        catch (BussinesException ex)
        {
            return BadRequest(new ResponseDTO(Status.ERROR, ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ResponseDTO(Status.ERROR, ex.Message));
        }
    }

    [HttpPatch("unfavorite/{pdfName}")]
    public async Task<IActionResult> UnfavoritePDFByName([FromRoute] string pdfName)
    {
        try
        {
            string? userEmailClaim = User.FindFirstValue(ClaimTypes.Email);
            await _fileService.UnfavoriteFileByName(pdfName, userEmailClaim);
            return Ok(new ResponseDTO(Status.OK, "PDF desfavoritado."));
        }
        catch (BussinesException ex)
        {
            return BadRequest(new ResponseDTO(Status.ERROR, ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ResponseDTO(Status.ERROR, ex.Message));
        }
    }
}

