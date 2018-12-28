﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Human_Diplomacy : EventBase
{
    public Human_Diplomacy(string n,bool g,int w, string d) :base(n,g,w,d)
    {

    }
    public override void DoEvent(List<Group> droup_list, Group group)
    {
        //由於紅毛猩猩教導猩猩族群學習語言，使猩猩可以跟人類外交，且減少病毒擴散，使解藥研發進度提升，外交指數上升
        for(int i = 0;i<4;i++)
        {
            droup_list[i].Resource.antidote += 2;
            droup_list[i].Attributes.diplomatic += 5;
        }
        State = group.State;
    }
}