using System.Collections.Generic;
using LiXinModels.Examination.DBModel;

namespace LiXinInterface.Examination
{
    public interface IExamination
    {
        /// <summary>
        ///     新增考试
        /// </summary>
        /// <param name="exam"></param>
        /// <returns></returns>
        int AddExamination(tbExamination exam);

        /// <summary>
        ///     修改考试
        /// </summary>
        /// <param name="exam"></param>
        /// <returns></returns>
        bool ModifyExamination(tbExamination exam);

        /// <summary>
        ///     判断考试名称是否存在
        /// </summary>
        /// <param name="examinationTitle"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        bool IsExistExamTitle(string examinationTitle, int id);

        /// <summary>
        ///     获取所有考试
        /// </summary>
        /// <returns></returns>
        List<tbExamination> GetAllExamination();

        /// <summary>
        ///     获取指定ID集合的考试
        /// </summary>
        /// <param name="examIDs">考试ID集合</param>
        /// <returns></returns>
        List<tbExamination> GetAllExamination(IEnumerable<int> examIDs);

        tbExamination GetExamination(int id);

        /// <summary>
        ///     删除考试授权人员，添加新的授权人员
        /// </summary>
        /// <param name="examinationID"></param>
        /// <param name="userIDs"></param>
        /// <returns></returns>
        bool SaveExamSendStudent(int examinationID, IEnumerable<int> userIDs);

        List<tbExamSendStudent> GettbExamSendStudent(int examId);

        
        /// <summary>
        ///     得到该考试所有学生
        /// </summary>
        /// <param name="examId"></param>
        /// <returns></returns>
        List<tbExamSendStudent> GettbExamSendAllStudent(int examId);
        /// <summary>
        ///     根据考试ID  获取已经提交考试的（考生关联考试）列表
        /// </summary>
        /// <param name="examId"></param>
        /// <returns></returns>
        List<tbExamSendStudent> GetAllStudentByExamId(int examId);

        /// <summary>
        ///     根据 考生关联考试ID  获取考生答题详情
        /// </summary>
        /// <returns></returns>
        tbExamSendStudent GetExamSendStudent(int id);

        /// <summary>
        ///     修改 考试关联考生实体    2012-8-29 13:49:30【复评成绩时用】
        /// </summary>
        /// <param name="examSendStudent"></param>
        /// <returns></returns>
        bool ModifyExamSendStudent(tbExamSendStudent examSendStudent);

        /// <summary>
        /// 添加课程下的考试
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int InserttbExamSendStudent(tbExamSendStudent model);

        /// <summary>
        /// 当SourceType=1，在根据RelationID和UserId删除
        /// </summary>
        /// <param name="relationId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        bool DeleteExamSendStudentWithByCourseIdAndUserId(int relationId, int UserId, int SourceType);

        /// <summary>
        /// 当SourceType=1，在根据RelationID和UserId查找数据
        /// </summary>
        /// <param name="relationId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        tbExamSendStudent GetSingletbExamSendStudentByCourseIdAndUserId(int relationId, int UserId,int SourceType);


        /// <summary>
        /// 获取课程下所有学习试卷的人员
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        List<tbExamSendStudent> GetExamSendStudentListWithCourseId(int courseId);


        /// <summary>
        /// 修改课程是否获得学时 0:没有获得  1:获得  获得后后面的考试就不在获得学时 标志位
        /// </summary>
        /// <param name="tbexamsendstudentid">id</param>
        /// <param name="number">0:1</param>
        /// <returns></returns>
        int UpdateNumber(int tbexamsendstudentid, int number);



        int UpdateTestTimes(int tbexamsendstudentid, int TestTimes);
    }
}