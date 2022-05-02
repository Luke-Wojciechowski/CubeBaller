using System.Collections.Generic;
using UnityEngine;

namespace Ingame.AI.Learning
{
    public class Perceptron
    {
        private List<float> _inputs = new List<float>();
        private List<float> _weights = new List<float>();
        private float _bias;
        private float _output;
        private float _error;
        private const float WEIGHTS_INIT_RAND = 2.4f; 
        
        
        public float Output
        {
            get => _output;
            set => _output = value;
        }

        public List<float> Weights => _weights;
        public float Error
        {
            get => _error;
            set => _error = value;
        }

        public List<float> Inputs => _inputs;
        public float Bias
        {
            set => _bias = value;
            get => _bias;
        }
        public int CountInputs => _weights.Count;
        public Perceptron(int inputsNum)
        {
            _bias = Random.Range(-WEIGHTS_INIT_RAND/inputsNum,WEIGHTS_INIT_RAND/inputsNum);
            for (var i = 0; i < inputsNum; i++)
            {
                _weights.Add(Random.Range(-WEIGHTS_INIT_RAND/inputsNum,WEIGHTS_INIT_RAND/inputsNum));
            }
        }

        
        public void InitInputs(List<float> l) => _inputs = new List<float>(l);
       
        public float GetValue()
        {
            float result = 0;
            for (int i = 0; i < _inputs.Count; i++)
            {
                result += _inputs[i] * _weights[i];
            }
            result += _bias;
            return result;
        }
    }
}