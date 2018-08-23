using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paint : MonoBehaviour
{
    public Material[] paints;
    //public Shader testShader;
    public Texture redX;

    private Material selectedMaterial;

    // Use this for initialization
    void Start()
	{
        selectedMaterial = paints[0];
	}
	
	// Update is called once per frame
	void Update()
	{
        if (Input.GetMouseButton(0))
        {
            RaycastHit hitInfo = new RaycastHit();

            //Ray for mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Physics.Raycast(ray, out hitInfo, 4.0f);

            if (hitInfo.collider != null)
            {
                if (hitInfo.collider.tag == "Wall")
                {
                    Renderer hitRenderer = hitInfo.transform.gameObject.GetComponent<Renderer>();
                    //hitRenderer.material = selectedMaterial;
                    hitRenderer.material.EnableKeyword("_DETAIL_MULX2");
                    hitRenderer.material.SetTexture("_DetailAlbedoMap", redX);
                    Transform paintLayer = hitInfo.collider.transform.parent.GetChild(1);
                    paintLayer.gameObject.SetActive(true);
                }
            }
        }
	}
}