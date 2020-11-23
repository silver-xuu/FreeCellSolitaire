using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Solitaire : Singleton<Solitaire>
{

    /* Public variables goes here */
    public Sprite[] cards;
    public GameObject cardPrefab;


    public Transform[] cascadesPos;
    public Transform[] freeCellPos;
    public Transform[] foundationPos;

    public static string[] suits = new string[] { "C", "D", "H", "S"};
    public static string[] values = new string[] { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };

    public static int finishCount = 0;

    public List<string>[] cascades;
    public string[] freeCells = new string[4] {"","","",""};
    

    public List<string> deck;

    /* Private variables goes here*/
    private List<string> cascades1 = new List<string>();
    private List<string> cascades2 = new List<string>();
    private List<string> cascades3 = new List<string>();
    private List<string> cascades4 = new List<string>();
    private List<string> cascades5 = new List<string>();
    private List<string> cascades6 = new List<string>();
    private List<string> cascades7 = new List<string>();
    private List<string> cascades8 = new List<string>();

    public string[] foundations;

    // Start is called before the first frame update
    void Start()
    {
        cascades = new List<string>[] { cascades1, cascades2, cascades3, cascades4, cascades5, cascades6, cascades7, cascades8 };
        foundations = new string[4] { "C A", "D A", "H A", "S A" };
        PlayCards();
    }

    public void PlayCards()
    {
        deck = GenerateDeck();
        ShuffleDeck(deck);
        SolitaireSort();
        SolitaireDeal();
        
    }
    private void Update()
    {
        //switch to finish scene when win
        if (finishCount >= 4)
        {
            endScene(1);
            TimeManager.Instance.SaveTimeStampToJson();
        }
        
    }

    //generate all cards in deck
    public static List<string> GenerateDeck()
    {
        List<string> newDeck = new List<string>();
        foreach (string s in suits) { 
            foreach(string v in values)
            {
                newDeck.Add(s +" "+ v);
            }

        }

        return newDeck;
    }



    //https://stackoverflow.com/questions/273313/randomize-a-listt
    //shuffle the list
    private void ShuffleDeck<T>(List<T> deckList)
    {
        System.Random random = new System.Random();
        int n = deckList.Count;
        while (n > 1)
        {
            int k = random.Next(n);
            n--;
            T temp = deckList[k];
            deckList[k] = deckList[n];
            deckList[n] = temp;
        }
    }

    //display cards
    private void SolitaireDeal()
    {
        for(int i = 0; i < 8; i++)
        {
            float yOffset = 0f;
            float zOffset = -0.05f;
            //foreach(string card in cascades[i])
            //{
            //    GameObject newCard = Instantiate(cardPrefab, new Vector3(cascadesPos[i].position.x, cascadesPos[i].position.y + yOffset, cascadesPos[i].position.z + zOffset), Quaternion.identity, cascadesPos[i]);
            //    newCard.name = card;


            //    yOffset -= 0.43f;
            //    zOffset -= 0.01f;
            //}
            for(int j = 0; j < cascades[i].Count; j++)
            {
                GameObject newCard;
                // if the cards are stackable, add it to the most bottom to the game object 
                if (Stackable(i, j))
                {
                    Debug.Log(Stackable(i, j));
                    Transform targetParent = cascadesPos[i];
                    //cascadesPos[i].GetChild(cascadesPos[i].childCount - 1);
                    while (targetParent.childCount > 0)
                    {
                        targetParent = targetParent.GetChild(targetParent.childCount - 1);
                    }
                    newCard = Instantiate(cardPrefab, targetParent.position- new Vector3(0, 0.43F, 0.01f), Quaternion.identity, targetParent);
                    newCard.transform.localScale = new Vector3(1f, 1f, 1f);
                    
                }
                else
                {
                    newCard = Instantiate(cardPrefab, new Vector3(cascadesPos[i].position.x, cascadesPos[i].position.y + yOffset, cascadesPos[i].position.z + zOffset), Quaternion.identity, cascadesPos[i]);
                }
                newCard.name = cascades[i][j];
                    
                yOffset -= 0.43f;
                zOffset -= 0.01f;
            }
        }
    }

    //sort the solitare into different cascades
    private void SolitaireSort()
    {
        int cascadesNumber = 0;
        foreach(string card in deck)
        {
            cascades[cascadesNumber].Add(card);
            cascadesNumber++;
            if (cascadesNumber == 8)
                cascadesNumber = 0;
        }
    }
    public int FreeCell(string find)
    {
        for (int i = 0; i < 4; i++)
        {
            if (freeCells[i]==find)
                return i;
        }
        return -1;
    }
    public int IntoAllFoundations(GameObject card)
    {

        for(int i = 0; i < 4; i++)
        {

                if (IntoFoundation(card, i))
                    return i;
            
        }

        return -1;
    }
    public bool IntoFoundation(GameObject card,int index)
    {
        if (foundations[index] == card.name)
        {

            return true;
        }
        return false;
    }
    bool Stackable(int cascadeIndex, int newCardIndex)
    {
        if (newCardIndex > 0)
        {
            string previousCard = cascades[cascadeIndex][newCardIndex - 1];
            string[] previousCardName = previousCard.Split(' ');
            string newCard = cascades[cascadeIndex][newCardIndex];
            string[] newCardName = newCard.Split(' ');
            if (previousCardName[0] == newCardName[0])
            {
                return false;
            }
            else
            {
                if (previousCardName[0] == "S" && newCardName[0] == "C")
                    return false;
                if (previousCardName[0] == "C" && newCardName[0] == "S")
                    return false;
                if (previousCardName[0] == "H" && newCardName[0] == "D")
                    return false;
                if (previousCardName[0] == "D" && newCardName[0] == "H")
                    return false;
            }
            if (ConvertCardName(previousCardName[1]) == ConvertCardName(newCardName[1]) + 1)
            {
                return true;
            }
                
        }

        return false;
    }
    int ConvertCardName(string name)
    {
        if (name == "A")
            return 1;
        if (name == "J")
            return 11;
        if (name == "Q")
            return 12;
        if (name == "K")
            return 13;

        return int.Parse(name);
    }
    public string NextCardFace(string card)
    {
        string[] names = card.Split(' ');
        if (names[1] != "J" && names[1] != "Q" && names[1] != "K" && names[1] != "A" && names[1] != "10")
        {
            names[1] = (int.Parse(names[1]) + 1).ToString();
        }
        else if (names[1] == "A")
        {
            names[1] = "2";
        }
        else if (names[1] == "10")
        {
            names[1] = "J";
        }
        else if (names[1] == "J")
        {
            names[1] = "Q";
        }
        else if (names[1] == "Q")
        {
            names[1] = "K";
        }
        return names[0] + ' ' + names[1];
        
    }
    public void endScene(int isFinished)
    {
        GameSceneManager.SwitchScene(2);
        PlayerPrefs.SetInt("isFinished", isFinished);
    }
}
