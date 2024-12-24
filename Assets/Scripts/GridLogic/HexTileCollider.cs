using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshCollider))]
public class HexTileCollider : MonoBehaviour, ITargetSelectable
{
    private HexTile hextile;
    Color _color;
    MeshRenderer meshRenderer;
    private void Start()
    {
        hextile = GetComponentInParent<HexTile>();
        meshRenderer = GetComponent<MeshRenderer>();
        _color = meshRenderer.material.color;
    }


    public void OnHighlightTarget()
    {
        Color color = Color.gray;
        meshRenderer.material.color = color;
    }

    public void OnSelectTarget()
    {
        TileManager.instance.OnSelectedTile(hextile);
    }

    private void OnMouseExit()
    {
        meshRenderer.material.color = _color;
    }
}
