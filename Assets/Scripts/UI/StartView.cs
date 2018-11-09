using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartView : ViewBase {

    [Header("View References")]

    public ViewBase optionsView;
    public GameObject lobbyCam;
    public GameObject inGameUI;
    public GameObject mainUI;
    public Button startButton;
    public Button optionsButton;
    public Button exitButton;

    protected override void OnInit()
    {
        startButton.onClick.AddListener(() =>
        {
            lobbyCam.SetActive(false);
            mainUI.SetActive(false);
            inGameUI.SetActive(true);
            LevelManager.instance.StartGame();
        });

        optionsButton.onClick.AddListener(() =>
        {
            //this.Hide();
            //optionsView.Show();
        });

        exitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
    protected override void OnShow()
    {
        lobbyCam.SetActive(true);
        mainUI.SetActive(true);
        inGameUI.SetActive(false);

    }
}
