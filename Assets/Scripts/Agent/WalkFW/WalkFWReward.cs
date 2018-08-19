﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class WalkFWReward : MonoBehaviour
{

    public bool debug = false;
    private StandingRewardHumanoid standingReward;
    private VelocityReward velocityReward;
    private BodyParts bodyParts;
    public float reward;
    public float standingRewardVal;
    public float velocityRewardVal;
    
    

    private void Start()
    {
        bodyParts = GetComponent<BodyParts>();
        standingReward = new StandingRewardHumanoid(bodyParts);
        velocityReward = new VelocityReward(Vector2.right, 10f, bodyParts.root);

        standingReward.Init();

        if (debug)
            InvokeRepeating("getReward", 0.0f, .1f);
    }

    public float getReward()
    {
        standingRewardVal = standingReward.getReward();
        velocityRewardVal = velocityReward.getReward() * 3;
        reward = (standingRewardVal + velocityRewardVal) / 4;
        
        return reward;
    }
}
