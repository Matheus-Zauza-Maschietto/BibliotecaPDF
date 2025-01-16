using System.Numerics;
using System.Runtime.InteropServices.JavaScript;
using B2Net.Models;
using Microsoft.AspNetCore.Identity;

namespace bibliotecaPDF.Models;

public class User: IdentityUser
{
    public string Name { get; set; }
    public List<PdfFile> Files { get; set; }
    public Int64 ByteAmounts { get; set; }
    public CapacityPlan CapacityPlan { get; set; }
    public int CapacityPlanId { get; set; }
}
