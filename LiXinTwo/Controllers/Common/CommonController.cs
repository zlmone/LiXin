using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using LiXinBLL.ClassRoom;
using LiXinBLL.NewClassManage;
using LiXinBLL.User;
using LiXinCommon;
using LiXinModels.User;
using LiXinModels;
using LiXinBLL;
using LiXinBLL.Report_CPA;

namespace LiXinControllers
{
    #region Sys_User[][] to SeatUser[][]
    /// <summary>
    /// 
    /// </summary>
    public static class Extend
    {
        public static List<SeatPlan.SeatUser[][]> ConvertToSeatUser(this List<Sys_User[][]> seatUsers)
        {
            var result = new List<SeatPlan.SeatUser[][]>();
            foreach (var v in seatUsers)
            {
                var item = new SeatPlan.SeatUser[v.Length][];
                for (var i = 0; i < v.Length; i++)
                {
                    item[i] = new SeatPlan.SeatUser[v[i].Length];
                    for (var j = 0; j < v[i].Length; j++)
                    {
                        if (v[i][j] == null)
                        {
                            item[i][j] = null;
                        }
                        else
                        {
                            item[i][j] = new SeatPlan.SeatUser
                                {
                                    UserId = v[i][j].UserId,
                                    Sex = v[i][j].Sex,
                                    Username = v[i][j].Username,
                                    Realname = v[i][j].Realname,
                                    Ename = v[i][j].Ename,
                                    IsInternExp = v[i][j].IsInternExp,
                                    Major = v[i][j].Major,
                                    GraduateSchool = v[i][j].GraduateSchool
                                };
                        }
                    }
                }

                result.Add(item);
            }

            return result;
        }
    }

    #endregion

    /// <summary>
    ///     通过模块
    /// </summary>
    public class CommonController : BaseController
    {
        private Report_CPABL cpaBL = new Report_CPABL();
        #region 安排座位
           
        /// <summary>
        /// 集中课程安排座位，教室、禁用座位、规则需成对匹配出现
        /// </summary>
        /// <param name="roomIds">以','分割的教室Id</param>
        /// <param name="classIds">以','分割的班级Id</param>
        /// <param name="disableSeats">组内';'分割，组外':'分割。
        /// 以行列的顺序排列
        /// 例如：1,1;1,2:1,1;1,3</param>
        /// <param name="rules">0：不选择；1：选择。组内','分割，组外':'分割。
        /// 四位的顺序分别为：性别、实习经验、院校、专业
        /// 例如1,0,0,1:0,0,1,0</param>
        /// <param name="rowcount">行数</param>
        /// <param name="colcount">列数</param>
        /// <param name="roomType">类型：0方桌；1：圆桌</param>
        /// <returns></returns>
        public JsonResult ArrangeSeat(string roomIds, string classIds, string disableSeats, string rules, int rowcount, int colcount, int roomType)
        {
            var ran = new Random();
            var rooms = roomIds == "" ? new string[] { "0" } : roomIds.Split(',');
            var classes = classIds.Split(',');
            var disableSeatses = disableSeats.Split(':');
            var ruleses = rules.Split(':');

            //教室、禁用座位、规则需成对匹配出现
            if (!(rooms.Length == disableSeatses.Length && rooms.Length == ruleses.Length))
            {
                return Json(new
                {
                    message = "教室、禁用座位、规则需成对匹配出现！"
                }, JsonRequestBehavior.AllowGet);
            }

            var totalCount = 0;
            //获取班级的人
            var classUsers = classes.Select(t => new NewClassBL().GetClassPersonsList(out totalCount, t.StringToInt32())).ToList();

            var seats = new List<Sys_User[][]>();

            //for (var i = 0; i < rooms.Length; i++)
            //{
            //排座规则
            var rule = ruleses[0].Split(',');
            if (rule.Length != 5)
            {
                return Json(new
                {
                    message = "排座规则匹配有误！"
                }, JsonRequestBehavior.AllowGet);
            }

            var disableSeat = disableSeatses[0].Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            var listdisableSeats = new List<SeatPlan.DisableSeatModel>();

            //禁用的座位
            foreach (var rc in disableSeat.Select(t => t.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)))
            {
                if (roomType == 0)
                {
                    if (rc.Length != 2)
                    {
                        return Json(new
                        {
                            message = "禁用座位匹配有误！"
                        }, JsonRequestBehavior.AllowGet);
                    }

                    listdisableSeats.Add(new SeatPlan.DisableSeatModel
                    {
                        Row = rc[0].StringToInt32(),
                        Column = rc[1].StringToInt32()
                    });
                }
            }

            ////教室
            //var room = new NewClassRoomBL().GetRoom(rooms[i].StringToInt32());
            ////需要排序的人员
            var users = new List<Sys_User>();

            ////教室还有的空位数
            //var reNumber = room.MinNum - listdisableSeats.Count;
            //if (reNumber > 0)
            //{
            //    foreach (var t in classUsers)
            //    {
            //        reNumber = room.MinNum - listdisableSeats.Count - users.Count;
            //        //教室已经排完
            //        if (reNumber == 0)
            //        {
            //            continue;
            //        }
            //        //该班级的人已经都选完了
            //        if (t.Count == 0)
            //        {
            //            continue;
            //        }

            //        //空位大于所选的班级人数，班级人员全部选用
            //        if (reNumber >= t.Count)
            //        {
            //            users.AddRange(t);
            //            t.Clear();
            //        }
            //        else
            //        {
            //            while (reNumber > 0 && t.Count > 0)
            //            {
            //                var item = t[ran.Next(t.Count)];
            //                users.Add(item);
            //                t.Remove(item);

            //                reNumber = room.MinNum - listdisableSeats.Count - users.Count;
            //            }
            //        }
            //    }
            //}
            classUsers.ForEach(p => p.ForEach(users.Add));

            //座位排序
            var tempulist = SeatPlan.TempArrangeSeat(new SeatPlan.SeatParaModel
            {
                Row = rowcount,
                Column = colcount,
                IsSexDifferent = rule[0].StringToInt32() == 1,
                IsHaveExp = rule[1].StringToInt32() == 1,
                IsUniversitiesDifferent = rule[2].StringToInt32() == 1,
                IsProfessionalDifferent = rule[3].StringToInt32() == 1,
                IsGroupLink = rule[4].StringToInt32() == 1,
                Users = users,
                DisableSeats = listdisableSeats
            });

            var newCollection = new List<Sys_User>();
            foreach (var c in classIds.Split(','))
            {
                newCollection.AddRange(rule[4].StringToInt32() == 1
                    ? tempulist.Where(p => p.ClassId == c.StringToInt32()).OrderBy(p => p.GroupId)
                    : tempulist.Where(p => p.ClassId == c.StringToInt32()));
            }
            var newseatuser = new List<object>();
            newCollection.ForEach(p => newseatuser.Add(new { p.UserId, p.Realname, p.Sex }));

            //seats.Add(seat);
            //}

            return Json(new
            {
                message = "",
                result = newseatuser //seats.ConvertToSeatUser()
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 分组带教安排座位，教室、学员、禁用座位、规则需成对匹配出现
        /// </summary>
        /// <param name="roomIds">以','分割的教室Id</param>
        /// <param name="classUserIds">按教室顺序，每个教室的学员ID以','分割的，每个教室之间用':'分割，如：1,2,3:4,5,6:7,8,9</param>
        /// <param name="disableSeats">组内';'分割，组外':'分割。
        /// 以行列的顺序排列
        /// 例如：1,1;1,2:1,1;1,3</param>
        /// <param name="rules">0：不选择；1：选择。组内','分割，组外':'分割。
        /// 四位的顺序分别为：性别、实习经验、院校、专业
        /// 例如1,0,0,1:0,0,1,0</param>
        /// <returns></returns>
        public JsonResult CreateGroupTeachSeat(string roomIds, string classUserIds, string disableSeats, string rules)
        {
            var ran = new Random();
            var rooms = roomIds.Split(',');
            var itemClassUserIds = classUserIds.Split(':');
            var disableSeatses = disableSeats.Split(':');
            var ruleses = rules.Split(':');

            //教室、禁用座位、规则需成对匹配出现
            if (!(rooms.Length == disableSeatses.Length && rooms.Length == ruleses.Length && rooms.Length == itemClassUserIds.Length))
            {
                return Json(new
                {
                    message = "教室、学员、禁用座位、规则需成对匹配出现！"
                }, JsonRequestBehavior.AllowGet);
            }

            var seats = new List<Sys_User[][]>();

            for (int i = 0; i < rooms.Length; i++)
            {
                //排座规则
                var rule = ruleses[i].Split(',');
                if (rule.Length != 4)
                {
                    return Json(new
                    {
                        message = "排座规则匹配有误！"
                    }, JsonRequestBehavior.AllowGet);
                }

                var disableSeat = disableSeatses[i].Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                var listdisableSeats = new List<SeatPlan.DisableSeatModel>();

                //禁用的座位
                for (var j = 0; j < disableSeat.Length; j++)
                {
                    var rc = disableSeat[j].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    if (rc.Length != 2)
                    {
                        return Json(new
                        {
                            message = "禁用座位匹配有误！"
                        }, JsonRequestBehavior.AllowGet);
                    }

                    listdisableSeats.Add(new SeatPlan.DisableSeatModel
                    {
                        Row = rc[0].StringToInt32(),
                        Column = rc[1].StringToInt32()
                    });
                }

                //教室
                var room = new NewClassRoomBL().GetRoom(rooms[i].StringToInt32());
                //需要排序的人员
                var users = new UserBL().GetList(string.Format(@"UserId IN ({0}) AND IsDelete=0", string.IsNullOrEmpty(itemClassUserIds[i]) ? "0" : itemClassUserIds[i]));

                //座位排序
                var seat = SeatPlan.ArrangeSeat(new SeatPlan.SeatParaModel
                {
                    Row = room.RowNumber,
                    Column = room.ColumnNumber,
                    IsSexDifferent = rule[0].StringToInt32() == 1,
                    IsHaveExp = rule[1].StringToInt32() == 1,
                    IsUniversitiesDifferent = rule[2].StringToInt32() == 1,
                    IsProfessionalDifferent = rule[3].StringToInt32() == 1,
                    Users = users,
                    DisableSeats = listdisableSeats
                });

                seats.Add(seat);
            }

            return Json(new
            {
                message = "",
                result = seats.ConvertToSeatUser()
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region view
        
       
        public ActionResult RightError(string backUrl)
        {
            ViewBag.backUrl = backUrl;
            return View();
        }


        public ActionResult SiteMapLink(string linkName)
        {
            var str = "";
            GetRightPath(AllRights, linkName, ref str);

            ViewBag.CnTitle = str;//CodeHelper.GetNavigateString(linkName, new CultureInfo("zh-CN")) ?? linkName;
            //ViewBag.UsTitle = CodeHelper.GetNavigateString(linkName, new CultureInfo("en-US")) ?? linkName;
            return View();
        }

        public ActionResult ReportYear(string id = "report_selYear")
        {
            ViewBag.Year = cpaBL.GetCourseYear();
            ViewBag.nowyear = DateTime.Now.Year;
            ViewBag.id = id;
            return View();
        }

        /// <summary>
        /// 等于1 查询所有  等于0 查询执业CPA所包含的级别  2 所有讲师的薪酬级别
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public ActionResult ReportPayGrade(int type = 1)
        {
            // ViewBag.PayGrade = cpaBL.GetPayGrade(" 1=1", type);
            ViewBag.type = type;
            return View();
        }

        /// <summary>
        /// CPA关系所在地
        /// </summary>
        /// <returns></returns>
        public ActionResult ReportCPARelationship()
        {
            // ViewBag.PayGrade = cpaBL.GetPayGrade(" 1=1", type);
            return View();
        }
        #endregion

        #region method
        
        
        /// <summary>
        ///     获取当前请求的页面的菜单
        /// </summary>
        /// <param name="urs">权限集合</param>
        /// <param name="rstr">权限名称</param>
        /// <param name="path">路径</param>
        /// <param name="flag">层级标志</param>
        /// <returns></returns>
        private void GetRightPath(List<Sys_Right> urs, string rstr, ref string path, int flag = 0)
        {
            if (flag == 0)
            {
                path = (CodeHelper.GetNavigateString(rstr, new CultureInfo("zh-CN")) ?? rstr) + path;
            }
            else
            {
                path = (CodeHelper.GetNavigateString(rstr, new CultureInfo("zh-CN")) ?? rstr) + string.Format(" > {0}", flag == 1 ? "</span>" : "") + path;
            }
            flag++;
            var obj = urs.FirstOrDefault(p => p.RightName == rstr);
            if (obj != null)
            {
                var fr = urs.FirstOrDefault(p => p.RightId == obj.ParentId);
                if (fr != null)
                    GetRightPath(urs, fr.RightName, ref path, flag);
                else
                    path = "<span>" + path;
            }
        }

        /// <summary>
        /// 获取我的模块
        /// </summary>
        /// <returns></returns>
        public JsonResult GetMyModule()
        {
            var module1 = UserRights.Where(p => p.ModuleName != null && p.RightType == 0 && p.ModuleName.Trim().ToLower() == "MyTrain".ToLower());//事务所培训（前端）
            var module2 = UserRights.Where(p => p.ModuleName != null && p.RightType == 0 && p.ModuleName.Trim().ToLower() == "TrainManage".ToLower());//事务所培训管理
            var module3 = UserRights.Where(p => p.ModuleName != null && p.RightType == 0 && p.ModuleName.Trim().ToLower() == "DepManage".ToLower());//部门分所管理
            var module4 = UserRights.Where(p => p.ModuleName != null && p.RightType == 0 && p.ModuleName.Trim().ToLower() == "SystemManage".ToLower());//系统管理
            var module5 = UserRights.Where(p => p.ModuleName != null && p.RightType == 0 && p.ModuleName.Trim().ToLower() == "Knowledge".ToLower());//知识中心
            var module6 = UserRights.Where(p => p.ModuleName != null && p.RightType == 0 && p.ModuleName.Trim().ToLower() == "FAQ".ToLower());//FAQ
            var module7 = UserRights.Where(p => p.ModuleName != null && p.RightType == 0 && p.ModuleName.Trim().ToLower() == "NewMyTrain".ToLower());//新员工（前端）
            var module8 = UserRights.Where(p => p.ModuleName != null && p.RightType == 0 && p.ModuleName.Trim().ToLower() == "MyDepTrain".ToLower());//部门分所（前端）

            var list = new List<object>();
            if (CurrentUser.IsMain == 0)
            {
                if (module1.Any())
                {
                    list.Add(new { name = "我的培训", module = "MyTrain", ename = "My Training", order = list.Count + 1 });
                }
                else if (module7.Any())
                {
                    list.Add(new { name = "我的培训", module = "NewMyTrain", ename = "My Training", order = list.Count + 1 });
                }
            }
            else
            {
                if (module1.Any())
                {
                    list.Add(new { name = "我的培训", module = "MyTrain", ename = "My Training", order = list.Count + 1 });
                }
                else if (module7.Any())
                {
                    list.Add(new { name = "我的培训", module = "NewMyTrain", ename = "My Training", order = list.Count + 1 });
                }
                else if (module8.Any())
                {
                    list.Add(new { name = "我的培训", module = "MyDepTrain", ename = "My Training", order = list.Count + 1 });
                }
            }
            if (module2.Any())
                list.Add(new
                {
                    name = "培训管理",
                    module = "TrainManage",
                    ename = "Training Management",
                    order = list.Count + 1
                });
            if (module3.Any())
                list.Add(new
                {
                    name = "部门/分所管理",
                    module = "DepManage",
                    ename = "Department / Office Management",
                    order = list.Count + 1
                });
            if (module4.Any())
                list.Add(new
                {
                    name = "系统管理",
                    module = "SystemManage",
                    ename = "System Management",
                    order = list.Count + 1
                });
            if (module5.Any())
                list.Add(new
                {
                    name = "知识中心",
                    module = "Knowledge",
                    ename = "Knowledge Center",
                    order = list.Count + 1
                });
            if (module6.Any())
                list.Add(new
                {
                    name = "培训政策及FAQ",
                    module = "FAQ",
                    ename = "Department / Office Management",
                    order = list.Count + 1
                });
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MenuHtml()
        {
            if (UserRights.Count == 0)
            {
                ViewBag.MenuStr = "";
                return View();
            }

            string rurl = Request.Url.AbsolutePath.Remove(0, 1); //当前请求的页面url
            string[] urls = rurl.Split('/');
            rurl = urls[0] + "/" + urls[1];
            var rt = GetSubMenu(UserRights, rurl, 0);

            var htmlMenu = new StringBuilder("");
            var rootRight = UserRights.First(p => p.ParentId == 0);
            var tempUserRights = UserRights.Where(p => p.ParentId == rootRight.RightId && p.RightType == 0 && (p.ModuleName == null ? "" : p.ModuleName).ToLower() == Session["moduleName"].ToString().ToLower()).OrderBy(p => p.ShowOrder);
            if (!string.IsNullOrEmpty(CurrentUser.SubordinateSubstation) && CurrentUser.SubordinateSubstation.Contains("上海"))
            {
                tempUserRights = tempUserRights.Where(p => p.RightId != 212 && p.RightId != 220 && p.RightId != 264 && p.RightId != 219 && p.RightId != 335).ToList().OrderBy(p => p.ShowOrder);
            }

            var zindex = 50;
            foreach (var right in tempUserRights)
            {
                zindex--;
                var path = right.Path.IndexOf("/", System.StringComparison.Ordinal) >= 0 ? right.Path : "#";
                var title = CodeHelper.GetNavigateString(right.RightName);
                if (string.IsNullOrEmpty(title))
                    title = right.RightName;
                if (path != "#" && !path.StartsWith("/"))
                {
                    path = "/" + path;
                }
                var subRight = UserRights.Where(p => p.ParentId == right.RightId && p.RightType == 0);
                if (!string.IsNullOrEmpty(CurrentUser.SubordinateSubstation) && CurrentUser.SubordinateSubstation.Contains("上海"))
                {
                    subRight = subRight.Where(p => p.RightId != 212 && p.RightId != 220 && p.RightId != 264 && p.RightId != 219 && p.RightId != 335).ToList().OrderBy(p => p.ShowOrder);
                }
                htmlMenu.AppendFormat(
                    "<li name='{0}' style='z-index:{5}'><a class='" + ((rt != null && rt.Path.ToLower() == right.Path.ToLower()) ? "On" : "") + "' href='{3}'>{1}</a>{2}{4}",
                    right.RightName,
                    title,
                    (subRight.Any() ? "<i></i>" : ""),
                    (subRight.Any() ? "#" : path),
                    (subRight.Any() ? "<ul>" : ""),
                    zindex);
                foreach (var c in subRight.OrderBy(p => p.ShowOrder))
                {
                    var p = c.Path;
                    if (!p.StartsWith("/"))
                    {
                        p = "/" + p;
                    }
                    var t = CodeHelper.GetNavigateString(c.RightName);
                    if (string.IsNullOrEmpty(t))
                    {
                        t = c.RightName;
                    }
                    htmlMenu.AppendFormat("<li><a href='{0}'><span>{1}</span></a></li>", p, t);
                }
                htmlMenu.Append(string.Format("{0}</li>", (subRight.Any() ? "</ul>" : "")));
            }

            if (Session["moduleName"].ToString().ToLower() == "mytrain")
            {
                //如果老员工转成的新员工加一个连接
                if (CurrentUser.IsNew == 1)
                {
                    htmlMenu.AppendFormat("<li name='{0}' style='z-index:{5}'><a class='' href='{3}'>{1}</a>{2}{4}", "新进员工培训专区", "新进员工培训专区", "", "/Common/RedirectUrl?blockName=NewMyTrain", "", 1);
                }
                htmlMenu.AppendFormat("<li name='{0}' style='z-index:{5}'><a class='' href='{3}'>{1}</a>{2}{4}",
                                      "部门分所培训专区", "部门分所培训专区", "", "/Common/RedirectUrl?blockName=MyDepTrain", "", 1);
            }

            ViewBag.MenuStr = htmlMenu.ToString();
            return View();
        }

        /// <summary>
        /// 菜单跳转连接
        /// </summary>
        public void RedirectUrl(string blockName)
        {
            Session["moduleName"] = blockName;
            var redirectUrl = "";
            switch (blockName)
            {
                case "MyTrain":
                    redirectUrl = (CurrentUser.DeptName == "待分配部门" && CurrentUser.IsNew == 1) ? "/Home/NewMyTrainIndex" : "/Home/MyTrainIndex";
                    break;
                case "NewMyTrain":
                    redirectUrl = "/Home/NewMyTrainIndex";
                    break;
                case "MyDepTrain":
                    redirectUrl = "/Home/MyDepIndex";
                    break;
                case "DepManage":
                    redirectUrl = "/Home/DepManageIndex";
                    break;
                case "TrainManage":
                    redirectUrl = "/PlanManage/Index";
                    break;
                case "Knowledge":
                    redirectUrl = "/ReResourceManage/ReMyResourceManage";
                    break;
                case "SystemManage":
                    redirectUrl = "/Home/SystemManageIndex";
                    break;
                case "Announcement":
                    redirectUrl = "/SystemManage/NoteListShow";
                    break;
                case "FAQ":
                    redirectUrl = "/SystemManage/FAQ_NoteListShow";
                    break;
                case "Person":
                    redirectUrl = "/UserManage/UserInfoIndex";
                    break;
                default:
                    redirectUrl = "";
                    break;
            }
            if (redirectUrl == "")
                //return View("ErroMessage");
                Response.Redirect("/Common/ErroMessage");
            Response.Redirect(redirectUrl);
            //return View("");
            //var right = UserRights.FirstOrDefault(p => p.RightName == blockName);
            //if (right == null) { return View("ErroMessage"); }
            //else
            //{
            //    var r = UserRights.Where(p => p.ParentId == right.RightId && p.RightType == 0).OrderBy(p => p.ShowOrder).First();
            //    Response.Redirect("/" + r.Path);
            //    return View("");
            //}
        }

        /// <summary>
        ///     获取当前请求的页面的菜单
        /// </summary>
        /// <param name="urs">权限集合</param>
        /// <param name="url">当前请求的url</param>
        /// <param name="flag">0：总（一级）菜单；1：子（二级）菜单</param>
        /// <returns></returns>
        private Sys_Right GetSubMenu(List<Sys_Right> urs, string url, int flag)
        {
            Sys_Right obj = urs.FirstOrDefault(p => p.Path.ToLower() == url.ToLower());
            if (obj != null)
            {
                Session["moduleName"] = string.IsNullOrEmpty(obj.ModuleName)
                                            ? Session["moduleName"].ToString()
                                            : obj.ModuleName;
                Sys_Right fr = urs.FirstOrDefault(p => p.RightId == obj.ParentId);
                if (obj.RightType == 0 &&
                    (flag == 1 || (flag == 0 && (fr != null && fr.ParentId == 0))))
                    return obj;
                Sys_Right sysRight = urs.FirstOrDefault(p => p.RightId == obj.ParentId);
                if (sysRight != null)
                    return GetSubMenu(urs, sysRight.Path, flag);
            }
            return null;
        }

        /// <summary>
        /// 出错提示
        /// </summary>
        /// <returns></returns>
        public ActionResult ErroMessage()
        {
            return View();
        }

        /// <summary>
        /// 文件上传调用的方法
        /// </summary>
        /// <param name="FileData"></param>
        /// <param name="folder"></param>
        /// <param name="detailFlag">detailFlag : 0 :返回上传后的文件名 1: 返回 "原始文件名,Guid文件名,文件大小"</param>
        /// <returns></returns>    
        public ContentResult UploadFileAction(HttpPostedFileBase FileData, string folder, int detailFlag = 0)
        {
            string filename = "";
            string resultName = "";
            if (null != FileData)
            {
                try
                {
                    filename = Path.GetFileName(FileData.FileName); //获得文件名
                    string fullPathname = Path.Combine(folder, filename);
                    //文件后缀名
                    string suffix = FileData.FileName.Substring(FileData.FileName.LastIndexOf(".") + 1).ToLower();
                    resultName = Guid.NewGuid() + "." + suffix;
                    saveFile(FileData, folder, resultName);
                }
                catch (Exception ex)
                {
                    resultName = ex.ToString();
                }
            }
            if (detailFlag == 1)
            {
                resultName = FileData.FileName + "|" + resultName + "|" + FileData.ContentLength / 1024;
            }
            return Content(resultName);
        }

        public bool saveFile(HttpPostedFileBase postedFile, string filepath, string saveName)
        {
            bool result = false;
            string a = HttpContext.Server.MapPath(filepath);
            if (!Directory.Exists(a))
            {
                Directory.CreateDirectory(a);
            }
            try
            {
                postedFile.SaveAs((a + "\\" + saveName));
                result = true;
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.Message);
            }
            return result;
        }

        /// <summary>
        /// 获取所有培训级别
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAllTrainGrade()
        {
            return Json(AllTrainGrade, JsonRequestBehavior.AllowGet);
        }


        public bool DownLoadFile(string url)
        {
            try
            {
                FileInfo fi = new FileInfo(Server.MapPath(url));//excelFile为文件在服务器上的地址
                var excelName = fi.Name;
                HttpResponse contextResponse = System.Web.HttpContext.Current.Response;
                contextResponse.Redirect(url, false);
                contextResponse.Clear();
                contextResponse.Buffer = true;
                contextResponse.Charset = "utf-8"; //设置了类型为中文防止乱码的出现 
                contextResponse.AppendHeader("Content-Disposition", String.Format("attachment;filename={0}", excelName)); //定义输出文件和文件名 
                contextResponse.AppendHeader("Content-Length", fi.Length.ToString());
                contextResponse.ContentEncoding = System.Text.Encoding.UTF8;
                contextResponse.ContentType = "application/vnd.ms-excel;charset=utf-8";//设置输出文件类型为excel文件。 
                contextResponse.WriteFile(fi.FullName);
                contextResponse.Flush();
                contextResponse.End();
                return true;
            }
            catch
            {
                return false;
            }

        }

        /// <summary>
        /// 查询薪酬级别
        /// </summary>
        /// <returns></returns>
        public JsonResult GetPayGrade(string name, int type)
        {
            string where = " 1=1 ";
            if (!string.IsNullOrEmpty(name))
            {
                where += " and  PayGrade like '%" + name.ReplaceSql() + "%'";
            }

            return Json(new { dataList = cpaBL.GetPayGrade(where, type) }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 查询CPA关系所在地
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public JsonResult GetCPARelationship(string name)
        {
            string where = " 1=1 ";
            if (!string.IsNullOrEmpty(name))
            {
                where += " and  CPARelationship like '%" + name.ReplaceSql() + "%'";
            }

            //var List = cpaBL.GetCPARelationship();
            return Json(new { dataList = cpaBL.GetCPARelationship(where) }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}