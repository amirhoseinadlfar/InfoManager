using Microsoft.AspNetCore.Mvc;

using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace InfoManager.Server.Controllers
{
    [ApiController]
    [Route("/api/[action]")]
    [Produces("application/json")]
    public class HomeController : ControllerBase
    {
        static Dictionary<string,object> limits = null;
        [HttpGet()]
        public async Task<Dictionary<string,object>> GetLimits()
        {
            if (limits is not null)
                return limits;
            var requestTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(x => x.Namespace == "InfoManager.Server.Controllers.Requests")
                .ToArray();
            limits = new Dictionary<string, object>();
            foreach (var item in requestTypes)
            {
                var props = item.GetMembers(BindingFlags.Instance | BindingFlags.Public)
                    .Where(x=>x.MemberType == MemberTypes.Property)
                    .Cast<PropertyInfo>()
                    .ToArray();
                Dictionary<string, object> vs = new Dictionary<string, object>();
                string r = item.Name;
                if(r.EndsWith("Request"))
                {
                    r = r[..^"Request".Length];
                }
                if (char.IsLower(r[0]) == false)
                {
                    char[] r2 = r.ToCharArray();
                    r2[0] = char.ToLower(r2[0]);
                    r = new string(r2);
                }
                limits.Add(r,vs);
                foreach (var p in props)
                {
                    Dictionary<string, object> lims = new Dictionary<string, object>();
                    string pname = p.Name;
                    if (char.IsUpper( pname[0]))
                    {
                        var pnameArr = pname.ToCharArray();
                        pnameArr[0] = char.ToLower(pnameArr[0]);
                        pname = new string(pnameArr);
                    }    
                    vs.Add(pname, lims);
                    if (p.PropertyType == typeof(string))
                    {
                        var lenAtt = p.GetCustomAttribute<StringLengthAttribute>();
                        if(lenAtt is not null)
                        {
                            lims.Add("min", lenAtt.MinimumLength);
                            lims.Add("max", lenAtt.MaximumLength);
                        }
                    }
                }
            }
            return limits;
        }
    }
}
