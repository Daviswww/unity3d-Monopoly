﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event
{
    private EventPool apes;
    private EventPool forest;
    private EventPool personal;
    private EventPool word;
    private EventPool block;
    private List<int> Event_Weight;
    private int diplomatic_weight;
    private int peace_weight;
    private int leadership_weight;

    public Event()
    {
        apes = new EventPool();
        forest = new EventPool();
        personal = new EventPool();
        word = new EventPool();
        block = new EventPool();
        Add_Apes_Event();
        Add_Forest_Event();
        Add_Personal_Event();
        Add_Word_Event();
    }

    private void Add_Apes_Event()
    {
        //外交
        apes.AddEvent(new Apse_Migrate("人類發現猩猩的所在位置" ,false ,8 ,"由於人類發現猩猩的所在位置，使得猩猩必須遷移" ,"EventImage/AllEvent"));
        apes.AddEvent(new Apes_Time_Out("猩猩被人類俘虜" ,false ,8 ,"由於猩猩被人類抓走成為俘虜，猩猩暫停一回合" ,"EventImage/Time_Out"));
        //和平
        apes.AddEvent(new Peace_Increase("猩猩們不自相殘殺" ,true ,8 ,"由於猩猩們不自相殘殺，使和平指數上升10" ,"AllEvent" ,10));
        apes.AddEvent(new Diplomatic_Increase("猩猩放棄鬥爭" ,true ,8 ,"由於猩猩死傷慘重,所以放棄鬥爭，使猩猩外交指數上升2" ,"EventImage/AllEvent" ,2));
        apes.AddEvent(new Orangutan_Reduction("猩猩看不慣凱薩" ,false ,8 ,"由於猩猩看不慣凱薩，發生內鬥，使猩猩和平指數下降20，猩猩人口減少4%" ,"EventImage/AllEvent"));
        //領導
        apes.AddEvent(new Everyone_People_Decrease("帶著猿流感的猩猩失控爆走" ,false ,5 ,"帶著猿流感的猩猩失控爆走，使得病毒擴散加快，人類死亡100" ,"EventImage/Virus" ,100));
        apes.AddEvent(new Every_People_Time_Out("人類軍營慘遭猩猩入侵" ,false ,5 ,"由於人類軍營慘遭猩猩入侵，導致損失慘重，所有人類暫停一回合" ,"EventImage/Time_Out"));
        apes.AddEvent(new Human_Diplomacy("紅毛猩猩教導猩猩族群學習語言" ,true ,5 ,"由於紅毛猩猩教導猩猩族群學習語言，使猩猩可以跟人類外交，且減少病毒擴散，使解藥研發進度提升，外交指數上升" ,"EventImage/Human_Diplomacy"));

        apes.AddEvent(new People_Increase("猩猩發現落單人類" ,true ,5 ,"由於猩猩發現落單人類，並將之規劃為人民，猩猩增加人口200" ,"EventImage/AllEvent" ,200));
        apes.AddEvent(new Military_Increase("猩猩俘虜人類士兵" ,true ,5 ,"俘虜人類士兵，軍人增加100" ,"EventImage/AllEvent" ,100));
        apes.AddEvent(new People_Decrease_percent("森林大火" ,false ,1 ,"森林大火，造成猩猩人口死亡50%" ,"EventImage/file" ,50));
    }

    private void Add_Forest_Event()
    {
        //外交
        forest.AddEvent(new Commicave_With_Apes("和猩猩談判成功" ,true ,6 ,"由於和猩猩談判成功，使猩猩和玩家外交指數上升" ,"EventImage/Diplomat"));
        forest.AddEvent(new Diplomatic_Decrease("人類和猩猩起衝突" ,false ,8 ,"人類和猩猩起衝突，外交減少10" ,"EventImage/Apse_attack" ,10));
        //和平
        forest.AddEvent(new Military_Decrease("軍隊內鬥" ,false ,8 ,"軍隊內鬥，軍隊內互相打鬥，造成傷亡，軍隊減少50人" ,"EventImage/Person_attacked" ,50));
        //領導
        forest.AddEvent(new Discover_Secret_Passage_Forest("途中發現祕密通道" ,false ,5 ,"由於途中發現祕密通道，可直達森林入口" ,"EventImage/Secret"));
        forest.AddEvent(new Military_Decrease_half("軍官叛逃" ,false ,5 ,"因軍官受不了上級作法，帶領軍隊叛逃，軍隊人口縮減一半 " ,"EventImage/AllEvent"));
        forest.AddEvent(new Military_Increase_percent("截獲猩猩軍營" ,false ,5 ,"截獲猩猩軍營，獲得大量兵力，軍隊增加100%" ,"EventImage/AllEvent" ,100));

        forest.AddEvent(new Military_Decrease("遭受猩猩偷襲" ,false ,5 ,"闖入森林，遭受群居猩猩偷襲，軍隊減少20%" ,"EventImage/Person_attacked2" ,20));
        forest.AddEvent(new Dice_Again("從俘虜中逃出", true, 6, "從俘虜中逃出，再次擲骰子", "EventImage/AllEvent"));
        forest.AddEvent(new Every_People_Time_Out("猩猩入侵軍營" ,false ,5 ,"由於軍營慘遭猩猩入侵，導致損失慘重，所有人類暫停一回合 " ,"EventImage/Person_attacked"));
        forest.AddEvent(new People_Time_Out("被猩猩抓走成為俘虜" ,false ,5 ,"由於被猩猩抓走成為俘虜，暫停一回合" ,"EventImage/Time_Out"));
        forest.AddEvent(new People_Time_Out("遭遇猩猩攻擊" ,false ,8 ,"遭遇猩猩攻擊，暫停一回合" ,"EventImage/Apse_attack"));
        forest.AddEvent(new People_Decrease_percent("野外遭遇帶著猿流感的猩猩" ,false ,5 ,"野外遭遇帶著猿流感的猩猩，使得病毒擴散加快，人類死亡3%" ,"EventImage/Virus"));
        forest.AddEvent(new Apse_Migrate("人類發現猩猩的所在位置" ,true ,3 ,"由於人類發現猩猩的所在位置，使得猩猩必須遷移" ,"EventImage/AllEvent"));
        forest.AddEvent(new Diplomatic_Decrease("人類與帶原猩猩接觸" ,false ,5 ,"人類與帶原猩猩接觸，使得語言能力退化，外交能力喪失，外交減少90%" ,"EventImage/Diplomatic_Iindex_Rises" ,90));

    }

    private void Add_Personal_Event()
    {
        //外交
        personal.AddEvent(new Military_Increase_percent("猩猩叛逃投靠人類" ,true ,8 ,"猩猩叛逃投靠人類，使軍隊增加20%" ,"EventImage/Person_attacked2" ,20));
        personal.AddEvent(new People_Increase("敵方陣營人民投靠" ,true ,8 ,"敵方陣營人民投靠，使人數增加100" ,"EventImage/AllEvent" ,100));
        //和平
        personal.AddEvent(new Peace_Decrease("人類內鬥" ,false ,6 ,"人類軍對下級和上級處不來，和平減少10" ,"EventImage/AllEvent" ,10));
        personal.AddEvent(new Military_Decrease("發生叛亂" ,false ,6 ,"發生叛亂，軍官帶兵反抗統帥，使軍人死亡100" ,"EventImage/AllEvent" ,100));
        //領導
        personal.AddEvent(new Military_Increase_percent("研發出新武器" ,true ,7 ,"由於個人研發新武器，使軍隊增加10%" ,"EventImage/Military_Increase" ,10));
        personal.AddEvent(new Secret_Passage_Time_Out("實驗室密道" ,true ,4 ,"在路途中發現能直達實驗室的秘密通道，但必須暫停2回合" ,"EventImage/Secret"));
        personal.AddEvent(new People_Time_Out("被猩猩抓走成為俘虜" ,false ,4 ,"由於被猩猩抓走成為俘虜，暫停一回合" ,"EventImage/Time_Out"));
        personal.AddEvent(new Discover_Secret_Passage_Forest("途中發現祕密通道" ,true ,5 ,"由於途中發現祕密通道，可直達森林入口" ,"AllEEventImage/Secret"));

        personal.AddEvent(new GetResourceEvent("在礦場中意外發現稀有礦物" ,true ,11 ,"在礦場中意外發現稀有礦物，個人所得資源增加10%" ,"EventImage/Resources_Increase" ,10));
        personal.AddEvent(new People_Time_Out("受到戰爭流彈波及" ,false ,6 ,"個人受到戰爭流彈波及，暫停一回合養傷" ,"EventImage/Time_Out"));
        personal.AddEvent(new Military_Decrease_half("營地發生雪崩" ,false ,4 ,"營地發生雪崩，個人軍隊人口縮減一半 " ,"EventImage/Avalanche"));
        personal.AddEvent(new People_Decrease_Half("礦場倒塌" ,false ,3 ,"由於礦場倒塌，人口減少一半" ,"EventImage/AllEvent"));
        personal.AddEvent(new People_Time_Out("猩猩發現落單人類" ,false ,6 ,"由於猩猩發現落單人類，個人暫停一回合" ,"EventImage/Time_Out"));
        personal.AddEvent(new People_Time_Out("在橋上不慎踩空" ,false ,7 ,"由於在橋上不慎踩空，暫停一回合" ,"EventImage/Time_Out"));
        personal.AddEvent(new Military_Decrease("軍營中發現帶有病毒猩猩的屍體" ,false ,4 ,"軍營中發現帶有病毒猩猩的屍體，使軍隊人口感染人數增加，軍隊減少10%" ,"EventImage/AllEvent" ,10));
        personal.AddEvent(new Antidote_Decrease("途中發現帶有變種病毒死亡的人類屍體" ,false ,5 ,"途中發現帶有變種病毒死亡的人類屍體，使解藥研究程度下降5%" ,"EventImage/AllEvent" ,5));
        personal.AddEvent(new Diplomatic_Decrease("人類遭遇猿流感" ,false ,5 ,"人類遭遇猿流感，使得語言能力退化，外交能力喪失，外交減少90%" ,"EventImage/Virus"));
        personal.AddEvent(new Military_Decrease("為防止軍隊病毒災情擴散" ,false ,7 ,"為防止軍隊病毒災情擴散，將處決軍隊受感染人數，軍隊人數減少5%" ,"EventImage/Execution" ,5));
        personal.AddEvent(new People_Increase_percent("發現病毒對兒童無效" ,true ,10 ,"發現病毒對兒童無效，使人數增加20%" ,"EventImage/Virus" ,20));
        personal.AddEvent(new Go_To_The_Designated_Location("途中意外發現軍用直升機" ,true ,7 ,"途中意外發現軍用直升機，可前隨機前往自己的其中一個領地" ,"EventImage/Helicopter"));
        personal.AddEvent(new Antibody_Increased("解藥研發突破" ,true ,10 ,"解藥研發突破，使解藥研發提升2%" ,"EventImage/Virus" ,2));
        personal.AddEvent(new Win_Event("遊戲結束" ,true ,1 ,"運氣真好，遊戲結束，" ,"EventImage/Win"));
        personal.AddEvent(new Lose_Event("你輸了" ,true ,1 ,"運氣真背，竟然然抽到此事件，你的遊戲結束了" ,"EventImage/Lose"));

    }

    private void Add_Word_Event()
    {
        word.AddEvent(new EarthquakeEvent("發生地震" ,false ,6 ,"發生大地震，所有玩家損失10%人口" ,"EventImage/AllEvent"));
        word.AddEvent(new Everyone_Virus_Increase("受到感染的人類失去控制" ,false ,6 ,"受到感染的人類失去控制，到處亂跑，使病毒增加快速 ，人類大量感染，所有人類死亡10%" ,"EventImage/AllEvent"));
        word.AddEvent(new Everyone_Antibody_Increased("醫院科技技術進步" ,true ,6 ,"由於醫院科技技術進步，使得人類對抗病毒抗體增加，人類解藥研發程度上升5%" ,"EventImage/Virus" ,5));
        word.AddEvent(new Everyone_People_Increase("遇到沒受感染的人類" ,true ,6 ,"遇到沒受感染的人類，使得人口增加" ,"EventImage/Virus"));
        word.AddEvent(new Population_Decline("醫院由於災情慘重" ,false ,6 ,"醫院由於災情慘重，使得解藥研究降低，且所有人口減少" ,"EventImage/AllEvent"));
        word.AddEvent(new Lack_Of_Energy("城市人口增加" ,false ,6 ,"城市人口增加，使得能源不足，資源減半，人口增加200" ,"EventImage/AllEvent"));
        word.AddEvent(new Everyone_People_Decrease("所有人類遭遇野生猩猩發動攻擊，失去人口數量" ,false ,6 ,"所有人類遭遇野生猩猩發動攻擊，失去人口數量" ,"EventImage/Person_attacked"));
        word.AddEvent(new Everyone_Antibody_Increased("人類發現新型解藥" ,true ,6 ,"人類發現新型解藥，使得所有人解藥研發程度提升2%" ,"EventImage/Lab" ,2));
        word.AddEvent(new Apse_Defection("猩猩叛逃" ,true ,6 ,"猩猩叛逃，所有人類獲得100軍隊，猩猩減少100軍隊" ,"EventImage/Military_Increase"));
        word.AddEvent(new Change_Plant("地殼移動" ,true ,6 ,"地殼移動，隨機兩位玩家位置交換" ,"EventImage/AllEvent"));
        word.AddEvent(new EveryoneGetResourceEvent("發現遠古金礦" ,true ,10 ,"發現遠古金礦，每人獲得資源增加5%" ,"EventImage/AllEvent" ,5));
        word.AddEvent(new Everyone_eace_Increase("和平紀念日" ,true ,10 ,"和平紀念日，增加所有人的和平指數" ,"EventImage/AllEvent" ,5));

    }
    private void caculate_weight(Group group)
    {
        diplomatic_weight = System.Convert.ToInt32(group.Attributes.diplomatic * 0.1);
        peace_weight = System.Convert.ToInt32(group.Attributes.peace * 0.1);
        leadership_weight = System.Convert.ToInt32(group.Attributes.leadership * 0.1);
    }

    public EventBase doEvent(Eventtype eventtype ,List<Group> droup_list ,Group group)
    {
        EventBase doingevent;
        switch ( eventtype )
        {
            case Eventtype.Apes:
                apes.SetWeight(0 ,-1 * diplomatic_weight);
                apes.SetWeight(1 ,-1 * diplomatic_weight);
                apes.SetWeight(2 ,peace_weight);
                apes.SetWeight(3 ,peace_weight);
                apes.SetWeight(4 ,-1 * peace_weight);
                apes.SetWeight(5 ,leadership_weight);
                apes.SetWeight(6 ,leadership_weight);
                apes.SetWeight(7 ,leadership_weight);
                doingevent = apes.GetEvent();
                break;
            case Eventtype.Forest:
                forest.SetWeight(0 ,diplomatic_weight);
                forest.SetWeight(1 ,-1 * diplomatic_weight);
                forest.SetWeight(2 ,-1 * peace_weight);
                forest.SetWeight(3 ,leadership_weight);
                forest.SetWeight(4 ,-1 * leadership_weight);
                forest.SetWeight(5 ,leadership_weight);
                doingevent = forest.GetEvent();
                break;
            case Eventtype.Personal:
                personal.SetWeight(0 ,diplomatic_weight);
                personal.SetWeight(1 ,diplomatic_weight);
                personal.SetWeight(2 ,-1 * peace_weight);
                personal.SetWeight(3 ,-1 * peace_weight);
                personal.SetWeight(4 ,leadership_weight);
                personal.SetWeight(5 ,leadership_weight);
                personal.SetWeight(6 ,-1 * leadership_weight);
                personal.SetWeight(7 ,leadership_weight);
                doingevent = personal.GetEvent();
                break;
            case Eventtype.Word:
                doingevent = word.GetEvent();
                Poisoning(50 ,droup_list ,doingevent);
                break;
            case Eventtype.Attack_Plant:
                doingevent = new Attack();
                break;
            case Eventtype.Battle:
                doingevent = new Battle();
                break;
            default:
                doingevent = block.GetEvent();
                break;
        }
        doingevent.DoEvent(droup_list ,group);
        return doingevent;
    }
    private void Poisoning(int deal ,List<Group> droup_list ,EventBase e)
    {
        string str = "";
        for ( int i = 0 ; i < droup_list.Count - 1 ; i++ )
        {
            if ( droup_list[i].Resource.antidote == 0 ) deal = 50;
            else if ( droup_list[i].Resource.antidote == 100 ) deal = 0;
            else deal = Convert.ToInt32(droup_list[i].Resource.civilian * ( ( droup_list[i].Resource.antidote / 100.0 ) * ( droup_list[i].Resource.antidote / 100.0 ) ));

            droup_list[i].Resource.civilian -= deal;
            str += droup_list[i].name + "感染人口死亡" + deal.ToString() + "\n";
        }
        e.Detail = str + e.Detail;
        e.Short_detail = "感染人口死亡\n" + e.Short_detail;
    }
}
