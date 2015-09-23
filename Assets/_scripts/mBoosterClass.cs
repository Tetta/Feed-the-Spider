using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class mBoosterClass : MonoBehaviour {

    public GameObject[] cards = new GameObject[5];
    public Transform icons;
    public static int counterOpenCard;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    void OnEnable() {
        //default cards
        for (int i = 0; i < 5; i++) {
            if (cards[i].transform.GetChild(0).childCount != 0) Destroy(cards[i].transform.GetChild(0).GetChild(0).gameObject);
            cards[i].transform.rotation = Quaternion.identity;
            cards[i].transform.GetChild(0).gameObject.SetActive(false);
            cards[i].SetActive(false);

        }
        transform.parent.parent.FindChild("exit open booster menu").localPosition = new Vector3(0, 0, -10000);
        //показываем бустер
        transform.GetChild(0).gameObject.SetActive(true);
        GetComponent<BoxCollider>().enabled = true;

        Debug.Log("OnEnable booster");
    }

    void OnPress(bool isPressed) {
        GameObject icon = new GameObject();

        if (!isPressed) {
            //GameObject bonusesTemp = GameObject.Instantiate(GameObject.Find("bonuses temp"), new Vector2(0, 0), Quaternion.identity) as GameObject;
            //bonusesTemp.transform.parent = giftMenu.transform.GetChild(0).GetChild(2);
            //bonusesTemp.transform.localScale = new Vector3(1, 1, 1);
            //шансы на картах
            Dictionary<string, int> portions = new Dictionary<string, int>();
            portions["hints_1"] = 30; portions["hints_2"] = 15; portions["hints_3"] = 10;
            portions["webs_1"] = 150; portions["webs_2"] = 75; portions["webs_3"] = 50;
            portions["teleports_1"] = 150; portions["teleports_2"] = 75; portions["teleports_3"] = 50;
            portions["collectors_1"] = 150; portions["collectors_2"] = 75; portions["collectors_3"] = 50;
            portions["coins_50"] = 150; portions["coins_100"] = 75; portions["coins_250"] = 30;
            portions["energy_5"] = 150; portions["energy_10"] = 75; portions["energy_15"] = 30;
            portions["skin2_0"] = 30; portions["skin3_0"] = 30; portions["skin4_0"] = 30; portions["skin5_0"] = 30;
            portions["berry2_0"] = 30; portions["berry3_0"] = 30; portions["berry4_0"] = 30; portions["berry5_0"] = 30;
            portions["hat2_0"] = 30; portions["hat3_0"] = 30; portions["hat4_0"] = 30; portions["hat5_0"] = 30;
            //Vector2[] positions = {new Vector2[], 6.6, 7.7};
            //Vector3[] positions = { new Vector3(-50, -70, 0), new Vector3(250, 220, 0), new Vector3(-290, 240, 0), new Vector3(300, -320, 0), new Vector3(-220, -370, 0) };
            for (int i = 0; i < 5; i++) {
                cards[i].SetActive(true);
                int counter = 0;
                int bonusRand = Mathf.CeilToInt(UnityEngine.Random.Range(0, 1840));

                foreach (var portion in portions) {
                    if (bonusRand >= counter && bonusRand < counter + portion.Value) {
                        //название карты и количество
                        string bonusName = portion.Key.Split(new Char[] { '_' })[0];
                        int bonusCount = int.Parse(portion.Key.Split(new Char[] { '_' })[1]);
                        Debug.Log("--------------");
                        Debug.Log(bonusName);
                        Debug.Log(bonusCount);
                        //иконка
                        icon = Instantiate(icons.FindChild(bonusName).gameObject, icons.FindChild(bonusName).transform.position + cards[i].transform.position, Quaternion.identity) as GameObject;
                        Debug.Log(icon);
                        icon.transform.parent = cards[i].transform.GetChild(0);
                        icon.transform.localScale = new Vector2(1, 1);
                        icon.transform.localPosition = new Vector3(icon.transform.localPosition.x, icon.transform.localPosition.y, 0);
                        icon.SetActive(true);
                        //сохранение результата
                        if (bonusName == "hints" || bonusName == "webs" || bonusName == "teleports" || bonusName == "collectors" || bonusName == "coins") initClass.progress[bonusName] += bonusCount;
                        else if (bonusName == "energy") {
                            initClass.progress["energyTime"] -= bonusCount * lsEnergyClass.costEnergy;
                            initClass.progress["energy"] += bonusCount;
                        }
                        else {
                            if (initClass.progress[bonusName] == 0) initClass.progress[bonusName] = 1;
                            else
                                //если есть, начислять монеты. поменять количество и добавить всплывающую иконку монет.
                                initClass.progress["coins"] += 100;
                        }

                        break;
                    }

                    counter += portion.Value;
                }




            }

            initClass.progress["boosters"]--;
            marketClass.instance.boostersLabel.text = initClass.progress["boosters"].ToString();
            initClass.saveProgress();
            //убираем бустер
            transform.GetChild(0).gameObject.SetActive(false);
            GetComponent<BoxCollider>().enabled = false;
        }

    }


}
