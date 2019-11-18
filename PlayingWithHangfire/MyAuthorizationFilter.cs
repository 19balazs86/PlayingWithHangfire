using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace PlayingWithHangfire
{
  public class MyAuthorizationFilter : IDashboardAuthorizationFilter
  {
    public bool Authorize([NotNull] DashboardContext context)
    {
      var httpContext = context.GetHttpContext();

      return httpContext.User.Identity.IsAuthenticated;
    }
  }
}
