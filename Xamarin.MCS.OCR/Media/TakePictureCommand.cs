﻿using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.IO;
using System.Threading.Tasks;
using Wibci.LogicCommand;

namespace Xamarin.MCS.OCR.Media
{
    public enum CameraOption
    {
        Back,
        Front
    }

    public interface ITakePictureCommand : IAsyncLogicCommand<TakePictureRequest, SelectPictureResult>
    {
    }

    public class ChoosePictureRequest
    {
        public int? MaxPixelDimension { get; set; }
    }

    public class SelectPictureResult : TaskCommandResult
    {
        public Stream ImageStream { get; set; }
    }

    public class TakePictureCommand : AsyncLogicCommand<TakePictureRequest, SelectPictureResult>, ITakePictureCommand
    {
        private IMedia MediaPicker
        {
            get { return CrossMedia.Current; }
        }

        public override async Task<SelectPictureResult> ExecuteAsync(TakePictureRequest request)
        {
            var retResult = new SelectPictureResult();

            await MediaPicker.Initialize();

            if (!MediaPicker.IsCameraAvailable || !MediaPicker.IsTakePhotoSupported)
            {
                retResult.Notification.Add("No camera available :(");
                retResult.TaskResult = TaskResult.Failed;
                return retResult;
            }

            StoreCameraMediaOptions options = new StoreCameraMediaOptions();
            options.Directory = "SyncfusionSamples";
            options.Name = DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
            if (request.MaxPixelDimension.HasValue)
            {
                options.PhotoSize = PhotoSize.Custom;
                options.CustomPhotoSize = request.MaxPixelDimension.Value;
            }

            var mediaFile = await MediaPicker.TakePhotoAsync(options);

            if (mediaFile != null)
            {
                using (mediaFile)
                {
                    retResult.ImageStream = mediaFile.GetStream();
                }

                retResult.TaskResult = TaskResult.Success;
            }
            else
            {
                retResult.TaskResult = TaskResult.Canceled;
                retResult.Notification.Add("Select picture canceled");
            }

            return retResult;
        }
    }

    public class TakePictureRequest : ChoosePictureRequest
    {
        public TakePictureRequest()
        {
            CameraOption = CameraOption.Back;
        }

        public static TakePictureRequest Default
        {
            get { return new TakePictureRequest(); }
        }

        public CameraOption CameraOption { get; set; }
    }
}