using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARInteractionManager : MonoBehaviour
{

    [SerializeField] private Camera aRCamera;
    private ARRaycastManager aRaycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private GameObject ARPointer;
    private GameObject item3DModel;
    private GameObject itemSelected;
    private bool isInitialPosition;
    private bool isOverUI;
    private bool isOver3DModel;
    private Vector2 initialTouchPos;
    

    public GameObject Item3DModel
    {
        set
        {
            item3DModel = value;
            item3DModel.transform.position = ARPointer.transform.position;
            item3DModel.transform.parent = ARPointer.transform;
            isInitialPosition = true;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        ARPointer = transform.GetChild(0).gameObject;
        GameManager.instance.OnMainMenu += SetItemPosition;
    }



    // Update is called once per frame
    void Update()
    {
        if (isInitialPosition) 
        {
         Vector2 middlePointScreen = new Vector2( Screen.width/2, Screen.height/2 );
          aRaycastManager.Raycast (middlePointScreen, hits, TrackableType.Planes);
            if (hits.Count > 0)
            {
                transform.position = hits[0].pose.position;
                transform.rotation = hits[0].pose.rotation;
                ARPointer.SetActive (false);
                isInitialPosition = false;
            }
        }
        if (Input.touchCount > 0)
        {
            Touch touchOne = Input.GetTouch(0);
            if (touchOne.phase == TouchPhase.Began)
            {
                var touchPosition = touchOne.position;
                isOverUI = IsTapOverUI (touchPosition);
                isOver3DModel = IsTapOver3DModel(touchPosition);  
            }
            if (touchOne.phase == TouchPhase.Moved)
            {
                if (aRaycastManager.Raycast(touchOne.position, hits, TrackableType.Planes))
                {
                    Pose hitPose = hits[0].pose;
                    if (isOverUI && isOver3DModel) 
                    {
                        transform.position = hitPose.position;
                    }
                }

            }
            if (Input.touchCount == 2)
            {
                Touch touchTwo = Input.GetTouch(1);
                if (touchOne.phase == TouchPhase.Began || touchTwo.phase == TouchPhase.Began) 
                {
                 initialTouchPos = touchTwo.position-touchOne.position;
                }
                if (touchOne.phase == TouchPhase.Moved || touchTwo.phase == TouchPhase.Moved) 
                {
                 Vector2 currentTouchPos = touchTwo.position - touchOne.position;
                    float angle = Vector2.SignedAngle(initialTouchPos, currentTouchPos);
                    item3DModel.transform.rotation= Quaternion.Euler(0, item3DModel.transform.eulerAngles.y -angle, 0);
                    initialTouchPos=currentTouchPos;
                }
            }
            if (isOver3DModel && item3DModel == null && !isOverUI) 
            {
                GameManager.instance.ARPosition();
                item3DModel = itemSelected;
                itemSelected = null;
                ARPointer.SetActive(true);
                transform.position = item3DModel.transform.position;
                item3DModel.transform.parent = ARPointer.transform;
            } 
        }

     
    }

    private bool IsTapOver3DModel(Vector2 touchPosition)
    {
        Ray ray = aRCamera.ScreenPointToRay(touchPosition);

        if (Physics.Raycast(ray, out RaycastHit hit3DModel))
            if (hit3DModel.collider.CompareTag("Item"))
            {
                itemSelected = hit3DModel.transform.gameObject;
                return true;
            }

        return false;
    }

    private bool IsTapOverUI(Vector2 touchPosition)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = new Vector2 (touchPosition.x, touchPosition.y);

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll (eventData, results);
        
        return results.Count > 0;
    }

    private void SetItemPosition()
    {
        if (item3DModel != null) 
        {
         item3DModel.transform.parent = null;
            ARPointer.SetActive(false);
            item3DModel = null;
        }
    }
    public void DeleteItem ()
    {
        Destroy(item3DModel);
        ARPointer.SetActive(false);
        GameManager.instance.MainMenu();
    }


}
