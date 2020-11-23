using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCellCommand : ICommand
{
    int freeCellIndx, freeCellIndx2 = -1;
    GameObject siblingCard, selectedCard;
    Transform oldParent;
    Vector3 oldPosition;

    Solitaire solitaire;
   
    public FreeCellCommand(int freeCellIndx, GameObject selectedCard,Vector3 originPosition)
    {
        this.freeCellIndx = freeCellIndx;
        this.selectedCard = selectedCard;
        this.solitaire = Solitaire.Instance;
        this.oldPosition = originPosition;;
        this.oldParent = selectedCard.transform.parent;
        
    }

    public void Execute()
    {
        freeCellIndx2 = solitaire.FreeCell(selectedCard.name);
        if (freeCellIndx2 != -1)
            solitaire.freeCells[freeCellIndx2] = "";

        solitaire.freeCells[freeCellIndx] = selectedCard.name;


        // change the new last child of the cascade column and its decesendants to be selectable
        siblingCard = UserInput.GetSiblingCard(selectedCard.transform)?.gameObject;

        //move to free cell
        selectedCard.transform.position = solitaire.freeCellPos[freeCellIndx].position - new Vector3(0, 0, 0.1f);
        selectedCard.transform.SetParent(solitaire.freeCellPos[freeCellIndx]);

    }

    // Reverse the executed action
    public void UndoAction()
    {
        if (freeCellIndx2 != -1)
            solitaire.freeCells[freeCellIndx2] = selectedCard.name;

        solitaire.freeCells[freeCellIndx] = "";
        if (siblingCard != null)
        {
            CardFace[] allChildrenCard = siblingCard.GetComponentsInChildren<CardFace>();
            foreach (CardFace cardFace in allChildrenCard)
            {
                cardFace.selectable = false;
            }
        }
        
        selectedCard.transform.SetParent(oldParent);
        selectedCard.transform.position = oldPosition;
    }
    public override string ToString()
    {
        return "Move Card " + selectedCard.name + " to free cell";
    }
}
