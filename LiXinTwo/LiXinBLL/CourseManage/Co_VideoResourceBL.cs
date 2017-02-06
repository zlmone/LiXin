using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinDataAccess.CourseManage;
using LiXinInterface.CourseManage;
using LiXinModels.CourseManage;

namespace LiXinBLL.CourseManage
{
    public class Co_VideoResourceBL : ICo_VideoResource
    {
        public Co_VideoResourceDB CoVideoResourceDb;

        public Co_VideoResourceBL()
        {
            CoVideoResourceDb = new Co_VideoResourceDB();
        }


        public Co_VideoResource GetCo_VideoResourceeResource(int Id)
        {
            return CoVideoResourceDb.GetCo_VideoResourceeResource(Id);
        }

        public List<Co_VideoResource> GetList(int Id)
        {
            return CoVideoResourceDb.GetList(Id);
        }

        public bool AddCo_VideoResource(Co_VideoResource model)
        {
            return CoVideoResourceDb.AddCo_VideoResource(model);
        }

        public bool DeleteCo_VideoResource(int Id)
        {
            return CoVideoResourceDb.DeleteCo_VideoResource(Id);
        }
    }
}
