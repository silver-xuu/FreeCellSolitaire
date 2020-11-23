using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyCascadeCommand : ICommand
{
    Solitaire solitaire;
    GameObject selectedCard, siblingCard, emptyCascade;
    int freeCellIndx = -1;
    Vector3 oldPosition;
    Transform oldParent;

    public EmptyCascadeCommand(GameObject selectedCard, GameObject emptyCascade, Vector3 originPosition)
    {
        this.selectedCard = selectedCard;
        this.emptyCascade = emptyCascade;
        this.solitaire = Solitaire.Instance;
        //record oldposition and parent
        this.oldPosition = originPosition;
        this.oldParent = selectedCard.transform.parent;
    }
    public void Execute()
    {
        //remove from the free cell if the card is in it
        //int freeCellIndx = solitaire.FreeCell(selectedCard.name);
        //if (freeCellIndx != -1)
        //    solitaire.freeCells[freeCellIndx] = "";

        
        siblingCard = UserInput.GetSiblingCard(selectedCard.transform)?.gameObject;
        MoveCard(selectedCard.transform, emptyCascade.transform, new Vector3(0, 0, 0.03f));

        
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

    }
    public void MoveCard(Transform selected, Transform target, Vector3 positionVariable)
    {
        //remove from the free cell if the card is in it
         freeCellIndx = solitaire.FreeCell(selected.name);
        if (freeCellIndx != -1)
            solitaire.freeCells[freeCellIndx] = "";

        selectedCard.transform.position = target.position - positionVariable;
        selectedCard.transform.SetParent(target);
    }
    public override string ToString()
    {
        return "Move Card " + selectedCard.name + " to empty cascade";
    }
}
