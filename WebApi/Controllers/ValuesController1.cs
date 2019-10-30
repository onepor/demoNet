using Emrys.FlashLog;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using My_Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApi.Controllers
{
    [RoutePrefix("api/Values")]
    public class ValuesController : ApiController
    {


        //[HttpPost]
        //public string CsWebApiLog()
        //{
        //    //FlashLogger.Info("CsWebApiLog");

        //    localhost.TestWebServices TestService = new localhost.TestWebServices();

        //    string RetData = TestService.LoginPlayer("123", "123456", "127.0.0.1", "wb");

        //    return "WebServiceRetData：" + RetData;
        //}


        //[Route("token"), HttpPost]
        [HttpPost]
        public IHttpActionResult GetToken()
        {
            var dic = new Dictionary<string, object>();
            foreach (var queryNameValuePair in Request.GetQueryNameValuePairs())
            {
                dic.Add(queryNameValuePair.Key, queryNameValuePair.Value);
            }
            var token = new webapi.Common.JWTHelper().Encode(dic, "shengyu", 30);
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