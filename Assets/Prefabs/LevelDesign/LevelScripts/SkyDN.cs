using UnityEngine;
using System.Collections;

public class SkyDN : MonoBehaviour {

    void Update()
    {

        MeshRenderer mr = GetComponent<MeshRenderer>();

        Material mat = mr.material;

        Vector2 offset = mat.mainTextureOffset;

        offset.y -= Time.deltaTime / 20f;
        if (offset.y >0.05)
            mat.mainTextureOffset = offset;

    }
}
