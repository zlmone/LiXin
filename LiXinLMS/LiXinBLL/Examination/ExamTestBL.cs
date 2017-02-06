using System;
using System.Collections.Generic;
using System.Linq;
using LiXinInterface.CourseManage;
using LiXinInterface.SystemManage;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using LiXinCommon;
using LiXinInterface.Examination;
using LiXinModels.Examination;
using LiXinModels.Examination.DBModel;
using LiXinModels.Examination.ShowModel;
using RippleDataAccess.Examination;

namespace LiXinBLL.Examination
{
    public class ExamTestBL : IExamTest
    {
        protected ExamTestDB Etdb;
        protected ICo_CoursePaper ICoCoursePaperBL;
        protected ICo_Course ICourseBL;
        protected ISys_ParamConfig ISysParamConfigBL;

        public ExamTestBL(ICo_CoursePaper _ICoCoursePaperBL, ICo_Course _ICourseBL, ISys_ParamConfig _ISysParamConfigBL)
        {
            Etdb = new ExamTestDB();
            ICoCoursePaperBL = _ICoCoursePaperBL;
            ICourseBL = _ICourseBL;
            ISysParamConfigBL = _ISysParamConfigBL;

        }

        /// <summary>
        ///     获取试题集合
        /// </summary>
        /// <param name="quIDlist">试题ID集合</param>
        /// <returns></returns>
        public List<tbQuestion> GetQuestionList(IEnumerable<int> quIDlist)
        {
            var arr = new BsonArray();
            quIDlist.ToList().ForEach(p => arr.Add(p));
            return Etdb.GetAllList<tbQuestion>(Query.In("_id", arr));
        }

        /// <summary>
        ///     获取单个考试人员答案
        /// </summary>
        /// <param name="euID">考试人员ID</param>
        /// <returns></returns>
        public tbExamSendStudent GetExamUser(int euID)
        {
            return Etdb.GetSingleByID<tbExamSendStudent>(euID);
        }

        /// <summary>
        ///     保存单个考试人员答案
        /// </summary>
        /// <param name="eu">考试人员信息</param>
        /// <returns></returns>
        public bool SaveExamUser(tbExamSendStudent eu)
        {
            return Etdb.Modify(eu);
        }

        /// <summary>
        ///     获取我的考试
        /// </summary>
        /// <param name="userID">UserID</param>
        /// <returns></returns>
        public List<tbExamSendStudent> GetMyExamList(int userID)
        {
            IMongoQuery query = Query.And(Query.EQ("Status", 0), Query.EQ("UserID", userID), Query.EQ("SourceType", 0));
            return Etdb.GetAllList<tbExamSendStudent>(query);
        }

        /// <summary>
        ///     获取考试下的参考人员
        /// </summary>
        /// <param name="examIDs">考试ID集合</param>
        /// <returns></returns>
        public List<tbExamSendStudent> GetExamSendStudentList(IEnumerable<int> examIDs)
        {
            IMongoQuery query = Query.And(Query.EQ("Status", 0), Query.In("RelationID", new BsonArray(examIDs)),
                                          Query.EQ("SourceType", 0));
            return Etdb.GetAllList<tbExamSendStudent>(query);
        }

        /// <summary>
        ///     获取考试的基本信息
        /// </summary>
        /// <param name="examUserID">考试人员ID</param>
        /// <param name="type">0:独立的考试</param>
        /// <returns></returns>
        public ExamBaseInforShow GetExamBaseInforShow(int examUserID, int type = 0)
        {
            var examBaseInfor = new ExamBaseInforShow();
            var examuser = Etdb.GetSingleByID<tbExamSendStudent>(examUserID); //考生考试信息
            if (type == 0)
            {
                var exam = Etdb.GetSingleByID<tbExamination>(examuser.RelationID);
                examBaseInfor.ExamEndTime = exam.ExamEndTime.ToLocalTime().ToString("yyyy-MM-dd HH:mm");
                examBaseInfor.ExamLength = exam.ExamLength;
                var paper = Etdb.GetSingleByID<tbExampaper>(exam.PaperID);
                if (paper.ExamType == 0)
                {
                    examBaseInfor.ExamScore = exam.PercentFlag == 0 ? 100 : paper.QuestionList.Sum(p => p.QScore);
                }
                else
                {
                    examBaseInfor.ExamScore = exam.PercentFlag == 0 ? 100 : paper.ExampaperScore;
                }
                examBaseInfor.ExamShowWay = exam.ShowType;
                examBaseInfor.ExamStartTime = exam.ExamBeginTime.ToLocalTime().ToString("yyyy-MM-dd HH:mm");
                examBaseInfor.ExamTitle = exam.ExaminationTitle;
                examBaseInfor.PassScore = exam.PercentFlag == 0 ? Convert.ToInt32(exam.PassScore) : Convert.ToInt32(exam.PassScore * examBaseInfor.ExamScore / 100);
                examBaseInfor.PercentScore = exam.PercentFlag;
                examBaseInfor.ResultFlag = exam.PublishResult;
            }
            else
            {
                if (examuser!=null)
                { 

                var coursepaper = ICoCoursePaperBL.GetCo_CourseMainPaper(examuser.RelationID);
                examBaseInfor.ExamLength = coursepaper.Length;

                    var course = ICourseBL.GetCo_Course(examuser.RelationID);
                    var Sys_ParamConfig = ISysParamConfigBL.GetSys_ParamConfig(26);
                //examBaseInfor.ExamScore = coursepaper.TotalScore;
                examBaseInfor.ExamStartTime =course.EndTimeStr;
                examBaseInfor.ExamEndTime = course.EndTime.AddHours(Convert.ToDouble(Sys_ParamConfig.ConfigValue.Split(';')[0])).ToString("yyyy-MM-dd HH:mm");
                //var exam = Etdb.GetSingleByID<tbExamination>(examuser.RelationID);
                var paper = Etdb.GetSingleByID<tbExampaper>(examuser.ExamPaperID);
                examBaseInfor.PassScore =Convert.ToInt32(coursepaper.LevelScore * 0.01 * paper.ExampaperScore);//考试通过分数线
                if (paper.ExamType == 0)
                {
                    examBaseInfor.ExamScore =  paper.QuestionList.Sum(p => p.QScore);
                }
                else
                {
                    examBaseInfor.ExamScore =  paper.ExampaperScore;
                }
                }
            }

           
            return examBaseInfor;
        }

        /// <summary>
        ///     获取考试的基本信息
        /// </summary>
        /// <param name="examUserID">考试人员ID</param>
        /// <param name="type">0:独立的考试</param>
        /// <returns></returns>
        public ExampaperShow GetExampaperBaseInforShow(int examUserID, int type = 0)
        {
            var exampaper = new ExampaperShow(); //new 试卷对象           
            var examuser = Etdb.GetSingleByID<tbExamSendStudent>(examUserID); //单个考生考试信息
            //var exam = new tbExampaper();
            tbExampaper exam = null;
            //var examination = new tbExamination(); //考试分类对象
            tbExamination examination = null;
            exam = Etdb.GetSingleByID<tbExampaper>(examuser.ExamPaperID); //试卷对象
            if (type == 0)
            {
                examination = Etdb.GetSingleByID<tbExamination>(examuser.RelationID);
            }
            exampaper.ExampaperType = exam.ExamType;
            exampaper.QuestionTypeStrShow = exam.QuestionTypeOrder.GetQuestionTypeStr();
            //获取试题
            if (examination != null)
            { 
                GetExampaper(exampaper, exam, examuser, examination);
            }
            else
            {
                GetExampaper(exampaper, exam, examuser);
            }
            //初始化学员答案
            exampaper.QuestionList.ForEach(p => exampaper.StudentAnswer.Add(new StudentAnswer
                {
                    Answer = p.UserAnswer,
                    Qid = p.QuestionID,
                    QType = p.QType
                }));
            return exampaper;
        }

        /// <summary>
        ///     获取我的考试试题
        /// </summary>
        /// <returns></returns>
        public List<tbQuestion> GetMyExaminationQuestionList(int euID)
        {
            var b = new BsonArray(Etdb.GetSingleByID<tbExamSendStudent>(euID).StudentAnswerList.Select(p => p.Qid));
            return Etdb.GetAllList<tbQuestion>(Query.In("_id", b));
        }

        /// <summary>
        ///     获取试卷的试题
        /// </summary>
        /// <param name="exampaper">试卷信息</param>
        /// <param name="exam">试卷信息</param>
        /// <param name="examuser">学员答案</param>
        /// <param name="examination">考试信息</param>
        /// <returns></returns>
        public void GetExampaper(ExampaperShow exampaper, tbExampaper exam, tbExamSendStudent examuser,
                                 tbExamination examination)
        {
            if (exampaper.ExampaperType == 0)
            {
                #region 正常

                IMongoQuery query = Query.In("_id", new BsonArray(exam.QuestionList.Select(p => p.Qid)));
                List<tbQuestion> quList = Etdb.GetAllList<tbQuestion>(query); //获取试题
                exam.QuestionList.ForEach(p =>
                    {
                        tbQuestion qu = quList.FirstOrDefault(q => q._id == p.Qid);
                        if (qu != null)
                        {
                            ReStudentExamAnswer an = examuser == null
                                                         ? null
                                                         : examuser.StudentAnswerList.FirstOrDefault(
                                                             pa => pa.Qid == p.Qid);
                            var newqu = new MQuestion
                                {
                                    QAnswerType = qu.QuestionAnswer[0].AnswerType,
                                    QType = qu.QuestionType,
                                    QuestionContent = qu.QuestionContent,
                                    QuestionID = qu._id,
                                    QuestionLevel = ((QuestionLevel) qu.QuestionLevel).ToString(),
                                    QuestionOrder = p.QOrder,
                                    Score = p.QScore,
                                    UserAnswer = an == null ? "" : (an.Answer),
                                    FillBlankCount =
                                        qu.QuestionContent.Split(new[] {"()"}, StringSplitOptions.None).Count() - 1
                                };
                            qu.QuestionAnswer.OrderBy(o => o.Order)
                              .ToList()
                              .ForEach(pa => newqu.QuestionAnswer.Add(new MQuestionAnswer
                                  {
                                      AnswerContent = pa.Answer,
                                      AnswerFlag = pa.AnswerFlag,
                                      AnswerType = pa.AnswerType,
                                      Order = pa.Order,
                                      QuID = newqu.QuestionID,
                                      QType = newqu.QType
                                  }));
                            qu.FileUpload.ForEach(pa => newqu.FileUpload.Add(new QuestionFile
                                {
                                    _fileName = pa.FileName,
                                    _fileType = pa.FileType,
                                    _realName = pa.RealName
                                }));
                            exampaper.QuestionList.Add(newqu);
                        }
                    });

                #endregion
            }
            else
            {
                #region 随机

                List<tbQuestion> questionList = Etdb.GetAllList<tbQuestion>(Query.EQ("Status", 0));
                exam.QuestionRule.ForEach(p =>
                    {
                        var newqulist = new List<tbQuestion>();
                        foreach (string s in p.QLevelStr.Split(';'))
                        {
                            newqulist.AddRange(
                                questionList.Where(
                                    qu =>
                                    p.QSort == qu.QuestionSortID && p.Qtype == qu.QuestionType &&
                                    s.Split(':')[0].StringToInt32() == qu.QuestionLevel)
                                            .ToList()
                                            .RandomGetSome(s.Split(':')[1].StringToInt32()));
                        }
                        newqulist.ForEach(qu =>
                            {
                                var newqu = new MQuestion
                                    {
                                        QAnswerType = qu.QuestionAnswer[0].AnswerType,
                                        QType = qu.QuestionType,
                                        QuestionContent = qu.QuestionContent,
                                        QuestionID = qu._id,
                                        QuestionLevel = ((QuestionLevel) qu.QuestionLevel).ToString(),
                                        QuestionOrder = 0,
                                        Score = p.QScore,
                                        UserAnswer = "",
                                        FillBlankCount = qu.QuestionContent.Split(new[] { "()" }, StringSplitOptions.None).Count() - 1
                                    };
                                qu.QuestionAnswer.OrderBy(o => o.Order)
                                  .ToList()
                                  .ForEach(pa => newqu.QuestionAnswer.Add(new MQuestionAnswer
                                      {
                                          AnswerContent = pa.Answer,
                                          AnswerFlag = pa.AnswerFlag,
                                          AnswerType = pa.AnswerType,
                                          Order = pa.Order,
                                          QuID = newqu.QuestionID,
                                          QType = newqu.QType
                                      }));
                                qu.FileUpload.ForEach(pa => newqu.FileUpload.Add(new QuestionFile
                                    {
                                        _fileName = pa.FileName,
                                        _fileType = pa.FileType,
                                        _realName = pa.RealName
                                    }));
                                exampaper.QuestionList.Add(newqu);
                            });
                    });

                #endregion
            }
            if (exampaper.ExampaperType == 1 || (examination._id > 0 && examination.RadomOrderFlag == 1))
            //if (exampaper.ExampaperType == 1 )
            {
                //排序
                int order = 1;
                foreach (string s in exam.QuestionTypeOrder.Split(','))
                {
                    exampaper.QuestionList.Where(p => p.QType == s.StringToInt32())
                             .ToList()
                             .RandomListOrder()
                             .ForEach(p =>
                                 {
                                     p.QuestionOrder = order;
                                     order++;
                                 });
                }
            }
            exampaper.QuestionList = exampaper.QuestionList.OrderBy(p => p.QuestionOrder).ToList();
        }

        /// <summary>
        /// 获取试卷的试题
        /// </summary>
        /// <param name="exampaper">试卷信息</param>
        /// <param name="exam">试卷信息</param>
        /// <param name="examuser">学员答案</param>
        public void GetExampaper(ExampaperShow exampaper, tbExampaper exam, tbExamSendStudent examuser)
        {
            if (exampaper.ExampaperType == 0)
            {
                #region 正常

                IMongoQuery query = Query.In("_id", new BsonArray(exam.QuestionList.Select(p => p.Qid)));
                List<tbQuestion> quList = Etdb.GetAllList<tbQuestion>(query); //获取试题
                exam.QuestionList.ForEach(p =>
                {
                    tbQuestion qu = quList.FirstOrDefault(q => q._id == p.Qid);
                    if (qu != null)
                    {
                        ReStudentExamAnswer an = examuser == null
                                                     ? null
                                                     : examuser.StudentAnswerList.FirstOrDefault(
                                                         pa => pa.Qid == p.Qid);
                        var newqu = new MQuestion
                        {
                            QAnswerType = qu.QuestionAnswer[0].AnswerType,
                            QType = qu.QuestionType,
                            QuestionContent = qu.QuestionContent,
                            QuestionID = qu._id,
                            QuestionLevel = ((QuestionLevel)qu.QuestionLevel).ToString(),
                            QuestionOrder = p.QOrder,
                            Score = p.QScore,
                            UserAnswer = an == null ? "" : (an.Answer),
                            FillBlankCount =
                                qu.QuestionContent.Split(new[] { "()" }, StringSplitOptions.None).Count() - 1
                        };
                        qu.QuestionAnswer.OrderBy(o => o.Order)
                          .ToList()
                          .ForEach(pa => newqu.QuestionAnswer.Add(new MQuestionAnswer
                          {
                              AnswerContent = pa.Answer,
                              AnswerFlag = pa.AnswerFlag,
                              AnswerType = pa.AnswerType,
                              Order = pa.Order,
                              QuID = newqu.QuestionID,
                              QType = newqu.QType
                          }));
                        qu.FileUpload.ForEach(pa => newqu.FileUpload.Add(new QuestionFile
                        {
                            _fileName = pa.FileName,
                            _fileType = pa.FileType,
                            _realName = pa.RealName
                        }));
                        exampaper.QuestionList.Add(newqu);
                    }
                });

                #endregion
            }
            else
            {
                #region 随机

                List<tbQuestion> questionList = Etdb.GetAllList<tbQuestion>(Query.EQ("Status", 0));
                exam.QuestionRule.ForEach(p =>
                {
                    var newqulist = new List<tbQuestion>();
                    foreach (string s in p.QLevelStr.Split(';'))
                    {
                        newqulist.AddRange(
                            questionList.Where(
                                qu =>
                                p.QSort == qu.QuestionSortID && p.Qtype == qu.QuestionType &&
                                s.Split(':')[0].StringToInt32() == qu.QuestionLevel)
                                        .ToList()
                                        .RandomGetSome(s.Split(':')[1].StringToInt32()));
                    }
                    newqulist.ForEach(qu =>
                    {
                        var newqu = new MQuestion
                        {
                            QAnswerType = qu.QuestionAnswer[0].AnswerType,
                            QType = qu.QuestionType,
                            QuestionContent = qu.QuestionContent,
                            QuestionID = qu._id,
                            QuestionLevel = ((QuestionLevel)qu.QuestionLevel).ToString(),
                            QuestionOrder = 0,
                            Score = p.QScore,
                            UserAnswer = "",
                            FillBlankCount = qu.QuestionContent.Split(new[] { "()" }, StringSplitOptions.None).Count() - 1
                        };
                        qu.QuestionAnswer.OrderBy(o => o.Order)
                          .ToList()
                          .ForEach(pa => newqu.QuestionAnswer.Add(new MQuestionAnswer
                          {
                              AnswerContent = pa.Answer,
                              AnswerFlag = pa.AnswerFlag,
                              AnswerType = pa.AnswerType,
                              Order = pa.Order,
                              QuID = newqu.QuestionID,
                              QType = newqu.QType
                          }));
                        qu.FileUpload.ForEach(pa => newqu.FileUpload.Add(new QuestionFile
                        {
                            _fileName = pa.FileName,
                            _fileType = pa.FileType,
                            _realName = pa.RealName
                        }));
                        exampaper.QuestionList.Add(newqu);
                    });
                });

                #endregion
            }
            //if (exampaper.ExampaperType == 1 || (examination._id > 0 && examination.RadomOrderFlag == 1))
            if (exampaper.ExampaperType == 1 )
            {
                //排序
                int order = 1;
                foreach (string s in exam.QuestionTypeOrder.Split(','))
                {
                    exampaper.QuestionList.Where(p => p.QType == s.StringToInt32())
                             .ToList()
                             .RandomListOrder()
                             .ForEach(p =>
                             {
                                 p.QuestionOrder = order;
                                 order++;
                             });
                }
            }
            exampaper.QuestionList = exampaper.QuestionList.OrderBy(p => p.QuestionOrder).ToList();
        }
    }

    public static class QuExtnedMethod
    {
        /// <summary>
        ///     获取试卷所含的试题类型集合字符串
        /// </summary>
        /// <param name="str">1,2,3……</param>
        /// <returns></returns>
        public static string GetQuestionTypeStr(this string str)
        {
            string s = "";
            foreach (string t in str.Split(','))
            {
                switch (t)
                {
                    case "1":
                        s += s == "" ? ("问答题") : (",问答题");
                        break;
                    case "2":
                        s += s == "" ? ("单选题") : (",单选题");
                        break;
                    case "3":
                        s += s == "" ? ("多选题") : (",多选题");
                        break;
                    case "4":
                        s += s == "" ? ("判断题") : (",判断题");
                        break;
                    case "5":
                        s += s == "" ? ("填空题") : (",填空题");
                        break;
                    case "6":
                        s += s == "" ? ("情景题") : (",情景题");
                        break;
                }
            }
            return s;
        }
    }
}