using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Valghalla.Internal.Application.Modules.Shared.Team.Responses
{
    public sealed record TeamSharedResponse
    {
        public Guid Id { get; init; }
        public string Name { get; set; } = null!;
        public string ShortName { get; set; } = null!;
    }
}
