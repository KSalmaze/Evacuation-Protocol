using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextInteraction : Interaction
{
    [TextArea][SerializeField] private string text;

    public override void Interact()
    {
        gameManager.ShowText(text);
    }

}
