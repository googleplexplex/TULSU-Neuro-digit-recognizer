using System.Linq;

namespace demoapp.Model
{
    class Network
    {
        public Network(NetworkMode nm) => input_layer = new InputLayer(nm);
        //все слои сети
        private InputLayer input_layer = null;
        public HiddenLayer hidden_layer1 = new HiddenLayer(80, 784, NeuronType.Hidden, nameof(hidden_layer1));
        public OutputLayer output_layer = new OutputLayer(10, 80, NeuronType.Output, nameof(output_layer));
        //массив для хранения выхода сети
        public double[] fact = new double[10];
        //непосредственно обучение
        public void Train(Network net)//backpropagation method
        {
            int epoches = 1200;
            for (int k = 0; k < epoches; ++k)
            {
                for (int i = 0; i < net.input_layer.Trainset.Length; ++i)
                {
                    //прямой проход
                    ForwardPass(net, net.input_layer.Trainset[i].Item1);
                    //вычисление ошибки по итерации
                    double[] errors = new double[net.fact.Length];
                    for (int x = 0; x < errors.Length; ++x)
                    {
                        errors[x] = (x == net.input_layer.Trainset[i].Item2) ? -(net.fact[x] - 1.0d) : -net.fact[x];
                    }
                    //обратный проход и коррекция весов
                    double[] temp_gsums1 = net.output_layer.BackwardPass(errors);
                    net.hidden_layer1.BackwardPass(temp_gsums1);
                }
            }

            //загрузка скорректированных весов в "память"
            //net.hidden_layer1.WeightInitialize(MemoryMode.SET, nameof(hidden_layer1));
            //net.output_layer.WeightInitialize(MemoryMode.SET, nameof(output_layer));
        }
        //тестирование сети
        public void Test(Network net)
        {
            int goodPredictions = 0;
            int badPredictions = 0;

            for (int i = 0; i < net.input_layer.Testset.Length; ++i)
            {
                ForwardPass(net, net.input_layer.Testset[i].Item1);
                if (net.fact.Max() == net.input_layer.Testset[i].Item2)
                    goodPredictions++;
                else
                    badPredictions++;
            }
        }
        public void ForwardPass(Network net, double[] netInput)
        {
            net.hidden_layer1.Data = netInput;
            net.hidden_layer1.Recognize(null, net.output_layer);
            net.output_layer.Recognize(net, null);
        }
    }
}
