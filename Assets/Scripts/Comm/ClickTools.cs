using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickTools : MonoBehaviour {

    public static Button Get(GameObject go)
    {
        Button btn = null;
        if (go.GetComponent<Button>() == null)
        {
            btn = go.AddComponent<Button>();
            btn.transition = Selectable.Transition.None;
        }
        else
        {
            btn = go.GetComponent<Button>();
        }

        return btn;
    }
}
