using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    //[SerializeField] float maxSpeed;
    //[SerializeField] float rotationSpeed;
    //[SerializeField] float magnitude;

    //private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        //rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveDirection = Vector3.zero;
        
        if (Input.GetKey(KeyCode.W))
        {
            moveDirection += Vector3.up;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveDirection += -Vector3.right;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveDirection += -Vector3.up;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDirection += Vector3.right;
        }

        if (moveDirection != Vector3.zero)
        {
            transform.Translate(moveDirection.normalized * moveSpeed * Time.deltaTime, Space.World);
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 90);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Npc")
        {
            Game.Instance.SOMA.PlayMusic("victory"); 
            SceneManager.LoadScene(3);
           
            //if (Game.Instance != null)
            //    Game.Instance.SOMA.PlaySound("Explode");

        }
    }
}
