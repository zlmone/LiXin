using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.SystemManage;

namespace LiXinInterface
{
    public interface ISys_NoteResource
    {
        List<Sys_NoteResource> GetNoteResourceNote(string where = "1=1");

        bool AddNoteResource(Sys_NoteResource model);

        void DeleteNoteResource(string NoteResourceID);
    }
}
