using UnityEngine;
using System.Collections;

public class ChooseBloodSplatter : MonoBehaviour {

    public Sprite[] splatters;

    private SpriteRenderer sr;

	void Start () {
	    sr = GetComponent<SpriteRenderer>();
        int choice = Random.Range(0, splatters.Length);
        sr.sprite = splatters[choice];
        transform.localScale = new Vector3(0.75f, 0.75f, 1);
	}
}
