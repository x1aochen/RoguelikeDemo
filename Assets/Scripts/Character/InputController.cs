using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class InputController : MonoBehaviour
    {
        private float moveX, moveY;
        private PlayerController player;
        private Vector3 cursorPos;
        private Vector3 lastMousePos;
        private Transform cursor;

        private void Awake()
        {
            player = PlayerController.instance;
            cursor = GameObject.Find("Cursor").transform;
        }

        private void Update()
        {
            Fire();
            Move();
            Rotate();
            
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                player.SwitchWeapon(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                player.SwitchWeapon(1);
            }

        }

        private void Fire()
        {
            if (Input.GetMouseButton(0))
            {
                player.Attack();
            }

        }

        private void Rotate()
        {
            //准星点位置，将屏幕坐标系中 鼠标位置转为世界坐标
            cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            cursorPos.z = 0;
            //鼠标没动过就不要做判断了
            //if (cursorPos == lastMousePos)
            //{
            //    return;
            //}

            cursor.position = new Vector3(cursorPos.x, cursorPos.y, cursor.position.z);

            player.Rotate(cursorPos);
            lastMousePos = cursorPos;
        }


        private void Move()
        {
            moveX = Input.GetAxis("Horizontal");
            moveY = Input.GetAxis("Vertical");

        }


        private void FixedUpdate()
        {
            player.Move(moveX, moveY);
        }



    }
}
