using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using teleferic_core_domain.Entities;

namespace teleferic_commerce_infrastructure.Extensions.TokenExtensions
{
    public interface ITokenService
    {
        string CreateToken(ApplicationUser user, IList<string> roles);
    }
}
