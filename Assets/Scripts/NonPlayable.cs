using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayable : MonoBehaviour
{

    public float dislayTime = 4f;
    
    public GameObject dialogBox;

    private float timerDisplay;

    // Start is called before the first frame update
    void Start()
    {

        dialogBox.SetActive(false);
        timerDisplay = -1.0f;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timerDisplay >= 0)
        {
            timerDisplay -= Time.deltaTime;
            if(timerDisplay < 0)
            {
                dialogBox.SetActive(false);
            }
        }
    }

    public void DisplayDialog()
    {
        timerDisplay = dislayTime;
        dialogBox.SetActive(true);
    }
}
