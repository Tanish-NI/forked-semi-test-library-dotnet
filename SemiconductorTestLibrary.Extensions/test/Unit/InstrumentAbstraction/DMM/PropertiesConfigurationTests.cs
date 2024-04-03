﻿using System;
using System.Collections.Generic;
using System.Linq;
using NationalInstruments.ModularInstruments.NIDmm;
using NationalInstruments.Restricted;
using NationalInstruments.SemiconductorTestLibrary.Common;
using NationalInstruments.SemiconductorTestLibrary.InstrumentAbstraction;
using NationalInstruments.SemiconductorTestLibrary.InstrumentAbstraction.DMM;
using NationalInstruments.TestStand.SemiconductorModule.CodeModuleAPI;
using Xunit;
using static NationalInstruments.SemiconductorTestLibrary.InstrumentAbstraction.DMM.InitializeAndClose;
using static NationalInstruments.Tests.SemiconductorTestLibrary.Utilities.TSMContext;

namespace NationalInstruments.Tests.SemiconductorTestLibrary.Unit.InstrumentAbstraction.DMM
{
    [Collection("NonParallelizable")]
    public sealed class PropertiesConfigurationTests : IDisposable
    {
        private readonly ISemiconductorModuleContext _tsmContext;
        private readonly TSMSessionManager _sessionManager;

        public PropertiesConfigurationTests()
        {
            _tsmContext = CreateTSMContext("DMMTestsWith4081.pinmap");
            _sessionManager = new TSMSessionManager(_tsmContext);
            Initialize(_tsmContext);
        }

        public void Dispose()
        {
            Close(_tsmContext);
        }

        [Fact]
        public void ConfigureACBandwidth_ValuesConfigured()
        {
            var sessionsBundle = _sessionManager.DMM(new string[] { "DUTPin_4065", "SystemPin_4070", "DUTPin_4081", "SystemPin_4081" });
            sessionsBundle.ConfigureACBandwidth(minimumFrequency: 100, maximumFrequency: 10000);

            foreach (var sessionInfo in sessionsBundle.InstrumentSessions)
            {
                Assert.Equal(100, sessionInfo.Session.AC.FrequencyMin);
                // It seems the maximum frequency is not configurable, it's fixed at 100kHz for 4065 and 300kHz for 4070.
                // Assert.Equal(1000, sessionInfo.Session.AC.FrequencyMax);
            }
        }

        [Fact]
        public void ConfigureApertureTimeInSeconds_ValuesConfigured()
        {
            var sessionsBundle = _sessionManager.DMM(new string[] { "DUTPin_4065", "SystemPin_4070", "DUTPin_4081", "SystemPin_4081" });
            sessionsBundle.ConfigureApertureTime(DmmApertureTimeUnits.Seconds, 0.1);

            foreach (var sessionInfo in sessionsBundle.InstrumentSessions)
            {
                Assert.Equal(DmmApertureTimeUnits.Seconds, sessionInfo.Session.Advanced.ApertureTimeUnits);
                Assert.Equal(0.1, sessionInfo.Session.Advanced.ApertureTime);
            }
        }

        [Fact]
        public void ConfigureApertureTimeInPowerLineCycles_ValuesConfigured()
        {
            var sessionsBundle = _sessionManager.DMM(new string[] { "DUTPin_4065", "SystemPin_4070", "DUTPin_4081", "SystemPin_4081" });
            sessionsBundle.ConfigureApertureTime(DmmApertureTimeUnits.PowerLineCycles, 2);

            foreach (var sessionInfo in sessionsBundle.InstrumentSessions)
            {
                Assert.Equal(DmmApertureTimeUnits.PowerLineCycles, sessionInfo.Session.Advanced.ApertureTimeUnits);
                Assert.Equal(2, sessionInfo.Session.Advanced.ApertureTime);
            }
        }

        [Fact]
        public void ConfigureMeasurementAbsolute_ValuesConfigured()
        {
            var sessionsBundle = _sessionManager.DMM(new string[] { "DUTPin_4065", "SystemPin_4070", "DUTPin_4081", "SystemPin_4081" });
            sessionsBundle.ConfigureMeasurementAbsolute(DmmMeasurementFunction.DCCurrent, 0.2, 0.001);

            foreach (var sessionInfo in sessionsBundle.InstrumentSessions)
            {
                Assert.Equal(DmmMeasurementFunction.DCCurrent, sessionInfo.Session.MeasurementFunction);
                // It seems these properties are not configurable for certain devices.
                // Assert.Equal(0.2, sessionInfo.Session.Range);
                // Assert.Equal(0.01, sessionInfo.Session.Resolution);
            }
        }

        [Fact]
        public void ConfigureMeasurementDigits_ValuesConfigured()
        {
            var sessionsBundle = _sessionManager.DMM(new string[] { "DUTPin_4065", "SystemPin_4070", "DUTPin_4081", "SystemPin_4081" });
            sessionsBundle.ConfigureMeasurementDigits(DmmMeasurementFunction.ACVolts, 0.2, 3.5);

            foreach (var sessionInfo in sessionsBundle.InstrumentSessions)
            {
                Assert.Equal(DmmMeasurementFunction.ACVolts, sessionInfo.Session.MeasurementFunction);
                // It seems these properties are not configurable for certain devices.
                // Assert.Equal(0.2, sessionInfo.Session.Range);
                Assert.Equal(3.5, sessionInfo.Session.DigitsResolution);
            }
        }

        [Fact]
        public void ConfigureMultiPoint_ValuesConfigured()
        {
            var sessionsBundle = _sessionManager.DMM(new string[] { "DUTPin_4065", "SystemPin_4070", "DUTPin_4081", "SystemPin_4081" });
            sessionsBundle.ConfigureMultiPoint(triggerCount: 2, sampleCount: 3, sampleTrigger: "Immediate", sampleIntervalInSeconds: 0.1);

            foreach (var sessionInfo in sessionsBundle.InstrumentSessions)
            {
                Assert.Equal(2, sessionInfo.Session.Trigger.MultiPoint.TriggerCount);
                Assert.Equal(3, sessionInfo.Session.Trigger.MultiPoint.SampleCount);
                Assert.Equal("Immediate", sessionInfo.Session.Trigger.MultiPoint.SampleTrigger);
                // It seems sample internal is not configurable.
                // Assert.Equal(0.1, sessionInfo.Session.Trigger.MultiPoint.SampleInterval.TotalSeconds);
            }
        }

        [Fact]
        public void ConfigurePowerlineFrequency_ValuesConfigured()
        {
            var sessionsBundle = _sessionManager.DMM(new string[] { "DUTPin_4065", "SystemPin_4070", "DUTPin_4081", "SystemPin_4081" });
            sessionsBundle.ConfigurePowerlineFrequency(50);

            foreach (var sessionInfo in sessionsBundle.InstrumentSessions)
            {
                Assert.Equal(50, sessionInfo.Session.Advanced.PowerlineFrequency);
            }
        }

        [Fact]
        public void ConfigureTrigger_ValuesConfigured()
        {
            var sessionsBundle = _sessionManager.DMM(new string[] { "DUTPin_4065", "SystemPin_4070", "DUTPin_4081", "SystemPin_4081" });
            sessionsBundle.ConfigureTrigger(DmmTriggerSource.SoftwareTrigger, triggerDelayInSeconds: 0.01);

            foreach (var sessionInfo in sessionsBundle.InstrumentSessions)
            {
                Assert.Equal(DmmTriggerSource.SoftwareTrigger, sessionInfo.Session.Trigger.Source);
                Assert.Equal(0.01, sessionInfo.Session.Trigger.Delay.TotalSeconds);
            }
        }

        [Fact]
        public void SendSoftwareTriggerSucceeds()
        {
            var sessionsBundle = _sessionManager.DMM(new string[] { "DUTPin_4065", "SystemPin_4070", "DUTPin_4081", "SystemPin_4081" });
            sessionsBundle.ConfigureMeasurementDigits();
            sessionsBundle.ReadMultiPoint(1, 1000);
            sessionsBundle.SendSoftwareTrigger();
        }

        [Fact]
        public void ConfigureADCCalibration_ValuesConfigured()
        {
            var sessionsBundle = _sessionManager.DMM(new string[] { "SystemPin_4070", "DUTPin_4081", "SystemPin_4081" });

            sessionsBundle.ConfigureADCCalibration(DmmAdcCalibration.On);
            foreach (var sessionInfo in sessionsBundle.InstrumentSessions)
            {
                Assert.Equal(DmmAdcCalibration.On, sessionInfo.Session.Advanced.AdcCalibration);
            }

            sessionsBundle.ConfigureADCCalibration(DmmAdcCalibration.Off);
            foreach (var sessionInfo in sessionsBundle.InstrumentSessions)
            {
                Assert.Equal(DmmAdcCalibration.Off, sessionInfo.Session.Advanced.AdcCalibration);
            }
        }

        [Fact]
        public void ConfigureADCCalibration_ThrowsExecption()
        {
            var sessionsBundle = _sessionManager.DMM(new string[] { "DUTPin_4065", "SystemPin_4070", "DUTPin_4081", "SystemPin_4081" });

            var exception = Assert.Throws<AggregateException>(() => sessionsBundle.ConfigureADCCalibration(DmmAdcCalibration.On));
            foreach (var innnerException in exception.InnerExceptions)
            {
                Assert.IsType<NIMixedSignalException>(innnerException);
            }
        }

        [Fact]
        public void ConfigureAutoZero_ValuesConfigured()
        {
            var sessionsBundle = _sessionManager.DMM(new string[] { "DUTPin_4065", "SystemPin_4070", "DUTPin_4081", "SystemPin_4081" });
            var modelStrings = new List<string>();
            var values = new List<DmmAuto>();
            // Check default value
            foreach (var sessionInfo in sessionsBundle.InstrumentSessions)
            {
                if (sessionInfo.Session.DriverIdentity.InstrumentModel.Contains("4081"))
                {
                    Assert.Equal(DmmAuto.On, sessionInfo.Session.Advanced.AutoZero);
                }
                else
                {
                    Assert.Equal(DmmAuto.Auto, sessionInfo.Session.Advanced.AutoZero);
                }
            }

            sessionsBundle.ConfigureAutoZero(DmmAuto.On);
            foreach (var sessionInfo in sessionsBundle.InstrumentSessions)
            {
                Assert.Equal(DmmAuto.On, sessionInfo.Session.Advanced.AutoZero);
            }
        }

        [Fact]
        public void ConfigureAutoZero_ThrowsExecption()
        {
            var sessionsBundle = _sessionManager.DMM(new string[] { "DUTPin_4065", "SystemPin_4070", "DUTPin_4081", "SystemPin_4081" });

            // Once not supported on 4065.
            var exception = Assert.Throws<AggregateException>(() => sessionsBundle.ConfigureAutoZero(DmmAuto.Once));
            foreach (var innnerException in exception.InnerExceptions)
            {
                Assert.IsType<NIMixedSignalException>(innnerException);
            }

            // Off not supported on 4065.
            exception = Assert.Throws<AggregateException>(() => sessionsBundle.ConfigureAutoZero(DmmAuto.Off));
            foreach (var innnerException in exception.InnerExceptions)
            {
                Assert.IsType<NIMixedSignalException>(innnerException);
            }
        }
    }
}
