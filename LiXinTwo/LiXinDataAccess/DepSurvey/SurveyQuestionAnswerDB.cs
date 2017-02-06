using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.Survey;
using Retech.Core;
using Retech.Data;

namespace LiXinDataAccess.DeptSurvey
{
    public partial class SurveyQuestionDB : BaseRepository
    {
        public List<Survey_QuestionAnswer> GetSurvey_QuestionAnswerByQuestionID(int QuestionID)
        {
            using (var connection = OpenConnection())
            {
                string sqlstr = "select * from Dep_Survey_QuestionAnswer where Status=0 and  QuestionID=" + QuestionID;
                return connection.Query<Survey_QuestionAnswer>(sqlstr).ToList();
            }
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(Survey_QuestionAnswer model)
        {
            using (var connection = OpenConnection())
            {
                const string sql = @"INSERT INTO dbo.Dep_Survey_QuestionAnswer
	                                (
	                                QuestionID,
	                                AnswerContent,
	                                Score,
	                                Status,
	                                ShowOrder
	                                )
                                VALUES 
	                                (
	                                @questionid,
	                                @answercontent,
	                                @score,
	                                0,
	                                @showorder
	                                )
                                  ;SELECT @@IDENTITY ID";
                var param = new
                {
                    questionid=model.QuestionID,
                    answercontent=model.AnswerContent,
                    score=model.Score,
                    showorder=model.ShowOrder

                };
                dynamic item = connection.Query<dynamic>(sql, param).FirstOrDefault();
                model.QuestionID = decimal.ToInt32(item.ID);
            }
        }

    }
}
