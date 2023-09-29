using HelmoBilite.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Linq;

namespace HelmoBilite.ViewModel
{
    public class DeliveryViewModel
    {
        [Display(Name = "Driver")]
        public Driver? Driver { get; set; }


        [Display(Name = "Truck")]
        public Truck? Truck { get; set; }


        [Display(Name = "Client")]
        public Client? Client { get; set; }


        [Display(Name = "Weight (Kg)")]
        public float Weight { get; set; }


        [Display(Name = "Wanted Start Date")]
        [Required(ErrorMessage = "Please enter the Wanted Delivery Start Date.")]
        [DateValidation("" , ErrorMessage = "Cannot start a Delivery in the past.")]
        public DateTime WantedStartDate { get; set; }


        [Display(Name = "Wanted End Date")]
        [Required(ErrorMessage = "Please enter the Wanted Delivery End Date.")]
        [DateValidation("WantedStartDate", ErrorMessage = "Wanted Delivery End Date must be later than Wanted Delivery Start Date.")]
        
        public DateTime WantedEndDate { get; set; }

        [Display(Name = "Pick up Address")]
        [Required(ErrorMessage = "Please enter pick up adress.")]
        public string PickUpAddress { get; set; }


        [Display(Name = "Delivery Address")]
        [Required(ErrorMessage = "Please enter delivery adress.")]
        public string DeliveryAddress { get; set; }

        
        [Display(Name = "Content")]
        [Required(ErrorMessage = "Please enter the content of the delivery.")]
        public string Content { get; set; }

        [Display(Name = "Delivery Status")]
        public Status? Status { get; set; }


        [Display(Name = "Comment")]
        [Column(TypeName = "nvarchar(300)")]
        public string? Comment { get; set; }


        [AttributeUsage(AttributeTargets.Property)]
        public class DateValidationAttribute : ValidationAttribute
        {
            private readonly string _property;
            public DateValidationAttribute(string property)
            {
                _property = property;
            }

            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                var property = validationContext.ObjectType.GetProperty(_property);
                if (property != null)
                {
                    var startDateValue = property.GetValue(validationContext.ObjectInstance);
                    if (validateTime(startDateValue, value))
                    {
                        return ValidationResult.Success;
                    }
                }
                else
                {
                    if (_property == "")
                    {
                        if (validateTime(DateTime.Now, value))
                        {
                            return ValidationResult.Success;
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Property with this name not found");
                    }
                }

                return new ValidationResult(ErrorMessage);
            }
            private Boolean validateTime(object start, object end)
            {
                if(start is null || end is null)
                {
                    return false;
                }
                return start is DateTime startDate && end is DateTime endDate && endDate > startDate;
            }
        }

       
    }
}
