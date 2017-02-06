using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.SystemManage;
using System.Data;
using Retech.Core;
using Retech.Data;

namespace LiXinDataAccess.SystemManage
{
    public class Sys_NoteResourceDB : BaseRepository
    {

        public List<Sys_NoteResource> GetNoteResourceNote(string where = "1=1")
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = string.Format(@"SELECT * FROM Sys_NoteResource
WHERE IsDelete=0 and {0}", where);
                return conn.Query<Sys_NoteResource>(sql).ToList();
            }

        }



        public bool AddNoteResource(Sys_NoteResource model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = @"INSERT INTO Sys_NoteResource
	                            (
	                            NoteId,
	                            RealName,
                                FileName,
	                            CreateDate,    
	                            IsDelete, 
                              	type
	                            )
                            VALUES 
	                            (
	                            @NoteId,
	                            @RealName,
                                @FileName,
	                            @CreateDate,
	                            @IsDelete,
                                @type                        
	                            )
SELECT @@IDENTITY as id";
                var param = new
                {
                    NoteId = model.NoteId,
                    RealName = model.RealName,
                    FileName = model.FileName,                    
                    CreateDate = DateTime.Now,
                    IsDelete = model.IsDelete,
                    type=model.type
                   
                };
                decimal id = conn.Query<decimal>(sql, param).FirstOrDefault();
                model.Id = decimal.ToInt32(id);

                return model.Id > 0;
            }
        }



        public void DeleteNoteResource(string NoteResourceID)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = string.Format(@"UPDATE Sys_NoteResource
SET  IsDelete=1
WHERE Id in ({0})", NoteResourceID);
                conn.Execute(sql);
            }
        }
    }
}
