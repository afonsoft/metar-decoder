# TafChunkDecoder Class


\[Missing &lt;summary&gt; documentation for "T:Taf.Decoder.chunkdecoder.TafChunkDecoder"\]



## Definition
**Namespace:** <a href="N_Taf_Decoder_chunkdecoder.md">Taf.Decoder.chunkdecoder</a>  
**Assembly:** Taf.Decoder (in Taf.Decoder.dll) Version: 1.0.2+ad2a158933a1e4d81e31e9a7f1a0faf24fc95eed  
**XMLNS for XAML:** Not mapped to an xmlns.

**C#**
``` C#
public abstract class TafChunkDecoder : ITafChunkDecoder
```

<table><tr><td><strong>Inheritance</strong></td><td><a href="https://learn.microsoft.com/dotnet/api/system.object" target="_blank" rel="noopener noreferrer">Object</a>  →  TafChunkDecoder</td></tr>
<tr><td><strong>Derived</strong></td><td><a href="T_Taf_Decoder_chunkdecoder_CloudChunkDecoder.md">Taf.Decoder.chunkdecoder.CloudChunkDecoder</a><br /><a href="T_Taf_Decoder_chunkdecoder_DatetimeChunkDecoder.md">Taf.Decoder.chunkdecoder.DatetimeChunkDecoder</a><br /><a href="T_Taf_Decoder_chunkdecoder_EvolutionChunkDecoder.md">Taf.Decoder.chunkdecoder.EvolutionChunkDecoder</a><br /><a href="T_Taf_Decoder_chunkdecoder_ForecastPeriodChunkDecoder.md">Taf.Decoder.chunkdecoder.ForecastPeriodChunkDecoder</a><br /><a href="T_Taf_Decoder_chunkdecoder_IcaoChunkDecoder.md">Taf.Decoder.chunkdecoder.IcaoChunkDecoder</a><br /><a href="T_Taf_Decoder_chunkdecoder_ReportTypeChunkDecoder.md">Taf.Decoder.chunkdecoder.ReportTypeChunkDecoder</a><br /><a href="T_Taf_Decoder_chunkdecoder_SurfaceWindChunkDecoder.md">Taf.Decoder.chunkdecoder.SurfaceWindChunkDecoder</a><br /><a href="T_Taf_Decoder_chunkdecoder_TemperatureChunkDecoder.md">Taf.Decoder.chunkdecoder.TemperatureChunkDecoder</a><br /><a href="T_Taf_Decoder_chunkdecoder_VisibilityChunkDecoder.md">Taf.Decoder.chunkdecoder.VisibilityChunkDecoder</a><br /><a href="T_Taf_Decoder_chunkdecoder_WeatherChunkDecoder.md">Taf.Decoder.chunkdecoder.WeatherChunkDecoder</a></td></tr>
<tr><td><strong>Implements</strong></td><td><a href="T_Taf_Decoder_chunkdecoder_ITafChunkDecoder.md">ITafChunkDecoder</a></td></tr>
</table>



## Constructors
<table>
<tr>
<td><a href="M_Taf_Decoder_chunkdecoder_TafChunkDecoder__ctor.md">TafChunkDecoder</a></td>
<td>Initializes a new instance of the TafChunkDecoder class</td></tr>
</table>

## Methods
<table>
<tr>
<td><a href="M_Taf_Decoder_chunkdecoder_TafChunkDecoder_Consume.md">Consume</a></td>
<td>Extract the corresponding chunk from the remaining taf.</td></tr>
<tr>
<td><a href="M_Taf_Decoder_chunkdecoder_TafChunkDecoder_ConsumeOneChunk.md">ConsumeOneChunk</a></td>
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
<td><a href="M_Taf_Decoder_chunkdecoder_TafChunkDecoder_GetRegex.md">GetRegex</a></td>
<td> </td></tr>
<tr>
<td><a href="M_Taf_Decoder_chunkdecoder_TafChunkDecoder_GetResults.md">GetResults</a></td>
<td> </td></tr>
<tr>
<td><a href="https://learn.microsoft.com/dotnet/api/system.object.gettype" target="_blank" rel="noopener noreferrer">GetType</a></td>
<td>Gets the <a href="https://learn.microsoft.com/dotnet/api/system.type" target="_blank" rel="noopener noreferrer">Type</a> of the current instance.<br />(Inherited from <a href="https://learn.microsoft.com/dotnet/api/system.object" target="_blank" rel="noopener noreferrer">Object</a>)</td></tr>
<tr>
<td><a href="https://learn.microsoft.com/dotnet/api/system.object.memberwiseclone" target="_blank" rel="noopener noreferrer">MemberwiseClone</a></td>
<td>Creates a shallow copy of the current <a href="https://learn.microsoft.com/dotnet/api/system.object" target="_blank" rel="noopener noreferrer">Object</a>.<br />(Inherited from <a href="https://learn.microsoft.com/dotnet/api/system.object" target="_blank" rel="noopener noreferrer">Object</a>)</td></tr>
<tr>
<td><a href="M_Taf_Decoder_chunkdecoder_TafChunkDecoder_Parse.md">Parse</a></td>
<td> </td></tr>
<tr>
<td><a href="https://learn.microsoft.com/dotnet/api/system.object.tostring" target="_blank" rel="noopener noreferrer">ToString</a></td>
<td>Returns a string that represents the current object.<br />(Inherited from <a href="https://learn.microsoft.com/dotnet/api/system.object" target="_blank" rel="noopener noreferrer">Object</a>)</td></tr>
</table>

## See Also


#### Reference
<a href="N_Taf_Decoder_chunkdecoder.md">Taf.Decoder.chunkdecoder Namespace</a>  
