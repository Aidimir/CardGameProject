using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DragDrop : NetworkBehaviour
{
    public GameManager GameManager;
    public GameObject Canvas;
    public PlayerManager PlayerManager;

    private bool isDragging = false;
    private bool isOverDropZone = false;
    private bool isDraggable = true;
    private GameObject dropZone;
    private GameObject startParent;
    private Vector2 startPosition;

    private void Start()
    {
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        Canvas = GameObject.Find("Main Canvas");
        NetworkIdentity netWorkIdentity = NetworkClient.connection.identity;
        PlayerManager = netWorkIdentity.GetComponent<PlayerManager>();

        if (!hasAuthority)
        {
            isDraggable = false;
        }
    }
    void Update()
    {
        if (isDragging)
        {
         if(PlayerManager.IsMyTurn)
            {
                transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                transform.SetParent(Canvas.transform, true);
            }
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerManager.combine = false;

        if (collision.gameObject == PlayerManager.PlayerSockets[PlayerManager.CardsPlayed])
        {
            isOverDropZone = true;
            dropZone = collision.gameObject;
            PlayerManager.combine = false;
        }
        else if (collision.gameObject == PlayerManager.PlayerCombinerSockets[0] || collision.gameObject == PlayerManager.PlayerCombinerSockets[01])
        {
            isOverDropZone = true;
            dropZone = collision.gameObject;
            PlayerManager.combine = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isOverDropZone = false;
        dropZone = null;
        PlayerManager.combine = false;
    }

    public void StartDrag()
    {
        if (!isDraggable) return;
        startParent = transform.parent.gameObject;
        startPosition = transform.position;
        if (PlayerManager.IsMyTurn)
        {
            isDragging = true;
        }
    }

    public void EndDrag()
    {
        if (!isDraggable) return;
        isDragging = false;

        if (isOverDropZone && PlayerManager.IsMyTurn)
        {
            transform.SetParent(dropZone.transform, false);
            isDraggable = false;
            if (PlayerManager.FirstCombineSocketIsBusy == false && PlayerManager.SecondCombineSocketIsBusy == false && PlayerManager.combine == false)
            {
                PlayerManager.PlayCard(gameObject);
            }
            else
            {
                PlayerManager.ShowCard(gameObject);
            }
        }
        else
        {
            transform.position = startPosition;
            transform.SetParent(startParent.transform, false);
        }

        PlayerManager.combine = false;
    }
}
