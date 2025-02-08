using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowing : MonoBehaviour
{
    public Transform playerToFollow;
    public float stopDistance;
    public float speed = 1;

    private Animator animator;
    private Vector2 lastPosition;

    void Start()
    {
        animator = GetComponent<Animator>(); // Obtiene el Animator

        if (playerToFollow == null)
        {
            playerToFollow = GameObject.FindGameObjectWithTag("Player").transform;
        }

        lastPosition = transform.position;
    }

    void Update()
    {
        if (playerToFollow != null)
        {
            Vector2 currentPosition = transform.position;

            if (Vector2.Distance(currentPosition, playerToFollow.position) > stopDistance)
            {
                Vector2 newPosition = Vector2.MoveTowards(currentPosition, playerToFollow.position, speed * Time.deltaTime);
                transform.position = newPosition;

                // Calcula la direcci?n del movimiento
                Vector2 moveDirection = newPosition - lastPosition;

                // Normaliza para obtener una direcci?n
                moveDirection.Normalize();

                // Actualiza los par?metros del Animator
                animator.SetFloat("Horizontal", moveDirection.x);
                animator.SetFloat("Vertical", moveDirection.y);
                animator.SetBool("IsMoving", moveDirection.magnitude > 0.01f); // Activa o desactiva la animaci?n

                lastPosition = newPosition;
            }
            else
            {
                animator.SetBool("IsMoving", false); // Si no se mueve, vuelve a Idle
            }
        }
    }
}