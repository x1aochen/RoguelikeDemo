using UnityEngine;

public class SelectTarget
{
    /// <summary>
    /// 查询目标是否在视野内
    /// </summary>
    /// <param name="searcher">自身</param>
    /// <param name="targets">目标</param>
    /// <param name="dist">距离</param>
    /// <param name="angle">角度</param>
    /// <returns></returns>
    public static bool FindTargets(Transform searcher, Transform targets, float maxDist,float angle)
    {
        //先判断是否在距离内
        Vector3 tmp = searcher.position - targets.position;

        if (tmp.sqrMagnitude > maxDist * maxDist)
            return false;
        
        //判断弧度
        float dot = Vector3.Dot(searcher.forward, tmp.normalized);
        angle *= Mathf.Deg2Rad;

        return dot < angle;
    }
}

