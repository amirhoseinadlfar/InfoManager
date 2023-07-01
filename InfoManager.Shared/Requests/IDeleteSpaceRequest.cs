using System;
using System.Collections.Generic;
using System.Text;

namespace InfoManagerShared.Requests;

public interface IDeleteSpaceRequest
{
    public int SpaceId { get; set; }
}
