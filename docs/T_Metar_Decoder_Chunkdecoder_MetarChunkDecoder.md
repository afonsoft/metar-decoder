# MetarChunkDecoder Class


\[Missing &lt;summary&gt; documentation for "T:Metar.Decoder.Chunkdecoder.MetarChunkDecoder"\]



## Definition
**Namespace:** <a href="N_Metar_Decoder_Chunkdecoder.md">Metar.Decoder.Chunkdecoder</a>  
**Assembly:** Metar.Decoder (in Metar.Decoder.dll) Version: 1.0.5+ad2a158933a1e4d81e31e9a7f1a0faf24fc95eed  
**XMLNS for XAML:** Not mapped to an xmlns.

**C#**
``` C#
public abstract class MetarChunkDecoder : IMetarChunkDecoder
```

<table><tr><td><strong>Inheritance</strong></td><td><a href="https://learn.microsoft.com/dotnet/api/system.object" target="_blank" rel="noopener noreferrer">Object</a>  →  MetarChunkDecoder</td></tr>
<tr><td><strong>Derived</strong></td><td><a href="T_Metar_Decoder_Chunkdecoder_CloudChunkDecoder.md">Metar.Decoder.Chunkdecoder.CloudChunkDecoder</a><br /><a href="T_Metar_Decoder_Chunkdecoder_DatetimeChunkDecoder.md">Metar.Decoder.Chunkdecoder.DatetimeChunkDecoder</a><br /><a href="T_Metar_Decoder_Chunkdecoder_IcaoChunkDecoder.md">Metar.Decoder.Chunkdecoder.IcaoChunkDecoder</a><br /><a href="T_Metar_Decoder_Chunkdecoder_PresentWeatherChunkDecoder.md">Metar.Decoder.Chunkdecoder.PresentWeatherChunkDecoder</a><br /><a href="T_Metar_Decoder_Chunkdecoder_PressureChunkDecoder.md">Metar.Decoder.Chunkdecoder.PressureChunkDecoder</a><br /><a href="T_Metar_Decoder_Chunkdecoder_RecentWeatherChunkDecoder.md">Metar.Decoder.Chunkdecoder.RecentWeatherChunkDecoder</a><br /><a href="T_Metar_Decoder_Chunkdecoder_ReportStatusChunkDecoder.md">Metar.Decoder.Chunkdecoder.ReportStatusChunkDecoder</a><br /><a href="T_Metar_Decoder_Chunkdecoder_ReportTypeChunkDecoder.md">Metar.Decoder.Chunkdecoder.ReportTypeChunkDecoder</a><br /><a href="T_Metar_Decoder_Chunkdecoder_RunwayVisualRangeChunkDecoder.md">Metar.Decoder.Chunkdecoder.RunwayVisualRangeChunkDecoder</a><br /><a href="T_Metar_Decoder_Chunkdecoder_SurfaceWindChunkDecoder.md">Metar.Decoder.Chunkdecoder.SurfaceWindChunkDecoder</a><br /><a href="T_Metar_Decoder_Chunkdecoder_TemperatureChunkDecoder.md">Metar.Decoder.Chunkdecoder.TemperatureChunkDecoder</a><br /><a href="T_Metar_Decoder_Chunkdecoder_VisibilityChunkDecoder.md">Metar.Decoder.Chunkdecoder.VisibilityChunkDecoder</a><br /><a href="T_Metar_Decoder_Chunkdecoder_WindShearChunkDecoder.md">Metar.Decoder.Chunkdecoder.WindShearChunkDecoder</a></td></tr>
<tr><td><strong>Implements</strong></td><td><a href="T_Metar_Decoder_Chunkdecoder_IMetarChunkDecoder.md">IMetarChunkDecoder</a></td></tr>
</table>



## Constructors
<table>
<tr>
<td><a href="M_Metar_Decoder_Chunkdecoder_MetarChunkDecoder__ctor.md">MetarChunkDecoder</a></td>
<td>Initializes a new instance of the MetarChunkDecoder class</td></tr>
</table>

## Methods
<table>
<tr>
<td><a href="M_Metar_Decoder_Chunkdecoder_MetarChunkDecoder_Consume.md">Consume</a></td>
<td>Extract the corresponding chunk from the remaining metar.</td></tr>
<tr>
<td><a href="M_Metar_Decoder_Chunkdecoder_MetarChunkDecoder_ConsumeOneChunk.md">ConsumeOneChunk</a></td>
<td>Consume one chunk blindly, without looking for the specific pattern (only whitespace).</td></tr>
<tr>
<td><a href="https://learn.microsoft.com/dotnet/api/system.object.equals#system-object-equals(system-object)" target="_blank" rel="noopener noreferrer">Equals</a></td>
<td>Determines whether the specified object is equal to the current object.<br />(Inherited from <a href="https://learn.microsoft.com/dotnet/api/system.object" target="_blank" rel="noopener noreferrer">Object</a>)</td></tr>
<tr>
<td><a href="https://learn.microsoft.com/dotnet/api/system.object.finalize" target="_blank" rel="noopener noreferrer">Finalize</a></td>
<td>Allows an object to try to free resources and perform other cleanup operations before it is reclaimed by garbage collection.<br />(Inherited from <a href="https://learn.microsoft.com/dotnet/api/system.object" target="_blank" rel="noopener noreferrer">Object</a>)</td></tr>
<tr>
<td><a href="https://learn.microsoft.com/dotnet/api/system.object.gethashcode" target="_blank" rel="noopener noreferrer">GetHashCode</a></td>
<td>Serves as the default hash function.<br />(Inherited from <a href="https://learn.microsoft.com/dotnet/api/system.object" target="_blank" rel="noopener noreferrer">Object</a>)</td></tr>
<tr>
<td><a href="M_Metar_Decoder_Chunkdecoder_MetarChunkDecoder_GetRegex.md">GetRegex</a></td>
<td>GetRegex</td></tr>
<tr>
<td><a href="M_Metar_Decoder_Chunkdecoder_MetarChunkDecoder_GetResults.md">GetResults</a></td>
<td>GetResults</td></tr>
<tr>
<td><a href="https://learn.microsoft.com/dotnet/api/system.object.gettype" target="_blank" rel="noopener noreferrer">GetType</a></td>
<td>Gets the <a href="https://learn.microsoft.com/dotnet/api/system.type" target="_blank" rel="noopener noreferrer">Type</a> of the current instance.<br />(Inherited from <a href="https://learn.microsoft.com/dotnet/api/system.object" target="_blank" rel="noopener noreferrer">Object</a>)</td></tr>
<tr>
<td><a href="https://learn.microsoft.com/dotnet/api/system.object.memberwiseclone" target="_blank" rel="noopener noreferrer">MemberwiseClone</a></td>
<td>Creates a shallow copy of the current <a href="https://learn.microsoft.com/dotnet/api/system.object" target="_blank" rel="noopener noreferrer">Object</a>.<br />(Inherited from <a href="https://learn.microsoft.com/dotnet/api/system.object" target="_blank" rel="noopener noreferrer">Object</a>)</td></tr>
<tr>
<td><a href="M_Metar_Decoder_Chunkdecoder_MetarChunkDecoder_Parse.md">Parse</a></td>
<td>Parse</td></tr>
<tr>
<td><a href="https://learn.microsoft.com/dotnet/api/system.object.tostring" target="_blank" rel="noopener noreferrer">ToString</a></td>
<td>Returns a string that represents the current object.<br />(Inherited from <a href="https://learn.microsoft.com/dotnet/api/system.object" target="_blank" rel="noopener noreferrer">Object</a>)</td></tr>
</table>

## See Also


#### Reference
<a href="N_Metar_Decoder_Chunkdecoder.md">Metar.Decoder.Chunkdecoder Namespace</a>  
