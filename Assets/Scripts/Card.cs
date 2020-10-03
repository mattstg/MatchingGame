using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class Card : MonoBehaviour
{
    bool parentIsGreen;
    [HideInInspector] public bool isGreen;
    bool followCursor = false;

    public void InitializeCard(bool parentIsGreen)
    {
        this.parentIsGreen = parentIsGreen;
        isGreen = Random.value > .5f;
        StartCoroutine(RevealColor());
        GetComponentInChildren<Text>().text = Random.Range(0, 100).ToString();
    }

    public void OnClick()
    {
        if (MainScript.instance.currentGameStage == GameStage.GuessingPhase)
        {
            parentIsGreen = !parentIsGreen; //flip it
            transform.SetParent(MainScript.instance.GetLayout(parentIsGreen).transform);
        }
    }

    IEnumerator RevealColor()
    {
        GetComponent<Image>().color = (isGreen) ? Color.green : Color.red;
        yield return new WaitForSeconds(MainScript.REVEAL_TIME);
        GetComponent<Image>().color = Color.white;
    }
}
