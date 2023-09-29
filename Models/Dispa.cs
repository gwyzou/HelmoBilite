using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace HelmoBilite.Models;

public class Dispa : HelmoMember
{
    [Display(Name = "Diploma")]
    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public Diploma Diploma { get; set; }
}