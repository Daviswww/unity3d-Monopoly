﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingDisplayController : MonoBehaviour
{
    public GlobalManager globalManager;
    public BuildingBlock currentBuildingBlock;
    public GameState nextGameState;

    private List<Building> buildingList;
    private List<GameObject> buildingEntityList;
    private Building currentBuilding;
    private GameObject currentBuildingEntity;

    private GameObject checkoutButton;

    private void Awake()
    {
        createBuildingList();
        createBuildingEntityList();
        checkoutButton = gameObject.transform.Find("checkoutButton").gameObject;
        checkoutButton.SetActive(false);
    }
    public void selectBuilding(int no)
    {
        currentBuilding = buildingList[no];

        currentBuildingEntity.SetActive(false);
        currentBuildingEntity = buildingEntityList[no];
        currentBuildingEntity.transform.position = currentBuildingBlock.BuildingLocation;
        currentBuildingEntity.SetActive(true);

        checkoutButton.SetActive(true);
    }
    public void checkoutButtonClick()
    {
        checkoutButton.SetActive(false);
        gameObject.SetActive(false);
        currentBuildingBlock.Building = new Building(currentBuilding);
        bool canBuy = currentBuildingBlock.Building.build(globalManager.CurrentPlayer ,currentBuildingBlock.BuildingLocation);

        if(canBuy)
        {
            currentBuildingBlock.Landlord = globalManager.CurrentPlayer;
            globalManager.CurrentPlayer.Resource.blockList.Add(currentBuildingBlock);//加到玩家擁有的blockList裡面

            currentBuildingEntity.SetActive(false);
            globalManager.GameState = nextGameState;
        }
        else
        {
            globalManager.DisplayManager.displayCantNotBuy(nextGameState);
        }
    }

    /*==========create==========*/
    private void createBuildingList()
    {
        buildingList = new List<Building>();
        Building.path = "PreFab/Building/";
        string[] bName = {"城市" ,"醫院" ,"軍營" ,"礦場"};
        string[] bFileName = {"A" ,"A" ,"A" ,"A"};

        for ( int i = 0 ; i < 4 ; i++ )
        {
            Building building = new Building();
            building.Name = bName[i];
            building.FileName = bFileName[i];

            buildingList.Add(building);
        }
        currentBuilding = buildingList[0];
    }
    private void createBuildingEntityList()
    {
        buildingEntityList = new List<GameObject>();
        
        for(int i = 0 ; i < buildingList.Count ; i++ )
        {
            GameObject buindingEntity = Resources.Load<GameObject>(Building.path + buildingList[i].FileName);
            buindingEntity = GameObject.Instantiate(buindingEntity);
            buildingEntityList.Add(buindingEntity);
            buindingEntity.SetActive(false);
        }
        currentBuildingEntity = buildingEntityList[0];
    }
}
