using Bogus.DataSets;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace HelmoBilite.Models;

public enum DriverLicence
{
    B,
    C,
    Ce
}

public enum TruckBrand
{
    Iveco,
    Mann,
    Mercedes,
    Renault,
    Scania,
    Volvo,
    Daf
}

public enum Diploma
{
    Cess,
    Bachelor,
    Master
}

public enum Status
{
    [Description("Waiting assignment")]
    Unassigned,
    [Description("Waiting delivery")]
    Pending,
    [Description("Delivery succeded")]
    Success,
    [Description("Delivery cancelled")]
    Cancelled
}

public enum CancelReason
{
    
    [Display(Name = "The driver was sick")]
    DSickness,
    [Display(Name = "The driver had an accident")]
    DAccident,
    [Display(Name = "The client was unavailable")]
    CUnavailability
}

public enum Tag
{
    [Description("Bad Payer")]
    BadPayer,
    [Description("Good Payer")]
    GoodPayer
}