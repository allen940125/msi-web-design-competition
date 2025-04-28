using System;
using Datamanager;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    [SerializeField] Image image;
    void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var itemData = GameContainer.Get<DataManager>().GetDataByID<ItemDataBaseTemplete>(101);
            image.sprite = itemData.ItemIconPath;
        }
    }
}
