using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using LiXinDataAccess.Examination;
using LiXinInterface.Examination;
using LiXinModels.Examination.DBModel;
using LiXinDataAccess.Report_CPA;

namespace LiXinBLL.Examination
{
    public class ExaminationBL : IExamination
    {
        private readonly ExaminationDB _examinationDb;

        public ExaminationBL()
        {
            _examinationDb = new ExaminationDB();
        }

        /// <summary>
        ///     新增考试
        /// </summary>
        /// <param name="exam"></param>
        /// <returns></returns>
        public int AddExamination(tbExamination exam)
        {
            return _examinationDb.Insert(exam);
        }

        /// <summary>
        ///     判断考试名称是否存在
        /// </summary>
        /// <param name="examinationTitle"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsExistExamTitle(string examinationTitle, int id)
        {
            return _examinationDb.IsExsitName<tbExamination>(new Dictionary<string, object>() { { "ExaminationTitle", examinationTitle } }, id);
        }

        /// <summary>
        ///     修改考试
        /// </summary>
        /// <param name="exam"></param>
        /// <returns></returns>
        public bool ModifyExamination(tbExamination exam)
        {
            return _examinationDb.Modify(exam);
        }

        /// <summary>
        ///     获取所有考试
        /// </summary>
        /// <returns></returns>
        public List<tbExamination> GetAllExamination()
        {
            return _examinationDb.GetAllList<tbExamination>(false);
        }

        /// <summary>
        ///     获取指定ID集合的考试
        /// </summary>
        /// <param name="examIDs">考试ID集合</param>
        /// <returns></returns>
        public List<tbExamination> GetAllExamination(IEnumerable<int> examIDs)
        {
            IMongoQuery query = Query.And(Query.EQ("Status", 0), Query.In("_id", new BsonArray(examIDs)));
            return _examinationDb.GetAllList<tbExamination>(query);
        }

        /// <summary>
        ///     获取指定考试   0
        /// </summary>
        /// <returns></returns>
        public tbExamination GetExamination(int id)
        {
            return _examinationDb.GetAllList<tbExamination>(false).FirstOrDefault(e => e._id == id && e.Status == 0);
        }

        #region 考试授权

        /// <summary>
        ///     删除考试授权人员，添加新的授权人员
        /// </summary>
        /// <param name="examinationID"></param>
        /// <param name="userIDs"></param>
        /// <returns></returns>
        public bool SaveExamSendStudent(int examinationID, IEnumerable<int> userIDs)
        {
            _examinationDb.DeleteExamSendStudentWithRelationID(examinationID);

            var exam = _examinationDb.GetSingleByID<tbExamination>(examinationID);
            if (exam == null)
                return false;

            IEnumerable<tbExamSendStudent> examSendStudents = userIDs.Select(p => new tbExamSendStudent
                {
                    DoExamStatus = 0,
                    ExamPaperID = exam.PaperID,
                    IsPass = 0,
                    LastUpdateTime = DateTime.Now,
                    RelationID = examinationID,
                    SourceType = 0,
                    Status = 0,
                    SubmitTime = DateTime.Now,
                    TestTimes = 0,
                    UserID = p
                });
            return _examinationDb.Insert(examSendStudents);
        }

        /// <summary>
        ///     得到该考试所有学生
        /// </summary>
        /// <param name="examId"></param>
        /// <returns></returns>
        public List<tbExamSendStudent> GettbExamSendAllStudent(int examId)
        {
            return
                _examinationDb.GetAllList<tbExamSendStudent>(false)
                              .Where(p => p.RelationID == examId && p.SourceType == 0)
                              .ToList();
        }
        /// <summary>
        /// 根据课程ID和学院ID 获取记录
        /// </summary>
        /// <param name="relationId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public tbExamSendStudent GetSingletbExamSendStudentByCourseIdAndUserId(int relationId, int UserId, int SourceType)
        {
            return _examinationDb.GetExamSendStudentWithByCourseIdAndUserId(relationId, UserId, SourceType);
        }

        /// <summary>
        ///     得到该考试未开始考试的学生
        /// </summary>
        /// <param name="examId"></param>
        /// <returns></returns>
        public List<tbExamSendStudent> GettbExamSendStudent(int examId)
        {
            return
                _examinationDb.GetAllList<tbExamSendStudent>(false)
                              .Where(p => p.DoExamStatus == 0 && p.RelationID == examId && p.SourceType == 0)
                              .ToList();
        }

        #endregion

        #region 考试复评

        /// <summary>
        ///     根据考试ID  获取已经提交考试的（考生关联考试）列表
        /// </summary>
        /// <param name="examId"></param>
        /// <returns></returns>
        public List<tbExamSendStudent> GetAllStudentByExamId(int examId)
        {
            return
                _examinationDb.GetAllList<tbExamSendStudent>(false)
                              .Where(p => p.DoExamStatus == 2 && p.RelationID == examId && p.SourceType == 0)
                              .ToList();
        }

        /// <summary>
        ///     根据 考生关联考试ID  获取考生答题详情
        /// </summary>
        /// <returns></returns>
        public tbExamSendStudent GetExamSendStudent(int id)
        {
            return _examinationDb.GetAllList<tbExamSendStudent>(false).Where(p => p._id == id).FirstOrDefault();
        }



        /// <summary>
        ///     修改 考试关联考生实体    2012-8-29 13:49:30【复评成绩时用】
        /// </summary>
        /// <param name="examSendStudent"></param>
        /// <returns></returns>
        public bool ModifyExamSendStudent(tbExamSendStudent examSendStudent)
        {
            return _examinationDb.Modify(examSendStudent);
        }

        #endregion



        /// <summary>
        /// 课程下考试添加记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int InserttbExamSendStudent(tbExamSendStudent model)
        {
            return _examinationDb.Insert(model);

        }

        public bool DeleteExamSendStudentWithByCourseIdAndUserId(int relationId, int UserId, int SourceType)
        {
            return _examinationDb.DeleteExamSendStudentWithByCourseIdAndUserId(relationId, UserId, SourceType);
        }

        public List<tbExamSendStudent> GetExamSendStudentListWithCourseId(int courseId)
        {
            return _examinationDb.GetExamSendStudentListWithCourseId(courseId);
        }

        public int UpdateNumber(int tbexamsendstudentid, int number)
        {
            return _examinationDb.UpdateNumber(tbexamsendstudentid, number);
        }
        public int UpdateTestTimes(int tbexamsendstudentid, int TestTimes)
        {
            return _examinationDb.UpdateTestTimes(tbexamsendstudentid, TestTimes);
        }

        /// <summary>
        /// 根据部门分所课程ID和试卷ID获取在线考试人员列表（已考过的人员）
        /// </summary>
        /// <param name="depCourseId">部门分所课程ID</param>
        /// <param name="examPaperId">试卷ID</param>
        /// <returns></returns>
        public List<tbExamSendStudent> GetExamSendStudentByDeptSelfList(int depCourseId, int examPaperId)
        {
            return _examinationDb.GetExamSendStudentByDeptSelfList(depCourseId, examPaperId);
        }

        public List<tbExamSendStudent> GetALLExamByOneTwoFive()
        {
            return _examinationDb.GetALLExamByOneTwoFive();
        }

        /// <summary>
        /// 统计个人考试通过次数 包括 集中 视频 部门学习
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public List<tbExamSendStudent> GetPassOnlineNum(string year, int userid)
        {

            var listcourse = (new ReportCPADB()).GetCourseIDInYear("  YearPlan=" + year + " and isdelete=0 and Publishflag=1");
            var query = Query.And(Query.In("SourceType", new BsonArray(new List<int>() { 1, 2 })), Query.EQ("Number", 1), Query.EQ("UserID", userid));

            var list = _examinationDb.GetAllList<tbExamSendStudent>(query, 1, 10, "RelationID");
            return list.Where(p => listcourse.Contains(p.RelationID)).ToList();

        }

        public List<tbExamSendStudent> GetPassOnlineNumForDep(string year, int userid)
        {
            var listcourse = (new ReportCPADB()).GetCourseIDInYearForDep("  YearPlan=" + year + " and isdelete=0 and Publishflag=1");
            var query = Query.And(Query.In("SourceType", new BsonArray(new List<int>() {5})), Query.EQ("Number", 1), Query.EQ("UserID", userid));

            var list = _examinationDb.GetAllList<tbExamSendStudent>(query, 1, 10, "RelationID");
            return list.Where(p => listcourse.Contains(p.RelationID)).ToList();
        }


            
    }
}