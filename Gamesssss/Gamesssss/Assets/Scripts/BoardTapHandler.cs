using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class BoardTapHandler : MonoBehaviour, IPointerDownHandler {

    public GameController goController;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnPointerDown(PointerEventData ped) 
    {
        Vector3 localHit = transform.InverseTransformPoint(ped.pressPosition);

        goController.OnBoardTapped( localHit );

    }
}
