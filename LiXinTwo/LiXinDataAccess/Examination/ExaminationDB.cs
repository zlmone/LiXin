using System.Collections.Generic;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using LiXinModels.Examination.DBModel;
using MongoDB.Bson;

namespace LiXinDataAccess.Examination
{
    public class ExaminationDB : BaseMethod
    {
        /// <summary>
        ///     删除ExamSendStudent表中一个考试的所有人信息（未考的）
        /// </summary>
        /// <param name="relationID"></param>
        /// <returns></returns>
        public bool DeleteExamSendStudentWithRelationID(int relationID)
        {
            return Delete<tbExamSendStudent>(Query.And(new[]
                {
                    Query.EQ("RelationID", relationID),
                    Query.EQ("SourceType", 0),
                    Query.EQ("DoExamStatus", 0),
                    Query.EQ("Status", 0)
                }), false);
        }

        /// <summary>
        /// 当SourceType=1，在根绝RelationID和UserId删除  SourceType:1是课程2是视频
        /// </summary>
        /// <param name="relationId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public bool DeleteExamSendStudentWithByCourseIdAndUserId(int relationId, int UserId, int SourceType)
        {
            return Delete<tbExamSendStudent>(Query.And(new[]
                {
                    Query.EQ("RelationID", relationId),
                    Query.EQ("UserID",UserId),
                    Query.EQ("SourceType", SourceType),
                    //Query.EQ("DoExamStatus", 0),
                    //Query.EQ("Status", 0)
                }), true);
        }

        /// <summary>
        /// 修改课程是否获得学时 0:没有获得  1:获得  获得后后面的考试就不在获得学时 标志位
        /// </summary>
        /// <param name="tbexamsendstudentid">id</param>
        /// <param name="number">0:1</param>
        /// <returns></returns>
        public int UpdateNumber(int tbexamsendstudentid, int number)
        {
            try
            {
                MongoCollection<tbExamSendStudent> coll =
                    BaseDB.CreateInstance().GetCollection<tbExamSendStudent>("tbExamSendStudent");
                //var update = new UpdateDocument
                //    {
                //        {"Number", number}
                //    };
                //coll.Update(Query.EQ("_id", tbexamsendstudentid), update,UpdateFlags.Multi);
                //修改单条数据
                coll.Update(Query.EQ("_id", tbexamsendstudentid), Update.Set("Number", number));
                return tbexamsendstudentid;
            }
            catch
            {
                return 0;
            }
            finally
            {
                BaseDB.MongoService.Disconnect();
            }
        }



        public int UpdateTestTimes(int tbexamsendstudentid, int TestTimes)
        {
            try
            {
                MongoCollection<tbExamSendStudent> coll =
                    BaseDB.CreateInstance().GetCollection<tbExamSendStudent>("tbExamSendStudent");
                //var update = new UpdateDocument
                //    {
                //        {"Number", number}
                //    };
                //coll.Update(Query.EQ("_id", tbexamsendstudentid), update,UpdateFlags.Multi);,

                //修改单条数据
                coll.Update(Query.EQ("_id", tbexamsendstudentid), Update.Set("PaperScore", 0));
                coll.Update(Query.EQ("_id", tbexamsendstudentid), Update.Set("TestTimes", TestTimes));
                coll.Update(Query.EQ("_id", tbexamsendstudentid), Update.Set("StudentAnswerList", ""));

                //var update = new UpdateDocument
                //      {
                //          {"PaperScore",  0},
                //          {"TestTimes", TestTimes},
                //          {"Number",0}

                //      };
                //coll.Update(Query.EQ("_id", tbexamsendstudentid), update);

                return tbexamsendstudentid;
            }
            catch
            {
                return 0;
            }
            finally
            {
                BaseDB.MongoService.Disconnect();
            }
        }



        /// <summary>
        /// 根据课程ID和学员ID 获取一条数据
        /// </summary>
        /// <param name="relationId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public tbExamSendStudent GetExamSendStudentWithByCourseIdAndUserId(int relationId, int UserId, int SourceType)
        {

            try
            {
                MongoCollection<tbExamSendStudent> coll = BaseDB.CreateInstance().GetCollection<tbExamSendStudent>("tbExamSendStudent");

                return coll.FindOne(Query.And(new[] { Query.EQ("RelationID", relationId), Query.EQ("SourceType", SourceType), Query.EQ("UserID", UserId) }));
            }
            catch
            {
                return new tbExamSendStudent();
            }
            finally
            {
                BaseDB.MongoService.Disconnect();
            }
        }





        /// <summary>
        /// 根据课程ID和学员ID 获取一条数据
        /// </summary>
        /// <param name="relationId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public tbExamSendStudent GetExamSendStudentBySQL2005(int relationId, int UserId, int CoursePaperId, int ExamPaperID)
        {

            try
            {
                MongoCollection<tbExamSendStudent> coll = BaseDB.CreateInstance().GetCollection<tbExamSendStudent>("tbExamSendStudent");

                return coll.FindOne(Query.And(new[] { Query.EQ("RelationID", relationId), Query.EQ("UserID", UserId), Query.EQ("CoursePaperId", CoursePaperId), Query.EQ("ExamPaperID", ExamPaperID) }));
            }
            catch
            {
                return new tbExamSendStudent();
            }
            finally
            {
                BaseDB.MongoService.Disconnect();
            }
        }
        
        /// <summary>
        /// 根据课程ID和学员ID 获取一条数据
        /// </summary>
        /// <param name="relationId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public tbExamSendStudent GetExamSendStudentBySQL2008(int relationId, int UserId, int CoursePaperId, int ExamPaperID, int SourceType)
        {

            try
            {
                MongoCollection<tbExamSendStudent> coll = BaseDB.CreateInstance().GetCollection<tbExamSendStudent>("tbExamSendStudent");

                return coll.FindOne(Query.And(new[] { Query.EQ("RelationID", relationId), Query.EQ("SourceType", SourceType), Query.EQ("UserID", UserId), Query.EQ("CoursePaperId", CoursePaperId), Query.EQ("ExamPaperID", ExamPaperID) }));
            }
            catch
            {
                return new tbExamSendStudent();
            }
            finally
            {
                BaseDB.MongoService.Disconnect();
            }
        }

        /// <summary>
        /// 查询课程下试卷所有考试人员数据
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public List<tbExamSendStudent> GetExamSendStudentListWithCourseId(int courseId)
        {
            return GetAllList<tbExamSendStudent>(Query.And(new[]
                {
                    Query.EQ("RelationID", courseId),
                    Query.EQ("SourceType", 1)
                }));
        }


        public List<tbExamination> GetPublishExam()
        {
            return GetAllList<tbExamination>(Query.And(new[]
                {
                    Query.EQ("PublishResult", 1),
                    Query.EQ("Status", 0)
                }));
        }

        public List<tbExamSendStudent> GettbExamSendStudentWithExamId(int ExaminationId)
        {
            return GetAllList<tbExamSendStudent>(Query.And(new[]
                {
                    Query.EQ("RelationID", ExaminationId),
                    Query.EQ("SourceType", 0),
                    Query.EQ("Status", 0)
                }));
        }

        public List<tbExamSendStudent> GettbExamSendStudent()
        {
            return GetAllList<tbExamSendStudent>(Query.And(new[]
                {
                    Query.EQ("SourceType", 0),
                    Query.EQ("Status", 0)
                }));
        }

        /// <summary>
        /// 获取新进员工的考试信息
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<tbExamSendStudent> GetListByUserID()
        {
            return GetAllList<tbExamSendStudent>(Query.And(new[]
                {
                    Query.EQ("SourceType", 3)
                }));
        }


        /// <summary>
        /// 根据人员和课程ID，查询出单条考试成绩 新员工专用
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public tbExamSendStudent GetListByUserID(int UserId, int relationId)
        {
            MongoCollection<tbExamSendStudent> coll = BaseDB.CreateInstance().GetCollection<tbExamSendStudent>("tbExamSendStudent");

            return coll.FindOne(Query.And(new[]
                {
                     Query.EQ("RelationID", relationId), Query.EQ("UserID", UserId), Query.EQ("SourceType", 3)
                }));
        }

        /// <summary>
        /// 根据课程ID查询所有记录 
        /// </summary>
        /// <param name="relationId"></param>
        /// <returns></returns>
        public List<tbExamSendStudent> GetListByrelationId(int relationId)
        {
            return GetAllList<tbExamSendStudent>(Query.And(new[]
                {
                    Query.EQ("RelationID", relationId),
                    Query.EQ("SourceType", 2)
                }));

        }


        /// <summary>
        /// 找出集中 视频 部门自学所有数据
        /// </summary>
        /// <param name="relationId"></param>
        /// <returns></returns>
        public List<tbExamSendStudent> GetALLExamByOneTwoFive()
        {

            List<int> a = new List<int>();
            a.Add(1);
            a.Add(2);
            a.Add(5);
            return GetAllList<tbExamSendStudent>(Query.And(new[]
                {
                    Query.In("SourceType", new BsonArray(a))
                 
                }));

        }

        //public List<tbExamSendStudent> GetPassOnlineNum(int userid)
        //{

        //    var query = Query.And(Query.In("SourceType", new BsonArray(new List<int>() { 1, 2, 5 })), Query.EQ("Number", 1), Query.EQ("UserID", userID));

        //    return IExaminationrBL.GetAllList<tbExamSendStudent>(query, 1, 10, "UserID", "RelationID");

        //    List<tbExamSendStudent> examStudentList =
        //        _examinationDB.GetAllList<tbExamSendStudent>(Query.And(Query.EQ("Status", 0)));
        //}
        

        #region == 二期 部门/分所自学 add by yxt ==
        /// <summary>
        /// 根据部门分所课程ID和试卷ID获取在线考试人员列表（已考过的人员）
        /// </summary>
        /// <param name="depCourseId">部门分所课程ID</param>
        /// <param name="examPaperId">试卷ID</param>
        /// <returns></returns>
        public List<tbExamSendStudent> GetExamSendStudentByDeptSelfList(int depCourseId, int examPaperId)
        {
            return GetAllList<tbExamSendStudent>(Query.And(new[]
                {
                    Query.EQ("RelationID", depCourseId),
                    Query.EQ("SourceType", 5),
                    Query.EQ("ExamPaperID", examPaperId),
                    Query.EQ("DoExamStatus", 2),
                    Query.EQ("Status", 0)
                }));
        }
        #endregion

        /// <summary>
        /// 根据课程ID获取考试人员列表
        /// </summary>
        /// <param name="relationId"></param>
        /// <returns></returns>
        public List<tbExamSendStudent> GetExamSendStudentListBySQL2008(int relationId,  int CoursePaperId, int ExamPaperID, int SourceType)
        {

            return
                GetAllList<tbExamSendStudent>(
                    Query.And(new[]
                                  {
                                      Query.EQ("RelationID", relationId), Query.EQ("SourceType", SourceType),
                                      Query.EQ("CoursePaperId", CoursePaperId), Query.EQ("ExamPaperID", ExamPaperID)
                                  }));


        }
    }
}