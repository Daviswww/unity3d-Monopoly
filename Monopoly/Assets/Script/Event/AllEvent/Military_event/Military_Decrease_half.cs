﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


class Military_Decrease_half : EventBase
{ 
        public Military_Decrease_half(string n, bool g, int w, string d, string p) : base(n, g, w, d, p)
        {

        }
        public override void DoEvent(List<Group> droup_list, Group group)
        {
            //個人軍隊人口縮減一半 
            group.Resource.army /=2;
            this.Short_detail = group.name + "軍隊減少一半";
            State = group.State;
        }
}
