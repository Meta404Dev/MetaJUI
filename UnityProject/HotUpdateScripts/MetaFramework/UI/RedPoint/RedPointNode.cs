using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MetaFramework.UI
{
    public class RedPointNode
    {
        /// <summary>
        /// 节点名称
        /// </summary>        
        public string nodeName;

        /// <summary>
        /// 总的红点数量
        /// </summary>        
        public int pointCount = 0;

        /// <summary>
        /// 父节点
        /// </summary>
        public RedPointNode parent = null;

        /// <summary>
        /// 数量变化回调
        /// </summary>        
        public Action<RedPointNode> onChangeCount;

        /// <summary>
        /// 子节点字典 
        /// </summary>        
        public Dictionary<string, RedPointNode> dicChilds = new Dictionary<string, RedPointNode>();

        public RedPointNode(string nodeName)
        {
            this.nodeName = nodeName;
        }

        /// <summary>
        /// 添加子节点
        /// </summary>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        public RedPointNode AddChildNode(string nodeName)
        {
            RedPointNode childNode = new RedPointNode(nodeName);
            childNode.parent = this;

            dicChilds.Add(nodeName, childNode);

            return childNode;
        }

        /// <summary>  
        /// 设置当前节点的红点数量  
        /// </summary>  
        /// <param name="newCount"></param>  
        public void SetRedPointCount(int newCount)
        {
            //红点数量只能设置叶子节点  
            if (dicChilds.Count > 0)
            {
                Debug.LogError("Only Can Set Leaf Node!");
                return;
            }

            pointCount = newCount;

            NotifyPointCountChange();
        }

        /// <summary>  
        /// 计算当前红点数量  
        /// </summary>  
        private void ChangeRedPointCount()
        {
            int count = 0;
            foreach (var node in dicChilds.Values)
            {
                count += node.pointCount;
            }

            //红点有变化  
            if (count != pointCount)
            {
                pointCount = count;
                NotifyPointCountChange();
            }
        }

        /// <summary>  
        /// 通知红点数量变化  
        /// </summary>  
        private void NotifyPointCountChange()
        {
            onChangeCount?.Invoke(this);

            if (parent != null)
            {
                parent.ChangeRedPointCount();
            }
        }
    }
}
