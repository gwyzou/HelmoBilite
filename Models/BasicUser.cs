using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;

namespace HelmoBilite.Models;

public abstract class BasicUser : IdentityUser
{
    [Display(Name = "Last Name")]
    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string? LastName { get; set; }

    [Display(Name = "First Name")]
    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string? FirstName { get; set; }
    
    [Display(Name = "Profile Picture")]
    [Column(TypeName = "nvarchar(250)")]
    public string? ProfilePicture { get; set; }
}