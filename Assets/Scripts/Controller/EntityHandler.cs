using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityHandler : MonoBehaviour
{
    public static EntityHandler Instance { get; private set; }
    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public delegate void EntityOnFixedUpdate();
    static public event EntityOnFixedUpdate FixedUpdateEvent;

    private PlayerController playerController;
    [SerializeField] PlayerUpdate playerUpdate;
    public PlayerValues playerValues;
    public int coinValue = 15;

    private List<GardenController> gardenControllers = new List<GardenController>();
    [SerializeField] private List<GardenUpdate> gardenUpdates;
    [Header("GardenSettings")]
    [SerializeField]
    private int width;
    [SerializeField]
    private int height;
    [SerializeField]
    private float growTime;
    [SerializeField]
    private bool isCuted;
    [SerializeField]
    private float meshStep;

    // Start is called before the first frame update
    void Start()
    {        
        playerController = new PlayerController(new PlayerModel(playerValues.position, playerValues.normalizedSpeed, 40), //change settings 
                                                playerUpdate,
                                                gameObject.GetComponent<InputController>());
        foreach(GardenUpdate update in gardenUpdates)
        {
            gardenControllers.Add(new GardenController(new GardenModel(new Vector3(), width, height, growTime, isCuted, meshStep), update));
        }
    }

    private void FixedUpdate()
    {
        FixedUpdateEvent();
    }


}
