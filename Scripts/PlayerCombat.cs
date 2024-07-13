using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;

    public Transform attackPoint;
    public float attackRange = 0.5f; //�������� ��� �����
    public LayerMask enemyLayers; //���� ��� ����������� �����

    public int attackDamage = 20;

    public float attackRate = 2f;
    float nextAttackTime = 0f;

    void Update()
    {
        if (Time.time >= nextAttackTime) //���� ������� ����� ������ ��� ����� ������� �����
        {
            if (Input.GetKeyDown(KeyCode.Space) && !Players.died)//��������� ����� �� ����� ���� ��� �����
            {
                Attack();

                nextAttackTime = Time.time + 4f / attackRate; //�������� 0.5 � ������ �������� ������� � ������� ��� ��� ��������� ��� ����� �� ����� ��������� � ����� �� ��������� ����� ������� �� ������ ��������� �����
            }
        }


    }

    void Attack()
    {
        //������������� �������� �����
        animator.SetTrigger("Attack");




        //���������� ���� ������ ������� ��������� � �������� ������������ �����
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers); //������� ������ ������� ���������� �� ������� ����� � �������� ���� �������� � �������� ��� ������� �� ������� �������� ���� ����. �������� � ������ �����������

        //������� ���� ������ ����
        foreach (Collider enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().Damage(attackDamage);  //�������� ������ � ������� Enemy �� �������, � ����� ������ ������� ��������� �����
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)//���� ����� ����� ����� ����, ���������
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
