using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; 

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {  get; private set; }
    public TMP_Dropdown dropdown;
    public GameObject fade; 

    public HexTile settlement;
    Vector2Int hexSize = new Vector2Int(10,10);
    private void Awake()
    {
        Instance = this;
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        fade.SetActive(false);
        SceneManager.activeSceneChanged += Initialize;
    }

    public void GetDropdown()
    {
        int r = dropdown.value;
        if(r == 0)
        {
            hexSize = new Vector2Int(10,10);
        }
        if (r == 1)
        {
            hexSize = new Vector2Int(20, 20);
        }
        if (r == 2)
        {
            hexSize = new Vector2Int(50, 50);
        }
        Debug.Log(r);
    }

    public void StartBuilding()
    {
        fade.SetActive(true);
        SceneManager.LoadScene("InGame",LoadSceneMode.Single);
    }

    private void Initialize(Scene current, Scene next)
    {
        if (next.isLoaded && next.name == "InGame")
        {
            Invoke(nameof(Affect), 0.5f);
        }
    }

    public void Affect()
    {
        HexGrid grid = TileManager.instance.InitializeGrid();
        grid.gridSize = hexSize;
        grid.LayoutGrid();
    }
}
