using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class iClickClass : MonoBehaviour {
    public static GameObject backTransition = null;

    public string functionStart = "";
    public string functionEnable = "";
    public string functionClick = "";
    public string functionPress = "";
    public string functionPressButton = "";
    public string functionPressButtonTransition = "";
    public string functionDragStart = "";
    public string functionDrag = "";
    public static GameObject currentButton = null;
    //public List<EventDelegate> onFinished = new List<EventDelegate>();

    private float timeSinceTouch = 0;
    private bool enableDrag = false;
    //public string functionDestroy = "";

    // Use this for initialization
    void Start() {
        if (functionStart != "") SendMessage(functionStart);
    }

    // Update is called once per frame
    void Update() {

    }


    void OnEnable() {
        if (functionEnable != "") SendMessage(functionEnable);
    }

    void OnClick() {
        if (functionClick != "") SendMessage(functionClick);
    }

    void OnPress(bool isPressed) {
        if (!isPressed && functionPress != "") SendMessage(functionPress, isPressed);
        if (!isPressed && functionPressButton != "") StartCoroutine("coroutinePressButton");
        if (!isPressed && functionPressButtonTransition != "") StartCoroutine("coroutinePressButtonTransition");
    }

    void OnDragStart() {
        if (functionDragStart != "") SendMessage(functionDragStart);
    }
    void OnDrag() {
        if (functionDrag != "") SendMessage(functionDrag);
    }

    public IEnumerator coroutinePressButton() {
        GetComponent<Animator>().Play("button");
        yield return StartCoroutine(staticClass.waitForRealTime(0.2F));
        Debug.Log("coroutinePressButton: " + functionPressButton);
        SendMessage(functionPressButton);
    }
    public IEnumerator coroutinePressButtonTransition() {
        GetComponent<Animator>().Play("button");
        yield return StartCoroutine(staticClass.waitForRealTime(0.2F));
        GameObject.Find("panel back transition").GetComponent<Animator>().Play("back transition enabled");
        //GameObject.Find("back transition").GetComponent<Animator> ().Play ("back transition exit");
        //yield return StartCoroutine(staticClass.waitForRealTime(0.2F));
        Debug.Log("functionPressButtonTransition: " + functionPressButtonTransition);
        SendMessage(functionPressButtonTransition);
        //yield return StartCoroutine(staticClass.waitForRealTime(0.2F));
    }



    void enableCard() {
        //если куплен
        //Debug.Log (name);
        //Debug.Log (initClass.progress [name]);
        if (initClass.progress.Count == 0) initClass.getProgress();
        if (initClass.progress[name] >= 1) {
            //skin
            transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
            transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
            //title
            transform.GetChild(2).gameObject.SetActive(true);
            GetComponent<UIWidget>().alpha = 1;


        }
        else {
            transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
            transform.GetChild(2).gameObject.SetActive(false);
            GetComponent<UIWidget>().alpha = 0.5F;
        }

        //если выбран текущий скин или шапка или ягода
        if (initClass.progress[name] == 2) {
            pressCard(false);
        }
    }

    void pressCard(bool isPressed) {
        if (!isPressed) {
            //stop all animation
            for (int i = 0; i < 5; i++) {
                Transform prevObject = transform.parent.GetChild(i);
                if (initClass.progress[name] >= 1) prevObject.GetChild(1).gameObject.SetActive(false);
                if (prevObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("card selected") ||
                    prevObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("card select")
                    )
                    prevObject.GetComponent<Animator>().Play("default");
            }

            //start включаем preview текущий скин и выключаем все остальные
            string skinName = name;
            if (name.Substring(0, 3) == "hat") skinName = staticClass.currentSkin;
            Transform previewObj = transform.parent.parent.GetChild(1).GetChild(0);
            for (int i = 0; i < 5; i++) {
                if (previewObj.GetChild(i).name == skinName) {
                    previewObj.GetChild(i).gameObject.SetActive(true);
                    //включаем описание скина
                    previewObj.GetChild(i + 5).gameObject.SetActive(true);
                    if (name.Length != 6) previewObj.GetChild(i).GetComponent<Animator>().Play("spider hi");
                    if (name.Substring(0, 3) == "hat") {
                        //включаем текущую шапку и выключаем все остальные
                        for (int j = 0; j < 4; j++) {
                            if (previewObj.GetChild(i).GetChild(0).GetChild(j).name == name) {
                                previewObj.GetChild(i).GetChild(0).GetChild(j).gameObject.SetActive(true);
                            }
                            else
                                previewObj.GetChild(i).GetChild(0).GetChild(j).gameObject.SetActive(false);
                        }
                    }

                }
                else {
                    previewObj.GetChild(i).gameObject.SetActive(false);
                    previewObj.GetChild(i + 5).gameObject.SetActive(false);
                }

            }
            //end включаем preview текущий скин и выключаем все остальные

            GetComponent<Animator>().Play("card select");
            //если куплен, то выбираем
            if (initClass.progress[name] >= 1) {
                transform.GetChild(1).gameObject.SetActive(true);

                // = 1 и запись в static
                if (name.Substring(0, 4) == "skin") {
                    initClass.progress[staticClass.currentSkin] = 1;
                    staticClass.currentSkin = name;
                    staticClass.changeSkin();
                }
                else if (name.Substring(0, 3) == "hat") {
                    initClass.progress[staticClass.currentHat] = 1;
                    staticClass.currentHat = name;
                    staticClass.changeHat();
                }
                else if (name.Substring(0, 5) == "berry") {
                    initClass.progress[staticClass.currentBerry] = 1;
                    staticClass.currentBerry = name;
                    staticClass.changeBerry();
                }
                initClass.progress[name] = 2;
                initClass.saveProgress();

                //выключаем get booster
                transform.parent.parent.GetChild(1).GetChild(1).gameObject.SetActive(false);
            }
            else
                transform.parent.parent.GetChild(1).GetChild(1).gameObject.SetActive(true);

        }
    }

    void openCard(bool isPressed) {
        if (!isPressed) {
            if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("card idle")) {
                GetComponent<Animator>().Play("card open");
                mBoosterClass.counterOpenCard++;
                if (mBoosterClass.counterOpenCard >= 5) {
                    Debug.Log("counterOpenCard >= 5");
                    transform.parent.parent.FindChild("exit open booster menu").localPosition = new Vector3(0, 0, -1);
                    mBoosterClass.counterOpenCard = 0;
                }
            }
        }
    }

    void buyCardForCoins() {
        int amount = 1;
        int cost = 100;
        if (initClass.progress["coins"] < cost) transform.GetChild(0).GetComponent<Animator>().Play("alpha disable");
        else {
            initClass.progress["coins"] -= cost;
            initClass.progress["boosters"] += amount;
            initClass.saveProgress();
            marketClass.instance.boostersLabel.text = initClass.progress["boosters"].ToString();

        }
    }
    public IEnumerator CoroutineLoadLevel() {
        Time.timeScale = 0;
        Application.backgroundLoadingPriority = ThreadPriority.Low;
        AsyncOperation async = new AsyncOperation();

        if (name == "start level menu") async = Application.LoadLevelAsync("level menu");
        else if (name == "button back") async = Application.LoadLevelAsync("menu");
        if (name == "restart") async = Application.LoadLevelAsync(Application.loadedLevel);
        else if (name == "button next") {
            async = Application.LoadLevelAsync("level menu");
        }
        else if (name == "button play 0") {
            initLevelMenuClass.levelDemands = 0;
            async = Application.LoadLevelAsync("level" + initClass.progress["currentLevel"]);
        }
        else if (name == "button play 1") {
            initLevelMenuClass.levelDemands = 1;
            async = Application.LoadLevelAsync("level" + initClass.progress["currentLevel"]);
        }
        else if (name.Substring(0, 5) == "level") {
            if (initClass.progress["lastLevel"] >= Convert.ToInt32(name.Substring(5)) - 1) async = Application.LoadLevelAsync("level" + Convert.ToInt32(name.Substring(5)));
        }
        async.allowSceneActivation = false;
        yield return StartCoroutine(staticClass.waitForRealTime(0.5F));
        async.allowSceneActivation = true;
        yield return async;


    }

    void pressMarketItem(bool isPressed) {
        if (!isPressed) {
            GetComponent<Animator>().Play("button");
            Debug.Log(GetComponent<Animator>());
            Debug.Log(44);
        }
        //off for tests
        //if (onFinished != null) EventDelegate.Execute(onFinished);

        //old code
        //transform.parent.GetComponent<UIScrollView>().Press(isPressed);
        //if (!isPressed) {
        //	GetComponent<UIPlayAnimation>().enabled = true;
        //	GetComponent<UIButtonScale>().enabled = true;
        //	enableDrag = false;
        //}
        //timeSinceTouch = Time.time;
    }


    void OnDestroy() {
    }


    void initSettings() {
        if (name == "music button") {
            if (initClass.progress["music"] == 1) transform.GetChild(0).gameObject.SetActive(false);
            else transform.GetChild(0).gameObject.SetActive(true);
        }
        if (name == "soung button") {
            if (initClass.progress["sound"] == 1) transform.GetChild(0).gameObject.SetActive(false);
            else transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    void clickSound() {
        if (initClass.progress["sound"] == 0) {
            initClass.setSound(true);
            initClass.progress["sound"] = 1;
            initClass.saveProgress();
            transform.GetChild(0).gameObject.SetActive(false);
        }
        else {
            initClass.setSound(false);
            initClass.progress["sound"] = 0;
            initClass.saveProgress();
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    void clickMusic() {
        if (initClass.progress["music"] == 0) {
            GameObject.Find("music").GetComponent<AudioSource>().enabled = true;
            initClass.progress["music"] = 1;
            initClass.saveProgress();
            transform.GetChild(0).gameObject.SetActive(false);
        }
        else {
            GameObject.Find("music").GetComponent<AudioSource>().enabled = false;
            initClass.progress["music"] = 0;
            initClass.saveProgress();
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }



    void showAchievements() {
        //NGUIDebug.Log("mGooglePlayClass OnClick");
        GooglePlayManager.instance.ShowAchievementsUI();
    }

    void showLeaderboards() {
        GooglePlayManager.instance.ShowLeaderBoardsUI();
    }

    void connectGooglePlay() {
        try {
            if (initClass.progress["googlePlay"] == 0) {
                GooglePlayConnection.instance.connect();
            }
            else {
                GooglePlayConnection.instance.disconnect();
            }

        }
        catch (System.Exception ex) {
            Debug.Log(ex.StackTrace);
        }
    }

    void resetProgress() {

        //сброс прогресса
        PlayerPrefs.SetString("progress", staticClass.strProgressDefault);
        initClass.getProgress();
        GooglePlayManager.instance.ResetAllAchievements();
        GooglePlayManager.instance.SubmitScore("leaderboard_test_leaderboard", 0);
    }

    void loadLevel() {
        StartCoroutine(CoroutineLoadLevel());
    }



    void openMenu() {
        Debug.Log("openMenu: " + name);
        GameObject menu = null;
        //if (name == "button market") marketClass.instance.gameObject.SetActive(true);
        if (name == "button market") {
            marketClass.instance.transform.position = new Vector3(0, 0, -1);
        }
        else if (name == "next level menu") {
            menu = GameObject.Find("level menu");
            menu.SetActive(false);
            menu = transform.parent.parent.GetChild(3).gameObject;
            menu.SetActive(true);
        }
        else if (name == "prev level menu") {
            menu = GameObject.Find("level menu 2");
            menu.SetActive(false);
            menu = transform.parent.parent.GetChild(2).gameObject;
            menu.SetActive(true);
        }
        else if (name == "pause") {
            menu = transform.parent.GetChild(1).gameObject;
            menu.SetActive(true);
            Time.timeScale = 0;
        }
        else if (name == "play") {
            menu = GameObject.Find("pause menu");
            menu.SetActive(false);
            Time.timeScale = 1;

        }
        else if (name == "exit energy menu") GameObject.Find("energyLabel").SendMessage("stopCoroutineEnergyMenu");
        else if (name == "button settings") GameObject.Find("settings folder").transform.GetChild(0).gameObject.SetActive(true);
        else if (name == "open booster") {
            if (initClass.progress["boosters"] > 0) {
                marketClass.instance.openBoosterMenu.SetActive(true);
                marketClass.instance.boosterMenu.SetActive(false);
            }

        }
    }
    public void closeMenu() {
        StartCoroutine(coroutineCloseMenu());
    }

    public IEnumerator coroutineCloseMenu() {
        yield return new WaitForSeconds(0F);
        Debug.Log(name);
        GameObject menu = null;
        if (name == "button exit level menu") {
            menu = GameObject.Find("level menu");
            menu.transform.GetChild(0).GetComponent<Animator>().Play("menu exit");
            yield return new WaitForSeconds(0.2F);
            menu.SetActive(false);
        }
        else if (name == "next level menu") {
            menu = GameObject.Find("level menu");
            menu.SetActive(false);
            menu = transform.parent.parent.GetChild(3).gameObject;
            menu.SetActive(true);
        }
        else if (name == "prev level menu") {
            menu = GameObject.Find("level menu 2");
            menu.SetActive(false);
            menu = transform.parent.parent.GetChild(2).gameObject;
            menu.SetActive(true);
        }
        else if (name == "pause") {
            menu = transform.parent.GetChild(1).gameObject;
            menu.SetActive(true);
            Time.timeScale = 0;
        }
        else if (name == "play") {
            menu = GameObject.Find("pause menu");
            yield return StartCoroutine(staticClass.waitForRealTime(0.2F));
            menu.SetActive(false);
            if (gYetiClass.yetiState == "") Time.timeScale = 1;

        }
        else if (name == "exit energy menu") {
            GameObject.Find("energy menu").transform.GetChild(0).GetComponent<Animator>().Play("menu exit");
            yield return new WaitForSeconds(0.2F);
            GameObject.Find("energy").SendMessage("stopCoroutineEnergyMenu");


        }
        else if (name == "exit thanks menu") {
            menu = GameObject.Find("root/Camera/UI Root/thanks menu");
            if (menu != null) {
                menu.transform.GetChild(0).GetComponent<Animator>().Play("menu exit");
                yield return new WaitForSeconds(0.2F);
                Destroy(menu);
            }
            else {
                menu = marketClass.instance.thanksMenu;
                menu.transform.GetChild(0).GetComponent<Animator>().Play("menu exit");
                yield return new WaitForSeconds(0.2F);
                menu.SetActive(false);
            }

        }
        else if (name == "button settings exit") transform.parent.parent.gameObject.SetActive(false);
        //else if (name == "button market exit") marketClass.instance.gameObject.SetActive(false); 
        else if (name == "button market exit") marketClass.instance.transform.position = new Vector3(0, 0, -10000);
        else if (name == "exit open booster menu") {
            marketClass.instance.openBoosterMenu.SetActive(false);
            marketClass.instance.boosterMenu.SetActive(true);
        }
    }

    //public IEnumerator CoroutineCloseMenu(){

    //}

    void selectLanguage() {
        Localization.language = name;
    }


    void buyEnergy() {
        marketClass.instance.item = gameObject;
        marketClass.instance.purchaseForCoins();
    }

    void clickBonusesArrow() {
        if (name == "arrow right") {
            gameObject.SetActive(false);
            GameObject.Find("bonuses/tween").transform.GetChild(1).gameObject.SetActive(true);
        }
        else {
            gameObject.SetActive(false);
            GameObject.Find("bonuses/tween").transform.GetChild(0).gameObject.SetActive(true);
        }

    }



}
