using System.Collections.Generic;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using LiXinModels.Examination.DBModel;

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



    }
}