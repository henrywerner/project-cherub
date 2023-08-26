using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldContainer : MonoBehaviour
{
    private NoteBase[] _childNotes;

    // unity said no constructors in a MonoBehaviour, so I made my own lmao
    public void Constructor(Note rootNote, NoteChild[] children) 
    {

        int currentHighway = rootNote.h;

        // Generate the first note?


        for (int i = 0; i < children.Length; i++)
        {
            NoteChild child = children[i];

            if (child.h != currentHighway)
            {
                // lane switch

                // add flick note in current lane

                // add continue hold note in new lane
            }

            if (i == children.Length - 1)
            {
                // add HoldEnd to child array
            }
        }
    }
}
