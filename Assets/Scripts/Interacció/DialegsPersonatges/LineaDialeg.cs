using UnityEngine;
[System.Serializable]
//system.serializable permet que aquesta classe es pugui mostrar a l'inspector de Unity, el que facilita la creació de diàlegs a través de l'editor
public struct LiniaDialeg
{
    public string nomPersonatge;
    [TextArea(3,10)]
    public string textDialeg;
    public Sprite imatgePersonatge;
}