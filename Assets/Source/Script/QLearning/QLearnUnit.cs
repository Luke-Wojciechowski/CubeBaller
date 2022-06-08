using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QLearnUnit{

	private  int numInputs;
	private  int numOutputs;
	private  int numHidden;
	private int _numPerHidden;
	private float alpha;
	private List<Layer> layers = new List<Layer>();

	public int NumInputs
	{
		get => numInputs;
		set => numInputs = value;
	}

	public int NumOutputs
	{
		get => numOutputs;
		set => numOutputs = value;
	}

	public int NumHidden
	{
		get => numHidden;
		set => numHidden = value;
	}

	public int NumPerHidden
	{
		get => _numPerHidden;
		set => _numPerHidden = value;
	}

	public float Alpha
	{
		get => alpha;
		set => alpha = value;
	}

	public List<Layer> Layers
	{
		get => layers;
		set => layers = value;
	}

	public QLearnUnit(int numberInput, int numberOfOutputs, int numberOfLayers, int numberOfInputsPerLayer, float a)
	{
		numInputs = numberInput;
		numOutputs = numberOfOutputs;
		numHidden = numberOfLayers;
		_numPerHidden = numberOfInputsPerLayer;
		alpha = a;

		if(numHidden > 0)
		{
			layers.Add(new Layer(_numPerHidden, numInputs));

			for(int i = 0; i < numHidden-1; i++)
			{
				layers.Add(new Layer(_numPerHidden, _numPerHidden));
			}

			layers.Add(new Layer(numOutputs, _numPerHidden));
		}
		else
		{
			layers.Add(new Layer(numOutputs, numInputs));
		}
	}

	public void Train(List<float> inputValues, List<float> desiredOutput)
	{
		List<float> outputValues = new List<float>();
		outputValues = CalcOutput(inputValues);
		UpdateWeights(outputValues, desiredOutput);
		
	}
	
	public List<float> CalcOutput(List<float> inputValues)
	{
		List<float> inputs = new List<float>();
		List<float> outputValues = new List<float>();
		int currentInput = 0;

		if(inputValues.Count != numInputs)
			return outputValues;
		

		inputs = new List<float>(inputValues);
		for(int i = 0; i < numHidden + 1; i++)
		{
				if(i > 0)
				{
					inputs = new List<float>(outputValues);
				}
				outputValues.Clear();

				for(int j = 0; j < layers[i].NumNeurons; j++)
				{
					var triggerValue = 0f;
					layers[i].Neurons[j].Inputs.Clear();

					for(int k = 0; k < layers[i].Neurons[j].NumInputs; k++)
					{
					    layers[i].Neurons[j].Inputs.Add(inputs[currentInput]);
						triggerValue += layers[i].Neurons[j].Weights[k] * inputs[currentInput];
						currentInput++;
					}

					triggerValue -= layers[i].Neurons[j].Bias;

					if(i == numHidden)
						layers[i].Neurons[j].Output = ActivationFunctionO(triggerValue);
					else
						layers[i].Neurons[j].Output = ActivationFunction(triggerValue);
					
					outputValues.Add(layers[i].Neurons[j].Output);
					currentInput = 0;
				}
		}
		return outputValues;
	}
 
	
	void UpdateWeights(List<float> outputs, List<float> desiredOutput)
	{
		float error;
		for(int i = numHidden; i >= 0; i--)
		{
			for(int j = 0; j < layers[i].NumNeurons; j++)
			{
				if(i == numHidden)
				{
					error = desiredOutput[j] - outputs[j];
					layers[i].Neurons[j].ErrorGradient = outputs[j] * (1-outputs[j]) * error;
				}
				else
				{
					layers[i].Neurons[j].ErrorGradient = layers[i].Neurons[j].Output * (1-layers[i].Neurons[j].Output);
					var errorGradSum = 0f;
					for(int p = 0; p < layers[i+1].NumNeurons; p++)
					{
						errorGradSum += layers[i+1].Neurons[p].ErrorGradient * layers[i+1].Neurons[p].Weights[j];
					}
					layers[i].Neurons[j].ErrorGradient *= errorGradSum;
				}	
				for(int k = 0; k < layers[i].Neurons[j].NumInputs; k++)
				{
					if(i == numHidden)
					{
						error = desiredOutput[j] - outputs[j];
						layers[i].Neurons[j].Weights[k] += alpha * layers[i].Neurons[j].Inputs[k] * error;
					}
					else
					{
						layers[i].Neurons[j].Weights[k] += alpha * layers[i].Neurons[j].Inputs[k] * layers[i].Neurons[j].ErrorGradient;
					}
				}
				layers[i].Neurons[j].Bias += alpha * -1 * layers[i].Neurons[j].ErrorGradient;
			}

		}

	}


	float ActivationFunction(float value)
	{
		return TanH(value);
	}

	float ActivationFunctionO(float value)
	{
		return Sigmoid(value);
	}

	float TanH(float value)
	{
		var k =  System.Math.Exp(-2*value);
    	return (float) (2 / (1.0f + k) - 1);
	}
	

	float Sigmoid(float value) 
	{
    	var k = System.Math.Exp(value);
    	return (float) (k / (1.0f + k));
	}
}
