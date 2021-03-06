﻿using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Agent;
using MLAgents;
using UnityEngine;

internal class WalkingBWAgent : Agent
{
    private ApplicationSettings applicationSettings;
    private IActions actions;
    
    private Observations observations;
    private Humanoid2DResetPos resetPos;
    private WalkBWReward rewards;
    
    public int steps = 0;
    public int maxSteps = 100;
    
    public bool ready = true;
    private float rewardAnim = 0f;
    public float sumRewards = 0f;

    public override void InitializeAgent()
    {
        observations = GetComponent<Observations>();
        rewards = GetComponent<WalkBWReward>();
        actions = GetComponent<Humanoid2DActionsAngPos>();
        resetPos = GetComponent<Humanoid2DResetPos>();
        observations.addToRemove(new[]{"root_pos_x"});
        
    }

    public override void CollectObservations()
    {

        List<float> observations = this.observations.getObservations();

        foreach (var observation in observations) AddVectorObs(observation);
    }

    protected override void MakeRequests(int academyStepCounter)
    {
        agentParameters.numberOfActionsBetweenDecisions =
            Mathf.Max(agentParameters.numberOfActionsBetweenDecisions, 1);
        if (!agentParameters.onDemandDecision)
        {
            RequestAction();
            if (academyStepCounter % agentParameters.numberOfActionsBetweenDecisions == 0)
            {
                RequestDecision();

            }
        }
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {

        steps++;
        List<float> actions = vectorAction.ToList();
        List<float> actionsClamped = new List<float>();
        foreach (var var in actions)
            actionsClamped.Add(Mathf.Clamp(var, -1f, 1f));

        
        this.actions.applyActions(actionsClamped);
        rewardAnim = rewards.getReward();

        //bool terminateAgent = terminateFn.isTerminated();

        //sumRewards += rewardAnim;
        SetReward(rewardAnim);

        if (steps > maxSteps)
        {
            //sumRewards = sumRewards > 0 ? sumRewards : 0;
            SetReward(rewardAnim);
            //sumRewards = 0f;
            steps = 0;
            resetPos.ResetPosition();
            Done();
        }
    }

    public override void AgentReset()
    {
        
    }


   
}