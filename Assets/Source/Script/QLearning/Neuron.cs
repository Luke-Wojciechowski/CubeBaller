using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neuron {

	private int numInputs;
	private float bias;
	private float output;
	private float errorGradient;
	private List<float> weights = new List<float>();
	private List<float> inputs = new List<float>();

	public int NumInputs
	{
		get => numInputs;
		set => numInputs = value;
	}

	public float Bias
	{
		get => bias;
		set => bias = value;
	}

	public float Output
	{
		get => output;
		set => output = value;
	}

	public float ErrorGradient
	{
		get => errorGradient;
		set => errorGradient = value;
	}

	public List<float> Weights
	{
		get => weights;
		set => weights = value;
	}

	public List<float> Inputs
	{
		get => inputs;
		set => inputs = value;
	}

	public Neuron(int nInputs)
	{
		float weightRange =  3f/ nInputs;
		bias = UnityEngine.Random.Range(-weightRange,weightRange);
		numInputs = nInputs;

		for(int i = 0; i < nInputs; i++)
			weights.Add(UnityEngine.Random.Range(-weightRange,weightRange));
	}
}
