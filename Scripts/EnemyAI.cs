using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public Animator anim;



    public float health;

    //Patroling
    public Vector3 walkPoint; //����� ������
    bool walkPointSet; //����������� �� ����� ������
    public float walkPointRange; //���������� ���������� ����� ������

    //Attacking


    //States
    public float sightRange; //��������� ���������, ��������� ��� ������
    public bool playerInSightRange; //��������� �� ����� � �������� ������������
    public bool playerStopDistance; //��������� ����� � ���� ��������� �����
    public float stop;


    private void Awake()//����� �� ������������� ���������� � ������
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();

    }
    private void Update()
    {
        //��������� �� ����� � ���������� ��� �� ���������� �����
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);//������� ����� (���� ������� ��� �����, ��������� ������ ��� ���������, �� ��� ����� � �������� ����� ����)
        playerStopDistance = Physics.CheckSphere(transform.position, stop, whatIsPlayer);

        if (!playerInSightRange && !playerStopDistance) Patroling(); //���� ����� �� ��������� � ���� ������ ��������� ��� � ���� ������ ����� ���������: ���������.
        if (playerInSightRange && !playerStopDistance) ChasePlayer(); //���� ����� ��������� � �������� ���������, �� �� � �������� ������������ �����: ������������ ������.
                                                                      //���� ����� ��������� � �������� ��������� � � �������� �����: ���������.
        if (playerInSightRange && playerStopDistance) Stoping();



    }
    private void Stoping()
    {
        agent.SetDestination(transform.position);
        Vector3 direction = player.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        int combatEnemy = UnityEngine.Random.Range(0, 2);
        {
            anim.SetBool("Attack", true);
        }



    }


    private void Patroling()//���������
    {
        anim.SetBool("isRunning", false);
        anim.SetBool("Attack", false);
        if (!walkPointSet) SearchWalkPoint();//���� ����� ������ �� ������, ����� ����� ������.

        if (walkPointSet)//���� ����� ������� ��� ������ �����������. ����� �������� ����� �������(����������)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;//��������� ��������� �� ����� ����� ������� ��� ������

        if (distanceToWalkPoint.magnitude < 1f)//���� ���������� ������ 1, �� �� ��������� ����� ������ � ������������ ����� ������ � ������������ ����� ������ ����� ������ ���� ������
        {
            walkPointSet = false;//����� ����� ��� ������������� ������ ������ ����� �����.
        }
    }
    private void SearchWalkPoint()
    {
        //��������� �������� ����� � ���������
        float randomZ = Random.Range(-walkPointRange, walkPointRange); //��������Z ��������� ��������(������������� ����� �����������, ������������� ����� �����������)������ ��������� �������� � ����������� �� ���� ��������� ����� �������� ����� ����� ������.
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ); //���������� ����� �������� � ����� ������3(���� ������� � + ��������� �������� �, ���� ������� y, ���� ������� z + ��������� �������� z)

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))//����� ��������� ��� ��� ����� �� �� ��������� �����: � ������� ���� �������� ������������� �� ��� ����� ��������� �� �����
        {
            walkPointSet = true; //���� ��� ��� ����������� ����� ����� ��� ������ � ���.
        }
    }

    private void ChasePlayer()//������������� ������
    {
        agent.SetDestination(player.position);//����� �������� ����� ����������(������� ������)
        anim.SetBool("isRunning", true);
        anim.SetBool("Attack", false);
    }
    private void OnDrawGizmosSelected()//��� ������������ ��������� ����� � ������
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stop);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);

    }


}




