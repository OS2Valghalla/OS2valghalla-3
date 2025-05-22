using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Valghalla.Internal.Application.Modules.Tasks.Requests
{
    public sealed record MoveTasksRequest
    {
        public string[]? TaskIds { get; set; }
        public Guid? targetTeamId { get; set; }
        public Guid? sourceTeamId { get; set; }
    }
}
