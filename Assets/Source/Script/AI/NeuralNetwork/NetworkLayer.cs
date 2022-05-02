using System.Collections.Generic;

namespace Ingame.AI.Learning.Network
{
    public class NetworkLayer
    {
        private List<Perceptron> _perceptrons = new List<Perceptron>();

        public NetworkLayer(int neurons, int inputsNum)
        {
            for (int i = 0; i < neurons; i++)
            {
                _perceptrons.Add(new Perceptron(inputsNum));
            }
        }

        public int CountPerceptrons => _perceptrons.Count;
        public List<Perceptron> Perceptrons => _perceptrons;
    }
}