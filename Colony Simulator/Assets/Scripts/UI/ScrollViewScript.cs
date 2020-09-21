using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollViewScript : MonoBehaviour
{
    public GameObject elementPrefab;
    public RectTransform content;
    public float heightOffset;

    public List<GameObject> elements = new List<GameObject>();

    public GameObject AddElement() {

        GameObject newElement = Instantiate(elementPrefab, content.transform);

        float offset = heightOffset * elements.Count;

        content.sizeDelta = new Vector2(content.sizeDelta.x, -offset - heightOffset);

        newElement.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, offset);
        elements.Add(newElement);
        
        return newElement;
    }

    public void ClearViewport() {

        foreach(GameObject element in elements)
            Destroy(element);

        content.sizeDelta = new Vector2(content.sizeDelta.x, 0);
        elements = new List<GameObject>();
    }
}
