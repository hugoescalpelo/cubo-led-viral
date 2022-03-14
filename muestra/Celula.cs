using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Celula : MonoBehaviour {
    public GameObject cellObject;
    public bool vivo = true;
    public int type = 0;
    //private bool lastState = true;
    float scale;

    public Celula() {
        vivo = false;
        type = -1;
    }

    public Celula(GameObject pref, Vector3 origin, GameObject parent, GameObject body) {
        cellObject = Instantiate(pref, origin, Quaternion.identity, parent.transform);
        cellObject.GetComponent<SpringJoint>().connectedBody = body.GetComponent<Rigidbody>();
        cellObject.SetActive(true);
        scale = cellObject.transform.localScale.x;
        float randomScale = Random.Range(1f, 2f);
        cellObject.transform.localScale *= randomScale;// new Vector3(scale, scale, scale);
    }

    public Celula(GameObject pref, Vector3 origin, GameObject parent, GameObject body, int tipe) {
        type = tipe;
        cellObject = Instantiate(pref, origin, Quaternion.identity, parent.transform);
        cellObject.GetComponent<SpringJoint>().connectedBody = body.GetComponent<Rigidbody>();
        cellObject.SetActive(true);
        scale = cellObject.transform.localScale.x;
        float randomScale = Random.Range(1f, 2f);
        cellObject.transform.localScale *= randomScale;
        //cellObject.GetComponent<SpringJoint>().connectedBody = body.GetComponent<Rigidbody>();
    }

    //public void viveViveMuere() {
    //    ///int vive = Random.Range(0, 2);
    //    if (vivo != cellObject.GetComponent<MeshRenderer>().enabled) {
    //        if (!vivo) {
    //            cellObject.transform.localScale = new Vector3(.3f, .3f, .3f);
    //        } else {
    //            cellObject.transform.localScale = new Vector3(scale, scale, scale);
    //            float randomScale = Random.Range(1f, 2f);
    //            cellObject.transform.localScale *= randomScale;
    //        }
    //    }
    //    cellObject.GetComponent<MeshRenderer>().enabled = vivo;// > 0 ? true : false;
    //    cellObject.name = vivo.ToString();// + " " + cellObject.name;
    //    /*
    //    if (vivo) {
    //        //cellObject.SetActive(vive > 0 ? true : false);
    //        cellObject.GetComponent<MeshRenderer>().enabled = vivo;// > 0 ? true : false;
    //    } else {
    //        //cellObject.SetActive(vive > 0 ? false : true);
    //        cellObject.GetComponent<MeshRenderer>().enabled = vivo;// > 0 ? true : false;
    //    }*/
    //}




    // Start is called before the first frame update
    /*void Start()
    {
        
    }
    */
    // Update is called once per frame
    //void Update() {
    //    Debug.Log(cellObject.name);
    //    if ((!lastState) && vivo) {
    //        cellObject.SetActive(true);
    //        lastState = vivo;
    //    } else if (lastState && !vivo) {
    //        cellObject.SetActive(false);
    //        lastState = vivo;
    //    }
    //}
}
