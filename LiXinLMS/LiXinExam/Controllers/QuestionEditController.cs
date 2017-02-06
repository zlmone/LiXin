using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LiXinCommon;
using LiXinLanguage;
using LiXinModels.Examination.DBModel;
using System.Threading;

namespace LiXinExam.Controllers
{
    public partial class QuestionController
    {
        #region 页面呈现

        /// <summary>
        ///     问题编辑页面呈现
        /// </summary>
        public ActionResult QuestionEdit()
        {
            Response.Expires = 0;
            int id = Convert.ToInt32(Request.QueryString["id"]);
            if (id > 0)
            {
                tbQuestion qu = EQuestionBL.GetSingleByID(id);
                ViewData["BaseInfor"] = qu;
                ViewData["fatherModel"] = ESortBL.GetAllQuestionSortDictionary()[qu.QuestionSortID].Title;
            }
            else
            {
                ViewData["BaseInfor"] = new tbQuestion();
                if (Request.QueryString["sortID"] != null)
                {
                    ViewData["fatherModel"] = Request.QueryString["sortID"] == "0"
                                                  ? "无"
                                                  : ESortBL.GetAllQuestionSortDictionary()[
                                                      Convert.ToInt32(Request.QueryString["sortID"])].Title;
                }
                else
                    ViewData["fatherModel"] = "无";
            }
            return View();
        }

        /// <summary>
        ///     问答题编辑页面呈现
        /// </summary>
        public ActionResult QuestionSubjectEdit()
        {
            Response.Expires = 0;
            int id = Convert.ToInt32(Request.QueryString["id"]);
            if (id > 0)
            {
                tbQuestion qu = EQuestionBL.GetSingleByID(id);
                ViewData["BaseInfor"] = qu;
                ViewData["QuestionInfor"] = qu.QuestionAnswer.OrderByDescending(p => p.Order).ToList();
            }
            else
            {
                ViewData["BaseInfor"] = new tbQuestion();
                ViewData["QuestionInfor"] = new List<QuestionAnswer>();
            }
            return View();
        }

        /// <summary>
        ///     单选题编辑页面呈现
        /// </summary>
        public ActionResult QuestionSingleEdit()
        {
            Response.Expires = 0;
            int id = Convert.ToInt32(Request.QueryString["id"]);
            if (id > 0)
            {
                tbQuestion qu = EQuestionBL.GetSingleByID(id);
                ViewData["BaseInfor"] = qu;
                ViewData["QuestionInfor"] = qu.QuestionAnswer.OrderByDescending(p => p.Order).ToList();
            }
            else
            {
                ViewData["BaseInfor"] = new tbQuestion();
                ViewData["QuestionInfor"] = new List<QuestionAnswer>();
            }
            return View();
        }

        /// <summary>
        ///     多选题编辑页面呈现
        /// </summary>
        public ActionResult QuestionMultipleEdit()
        {
            Response.Expires = 0;
            int id = Convert.ToInt32(Request.QueryString["id"]);
            if (id > 0)
            {
                tbQuestion qu = EQuestionBL.GetSingleByID(id);
                ViewData["BaseInfor"] = qu;
                ViewData["QuestionInfor"] = qu.QuestionAnswer.OrderByDescending(p => p.Order).ToList();
            }
            else
            {
                ViewData["BaseInfor"] = new tbQuestion();
                ViewData["QuestionInfor"] = new List<QuestionAnswer>();
            }
            return View();
        }

        /// <summary>
        ///     填空题编辑页面呈现
        /// </summary>
        public ActionResult QuestionFillblankEdit()
        {
            Response.Expires = 0;
            int id = Convert.ToInt32(Request.QueryString["id"]);
            if (id > 0)
            {
                tbQuestion qu = EQuestionBL.GetSingleByID(id);
                ViewData["BaseInfor"] = qu;
                ViewData["QuestionInfor"] = qu.QuestionAnswer.OrderByDescending(p => p.Order).ToList();
            }
            else
            {
                ViewData["BaseInfor"] = new tbQuestion();
                ViewData["QuestionInfor"] = new List<QuestionAnswer>();
            }
            return View();
        }

        /// <summary>
        ///     判断题编辑页面呈现
        /// </summary>
        public ActionResult QuestionJudgeEdit()
        {
            Response.Expires = 0;
            int id = Convert.ToInt32(Request.QueryString["id"]);
            if (id > 0)
            {
                tbQuestion qu = EQuestionBL.GetSingleByID(id);
                ViewData["BaseInfor"] = qu;
                ViewData["QuestionInfor"] = qu.QuestionAnswer.OrderByDescending(p => p.AnswerFlag).ToList();
            }
            else
            {
                ViewData["BaseInfor"] = new tbQuestion();
                ViewData["QuestionInfor"] = new List<QuestionAnswer>();
            }
            return View();
        }

        /// <summary>
        ///     情景题编辑页面呈现
        /// </summary>
        public ActionResult QuestionMultimediaEdit()
        {
            Response.Expires = 0;
            int id = Convert.ToInt32(Request.QueryString["id"]);
            if (id > 0)
            {
                tbQuestion qu = EQuestionBL.GetSingleByID(id);
                ViewData["BaseInfor"] = qu;
                ViewData["QuestionInfor"] = qu.QuestionAnswer.OrderByDescending(p => p.Order).ToList();
            }
            else
            {
                ViewData["BaseInfor"] = new tbQuestion();
                ViewData["QuestionInfor"] = new List<QuestionAnswer>();
            }
            return View();
        }

        #endregion

        #region 保存操作问题

        /// <summary>
        ///     判断附件的状态
        /// </summary>
        private tbQuestion UpdateQuestionFiles(tbQuestion qu, IEnumerable<FileUpload> files)
        {
            List<string> noUpdateFile = Request.Form["noUpdateFile"] == ""
                                            ? new List<string>()
                                            : Request.Form["noUpdateFile"].Split(';').ToList();
            List<FileUpload> newfiles = qu.FileUpload.Where(file => noUpdateFile.Contains(file.FileName)).ToList();
            foreach (FileUpload f in files.Where(f => newfiles.All(p => p.FileName != f.FileName)))
            {
                newfiles.Add(f);
            }
            qu.FileUpload = newfiles;
            return qu;
        }

        /// <summary>
        ///     获取上传的文件，返回list集合
        /// </summary>
        /// <param name="type">0：图片，1：音频，2：视频</param>
        /// <returns></returns>
        public List<FileUpload> FileUpload(int type)
        {
            var flv = new List<string> { ".avi", ".rmvb", ".wmv", ".flv" };

            var listfileUpload = new List<FileUpload>();

            HttpFileCollectionBase files = Request.Files;
            for (int i = 0; i < files.Count; i++)
            {
                var fileUpload = new FileUpload();
                string fileName = Path.GetFileName(files[i].FileName);
                string fileExten = Path.GetExtension(fileName); //扩展名

                if (type != 2 || (type == 2 && flv.Contains(fileExten.ToLower())))
                {
                    var SaveFileName = DateTime.Now.ToString("yyyymmddhhmmss") +
                                          (new Random().Next(10, 99).ToString()) + i.ToString();

                    var oldName = SaveFileName + fileExten;
                    var converName = SaveFileName + ".flv";
                    fileUpload.FileName = oldName;
                    if (type == 2)
                    {
                        fileUpload.FileName = converName;
                    }

                    //保存的文件名，防止名称冲突
                    fileUpload.FileType = type;
                    fileUpload.RealName = fileName;
                    listfileUpload.Add(fileUpload);

                    if (!Directory.Exists(Server.MapPath("../ClientBin/UploadFile")))
                    {
                        Directory.CreateDirectory(Server.MapPath("../ClientBin/UploadFile"));
                    }

                    string path = "../ClientBin/UploadFile/";

                    if (type == 2)
                    {

                        files[i].SaveAs(Server.MapPath(path + oldName));
                        if (fileExten.ToLower() != ".flv")
                        {
                           var mc= new MediaConvert(oldName, converName);
                           Thread th = new Thread(new ThreadStart(mc.ConvertMedia));
                           th.Start();
                        }

                    }
                    else
                    {
                        files[i].SaveAs(Server.MapPath(path + fileUpload.FileName));
                    }
                }
            }
            return listfileUpload;
        }
        

        /// <summary>
        ///     提交保存问题
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        public JsonResult SubmitQuestion()
        {
            string content = Request.Form["hiddenQuestionContent"].NoHtml();
            if (content.Replace("&nbsp;", "").Trim() == "")
            {
                return Json(new
                    {
                        result = 0,
                        content = "请输入试题题干",
                    }, "text/html", JsonRequestBehavior.AllowGet);
            }

            //ID
            int id = Convert.ToInt32(Request.QueryString["id"]);
            //类型
            int qType = Convert.ToInt32(Request.Form["hiddenSelQuestionType"]);
            int newid = 0;
            bool flag = true;
            int fileType = Convert.ToInt32(Request.QueryString["type"]); //0：图片，1：音频，2：视频
            if (qType == 6)
            {
                HttpFileCollectionBase files = Request.Files;
                for (int i = 0; i < files.Count; i++)
                {
                    if (((fileType != 0 && files[i].ContentLength > 8388608) ||
                         (fileType == 0 && files[i].ContentLength > 512000)) && flag)
                    {
                        flag = false;
                        break;
                    }
                }
            }
            int questionType = Convert.ToInt32(Request.Form["hiddenSelQuestionType"]);

            if (flag)
            {
                List<FileUpload> listfileUpload = qType == 6 ? FileUpload(fileType) : new List<FileUpload>();
                tbQuestion question = id > 0
                                          ? EQuestionBL.GetSingleByID(id)
                                          : new tbQuestion
                                              {
                                                  Status = 0,
                                                  LastUpdateTime = DateTime.Now,
                                                  _id = id,
                                                  UserID = Session["userID"] == null ? 0 : CurrentUser.UserId,
                                                  FileUpload = listfileUpload,
                                                  CreateTime = DateTime.Now
                                              };
                question.QuestionAnswerKeys = Request.Form["txtQuestionAnswerKeys"] ?? "";
                question.QuestionLevel = Convert.ToInt32(Request.Form["hiddenSelQuestionLevel"]);
                question.QuestionSortID = Convert.ToInt32(Request.Form["hiddenSelQuestionSort"]);
                question.QuestionType = questionType;
                question.QuestionAnalysis = Request.Form["txtQuestionAnalysis"] ?? "";
                question.QuestionKey = Request.Form["hiddenSelQuestionKey"].StringToInt32();
                question.QuestionAnswer = new List<QuestionAnswer>();


                if (qType == 6 && listfileUpload.Count > 0)
                    question = UpdateQuestionFiles(question, listfileUpload);

                switch (qType)
                {
                    case 1: //主观题
                        newid = SubmitSubjectQuestion(id, question);
                        break;
                    case 2: //单选题
                        newid = SubmitSingleQuestion(id, question);
                        break;
                    case 3: //多选题
                        newid = SubmitMultipeQuestion(id, question);
                        break;
                    case 4: //判断题
                        newid = SubmitJudgeQuestion(id, question);
                        break;
                    case 5: //填空题
                        newid = SubmitFillblankQuestion(id, question);
                        break;
                    case 6: //情景题
                        newid = SubmitMultimediaQuestion(id, question);
                        break;
                }

                if (newid > 0)

                    return Json(new
                        {
                            result = 1,
                            questionID = newid,
                            content = CommonLanguage.Common_AddSuccess,
                        }, "text/html", JsonRequestBehavior.AllowGet);
                return Json(new
                    {
                        result = 0,
                        content = CommonLanguage.Common_AddFailed,
                        url = ""
                    }, "text/html", JsonRequestBehavior.AllowGet);
            }
            return Json(new
                {
                    result = -1,
                    content = CommonLanguage.Common_Tip_UpLoadFileLimit,
                }, "text/html", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     主观题保存
        /// </summary>
        [ValidateInput(false)]
        public int SubmitSubjectQuestion(int qID, tbQuestion question)
        {
            try
            {
                question.QuestionContent = Request.Form["hiddenQuestionContent"];
                question.QuestionAnswer.Add(new QuestionAnswer
                    {
                        Answer = Request.Form["txtQuestionAnswer"],
                        AnswerFlag = 1,
                        AnswerType = 1,
                        Order = 1
                    });
                return qID > 0 ? EQuestionBL.ModifyByID(question) : EQuestionBL.Insert(question);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        ///     填空题保存
        /// </summary>
        public int SubmitFillblankQuestion(int qID, tbQuestion question)
        {
            try
            {
                question.QuestionContent =
                    Request.Form["hiddenQuestionContent"].Replace("（", "(")
                                                         .Replace("（", "(")
                                                         .Replace("（", "(")
                                                         .Replace("）", ")")
                                                         .Replace("）", ")")
                                                         .Replace("）", ")"); //转换成英文括号，用于识别
                question.QuestionAnswer.Add(new QuestionAnswer
                    {
                        Answer = Request.Form["txtQuestionAnswer"],
                        AnswerFlag = 1,
                        AnswerType = 1,
                        Order = 1
                    });
                return qID > 0 ? EQuestionBL.ModifyByID(question) : EQuestionBL.Insert(question);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        ///     单选题保存
        /// </summary>
        public int SubmitSingleQuestion(int qID, tbQuestion question)
        {
            try
            {
                question.QuestionContent = Request.Form["hiddenQuestionContent"];
                string questionAnswer = Request.Form["hiddenQuestionAnswer"];
                if (questionAnswer != "")
                {
                    int count = 1;
                    foreach (
                        var qa in
                            questionAnswer.Split(new[] { "!!%!%!%!!" }, StringSplitOptions.RemoveEmptyEntries)
                                          .Select(
                                              quAnswer =>
                                              quAnswer.Split(new[] { "***!!***" }, StringSplitOptions.RemoveEmptyEntries))
                        )
                    {
                        question.QuestionAnswer.Add(new QuestionAnswer
                            {
                                Answer = qa[0],
                                AnswerFlag = Convert.ToInt32(qa[1]),
                                AnswerType = 2,
                                Order = count
                            });
                        count++;
                    }
                }
                return qID > 0 ? EQuestionBL.ModifyByID(question) : EQuestionBL.Insert(question);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        ///     多选题保存
        /// </summary>
        public int SubmitMultipeQuestion(int qID, tbQuestion question)
        {
            try
            {
                question.QuestionContent = Request.Form["hiddenQuestionContent"];
                string questionAnswer = Request.Form["hiddenQuestionAnswer"];
                if (questionAnswer != "")
                {
                    int count = 1;
                    foreach (
                        var qa in
                            questionAnswer.Split(new[] { "!!%!%!%!!" }, StringSplitOptions.RemoveEmptyEntries)
                                          .Select(
                                              quAnswer =>
                                              quAnswer.Split(new[] { "***!!***" }, StringSplitOptions.RemoveEmptyEntries))
                        )
                    {
                        question.QuestionAnswer.Add(new QuestionAnswer
                            {
                                Answer = qa[0],
                                AnswerFlag = Convert.ToInt32(qa[1]),
                                AnswerType = 3,
                                Order = count
                            });
                        count++;
                    }
                }
                return qID > 0 ? EQuestionBL.ModifyByID(question) : EQuestionBL.Insert(question);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        ///     判断题保存
        /// </summary>
        public int SubmitJudgeQuestion(int qID, tbQuestion question)
        {
            try
            {
                question.QuestionContent = Request.Form["hiddenQuestionContent"];
                question.QuestionAnswer.Add(new QuestionAnswer
                    {
                        Answer = Request.Form["txtQuestionAnswer"], //0:正确答案；1:错误答案
                        AnswerFlag = 1,
                        AnswerType = 5,
                        Order = 1
                    });
                return qID > 0 ? EQuestionBL.ModifyByID(question) : EQuestionBL.Insert(question);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        ///     情景题保存
        /// </summary>
        public int SubmitMultimediaQuestion(int qID, tbQuestion question)
        {
            try
            {
                question.QuestionContent = Request.Form["hiddenQuestionContent"];
                int answerType = Convert.ToInt32(Request.Form["txtQuestionAnswerType"]);
                if (answerType == 0)
                {
                    question.QuestionAnswer.Add(new QuestionAnswer
                        {
                            Answer = Request.Form["hiddenQuestionAnswer"],
                            AnswerFlag = 1,
                            AnswerType = 0,
                            Order = 1
                        });
                }
                else
                {
                    string questionAnswer = Request.Form["hiddenQuestionAnswer"];
                    if (questionAnswer != "")
                    {
                        int count = 1;
                        foreach (
                            var qa in
                                questionAnswer.Split(new[] { "!!%!%!%!!" }, StringSplitOptions.RemoveEmptyEntries)
                                              .Select(
                                                  quAnswer =>
                                                  quAnswer.Split(new[] { "***!!***" },
                                                                 StringSplitOptions.RemoveEmptyEntries)))
                        {
                            question.QuestionAnswer.Add(new QuestionAnswer
                                {
                                    Answer = qa[0],
                                    AnswerFlag = Convert.ToInt32(qa[1]),
                                    AnswerType = answerType,
                                    Order = count
                                });
                            count++;
                        }
                    }
                }
                return qID > 0 ? EQuestionBL.ModifyByID(question) : EQuestionBL.Insert(question);
            }
            catch
            {
                return 0;
            }
        }

        #endregion
    }
}