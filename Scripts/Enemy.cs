using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator animator;

    public int maxHealth = 100;
    int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void Damage(int damage)
    {
        currentHealth -= damage;

        //������������� �������� ����
        FindObjectOfType<AudioManager>().Play("Player");

        animator.SetTrigger("Hurt");
        

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy died!");

        //�������� ������
       
        animator.SetBool("IsDead", true);

        GetComponent<Collider>().enabled = false; //�������� ������ � ���������� � ���������� ��������� ����������
        GetComponent<EnemyAgainWithoutAttack>().enabled = false;

        //������� ����� �� �����
        this.enabled = false;
        FindObjectOfType<AudioManager>().Play("Die");
    }
}
