using Microsoft.AspNetCore.Mvc.Rendering;

namespace HelmoBilite.Views
{
    public static class DeliveryNavManager
    {
        public static string? PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["Title"] as string
                ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }

        public static string? DeliverySubMenuNavClass(ViewContext viewContext, string page)
        {
            var controllerName = viewContext.RouteData.Values["controller"]?.ToString();

            if (!viewContext.HttpContext.User.IsInRole("Admin") || controllerName != "Delivery" || viewContext.ViewData["Title"]== "Statistics")
            {
                return PageNavClass(viewContext, page);
            }
            return "active";
            
        }
    }
}
