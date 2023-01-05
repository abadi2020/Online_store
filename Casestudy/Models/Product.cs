using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Casestudy.Models
{
    public class Product
    { 
    [Key]
    public string Id { get; set; }
    [ForeignKey("BrandId")]
    public Brand Brand { get; set; } // generates FK
    [Required]
    public int BrandId { get; set; }
    [Required]
    public string ProductName { get; set; }
    [Required]
    public string GraphicName { get; set; }
    [Required]
    [Column(TypeName = "money")]
    public decimal CostPrice { get; set; }
    [Required]
    [Column(TypeName = "money")]
    public decimal MSRB { get; set; }
    [Required]
    public int QtyOnHanad { get; set; }
    [Required]
    public int QtyOnBackOrder { get; set; }
    [Required]
    [StringLength(2000)]
    public string Description { get; set; }
    [Column(TypeName = "timestamp")]
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    [MaxLength(8)]
    public byte[] Timer { get; set; }
}
}
