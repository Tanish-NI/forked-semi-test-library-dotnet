﻿using NationalInstruments.DAQmx;
using NationalInstruments.SemiconductorTestLibrary.Common;
using NationalInstruments.SemiconductorTestLibrary.DataAbstraction;
using static NationalInstruments.SemiconductorTestLibrary.Common.ParallelExecution;

namespace NationalInstruments.SemiconductorTestLibrary.InstrumentAbstraction.DAQmx
{
    /// <summary>
    /// Defines operations on digital input channels.
    /// </summary>
    public static class DigitalInput
    {
        /// <summary>
        /// Reads a single sample and returns pin- and site-aware object of type Boolean.
        /// </summary>
        /// <param name="tasksBundle">The <see cref="DAQmxTasksBundle"/> object.</param>
        /// <returns>Per-site per-pin boolean samples.</returns>
        public static PinSiteData<bool> ReadDigitalSingleSample(this DAQmxTasksBundle tasksBundle)
        {
            return tasksBundle.DoAndReturnPerSitePerPinResults(taskInfo =>
            {
                taskInfo.VerifyTaskType(DAQmxTaskType.DigitalInput);
                if (taskInfo.Task.DIChannels.HasSingleChannel())
                {
                    var channel = new DigitalSingleChannelReader(taskInfo.Task.Stream);
                    return new bool[] { channel.ReadSingleSampleSingleLine() };
                }
                else
                {
                    var channel = new DigitalMultiChannelReader(taskInfo.Task.Stream);
                    return channel.ReadSingleSampleSingleLine();
                }
            });
        }

        /// <summary>
        /// Reads the samples and returns pin- and site-aware object of type DigitalWaveform, where each element in the waveform represents one sample read.
        /// By default, all available samples are returned, unless otherwise specified by passing in value via the samplesToRead argument.
        /// </summary>
        /// <param name="tasksBundle">The <see cref="DAQmxTasksBundle"/> object.</param>
        /// <param name="samplesToRead">The number of samples to read.</param>
        /// <returns>Per-site per-pin digital waveform samples.</returns>
        public static PinSiteData<DigitalWaveform> ReadDigitalWaveform(this DAQmxTasksBundle tasksBundle, int samplesToRead = -1)
        {
            return tasksBundle.DoAndReturnPerSitePerPinResults(taskInfo =>
            {
                taskInfo.VerifyTaskType(DAQmxTaskType.DigitalInput);
                if (taskInfo.Task.DIChannels.HasSingleChannel())
                {
                    var channel = new DigitalSingleChannelReader(taskInfo.Task.Stream);
                    return new DigitalWaveform[] { channel.ReadWaveform(samplesToRead) };
                }
                else
                {
                    var channel = new DigitalMultiChannelReader(taskInfo.Task.Stream);
                    return channel.ReadWaveform(samplesToRead);
                }
            });
        }

        /// <summary>
        /// Reads digital multiple U32 samples.
        /// </summary>
        /// <param name="tasksBundle">The <see cref="DAQmxTasksBundle"/> object.</param>
        /// <param name="samplesToRead">The number of samples to read.</param>
        /// <returns>Per-site per-pin unsigned integer samples.</returns>
        public static PinSiteData<uint[]> ReadDigitalMultiSampleU32(this DAQmxTasksBundle tasksBundle, int samplesToRead = -1)
        {
            return tasksBundle.DoAndReturnPerSitePerPinResults(taskInfo =>
            {
                taskInfo.VerifyTaskType(DAQmxTaskType.DigitalInput);
                if (taskInfo.Task.DIChannels.HasSingleChannel())
                {
                    var channel = new DigitalSingleChannelReader(taskInfo.Task.Stream);
                    return new uint[][] { channel.ReadMultiSamplePortUInt32(samplesToRead) };
                }
                else
                {
                    var channel = new DigitalMultiChannelReader(taskInfo.Task.Stream);
                    return channel.ReadMultiSamplePortUInt32(samplesToRead).ToJaggedArray();
                }
            });
        }
    }
}
