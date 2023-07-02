using System;
using System.Collections.Generic;
using System.Text;

namespace InfoManager.Shared.Requests;

public interface ICreateTableRequest
{
    public int SpaceId { get; set; }
    public string TableName { get; set; }
}
