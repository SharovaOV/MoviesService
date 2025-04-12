namespace Movies.Api.Constants
{
    public static class IdentityExtensions
    {
        public static Guid? GetUserId(this HttpContext context)
        {
            var userId = context.User.Claims.SingleOrDefault(x => x.Type == "userid");
            if (Guid.TryParse(userId?.Value, out var passedId))
                return passedId;

            return null;
        }
    }
}
