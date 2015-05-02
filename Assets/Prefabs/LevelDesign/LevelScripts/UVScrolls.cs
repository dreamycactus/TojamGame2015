﻿using UnityEngine;
using System.Collections;

public class UVScrolls : MonoBehaviour {

    public float parralax = 2f;

    void Update()
    {

        MeshRenderer mr = GetComponent<MeshRenderer>();

        Material mat = mr.material;

        Vector2 offset = mat.mainTextureOffset;

        offset.x = transform.position.x / transform.localScale.x / parralax;

        offset.y = transform.position.y / transform.localScale.y / (10*parralax)+0.91f ;

        mat.mainTextureOffset = offset;

    }
}
