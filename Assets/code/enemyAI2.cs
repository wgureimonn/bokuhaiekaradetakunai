using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class enemyAI2 : MonoBehaviour
{
	public AudioClip em2;
	private AudioSource au;
    int now=0;
    int g=1;
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
    private int co=0;
    public static bool flag2;
    private Vector2Int pos;
    private Vector2Int ipos;
    
    int x;
    int xa;
    int z = 0;
    int za;
    Ray ray;
    RaycastHit hitInfo;
    public GameObject kaminome;
    
    

   // Use this for initialization
    void Start()
    {
        
        hp=20;
        pos = new Vector2Int(0, 0);
        SI = new int[10,10];
        SC = new int[10,10];
        au = gameObject.GetComponent<AudioSource>();
        openclose();
        suitei();
        cost();
        score();
        InvokeRepeating("idou", 1, 0.71f);
    }
    private void score()
    {
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
        for (x = 0; x < 10; x++)
        {
            for (z = 0; z < 10; z++)
            {
                if (wallmaker2.map[x, z] == 0||wallmaker2.map[x,z]==3)
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
                if(OC[x,z]==3){
                    OC[x,z]=0;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
       if(SI[pos.x, pos.y] <= 7){
            flush.flag3=true; 
            ppp.flag4 = true;
       }
        if(flag2){
            flag2=false;
            openclose();
            suitei();
            cost();
            score();

        }
        
        

    }
    void idou()
    {
        if(SI[pos.x, pos.y] <= 7){
            flush.flag3=true;         
        }
        round();
        transform.position = new Vector3(pos.x, 1, pos.y);
        au.PlayOneShot(em2);
        if(pos.x==9&&pos.y==9){
            SceneManager.LoadScene("gameover");
        }
        if(wallmaker2.map[pos.x,pos.y]==3){
            hp=hp-3;
        }
        hp--;
        if(hp<0){
            Destroy(gameObject);
            hakai();
        }

    }
    void round()
    {
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
            hakai();
            enemyAI.flag = true;
            enemyAI2.flag2 = true;
            flush.flag3 = true;
            ppp.flag4 = true;
            Destroy(gameObject);
        }
        else
        {
            pos += houkou[Random.Range(0, houkou.Count)];
        }
    }

    private void hakai()
    {
        if (Physics.Raycast(kaminome.transform.position, kaminome.transform.forward, out hitInfo, 1))
        {
            if (hitInfo.collider.gameObject.CompareTag("wall"))
            {
                Destroy(hitInfo.collider.gameObject);
                wallmaker2.map[pos.x, pos.y + 1] = 0;

            }
        }
        if (Physics.Raycast(kaminome.transform.position, kaminome.transform.right, out hitInfo, 1))
        {
            if (hitInfo.collider.gameObject.CompareTag("wall"))
            {
                Destroy(hitInfo.collider.gameObject);
                wallmaker2.map[pos.x + 1, pos.y] = 0;
            }
        }
        if (Physics.Raycast(kaminome.transform.position, -kaminome.transform.forward, out hitInfo, 1))
        {
            if (hitInfo.collider.gameObject.CompareTag("wall"))
            {
                Destroy(hitInfo.collider.gameObject);
                wallmaker2.map[pos.x, pos.y - 1] = 0;
            }
        }
        if (Physics.Raycast(kaminome.transform.position, -kaminome.transform.right, out hitInfo, 1))
        {
            if (hitInfo.collider.gameObject.CompareTag("wall"))
            {
                Destroy(hitInfo.collider.gameObject);
                wallmaker2.map[pos.x - 1, pos.y] = 0;
            }
        }
    }
}
