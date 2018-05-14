using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class enemyAI : MonoBehaviour
{
    int now=0;
    int g=1;
    public static bool flag;
    private int[,] OC;
    private int[,] SI;
    private int[,] C;
    private int[,] SC;
    int Gx = 10;
    int Gz = 10;
    int cc = 1;
    int cp;
    int sui;
    private int hp;
    private Vector2Int pos;
    private Vector2Int ipos;
    private Vector3 gen;

    int x;
    int xa;
    int z = 0;
    int za;
    float tm=1.0f;

    void Start()
    {
        hp =30;
        pos = new Vector2Int(0, 0);
        openclose();
        suitei();
        cost();
        score();
        InvokeRepeating("idou", 1, 1.0f);
    }
    private void score()
    {
        SC = new int[10, 10];
        for (x = 0; x < 10; x++)
        {
            for (z = 0; z < 10; z++)
            {
                if (OC[x, z] == 0 || OC[x, z] == 1)
                {
                    SC[x, z] = SI[x, z] + C[x, z];
                }
            }
        }
    }

    private void cost()
    {
        C = new int[10, 10];
        for (x = 0; x < 10; x++)
        {
            for (z = 0; z < 10; z++)
            {
                if ((x == pos.x) && (z == pos.y) && (OC[x, z] == 0))
                {
                    C[x, z] = 0;
                    OC[x, z] = 1;
                    if (z < 9)
                    {
                        if (OC[x, z + 1] == 0)
                        {
                            C[x, z + 1] = C[x, z] + 1;
                            OC[x, z + 1] = 1;
                            cp = 1;
                        }
                    }
                    if (x < 9)
                    {
                        if (OC[x + 1, z] == 0)
                        {
                            C[x + 1, z] = C[x, z] + 1;
                            OC[x + 1, z] = 1;
                            cp = 1;
                        }
                    }
                    if (z > 0)
                    {
                        if (OC[x, z - 1] == 0)
                        {
                            C[x, z - 1] = C[x, z] + 1;
                            OC[x, z - 1] = 1;
                            cp = 1;
                        }
                    }
                    if (x > 0)
                    {
                        if (OC[x - 1, z] == 0)
                        {
                            C[x - 1, z] = C[x, z] + 1;
                            OC[x - 1, z] = 1;
                            cp = 1;
                        }
                    }
                }
            }
        }
        cc = 1;
        while (true)
        {
            cp = 0;
            for (x = 0; x < 10; x++)
            {
                for (z = 0; z < 10; z++)
                {
                    if (C[x, z] == cc)
                    {
                        if (z < 9)
                        {
                            if (OC[x, z + 1] == 0)
                            {
                                C[x, z + 1] = C[x, z] + 1;
                                OC[x, z + 1] = 1;
                                cp = 1;
                            }
                        }
                        if (x < 9)
                        {
                            if (OC[x + 1, z] == 0)
                            {
                                C[x + 1, z] = C[x, z] + 1;
                                OC[x + 1, z] = 1;
                                cp = 1;
                            }
                        }
                        if (z > 0)
                        {
                            if (OC[x, z - 1] == 0)
                            {
                                C[x, z - 1] = C[x, z] + 1;
                                OC[x, z - 1] = 1;
                                cp = 1;
                            }
                        }
                        if (x > 0)
                        {
                            if (OC[x - 1, z] == 0)
                            {
                                C[x - 1, z] = C[x, z] + 1;
                                OC[x - 1, z] = 1;
                                cp = 1;
                            }
                        }
                    }
                }
            }

            if (cp == 0)
            {
                break;
            }
            cc = cc + 1;
        }

    }
    private void suitei()
    {
        SI = new int[10, 10];
        for (x = 0; x < 10; x++)
        {
            for (z = 0; z < 10; z++)
            {
                if (wallmaker2.map[x, z] == 0)
                {
                    SI[x, z] = Mathf.Abs(Gx - x) + Mathf.Abs(Gz - z);
                }
            }
        }

    }
    private void openclose()
    {
        OC = new int[10, 10];
        for (x = 0; x < 10; x++)
        {
            for (z = 0; z < 10; z++)
            {
                OC[x, z] = wallmaker2.map[x, z];
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("up"))
        {
            transform.position -= new Vector3(1, 0, 0);
        }
        if (Input.GetKeyDown("down"))
        {
            transform.position += new Vector3(1, 0, 0);
        }
        if (Input.GetKeyDown("right"))
        {
            transform.position += new Vector3(0, 0, 1);
        }
        if (Input.GetKeyDown("left"))
        {
            transform.position -= new Vector3(0, 0, 1);
        }
        if(flag){
            flag=false;
            openclose();
            cost();
            score();

        }

    }
    void idou()
    {
        if (now == 0 && SI[pos.x, pos.y] <= 7)
        {
            now = 1;
        }
        if (now == 1)
        {
            goal();
            now = 2;
            g=-1;
        }
        else
        {
            round();
        }
        transform.position =Vector3.Lerp(gen, new Vector3(pos.x, 1, pos.y),1);
        if(pos.x==9&&pos.y==9){
            SceneManager.LoadScene("gameover");
        }
        hp--;
        if(hp<0){
            Destroy(gameObject);
        }

    }
    void round()
    {
        gen = new Vector3(pos.x,1,pos.y);
        List<Vector2Int> houkou = new List<Vector2Int>();
        if ((pos.y < 9) && (C[pos.x, pos.y + 1] == C[pos.x, pos.y] + g))
        {
            houkou.Add(Vector2Int.up);
        }
        if ((pos.x < 9) && (C[pos.x + 1, pos.y] == C[pos.x, pos.y] + g))
        {
            houkou.Add(Vector2Int.right);
        }
        if ((pos.x > 0) && (C[pos.x - 1, pos.y] == C[pos.x, pos.y] + g))
        {
            houkou.Add(Vector2Int.left);
        }
        if ((pos.y > 0) && (C[pos.x, pos.y - 1] == C[pos.x, pos.y] + g))
        {
            houkou.Add(Vector2Int.down);
        }
        if (houkou.Count == 0)
        {
        if(pos.x==9&&pos.y==9){
            SceneManager.LoadScene("gameover");
        }
            openclose();
            cost();
            score();
        }
        else
        {
            pos += houkou[Random.Range(0, houkou.Count)];
        }
    }
    void goal()
    {
        ipos.x=pos.x;
        ipos.y=pos.y;
        pos.x=Gx-1;
        pos.y=Gz-1;
        openclose();
        cost();
        score();
        pos.x=ipos.x;
        pos.y=ipos.y;
    }
}
