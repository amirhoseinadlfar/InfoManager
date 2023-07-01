using System;
using System.Collections.Generic;
using System.Text;

namespace InfoManagerShared.Requests;

public interface IDeleteMemberRequest
{
    public int SpaceId { get; set; }
    public int MemberId { get; set; }
}
