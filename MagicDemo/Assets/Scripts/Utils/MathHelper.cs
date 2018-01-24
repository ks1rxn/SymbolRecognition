using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

class MathHelper {
	private static Random m_random = new Random();

	public static Vector3 GetPointAround(Vector3 initialPoint, float minDistance, float maxDistance) {
		float angle = (float) Random.NextDouble() * 360;
		float distance = Random.Next((int)(maxDistance - minDistance)) + minDistance;
		return new Vector3(initialPoint.x + Mathf.Cos(angle * Mathf.PI / 180) * distance, 0, initialPoint.z + Mathf.Sin(angle * Mathf.PI / 180) * distance);
	}

	public static Vector3 GetPointAround(Vector3 initialPoint, Vector3 lookVector, float maxAngleFromLook, float minDistance, float maxDistance) {
		float angle = -AngleBetweenVectors(Vector3.right, lookVector) + ((float) Random.NextDouble() * maxAngleFromLook * 2 - maxAngleFromLook);
		float distance = Random.Next((int)(maxDistance - minDistance)) + minDistance;
		return new Vector3(initialPoint.x + Mathf.Cos(angle * Mathf.PI / 180) * distance, 0, initialPoint.z + Mathf.Sin(angle * Mathf.PI / 180) * distance);
	}

	public static Vector3 GetPointOnSphere(Vector3 initialPoint, float minRadius, float maxRadius) {
		float distance = (float)Random.NextDouble() * (maxRadius - minRadius) + minRadius;
		float angleXZ = Random.Next(360);
		float angleY = Random.Next(360);
		return new Vector3(distance * Mathf.Cos(angleXZ * Mathf.PI / 180) * Mathf.Sin(angleY * Mathf.PI / 180),
			distance * Mathf.Cos(angleY * Mathf.PI / 180),
			distance * Mathf.Sin(angleXZ * Mathf.PI / 180) * Mathf.Sin(angleY * Mathf.PI / 180));
	}

	public static float AngleBetweenVectors(Vector3 a, Vector3 b) {
		float angleToTarget = Vector3.Angle(a, b);
		if (Vector3.Cross(a, b).y < 0) {
			angleToTarget = -angleToTarget;
		}
		return angleToTarget;
	}

	public static float AngleBetweenVectorsZ(Vector3 a, Vector3 b) {
		float angleToTarget = Vector3.Angle(a, b);
		if (Vector3.Cross(a, b).z < 0) {
			angleToTarget = -angleToTarget;
		}
		return angleToTarget;
	}

	public static Vector3 AngleFrom360To180(Vector3 angle) {
		return new Vector3(angle.x > 180 ? angle.x - 360 : angle.x, angle.y > 180 ? angle.y - 360 : angle.y, angle.z > 180 ? angle.z - 360 : angle.z);
	}

	public static float ValueWithDispertion(float value, float dispertion) {
		return (float)Random.NextDouble() * dispertion * 2 - dispertion + value;
	}

	public static float CalculateAverageValue(List<float> values) {
		float sumOfValues = 0;
		for (int i = 0; i != values.Count; i++) {
			sumOfValues += values[i];
		}
		float averageValue = sumOfValues / values.Count;
		return averageValue;
	}

	public static float CalculateDispertionValue(List<float> values, float averageValue) {
		float sumOfDispertionValues = 0;
		for (int i = 0; i != values.Count; i++) {
			sumOfDispertionValues += Mathf.Pow(values[i] - averageValue, 2);
		}
		float dispertionValue = Mathf.Sqrt(sumOfDispertionValues / values.Count);
		return dispertionValue;
	}

	public static Random Random {
		get {
			return m_random;
		}
	}
}

public class VectorPid {
     public float pFactor, iFactor, dFactor;
 
     private Vector3 integral;
     private Vector3 lastError;
 
     public VectorPid(float pFactor, float iFactor, float dFactor) {
         this.pFactor = pFactor;
         this.iFactor = iFactor;
         this.dFactor = dFactor;
    }

	public VectorPid(Vector3 factors) {
		pFactor = factors.x;
		iFactor = factors.y;
		dFactor = factors.z;
	}
 
     public Vector3 Update(Vector3 currentError, float timeFrame) {
         integral += currentError * timeFrame;
         Vector3 deriv = (currentError - lastError) / timeFrame;
         lastError = currentError;
         return currentError * pFactor
             + integral * iFactor
             + deriv * dFactor;
	}

 }

public class FloatPid {
     public float pFactor, iFactor, dFactor;

	 private float integral;
     private float lastError;
 
     public FloatPid(float pFactor, float iFactor, float dFactor) {
         this.pFactor = pFactor;
         this.iFactor = iFactor;
         this.dFactor = dFactor;
     }
 
     public float Update(float currentError, float timeFrame) {
         integral += currentError * timeFrame;
         float deriv = (currentError - lastError) / timeFrame;
         lastError = currentError;
         return currentError * pFactor
             + integral * iFactor
             + deriv * dFactor;
     }
 }