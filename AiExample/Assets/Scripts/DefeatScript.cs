using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DefeatScript : MonoBehaviour
{
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void restart()
    {
        Game.Instance.SOMA.PlayMusic("8_Bit_Adventure");
        SceneManager.LoadScene(1);
    }
}
