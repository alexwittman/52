using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DestroyPoint : MonoBehaviour {

    //Sets the starting position off the screen.
	void Start () {
        transform.position = new Vector2( 1.5f * Screen.width, transform.position.y);
	}

}
