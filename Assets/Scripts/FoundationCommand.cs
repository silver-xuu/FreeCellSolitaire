using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoundationCommand : ICommand
{
    Solitaire solitaire;
    GameObject selectedCard, siblingCard, target;
    Vector3 oldPosition;
    Transform oldParent;
    string oldFoundationFace,newFoundationFace;

    int freeCellIndx = -1,foundationIndx;
    public FoundationCommand(GameObject selected, Vector3 oldPosition,int foundationIndx)
    {
        this.solitaire = Solitaire.Instance;
        this.selectedCard = selected;
        this.oldPosition = oldPosition;
        this.foundationIndx = foundationIndx;
        this.oldParent = selectedCard.transform.parent;
        this.oldFoundationFace = selected.name;
        this.newFoundationFace = solitaire.NextCardFace(oldFoundationFace);
    }


    public void Execute()
    {
        siblingCard = UserInput.GetSiblingCard(selectedCard.transform)?.gameObject;
        selectedCard.tag = "Foundation";

        solitaire.foundations[foundationIndx] = newFoundationFace;
        selectedCard.transform.position=solitaire.foundationPos[foundationIndx].position - new Vector3(0, 0, 0.01f);
        selectedCard.transform.SetParent(solitaire.foundationPos[foundationIndx]);
    }

    public void UndoAction()
    {
        if (freeCellIndx != -1)
            solitaire.freeCells[freeCellIndx] = selectedCard.name;

        if (siblingCard != null)
        {
            CardFace[] allChildrenCard = siblingCard.GetComponentsInChildren<CardFace>();
            foreach (CardFace cardFace in allChildrenCard)
            {
                cardFace.selectable = false;
            }
        }

        selectedCard.transform.position = oldPosition;
        selectedCard.transform.SetParent(oldParent);
        selectedCard.tag = "Card";
        solitaire.foundations[foundationIndx] = oldFoundationFace;
    }
    public override string ToString()
    {
        return "Move Card " + selectedCard.name + " to foundation";
    }
}
