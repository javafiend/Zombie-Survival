using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewInitializer : MonoBehaviour {

    public ViewBase entryView;
	// Use this for initialization
	void Awake()
    {
        ViewBase[] views = GameObject.FindObjectsOfType<ViewBase>();
        for(int i = 0; i < views.Length; i++)
        {
            ViewBase view = views[i];
            view.viewObject.SetActive(true);
            if(view == entryView)
            {
                continue;
            }
            view.viewObject.SetActive(false);
        }
    }
}
