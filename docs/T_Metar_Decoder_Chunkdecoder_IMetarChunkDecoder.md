# IMetarChunkDecoder Interface


\[Missing &lt;summary&gt; documentation for "T:Metar.Decoder.Chunkdecoder.IMetarChunkDecoder"\]



## Definition
**Namespace:** <a href="N_Metar_Decoder_Chunkdecoder.md">Metar.Decoder.Chunkdecoder</a>  
**Assembly:** Metar.Decoder (in Metar.Decoder.dll) Version: 1.0.5+ad2a158933a1e4d81e31e9a7f1a0faf24fc95eed  
**XMLNS for XAML:** Not mapped to an xmlns.

**C#**
``` C#
public interface IMetarChunkDecoder
```



## Methods
<table>
<tr>
<td><a href="M_Metar_Decoder_Chunkdecoder_IMetarChunkDecoder_GetRegex.md">GetRegex</a></td>
<td>Get the regular expression that will be used by chunk decoder Each chunk decoder must declare its own.</td></tr>
<tr>
<td><a href="M_Metar_Decoder_Chunkdecoder_IMetarChunkDecoder_Parse.md">Parse</a></td>
<td>Decode the chunk targeted by the chunk decoder and returns the decoded information and the remaining metar without this chunk.</td></tr>
</table>

## See Also


#### Reference
<a href="N_Metar_Decoder_Chunkdecoder.md">Metar.Decoder.Chunkdecoder Namespace</a>  
