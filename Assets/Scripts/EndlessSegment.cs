using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EndlessSegment : MonoBehaviour
{
    public GameObject Start;
    public GameObject Finish;
  [Header("Init")]
  [SerializeField] private List<SideObject> objects;
  // [SerializeField] private GraphicType type;
  // private void Update()
  // {
  //   if (Keyboard.current.kKey.wasPressedThisFrame)
  //   {
  //     ActivateObjects(type);
  //   }
  //   
  // }

  private void ActivateObjects(GraphicType graphicType)
  {
    foreach (var obj in objects)
    {
      foreach (var graph in obj.graphics)
      {
        graph.gO.SetActive(graph.graphicType == graphicType);
      }
    }
    
    
  }
}

