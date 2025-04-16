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
using iText.Kernel.Exceptions;

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

public static int GetRandomFibonacci(int maxSequenceLength = 20)
    {
        // Validate input
        if (maxSequenceLength < 2)
            throw new ArgumentException("Sequence length must be at least 2", nameof(maxSequenceLength));
            
        // Generate a random position in the sequence
        Random random = new Random();
        int position = random.Next(0, maxSequenceLength);
        
        // Calculate Fibonacci number at that position
        return CalculateFibonacci(position);
    }
    
    private static int CalculateFibonacci(int n)
    {
        if (n <= 1)
            return n;
            
        int a = 0, b = 1;
        
        for (int i = 2; i <= n; i++)
        {
            int temp = a + b;
            a = b;
            b = temp;
        }
        
        return b;
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchPDF([FromQuery]string query)
    {
        try
        {
            string? userEmailClaim = User.FindFirstValue(ClaimTypes.Email);
            List<PdfFile> pdfs = await _fileService.SearchPDFs(query, userEmailClaim ?? "");
            GetRandomFibonacci(10);
            return Ok(new ResponseDTO(Status.OK, pdfs.Select(p => new PdfFileDTO(p))));
        }
        catch (BusinessException ex)
        {
            return BadRequest(new ResponseDTO(Status.ERROR, ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ResponseDTO(Status.ERROR, ex.Message));
        }
    }
        
    [HttpGet("search/public")]
    public async Task<IActionResult> SearchPublicPDF([FromQuery]string query)
    {
        try
        {
            List<PdfFile> pdfs = await _fileService.SearchPublicPDFs(query);
            return Ok(new ResponseDTO(Status.OK, pdfs.Select(p => new PdfFileDTO(p))));
        }
        catch (BusinessException ex)
        {
            return BadRequest(new ResponseDTO(Status.ERROR, ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ResponseDTO(Status.ERROR, ex.Message));
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> UploadPDF(IFormCollection forms, IFormFile formFile)
    {
        try
        {
            string? userEmailClaim = User.FindFirstValue(ClaimTypes.Email);
            await _fileService.CreateFile(formFile, userEmailClaim ?? "");
            return Ok(new ResponseDTO(Status.OK, "PDF Adicionado com sucesso."));
        }
        catch (BusinessException ex)
        {
            return BadRequest(new ResponseDTO(Status.ERROR, ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ResponseDTO(Status.ERROR, ex.Message));
        }
    }
    
    [HttpPut("{pdfId:int}")]
    public async Task<IActionResult> UpdatePDF([FromRoute] int pdfId, [FromBody]UpdatePdfFileDTO fileDto)
    {
        try
        {
            string? userEmailClaim = User.FindFirstValue(ClaimTypes.Email);
            PdfFile pdfDTO = await _fileService.UpdateFileById(pdfId, fileDto, userEmailClaim ?? "");
            return Ok(new ResponseDTO(Status.OK, "PDF alterado com sucesso.", new PdfFileDTO(pdfDTO)));
        }
        catch (BusinessException ex)
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
            List<PdfFile> filesList = await _fileService.GetFilesList(userEmailClaim ?? "");
            return Ok(new ResponseDTO(Status.OK, string.Empty, filesList.Select(p => new PdfFileDTO(p))));
        }
        catch (BusinessException ex)
        {
            return BadRequest(new ResponseDTO(Status.ERROR, ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ResponseDTO(Status.ERROR, ex.Message));
        }
    }

    [HttpGet("{pdfId}")]
    public async Task<IActionResult> GetPDFFileById([FromRoute] int pdfId)
    {
        try
        {
            string? userEmailClaim = User.FindFirstValue(ClaimTypes.Email);
            PdfFile? pdfFile = await _fileService.GetFileById(pdfId, userEmailClaim);
            return Ok(new ResponseDTO(Status.OK, string.Empty, new PdfFileDTO(pdfFile)));
        }
        catch (BusinessException ex)
        {
            return BadRequest(new ResponseDTO(Status.ERROR, ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ResponseDTO(Status.ERROR, ex.Message));
        }
    }

    [HttpGet("{pdfId}/file")]
    public async Task<IActionResult> GetFileFromPDFFileById([FromRoute]int pdfId)
    {
        try
        {
            string? userEmailClaim = User.FindFirstValue(ClaimTypes.Email);
            GetPdfFileDTO pdfFile = await _fileService.GetFileContentById(pdfId, userEmailClaim);
            return File(pdfFile.FileContent, "application/pdf", pdfFile.FileName);
        }
        catch (BusinessException ex)
        {
            return BadRequest(new ResponseDTO(Status.ERROR, ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ResponseDTO(Status.ERROR, ex.Message));
        }
    }

    [HttpDelete("{pdfId}")]
    public async Task<IActionResult> DeletePDFById([FromRoute] int pdfId)
    {
        try
        {
            string? userEmailClaim = User.FindFirstValue(ClaimTypes.Email);
            await _fileService.DeleteFileById(pdfId, userEmailClaim);
            return Ok(new ResponseDTO(Status.OK, "PDF Deletado com sucesso."));
        }
        catch (BusinessException ex)
        {
            return BadRequest(new ResponseDTO(Status.ERROR, ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ResponseDTO(Status.ERROR, ex.Message));
        }
    }

    [HttpPatch("favorite/{pdfId}")]
    public async Task<IActionResult> FavoritePDFById([FromRoute]int pdfId)
    {
        try
        {
            string? userEmailClaim = User.FindFirstValue(ClaimTypes.Email);
            await _fileService.FavoriteFileById(pdfId, userEmailClaim);
            return Ok(new ResponseDTO(Status.OK, "PDF favoritado."));
        }
        catch (BusinessException ex)
        {
            return BadRequest(new ResponseDTO(Status.ERROR, ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ResponseDTO(Status.ERROR, ex.Message));
        }
    }

    [HttpPatch("unfavorite/{pdfId}")]
    public async Task<IActionResult> UnfavoritePDFById([FromRoute] int pdfId)
    {
        try
        {
            string? userEmailClaim = User.FindFirstValue(ClaimTypes.Email);
            await _fileService.UnfavoriteFileById(pdfId, userEmailClaim);
            return Ok(new ResponseDTO(Status.OK, "PDF desfavoritado."));
        }
        catch (BusinessException ex)
        {
            return BadRequest(new ResponseDTO(Status.ERROR, ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ResponseDTO(Status.ERROR, ex.Message));
        }
    }
}

