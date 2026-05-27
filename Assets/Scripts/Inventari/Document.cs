using UnityEngine;

[CreateAssetMenu(fileName = "NouDocument", menuName = "Documents/Document")]
public class DocumentData : ScriptableObject
{
    public string id;
    public string titol;

    [TextArea(10, 30)]
    public string contingut;
}