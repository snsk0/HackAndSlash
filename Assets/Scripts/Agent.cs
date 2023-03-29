using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Runtime.Enemy.Component;

public class Agent : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform target;
    [SerializeField] private LineRenderer lrenderer;
    [SerializeField] private NavMeshObstacle obstacle;
    [SerializeField] private EnemyMove move;
    [SerializeField] private EnemyLook look;
    private bool ismove = true;


    private void Start()
    {
        agent.updatePosition = false;
        agent.updateRotation = false;
        agent.SetDestination(target.position);
    }
    private void Update()
    {
        if (agent.pathPending == true) return;

        agent.SetDestination(target.position);

        //agent���W����苗���ɕ␳����(���ɕۂƌ������Ɉړ����Ȃ�)
        //�������� agent�Ƃ̋��������ɂ���(maxAgentDistance�ɌŒ肷�邩�ǂ���
        Vector3 direction = agent.nextPosition - transform.position;
        direction = direction.normalized * 1.0f;
        agent.nextPosition = transform.position + direction;


        //�����̔���(agent)�ƁA�Ǐ]�����̔���
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.enabled = false;
            ismove = false;
            obstacle.enabled = true;
            this.enabled = false;
        }
        else
        {
            //�ړ��Ɖ�]����
            lrenderer.SetPositions(agent.path.corners);
            move.MoveByWorldDir(agent.nextPosition - transform.position);
        }
    }
}
