using System.Xml;

namespace demoapp.Model
{
    abstract class Layer
    {
        protected Layer(int non, int nopn, NeuronType nt, string type)
        {
            numofneurons = non;
            numofprevneurons = nopn;
            Neurons = new Neuron[non];
            double[,] Weights = WeightInitialize(MemoryMode.GET, type);
            lastdeltaweights = Weights;
            for (int i = 0; i < non; ++i)
            {
                double[] temp_weights = new double[nopn + 1];
                for (int j = 0; j < nopn + 1; ++j)
                    temp_weights[j] = Weights[i, j];
                Neurons[i] = new Neuron(null, temp_weights, nt);//про подачу null на входы ниже
            }
        }
        protected int numofneurons;//число нейронов текущего слоя
        protected int numofprevneurons;//число нейронов предыдущего слоя
        protected const double learningrate = 0.005d;//скорость обучения
        protected const double momentum = 0.03d;//момент инерции
        protected double[,] lastdeltaweights;//веса предыдущей итерации обучения
        Neuron[] _neurons;//массив нейронов текущего слоя
        public Neuron[] Neurons { get => _neurons; set => _neurons = value; }
        public double[] Data
        {
            set
            {
                for (int i = 0; i < Neurons.Length; ++i)
                {
                    Neurons[i].Inputs = value;
                    Neurons[i].Activator(Neurons[i].Inputs, Neurons[i].Weights);
                }
            }
        }
        public double[,] WeightInitialize(MemoryMode mm, string type)
        {
            double[,] _weights = new double[numofneurons, numofprevneurons + 1];
            XmlDocument memory_doc = new XmlDocument();
            memory_doc.Load(System.IO.Path.Combine("Resources", $"{type}_memory.xml"));
            XmlElement memory_el = memory_doc.DocumentElement;
            switch (mm)
            {
                case MemoryMode.GET:
                    for (int l = 0; l < _weights.GetLength(0); ++l)
                    {
                        for (int k = 0; k < _weights.GetLength(1); ++k)
                            _weights[l, k] = double.Parse(memory_el.ChildNodes.Item(k + _weights.GetLength(1) * l).InnerText.Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture); //парсинг
                    }
                    break;
                case MemoryMode.SET:
                    for (int l = 0; l < numofneurons; ++l)
                        for (int k = 0; k < numofprevneurons + 1; ++k)
                            memory_el.ChildNodes.Item(k + (numofprevneurons + 1) * l).InnerText = Neurons[l].Weights[k].ToString();
                    break;
            }
            memory_doc.Save(System.IO.Path.Combine("Resources", $"{type}_memory.xml"));
            return _weights;
        }
        abstract public void Recognize(Network net, Layer nextLayer);//для прямых проходов
        abstract public double[] BackwardPass(double[] stuff);//и обратных
    }
}
