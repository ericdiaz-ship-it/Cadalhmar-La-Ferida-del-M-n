using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DatabaseDocuments", menuName = "Documents/Database")]
public class DocumentsDatabase : ScriptableObject
{
    public List<DocumentData> documents;

    private Dictionary<string, DocumentData> lookup;

    public void Init()
    {
        lookup = new Dictionary<string, DocumentData>();

        foreach (var doc in documents)
        {
            lookup[doc.id] = doc;
        }
    }

    public DocumentData GetDocument(string id)
    {
        if (lookup == null)
            Init();

        return lookup.TryGetValue(id, out var doc) ? doc : null;
    }
}