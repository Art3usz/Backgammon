using UnityEngine;
using System.Collections;

public class numberOfField : MonoBehaviour
{
    private int number = 0;
    private bool player = true;
    void OnMouseDown()
    {
        if (player)
            sendNum();
    }

    void sendNum()
    {
        GameObject.FindGameObjectWithTag("kontroler").GetComponent<rulesAndMoves>().clickField(number);
    }
    void setNum(int k)
    {
        number = k;
    }

    void setPlayer(bool k=true)
    {
        player = k;
    }
}
