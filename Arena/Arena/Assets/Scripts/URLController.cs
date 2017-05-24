using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class URLController : MonoBehaviour {

    public WWWCallbackFunction wwwCallback;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public WWW GET(string url)
    {
        WWW www = new WWW(url);
        StartCoroutine(WaitForRequest(www));
        return www;
    }

    public WWW POST(string url, Dictionary<string, string> post)
    {
        WWWForm form = new WWWForm();
        foreach (KeyValuePair<string, string> post_arg in post)
        {
            form.AddField(post_arg.Key, post_arg.Value);
        }
        WWW www = new WWW(url, form);

        StartCoroutine(WaitForRequest(www));
        return www;
    }

    private IEnumerator WaitForRequest(WWW www)
    {
        yield return www;

        wwwCallback( www );


    }
}
