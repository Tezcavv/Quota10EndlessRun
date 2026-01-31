using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EndlessSegment : MonoBehaviour
{
  [Header("Init")]
  [SerializeField] private List<SegmentGraphics> graphics;
  [SerializeField] private GraphicType type;
  private void Update()
  {
    if (Keyboard.current.kKey.wasPressedThisFrame)
    {
      ActivateObjects(type);
    }
  }

  private void ActivateObjects(GraphicType graphicType)
  {
    foreach (var objectSegm in graphics)
    {
      objectSegm.gameObject.SetActive(objectSegm.graphicType == graphicType);
    }
    
  }
}


[Serializable]
public class SegmentGraphics
{
  public GraphicType graphicType;
  public GameObject gameObject;
}

public enum GraphicType
{
  COL_CIRCLE_THIN,
  COL_CIRCLE_3WAY,
  COL_SQUARE,
  COL_MEDIEVAL
}