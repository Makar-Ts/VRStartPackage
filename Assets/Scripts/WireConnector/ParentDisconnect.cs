using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentDisconnect : MonoBehaviour
{
    public void Disconnect() {
        transform.parent = null;
    }
}
