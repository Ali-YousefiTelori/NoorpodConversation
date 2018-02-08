using Microsoft.Win32;
using NAudio.Mixer;
using NAudio.Utils;
using NAudio.Wave;
using Newtonsoft.Json;
using NoorpodConversation.BaseViewModels.Views;
using NoorpodConversation.Models;
using NoorpodConversation.Services;
using SignalGo.Shared.Log;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NoorpodConversation.UI
{
    public class UploadFile
    {
        public UploadFile()
        {
            ContentType = "application/octet-stream";
        }
        public string Name { get; set; }
        public string Filename { get; set; }
        public string ContentType { get; set; }
        public Stream Stream { get; set; }
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            //BlockingCollection<int> bc = new BlockingCollection<int>();

            //Task.Factory.StartNew(() =>
            //{
            //    while (true)
            //    {
            //        Thread.Sleep(5000);
            //        bc.Add(1);
            //        bc.Add(2);
            //        bc.Add(3);
            //        var get = bc.Take();
            //    }
            //});

            //Task.Factory.StartNew(() =>
            //{
            //    int get = 0;
            //    bc.TryTake(out get);
            //    get = bc.Take();
            //    int i = 0;
            //});


            //return;

            ViewModels.Helpers.ViewsUtility.MainWindow = this;
            NoorpodConversation.ViewModels.Helpers.ApplicationHelper.InitializeDispatcher(Dispatcher);
            ApplicationHelper.Current.AddResourceDictionary(Application.Current.Resources);
            string fileName = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "userData.db");
            LoginPageBaseViewModel.LoadDataFunction = () =>
            {
                //try
                //{
                //    if (File.Exists(fileName))
                //    {
                //        var json = File.ReadAllText(fileName);
                //        return JsonConvert.DeserializeObject<UserClient>(json);
                //    }
                //}
                //catch (Exception ex)
                //{
                //    AutoLogger.LogError(ex, "Read Registry");
                //}
                return null;
            };

            LoginPageBaseViewModel.SaveFunction = (client) =>
            {
                //try
                //{
                //    File.WriteAllText(fileName, JsonConvert.SerializeObject(client));
                //}
                //catch (Exception ex)
                //{
                //    //GetAdminAccess.ElevateProcess(Process.GetCurrentProcess());
                //    AutoLogger.LogError(ex, "Save Registry");
                //}
            };
            ApplicationHelper.Current.RefreshCommandsAction = () =>
            {
                ApplicationHelper.Current.EnterDispatcherThreadAction(() =>
                {
                    CommandManager.InvalidateRequerySuggested();
                });
            };
            NoorpodInforming.InformingManager.InitializeBase();
            InitializeComponent();
            NoorpodServiceHelper.AudioReceivedFunction = (bytes) =>
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    try
                    {
                        if (bwp != null)
                        {
                            //var bytes = SevenZip.SevenZipExtractor.ExtractBytes(com);
                            bwp.AddSamples(bytes, 0, bytes.Length);
                            //bwp.AddSamples(com, 0, com.Length);
                        }
                    }
                    catch (Exception ex)
                    {
                        AutoLogger.LogError(ex, "NoorpodServiceHelper.AudioReceivedFunction");
                    }
                }));
            };

            NoorpodServiceHelper.OnChangedSpeeckQaulityAction = (buffer, simpleRate) =>
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    try
                    {
                        waveIn.Dispose();
                        bufferMilliseconds = buffer;
                        sampleRate = simpleRate;
                        btnRecourd_Click(null, null);
                        btnPlay_Click(null, null);
                    }
                    catch (Exception ex)
                    {
                        AutoLogger.LogError(ex, "NoorpodServiceHelper.OnChangedSpeeckQaulityAction");
                        MessageBox.Show(ex.Message);
                    }
                }));
            };
            Closing += MainWindow_Closing;
            btnRecourd_Click(null, null);
            btnPlay_Click(null, null);
            Activated += MainWindow_Activated;
            Deactivated += MainWindow_Deactivated;
        }

        private void MainWindow_Deactivated(object sender, EventArgs e)
        {
            InformingMessage.CanShowMessage = true;
        }

        private void MainWindow_Activated(object sender, EventArgs e)
        {
            InformingMessage.CanShowMessage = false;
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            NoorpodServiceHelper.ApplicationClosed = true;
            NoorpodServiceHelper.Disconnect();
            Application.Current.Shutdown();
        }
        
        public byte[] UploadFiles(string address, IEnumerable<UploadFile> files, NameValueCollection values)
        {
            var request = WebRequest.Create(address);
            request.Method = "POST";
            var boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x", NumberFormatInfo.InvariantInfo);
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            boundary = "--" + boundary;

            using (var requestStream = request.GetRequestStream())
            {
                // Write the values
                foreach (string name in values.Keys)
                {
                    var buffer = Encoding.ASCII.GetBytes(boundary + Environment.NewLine);
                    requestStream.Write(buffer, 0, buffer.Length);
                    buffer = Encoding.ASCII.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\"{1}{1}", name, Environment.NewLine));
                    requestStream.Write(buffer, 0, buffer.Length);
                    buffer = Encoding.UTF8.GetBytes(values[name] + Environment.NewLine);
                    requestStream.Write(buffer, 0, buffer.Length);
                }

                // Write the files
                foreach (var file in files)
                {
                    var buffer = Encoding.ASCII.GetBytes(boundary + Environment.NewLine);
                    requestStream.Write(buffer, 0, buffer.Length);
                    buffer = Encoding.UTF8.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"{2}", file.Name, file.Filename, Environment.NewLine));
                    requestStream.Write(buffer, 0, buffer.Length);
                    buffer = Encoding.ASCII.GetBytes(string.Format("Content-Type: {0}{1}{1}", file.ContentType, Environment.NewLine));
                    requestStream.Write(buffer, 0, buffer.Length);
                    file.Stream.CopyTo(requestStream);
                    buffer = Encoding.ASCII.GetBytes(Environment.NewLine);
                    requestStream.Write(buffer, 0, buffer.Length);
                }

                var boundaryBuffer = Encoding.ASCII.GetBytes(boundary + "--");
                requestStream.Write(boundaryBuffer, 0, boundaryBuffer.Length);
            }

            using (var response = request.GetResponse())
            using (var responseStream = response.GetResponseStream())
            using (var stream = new MemoryStream())
            {
                responseStream.CopyTo(stream);
                return stream.ToArray();
            }
        }

        List<WaveInCapabilities> currentMics = new List<WaveInCapabilities>();

        private void btnRefreshMic_Click(object sender, RoutedEventArgs e)
        {
            currentMics.Clear();
            int waveInDevices = WaveIn.DeviceCount;
            for (int waveInDevice = 0; waveInDevice < waveInDevices; waveInDevice++)
            {
                WaveInCapabilities deviceInfo = WaveIn.GetCapabilities(waveInDevice);
                currentMics.Add(deviceInfo);
            }
            micCombo.ItemsSource = currentMics;
        }

        //WaveFileWriter waveFile = null;
        MemoryStream playStream = new MemoryStream();
        private BufferedWaveProvider bwp = null;
        WaveIn waveIn;
        int sampleRate = 9000; // 8 kHz
        int bufferMilliseconds = 10;
        private void btnRecourd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                waveIn = new WaveIn();
                waveIn.DeviceNumber = 0;// micCombo.SelectedIndex;
                waveIn.DataAvailable -= WaveIn_DataAvailable;
                waveIn.DataAvailable += WaveIn_DataAvailable;
                int channels = 1; // mono
                waveIn.WaveFormat = new WaveFormat(sampleRate, channels);
                //using (WaveStream waveStream = WaveFormatConversionStream.CreatePcmStream(new Mp3FileReader(inputStream)))
                //waveFile = new WaveFileWriter(new IgnoreDisposeStream(playStream), waveIn.WaveFormat);
                //waveFile = new WaveFileWriter(playStream, waveIn.WaveFormat);
                waveIn.BufferMilliseconds = bufferMilliseconds;
                waveIn.StartRecording();

                //int waveInDeviceNumber = micCombo.SelectedIndex;
                //var mixerLine = new MixerLine((IntPtr)waveInDeviceNumber,
                //                               0, MixerFlags.WaveIn);
                //foreach (var control in mixerLine.Controls)
                //{
                //    if (control.ControlType == MixerControlType.Volume)
                //    {
                //        var volumeControl = control as UnsignedMixerControl;
                //        volumeControl.Value = volumeControl.MaxValue;
                //        break;
                //    }
                //}
                btnRecourd.IsEnabled = false;
            }
            catch (Exception ex)
            {
                AutoLogger.LogError(ex, "btnRecourd_Click");
                MessageBox.Show(ex.Message);
            }
        }

        //bool sendData = false;
        private void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            try
            {
                //if (waveFile != null)
                //{
                //waveFile.Write(e.Buffer, 0, e.BytesRecorded);
                //waveFile.Flush();
                //var bytes = e.Buffer.ToList().GetRange(0, e.BytesRecorded).ToArray();
                //bool hasValue = false;
                //for (int index = 0; index < e.BytesRecorded; index += 2)
                //{
                //    short sample = (short)((e.Buffer[index + 1] << 8) |
                //                            e.Buffer[index + 0]);
                //    float sample32 = sample / 32768f;
                //    if (sample32 > 0.05)
                //    {
                //        hasValue = true;
                //        break;
                //    }
                //}
                //if (hasValue && bwp != null)
                //    bwp.AddSamples(bytes, 0, bytes.Length);
                //return;
                if (NoorpodServiceHelper.IsSendAudioNow)
                {
                    NoorpodServiceHelper.connection.BufferSize = e.BytesRecorded;
                    NoorpodServiceHelper.SendAudio(e.Buffer.ToList().GetRange(0, e.BytesRecorded).ToArray());
                    //NoorpodServiceHelper.SendAudio(bytes);
                    //bwp.AddSamples(bytes, 0, bytes.Length);
                    //byte[] bytesToCompress = bytes;
                    //var com = SevenZip.SevenZipCompressor.CompressBytes(bytes);
                    //var decom = SevenZip.SevenZipExtractor.ExtractBytes(com);
                }
                //}
            }
            catch (Exception ex)
            {
                AutoLogger.LogError(ex, "WaveIn_DataAvailable");
            }
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //int sampleRate = 2000; // 8 kHz
                int channels = 1; // mono
                bwp = new BufferedWaveProvider(new WaveFormat(sampleRate, channels));
                bwp.DiscardOnBufferOverflow = true;
                var waveOut = new WaveOut();
                waveOut.Init(bwp);
                waveOut.Play();
                btnPlay.IsEnabled = false;
            }
            catch (Exception ex)
            {

            }
        }

        private void btnConnnect_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnplayMic_Click(object sender, RoutedEventArgs e)
        {
            // sendData = btnplayMic.IsChecked.Value;
        }
    }

    public static class GetAdminAccess
    {
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
        public static Process ElevateProcess(Process source)
        {
            //Create a new process
            Process target = new Process();
            target.StartInfo = source.StartInfo;
            target.StartInfo.FileName = source.MainModule.FileName;
            target.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(source.MainModule.FileName);

            //Required for UAC to work
            target.StartInfo.UseShellExecute = true;
            target.StartInfo.Verb = "runas";

            try
            {
                if (!target.Start())
                    return null;
            }
            catch (Win32Exception e)
            {
                //Cancelled
                if (e.NativeErrorCode == 1223)
                    return null;
                throw;
            };
            return target;
        }
    }
}
