using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MemoryTool  {

    [MenuItem("MemoryTool/ClearMemory")]
    public static void ClearMenory()
    {
        GameTool.DeleteAll();
        Debug.Log("清空所有PlayerPrefabs。");
    }
}
