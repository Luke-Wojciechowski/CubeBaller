using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layer {

	private int numNeurons;
	private List<Neuron> neurons = new List<Neuron>();

	public int NumNeurons
	{
		get => numNeurons;
		set => numNeurons = value;
	}

	public List<Neuron> Neurons
	{
		get => neurons;
		set => neurons = value;
	}

	public Layer(int nNeurons, int numNeuronInputs)
	{
		numNeurons = nNeurons;
		for(int i = 0; i < nNeurons; i++)
		{
			neurons.Add(new Neuron(numNeuronInputs));
		}
	}
}
