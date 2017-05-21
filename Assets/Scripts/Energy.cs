using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Energy : MonoBehaviour {

	public float maxEnergy = 5;
	
	float currentEnergy;
	
	// Getter
	public float CurrentEnergy {	get	{ return currentEnergy; } }
	public float FractionEnergy { get { return (float)(currentEnergy)/(float)(maxEnergy); } }
	
	void Start()
	{
		// initialize the component with zero health
		currentEnergy = 0;
	}

	// Adds a specified value to the component's current HP
	// To deduct HP from the component, use a negative value as the argument
	public void Modify(float amount)
	{
		currentEnergy += amount;

		// if component's current energy somehow exceeds max energy
		if (currentEnergy > maxEnergy)
		{
			// reset current HP to max energy
			currentEnergy = maxEnergy;
		}
		// otherwise, if component lost all energy
		else if (currentEnergy <= 0)
		{
			currentEnergy = 0;
		}
	}
	
	public void Set(float amount)
	{
		currentEnergy = amount;
		Modify (0); // run through the checks in Modify()
	}

}
