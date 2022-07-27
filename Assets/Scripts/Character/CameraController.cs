using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Player
{
    public class CameraController : MonoBehaviour
    {
        private Transform target;
        [SerializeField]
        private float moveSpeed;

        private void Start()
        {
            target = PlayerController.instance.transform;
        }

        private void LateUpdate()
        {
            transform.position = Vector3.Lerp(transform.position,
                new Vector3(target.position.x, target.position.y, transform.position.z)
                , moveSpeed * Time.deltaTime);
        }
    }
}