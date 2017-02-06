using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinDataAccess.CourseLearn;
using LiXinDataAccess.Examination;
using LiXinModels.CourseLearn;
using LiXinInterface.CourseLearn;
using LiXinModels.CourseManage;
using LiXinModels.Examination.DBModel;

namespace LiXinBLL.CourseLearn
{
    public class Cl_AttendceBL : ICl_Attendce
    {
        private Cl_AttendceDB attendceDb;

        private readonly ExaminationDB _ExamDB;
        private readonly ExampaperDB _paperDB;


         public Cl_AttendceBL()
        {
            attendceDb = new Cl_AttendceDB();
            _ExamDB = new ExaminationDB();
            _paperDB = new ExampaperDB();
        }

         public Cl_Attendce GetCl_Attendce(int CourseId, int UserId)
        {
            return attendceDb.GetCl_Attendce(CourseId, UserId);
        }

        public List<Cl_Attendce> GetList(string where = "1=1")
        {
            return attendceDb.GetList(where);
        }

        public List<Cl_Attendce> GetListByTeacher(out int totalCount, string courseId, int startIndex = 0, int pageLength = int.MaxValue)
        {
            return attendceDb.GetListByTeacher(out totalCount, courseId, startIndex, pageLength);
        }


        public List<Co_CourseShow> GetMyExamList(out int totalCount, int UserId = 0, string TrainGrade = "", int DeptId = 0, string where = " 1 = 1 ",
                                               int startIndex = 0,
                                               int pageLength = int.MaxValue)
        {
            var list = attendceDb.GetMyExamList(out totalCount, UserId, TrainGrade, DeptId, where, startIndex, pageLength);
            //totalCount = list.Count > 0 ? list[0].totalcount : 0;
            for (int i = 0; i < list.Count; i++)
            {
                tbExamSendStudent exam = new tbExamSendStudent();
                //2013-9-26 一期的考试里涵盖集中和视频考试 不能用type来标识
                if (list[i].Way == 1)
                {
                    exam = _ExamDB.GetExamSendStudentBySQL2008(list[i].Id, UserId, list[i].CoPaperID, list[i].PaperId, 1);
                }
                else if (list[i].Way == 2)
                {
                    exam = _ExamDB.GetExamSendStudentBySQL2008(list[i].Id, UserId, list[i].CoPaperID, list[i].PaperId, 2);
                }
                //tbExamSendStudent exam = _ExamDB.GetExamSendStudentBySQL2005(list[i].Id,UserId, list[i].CoPaperID, list[i].PaperId);
                //tbExamSendStudent exam = _ExamDB.GetExamSendStudentBySQL2008(list[i].Id, UserId, list[i].CoPaperID, list[i].PaperId,2);
                var exampaper = _paperDB.GetExampaper(list[i].PaperId);
                list[i].LevelScoreStr = Convert.ToInt32(list[i].LevelScore * 0.01 * (exampaper==null?0:exampaper.ExampaperScore));
                list[i].ExampaperScore = exampaper == null ? 0 : exampaper.ExampaperScore;
                //var CoCoursePaper = ICoCoursePaperBL.GetCo_CourseMainPaper(

                //((double)examUser.StudentAnswerList.Sum(p => p.GetScore) / (double)exampaper.ExampaperScore) * 100
                if (exam != null)
                {
                    //var exampaper = _paperDB.GetExampaper(exam.ExamPaperID);
                    //根据得分和试卷总分算出百分比
                    //double sum = 8 / 11;
                    //list[i].GetexamScore = (exam.StudentAnswerList.Sum(pa => pa.GetScore) / exampaper.ExampaperScore) * 100;
                    list[i].GetexamScore = exam.StudentAnswerList.Sum(pa => pa.GetScore);
                    list[i].ExamTestTimes = exam.TestTimes;  //记录考试次数,考了几次
                    //tbExampaper paper = _paperDB.GetExampaper(list[i].PaperId);
                    //list[i].ExamScore = paper.ExampaperScore;
                }
                else
                {
                    list[i].GetexamScore = 0;
                    //list[i].ExamScore
                }

            }
            return list;


        }

        public int GetMyExamListPassCount(int UserId = 0, string TrainGrade = "", int DeptId = 0, string where = " 1 = 1 ")
                                    
        
        {
            return attendceDb.GetMyExamListPassCount(UserId, TrainGrade, DeptId, where);
        }


        public List<Co_CourseShow> GetMyAttendcsList(out int totalCount,int UserId, string where = " 1 = 1 ",
                                               int startIndex = 0,
                                               int pageLength = int.MaxValue,int falg=0)
        {

            return attendceDb.GetMyAttendcsList(out totalCount,UserId, where, startIndex, pageLength,falg);
        }



        public List<Co_CourseShow> GetMyproposeList(out int totalCount,int UserId, string where = " 1 = 1 ",string num=" 1=1", int startIndex = 0,
                                                    int pageLength = int.MaxValue)
        {
            return attendceDb.GetMyproposeList(out totalCount,UserId, where,num, startIndex, pageLength);
        }


        public List<Co_CourseShow> GetMyAfterCourseList(out int totalCount, int UserId, string TrainGrade = "", int DeptId = 0, string where = " 1 = 1 ", string ALLnum = "", int startIndex = 0,
                                                    int pageLength = int.MaxValue)
        {
            //return attendceDb.GetMyAfterCourseList(out totalCount,UserId,TrainGrade,DeptId, where,ALLnum, startIndex, pageLength);
            return attendceDb.GetMyAfterCourseList(out totalCount, UserId, TrainGrade, DeptId, where, ALLnum, startIndex,
                                                   pageLength);
        }


       
        
    }
}
