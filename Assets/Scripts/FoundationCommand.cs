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
    static float buffer=0.01f;

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
        if (newFoundationFace.Split(' ')[1] == "K")
            Solitaire.finishCount++;
        Debug.Log(Solitaire.finishCount);
        selectedCard.transform.position=solitaire.foundationPos[foundationIndx].position - new Vector3(0, 0, buffer);
        selectedCard.transform.SetParent(solitaire.foundationPos[foundationIndx]);
        buffer += 0.01f;
        
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
