using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LiXinDataAccess.SystemManage;
using LiXinModels.SystemManage;
using LiXinInterface;

namespace LiXinBLL
{
    public class Sys_NoteResourceBL : ISys_NoteResource
    {
        private Sys_NoteResourceDB sys_NoteResurceDB;

        public Sys_NoteResourceBL()
        {
            sys_NoteResurceDB = new Sys_NoteResourceDB();
        }


        public List<Sys_NoteResource> GetNoteResourceNote(string where = "1=1")
        {
            return sys_NoteResurceDB.GetNoteResourceNote(where);
        }

        public bool AddNoteResource(Sys_NoteResource model)
        {
           return  sys_NoteResurceDB.AddNoteResource(model);
        }

        public void DeleteNoteResource(string NoteResourceID)
        {
             sys_NoteResurceDB.DeleteNoteResource(NoteResourceID);
        }

    }
}
