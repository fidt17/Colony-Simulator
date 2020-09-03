using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableController : MonoBehaviour
{
    public ISelectable entity;

    public GameObject selectionRim;

    public bool isSelected { get; private set; }

    public void Select() {

        isSelected = true;
        selectionRim.SetActive(true);
    }

    public void Deselect() {

        isSelected = false;
        selectionRim.SetActive(false);
    }
}
