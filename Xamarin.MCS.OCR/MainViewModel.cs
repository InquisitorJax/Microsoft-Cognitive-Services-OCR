using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Windows.Input;

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

        private void ChoosePicture()
        {
            throw new NotImplementedException();
        }

        private void LoadJobCard()
        {
            throw new NotImplementedException();
        }

        private void TakePicture()
        {
            throw new NotImplementedException();
        }
    }
}