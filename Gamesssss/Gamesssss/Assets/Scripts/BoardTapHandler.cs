using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class BoardTapHandler : MonoBehaviour, IPointerDownHandler {

    public GameController goController;

    bool _swiped;

	// Use this for initialization
	void Start () {
        _swiped = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnPointerDown(PointerEventData ped) 
    {
        Debug.Log( "OnPointerDown" );

        Vector3 localHit = transform.InverseTransformPoint(ped.pressPosition);

        goController.OnBoardTapped( localHit );

    }

    public void OnDrag(UnityEngine.EventSystems.BaseEventData eventData)
    {
        Debug.Log( "OnDrag!!!");


    }


    public void OnBeginDrag(UnityEngine.EventSystems.BaseEventData eventData)
    {
        Debug.Log( "OnBeginDrag!!!");

    }

    public void OnEndDrag(UnityEngine.EventSystems.BaseEventData eventData)
    {
        Debug.Log( "OnEndDrag!!!");

        if(_swiped==true) {
            return;
        }
        var pointerData = eventData as UnityEngine.EventSystems.PointerEventData;
        if (pointerData == null) { return; }

        if(  pointerData.position.x-pointerData.pressPosition.x>75) {
            goController.OnTouchSwipe( GameController.DIR_RIGHT );
            _swiped = true;
        }
        if(  pointerData.position.x-pointerData.pressPosition.x<75) {
            goController.OnTouchSwipe( GameController.DIR_LEFT );
            _swiped = true;
        }
        _swiped = false;
    }

}
