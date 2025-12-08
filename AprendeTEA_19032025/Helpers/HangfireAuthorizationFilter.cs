using Hangfire.Dashboard;

namespace AprendeTEA_19032025.Helpers
{
    // Solo permite ver /hangfire a usuarios autenticados con rol Admin
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            // Si quieres que solo los Admin puedan ver el dashboard:
            return httpContext.User.Identity?.IsAuthenticated == true
                   && httpContext.User.IsInRole("Admin");

            // Si lo quisieras abierto a cualquier autenticado:
            // return httpContext.User.Identity?.IsAuthenticated == true;
        }
    }
}
