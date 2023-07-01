using System;
using System.Collections.Generic;
using System.Text;

namespace InfoManagerShared.Requests
{
    public interface IEditSpaceRequest
    {
        public int SpaceId { get; set; }

        public string? Name { get; set; }
    }
}
