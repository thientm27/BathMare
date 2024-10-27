using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Player : MonoBehaviour
{
    public GameObject Camera;
    public GameObject FlashLight;
    // public bool DeleteSave;
    public bool Live = true;
    // public float HP = 100f;
    // public AudioClip OnFlashLight;
    // public AudioClip OffFlashLight;
    // public int DeathCount;
    // public float MouseSensitivity;
    // public float MasterVolume;
    // public int GraphicLevel = 1;
    // public int Language;
    // public Option option;
    // public Message message;
    // public DelayMessage delaymessage;
    // public CameraMove cam;
    // public ExamineRotation exam;
    public FirstPersonController fpsc;
    // public Pointer pointer;
    // public AudioSource soundplay;
    // public PassWordDoor passdoor;
    // public FlagManager fm;
    // public Title title;
    // public EscapeMenu escapemenu;
    public State NowState;

    private GameObject enemy;

    public enum State
    {
        None,
        Examining,
        UI,
        Movie
    }

    private void Start() => LoadOption();

    private void Update()
    {
        // if (Input.GetButtonDown("FlashLight")) {
        //     ToggleFlashLight();
        // }
        //
        // if (Input.GetButtonDown("Cancel"))
        // {
        // }
    }

    // public void EnterExamineMode(GameObject target, Vector3 position)
    // {
    //     SetCameraExamineMode(true);
    //     NowState = State.Examining;
    //     Cursor.lockState = CursorLockMode.None;
    //     Cursor.visible = true;
    //     EnableBlurEffect(true);
    //     exam.EnterExamine(target, position);
    //     pointer.Disable();
    //     DisableController();
    // }
    //
    // public void ExitExamineMode()
    // {
    //     NowState = State.None;
    //     EnableBlurEffect(false);
    //     pointer.Enable();
    //     SetCameraExamineMode(false);
    //     EnableController();
    // }

    public void TakeDamage(float duration, GameObject source)
    {
        // DisableController();
        // enemy = source;
        // iTween.ValueTo(gameObject, iTween.Hash(
        //     "from", HP,
        //     "to", 0,
        //     "time", duration,
        //     "onupdate", "UpdateHealth"
        // ));
    }

    // public void Die()
    // {
    //     Live = false;
    //     if (NowState == State.Examining)
    //         exam.ExitExamine();
    //
    //     if (NowState == State.UI)
    //         passdoor.ExitPassWord();
    //
    //     NowState = State.Movie;
    // }

    private void DisableController() => fpsc.enabled = false;
    
    private void EnableController() => fpsc.enabled = true;

    public void EnterPassWordMode()
    {
        NowState = State.UI;
        SetCameraExamineMode(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        EnableBlurEffect(true);
        // pointer.Disable();
        DisableController();
    }

    public void ExitPassWordMode()
    {
        NowState = State.None;
        EnableBlurEffect(false);
        // pointer.Enable();
        SetCameraExamineMode(false);
        EnableController();
    }

    private void ToggleFlashLight()
    {
        // if (!FlashLight.activeSelf)
        // {
        //     PlaySound(OnFlashLight);
        //     FlashLight.SetActive(true);
        // }
        // else
        // {
        //     PlaySound(OffFlashLight);
        //     FlashLight.SetActive(false);
        // }
    }

    private void SetCameraExamineMode(bool isExamining)
    {
        // var cameraMove = Camera.GetComponent<CameraMove>();
        // cameraMove.Examining = isExamining;
        // cameraMove.ToggleCamera();
    }

    private void EnableBlurEffect(bool enable)
    {
        // Camera.GetComponent<Blur>().enabled = enable;
    }

    private void UpdateHealth(float newHealth)
    {
        // HP = newHealth;
        //
        // if (HP <= 90f)
        // {
        //     DeathCount++;
        //     ES2.Save(DeathCount, "DeathCount");
        //     Save();
        //     Application.LoadLevel("Bathroom");
        // }
    }

    // private void PlaySound(AudioClip clip) => soundplay.PlayOneShot(clip);

    public void Save()
    {
        // ES2.Save(DeathCount, "DeathCount");
        // SaveOption();
    }

    public void SaveOption()
    {
        // ES2.Save(MouseSensitivity, "MouseSensitivity");
        // ES2.Save(MasterVolume, "MasterVolume");
        // ES2.Save(Language, "Language");
        // ES2.Save(GraphicLevel, "GraphicLevel");
    }

    private void Load()
    {
        // if (ES2.Exists("DeathCount"))
        // {
        //     GraphicLevel = ES2.Load<int>("GraphicLevel");
        //     DeathCount = ES2.Load<int>("DeathCount");
        //     MouseSensitivity = ES2.Load<float>("MouseSensitivity");
        //     MasterVolume = ES2.Load<float>("MasterVolume");
        //     Language = ES2.Load<int>("Language");
        // }
    }

    private void LoadOption()
    {
        // option.gameObject.SetActive(true);
        // option.LoadOption();
    }
}
