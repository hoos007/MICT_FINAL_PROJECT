using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.Numerics;

namespace MICT_Final_Project
{
    static class HopFieldProcess
    {
        public static List<Image> image = new List<Image>();
        public static Image testImage;
        private static int width = 75;
        private static int height = 100;
        public static List<int[]> img = new List<int[]>();
        public static int[] testImg = new int[width * height];
        public static int[] preImg = new int[width * height];
        public static int[] resultImg = new int[width * height];
        public static int[,] w = new int[width * height, width * height];
        private static int learnCnt = 0;

        public static void setImage(Image getimage, int type)
        {
            int x, y, brightness;
            int max = int.MinValue;
            int min = int.MaxValue;
            int T;
            int i;
            if (type == 0) image.Add(getimage);
            else if (type == 1) testImage = getimage;
            img.Add(new int[width * height]);
            Color color, gray;
            Bitmap gBitmap = new Bitmap(type == 0 ? image[learnCnt] : testImage);
            for (x = 0; x < ((type == 0) ? image[learnCnt].Width : testImage.Width); x++)
                for (y = 0; y < ((type == 0) ? image[learnCnt].Height : testImage.Height); y++)
                {
                    color = gBitmap.GetPixel(x, y);
                    brightness = (int)(0.299 * color.R + 0.587 * color.G + 0.114 * color.B);
                    max = max < brightness ? brightness : max;
                    min = min > brightness ? brightness : min;
                    gray = Color.FromArgb(brightness, brightness, brightness);
                    gBitmap.SetPixel(x, y, gray);
                }
            T = (max + min) / 2;
            switch(type)
            {
                case 0:
                    i = 0;
                    for (y = 0; y < image[learnCnt].Height; y++)
                    {
                        for (x = 0; x < image[learnCnt].Width; x++)
                        {
                            gray = gBitmap.GetPixel(x, y);
                            brightness = gray.R <= T ? 1 : -1;
                            img[learnCnt][i] = brightness;
                            i++;
                        }
                    }
                    learnCnt++;
                    break;
                case 1:
                    i = 0;
                    for (y = 0; y < testImage.Height; y++)
                    {
                        for (x = 0; x < testImage.Width; x++)
                        {
                            gray = gBitmap.GetPixel(x, y);
                            brightness = gray.R <= T ? 1 : -1;
                            testImg[i] = brightness;
                            i++;
                        }
                    }
                    break;
            }

        }

        public static void wplus()
        {
            for (int i = 0; i < img[learnCnt-1].Length; i++) {
                for (int j = 0; j < img[learnCnt - 1].Length; j++) {
                    w[i, j] += img[learnCnt - 1][i] * img[learnCnt - 1][j];
                }
            }
            for (int i = 0; i< img[learnCnt - 1].Length; i++) {
                w[i, i] -= 1;
            }
        }

        public static void process()
        {
            int netSum;
            resultImg = (int[])testImg.Clone();
            do {
                preImg = (int[])testImg.Clone();
                for (int i = 0; i < width; i++) {
                    for (int j = i; j < testImg.Length; j += width) {
                        netSum = 0;
                        for (int k = 0; k < testImg.Length; k++) {
                             netSum += resultImg[k] * w[k, j];
                        }
                        testImg[j] = testImg[j] + netSum;
                        if ((testImg[j] + netSum) > 0) testImg[j] = 1;
                        else if ((testImg[j] + netSum) == 0) testImg[j] += 0 ;
                        else if ((testImg[j] + netSum) < 0) testImg[j] = -1;
                    }
                }
            } while (!testImg.SequenceEqual(preImg));
        }

        public static int resultCheck()
        {
            double count;
            double[] resultErrorRate = new double[learnCnt];
            for(int j=0; j< learnCnt; j++)
            {
                count = 0;
                for (int i = 0; i < img[j].Length; i++)
                {
                    if (testImg[i] != img[j][i])
                    {
                        count++;
                    }
                }
                resultErrorRate[j]= (count / img[j].Length) * 100;
            }

            int minIdx = -1;
            double minErrorRate = double.MaxValue;
            for(int i = 0; i<learnCnt; i++)
            {
                if(minErrorRate > resultErrorRate[i])
                {
                    minIdx = i;
                    minErrorRate = resultErrorRate[i];
                }
            }


            return minIdx;
        }
    }
}
