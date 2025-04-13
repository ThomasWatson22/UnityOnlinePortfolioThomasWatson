using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionTree
{
    public GameObject Agent { get; set; }
    public LOSCondition LOSNode { get; set; }
    public ChangeStateCondition ChangeStateNode { get; set; }

    public MoveToPatrolCondition MoveToPatrolNode { get; set; }

    public List<TreeNode> treeNodeList;


    public DecisionTree(GameObject agent)
    {
        // TODO: Fill in for Lab 7a.
        Agent = agent;
        treeNodeList = new List<TreeNode>();
    }

    public void MakeDecision()
    {
        // TODO: Fill in for Lab 7a.
        TreeNode currentNode = treeNodeList[0];
        while (!currentNode.isLeaf)
        {
            currentNode = ((ConditionNode)currentNode).Condition() ? currentNode.right : currentNode.left;
        }
        ((ActionNode)currentNode).Action();
    }

    public TreeNode AddNode(TreeNode parent, TreeNode child, TreeNodeType type)
    {
        // TODO: Fill in for Lab 7a.
        switch (type)
        {
            case TreeNodeType.LEFT_TREE_NODE:
                parent.left = child;
                break;
            case TreeNodeType.RIGHT_TREE_NODE:
                parent.right = child;
                break;
        }
        child.parent = parent;
        return child;
    }
}
