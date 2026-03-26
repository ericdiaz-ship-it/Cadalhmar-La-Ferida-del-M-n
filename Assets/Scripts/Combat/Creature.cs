using UnityEngine;
public class Creature : MonoBehaviour
{
    public Vector2Int localPosition;
    public GameObject selectionIndicator;
    void Start()
    {
        this.SetSelectionStatus(false);
    }
    public void SetSelectionStatus(bool selected)
    {
        this.selectionIndicator.SetActive(selected);
    }
}