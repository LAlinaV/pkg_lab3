using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using static System.Net.Mime.MediaTypeNames;

namespace pkglab3
{
    public partial class Form1 : Form
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        Bitmap bitmapImage;
        Mat image1;
        Mat result4 = new Mat();
        Mat image5;
        Mat grayImage1 = new Mat();
        Mat grayImage2 = new Mat();
        Mat grayImage3 = new Mat();
        double thresholdValue, maxValue = 255;
        Bitmap bitmapImage1;
        Bitmap bitmapImage2;
        Bitmap bitmapImage3;
        Bitmap bitmapImage4;
        Bitmap bitmapImage5;
        int blockSize;
        double C;
        int operation;
        int constanta;

        public Form1(Bitmap bitmapImage, double thresholdValue, double maxValue,int blockSize,double C, int operation,int constanta)
        {
            InitializeComponent();
            
            openFileDialog.Filter = "Файлы изображений (*.jpg, *.jpeg, *.png, *.gif)|*.jpg;*.jpeg;*.png;*.gif";
            this.bitmapImage = bitmapImage;
            this.thresholdValue = thresholdValue;
            this.maxValue = maxValue;
            this.blockSize = blockSize;
            this.C = C; 
            this.operation = operation;
            this.constanta = constanta;
            processPhoto();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void processPhoto()
        {
            pictureBox1.Image = bitmapImage;
            // Применение метода Оцу
            image1 = BitmapConverter.ToMat(bitmapImage);
            Cv2.CvtColor(image1, grayImage1, ColorConversionCodes.BGR2GRAY); 
            Cv2.Threshold(grayImage1, grayImage1, 0, maxValue, ThresholdTypes.Otsu);
            bitmapImage1 = BitmapConverter.ToBitmap(grayImage1);
            pictureBox2.Image = bitmapImage1;

            // Применение метода с фиксированным порогом
            
            Cv2.CvtColor(image1, grayImage2, ColorConversionCodes.BGR2GRAY);
            Cv2.Threshold(grayImage2, grayImage2, thresholdValue, maxValue, ThresholdTypes.Binary);
            bitmapImage2 = BitmapConverter.ToBitmap(grayImage2);
            pictureBox3.Image = bitmapImage2;

            // Применение адаптивной пороговой обработки
            
            Cv2.CvtColor(image1, grayImage3, ColorConversionCodes.BGR2GRAY);
            AdaptiveThresholdTypes adaptiveMethod = AdaptiveThresholdTypes.GaussianC;
            ThresholdTypes thresholdType = ThresholdTypes.Binary;
            Cv2.AdaptiveThreshold(grayImage3, grayImage3, maxValue, adaptiveMethod, thresholdType, blockSize, C);
            bitmapImage3 = BitmapConverter.ToBitmap(grayImage3);
            pictureBox4.Image = bitmapImage3;



            if (operation == 1)
            {
                Cv2.Add(image1, image1, result4);
            }
                
            else if (operation == 2)
            {
                Cv2.Multiply(image1, image1, result4);
            }
            else if (operation == 3)
            {
                Mat image10 = image1;
                
                Mat imageLog = new Mat(image10.Size(), MatType.CV_32FC3);

                // Применение логарифмического преобразования
                for (int i = 0; i < image10.Rows; i++)
                {
                    for (int j = 0; j < image10.Cols; j++)
                    {
                        Vec3b pixel = image10.At<Vec3b>(i, j);
                        imageLog.At<Vec3f>(i, j)[0] = (float)Math.Log(1 + pixel.Item0);
                        imageLog.At<Vec3f>(i, j)[1] = (float)Math.Log(1 + pixel.Item1);
                        imageLog.At<Vec3f>(i, j)[2] = (float)Math.Log(1 + pixel.Item2);
                    }
                }

                // Нормализация и преобразование в 8-битное отображение изображения
                Cv2.Normalize(imageLog, imageLog, 0, 255, NormTypes.MinMax);
                imageLog.ConvertTo(imageLog, MatType.CV_8U);
                result4 = imageLog;
            }
            else if (operation == 4)
            {
                Cv2.Pow(image1, constanta, result4);
            }
            else if (operation == 5)
            {
                Cv2.Multiply(image1, new Scalar(constanta), result4);
            }
            else if (operation == 6)
            {
                Cv2.Add(image1, new Scalar(constanta), result4);
            }
            else
            {
                Cv2.BitwiseNot(image1, result4);
            }
            // Поэлементное умножение изображений

            
            bitmapImage4 = BitmapConverter.ToBitmap(result4);
            pictureBox5.Image = bitmapImage4;


            // Линейное контрастирование
            image5 = BitmapConverter.ToMat(bitmapImage);
            double minValue1, maxValue1;
            Cv2.MinMaxLoc(image5, out minValue1, out maxValue1);

            // Линейное контрастирование
            double alpha = 255.0 / (maxValue - minValue1);
            double beta = -minValue1 * alpha;
            Cv2.ConvertScaleAbs(image5, image5, alpha, beta);
            bitmapImage5 = BitmapConverter.ToBitmap(image5);
            pictureBox6.Image = bitmapImage5;
            image1.Dispose();
            result4.Dispose();
            grayImage1.Dispose();
            grayImage2.Dispose();
            grayImage3.Dispose();
        }
        
    }
}
