using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoftBody
{
    public int verticesIndex;
    public Vector3 originalVertexPosition;
    public Vector3 currentVertexPosition;

    public Vector3 currentVelocity;

    public SoftBody(int verIndex, Vector3 initVertexPos, Vector3 currVertexPos, Vector3 currVelocity)
    {
        verticesIndex = verIndex;
        originalVertexPosition = initVertexPos;
        currentVertexPosition = currVertexPos;
        currentVelocity = currVelocity;
    }

    public Vector3 positionDifference()
    {
        return currentVertexPosition - originalVertexPosition;
    }

    public void updateVelocity(float bounceSpeed)
    {
        currentVelocity = currentVelocity - positionDifference() * bounceSpeed * Time.deltaTime;
    }

    public void settleObject(float stiffness)
    {
        currentVelocity *= 1f - stiffness * Time.deltaTime;
    }

    public void forceToVertex(Transform transf, Vector3 position, float force)
    {
       
        Vector3 distanceVertPoint = transf.InverseTransformPoint(transf.position) - transf.InverseTransformPoint(position);

        Vector3 forceWithDistance = currentVertexPosition - transf.InverseTransformPoint(position);

        float adaptedForce = force / (1f + forceWithDistance.sqrMagnitude);
        float velocity = adaptedForce * Time.deltaTime;
        currentVelocity += distanceVertPoint.normalized * velocity;
    }
}
