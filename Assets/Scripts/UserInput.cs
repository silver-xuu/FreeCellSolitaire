using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UserInput : MonoBehaviour
{

    public GameObject selected;
    public Vector3 originPosition;
    public Solitaire solitaire;

    private Vector3 mousePosition;
    // Start is called before the first frame update
    void Start()
    {
        //solitaire = FindObjectOfType<Solitaire>();
        solitaire = Solitaire.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        MouseInput();
    }
    void MouseInput()
    {

        if (Input.GetMouseButtonDown(0))
        {
            if (selected != null)
                ResetCard(selected);
            //get origin mouse position for drag calculation
            mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
            if (hit)
            {

                if (hit.collider.CompareTag("Card"))
                {
                    Card(hit.collider.gameObject);
                }
                if (hit.collider.CompareTag("Foundation"))
                {
                    selected = null;
                }

            }
            else
            {
                selected = null;
            }
        }

        //when dragging, card moves with mouse
        if (Input.GetMouseButton(0))
        {
            float deltaX = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f)).x - mousePosition.x;
            float deltaY = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f)).y - mousePosition.y;

            if (selected != null)
                selected.transform.position = new Vector3(originPosition.x + deltaX, originPosition.y + deltaY, -0.35f);

        }

        if (Input.GetMouseButtonUp(0))
        {
            if (selected != null&&selected.transform.position!=originPosition)
            {
                Debug.Log("mouse up");
                mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
                RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
                if (hit)
                {
                    //when card drag to another card
                    if (hit.collider.CompareTag("Card"))
                    {
                        Debug.Log(hit.collider.name);
                        if (Stackable(selected, hit.collider.gameObject))
                        {
                            
                            ICommand command = new StackCommand(selected,hit.collider.gameObject,originPosition);
                            CommandInvoker.AddCommand(command);
                            CardColorWhite(selected);
                            selected.layer = 0;
                            selected = null;

                        }
                    }
                    //when card drag to foundation area
                    if (hit.collider.CompareTag("Foundation"))
                    {
                        int foundationIndx = solitaire.IntoAllFoundations(selected);
                        if (foundationIndx == -1)
                        {
                            ResetCard(selected);

                        }
                        else
                        {
                            ICommand command = new FoundationCommand(selected, originPosition, foundationIndx);
                            CommandInvoker.AddCommand(command);
                        }
                        CardColorWhite(selected);
                        selected = null;
                    }
                    //when card drag to an empty cascade
                    if (hit.collider.CompareTag("Empty Cascade"))
                    {
                        //check if the cascade is really empty
                        if (hit.collider.transform.childCount == 0)
                        {
                            
                            ICommand command = new EmptyCascadeCommand(selected, hit.collider.gameObject,originPosition);
                            CommandInvoker.AddCommand(command);
                            CardColorWhite(selected);
                            selected.layer = 0;
                            selected = null;
                        }


                    }
                    //when card drag to free cell
                    if (hit.collider.CompareTag("Free Cell"))
                    {
                        if (selected.transform.childCount == 0)
                        {
                            int freeCellIndx = hit.collider.transform.GetSiblingIndex();
                            if (solitaire.freeCells[freeCellIndx] == "")
                            {
                                ////remove from the free cell if the card is in it
                                ICommand command = new FreeCellCommand(freeCellIndx, selected,originPosition);
                                CommandInvoker.AddCommand(command);

                                CardColorWhite(selected);
                                selected.layer = 0;
                                selected = null;
                            }
                        }



                    }
                }
                else
                {
                    ResetCard(selected);
                    //selected = null;
                }

                if (selected != null)
                {
                    ResetCard(selected,false);
                }


            }


        }

    }
    void Card(GameObject card)
    {
        if (card.GetComponent<CardFace>().selectable)
        {


            if (selected == null || selected != card)
            {

                selected = card;
                originPosition = selected.transform.position;
                CardColorYellow(selected);

                //ignore by raycast if they are being moveds
                selected.layer = 2;
            }
            else if (selected == card)
            {
                int foundationIndx = solitaire.IntoAllFoundations(selected);
                if (selected.transform.childCount == 0 && foundationIndx == -1)
                {
                    //when the same card clicked twice
                    //put the card into fountains if possible
                    //otherwise put the card into available free cell
                    //don't move the card if it already in the free cell 
                    int freeCellIndx = solitaire.FreeCell("");

                    if (freeCellIndx != -1)
                    {
                        ICommand command = new FreeCellCommand(freeCellIndx, selected, originPosition);
                        CommandInvoker.AddCommand(command);
                    }
                    
                }else if (foundationIndx != -1)
                {
                    ICommand command = new FoundationCommand(selected, originPosition, foundationIndx);
                    CommandInvoker.AddCommand(command);
                }

                selected.layer = 0;
                selected = null;
            }



        }

        else if (selected != null)
        {
            CardColorWhite(selected);
            selected = null;
            //warn the player that this card is not selectable
        }
    }

    // deteremine if two cards are stackable 
    bool Stackable(GameObject selectedCard, GameObject currentCard)
    {
        CardFace selectedCardFace = selectedCard.GetComponent<CardFace>();
        CardFace currentCardFace = currentCard.GetComponent<CardFace>();

        if (!currentCardFace.selectable)
            return false;

        if (selectedCardFace.suit == currentCardFace.suit)
        {
            return false;
        }
        else
        {
            if (selectedCardFace.suit == "D" && currentCardFace.suit == "H")
                return false;
            if (selectedCardFace.suit == "H" && currentCardFace.suit == "D")
                return false;
            if (selectedCardFace.suit == "C" && currentCardFace.suit == "S")
                return false;
            if (selectedCardFace.suit == "S" && currentCardFace.suit == "C")
                return false;
        }
        if (selectedCardFace.value == (currentCardFace.value - 1))
            return true;

        return false;
    }
    private void CardColorYellow(GameObject card)
    {
        card.GetComponent<SpriteRenderer>().color = Color.yellow;

        if (card.transform.childCount > 0)
        {
            foreach (Transform child in card.transform)
            {
                CardColorYellow(child.gameObject);
            }
        }

    }
    private void CardColorWhite(GameObject card)
    {

        card.GetComponent<SpriteRenderer>().color = Color.white;
        if (card.transform.childCount > 0)
        {
            foreach (Transform child in card.transform)
            {
                CardColorWhite(child.gameObject);
            }
        }

    }
    private void ResetCard(GameObject card,bool resetColor=true)
    {
        if(resetColor)
        CardColorWhite(card);

        card.transform.position = originPosition;
        card.layer = 0;
        //selected = null;
    }

    public static Transform GetSiblingCard(Transform selectedCard)
    {
        if (selectedCard.GetSiblingIndex() > 0)
        {
            Transform sibling = selectedCard.transform.parent.GetChild(selectedCard.transform.GetSiblingIndex() - 1);


            sibling.GetComponent<CardFace>().selectable = true;

            CardFace[] allChildrenCard = sibling.GetComponentsInChildren<CardFace>();
            foreach (CardFace cardFace in allChildrenCard)
            {
                cardFace.selectable = true;
            }
            return sibling;
        }
        return null;
    }

}

