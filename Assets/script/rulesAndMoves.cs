using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class rulesAndMoves : MonoBehaviour
{

    class Field
    {
        public int[] pawns;
        public int colorOfPawns;//0-nothing , -1 -white ,1-black
        public int numberOfPawns;//0-15

        public Field()
        {
            pawns = new int[15];
            colorOfPawns = 0;
            numberOfPawns = 0;
        }
    };

    Field[] featuresOfFields;
    private int sender = -1, receveir = -1;
    public GameObject[] fields;
    public GameObject[] blackPawns;
    public GameObject[] whitePawns;
    public Sprite[] whiteCube;
    public Sprite[] BlackCube;
    public Image[] cube;
    private bool[] canMove;
    private bool canEndTour;
    private int tour = 1;//1 black, -1 white 
    private ArrayList moves;
    private float[] posYPawns;
    public Text[] howManyPanwsText;
    public Button rollButton, endTourButton;
    private int minb = 0, maxw = 24;
    public Text pl1Name, pl2Name, tura;
    setingsOfGame soG;
    string pl1 = "Player 1", pl2 = "Player 2";
    int tryb = 0;//0-singleplayer,1-Multiplayer,2-NoPlayer

    // Use this for initialization
    void Start()
    {
        soG = GameObject.FindGameObjectWithTag("seti").GetComponent<setingsOfGame>();
        soG.getopj();
        canEndTour = false;
        endTourButton.enabled = false;
        rollButton.enabled = false;

        cube[0].enabled = cube[1].enabled = true;
        cube[2].enabled = cube[3].enabled = false;

        moves = new ArrayList();
        featuresOfFields = new Field[fields.Length];
        canMove = new bool[fields.Length];
        posYPawns = new float[10];

        //ustawianie odpowiednich wlasciwosci pól
        for (int i = 0; i < fields.Length; i++)
        {
            canMove[i] = false;
            featuresOfFields[i] = new Field();
            fields[i].SendMessage("setNum", i, SendMessageOptions.DontRequireReceiver);
        }
        //wlasniwosci "dworów"
        //bialego
        fields[26].SendMessage("setNum", 0, SendMessageOptions.DontRequireReceiver);
        featuresOfFields[26].colorOfPawns = -1;
        //czarnego
        fields[27].SendMessage("setNum", 25, SendMessageOptions.DontRequireReceiver);
        featuresOfFields[27].colorOfPawns = 1;


        //Status poczatkowy planszy
        //pozycja startowa pionkow czarnych
        featuresOfFields[12].numberOfPawns = 5;
        featuresOfFields[12].colorOfPawns = 1;
        featuresOfFields[12].pawns[0] = 8;
        featuresOfFields[12].pawns[1] = 9;
        featuresOfFields[12].pawns[2] = 10;
        featuresOfFields[12].pawns[3] = 11;
        featuresOfFields[12].pawns[4] = 12;

        featuresOfFields[19].numberOfPawns = 5;
        featuresOfFields[19].colorOfPawns = 1;
        featuresOfFields[19].pawns[0] = 0;
        featuresOfFields[19].pawns[1] = 1;
        featuresOfFields[19].pawns[2] = 2;
        featuresOfFields[19].pawns[3] = 3;
        featuresOfFields[19].pawns[4] = 4;

        featuresOfFields[17].numberOfPawns = 3;
        featuresOfFields[17].colorOfPawns = 1;
        featuresOfFields[17].pawns[0] = 5;
        featuresOfFields[17].pawns[1] = 6;
        featuresOfFields[17].pawns[2] = 7;

        featuresOfFields[1].numberOfPawns = 2;
        featuresOfFields[1].colorOfPawns = 1;
        featuresOfFields[1].pawns[0] = 13;
        featuresOfFields[1].pawns[1] = 14;

        //pozycja startowa pionkow 
        featuresOfFields[6].numberOfPawns = 5;
        featuresOfFields[6].colorOfPawns = -1;
        featuresOfFields[6].pawns[0] = 0;
        featuresOfFields[6].pawns[1] = 1;
        featuresOfFields[6].pawns[2] = 2;
        featuresOfFields[6].pawns[3] = 3;
        featuresOfFields[6].pawns[4] = 4;

        featuresOfFields[13].numberOfPawns = 5;
        featuresOfFields[13].colorOfPawns = -1;
        featuresOfFields[13].pawns[0] = 8;
        featuresOfFields[13].pawns[1] = 9;
        featuresOfFields[13].pawns[2] = 10;
        featuresOfFields[13].pawns[3] = 11;
        featuresOfFields[13].pawns[4] = 12;

        featuresOfFields[8].numberOfPawns = 3;
        featuresOfFields[8].colorOfPawns = -1;
        featuresOfFields[8].pawns[0] = 5;
        featuresOfFields[8].pawns[1] = 6;
        featuresOfFields[8].pawns[2] = 7;

        featuresOfFields[24].numberOfPawns = 2;
        featuresOfFields[24].colorOfPawns = -1;
        featuresOfFields[24].pawns[0] = 13;
        featuresOfFields[24].pawns[1] = 14;

        posYPawns[0] = blackPawns[8].transform.position.y;
        posYPawns[1] = blackPawns[9].transform.position.y;
        posYPawns[2] = blackPawns[10].transform.position.y;
        posYPawns[3] = blackPawns[11].transform.position.y;
        posYPawns[4] = blackPawns[12].transform.position.y;

        posYPawns[5] = blackPawns[0].transform.position.y;
        posYPawns[6] = blackPawns[1].transform.position.y;
        posYPawns[7] = blackPawns[2].transform.position.y;
        posYPawns[8] = blackPawns[3].transform.position.y;
        posYPawns[9] = blackPawns[4].transform.position.y;

        int k, d;
        do
        {
            moves.Clear();
            k = Random.Range(0, 6);
            d = Random.Range(0, 6);
            moves.Add(k + 1);
            moves.Add(d + 1);
            cube[0].sprite = whiteCube[k];
            cube[1].sprite = BlackCube[d];
            tour = (k < d) ? 1 : -1;
        } while (k == d);
        checkPosibleMoves();
        if (tryb == 0)
        {
            pl2 = "Computer";
            endTourButton.gameObject.SetActive(false);
        }
    }

    void OnGUI()
    {
        pl1Name.text = "Gracz 1: " + pl1;
        pl2Name.text = "Gracz 2: " + pl2;
        if (tour == -1) tura.text = "Tura gracza " + pl2;
        else if (tour == 1) tura.text = "Tura gracza " + pl1;
        /*
         * kolor czerwony mozna pionkiem wykonac ruch
         * kolor bialy na polu mozna postawic pionek
        */
        for (int i = 0; i < featuresOfFields.Length; i++)
        {
            howManyPanwsText[i].text = featuresOfFields[i].numberOfPawns.ToString();
            if (tryb == 2 || (tryb == 0 && tour == -1)) { howManyPanwsText[i].color = Color.black; }
            else if (canMove[i]) howManyPanwsText[i].color = Color.red;
            else howManyPanwsText[i].color = Color.black;
        }
        if (!(tryb == 2 || (tryb == 0 && tour == -1)) && sender > -1)
        {
            if (tour == 1)
            {
                for (int i = 0; i < moves.Count; i++)
                {
                    int j = sender + (int)moves[i];
                    if (j < 25 && j > 0 && (featuresOfFields[j].numberOfPawns < 2 || featuresOfFields[j].colorOfPawns == 1))
                    {
                        howManyPanwsText[j].color = Color.white;
                    }
                    else if (minb > 18 && ((j > 24 && minb == sender) || j == 24) && (((int)moves[i]) < 7))
                    {
                        howManyPanwsText[27].color = Color.white;
                    }
                }
            }
            else
            {
                for (int i = 0; i < moves.Count; i++)
                {
                    int j = (sender - (int)moves[i]);
                    if (j < 25 && j > 0 && (featuresOfFields[j].numberOfPawns < 2 || featuresOfFields[j].colorOfPawns == -1))
                    {
                        howManyPanwsText[j].color = Color.white;
                    }
                    else if (maxw < 7 && ((j < 1 && maxw == sender) || j == 0) && (((int)moves[i]) < 7))
                    {
                        howManyPanwsText[26].color = Color.white;
                    }
                }

            }
        }
    }

    void rollCube()
    {
        rollButton.enabled = false;
        sender = receveir = -1;
        moves.Clear();
        int k = 0, d = 0;
        cube[0].enabled = cube[1].enabled = true;
        cube[3].enabled = cube[2].enabled = false;

        k = Random.Range(0, 6);
        d = Random.Range(0, 6);
        moves.Add(k + 1);
        moves.Add(d + 1);
        cube[0].sprite = (tour == 1) ? BlackCube[k] : whiteCube[k];
        cube[1].sprite = (tour == 1) ? BlackCube[d] : whiteCube[d];
        if (k == d)
        {
            moves.Add(k + 1);
            moves.Add(d + 1);

            cube[3].enabled = cube[2].enabled = true;
            cube[2].sprite = cube[0].sprite;
            cube[3].sprite = cube[1].sprite;
        }
        checkPosibleMoves();
    }

    void endTour()
    {
        if (featuresOfFields[26].numberOfPawns == 15 || featuresOfFields[27].numberOfPawns == 15) return;
        sender = receveir = -1;
        rollButton.enabled = true;
        canEndTour = false;
        tour = (tour == 1) ? (-1) : (1);
        for (int i = 1; i < fields.Length; i++)
        {
            canMove[i] = false;
            if (tryb == 2 || (tryb == 0 && tour == -1))
                fields[i].SendMessage("setPlayer", false, SendMessageOptions.DontRequireReceiver);
            else
                fields[i].SendMessage("setPlayer", true, SendMessageOptions.DontRequireReceiver);
        }
        endTourButton.enabled = false;
    }

    void checkPosibleMoves()
    {
        int i, j, k;
        minb = 24;
        maxw = 0;
        for (i = 0; i < canMove.Length; i++)
        {
            canMove[i] = false;
            if ((minb > i) && (featuresOfFields[i].colorOfPawns == 1) && (featuresOfFields[i].numberOfPawns > 0)) minb = i;
            if ((25 > i) && (featuresOfFields[i].colorOfPawns == -1) && (featuresOfFields[i].numberOfPawns > 0)) maxw = i;
        }
        if (tour == 1)
        {
            if (featuresOfFields[0].numberOfPawns > 0)
            {
                for (i = 0; i < moves.Count; i++)
                {
                    j = (int)moves[i];
                    if (j < 7 && (featuresOfFields[j].numberOfPawns < 2 || featuresOfFields[j].colorOfPawns == 1))
                    {
                        minb = 0;
                        sender = 0;
                        canMove[0] = true;
                        return;
                    }

                }
            }
            else
            {
                for (k = 1; k < 25; k++)
                    if (featuresOfFields[k].numberOfPawns > 0 && featuresOfFields[k].colorOfPawns == 1)
                        for (i = 0; i < moves.Count; i++)
                        {
                            j = k + (int)moves[i];
                            if (j < 25 && (featuresOfFields[j].numberOfPawns < 2 || featuresOfFields[j].colorOfPawns == 1))
                            {
                                canMove[k] = true;
                            }
                            else if (minb > 18 && ((j > 24 && k == minb) || j == 25) && (((int)moves[i]) < 7))
                            {
                                canMove[k] = true;
                            }
                        }
            }
        }
        else
        {
            if (featuresOfFields[25].numberOfPawns > 0)
            {
                for (i = 0; i < moves.Count; i++)
                {
                    j = 25 - (int)moves[i];
                    if (j > 18 && (featuresOfFields[j].numberOfPawns < 2 || featuresOfFields[j].colorOfPawns == -1))
                    {
                        sender = 25;
                        maxw = 25;
                        canMove[25] = true;
                        return;
                    }
                }
            }
            else
            {
                for (k = 1; k < 25; k++)
                    if (featuresOfFields[k].numberOfPawns > 0 && featuresOfFields[k].colorOfPawns == -1)
                        for (i = 0; i < moves.Count; i++)
                        {
                            j = k - (int)moves[i];
                            if (j > 0 && (featuresOfFields[j].numberOfPawns < 2 || featuresOfFields[j].colorOfPawns == -1))
                            {
                                canMove[k] = true;
                            }
                            else if (maxw < 7 && ((j < 1 && (k == maxw)) || j == 0) && (((int)moves[i]) < 7))
                            {
                                canMove[k] = true;
                            }
                        }
            }
        }
    }

    public void clickField(int r)
    {
        if (featuresOfFields[26].numberOfPawns == 15 || featuresOfFields[27].numberOfPawns == 15) return;
        int i, j;
        if (tryb == 2 || (tryb == 0 && tour == -1))
            if (howManyPanwsText[26].color.Equals(Color.white)) r = 0;
            else if (howManyPanwsText[27].color.Equals(Color.white)) r = 25;
        if (r != sender && sender > -1)
        {

            if (tour == 1)
            {
                for (i = 0; i < moves.Count; i++)
                {
                    j = sender + (int)moves[i];
                    if (r == j && (featuresOfFields[j].numberOfPawns < 2 || featuresOfFields[j].colorOfPawns == 1))
                    {
                        moves[i] = 10000;
                        cube[i].enabled = false;
                        receveir = r;
                        break;
                    }
                    else if (minb > 18 && ((j > 24 && sender == minb) || 25 == j) && r == 25 && (((int)moves[i]) < 7))
                    {
                        moves[i] = 10000;
                        cube[i].enabled = false;
                        receveir = r;
                        break;
                    }
                }
            }
            else
            {
                for (i = 0; i < moves.Count; i++)
                {
                    j = (sender - (int)moves[i]);
                    if (r == j && (featuresOfFields[j].numberOfPawns < 2 || featuresOfFields[j].colorOfPawns == -1))
                    {
                        moves[i] = 10000;
                        cube[i].enabled = false;
                        receveir = r;
                        break;
                    }
                    else if (maxw < 7 && ((j < 1 && sender == maxw) || j == 0) && r == 0 && (((int)moves[i]) < 7))
                    {
                        moves[i] = 10000;
                        cube[i].enabled = false;
                        receveir = r;
                        break;
                    }
                }

            }
        }

        if (canMove[r] && receveir == -1) sender = r;

        if (sender > -1 && receveir > -1 && r == receveir)
        {
            if (receveir == 0 && maxw < 7) receveir = 26;
            if (receveir == 25 && minb > 18) receveir = 27;
            int q = 0;
            //zbicie pionka przeciwnika
            if (featuresOfFields[sender].colorOfPawns != featuresOfFields[receveir].colorOfPawns &&
                featuresOfFields[receveir].colorOfPawns != 0)
            {
                int t = 0;
                if (featuresOfFields[receveir].colorOfPawns == 1)
                {
                    blackPawns[featuresOfFields[receveir].pawns[0]].transform.position = fields[0].transform.position;
                }
                else if (featuresOfFields[receveir].colorOfPawns == -1)
                {
                    whitePawns[featuresOfFields[receveir].pawns[0]].transform.position = fields[25].transform.position;
                    t = 25;
                }
                featuresOfFields[receveir].numberOfPawns = 0;
                featuresOfFields[t].colorOfPawns = featuresOfFields[receveir].colorOfPawns;
                featuresOfFields[t].pawns[featuresOfFields[t].numberOfPawns] = featuresOfFields[receveir].pawns[0];
                featuresOfFields[t].numberOfPawns++;


            }
            if (!(tour == -1 && receveir == 26) && !(tour == 1 && receveir == 27))
            {
                //zwykly ruch;
                q = featuresOfFields[receveir].numberOfPawns;
                if (featuresOfFields[receveir].numberOfPawns > 4) q = 4;
                if (receveir > 12)
                {
                    q += 5;
                }


                if (featuresOfFields[sender].colorOfPawns == 1)
                    blackPawns[featuresOfFields[sender].pawns[(featuresOfFields[sender].numberOfPawns - 1)]].transform.position
                        = new Vector3(fields[receveir].transform.position.x, posYPawns[q]);
                else if (featuresOfFields[sender].colorOfPawns == -1)
                    whitePawns[featuresOfFields[sender].pawns[(featuresOfFields[sender].numberOfPawns - 1)]].transform.position
                        = new Vector3(fields[receveir].transform.position.x, posYPawns[q]);
            }
            //usuniecie pionka z gry
            else
            {
                if (featuresOfFields[sender].colorOfPawns == 1)
                    blackPawns[featuresOfFields[sender].pawns[featuresOfFields[sender].numberOfPawns - 1]].transform.position
                        = fields[27].transform.position;
                else if (featuresOfFields[sender].colorOfPawns == -1)
                    whitePawns[featuresOfFields[sender].pawns[featuresOfFields[sender].numberOfPawns - 1]].transform.position
                        = fields[26].transform.position;
            }
            featuresOfFields[receveir].colorOfPawns = featuresOfFields[sender].colorOfPawns;
            featuresOfFields[sender].numberOfPawns--;
            featuresOfFields[receveir].pawns[featuresOfFields[receveir].numberOfPawns] = featuresOfFields[sender].pawns[featuresOfFields[sender].numberOfPawns];
            if (featuresOfFields[sender].numberOfPawns < 0) featuresOfFields[sender].numberOfPawns = 0;
            if (featuresOfFields[sender].numberOfPawns == 0) featuresOfFields[sender].colorOfPawns = 0;
            featuresOfFields[receveir].numberOfPawns++;


            sender = -1;
            receveir = -1;
        }
        checkPosibleMoves();

        for (i = 0; i < canMove.Length; i++) if (canMove[i]) return;
        endTourButton.enabled = true;
        canEndTour = true;
    }

    IEnumerator END_game()
    {
        yield return new WaitForSeconds(10f);
        back();
    }



    void Update()
    {
        if (tryb == 2 || (tryb == 0 && tour == -1))
        {
            if (endTourButton.enabled) endTour();
            if (rollButton.enabled) rollCube();

            clickField(Random.Range(0, 26));
        }
        else if (tryb == 0 && canEndTour) endTour();

        if (featuresOfFields[26].numberOfPawns >= 15 || featuresOfFields[27].numberOfPawns >= 15)
        {
            StartCoroutine(END_game());
        }
    }
    void back()
    {
        SceneManager.LoadSceneAsync("Menu", LoadSceneMode.Single);
    }

    public void setpl1(string a)
    {
        pl1 = a;
    }

    public void setpl2(string a)
    {
        pl2 = a;
    }

    public void settryb(int a)
    {
        tryb = a;
    }

}


