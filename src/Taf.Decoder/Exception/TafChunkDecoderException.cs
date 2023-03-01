using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Taf.Decoder.chunkdecoder;

namespace Taf.Decoder
{
    [Serializable]
    public sealed class TafChunkDecoderException : Exception
    {
        public string RemainingTaf { get; private set; }
        public string NewRemainingTaf { get; private set; }

        public TafChunkDecoder ChunkDecoder { get; private set; }

        public TafChunkDecoderException()
        {
        }

        public TafChunkDecoderException(string message) : base(message)
        {
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        private TafChunkDecoderException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            RemainingTaf = info.GetString("RemainingTaf");
            NewRemainingTaf = info.GetString("NewRemainingTaf");
        }

        public TafChunkDecoderException(string remainingTaf, string newRemainingTaf, string message, TafChunkDecoder chunkDecoder) : base(message)
        {
            RemainingTaf = remainingTaf;
            NewRemainingTaf = newRemainingTaf;
            ChunkDecoder = chunkDecoder;
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }
            info.AddValue("NewRemainingTaf", RemainingTaf);
            info.AddValue("RemainingTaf", NewRemainingTaf);
            base.GetObjectData(info, context);
        }

        public static class Messages
        {
            private const string BadFormatFor = @"Bad format for ";
            private const string MissingOrBadFormatFor = "Missing or badly formatted ";

            //CloudChunkDecoder
            public const string CloudsInformationBadFormat = BadFormatFor + @"clouds information";

            //DatetimeChunkDecoder
            public const string BadDayHourMinuteInformation = MissingOrBadFormatFor + @"day/hour/minute information (""ddhhmmZ"" expected)";

            public const string InvalidDayHourMinuteRanges = @"Invalid values for day/hour/minute";

            //IcaoChunkDecoder
            public const string ICAONotFound = @"Station ICAO code not found (4 char expected)";

            //PressureChunkDecoder
            public const string AtmosphericPressureNotFound = @"Atmospheric pressure not found";

            //ReportStatusChunkDecoder
            public const string InvalidReportStatus = @"Invalid report status, expecting AUTO, NIL, or any other 3 letter word";

            public const string NoInformationExpectedAfterNILStatus = @"No information expected after NIL status";

            //RunwayVisualRangeChunkDecoder
            public const string InvalidRunwayQFURunwayVisualRangeInformation = @"Invalid runway QFU runway visual range information";

            //SurfaceWindChunkDecoder
            public const string SurfaceWindInformationBadFormat = BadFormatFor + @"surface wind information";

            public const string NoSurfaceWindInformationMeasured = @"No information measured for surface wind";
            public const string InvalidWindDirectionInterval = @"Wind direction should be in [0,360]";
            public const string InvalidWindDirectionVariationsInterval = @"Wind direction variations should be in [0,360]";

            //VisibilityChunkDecoder
            public const string ForVisibilityInformationBadFormat = BadFormatFor + @"visibility information";

            //WindShearChunkDecoder
            public const string InvalidRunwayQFURunwaVisualRangeInformation = @"Invalid runway QFU runway visual range information";

            //ForecastPeriodChunkDecoder
            public const string InvalidForecastPeriodInformation = @"forecast period information (""ddhh/ddhh"" expected)";

            public const string InvalidValuesForTheForecastPeriod = @"Invalid values for the forecast period";

            public const string InconsistentValuesForTemperatureInformation = @"Inconsistent values for temperature information";

            //EvolutionChunkDecoder
            public const string WeatherEvolutionBadFormat = BadFormatFor + @"weather evolution";

            public const string EvolutionInformationBadFormat = BadFormatFor + @"evolution information";

            public const string UnknownEntity = @"Unknown entity: ";
        }
    }
}