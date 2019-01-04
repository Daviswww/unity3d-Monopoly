﻿using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GlobalManager
{
    public Map map;
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

    public GlobalManager(List<Faction> factionList = null)
    {
        createMap();
        createGroupList(factionList);


        currentGroupIndex = 0;
        totalStep = 1;
        gameState = GameState.GlobalEvent;

        displayManager = new DisplayManager(this);
        events = new Event();
    }



    public void execute()
    {
        switch ( gameState )
        {
            case GameState.GlobalEvent:
                if(currentGroupIndex % groupList.Length == 0)
                {
                    //抽世界事件
                    EventBase eventData = events.doEvent(Eventtype.Word ,new List<Group>(groupList) ,CurrentPlayer);                    
                    gameState = GameState.Wait;
                    displayManager.displayEvent(eventData ,GameState.PersonalEvent);
                }
                else
                {
                    gameState = GameState.PersonalEvent;
                }
                CurrentPlayer.State = PlayerState.RollingDice;

                break;
            case GameState.PersonalEvent:
                if ( CurrentPlayer.InJailTime == 0 )
                {
                    //抽個人事件
                    EventBase eventData = events.doEvent(Eventtype.Personal ,new List<Group>(groupList) ,CurrentPlayer);                   
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
                            if ( Input.GetButtonDown("Jump") )
                            {
                                CurrentPlayer.State = PlayerState.Wait;
                                displayManager.displayRollingDice();//轉換到下一個階段
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
                            //交給displayManager
                            displayManager.displayStopAction(map.BlockList[CurrentPlayer.CurrentBlockIndex] ,GameState.End);

                            break;
                        case PlayerState.InJail:
                            CurrentPlayer.InJailTime--;
                            gameState = GameState.End;//直接結束

                            break;
                        case PlayerState.Wait:
                            //等待
                            break;
                    }
                }
                break;
            case GameState.End:
                CurrentPlayer.State = PlayerState.Wait;
                //交給displayManager
                displayManager.displayNextPlayer();

                break;
            case GameState.Wait:
                //等待
                break;
        }
    }
    public void nextPlayer()
    {
        currentGroupIndex = ( currentGroupIndex + 1 ) % Constants.PLAYERNUMBER;
        gameState = GameState.GlobalEvent;
    }



    /*==========設定遊戲物件==========*/
    private void createMap()
    {
        string path = Directory.GetCurrentDirectory();
        string target = @"\Assets\Resources\Map\MonopolyMap.json";
        string json = File.ReadAllText(path + target);

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
            foreach(Faction f in factions)
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
        Direction[] playerDirection = new Direction [Constants.PLAYERNUMBER]{Direction.North ,Direction.East ,Direction.South ,Direction.West};
        int[] playerIndex = new int[Constants.PLAYERNUMBER]{2 * 30 + 2 ,2 * 30 + 27 ,27 * 30 + 27 ,27 * 30 + 2};

        int i = 0;
        Group.blockList = map.BlockList;
        foreach ( Faction faction in factions )
        {
            groupList[i] = new Group(null
                                    ,createActorList(faction.actorList)/*faction.actorList.ToArray()*/
                                    ,new Attributes(faction.attributes)
                                    ,new Resource(faction.resource)
                                    ,map.BlockList[playerIndex[i]].standPoint()//?
                                    ,playerIndex[i]//?
                                    ,playerDirection[i]);//?

            groupList[i].CurrentActor.build(groupList[i].Location ,playerDirection[i]);
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
