using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The GameManager script, which controls several aspects of the game unaffected by loading scenes etc.
/// </summary>
public class GameManager : MonoBehaviour {

    #region Properties

    public static GameManager Instance { get; private set; }

    public bool IsControllerInput
    {
        get
        {
            return isControllerInput;
        }
        set
        {
            if (value)
            {
                if (controllerCount > 0)
                {
                    isControllerInput = value;
                }
            }
            else
            {
                isControllerInput = value;
            }
        }
    }

    public Vector3 RespawnPoint
    {
        get
        {
            return respawnPoint;
        }
        set
        {
            respawnPoint = value;
        }
    }

    public bool IsCursorVisible
    {
        get
        {
            return isCursorVisible;
        }
    }

    public bool IsDay
    {
        get
        {
            return isDay;
        }
    }

    #endregion

    #region Fields

    public event System.Action<int, int> OnInGameTimeChanged;

    [Header("Cannon Balls"), SerializeField] int cannonBallCount = 100; // How many cannonball projectiles will be instantiated at the beginning of the game for object pooling
    [SerializeField] public GameObject cannonBallParent; // Where in the hierarchy every cannonball gets stored to prevent the hierarchy from getting flodded
    [SerializeField] GameObject cannonBallPrefab;
    public Stack<GameObject> freeCannonBalls = new Stack<GameObject>(); // The cannonball stack of free to use cannonballs

    Vector3 respawnPoint = new Vector3(22f, 0f, -60); // TODO remove this obsolete field, because player position is read out of the save file now, when respawning

    private int controllerCount = 0; // How many controllers are connected, to check if the player can possibly play in controller mode

    CursorLockMode lockedToWindow; // The cursorLockMode which lets the mouse only move inside of the window

    [SerializeField] bool isControllerInput = false; // Stores if the player actually plays with controller

    bool isCursorVisible = true;

    public List<GameObject> enemiesNearbyPlayer;

    bool isDay;

    System.DateTime inGameTime;

    [SerializeField] GameObject sun;
    [Range(5, 9), SerializeField] int dayStartAt = 8;
    float dayNightCycleOffset = -120; // This number defines when the sunrise is. At -120, sunrise is at 8 am

    [Range(0, 23), SerializeField] int gameStartsAtDayTime;

    public event System.Action<int> OnEnemiesNearbyPlayerChanged;

    int oldEnemiesNearbyPlayerCount;

    #endregion

    #region Unity Messages

    private void Awake()
    {
        // Load the game if there is a savestate
        SaveFile save = SaveFileManager.LoadGame();
        GameObject.FindGameObjectWithTag("Player").transform.position = save.playerPos;
        // Lock the mouse to the game window
        lockedToWindow = CursorLockMode.Confined;
        Cursor.lockState = lockedToWindow;
        // Make this a singleton class
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            // If there is no GameManager besides this one, make this the game Manager
            Instance = this;
            DontDestroyOnLoad(gameObject);
            // Instantiate a defined number of balls into the free cannon balls stack (Preparation for object pooling)
            for (int i = 0; i < cannonBallCount; i++)
            {
                GameObject ballToPush = Instantiate(cannonBallPrefab, Vector3.zero, transform.rotation);
                ballToPush.SetActive(false);
                ballToPush.transform.SetParent(cannonBallParent.transform);
                freeCannonBalls.Push(ballToPush);
            }
        }
        oldEnemiesNearbyPlayerCount = enemiesNearbyPlayer.Count;
        SetUpSun(gameStartsAtDayTime);
    }

    private void Start()
    {
        InvokeRepeating("TimePasses", 0f, 1f);
    }

    private void Update()
    {
        GetControllerCount();
        if(isControllerInput && isCursorVisible)
        {
            isCursorVisible = false;
        }
        if(!isControllerInput && !isCursorVisible)
        {
            isCursorVisible = true;
        }
        if(oldEnemiesNearbyPlayerCount != enemiesNearbyPlayer.Count)
        {
            OnEnemiesNearbyPlayerChanged(enemiesNearbyPlayer.Count);
            oldEnemiesNearbyPlayerCount = enemiesNearbyPlayer.Count;
        }
        if (sun)
        {
            Quaternion newRot = Quaternion.Euler(new Vector3(GetSunRotationForTime(inGameTime.Hour, inGameTime.Minute), sun.transform.rotation.y, sun.transform.rotation.z));
            sun.transform.rotation = Quaternion.Lerp(sun.transform.rotation, newRot, 0.1f);
        }
    }

    #endregion

    #region Helper Methods

    void SetUpSun(int initialHoursToAdd)
    {
        // Calculate the offset based on the fact, that every hour, the cyclus goes on 15 degrees
        dayNightCycleOffset = dayStartAt * -15;

        // Add hours to define the beginning day time of the game
        inGameTime = inGameTime.AddHours(initialHoursToAdd);

        // Set the suns rotation to support the daytime
        sun.transform.rotation = Quaternion.Euler(new Vector3(GetSunRotationForTime(inGameTime.Hour, inGameTime.Minute), sun.transform.rotation.y, sun.transform.rotation.z));
    }

    void TimePasses()
    {
        inGameTime = inGameTime.AddMinutes(1f);
        // Change the time of day
        //if(sun)
        //{
        //    sun.transform.rotation = Quaternion.Lerp(sun.transform.rotation, Quaternion.Euler(new Vector3(GetSunRotationForTime(inGameTime.Hour, inGameTime.Minute), sun.transform.rotation.y, sun.transform.rotation.z)), 1f);
        //}
        if(OnInGameTimeChanged != null)
        {
            OnInGameTimeChanged(inGameTime.Hour, inGameTime.Minute);
        }
        if(inGameTime.Hour < dayStartAt + 12 && inGameTime.Hour >= dayStartAt)
        {
            isDay = true;
        }
        else
        {
            isDay = false;
        }
    }

    float GetSunRotationForTime(int hour, int minute)
    {
        return ((hour * 60 + minute) * 0.25f) + dayNightCycleOffset;
    }

    // Looks for any connected controller and updates the counter for every connected controller
    void GetControllerCount()
    {
        string[] names = Input.GetJoystickNames();
        controllerCount = 0;
        for (int i = 0; i < names.Length; i++)
        {
            if (!string.IsNullOrEmpty(names[i]))
            {
                controllerCount++;
            }
        }
    }

    // Return a free to use cannonball to shoot 
    public GameObject GetCannonBall(Vector3 positionToSpawn, Vector3 direction)
    {
        GameObject ballToReturn = freeCannonBalls.Pop();
        ballToReturn.transform.position = positionToSpawn;
        ballToReturn.transform.forward = direction;
        ballToReturn.SetActive(true);
        return ballToReturn;
    }

    #endregion

}
