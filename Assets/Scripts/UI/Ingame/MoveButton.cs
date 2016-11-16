using UnityEngine;
using CnControls;
using UnityEngine.UI;

public class MoveButton : MonoBehaviour
{
    public bool isLeft = false;
    public PlayerMovement movement;

    public SimpleButton btnMove;
    public RawImage image;

    Color orgin;
    // Use this for initialization
    void Start()
    {
        if (movement == null)
        {
            movement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        }

        if (btnMove == null)
        {
            btnMove = GetComponent<SimpleButton>();
        }

        if (image == null)
        {
            image = GetComponent<RawImage>();
        }
        orgin = image.color;
    }

    // Update is called once per frame
    void Update()
    {
        bool enable = false;
        if (movement && btnMove && image)
        {
            if (isLeft)
            {
                enable = movement.CanMoveLeft();
            }
            else
            {
                enable = movement.CanMoveRight();
            }
        }
        btnMove.enabled = enable;
        image.enabled = enable;
        //if(enable)
        //{
        //    btnMove.enabled = true;
        //    image.color = orgin ;
        //}
        //else
        //{
        //    btnMove.enabled = false;
        //    image.color = orgin * new Color(1f,1f,1f,0.5f);
        //}
    }
}
