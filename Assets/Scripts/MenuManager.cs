using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void OyunuBaslat()
    {
        // Build Profiles listesindeki 1 numaralý sahneyi açar
        SceneManager.LoadScene(1);
        Time.timeScale = 1f; // Oyunun donuk kalmadýðýndan emin olalým
    }

    public void OyundanCik()
    {
        Debug.Log("Çýkýþ yapýldý.");
        Application.Quit();
    }
}
