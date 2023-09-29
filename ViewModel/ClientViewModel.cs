using Bogus.DataSets;
using HelmoBilite.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HelmoBilite.ViewModel
{
    public class ClientViewModel
    {
        [Display(Name = "Company Name")]
        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string? CompanyName { get; set; }

        [Display(Name = "Company Address")]
        [PersonalData]
        [Column(TypeName = "nvarchar(250)")]
        public string? CompanyAddress { get; set; }

        [Display(Name = "Client Tag")]
        [Column(TypeName = "nvarchar(100)")]
        public Tag? Tag { get; set; }
    }
}
