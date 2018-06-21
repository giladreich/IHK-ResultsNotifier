using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace IHK.ResultsNotifier.Utils
{
    public class Audio : IDisposable
    {
        private Mp3FileReader mp3Reader;
        private LoopStream loopStream;
        private WaveOut waveOut;

        private readonly string filePath;

        public bool EnableLooping { get; }
        public bool Playing { get; private set; }

        public Audio(string filePath, bool enableLooping = false)
        {
            this.filePath = filePath;
            EnableLooping = enableLooping;
        }

        public void Init()
        {
            mp3Reader = new Mp3FileReader(filePath);
            waveOut = new WaveOut();
            if (EnableLooping)
            {
                loopStream = new LoopStream(mp3Reader);
                waveOut.Init(loopStream);
            }
            else
            {
                waveOut.Init(mp3Reader);
            }

            waveOut.PlaybackStopped += (sender, e) => Playing = false;
        }

        public void Play()
        {
            if (waveOut == null) return;

            waveOut.Play();
            Playing = true;
        }

        public void Stop()
        {
            waveOut?.Stop();
        }

        public void Pause()
        {
            if (waveOut == null) return;

            waveOut.Pause();
            Playing = false;
        }

        public void Resume()
        {
            if (waveOut == null) return;

            waveOut.Resume();
            Playing = true;
        }

        public void Dispose()
        {
            mp3Reader?.Close(); ;
            mp3Reader?.Dispose();
            loopStream?.Close();
            loopStream?.Dispose();
            waveOut?.Dispose();
        }

        internal sealed class LoopStream : WaveStream
        {
            WaveStream sourceStream;

            public bool EnableLooping { get; set; }

            public LoopStream(WaveStream sourceStream)
            {
                this.sourceStream = sourceStream;
                this.EnableLooping = true;
            }

            public override WaveFormat WaveFormat => sourceStream.WaveFormat;
            public override long Length => sourceStream.Length;
            public override long Position
            {
                get => sourceStream.Position;
                set => sourceStream.Position = value;
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                int totalBytesRead = 0;

                while (totalBytesRead < count)
                {
                    int bytesRead = sourceStream.Read(buffer, offset + totalBytesRead, count - totalBytesRead);
                    if (bytesRead == 0)
                    {
                        if (sourceStream.Position == 0 || !EnableLooping)
                        {
                            // something wrong with the source stream
                            break;
                        }

                        // loop
                        sourceStream.Position = 0;
                    }
                    totalBytesRead += bytesRead;
                }
                return totalBytesRead;
            }
        }
    }
}
