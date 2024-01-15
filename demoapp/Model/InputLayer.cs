using System.Drawing;

namespace demoapp.Model
{
    class InputLayer
    {
        public struct trainType
        {
            public double[] Item1;
            public byte Item2;
        }
        const int trainsetSize = 5000;
        const int testsetSize = 100;


        public InputLayer(NetworkMode nm)
        {
            System.Drawing.Bitmap bitmap;
            switch (nm)
            {
                case NetworkMode.Train:
                    for (int digit = 0; digit < 10; digit++)
                    {
                        for (int i = 0; i < trainsetSize; ++i)
                        {
                            bitmap = new Bitmap(Image.FromFile("MNIST Dataset\\TrainingImages\\" + digit+"\\"+i+".png"));

                            _trainset[i].Item2 = (byte)digit;
                            _trainset[i].Item1 = new double[28 * 28];

                            for (int m = 0; m < 28; ++m)
                            {
                                for (int n = 0; n < 28; ++n)
                                {
                                    _trainset[i].Item1[n + 28 * m] =
                                        (bitmap.GetPixel(n, m).R +
                                        bitmap.GetPixel(n, m).G +
                                        bitmap.GetPixel(n, m).B) / (765.0d);
                                }
                            }
                        }
                    }
                    //перетасовка обучающей выборки методом Фишера-Йетса
                    for (int n = Trainset.Length - 1; n >= 1; --n)
                    {
                        int j = random.Next(n + 1);
                        trainType temp = _trainset[n];
                        _trainset[n] = _trainset[j];
                        _trainset[j] = temp;
                    }
                    break;
                case NetworkMode.Test:
                    for (int digit = 0; digit < 10; digit++)
                    {
                        for (int i = 0; i < testsetSize; ++i)
                        {
                            bitmap = new Bitmap(Image.FromFile("MNIST Dataset\\TestImages\\" + digit + "\\" + i + ".png"));

                            _testset[i].Item2 = (byte)digit;
                            _testset[i].Item1 = new double[28 * 28];

                            for (int m = 0; m < 28; ++m)
                            {
                                for (int n = 0; n < 28; ++n)
                                {
                                    _testset[i].Item1[n + 28 * m] =
                                        (bitmap.GetPixel(n, m).R +
                                        bitmap.GetPixel(n, m).G +
                                        bitmap.GetPixel(n, m).B) / (765.0d);
                                }
                            }
                        }
                    }
                    break;
            }
        }

        private System.Random random = new System.Random();

        private trainType[] _trainset = new trainType[100]; //100 изображений в обучающей выборке
        public trainType[] Trainset { get => _trainset; }

        private trainType[] _testset = new trainType[9000];
        public trainType[] Testset { get => _testset; }
    }
}
