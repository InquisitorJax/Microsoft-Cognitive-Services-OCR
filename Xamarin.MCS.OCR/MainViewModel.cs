using System;
using System.Collections.Generic;
using Prism.Commands;
using Prism.Mvvm;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Validation;
using Wibci.LogicCommand;
using Xamarin.Forms;
using Xamarin.MCS.OCR.Media;

namespace Xamarin.MCS.OCR
{
    public class MainViewModel : BindableBase
    {
        private string _busyMessage;
        private bool _isBusy;
        private byte[] _sampleImage;

        private string _textResult;

        public MainViewModel()
        {
            ChoosePictureCommand = new DelegateCommand(ChoosePicture);
            TakePictureCommand = new DelegateCommand(TakePicture);
            LoadJobCardCommand = new DelegateCommand(LoadJobCard);
        }

        public string BusyMessage
        {
            get { return _busyMessage; }
            set { SetProperty(ref _busyMessage, value); }
        }

        private bool _useHandwritingRecog;

        public bool UseHandwritingRecog
        {
            get => _useHandwritingRecog;
            set => SetProperty(ref _useHandwritingRecog, value);
        }

        public ICommand ChoosePictureCommand { get; private set; }

        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }

        public ICommand LoadJobCardCommand { get; private set; }

        public byte[] SampleImage
        {
            get { return _sampleImage; }
            set { SetProperty(ref _sampleImage, value); }
        }

        public ICommand TakePictureCommand { get; private set; }

        public string TextResult
        {
            get { return _textResult; }
            set { SetProperty(ref _textResult, value); }
        }

        private IChoosePictureCommand ChoosePictureCommandLogic
        {
            get { return DependencyService.Get<IChoosePictureCommand>(); }
        }

        private IRecognizeTextFromImageCommand RecognizeTextCommand
        {
            get { return DependencyService.Get<IRecognizeTextFromImageCommand>(); }
        }

        private ITakePictureCommand TakePictureCommandLogic
        {
            get { return DependencyService.Get<ITakePictureCommand>(); }
        }

        private async void ChoosePicture()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            BusyMessage = "...processing image from device.";
            try
            {
                var picRequest = new ChoosePictureRequest()
                {
                    MaxPixelDimension = 500
                };

                var pictureResult = await ChoosePictureCommandLogic.ExecuteAsync(picRequest);

                if (pictureResult.TaskResult == TaskResult.Success)
                {
                    await ProcessImageStream(pictureResult.ImageStream);
                }
            }
            finally
            {
                NotBusy();
            }
        }

        private async void LoadJobCard()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            BusyMessage = "...processing sample job card.";
            try
            {
                string resoureceName = UseHandwritingRecog ? "JobCard.jpg" : "EmptyJobCard.png";
                var imageStream = ResourceLoader.GetEmbeddedResourceStream(GetType().Assembly, "JobCard.jpg");

                await ProcessImageStream(imageStream);
            }
            finally
            {
                NotBusy();
            }
        }

        private void NotBusy()
        {
            IsBusy = false;
            BusyMessage = null;
        }

        private async Task ProcessImageStream(Stream imageStream)
        {
            Requires.NotNull(imageStream, nameof(imageStream));

            TextResult = null;

            var submitStream = await imageStream.ResizeAsync(1200);

            SampleImage = submitStream.ToByteArray();            

            var request = new ImageRecognitionRequest(submitStream, UseHandwritingRecog);
            var recogResult = await RecognizeTextCommand.ExecuteAsync(request);

            if (recogResult.IsValid())
            {
                TextResult = String.Join(" ", recogResult.TextResults);
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("", recogResult.Notification.ToString(), "ok");
            }
        }

        private async void TakePicture()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            BusyMessage = "...processing image from camera.";
            try
            {
                var picRequest = new TakePictureRequest
                {
                    MaxPixelDimension = 800,
                    CameraOption = CameraOption.Back
                };

                var pictureResult = await TakePictureCommandLogic.ExecuteAsync(picRequest);

                if (pictureResult.TaskResult == TaskResult.Success)
                {
                    await ProcessImageStream(pictureResult.ImageStream);
                }
            }
            finally
            {
                NotBusy();
            }
        }
    }
}