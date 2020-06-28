using System.Collections;
using System.Collections.Generic;
using UiCore;
using UnityEngine;

public class PacketCommand : Controller {

    public override void Execute(object data1, object data2)
    {
         Debug.Log("PacketCommand触发");
    }

}
