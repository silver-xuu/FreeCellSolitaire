using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solitaire : MonoBehaviour
{
    /* Public variables goes here */
    public Sprite[] cards;
    public GameObject cardPrefab;

    public Transform[] cascadesPos;
    public Transform[] freeCellPos;
    public Transform[] fountainPos;

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

    [SerializeField]
    private string[] fountains;

    // Start is called before the first frame update
    void Start()
    {
        cascades = new List<string>[] { cascades1, cascades2, cascades3, cascades4, cascades5, cascades6, cascades7, cascades8 };
        fountains = new string[4] { "C A", "D A", "H A", "S A" };
        PlayCards();
    }

    // Update is called once per frame
    void Update()
    {


    }

    public void PlayCards()
    {
        deck = GenerateDeck();
        ShuffleDeck(deck);
        SolitaireSort();
        SolitaireDeal();
        
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
            foreach(string card in cascades[i])
            {
                GameObject newCard = Instantiate(cardPrefab, new Vector3(cascadesPos[i].position.x, cascadesPos[i].position.y + yOffset, cascadesPos[i].position.z + zOffset), Quaternion.identity, cascadesPos[i]);
                newCard.name = card;


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
    public bool IntoAllFountains(GameObject card)
    {

        for(int i = 0; i < 4; i++)
        {

                if (IntoFountain(card, i))
                    return true;
            
        }

        return false;
    }
    public bool IntoFountain(GameObject card,int index)
    {
        if (fountains[index] == card.name)
        {
            bool isFinshed = false;
            // change the new last child of the column to be selectable
            if (card.transform.GetSiblingIndex() > 0)
                card.transform.parent.GetChild(card.transform.GetSiblingIndex() - 1).GetComponent<CardFace>().selectable = true;
            int freeCellIndx = FreeCell(card.name);
            if (freeCellIndx != -1)
                freeCells[freeCellIndx] = "";

            card.transform.position = fountainPos[index].position-new Vector3(0,0,0.01f);
            card.tag = "Foundation";
            card.transform.SetParent(fountainPos[index]);
            fountainPos[index] = card.transform;
            string[] names = fountains[index].Split(' ');
            if(names[1] != "J" && names[1] != "Q" && names[1] != "K" && names[1] != "A" && names[1] != "10")
            {
                names[1] = (int.Parse(names[1]) + 1).ToString();
            }
            else if(names[1] == "A")
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
            else if (names[1] == "K")
            {
                isFinshed = true;
                finishCount++;
            }
            if (!isFinshed)
                fountains[index] = names[0] + ' ' + names[1];
            else
            {
                fountains[index] = "";
            }

            return true;
        }
        return false;
    }
}
