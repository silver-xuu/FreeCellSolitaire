using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackCommand : ICommand
{
    Solitaire solitaire;
    GameObject selectedCard, siblingCard, target;
    Vector3 oldPosition;
    Transform oldParent;

    int freeCellIndx=-1;

    public StackCommand(GameObject selectedCard,GameObject target,Vector3 originPosition)
    {
        this.selectedCard = selectedCard;
        this.target = target;
        this.oldPosition = originPosition;
        this.oldParent = selectedCard.transform.parent;
        this.solitaire = Solitaire.Instance;
        
    }
    public void Execute()
    {
        siblingCard = UserInput.GetSiblingCard(selectedCard.transform)?.gameObject;
        MoveCard(selectedCard.transform, target.transform, new Vector3(0f, 0.43f, 0.01f));
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

        freeCellIndx = solitaire.FreeCell(selectedCard.name);
        if (freeCellIndx != -1)
            solitaire.freeCells[freeCellIndx] = "";

        selectedCard.transform.position = target.position - positionVariable;
        selectedCard.transform.SetParent(target);
    }
    public override string ToString()
    {
        return "Stack card " + selectedCard.name +" under " + target.name;
    }
}
