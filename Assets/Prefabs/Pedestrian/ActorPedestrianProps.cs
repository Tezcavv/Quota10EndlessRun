using UnityEngine;

public class AttoreOggetti : MonoBehaviour
{
    public GameObject handProp;
    Transform handPropT;
    public GameObject headProp;
    Transform headPropT;
    public GameObject bodyProp;
    Transform bodyPropT;
    public GameObject backpackProp;
    Transform backpackPropT;

    public Transform handSocketT;
    public Transform headSocketT;
    public Transform bodySocketT;
    public Transform backpackSocketT;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (handProp)
        {
            handPropT = handProp.GetComponent<Transform>();
        }
        if (headProp)
        {
            headPropT = headProp.GetComponent<Transform>();
        }
        if (bodyProp)
        {
            bodyPropT = bodyProp.GetComponent<Transform>();
        }
        if (backpackProp)
        {
            backpackPropT = backpackProp.GetComponent<Transform>();
        }
    }

    // Update is called once per frame
    //void Update()
    //{
    //    if (headSocketT) {
    //        headPropT.position = headSocketT.position;
    //        headPropT.rotation = headSocketT.rotation;
    //    }

    //    if (handSocketT) {
    //        handPropT.position = handSocketT.position;
    //        handPropT.rotation = handSocketT.rotation;
    //    }

    //    if (bodyPropT) {
    //        bodyPropT.position = bodySocketT.position;
    //        bodyPropT.rotation = bodySocketT.rotation;
    //    }

    //    if (backpackPropT) {
    //        backpackPropT.position = backpackSocketT.position;
    //        backpackPropT.rotation = backpackSocketT.rotation;
    //    }
    //}
}
