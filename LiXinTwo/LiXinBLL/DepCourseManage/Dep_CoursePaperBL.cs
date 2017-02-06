using LiXinDataAccess.DepCourseManage;
using LiXinInterface.DepCourseManage;
using LiXinModels.DepCourseManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinBLL.DepCourseManage
{
    public class Dep_CoursePaperBL : IDep_CoursePaper
    {
        private Dep_CoursepaperDB dep_coursepaperdb;

        public Dep_CoursePaperBL(Dep_CoursepaperDB _dep_coursepaperdb)
        {
            dep_coursepaperdb = _dep_coursepaperdb;
        }

        public Dep_Coursepaper GetCo_CourseMainPaper(int CourseId)
        {
            return dep_coursepaperdb.GetCo_CourseMainPaper(CourseId);
        }
    }
}
