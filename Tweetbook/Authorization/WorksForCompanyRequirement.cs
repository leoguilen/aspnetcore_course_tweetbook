using Microsoft.AspNetCore.Authorization;

namespace Tweetbook.Authorization
{
    public class WorksForCompanyRequirement : IAuthorizationRequirement
    {
        public string DomainName { get; set; }
        public string Role { get; set; }

        public WorksForCompanyRequirement(string domainName, string role)
        {
            DomainName = domainName;
            Role = role;
        }
    }
}
