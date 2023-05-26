using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    [SerializeField]
    private ShipSetting shipSettings;


    private void Start()
    {
        
    }
    public void Init(List<ShipSetting.ShipType> ships)
    {

    }



    public void DeployPlayerShip(ShipSetting.ShipType shipType)
    {
        HexTile hexTile = TileManager.instance.ReturnLastHexTile();
        if (hexTile != null && hexTile.isEmpty)
        {
            Instantiate(shipSettings.GetShip(shipType).gameObject, hexTile.transform.position + new Vector3(0,.3f,0), Quaternion.Euler(new Vector3(-90,0,0)));
            hexTile.isEmpty = false;
        }

    }
}
