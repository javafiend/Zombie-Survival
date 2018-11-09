using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ViewBase : MonoBehaviour {

    public GameObject viewObject;
    protected virtual void OnShow() { }
    protected virtual void OnHide() { }
    protected virtual void OnInit() { }
    // Use this for initialization
    void Awake () {
        OnInit();
	}
	
	// Update is called once per frame
	public void Show() {
        OnShow();
        viewObject.SetActive(true);
	}

    public void Hide()
    {
        OnHide();
        viewObject.SetActive(false);
    }
}
