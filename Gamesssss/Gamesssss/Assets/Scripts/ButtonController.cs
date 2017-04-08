using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class ButtonController : MonoBehaviour {

    [Header("----------这里是UI对象----------")]
    public Text  TxtButtonText;
    public Image ImgButtonBGImage;
    public Image ImgButtomImage;

    public void SetText( string text, Color color ) {
    }

    public void SetImage( Sprite sprite, Color color ) {
        TxtButtonText.gameObject.SetActive( false );
        ImgButtomImage.gameObject.SetActive( true );

        ImgButtomImage.sprite = sprite;

        if(color!=Color.clear) {
            ImgButtomImage.color = color;
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
