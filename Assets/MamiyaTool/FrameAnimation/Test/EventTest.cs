using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTest : MonoBehaviour
{
    public void OnEvent(string key) {
        Debug.Log($"############# {key} #############");
    }
}
