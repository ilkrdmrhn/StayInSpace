using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemiKontrol : MonoBehaviour
{
    private  float hiz = 10f;
    public float inceAyar = 0.7f;
    public GameObject Mermi;
    public float mermininHizi=100f;
    public float atesEtmeAraligi=2f;
    public float can = 400f;
    float xmin ;
    float xmax ;

    public AudioClip Atessesi;
    public AudioClip OlumSesi;
    // Start is called before the first frame update
    void Start()
    {
        float uzaklik = transform.position.z - Camera.main.transform.position.z;
        Vector3 solUc = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, uzaklik));
        Vector3 sagUc = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, uzaklik));
        xmin = solUc.x + inceAyar;
        xmax = sagUc.x - inceAyar;
    }

    void AtesEtme ()
    {
        GameObject gemimizinMermisi = Instantiate(Mermi, transform.position + new Vector3(0,1f,0), Quaternion.identity) as GameObject;
            gemimizinMermisi.GetComponent<Rigidbody2D>().velocity = new Vector3(0, mermininHizi, 0);
        AudioSource.PlayClipAtPoint(Atessesi, transform.position);
    }
    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            InvokeRepeating("AtesEtme", 0.0000001f, atesEtmeAraligi);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            CancelInvoke("AtesEtme");
        }
        //geminin x deðeri -8 ile 8 arasýnda ise x deðeri atar.
        float yenix = Mathf.Clamp(transform.position.x, xmin, xmax);
        transform.position = new Vector3(yenix, transform.position.y, transform.position.z);

        if(Input.GetKey(KeyCode.LeftArrow))
        {
            // transform.position += new Vector3(-hiz*Time.deltaTime, 0,0);
            transform.position += Vector3.left * hiz * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            // transform.position += new Vector3(hiz * Time.deltaTime, 0,0);
            transform.position += Vector3.right * hiz * Time.deltaTime;
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        MermiKontrolu carpanMermi = collision.gameObject.GetComponent<MermiKontrolu>();
        if (carpanMermi)
        {
            carpanMermi.CarptigindaYokOl();
            can -= carpanMermi.ZararVerme();
            if (can <= 0)
            {
                Destroy(gameObject);
                AudioSource.PlayClipAtPoint(OlumSesi, transform.position);
            }
        }
    }
}
