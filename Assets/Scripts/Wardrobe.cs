using System;
using Game.UI;
using Gamemanager;
using UnityEngine;

public class Wardrobe : InteractableObject
{
    [SerializeField] private int currentTakeId;
    [SerializeField] private int maxTakeId = 3;
    
    public void TakeClothingOnWardrobe()
    {
        Debug.Log("TakeBookOnCabinet 在衣櫃上拿衣服");
        if (currentTakeId <= maxTakeId - 1)
        {
            currentTakeId++;

            if (currentTakeId == 1)
            {
                GetItem(110);
            }
            else if(currentTakeId == 2)
            {
                GetItem(111);
            }
            else if(currentTakeId == 3)
            {
                GetItem(112);
            }
        }
        else
        {
            GetItem(0);
        }
    }
}
