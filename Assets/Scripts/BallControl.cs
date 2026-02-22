using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class BallControl : MonoBehaviour
{
    // YENÝ: Paneller için referanslar
    public GameObject WinPanel;
    public GameObject LosePanel;

    public TextMeshProUGUI TimeText;
    public TextMeshProUGUI ChanceText;
    private PlayerControls controls;
    private Vector2 moveInput;
    private Rigidbody rb;
    public float speed = 10f;
    float timer = 10;
    int chanceTimer = 3;
    bool isGameOver = false; // Oyun bittiðinde her þeyi durdurmak için

    void Start()
    {
        ChanceText.text = chanceTimer.ToString();
        Time.timeScale = 1f; // Sahne yüklenince zamaný akýt

        // Panellerin kapalý baþladýðýndan emin olalým
        if (WinPanel != null) WinPanel.SetActive(false);
        if (LosePanel != null) LosePanel.SetActive(false);
    }

    void Update()
    {
        if (isGameOver) return; // Oyun bittiyse daha fazla iþlem yapma

        if (timer > 0)
        {
            timer -= Time.deltaTime;
            TimeText.text = timer.ToString("F0");
        }
        else
        {
            timer = 0;
            TimeText.text = "0";
            FinishGame(false); // Süre biterse KAYBETTÝN
        }
    }

    void Awake()
    {
        // 1. Önce referanslarý ve input sistemini hazýrla
        rb = GetComponent<Rigidbody>();
        controls = new PlayerControls();

        // BURASI DEÐÝÞTÝ: Action Map ismini "Player" yaptýysan böyle çaðýrmalýsýn
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;
    }

    // Yeni sistemde Input'larý aktif/pasif etmemiz gerekir
    void OnEnable() => controls.Enable();
    void OnDisable() => controls.Disable();

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(moveInput.x, 0, moveInput.y);
        rb.AddForce(movement * speed);
    }

    void OnCollisionEnter(Collision cls)
    {
        if (isGameOver) return;

        // 1. DURUM: FINISH (KAZANMA)
        if (cls.gameObject.name == "Finish")
        {
            FinishGame(true); // KAZANDIN
        }

        // 2. DURUM: ENGEL (CAN KAYBI)
        else if (cls.gameObject.CompareTag("Engel"))
        {
            if (chanceTimer > 0)
            {
                chanceTimer -= 1;
                ChanceText.text = chanceTimer.ToString();
            }

            if (chanceTimer <= 0)
            {
                FinishGame(false); // Can biterse KAYBETTÝN
            }
        }
    }
    // Ortak Bitiþ Fonksiyonu
    void FinishGame(bool win)
    {
        isGameOver = true;
        Time.timeScale = 0f; // Oyunu dondur
        controls.Disable(); // Kontrolleri kapat

        if (win)
        {
            WinPanel.SetActive(true);
            Debug.Log("Kazandýn Paneli Açýldý");
        }
        else
        {
            LosePanel.SetActive(true);
            Debug.Log("Kaybettin Paneli Açýldý");
        }

        // Fareyi görünür yap (Butonlara basabilmek için)
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // BUTONLAR ÝÇÝN FONKSÝYONLAR
    public void RetryGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void QuitGame()
    {
        Debug.Log("Çýkýþ Yapýldý");
        Application.Quit();
    }
}
