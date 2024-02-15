using Microsoft.AspNetCore.Mvc;

namespace bibliotecaPDF.Controllers;

[ApiController]
[Route("[controller]")]
public class TesteController : ControllerBase
{
    [HttpGet]
    public IActionResult GetFile()
    {
        // Caminho para o arquivo PDF no servidor
        string pdfFilePath = Path.Combine(Directory.GetCurrentDirectory(), "INGRESSOLAGUM.pdf");

        // Retorna o arquivo PDF
        return PhysicalFile(pdfFilePath, "application/pdf");
        Task<int> teste(){
            
        } 
    }
}
