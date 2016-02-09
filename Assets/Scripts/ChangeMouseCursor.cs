using UnityEngine;
using System.Collections;

public class ChangeMouseCursor : MonoBehaviour {

    public Texture2D cursorTexture;
    //private Transform cursor;

	void Start () {
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
        //Cursor.visible = false;
        //cursor = GameObject.Find("Cursor").GetComponent<Transform>();
	}

    /*
    void Update() {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursor.position = new Vector3(mousePos.x, mousePos.y, 0);
    }
    */
}
