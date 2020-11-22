using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CardFace : MonoBehaviour
{

    public bool selectable;
    public string suit;
    public int value;
   

    private Solitaire solitaire;
    private string[] names;
    // Start is called before the first frame update
    void Start()
    {
        selectable = false;
        List<string> deck = Solitaire.GenerateDeck();
        solitaire = FindObjectOfType<Solitaire>();
        int index = deck.IndexOf(this.name);
        this.GetComponent<SpriteRenderer>().sprite = solitaire.cards[index];
        // change the card to be selectable if it's the last in the list
        foreach (List<string> cascade in solitaire.cascades)
        {
            if (cascade.Last<string>() == this.name)
            {
                selectable = true;
                CardFace[] cardFaces= this.GetComponentsInParent<CardFace>();
                foreach(CardFace cardFace in cardFaces)
                {
                    cardFace.selectable = true;
                }
            }
        }
        names = this.name.Split(' ');
        suit = names[0];
        if(names[1]!="J"&& names[1] != "Q" && names[1] != "K"&&names[1]!="A")
        {
            value = int.Parse(names[1]);
        }
        else
        {
            if (names[1] == "A")
                value = 1;
            if (names[1] == "J")
                value = 11;
            if (names[1] == "Q")
                value = 12;
            if (names[1] == "K")
                value = 13;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
