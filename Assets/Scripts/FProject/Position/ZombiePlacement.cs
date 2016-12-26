using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FProject
{
    public class ZombiePlacement : MonoBehaviour
    {

        public AppearanceType appearanceType;
        public Transform[] target;
        private int[] targetCheckedList;
        private int currentCheck;

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1f, 0.5f, 0f);
            Gizmos.DrawSphere(base.transform.position, 0.1f);
            Gizmos.matrix = Matrix4x4.TRS(base.transform.position, base.transform.rotation, base.transform.lossyScale);
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        }

        private void InitCheckList()
        {
            targetCheckedList = new int[target.Length];
            for (int i = 0; i < targetCheckedList.Length; i++)
            {
                targetCheckedList[i] = 0;
            }
        }

        /// <summary>
        /// 获取可以到底的目的地
        /// </summary>
        /// <returns></returns>
        public Transform GetTargetShouldBeGo()
        {
            if (target == null || target.Length <= 0)
            {
                return null;
            }
            if (targetCheckedList == null || targetCheckedList.Length == 0)
            {
                InitCheckList();
            }
            List<int> list = new List<int>();
            for (int i = 0; i < target.Length; i++)
            {
                if (targetCheckedList[i] < currentCheck)
                {
                    list.Add(i);
                }
            }
            int num;
            if (list.Count == 0)
            {
                num = Random.Range(0, target.Length);
                currentCheck++;
            }
            else
            {
                int index = Random.Range(0, list.Count);
                num = list[index];
            }
            targetCheckedList[num]++;
            return target[num];
        }

        public float GetHalfWidth()
        {
            return transform.lossyScale.x / 2f;
        }

        public float GetHalfLength()
        {
            return transform.lossyScale.z / 2f;
        }
    }
}
