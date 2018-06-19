using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : BaseTank {

    #region Properties

    private int MaxHealth
    {
        get
        {
            return maxHealth;
        }
        set
        {
            maxHealth = value;
            if (OnMaxHealthChanged != null)
            {
                OnMaxHealthChanged(MaxHealth);
            }

        }
    }

    private int Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
            if (OnHealthChanged != null)
            {
                OnHealthChanged(Health);
            }
        }
    }

    private int MaxShield
    {
        get
        {
            return maxShield;
        }
        set
        {
            maxShield = value;
            if (OnMaxShieldChanged != null)
            {
                OnMaxShieldChanged(MaxShield);
            }

        }
    }

    private int Shield
    {
        get
        {
            return shield;
        }
        set
        {
            shield = value;
            if (OnShieldChanged != null)
            {
                OnShieldChanged(Shield);
            }
        }
    }

    // Lets the enemies react to a dead player, instead of keeping attacking
    public bool IsDead
    {
        get
        {
            return isDead;
        }
    }

    // Lets the camera reset its own rotation
    public Vector3 CockPitForward
    {
        get
        {
            return cockPit.transform.forward;
        }
    }

    // The attributes, so they can be viewed in the status menu.
    public float CockPitRotationSpeed
    {
        get
        {
            return cockPitRotationSpeed;
        }
        set
        {
            cockPitRotationSpeed = value;
        }
    }

    public int Level
    {
        get
        {
            return level;
        }
    }

    public int Attack
    {
        get
        {
            return attack;
        }
    }

    public float FireRate
    {
        get
        {
            return fireRate;
        }
    }

    public float ReloadSpeed
    {
        get
        {
            return reloadSpeed;
        }
    }

    public float KnockBack
    {
        get
        {
            return shootKnockback * shootKnockbackDuration;
        }
    }

    public int Defense
    {
        get
        {
            return defense;
        }
    }

    public float TopSpeed
    {
        get
        {
            return topSpeed;
        }
    }

    public float Acceleration
    {
        get
        {
            return acceleration;
        }
    }

    public float RotationSpeed
    {
        get
        {
            return rotationSpeed;
        }
    }

    public float Mass
    {
        get
        {
            return mass;
        }
    }

    public ScriptableAttackCockPitUpgrade AttackUpgrade
    {
        get
        {
            return equippedAttackUpgrade;
        }
    }

    public ScriptableBodyUpgrade BodyUpgrade
    {
        get
        {
            return equippedBodyUpgrade;
        }
    }

    public ScriptableTracksUpgrade TracksUpgrade
    {
        get
        {
            return equippedTracksUpgrade;
        }
    }

    public BasePlayerItem EquippedItem1
    {
        get
        {
            return equippedItems[0];
        }
    }

    public BasePlayerItem EquippedItem2
    {
        get
        {
            return equippedItems[1];
        }
    }

    public BasePlayerItem EquippedItem3
    {
        get
        {
            return equippedItems[2];
        }
    }

    private int ShotsInMagazine
    {
        get
        {
            return shotsInMagazine;
        }
        set
        {
            shotsInMagazine = value;
            if(OnShotsInMagazineChanged != null)
            {
                OnShotsInMagazineChanged(shotsInMagazine);
            }
        }
    }

    private int MagazineSize
    {
        get
        {
            return magazineSize;
        }
        set
        {
            magazineSize = value;
            if (OnMagazineSizeChanged != null)
            {
                OnMagazineSizeChanged(magazineSize);
            }
        }
    }

    #endregion

    #region Private Fields

    public event System.Action<int> OnShotsInMagazineChanged;
    public event System.Action<int> OnMagazineSizeChanged;

    public event System.Action<int, Sprite> OnEquippedItemChanged;
    public event System.Action<int, int> OnEquippedItemUsageCountChanged;

    public event System.Action<int> OnMaxHealthChanged;
    public event System.Action<int> OnHealthChanged;

    public event System.Action<int> OnMaxShieldChanged;
    public event System.Action<int> OnShieldChanged;

    PlayerInput input;
    GameObject cameraArm;
    Camera camera;

    // Stores an enemy if the player aims at one
    GameObject enemyInFront;
    // Stores the gameobject of an npc if the player aims at one
    GameObject npcToTalkTo;

    bool bIsGrounded = false;
    
    [SerializeField] float rayToGroundLength = 1f; 
    [SerializeField] float talkDistance = 5f; // How far the player can be away from an NPC and still talk to him instead of shoot at him
    
    [Header("Sounds"), SerializeField] AudioSource engineSound;
    [Range(0f, 0.1f), SerializeField] float engineSoundGain = 0.01f; // How much the engine sound volume gains when the player starts driving
    [Range(0f, 1f), SerializeField] float idleEngineVolume = 0.01f; // The volume of the engine sound, when the player is not driving
    [SerializeField] AudioSource flashLightSource;

    [Header("Camera Shake"), SerializeField] float shootCameraShakeAmount = 1f;
    [SerializeField] float shootCameraShakeDuration = 1f;

    LayerMask terrainLayer; // LayerMask with the ground included, to raycast for isGrounded

    // The equipped upgrades, whose values get added to the base attribute values
    ScriptableAttackCockPitUpgrade equippedAttackUpgrade;
    ScriptableBodyUpgrade equippedBodyUpgrade;
    ScriptableTracksUpgrade equippedTracksUpgrade;

    BasePlayerItem[] equippedItems = new BasePlayerItem[3];

    [Header("Meshes For Upgrades"), SerializeField] MeshFilter[] cockpitMeshes; // 0 = cockpit | 1 = barrel
    [SerializeField] MeshFilter bodyMesh;
    [SerializeField] MeshFilter[] tracksMeshes; // 0, 1 = tracks | 2 - 5 = wheels

    #endregion

    #region Unity Messages

    // Get necessary components, create ground layer and initialise attributes
    protected override void Awake()
    {
        base.Awake();
        input = GetComponent<PlayerInput>();
        cameraArm = Camera.main.transform.parent.gameObject;

        camera = Camera.main;

        // Create the layer Mask for the terrain
        int layer = LayerMask.NameToLayer("Terrain");
        terrainLayer = 1 << layer;

        // Equip the base Upgrades for the tank
        ChangeEquippedUpgrade(UpgradeManager.Instance.GetUpgrade<ScriptableAttackCockPitUpgrade>("BaseCockpit"));
        ChangeEquippedUpgrade(UpgradeManager.Instance.GetUpgrade<ScriptableBodyUpgrade>("BaseBody"));
        ChangeEquippedUpgrade(UpgradeManager.Instance.GetUpgrade<ScriptableTracksUpgrade>("BaseTracks"));

        //Equip the landmine
        EquippItem(0, ItemManager.Instance.GetItem<ItemLandmine>());
    }

    private void Start()
    {
        InitialDelegateCalls();
    }

    // Let the player move and shoot, whilst playing the right animations and particle effects, reacting to the environment at all times
    protected override void FixedUpdate()
    {
        if(isDead) { return; }
        GetInput();
        RotateTurret();
        RotateBody();
        UpdateIsGrounded();
        CalculateVelocity();
        if(input.UseItem1 && EquippedItem1)
        {
            UseItem(0);
        }
        if (input.UseItem2 && EquippedItem2)
        {
            UseItem(1);
        }
        if (input.UseItem3 && EquippedItem3)
        {
            UseItem(2);
        }
        if (input.ResetCam)
        {
            SetCamera();
        }
        if(input.ToggleFlashlight)
        {
            ToggleFlashlight();
        }
        // TODO this code block, having two times almost the same for no EventSystem and one EventSystem HAS TO BE CHANGED.
        if (EventSystem.current)
        {
            if (input.Shoot)
            {
                if (Time.realtimeSinceStartup > shootTime + fireRate && !EventSystem.current.IsPointerOverGameObject() && npcToTalkTo == null && shotsInMagazine > 0 && !isReloading)
                {
                    Shoot();
                }
                else if (npcToTalkTo != null && !EventSystem.current.IsPointerOverGameObject())
                {
                    Vector3 toNPC = npcToTalkTo.transform.position - transform.position;
                    if (toNPC.magnitude < talkDistance)
                    {
                        npcToTalkTo.GetComponent<NPCTank>().Interact();
                    }
                    else if(shotsInMagazine > 0 && !isReloading)
                    {
                        Shoot();
                    }
                }
            }
            if(input.Reload && magazineSize != shotsInMagazine && !isReloading)
            {
                StartReloading();
            }
        }
        else
        {
            if (input.Shoot && shotsInMagazine > 0)
            {
                if (Time.realtimeSinceStartup > shootTime + fireRate && npcToTalkTo == null && shotsInMagazine > 0 && !isReloading)
                {
                    Shoot();
                }
                else if (npcToTalkTo != null)
                {
                    Vector3 toNPC = npcToTalkTo.transform.position - transform.position;
                    if (toNPC.magnitude < talkDistance)
                    {
                        npcToTalkTo.GetComponent<NPCTank>().Interact();
                    }
                    else if (shotsInMagazine > 0 && !isReloading)
                    {
                        Shoot();
                    }
                }
            }
            if (input.Reload && magazineSize != shotsInMagazine)
            {
                StartReloading();
            }
        }
        if (engineSound)
        {
            PlayEngineSound();
        }
        base.FixedUpdate();
    }

    // Update the enemyInFront and npcToTalkTo Variables with the gameObjects in front of the player
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            enemyInFront = other.gameObject;
        }
        if(other.tag == "NPC")
        {
            npcToTalkTo = other.gameObject;
        }
    }

    // Update the enemyInFront and npcToTalkTo Variables to be empty again
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            enemyInFront = null;
        }
        if(other.tag == "NPC")
        {
            npcToTalkTo = null;
        }
    }

    #endregion

    #region Private Methods

    // When the player levels up, he gains attribute points, which he can use to strengthen specific attributes. 
    //When the player chose to do so, this method takes in a parameter, to define what attribute to enhance, and enhances that particular attribute then.
    public void SpendPointOnAttribute()
    {

    }

    protected override void ToggleFlashlight()
    {
        base.ToggleFlashlight();
        if(flashLightSource)
        {
            flashLightSource.PlayOneShot(flashLightSource.clip);
        }
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        if(OnHealthChanged != null)
        {
            OnHealthChanged(Health);
        }
        if(OnShieldChanged != null)
        {
            OnShieldChanged(Shield);
        }
    }

    void EquippItem(int slot, BasePlayerItem item)
    {
        equippedItems[slot] = item;
        item.ResetItemUsageTimes();
        if(OnEquippedItemChanged != null)
        {
            OnEquippedItemChanged(slot, item.Sprite);
        }
    }

    void UseItem(int slot)
    {
        equippedItems[slot].UseItem();
        if(OnEquippedItemUsageCountChanged != null)
        {
            OnEquippedItemUsageCountChanged(slot, equippedItems[slot].TimesOfUseLeft);
        }
    }

    // Changes the equipped attack upgrade
    void ChangeEquippedUpgrade(ScriptableAttackCockPitUpgrade upgrade)
    {
        equippedAttackUpgrade = upgrade;

        attack = baseAttack + upgrade.attack;
        fireRate = basefireRate + upgrade.fireRate;
        reloadSpeed = baseReloadSpeed + upgrade.reloadSpeed;
        MagazineSize = baseMagazineSize + upgrade.magazineSize;
        shootKnockback = baseShootKnockback + upgrade.shootKnockback;
        shootKnockbackDuration = baseShootKnockbackDuration + upgrade.shootKnockbackDuration;

        mass = baseMass + equippedAttackUpgrade.mass;
        if(equippedBodyUpgrade)
        {
            mass += equippedBodyUpgrade.mass;
        }
        if(equippedTracksUpgrade)
        {
            mass += equippedTracksUpgrade.mass;
        }

        // Change the mesh for the cockpit to give the player visual feedback about what upgrade he has equipped
        if (cockpitMeshes.Length > 1) // Bigger than 1, because there has to be one cockpit and one barrel mesh Filter inside of it
        {
            cockpitMeshes[0].mesh = upgrade.upgradeMesh;
            cockpitMeshes[1].mesh = upgrade.secondaryUpgradeMesh;
        }
    }

    // Changes the equipped body upgrade
    void ChangeEquippedUpgrade(ScriptableBodyUpgrade upgrade)
    {
        equippedBodyUpgrade = upgrade;

        Health = baseHealth + upgrade.health;
        MaxHealth = baseHealth + upgrade.health;
        defense = baseDefense + upgrade.defense;
        Shield = baseShield + upgrade.shield;
        MaxShield = baseShield + upgrade.shield;

        mass = baseMass + equippedBodyUpgrade.mass;
        if (equippedAttackUpgrade)
        {
            mass += equippedAttackUpgrade.mass;
        }
        if (equippedTracksUpgrade)
        {
            mass += equippedTracksUpgrade.mass;
        }

        // Change the mesh for the body to give the player visual feedback about what upgrade he has equipped
        if (bodyMesh)
        {
            bodyMesh.mesh = upgrade.upgradeMesh;
        }
    }

    // Changes the equipped tracks upgrade
    void ChangeEquippedUpgrade(ScriptableTracksUpgrade upgrade)
    {
        equippedTracksUpgrade = upgrade;

        topSpeed = baseTopSpeed + upgrade.topSpeed;
        acceleration = baseAcceleration + upgrade.acceleration;
        rotationSpeed = baseRotationSpeed + upgrade.rotationSpeed;

        mass = baseMass + equippedTracksUpgrade.mass;
        if (equippedBodyUpgrade)
        {
            mass += equippedBodyUpgrade.mass;
        }
        if (equippedAttackUpgrade)
        {
            mass += equippedAttackUpgrade.mass;
        }

        // Change the mesh for the tracks to give the player visual feedback about what upgrade he has equipped
        if (tracksMeshes.Length > 5) // Bigger than five, because there has to be two track mesh Filter and 4 wheel filter inside of it
        {
            tracksMeshes[0].mesh = upgrade.upgradeMesh;
            tracksMeshes[1].mesh = upgrade.secondaryUpgradeMesh;
            tracksMeshes[2].mesh = upgrade.wheelsMesh;
            tracksMeshes[3].mesh = upgrade.wheelsMesh;
            tracksMeshes[4].mesh = upgrade.wheelsMesh;
            tracksMeshes[5].mesh = upgrade.wheelsMesh;
        }
    }

    protected override void StartReloading()
    {
        base.StartReloading();
        OverlayController.Instance.TriggerReloadAnimation(reloadSpeed);
    }

    protected override void WhenReloadFinished()
    {
        base.WhenReloadFinished();
        if(OnShotsInMagazineChanged != null)
        {
            OnShotsInMagazineChanged(shotsInMagazine);
        }
    }

    // Make the tank die and explode and show the Gameover menu after that
    protected override void Die()
    {
        base.Die();
        GameoverMenu.Show();
    }

    // Get the player tank into the original form. Called after the tank respawns
    public void ResetPlayerTank()
    {
        Health = MaxHealth;
        foreach(var menu in MenuManager.Instance.MenuStack)
        {
            Destroy(menu.gameObject);
        }
        MenuManager.Instance.MenuStack.Clear();
        camera.GetComponentInParent<Animator>().SetTrigger("Idle");
        ShotsInMagazine = MagazineSize;
        isDead = false;
    }

    // Reset the camera to the current cockpit rotation or lock it to an enemy if one is in front of the player
    private void SetCamera()
    {
        if (enemyInFront)
        {
            cameraArm.GetComponent<CameraController>().FocussedObject = enemyInFront;
            return;
        }
        cameraArm.GetComponent<CameraController>().FocussedObject = null;
        cameraArm.GetComponent<CameraController>().CamResetRotation = aimDirection;
    }

    // Reads the input and writes the data into the right variables. Also converts to Camera Relative controls. 
    // Wont let the player aim, when hovering over UI.
    void GetInput()
    {
        // Calculate the offset angle on the y axis from the Camera's forward to the global forward
        float yAngle = Vector3.Angle(Vector3.forward, cameraArm.transform.forward);
        // Calculate the real 360 degree angle for when the rotation is anti clockwise
        if (cameraArm.transform.forward.x < 0f)
        {
            yAngle = 360 - yAngle;
        }
        moveDirection.x = input.Horizontal;
        moveDirection.z = input.Vertical;
        // Apply the offset angle on the y axis to the moveDirection vector --> Camera Relative Controls
        moveDirection = Quaternion.AngleAxis(yAngle, Vector3.up) * moveDirection;

        if (GameManager.Instance.IsControllerInput)
        {
            aimDirection.x = input.R_Horizontal;
            aimDirection.z = input.R_Vertical;
            // Apply the offset angle on the y axis to the aimDirection vector --> Camera Relative Controls
            aimDirection = Quaternion.AngleAxis(yAngle, Vector3.up) * aimDirection;
        }
        else
        {
            if (EventSystem.current)
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    Physics.Raycast(ray, out hit, 500f);
                    Vector3 mousePosInWorld = hit.point;
                    Vector3 targetAim = mousePosInWorld - transform.position;
                    aimDirection.x = targetAim.normalized.x;
                    aimDirection.z = targetAim.normalized.z;
                }
            }
            else
            {
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                Physics.Raycast(ray, out hit, 500f);
                Vector3 mousePosInWorld = hit.point;
                Vector3 targetAim = mousePosInWorld - transform.position;
                aimDirection.x = targetAim.normalized.x;
                aimDirection.z = targetAim.normalized.z;
            }
        }
    }

    // Shoot a projectile and shake the camera
    protected override void Shoot()
    {
        base.Shoot();
        OverlayController.Instance.FlashupAnimation();
        camShake.shakeAmount = shootCameraShakeAmount;
        camShake.shakeDuration = shootCameraShakeDuration;
        if (OnShotsInMagazineChanged != null)
        {
            OnShotsInMagazineChanged(shotsInMagazine);
        }
    }

    // Play the sound of the tank engine at the right volume
    private void PlayEngineSound()
    {
        if (velocity.magnitude * engineSoundGain > idleEngineVolume)
        {
            engineSound.volume = velocity.magnitude * engineSoundGain;
        }
        else
        {
            engineSound.volume = idleEngineVolume;
        }
    }

    // Check if the player is on the ground or not, to apply more gravity when he is in the air
    void UpdateIsGrounded()
    {
        if(Physics.Raycast(transform.position, Vector3.down, rayToGroundLength, terrainLayer))
        {
            bIsGrounded = true;
        }
        else
        {
            bIsGrounded = false;
        }
    }

    // Calls all the player delegates one time to ensure, that all the ui is up to date and initialized correctly
    private void InitialDelegateCalls()
    {
        // Update the Ammo display
        if (OnShotsInMagazineChanged != null)
        {
            OnShotsInMagazineChanged(ShotsInMagazine);
        }
        if (OnMagazineSizeChanged != null)
        {
            OnMagazineSizeChanged(MagazineSize);
        }
        // Update the item count display
        if (OnEquippedItemUsageCountChanged != null)
        {
            if (equippedItems[0])
            {
                OnEquippedItemUsageCountChanged(0, equippedItems[0].TimesOfUseLeft);
                if (OnEquippedItemChanged != null)
                {
                    OnEquippedItemChanged(0, equippedItems[0].Sprite);
                }
            }
            else
            {
                OnEquippedItemUsageCountChanged(0, 0);
            }
            if (equippedItems[1])
            {
                OnEquippedItemUsageCountChanged(1, equippedItems[1].TimesOfUseLeft);
                if (OnEquippedItemChanged != null)
                {
                    OnEquippedItemChanged(1, equippedItems[1].Sprite);
                }
            }
            else
            {
                OnEquippedItemUsageCountChanged(1, 0);
            }
            if (equippedItems[2])
            {
                OnEquippedItemUsageCountChanged(2, equippedItems[2].TimesOfUseLeft);
                if (OnEquippedItemChanged != null)
                {
                    OnEquippedItemChanged(2, equippedItems[2].Sprite);
                }
            }
            else
            {
                OnEquippedItemUsageCountChanged(2, 0);
            }
            if (OnHealthChanged != null)
            {
                OnHealthChanged(Health);
            }
            if (OnMaxHealthChanged != null)
            {
                OnMaxHealthChanged(MaxHealth);
            }
            if (OnShieldChanged != null)
            {
                OnShieldChanged(Shield);
            }
            if (OnMaxShieldChanged != null)
            {
                OnMaxShieldChanged(MaxShield);
            }
        }
    }

    #endregion
}
