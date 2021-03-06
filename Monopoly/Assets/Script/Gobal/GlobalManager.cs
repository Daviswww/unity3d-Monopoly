﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GlobalManager
{
    public Map map;
    private bool isAuto;
    private int round;

    private bool isComputer;
    private Group[] groupList;

    private int currentGroupIndex;
    private int totalStep;
    private bool isRolled;
    private bool isFinded;
    private GameState gameState;

    private DisplayManager displayManager;
    private Event events;


    public Group CurrentPlayer
    {
        get { return groupList[currentGroupIndex]; }
    }
    public int TotalStep
    {
        get { return totalStep; }
        set { totalStep = value; }
    }
    public GameState GameState
    {
        get
        {
            return gameState;
        }
        set
        {
            gameState = value;
        }
    }
    public Group[] GroupList
    {
        get
        {
            return groupList;
        }
    }
    public Event Events
    {
        get
        {
            return events;
        }
    }
    internal DisplayManager DisplayManager
    {
        get
        {
            return displayManager;
        }
    }
    public bool IsComputer
    {
        get
        {
            return isComputer;
        }
    }
    public bool IsAuto
    {
        get
        {
            return isAuto;
        }
        set
        {
            isAuto = value;
            isComputer = value;
        }
    }
    public int CurrentGroupIndex
    {
        get
        {
            return currentGroupIndex;
        }
    }

    public GlobalManager(List<Faction> factionList = null)
    {
        createMap();
        createGroupList(factionList);

        isComputer = false;
        currentGroupIndex = 0;
        totalStep = 1;
        gameState = GameState.GlobalEvent;

        displayManager = new DisplayManager(this);
        events = new Event();

        displayManager.displayPlayerInfo(factionList);
        round = 0;
    }



    public void execute()
    {
        switch ( gameState )
        {
            case GameState.GlobalEvent://抽世界事件
                round = round >= groupList.Length ? 0 : round;
                if ( round  == 0 )
                {
                    displayManager.day++;
                    displayManager.timeMsgPanel.GetComponent<Text>().text = string.Format("Day:{0:0000}" ,displayManager.day);
                    displayManager.displayWorldMsg(string.Format("Day:{0:0000}\n" ,displayManager.day) ,true);

                    EventBase eventData = events.doEvent(Eventtype.Word, createList(), CurrentPlayer);
                    gameState = GameState.Wait;

                    displayManager.displayEvent(eventData ,GameState.PersonalEvent);
                }
                else
                {
                    gameState = GameState.PersonalEvent;
                }

                if (CurrentPlayer.InJailTime == 0)
                {
                    CurrentPlayer.State = PlayerState.RollingDice;
                }
                break;

            case GameState.PersonalEvent://抽個人事件
                displayManager.displayWorldMsg("", true);                
                if ( CurrentPlayer.State != PlayerState.InJail )
                {                   
                    EventBase eventData = events.doEvent(Eventtype.Personal, createList(), CurrentPlayer);
                    gameState = GameState.Wait;
                    displayManager.displayEvent(eventData ,GameState.PlayerMovement);
                }
                else
                {
                    gameState = GameState.PlayerMovement;
                }

                break;
            case GameState.PlayerMovement:
                {
                    switch ( groupList[currentGroupIndex].State )
                    {
                        case PlayerState.RollingDice:
                            if (  isAuto || Input.GetButtonDown("Jump") )
                            {
                                CurrentPlayer.State = PlayerState.Wait;
                                displayManager.displayRollingDice();
                            }
                            break;
                        case PlayerState.SearchPath:
                            groupList[currentGroupIndex].findPathList(map ,totalStep);
                            CurrentPlayer.State = PlayerState.Wait;
                            displayManager.displaySearchPath(map);

                            break;
                        case PlayerState.Walking:
                            
                            groupList[currentGroupIndex].move();
                            displayManager.displayPlayerMovement();

                            break;
                        case PlayerState.End:
                            CurrentPlayer.State = PlayerState.Wait;
                            displayManager.displayStopAction(map.BlockList[CurrentPlayer.CurrentBlockIndex] ,GameState.End);

                            break;
                        case PlayerState.InJail:
                            CurrentPlayer.InJailTime--;
                            gameState = GameState.End;//直接結束
                            displayManager.displayWorldMsg(string.Format("{0}無法移動 剩下:{1}回合" ,CurrentPlayer.name ,CurrentPlayer.InJailTime));

                            break;
                        case PlayerState.Wait:
                            //等待
                            break;
                    }
                }
                break;
            case GameState.End:
                displayManager.displayNextPlayer();
                break;
            case GameState.Wait:
                //等待
                break;
        }
        displayManager.displayBlockInfo(map.BlockList[CurrentPlayer.CurrentBlockIndex]);
    }
    public void nextPlayer()
    {
        do
        {
            round++;
            currentGroupIndex = ( currentGroupIndex + 1 ) % groupList.Length;
        }
        while ( groupList[currentGroupIndex] == null );

        if ( IsAuto )
        {
            isComputer = true;
        }
        else
        {
            isComputer = ( currentGroupIndex == groupList.Length - 1 );
        }

        //isComputer = true;
        ///isComputer = false;
        gameState = GameState.GlobalEvent;
    }

    public List<Group> createList()
    {
        List<Group> groupL =  new List<Group>();
        for(int i = 0; i < groupList.Length; i++)
        {
            if (groupList[i] != null)
                groupL.Add(groupList[i]);
        }
        /*
        foreach ( Group g in groupL )
        {
            if ( g == null )
            {
                groupL.Remove(g);
            }
        }
        */
        return groupL;
    }

    /*==========設定遊戲物件==========*/
    private void createMap()
    {
        //string path = Directory.GetCurrentDirectory();
        //string target = @"\Assets\Resources\Map\MonopolyMap.json";
        //string json = File.ReadAllText(path + target);
        string path = Application.streamingAssetsPath;
        string target = "MonopolyMap.json";
        string json = File.ReadAllText(Path.Combine(path ,target));

        map = JsonConvert.DeserializeObject<Map>(json);
        map.build();
    }
    private void createGroupList(List<Faction> factionList)
    {
        List<Faction> factions;
        if ( factionList == null )
        {
            string path = Directory.GetCurrentDirectory();
            string target = @"\Assets\Resources\Faction\MonopolyFaction.json";
            string json = File.ReadAllText(path + target);
            factions = JsonConvert.DeserializeObject<List<Faction>>(json);
            Actor.Path = "PreFab/Actor/";
            foreach ( Faction f in factions )
            {
                f.actorList[0].FileName = "Player1";
            }
        }
        else
        {
            factions = factionList;
        }
        setGroupList(factions);
    }
    private void setGroupList(List<Faction> factions)
    {
        groupList = new Group[Constants.PLAYERNUMBER];
        Direction[] playerDirection = new Direction [Constants.PLAYERNUMBER]{Direction.North ,Direction.East ,Direction.South ,Direction.West ,Direction.unKnow};
        int[] playerIndex = new int[Constants.PLAYERNUMBER]{2 * 30 + 2 ,2 * 30 + 27 ,27 * 30 + 27 ,27 * 30 + 2 ,465};

        int i = 0;
        Group.blockList = map.BlockList;
        foreach ( Faction faction in factions )
        {

            groupList[i] = new Group(null
                                    ,createActorList(faction.actorList)
                                    ,new Attributes(faction.attributes)
                                    ,new Resource(faction.resource)
                                    ,map.BlockList[playerIndex[i]].standPoint()//?
                                    ,playerIndex[i]//?
                                    ,playerDirection[i]);//?


            groupList[i].name = "Player" + ( i + 1 );
            groupList[i].Resource.blockList.Add(map.BlockList[playerIndex[i]]);
            groupList[i].myBuildingList = new GameObject(groupList[i].name + "BuildingList");
            groupList[i].myBuildingList.transform.parent = Group.playerBuildingList.transform;
            //新增主堡
            if ( map.BlockList[playerIndex[i]] is BuildingBlock )
            {
                Building.path = "PreFab/Building/";
                BuildingBlock buildingBlock =  (BuildingBlock)( map.BlockList[playerIndex[i]] ) ;
                buildingBlock.Building = new Building("實驗室" ,"Ahome");
                buildingBlock.Building.build(groupList[i] ,buildingBlock.BuildingLocation ,false);
                buildingBlock.Landlord = groupList[i];
            }

            groupList[i].CurrentActor.build(groupList[i].Location ,playerDirection[i]);
            groupList[i].materialBall = Resources.Load<Material>(Faction.path + faction.fileName);//?
            groupList[i].materialBall = GameObject.Instantiate(groupList[i].materialBall);//?
            groupList[i].CurrentActor.Entity.transform.Find("FL/Circle.005").gameObject.GetComponent<Renderer>().material = groupList[i].materialBall;//?

            if ( ++i >= Constants.PLAYERNUMBER ) break;//temp
        }
    }
    private Actor[] createActorList(List<Actor> actorList)
    {
        Actor[] actors = new Actor[actorList.Count];
        int i = 0;
        foreach ( Actor a in actorList )
        {
            actors[i++] = new Actor(a);
        }
        return actors;
    }
}
