using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//İLKER
public class DusmanlarınCiktigiYer : MonoBehaviour
{
    public GameObject dusmanPrefabi;
    public float genislik;
    public float yukseklik;
    private float hiz = 5f;
    private  bool SagaHareket = true;
    private float xmax;
    private float xmin;
    public float yaratmayiGeciktirmeSuresi = 0.7f;

    // Start is called before the first frame update
    void Start()
    {
        float objeIleKameraninZsininFarki = transform.position.z - Camera.main.transform.position.z;
        Vector3 kameraninSolTarafi = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, objeIleKameraninZsininFarki));
        Vector3 kameraninSagTarafi = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, objeIleKameraninZsininFarki));
        xmax = kameraninSagTarafi.x;
        xmin = kameraninSolTarafi.x;
        DusmanlarintekTekYaratilmasi();
        //DusmanlarinYaratilmasi();
        
    }

    void DusmanlarinYaratilmasi()
    {
        foreach (Transform cocuk in transform)
        {
            GameObject dusman = Instantiate(dusmanPrefabi, cocuk.transform.position, Quaternion.identity) as GameObject;
            dusman.transform.parent = cocuk;
        }
    }

    void DusmanlarintekTekYaratilmasi()
    {
        Transform uygunPozison = SonrakiUygunPozisyon();
        if (uygunPozison)
        {
            GameObject dusman = Instantiate(dusmanPrefabi, uygunPozison.transform.position, Quaternion.identity) as GameObject;
            dusman.transform.parent = uygunPozison;
        }

        if (SonrakiUygunPozisyon())
        {
            Invoke("DusmanlarintekTekYaratilmasi", yaratmayiGeciktirmeSuresi);
        }
    }
    public void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(genislik, yukseklik));
    }
    // Update is called once per frame
    void Update()
    {
        if(SagaHareket)
        {
            //transform.position += new Vector3(hiz * Time.deltaTime, 0);
            transform.position += hiz*Vector3.right * Time.deltaTime;
        }
        else
        {
            transform.position += hiz * Vector3.left * Time.deltaTime; 
        }

        float sagSinir = transform.position.x + genislik/2;
        float solSinir = transform.position.x - genislik / 2;

        if(sagSinir>xmax )
        {
            SagaHareket = false;
           

        }
        else if(solSinir<xmin)
        {
            SagaHareket = true;
        }

        if (ButunDusmanlarOlduMu())
        {
            DusmanlarintekTekYaratilmasi();
            //DusmanlarinYaratilmasi();
        }
    }

    Transform SonrakiUygunPozisyon()
    {
        foreach (Transform CocuklarinPozisyonu in transform)
        {
            if (CocuklarinPozisyonu.childCount == 0)
            {
                return CocuklarinPozisyonu;
            }
        }

        return null;
    }

    bool ButunDusmanlarOlduMu()
    {
        foreach(Transform CocuklarinPozisyonu in transform)
        {
            if (CocuklarinPozisyonu.childCount > 0)
            {
                return false;
            }
        }

        return true;
    }
}
