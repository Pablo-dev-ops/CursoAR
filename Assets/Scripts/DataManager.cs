using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item1 : MonoBehaviour
{
    [SerializeField] private List<Item> items = new List<Item>();
    [SerializeField] private GameObject buttonContainer;
    [SerializeField] private ItemButtonManager itemButtonManager;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.OnItemsMenu += CreateButtons;
    }

    private void CreateButtons()
    {
        foreach (var item in items) 
        {
            ItemButtonManager itemButton;
            itemButton = Instantiate(itemButtonManager, buttonContainer.transform);
            itemButton.ItemName = item.itemName;
            itemButton.ItemDescription = item.itemDescription;
            itemButton.ItemImage = item.itemImage;
            itemButton.Item3DModel = item.item3DModel;
            itemButton.name = item.name;
        }

        GameManager.instance.OnItemsMenu -= CreateButtons;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
