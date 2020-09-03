using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : ISelectable
{
    public GameObject gameObject;
    public MotionController motionController;
    public SelectableController selectableController;
    public CharacterAnimationController animationController;

    public Character(GameObject go) {

        gameObject = go;

        motionController = gameObject.AddComponent<MotionController>();
        motionController.entity = this;

        SetUpSelectableController();
    }

    public void SetUpSelectableController() {

        selectableController = gameObject.AddComponent<SelectableController>();
        selectableController.entity = this;
        foreach(Transform child in gameObject.transform) {

            if (child.name == "SelectionRim") {
                selectableController.selectionRim = child.gameObject;
                return;
            }
        }

        if (selectableController.selectionRim == null)
            Debug.LogError("No SelectionRim was found on this gameObject:", gameObject);

        selectableController.Deselect();
    }

    protected abstract void SetAnimationController();
}
