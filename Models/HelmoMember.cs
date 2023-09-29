using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HelmoBilite.CustomValidation;
using Microsoft.AspNetCore.Identity;

namespace HelmoBilite.Models;

public abstract class HelmoMember : BasicUser
{
    [Display(Name = "Mat")]
    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string Mat { get; set; }
    
    [Display(Name = "Birthday")]
    [Column(TypeName = "Date")]
    [PersonalData]
    [DataType(DataType.Date)]
    [BirthDateValidation]
    public DateTime BirthdayOnly { get; set; }
    
}