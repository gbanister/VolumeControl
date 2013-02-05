namespace ClientPoc
{
    using System.Collections.ObjectModel;
    using System.Windows;
    using Caliburn.Micro;
    using NAudio.Wave;

    public class AppViewModel : Screen 
    {
        private double _volume;
        private IVolumeControl _volumeControl;
        private ObservableCollection<string> _micList;

        public AppViewModel()
        {
            _volumeControl = new VolumeControl(0); //IOC ME
        }

        protected override void OnActivate()
        {
            MicList = new ObservableCollection<string>();
            for (int n = 0; n < WaveIn.DeviceCount; n++)
            {
                MicList.Add((WaveIn.GetCapabilities(n).ProductName));
            }
            base.OnActivate();
        }

        public void TalkButton()
        {
            MessageBox.Show(Volume.ToString());
        }

        public double Volume
        {
            get { return _volumeControl.Level; }
            set {
                    _volumeControl.Level = value;
                    //NotifyOfPropertyChange(() => Volume);
                }
        }

        public ObservableCollection<string> MicList {
            get { return _micList; }
            set {
                if (_micList == value) return;
                _micList = value;
                NotifyOfPropertyChange(() => MicList);
            }
        }

    }
}
