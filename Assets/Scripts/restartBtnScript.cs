using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class restartBtnScript : MonoBehaviour
{
    [SerializeField] GameMng gameManager;
    new SpriteRenderer renderer;
    [SerializeField] Color defaultColor, hoverColor, pressedColor;
    [SerializeField] float positionX = 0f, positionY= -3f;

    void Start()
    {
        transform.position = new Vector3(positionX, positionY, transform.position.z);
        renderer = GetComponent<SpriteRenderer>();
        gameManager = FindObjectOfType<GameMng>();
        renderer.color = defaultColor;
    }




    private void OnMouseEnter()
    {
        Debug.Log("Enter");
        renderer.color = hoverColor;
    }

    private void OnMouseExit()
    {
        renderer.color = defaultColor;
    }

    private void OnMouseDown()
    {
        renderer.color = pressedColor;
    }
    private void OnMouseUp()
    {
        renderer.color = hoverColor;
        gameManager.restart();
    }
}
