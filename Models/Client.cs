using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace HelmoBilite.Models;

public class Client : BasicUser
{
    [Display(Name = "Company Name")]
    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string CompanyName { get; set; }

    [Display(Name = "Company Address")]
    [PersonalData]
    [Column(TypeName = "nvarchar(250)")]
    public string CompanyAddress { get; set; }

    [Display(Name = "Client Tag")]
    [Column(TypeName = "nvarchar(100)")]
    public Tag? Tag { get; set; }
}