using Metar.Decoder.Chunkdecoder;
using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Metar.Decoder
{
    /// <summary>
    /// MetarChunkDecoderException
    /// </summary>
    [Serializable]
    public sealed class MetarChunkDecoderException : Exception
    {
        public string RemainingMetar { get; private set; }
        public string NewRemainingMetar { get; private set; }

        public MetarChunkDecoder ChunkDecoder { get; private set; }

        /// <summary>
        /// MetarChunkDecoderException
        /// </summary>
        public MetarChunkDecoderException()
        {
        }

        /// <summary>
        /// MetarChunkDecoderException
        /// </summary>
        /// <param name="message">message</param>
        public MetarChunkDecoderException(string message) : base(message)
        {
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        private MetarChunkDecoderException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            RemainingMetar = info.GetString("RemainingMetar");
            NewRemainingMetar = info.GetString("NewRemainingMetar");
        }

        /// <summary>
        /// MetarChunkDecoderException
        /// </summary>
        /// <param name="remainingMetar">remainingMetar</param>
        /// <param name="newRemainingMetar">newRemainingMetar</param>
        /// <param name="message">message</param>
        /// <param name="chunkDecoder">chunkDecoder</param>
        public MetarChunkDecoderException(string remainingMetar, string newRemainingMetar, string message, MetarChunkDecoder chunkDecoder) : base(message)
        {
            RemainingMetar = remainingMetar;
            NewRemainingMetar = newRemainingMetar;
            ChunkDecoder = chunkDecoder;
        }

        /// <summary>
        /// GetObjectData
        /// </summary>
        /// <param name="info">info</param>
        /// <param name="context">context</param>
        /// <exception cref="ArgumentNullException"></exception>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }
            info.AddValue("NewRemainingMetar", RemainingMetar);
            info.AddValue("RemainingMetar", NewRemainingMetar);
            base.GetObjectData(info, context);
        }

        public static class Messages
        {
            private const string BadFormatFor = @"Bad format for ";

            //CloudChunkDecoder
            public const string CloudsInformationBadFormat = BadFormatFor + @"clouds information";

            //DatetimeChunkDecoder
            public const string BadDayHourMinuteInformation = @"Missing or badly formatted day/hour/minute information (""ddhhmmZ"" expected)";

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
        }
    }
}