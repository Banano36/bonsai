﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenCV.Net;
using System.ComponentModel;
using System.Drawing.Design;

namespace Bonsai.Vision
{
    public class BackgroundSubtraction : Projection<IplImage, IplImage>
    {
        IplImage image;
        IplImage difference;
        IplImage background;
        int averageCount;

        public BackgroundSubtraction()
        {
            BackgroundFrames = 1;
        }

        public int BackgroundFrames { get; set; }

        [Range(0, 1)]
        [Precision(2, .01)]
        [Editor(DesignTypes.NumericUpDownEditor, typeof(UITypeEditor))]
        public double AdaptationRate { get; set; }

        [Range(0, 255)]
        [Editor(DesignTypes.TrackbarEditor, typeof(UITypeEditor))]
        public double ThresholdValue { get; set; }

        public ThresholdType ThresholdType { get; set; }

        public override IplImage Process(IplImage input)
        {
            if (averageCount == 0)
            {
                image = new IplImage(input.Size, 32, input.NumChannels);
                difference = new IplImage(input.Size, 32, input.NumChannels);
                background = new IplImage(input.Size, 32, input.NumChannels);
                background.SetZero();
            }

            var output = new IplImage(input.Size, 8, input.NumChannels);
            if (averageCount < BackgroundFrames)
            {
                averageCount++;
                output.SetZero();
                ImgProc.cvAcc(input, background, CvArr.Null);
                if (averageCount == BackgroundFrames)
                {
                    Core.cvConvertScale(background, background, 1.0 / averageCount, 0);
                }
            }
            else
            {
                Core.cvConvert(input, image);
                Core.cvAbsDiff(image, background, difference);
                if (AdaptationRate > 0)
                {
                    ImgProc.cvRunningAvg(image, background, AdaptationRate, CvArr.Null);
                }

                ImgProc.cvThreshold(difference, output, ThresholdValue, 255, ThresholdType);
            }

            return output;
        }

        protected override void Unload()
        {
            averageCount = 0;
            if (background != null)
            {
                image.Close();
                difference.Close();
                background.Close();
                background = image = difference = null;
            }
            base.Unload();
        }
    }
}
