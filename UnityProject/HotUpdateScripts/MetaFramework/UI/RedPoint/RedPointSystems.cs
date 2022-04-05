using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MetaFramework.UI
{
    public class RedPointSystems
    {
        /// <summary>
        /// 红点树Root节点
        /// </summary>        
        private RedPointNode rootNode;

        /// <summary>
        /// 示例
        /// </summary>
        public void Example()
        {
            RedPointNode mainNode = new RedPointNode("Main");

            var mailNode = mainNode.AddChildNode("Mail");
            var taskNode = mainNode.AddChildNode("Task");
            var bagNode = mainNode.AddChildNode("Bag");

            mailNode.AddChildNode("Mail1");
            mailNode.AddChildNode("Mail2");
            mailNode.AddChildNode("Mail3");
        }

        /// <summary>
        /// 初始化红点树
        /// </summary>
        public void Init(RedPointNode rootNode)
        {
            this.rootNode = rootNode;
        }

        /// <summary>
        /// 注册红点改变事件
        /// </summary>
        /// <param name="nodeName"></param>
        /// <param name="callBack"></param>
        public void Register(string nodeName, Action<RedPointNode> callBack)
        {
            var node = FindNode(nodeName);
            if (node != null)
            {
                Debug.LogError("register failed! can not find the node:" + nodeName);
                return;
            }

            node.onChangeCount = callBack;
        }

        /// <summary>
        /// 派发事件
        /// </summary>
        /// <param name="nodeName"></param>
        /// <param name="newCount"></param>        
        public void Dispatch(string nodeName, int newCount)
        {
            var node = FindNode(nodeName);
            if (node != null)
            {
                Debug.LogError("register failed! can not find the node:" + nodeName);
                return;
            }

            node.SetRedPointCount(newCount);
        }

        private RedPointNode FindNode(string findNodeName)
        {
            return FindNode(rootNode, findNodeName);
        }

        private RedPointNode FindNode(RedPointNode node, string findNodeName)
        {
            foreach (var childNode in node.dicChilds)
            {
                if (childNode.Key.Equals(findNodeName)) return childNode.Value;

                var findResult = FindNode(childNode.Value, findNodeName);
                if (findResult != null) return findResult;
            }

            return null;
        }

    }
}
