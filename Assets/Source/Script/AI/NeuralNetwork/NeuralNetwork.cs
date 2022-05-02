using System;
using System.Collections.Generic;
using Ingame.AI.Learning.Network;

namespace Source.Script.AI.NeuralNetwork
{
    public class NeuralNetwork
    {
        private int _numOfInputs;
        private int _numOfOutputs;
        private int _numOfHiddenLayers;
        private int _numOfInputsPerLayer;
        private float _learningRate;
        private List<NetworkLayer> _layers = new List<NetworkLayer>();

        public NeuralNetwork(int inputs, int outputs, int layers,int perLayer, float learning)
        {
            _numOfInputs = inputs;
            _numOfOutputs = outputs;
            _numOfHiddenLayers = layers;
            _learningRate = learning;
            _numOfInputsPerLayer = perLayer;
            _layers.Add(new NetworkLayer(_numOfInputsPerLayer,_numOfInputs));
            
            if (_numOfHiddenLayers <= 0)
            {
                _layers.Add(new NetworkLayer(_numOfOutputs,_numOfInputs));
                return;
            }
            
            //hidden layer
            for (int i = 0; i < _numOfHiddenLayers; i++)
            {
                _layers.Add(new NetworkLayer(_numOfInputsPerLayer,_numOfInputsPerLayer));
            }
        }

        public List<float> Train(List<float> inputs, List<float> realOutput)
        {
            var result = Calculate(inputs);
            UpdateWeights(result, realOutput);
            return result;
        }

        private List<float> Calculate(List<float> inputs)
        {
            int currInput = 0;

            if (inputs.Count != _numOfInputs)
            {
                return null;
            }
            
            var inVal = new List<float>(inputs);
            var outVal = new List<float>();
            //hidden layers
            for (int i = 0; i <= _layers.Count; i++)
            {
                if (i>0)
                {
                    inVal = new List<float>(outVal);
                }
                outVal.Clear();
                for (int j = 0; j < _layers[i].CountPerceptrons; j++)
                {
                    _layers[i].Perceptrons[j].InitInputs(inVal);
                    var N = _layers[i].Perceptrons[j].GetValue();
                    N -= _layers[i].Perceptrons[j].Bias;
                    //todo 
                    //Create functions
                    if (i == _numOfHiddenLayers)
                    {
                        _layers[i].Perceptrons[j].Output = GetLinearFunction(N);
                    }
                    else
                    {
                        _layers[i].Perceptrons[j].Output = GetSigmoidFunction(N);
                    }
                    outVal.Add(_layers[i].Perceptrons[j].Output);
                }
            }
            return outVal;
        }

        private void UpdateWeights(List<float> outputs, List<float> desired)
        {
            var error = 0f;
            for (int i = _layers.Count; i >=0 ; i++)
            {
                for (int j = 0; j < _layers[i].CountPerceptrons; j++)
                {
                    if (_layers.Count == i)
                    {
                        error = desired[j] - outputs[j];
                        _layers[i].Perceptrons[j].Error = outputs[j] * (1-outputs[j]) * error ;
                    }
                    else
                    {
                        _layers[i].Perceptrons[j].Error = _layers[i].Perceptrons[j].Output * (1 - _layers[i].Perceptrons[j].Output);
                        float errorSum = 0;
                        for (int k = 0; k < _layers[i+1].CountPerceptrons; k++)
                        {
                            errorSum += _layers[i + 1].Perceptrons[k].Error * _layers[i + 1].Perceptrons[k].Weights[j];
                        }

                        _layers[i].Perceptrons[j].Error *= errorSum;
                    }

                    for (int k = 0; k < _layers[i].Perceptrons[j].CountInputs; k++)
                    {
                        if (i== _layers.Count)
                        {
                            error = desired[j] - outputs[j];
                            _layers[i].Perceptrons[j].Weights[k] +=
                                _learningRate * _layers[i].Perceptrons[j].Inputs[k] * error;
                        }
                        else
                        {
                            _layers[i].Perceptrons[j].Weights[k] +=
                                _learningRate * _layers[i].Perceptrons[j].Inputs[k] * _layers[i].Perceptrons[j].Error; 
                        }
                    }
                    _layers[i].Perceptrons[j].Bias += _learningRate * (-_layers[i].Perceptrons[j].Error);
                }
            }
        }
        
        private float GetLinearFunction(float val)
        {
            return val;
        }
        private float GetSigmoidFunction(float value) 
        {
            float k = (float)Math.Exp(value);
            return k / (1.0f + k);
        }
    }
}