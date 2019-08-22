using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class setingsOfGame : MonoBehaviour
{

    // Use this for initialization
    string pl1 = "Player 1", pl2 = "Player 2";
    int tryb = 0;//0-singleplayer,1-Multiplayer,2-NoPlayer
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadSceneAsync("Menu", LoadSceneMode.Single);
    }
    public void setpl1(string a)
    {
        if (a.Length == 0) a = "player 1";
        pl1 = a;
    }

    public void setpl2(string a)
    {
        if (a.Length == 0) a = "player 2";
        pl2 = a;
    }

    public void settryb(int a)
    {
        tryb = a;
        if (tryb == 0) pl2 = "Computer";
    }

    public void getopj()
    {
        rulesAndMoves rAM = GameObject.FindGameObjectWithTag("kontroler").GetComponent<rulesAndMoves>();
        if (rAM)
        {
            rAM.setpl1(pl1);
            rAM.setpl2(pl2);
            rAM.settryb(tryb);
        }
    }


}
