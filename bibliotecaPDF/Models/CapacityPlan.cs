using System.ComponentModel.DataAnnotations;

namespace bibliotecaPDF.Models;

public class CapacityPlan
{
    [Key]
    public int Id { get; set; }
    public ICollection<User> Users { get; set; }
    public Int64 BytesCapacity { get; set; }
    public string PlanName { get; set; }
    public double Value { get; set; }
}