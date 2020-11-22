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
        solitaire = FindObjectOfType<Solitaire>();
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
            if (selected != null)
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
                            // change the new last child of the column to be selectable
                            if (selected.transform.GetSiblingIndex() > 0)
                            {
                                Transform siblingCard = selected.transform.parent.GetChild(selected.transform.GetSiblingIndex() - 1);

                                siblingCard.GetComponent<CardFace>().selectable=true;
                                CardFace[] allChildrenCard = siblingCard.GetComponentsInChildren<CardFace>();
                                foreach (CardFace cardFace in allChildrenCard)
                                {
                                    cardFace.selectable = true;
                                }
                                //selected.transform.parent.GetChild(selected.transform.GetSiblingIndex() - 1).GetComponent<CardFace>().selectable = true;
                            }


                            //remove from the free cell if the card is in it
                            int freeCellIndx = solitaire.FreeCell(selected.name);
                            if (freeCellIndx != -1)
                                solitaire.freeCells[freeCellIndx] = "";

                            //move to stack
                            selected.transform.position = hit.transform.position - new Vector3(0f, 0.43f, 0.01f);
                            CardColorWhite(selected);
                            selected.transform.parent = hit.transform;
                            selected.transform.position = selected.transform.position - new Vector3(0, 0, 0.01f);
                            selected.layer = 0;
                            selected = null;

                        }
                    }
                    //when card drag to foundation area
                    if (hit.collider.CompareTag("Foundation"))
                    {

                        if (!solitaire.IntoAllFoundations(selected))
                        {
                            ResetCard(selected);

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
                            //remove from the free cell if the card is in it
                            int freeCellIndx = solitaire.FreeCell(selected.name);
                            if (freeCellIndx != -1)
                                solitaire.freeCells[freeCellIndx] = "";
                            // change the new last child of the column and its descendants to be selectable
                            if (selected.transform.GetSiblingIndex() > 0)
                            {
                                Transform siblingCard = selected.transform.parent.GetChild(selected.transform.GetSiblingIndex() - 1);

                                siblingCard.GetComponent<CardFace>().selectable = true;
                                CardFace[] allChildrenCard = siblingCard.GetComponentsInChildren<CardFace>();
                                foreach (CardFace cardFace in allChildrenCard)
                                {
                                    cardFace.selectable = true;
                                }
                                //selected.transform.parent.GetChild(selected.transform.GetSiblingIndex() - 1).GetComponent<CardFace>().selectable = true;
                            }

                            //move to empty cascades
                            selected.transform.position = hit.collider.transform.position - new Vector3(0, 0, 0.03f);
                            selected.transform.SetParent(hit.collider.transform);
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
                                //remove from the free cell if the card is in it
                                int freeCellIndx2 = solitaire.FreeCell(selected.name);
                                if (freeCellIndx2 != -1)
                                    solitaire.freeCells[freeCellIndx2] = "";

                                solitaire.freeCells[freeCellIndx] = selected.name;
                                // change the new last child of the column and its decesendants to be selectable
                                if (selected.transform.GetSiblingIndex() > 0)
                                {
                                    Transform siblingCard = selected.transform.parent.GetChild(selected.transform.GetSiblingIndex() - 1);

                                    siblingCard.GetComponent<CardFace>().selectable = true;
                                    CardFace[] allChildrenCard = siblingCard.GetComponentsInChildren<CardFace>();
                                    foreach (CardFace cardFace in allChildrenCard)
                                    {
                                        cardFace.selectable = true;
                                    }
                                    //selected.transform.parent.GetChild(selected.transform.GetSiblingIndex() - 1).GetComponent<CardFace>().selectable = true;
                                }



                                //move to free cell
                                selected.transform.position = solitaire.freeCellPos[freeCellIndx].position - new Vector3(0, 0, 0.1f);
                                selected.transform.SetParent(solitaire.freeCellPos[freeCellIndx]);

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
                    selected = null;
                }

                if (selected != null)
                {
                    ResetCard(selected);
                }
                
                
            }


        }

    }
    void Card(GameObject card)
    {
        if (card.GetComponent<CardFace>().selectable)
        {


            if (selected == null||selected!=card)
            {

                selected = card;
                originPosition = selected.transform.position;
                CardColorYellow(selected);

                //ignore by raycast if they are being moveds
                selected.layer = 2;
            }
            else if (selected == card)
            {
               
                if (selected.transform.childCount == 0 &&!solitaire.IntoAllFoundations(selected))
                {
                    //when the same card clicked twice
                    //put the card into fountains if possible
                    //otherwise put the card into available free cell
                    //don't move the card if it already in the free cell 
                    int freeCellIndx = solitaire.FreeCell("");
                    int freeCellIndx2 = solitaire.FreeCell(selected.name);
                   
                    if (freeCellIndx != -1 && freeCellIndx2 == -1)
                    {
                        solitaire.freeCells[freeCellIndx] = selected.name;
                        // change the new last child and its descendants of the column to be selectable
                        if (selected.transform.GetSiblingIndex() > 0)
                        {
                            Transform siblingCard = selected.transform.parent.GetChild(selected.transform.GetSiblingIndex() - 1);

                            siblingCard.GetComponent<CardFace>().selectable = true;

                            CardFace[] allChildrenCard = siblingCard.GetComponentsInChildren<CardFace>();
                            foreach (CardFace cardFace in allChildrenCard)
                            {
                                cardFace.selectable = true;
                            }
                        }

                        selected.transform.position = solitaire.freeCellPos[freeCellIndx].position - new Vector3(0, 0, 0.1f);
                        selected.transform.SetParent(solitaire.freeCellPos[freeCellIndx]);
                    }
                }

                selected.layer = 0;
                selected = null;
            }



        }

        else if(selected!=null)
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
            foreach(Transform child in card.transform)
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
    private void ResetCard(GameObject card)
    {
        CardColorWhite(card);
        card.transform.position = originPosition;
        card.layer = 0;
        //selected = null;
    }
}
