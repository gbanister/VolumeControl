namespace ClientPoc
{
    using System;
    using System.Linq;
    using NAudio.Mixer;
    using NAudio.Wave;

    public class VolumeControl : IVolumeControl
    {
        UnsignedMixerControl volumeControl;
        WaveIn waveIn;

        public VolumeControl(int deviceNumber)
        {
            waveIn = new WaveIn();
            waveIn.DeviceNumber = deviceNumber;
            //           waveIn.DataAvailable += waveIn_DataAvailable;
            //           waveIn.StartRecording();
            
            int waveInDeviceNumber = waveIn.DeviceNumber;
            if (Environment.OSVersion.Version.Major >= 6) // Vista and over
            {
                var mixerLine = waveIn.GetMixerLine();
                //new MixerLine((IntPtr)waveInDeviceNumber, 0, MixerFlags.WaveIn);
                foreach (var control in mixerLine.Controls.Where(control => control.ControlType == MixerControlType.Volume))
                {
                    this.volumeControl = control as UnsignedMixerControl;
                    //Level = desiredVolume;
                    break;
                }
            }
            else
            {
                var mixer = new Mixer(waveInDeviceNumber);
                foreach (var source in from destination in mixer.Destinations 
                                       where destination.ComponentType == MixerLineComponentType.DestinationWaveIn 
                                       from source in destination.Sources 
                                       where source.ComponentType == MixerLineComponentType.SourceMicrophone 
                                       select source)
                {
                    foreach (var control in source.Controls.Where(control => control.ControlType == MixerControlType.Volume))
                    {
                        volumeControl = control as UnsignedMixerControl;
                        //Level = desiredVolume;
                        break;
                    }
                }
            }
        }

        public double Level
        {
            get
            {
                return volumeControl.Percent;
            }
            set { volumeControl.Percent = value; }
        }
    }
}