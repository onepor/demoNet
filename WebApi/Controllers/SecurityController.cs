using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;

namespace WebApi.Controllers
{

    [RoutePrefix("api/Security")]
    public class SecurityController : ApiController
    {

        [Route("token"), HttpGet]
        public IHttpActionResult GetToken()
        {
            var dic = new Dictionary<string, object>();
            foreach (var queryNameValuePair in Request.GetQueryNameValuePairs())
            {
                dic.Add(queryNameValuePair.Key, queryNameValuePair.Value);
            }
            var token = new webapi.Common.JWTHelper().Encode(dic, "wp", 30);
            return Ok(token);
        }

        /// <summary>
        /// 返回token里加密的信息
        /// </summary>
        /// <returns></returns>
        [Route("GetUserInfoFromToken"), HttpGet]
        public IHttpActionResult GetUser()
        {
            var user = (System.Security.Claims.ClaimsPrincipal)User;
            var dic = new Dictionary<string, object>();
            foreach (var userClaim in user.Claims)
            {
                dic.Add(userClaim.Type, userClaim.Value);
            }
            return Ok(dic);
        }
    }
}
