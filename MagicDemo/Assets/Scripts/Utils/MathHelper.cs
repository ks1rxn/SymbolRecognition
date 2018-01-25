using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

class MathHelper {
	private static Random m_random = new Random();

	public static List<Vector3> ProjectPointsToPlane(List<Vector3> points, Vector3 sourcePoint, Vector3 normal) {
		for (int i = 0; i != points.Count; i++) { 
			Vector3 newPoint = points[i] - normal * (Vector3.Dot(points[i] - sourcePoint, normal) / Vector3.Dot(normal, normal));
			points[i] = newPoint;
		}
		return points;
	}

	public static Vector3 TranslatePointToNewBasis(Vector3 pointVector, Vector3 newRow1, Vector3 newRow2, Vector3 newRow3) {
		Vector3 pointNew = new Vector3(pointVector.x * newRow1.x + pointVector.y * newRow1.y + pointVector.z * newRow1.z, 
					pointVector.x * newRow2.x + pointVector.y * newRow2.y + pointVector.z * newRow2.z,
					pointVector.x * newRow3.x + pointVector.y * newRow3.y + pointVector.z * newRow3.z);
		return pointNew;
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