using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameStage { RevealPhase, GuessingPhase}
public class MainScript : MonoBehaviour
{
    public static MainScript instance;

    public Canvas mainCanvas;
    public GameStage currentGameStage;
    public static readonly float REVEAL_TIME = 6f;
    public Text levelText;
    public Image guessButton;

    public GameObject cardResource;
    public int score;
    public int currentLevel; //number of cards

    public GridLayoutGroup greenLayout;
    public GridLayoutGroup redLayout;

    private void Awake()
    {
        instance = this;
        InitializeLevel(currentLevel);
    }

    private void Update()
    {
        
    }

    public GridLayoutGroup GetLayout(bool green)
    {
        return (green) ? greenLayout : redLayout;
    }

    private void InitializeLevel(int lvl)
    {
        currentGameStage = GameStage.RevealPhase;
        foreach (Transform t in greenLayout.transform)
            GameObject.Destroy(t.gameObject);
        foreach (Transform t in redLayout.transform)
            GameObject.Destroy(t.gameObject);

        for(int i = 0; i < currentLevel * 2; i++)
        {
            bool isGreenparent = (Random.value > .5f) ? greenLayout.transform : redLayout.transform;
            Card newCard = GameObject.Instantiate(cardResource,GetLayout(isGreenparent).transform).GetComponent<Card>();
            newCard.InitializeCard(isGreenparent);
        }

        Invoke("SetMainStage", REVEAL_TIME);
    }

    void SetMainStage()
    {
        currentGameStage = GameStage.GuessingPhase;
    }
    
    public void SubmitGuess()
    {
        if(currentGameStage == GameStage.GuessingPhase)
        {
            bool allCorrect = AreAllCorrect();
            StartCoroutine(ColorGuessButton(allCorrect));
            if (allCorrect)
            {
                currentLevel++;
            }
            InitializeLevel(currentLevel);
        }
    }

    IEnumerator ColorGuessButton(bool correct)
    {
        float timeStarted = Time.time;
        Color lerpColor = (correct) ? Color.green : Color.red;
        float p = 0;
        while (p < 1)
        {
            p = (Time.time - timeStarted) / (2);
            guessButton.color = Color.Lerp(lerpColor, Color.white, p);
            yield return null;
        }
        guessButton.color = Color.white;
    }

    bool AreAllCorrect()
    {
        foreach (Transform t in greenLayout.transform)
            if (!t.GetComponent<Card>().isGreen)
                return false;
        foreach (Transform t in redLayout.transform)
            if (t.GetComponent<Card>().isGreen)
                return false;
        return true;
    }
   
}
