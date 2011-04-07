using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// An AI incorporates an algorithm and memory. It may also store data about how the algorithm learns
class AI
{
    public AI()
    {
#if false
        // First check if there is an incoming projectile
        AICollisionDecisionNode dodgeNode1 = new AICollisionDecisionNode(3, 1, 0, 0);
        AICollisionDecisionNode dodgeNode2 = new AICollisionDecisionNode(3, 0, 1, 0);
        dodgeNode1.setLeftChild(dodgeNode2);
        // If there is an incoming projectile, then jump
        AIJumpNode jumpNode = new AIJumpNode();
        //dodgeNode1.setLeftChild(jumpNode);
        dodgeNode2.setLeftChild(jumpNode);
        this.firstNeuron = dodgeNode1;
#else
        double[] v;
        AIFireNode fireNode1 = new AIFireNode(false);
        // check whether the current weapon has any ammo
        AIAmmoDecisionNode ammoNode1 = new AIAmmoDecisionNode();
        fireNode1.setNextNode(ammoNode1);
            // if you do have ammo, check whether we can save some cooldown time by switching weapons now
            AIFreeSwitchDecisionNode switchNode1 = new AIFreeSwitchDecisionNode();
            ammoNode1.setLeftChild(switchNode1);
        // if either event happens, then switch weapons
        AISelectWeaponNode selectWeaponNode2 = new AISelectWeaponNode();
        ammoNode1.setRightChild(selectWeaponNode2);
        switchNode1.setLeftChild(selectWeaponNode2);
        // else, if we don't want to switch
            // decide whether it's time to do another collision test
            AITickDecisionNode scanNode1 = new AITickDecisionNode(.1);
            switchNode1.setRightChild(scanNode1);
                // check whether the target is in range and shoot if it is
                AITargetDecisionNode targetNode1 = new AITargetDecisionNode(0, 0, 0);
                scanNode1.setLeftChild(targetNode1);
                    // if we're in range, then fire
                    AIFireNode fireNode2 = new AIFireNode(true);
                    targetNode1.setLeftChild(fireNode2);
                // else
                    // if we're not in range, then move closer
                    AITargetDecisionNode leftTargetNode = new AITargetDecisionNode(-1, 0, 10000);
                    targetNode1.setRightChild(leftTargetNode);
                        // if the user is on the left, go left
                        v = new double[2]; v[0] = -10000; v[1] = 0;
                        AISetVelocityNode goLeftNode1 = new AISetVelocityNode(v);
                        leftTargetNode.setLeftChild(goLeftNode1);
                    // else
                        // if the user is on the left, go right
                        v = new double[2]; v[0] = 10000; v[1] = 0;
                        AISetVelocityNode goRightNode1 = new AISetVelocityNode(v);
                        leftTargetNode.setRightChild(goRightNode1);
                    // since we're not in range, switch to a new weapon if it's been out of range for a while
                    AITickDecisionNode weaponSwitchCounter = new AITickDecisionNode(1);
                    goLeftNode1.setNextNode(weaponSwitchCounter);
                    goRightNode1.setNextNode(weaponSwitchCounter);
                    AISelectWeaponNode selectWeaponNode1 = new AISelectWeaponNode();
                    weaponSwitchCounter.setLeftChild(selectWeaponNode1);
        // Now check if there is an incoming projectile
        AICollisionDecisionNode dodgeNode1 = new AICollisionDecisionNode(3, 1, 0, 0.3);
        selectWeaponNode2.setNextNode(dodgeNode1);
        fireNode2.setNextNode(dodgeNode1);
        scanNode1.setRightChild(dodgeNode1);
        selectWeaponNode1.setNextNode(dodgeNode1);
            // if the projectile is close in time, check whether it will miss laterally
            AICollisionDecisionNode dodgeNode2 = new AICollisionDecisionNode(3, 0, 1, 0);
            dodgeNode1.setLeftChild(dodgeNode2);
            // If there is an incoming projectile, then dodge
            {
                AIPositionDecisionNode dodgeNode3 = new AIPositionDecisionNode(3, 0, 1, 20);
                dodgeNode2.setLeftChild(dodgeNode3);
                // If the projectile isn't too high, then jump and go past it
                {
                    // Jump
                    AIJumpNode jumpDodgeNode = new AIJumpNode();
                    dodgeNode3.setLeftChild(jumpDodgeNode);
                }
                // If the projectile is somewhat close run away from it
                {
                    AIPositionDecisionNode dodgeNode4 = new AIPositionDecisionNode(3, 1, 0, 0);
                    dodgeNode3.setRightChild(dodgeNode4);
                    // If it's on the left, go right
                    {
                        v = new double[2]; v[0] = 10000; v[1] = 0;
                        AISetVelocityNode dodgeLeftNode = new AISetVelocityNode(v);
                        dodgeNode4.setLeftChild(dodgeLeftNode);
                    }
                    // If it's on the right, go left
                    {
                        v = new double[2]; v[0] = -10000; v[1] = 0;
                        AISetVelocityNode dodgeRightNode = new AISetVelocityNode(v);
                        dodgeNode4.setRightChild(dodgeRightNode);
                    }
                }
            }
        // else
            // If there is no incoming projectile, then chase the opponent
            // If we're about to bump into a wall, jump
            AICollisionDecisionNode obstacleDecisionNode1 = new AICollisionDecisionNode(1, 1, 0, 0.01);
            dodgeNode1.setRightChild(obstacleDecisionNode1);
            dodgeNode2.setRightChild(obstacleDecisionNode1);
                AICollisionDecisionNode obstacleDecisionNode2 = new AICollisionDecisionNode(1, 0, 1, 0.01);
                obstacleDecisionNode1.setLeftChild(obstacleDecisionNode2);
                    AIPositionDecisionNode obstacleDecisionNode3 = new AIPositionDecisionNode(1, 0, -1, 40);
                    obstacleDecisionNode2.setLeftChild(obstacleDecisionNode3);
                        AIJumpNode wallJumpNode = new AIJumpNode();
                        obstacleDecisionNode3.setLeftChild(wallJumpNode);
            // If jumping would help hit the target, then jump
            AIPositionDecisionNode jumpCheckNode1 = new AIPositionDecisionNode(2, 0, 1, 250);
            obstacleDecisionNode1.setRightChild(jumpCheckNode1);
            obstacleDecisionNode2.setRightChild(jumpCheckNode1);
            obstacleDecisionNode3.setRightChild(jumpCheckNode1);
                AITickDecisionNode jumpCheckNode2 = new AITickDecisionNode(0.01);
                jumpCheckNode1.setLeftChild(jumpCheckNode2);
                    AITargetDecisionNode jumpCheckNode3 = new AITargetDecisionNode(0, 1, 10000);
                    jumpCheckNode2.setLeftChild(jumpCheckNode3);
                        AIJumpNode targetJumpNode = new AIJumpNode();
                        jumpCheckNode3.setLeftChild(targetJumpNode);

        /* // If there is no incoming projectile, then chase the user
        {
            AIPositionDecisionNode chaseNode = new AIPositionDecisionNode(2, 1, 0, 200);
            dodgeNode1.setRightChild(chaseNode);
            dodgeNode2.setRightChild(chaseNode);
            // if the user is on the left, go left
            v = new double[2]; v[0] = -100; v[1] = 0;
            AISetVelocityNode goLeftNode = new AISetVelocityNode(v);
            chaseNode.setLeftChild(goLeftNode);
            // if the user is on the right, go right
            v = new double[2]; v[0] = 100; v[1] = 0;
            AISetVelocityNode goRightNode = new AISetVelocityNode(v);
            chaseNode.setRightChild(goRightNode);
            // If we're about to bump into a wall, jump
            AICollisionDecisionNode obstacleDecisionNode1 = new AICollisionDecisionNode(1, 1, 0, 0.01);
            goLeftNode.setNextNode(obstacleDecisionNode1);
            goRightNode.setNextNode(obstacleDecisionNode1);
            AIPositionDecisionNode obstacleDecisionNode2 = new AIPositionDecisionNode(1, 0, -1, 40);
            obstacleDecisionNode1.setLeftChild(obstacleDecisionNode2);
            AIJumpNode wallJumpNode = new AIJumpNode();
            obstacleDecisionNode2.setLeftChild(wallJumpNode);
        }*/

        // remember the decision tree
        this.firstNeuron = fireNode1;
        fireNode1.setBrain(this);
        //this.firstNeuron = fireNode1;
#endif

    }
    public void think(Character body)
    {
        // clear the flags of what is nearby so that the information we get will still be current
        body.invalidateNeighborData();
        AINode currentNode = this.firstNeuron;
        this.shotError = null;
        while (currentNode != null)
        {
            currentNode.execute(body);
            currentNode = currentNode.getNextNode(body);
        }
        this.shotError = null;
    }
    public void reinforce(double quantity)
    {
        //this.firstNeuron.reinforce(quantity);
    }
    public void adjustBehavior()
    {
        //this.firstNeuron.adjustBehavior();
    }
    public void setState(int newState)
    {
        this.state = newState;
    }
    public int getState()
    {
        return this.state;
    }
    // gives a vector telling by how much the shot is expected to miss the opponent
    public double[] getShotError(Character body, GameObject target)
    {
        if (shotError == null)
            shotError = body.getCurrentWeapon().simulateShooting(target);
        return shotError;
    }
// private
    AINode firstNeuron;
    int state;
    double[] shotError;
}

// The AINode class is like a line of code.
class AINode
{
    public AINode()
    {
        this.currentLag = 100;
        this.deltaLag = 10;
        this.lagAmplitude = 10;
        this.averageLag = 100;
        //this.deltaThreshold = 1;
    }
    // run this line of code
    public virtual void execute(Character body)
    {
    }
    // figure out which line of code gets run next
    public virtual AINode getNextNode(Character body)
    {
        return null;
    }
    public virtual void setBrain(AI newBrain)
    {
        this.brain = newBrain;
    }
    public AI getBrain()
    {
        return this.brain;
    }
    // Praise/scolding, for the purpose of learning. Good things are positive numbers and bad things are negative numbers
    public virtual void reinforce(double quantity)
    {
        this.remainingReinforcement += quantity;
    }
    // We need to estimate the lag applied to reinforcement, in addition to the correct threshold value to use
    // So, when things are going badly, increase the amplitude of the lag estimate, and adjust the lag estimate
    // When things are going well, decrease the amplitude of the lag estimate and move the average lag estimate closer accordingly
    // Always adjust the threshold based on a running average of the reinforcement (using the lag to normalize the time)

    // adjust behavior based on previous reinforcement
    public virtual void adjustBehavior()
    {
        if (this.remainingReinforcement > 0)
        {
            // If we're improving, then we probably have the lag calculated about correctly
            // So, stop adjusting it as much
            double lagFraction = (this.currentLag - this.averageLag) / this.lagAmplitude;
            //double amplitudeDecrease = this.remainingReinforcement - this.averageReinforcement;
            this.lagAmplitude = lagAmplitude * averageLag / (averageLag + 1);
            this.averageLag = this.currentLag - this.lagAmplitude * lagFraction;
        }
        if (this.remainingReinforcement < 0)
        {
            // If we get here then we're not improving, so maybe the lag isn't being calculated correctly
            // So, adjust it some more. Lag is measured in number of timer ticks
            if (this.currentLag + deltaLag >= 1)
            {
                this.currentLag += deltaLag;
                if (Math.Abs(currentLag - averageLag) >= lagAmplitude)
                {
                    // If we've adjusted the lag pretty far already, then turn around. Also go more slowly to help convergence
                    this.deltaLag *= -.5;
                    // Increase the amplitude so that we are guaranteed to reach it eventually
                    this.lagAmplitude *= (1 - remainingReinforcement);
                }
            }
            else
            {
                // There can't be negative lag. Set this as the bottom of the oscillation
                this.averageLag = this.currentLag + this.lagAmplitude;
                // turn around and keep going
                this.deltaLag *= -.5;
            }
        }
        // Now update the moving average
        this.averageReinforcement = (this.remainingReinforcement + this.currentLag * this.averageReinforcement) / (this.currentLag + 1);
        /*double threshold = this.getThreshold();
        if (this.remainingReinforcement > this.previousReinforcement)
        {
            // If you're improving, then keep adjusting the guess more quickly
            this.deltaThreshold *= 2;
        }
        else
        {
            // If you're getting worse, then turn around and slow down
            this.deltaThreshold /= (double)-12;
            double estimate = this.estimateGoodThreshold();
            // Now, if you're moving toward the estimate, then move more quickly
            if (((estimate - threshold) > 0) == (deltaThreshold > 0))
            {
                this.deltaThreshold *= 2;
            }
        }
        // update previous values
        this.previousThreshold = threshold;
        this.previousReinforcement = this.remainingReinforcement;
        this.remainingReinforcement = 0;
        // now adjust finally
        this.adjustThreshold(this.deltaThreshold);*/
        this.remainingReinforcement = 0;
    }
    // tells how much reinforcement there has been since the last time it was reset
    public double getRemainingReinforcement()
    {
        return this.remainingReinforcement;
    }
    public double getEstimatedReinforcementLag()
    {
        // This next line is cheating for the purpose of testing
        //return 100;
        return this.currentLag;
    }
    public double getAverageReinforcement()
    {
        return this.averageReinforcement;
    }
    /*public virtual void adjustThreshold(double quantity)
    {
    }
    public virtual double estimateGoodThreshold()
    {
        return 0;
    }
    public virtual double getThreshold()
    {
        return 0;
    }*/
// private
    AI brain;
    double remainingReinforcement;

    double averageLag;
    double lagAmplitude;
    double currentLag;
    double averageReinforcement;
    double deltaLag;

    //double estimatedReinforcementLag;
    //double previousReinforcement;
    //double deltaThreshold;
    //double previousThreshold;
}

///////////////////////////////////////////////////////////////////////// Decision Nodes ///////////////////////////////////////////////////////////////////////////
// An AIDecisionNode is like an IF statement
class AIDecisionNode : AINode
{
    public AIDecisionNode()
    {
        //this.leftBranchTime = this.rightBranchTime = 1;
        //this.timeSinceLeftBranch = this.timeSinceRightBranch = 1;
        this.thresholdCertainty = 100;
    }
    public override AINode getNextNode(Character body)
    {
        return base.getNextNode(body);
    }
    public override void setBrain(AI newBrain)
    {
        base.setBrain(newBrain);
        if (this.leftChild != null)
            this.leftChild.setBrain(newBrain);
        if (this.rightChild != null)
            this.rightChild.setBrain(newBrain);
    }
    public AINode chooseLeftChild()
    {
        // keep track of the fact that we chose this child
        /*this.leftBranchTime += 1;
        this.timeSinceLeftBranch = 1;
        this.timeSinceRightBranch += 1;*/
        double shortLag = this.getEstimatedReinforcementLag();
        this.recentLeftBranchFraction = (this.recentLeftBranchFraction * shortLag + 1) / (shortLag + 1);
        double longLag = shortLag * shortLag;
        this.longLeftBranchFraction = (this.longLeftBranchFraction * longLag + 1) / (longLag + 1);
        // now return the appropriate one
        return this.leftChild;
    }
    public AINode chooseRightChild()
    {
        // keep track of the fact that we chose this child
        /*this.rightBranchTime += 1;
        this.timeSinceRightBranch = 1;
        this.timeSinceLeftBranch += 1;*/
        double shortLag = this.getEstimatedReinforcementLag();
        this.recentLeftBranchFraction = (this.recentLeftBranchFraction * shortLag + 0) / (shortLag + 1);
        double longLag = shortLag * shortLag;
        this.longLeftBranchFraction = (this.longLeftBranchFraction * longLag + 0) / (longLag + 1);
        // now return the appropriate one
        return this.rightChild;
    }
    public virtual void setLeftChild(AINode newChild)
    {
        this.leftChild = newChild;
    }
    public virtual void setRightChild(AINode newChild)
    {
        this.rightChild = newChild;
    }
    // Here is reinforcement for the purpose of learning. Quantity can be positive or negative
    public override void reinforce(double quantity)
    {
        //if (quantity < 0)
        //    quantity = quantity;
        // figure out how much importance to give to each branch
        double leftFraction = this.getLeftImportance();
        // figure out how quickly to decay toward the current value
        double lag = this.getEstimatedReinforcementLag();
        // reinforce left
        //double leftQuantity = leftFraction * quantity;
        //if (leftQuantity > 0)
            this.leftReinforcement = (this.leftReinforcement * lag + leftFraction * quantity) / (lag + leftFraction);
        //else
        //    this.leftPunishment += leftQuantity;
        if (leftChild != null)
            leftChild.reinforce(leftFraction * quantity);
        // reinforce right
        double rightFraction = 1 - leftFraction;
        //double rightQuantity = (1 - leftFraction) * quantity;
        //if (rightQuantity > 0)
            this.rightReinforcement = (this.rightReinforcement * lag + rightFraction * quantity) / (lag + rightFraction);
        //else
        //    this.rightPunishment += rightQuantity;
        if (rightChild != null)
            rightChild.reinforce(rightFraction * quantity);
        // do any other bookkeeping
        base.reinforce(quantity);
    }
    // returns a fraction (0 - 1) indicating how much importance to attribute to the left branch recently
    double getLeftImportance()
    {
        return this.recentLeftBranchFraction;
    }
    /*double getLeftImportance()
    {
        // figure out the overall fraction of times we've taken the left branch
        double leftFraction = leftBranchTime / (leftBranchTime + rightBranchTime);
        double rightFraction = 1 - leftFraction;
        // figure out the overall fraction of times we've taken the left branch since the last time we switched
        double recentLeftFraction = timeSinceRightBranch / (timeSinceLeftBranch + timeSinceRightBranch);
        double recentRightFraction = 1 - recentLeftFraction;

        double leftWeight = recentLeftFraction * recentLeftFraction / leftFraction;
        double rightWeight = recentRightFraction * recentRightFraction / rightFraction;
        double result = leftWeight / (leftWeight + rightWeight);
        if ((!(result <= 0)) && (!(result >= 0)))
        {
            result = result;
        }
        return result;
    }*/
    public override void adjustBehavior()
    {
        double average = this.getAverageReinforcement();
        // figure out how much better one side has been doing than the other
        double difference = this.leftReinforcement - this.rightReinforcement;
        // If we keep doing well, then adjust more slowly. If we're doing badly, then adjust more quickly
        // Make sure not to ever have division by zero
        if (this.thresholdCertainty + average > 0)
            this.thresholdCertainty += average;
        this.adjustThreshold(difference / Math.Sqrt(thresholdCertainty));
        /* // If we took each branch at least once, then move closer to optimal using the previous data
        double improvement = (leftReinforcement + rightReinforcement) - (oldLeftReinforcement + oldRightReinforcement);
        if (improvement > 0)
        {
            // If we're doing better, then keep moving in the same direction, and move more quickly
            this.deltaThreshold *= 1.1;
        }
        else
        {
            if (improvement < 0)
            {
                // If we're doing worse, then turn around and move more slowly
                this.deltaThreshold *= 0.7;
            }
            else
            {
                // if there is no change, then move to choose the more often successful side
                if (leftReinforcement > rightReinforcement)
                    deltaThreshold = Math.Abs(deltaThreshold);
                else
                    deltaThreshold = -Math.Abs(deltaThreshold);
                // Now move more quickly
                deltaThreshold *= 1.1;
            }
        }
        // now finally adjust the threshold
        this.adjustThreshold(deltaThreshold);
        */
        // reset reinforcement counters
        //this.oldLeftReinforcement = this.leftReinforcement;
        //this.oldRightReinforcement = this.rightReinforcement;
        //this.leftReinforcement = this.rightReinforcement = 0;
        /*this.oldReinforcementTime = this.reinforcementTime;
        this.reinforcementTime = 0;*/
        // do any other bookkeeping
        base.adjustBehavior();
        // tell the children to learn too
        if (leftChild != null)
            leftChild.adjustBehavior();
        if (rightChild != null)
            rightChild.adjustBehavior();
    }
    public virtual void adjustThreshold(double delta)
    {
    }
// private
    AINode leftChild;
    AINode rightChild;
    double recentLeftBranchFraction;
    double longLeftBranchFraction;
    /*double timeSinceLeftBranch;
    double timeSinceRightBranch;
    double leftBranchTime;
    double rightBranchTime;*/
    double leftReinforcement, rightReinforcement;//, oldLeftReinforcement, oldRightReinforcement;
    //double deltaThreshold;
    //double reinforcementTime, oldReinforcementTime;
    double thresholdCertainty;

}
// An AIStateDecisionNode is an IF statement where the branch condition is based on internal state
class AIStateDecisionNode : AIDecisionNode
{
    // constructor
    public AIStateDecisionNode(int comparisonThreshold)
    {
        this.threshold = comparisonThreshold;
    }
    // return the next node to execute
    public override AINode getNextNode(Character body)
    {
        if (this.getBrain().getState() <= this.threshold)
        {
            return this.chooseLeftChild();
        }
        else
        {
            return this.chooseRightChild();
        }
    }
// private
    int threshold;
}
// An AIGeometryDecisionNode is an IF statement where the branch condition is based on object locations
class AIGeometryDecisionNode : AIDecisionNode
{
    public GameObject getNeighborOfType(Character body, int type)
    {
        GameObject other;
        switch (type)
        {
            case 0:
                other = body.getMyGround();
                break;
            case 1:
                other = body.getNearestObstacle();
                break;
            case 2:
                other = body.getNearestEnemyCharacter();
                break;
            case 3:
                other = body.getNearestEnemyProjectile();
                break;
            default:
                other = null;
                System.Diagnostics.Trace.WriteLine("Invalid AIGeometryDecisionNode type");
                break;
        }
        return other;
    }
}
// An AICollisionDecisionNode is an IF statement that checks whether the object will collide with this one any time soon
// As forwardScale increases, the time until collision matters a lot
// As lateralScale increases, it becomes less acceptable for the two objects to miss each other by a larger amount
class AICollisionDecisionNode : AIGeometryDecisionNode
{
    public AICollisionDecisionNode(int typeToCheck, double forwardScale, double lateralScale, double threshold)
    {
        this.itsTypeToCheck = typeToCheck;
        this.itsForwardScale = forwardScale;
        this.itsLateralScale = lateralScale;
        this.itsThreshold = threshold;
    }
    public override AINode getNextNode(Character body)
    {
        GameObject other = this.getNeighborOfType(body, this.itsTypeToCheck);
        double forwardDist, lateralDist, speed;
        if (other == null)
        {
            return base.chooseRightChild();
            //forwardDist = lateralDist = 0;
        }
        else
        {
            // This is an approximation based on the assumption that both body and other are circles.
            // Eventually I'll make it better


            // calculate relative positions
            double[] myPosition = body.getCenter();
            double[] theirPosition = other.getCenter();
            double dx = theirPosition[0] - myPosition[0];
            double dy = theirPosition[1] - myPosition[1];
            double dist = Math.Sqrt(dx * dx + dy * dy);

            // calculate relative velocity
            double[] myVelocity = body.getVelocity();
            double[] theirVelocity = other.getVelocity();            
            double vx = myVelocity[0] - theirVelocity[0];
            double vy = myVelocity[1] - theirVelocity[1];
            speed = Math.Sqrt(vx * vx + vy * vy);

            // calculate sizes. If the shapes aren't actually circles, then we kind of make something up for the moment
            GameShape myShape = body.getShape();
            double myRadius = Math.Max(myShape.getWidth() / 2, myShape.getHeight() / 2);
            GameShape theirShape = other.getShape();
            double theirRadius = Math.Max(theirShape.getWidth() / 2, theirShape.getHeight() / 2);


            // Compute the point at which the objects will be closest to each other
            if (vx == 0 && vy == 0)
            {
                forwardDist = dist - myRadius - theirRadius;
                lateralDist = dist - myRadius - theirRadius;
            }
            else
            {
                forwardDist = (dx * vx + dy * vy) / speed - myRadius - theirRadius;
                lateralDist = Math.Abs((dx * vy - dy * vx) / speed) - myRadius - theirRadius;
            }
        }
        // At faster speeds it's easier for there to be a collision, so scale the threshold by the speed (Therefore, the threshold represents a number of seconds)
        if (forwardDist * this.itsForwardScale + lateralDist * this.itsLateralScale < itsThreshold * speed)
        {
            return base.chooseLeftChild();
        }
        else
        {
            return base.chooseRightChild();
        }
    }
    public override void adjustThreshold(double delta)
    {
        if (delta != 0)
            this.itsThreshold += delta;
        base.adjustThreshold(delta);
    }
// private
    int itsTypeToCheck;
    double itsForwardScale;
    double itsLateralScale;
    double itsThreshold;
}
// An AIPositionDecisoinNode is an IF statement that checks which side of this object the given object is
class AIPositionDecisionNode : AIGeometryDecisionNode
{
    public AIPositionDecisionNode(int typeToCheck, double scaleX, double scaleY, double threshold)
    {
        this.itsTypeToCheck = typeToCheck;
        this.itsScaleX = scaleX ;
        this.itsScaleY = scaleY;
        this.itsThreshold = threshold;
    }
    public override AINode getNextNode(Character body)
    {
        // find the other object to compare with
        GameObject other = this.getNeighborOfType(body, this.itsTypeToCheck);
        if (other == null)
        {
            return this.chooseRightChild();
        }
        else
        {
            // Calculate the directional distance
            double[] theirPosition = other.getCenter();
            double[] myPosition = body.getCenter();
            double dx = theirPosition[0] - myPosition[0];
            double dy = theirPosition[1] - myPosition[1];
            if (dx * this.itsScaleX + dy * itsScaleY < this.itsThreshold)
            {
                return this.chooseLeftChild();
            }
            else
            {
                return this.chooseRightChild();
            }
        }
    }
    public override void adjustThreshold(double delta)
    {
        this.itsThreshold += delta;
        base.adjustThreshold(delta);
    }
// private
    int itsTypeToCheck;
    double itsScaleX;
    double itsScaleY;
    double itsThreshold;
}
// An AITargetDecisionNode decides whether the target is in range for the current weapon
class AITargetDecisionNode : AIGeometryDecisionNode
{
// public
    public AITargetDecisionNode(double scaleX, double scaleY, double minDist)
    {
        // if the distance to the target is no more than minDist and
        // a move of scaleX, scaleY would get it closer to being in range, then return the left child
        // otherwise, return the right child
        this.itsScaleX = scaleX;
        this.itsScaleY = scaleY;
        this.itsThreshold = minDist;

    }
    public override AINode getNextNode(Character body)
    {
        // compute the expected minimum offset (x,y) of the projectile compared to the target
        GameObject target = body.getNearestEnemyCharacter();
        double[] offset = this.getBrain().getShotError(body, target);
        double dist = Math.Sqrt(offset[0] * offset[0] + offset[1] * offset[1]);
        // make sure that it's in range and that the move being considered will move it closer to in range
        if ((dist <= this.itsThreshold) && (this.itsScaleX * offset[0] + this.itsScaleY * offset[1] >= 0))
        {
            return this.chooseLeftChild();
        }
        else
        {
            return this.chooseRightChild();
        }
    }
// private
    double itsScaleX, itsScaleY, itsThreshold;
}
// An AIAmmoDecision node chooses the left child if we have ammo left
class AIAmmoDecisionNode : AIDecisionNode
{
    public override AINode getNextNode(Character body)
    {
        Weapon currentWeapon = body.getCurrentWeapon();
        if (currentWeapon != null)
        {
            if (body.getCurrentWeapon().getCurrentAmmo() >= 1)
                return this.chooseLeftChild();
            else
                return this.chooseRightChild();
        }
        return this.chooseRightChild();
    }
}
// An AIFreeSwitchDecisionNode chooses the left child if we're firing the current weapon but it will still fire even if we switch weapons
class AIFreeSwitchDecisionNode : AIDecisionNode
{
    public override AINode getNextNode(Character body)
    {
        Weapon currentWeapon = body.getCurrentWeapon();
        if (currentWeapon != null)
        {
            if (currentWeapon.isWarmingUp() && currentWeapon.canFireWhileInactive())
                return this.chooseLeftChild();
        }
        return this.chooseRightChild();
    }
}
// An AITickDecisionNode chooses a child based on counting timer tick
class AITickDecisionNode : AIDecisionNode
{
// public 
    public AITickDecisionNode(double leftFraction)
    {
        this.remainingLeftCounts = 0;
        this.itsLeftFraction = leftFraction;
    }
    public override AINode getNextNode(Character body)
    {
        this.remainingLeftCounts += this.itsLeftFraction;
        if (this.remainingLeftCounts > 0.5)
        {
            this.remainingLeftCounts -= 1;
            return this.chooseLeftChild();
        }
        else
        {
            return this.chooseRightChild();
        }
    }

// private
    double remainingLeftCounts;
    double itsLeftFraction;
};
////////////////////////////////////////////////////////////////////////// Action Nodes ////////////////////////////////////////////////////////////////////////////
// An AIActionNode class is like an assignment statement
class AIActionNode : AINode
{
    public override void execute(Character body)
    {
        // run the next line of code
        base.execute(body);
    }
    public override AINode getNextNode(Character body)
    {
        return this.nextNode;
    }
    public void setNextNode(AINode nextNode)
    {
        this.nextNode = nextNode;
    }
    public override void setBrain(AI newBrain)
    {
        base.setBrain(newBrain);
        if (this.nextNode != null)
            this.nextNode.setBrain(newBrain);
    }
    public override void reinforce(double quantity)
    {
        if (nextNode != null)
            this.nextNode.reinforce(quantity);
        base.reinforce(quantity);
    }
    public override void adjustBehavior()
    {
        base.adjustBehavior();
        if (this.nextNode != null)
            this.nextNode.adjustBehavior();
    }
// private
    AINode nextNode;
}
// An AIStateAssignmentNode sets the internal state to a certain value
class AIStateAssignmentNode : AIActionNode
{
    public AIStateAssignmentNode(int newState)
    {
        this.stateToSet = newState;
    }
    public override void execute(Character body)
    {
        // change the given state
        this.getBrain().setState(this.stateToSet);
        // run the next line of code
        base.execute(body);
    }
    // private
    int stateToSet;
}
// An AISetVelocityNode sets the desired velocity of the character
class AISetVelocityNode : AIActionNode
{
// public
    public AISetVelocityNode(double[] targetVelocity)
    {
        this.desiredVelocity = targetVelocity;
    }
    public override void execute(Character body)
    {
        body.setTargetVelocity(this.desiredVelocity);
        // run the next line of code
        base.execute(body);
    }
    // private
    double[] desiredVelocity;
}
// An AIJumpNode will make the body jump
class AIJumpNode : AIActionNode
{
// public
    public AIJumpNode()
    {
    }
    public override void execute(Character body)
    {
        body.jump();
        // run the next line of code
        base.execute(body);
    }
}
// An AIFireNode will make the body fire
class AIFireNode : AIActionNode
{
// public
    public AIFireNode(bool shouldFire)
    {
        this.firing = shouldFire;
    }
    public override void execute(Character body)
    {
        body.pressTrigger(this.firing);
        // run the next line of code
        base.execute(body);
    }
// private
    bool firing;
}
// An AISelectWeaponNode will make the body select a weapon
class AISelectWeaponNode : AIActionNode
{
// public
    public AISelectWeaponNode()
    {
    }
    public override void execute(Character body)
    {
        body.cycleWeaponForward();
        // run the next line of code
        base.execute(body);
    }
// private
}