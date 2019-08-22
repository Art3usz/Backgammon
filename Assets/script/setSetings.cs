using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class setSetings : MonoBehaviour
{
    public InputField pl1, pl2;
    public Dropdown tryb;//0-singleplayer,1-Multiplayer,2-NoPlayer
    setingsOfGame soG;

    // Use this for initialization
    void Start()
    {
        soG = GameObject.FindGameObjectWithTag("seti").GetComponent<setingsOfGame>();

        soG.getopj();
    }

    public void setpl1(string a)
    {
        pl1.text = a;
    }

    public void setpl2(string a)
    {
        pl2.text = a;
    }

    public void settryb(int a)
    {
        tryb.value = a;
    }

    void loadPlayGame()
    {
        SceneManager.LoadSceneAsync("game", LoadSceneMode.Single);
        soG.setpl1(pl1.text);
        soG.setpl2(pl2.text);
        soG.settryb(tryb.value);
    }

    public void setNamepl1(Text k)
    {
        soG.setpl1(k.text);
    }

    public void setNamepl2(Text k)
    {
        soG.setpl2(k.text);
    }

    public void settryb(Dropdown k)
    {
        soG.settryb(k.value);
        if (k.value == 0) { pl2.text = "Computer"; pl2.enabled = false; } else pl2.enabled = true;
    }
}
