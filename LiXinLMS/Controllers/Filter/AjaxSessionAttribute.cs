using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Retech.Cache;
using Retech.Core.Cache;
using LiXinModels.User;

namespace LiXinControllers.Filter
{
    /// <summary>
    ///     表示请求需要验证Session
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class AjaxSessionAttribute : ActionFilterAttribute
    {
        protected ICacheManager cacheManager = new MemoryCacheManager();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            MethodInfo[] methods =
                filterContext.ActionDescriptor.ControllerDescriptor.ControllerType.GetMethods();

            Type returnType =
                methods.First(
                    p =>
                    p.Name.Equals(filterContext.ActionDescriptor.ActionName, StringComparison.CurrentCultureIgnoreCase))
                       .ReturnType;

            if (returnType == typeof(JsonResult))
            {
                //var dict = cacheManager.Get<Dictionary<string, int>>("exists_sessions", () => { return new Dictionary<string, int>(); });
                var userInfo = filterContext.HttpContext.Session["currentUser"] as Sys_User;
                //Session为空的判断
                if (userInfo == null)
                {
                    string message = "未登录";
                    //没有Session，判断原因
                    //if (dict.ContainsKey(filterContext.HttpContext.Session.SessionID))
                    //{
                    //    //有SessionId，没有Session，属于Session过期
                    //    message = LiXinLanguage.CommonLanguage.Common_LoginMessage;
                    //}
                    var json = new JsonResult
                        {
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                            Data = new { result = 404, content = message }
                        };
                    filterContext.Result = json;
                    return;
                }
                //Session不为空
                //var kv = dict.FirstOrDefault(p => p.Value == userInfo.UserId);
                //if (kv.Key != null && kv.Key != filterContext.HttpContext.Session.SessionID)
                //{
                //    //Session不为空但是SessionId跟记录的不同，别处登录，当前被踢
                //    var json = new JsonResult
                //    {
                //        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                //        Data = new { result = 404, tipTitle = LiXinLanguage.CommonLanguage.Common_Tip, btnValue = LiXinLanguage.CommonLanguage.Common_Sure, content = LiXinLanguage.CommonLanguage.Common_OtherLogin }
                //    };
                //    filterContext.Result = json;
                //    return;
                //}
            }
            base.OnActionExecuting(filterContext);
        }
    }
}