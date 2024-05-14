using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCursor : MonoBehaviour
{
    [SerializeField] private Texture2D _crosshair;
    // Start is called before the first frame update
    void Start()
    {
        //Set the curosr origin to its center
        Vector3 cursorOffset = new Vector2(_crosshair.width/2, _crosshair.height);

        //Set the cursor to the crosshair sprite with given offset
        //and automatic switching to hardware default if necessary
        Cursor.SetCursor(_crosshair, cursorOffset, CursorMode.ForceSoftware);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
