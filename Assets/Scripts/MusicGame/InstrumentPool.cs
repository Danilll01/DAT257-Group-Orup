using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstrumentPool : MonoBehaviour
{
    [SerializeField] private GameObject prefabObject;

    [SerializeField] private GameObject snapPointsParent;

    [SerializeField] private GameObject extraLinesParent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.childCount == 1)
        {
            GameObject obj = Instantiate(prefabObject, transform.position, Quaternion.identity, transform);
            obj.GetComponent<DragAndDropNote>().SetSnapPointParent(snapPointsParent);
            obj.GetComponent<DragAndDropNote>().SetExtraLineParent(extraLinesParent);
        }
    }
}
