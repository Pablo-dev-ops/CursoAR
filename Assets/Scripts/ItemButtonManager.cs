using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ItemButtonManager : MonoBehaviour
{
    private string itemName;
    private string itemDescription;
    private Sprite itemImage;
    private GameObject item3DModel;
    private ARInteractionManager interactionManager;

    public string ItemName
    {
        set
        {
            itemName = value;
        }

    }

    public string ItemDescription { set => ItemDescription = value; }
    public Sprite ItemImage { set => ItemImage = value; }
    public GameObject Item3DModel { set => Item3DModel = value; }
    // Start is called before the first frame update
    void Start()
    {
        transform.GetChild(0).GetComponent<Text>().text = itemName;
        transform.GetChild(1).GetComponent<RawImage>().texture = itemImage.texture;
        transform.GetChild(2).GetComponent<Text>().text = itemDescription;

        var button = GetComponent<Button>();
        button.onClick.AddListener(GameManager.instance.ARPosition);
        button.onClick.AddListener(Create3dModel);

        interactionManager = FindObjectOfType<ARInteractionManager>();
    }
    private void Create3dModel()
    {
        interactionManager.Item3DModel = Instantiate(item3DModel);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
