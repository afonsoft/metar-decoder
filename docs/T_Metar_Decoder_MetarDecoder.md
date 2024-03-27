# MetarDecoder Class


MetarDecoder



## Definition
**Namespace:** <a href="N_Metar_Decoder.md">Metar.Decoder</a>  
**Assembly:** Metar.Decoder (in Metar.Decoder.dll) Version: 1.0.5+ad2a158933a1e4d81e31e9a7f1a0faf24fc95eed  
**XMLNS for XAML:** Not mapped to an xmlns.

**C#**
``` C#
public sealed class MetarDecoder
```

<table><tr><td><strong>Inheritance</strong></td><td><a href="https://learn.microsoft.com/dotnet/api/system.object" target="_blank" rel="noopener noreferrer">Object</a>  →  MetarDecoder</td></tr>
</table>



## Constructors
<table>
<tr>
<td><a href="M_Metar_Decoder_MetarDecoder__ctor.md">MetarDecoder</a></td>
<td>Initializes a new instance of the MetarDecoder class</td></tr>
</table>

## Methods
<table>
<tr>
<td><a href="https://learn.microsoft.com/dotnet/api/system.object.equals#system-object-equals(system-object)" target="_blank" rel="noopener noreferrer">Equals</a></td>
<td>Determines whether the specified object is equal to the current object.<br />(Inherited from <a href="https://learn.microsoft.com/dotnet/api/system.object" target="_blank" rel="noopener noreferrer">Object</a>)</td></tr>
<tr>
<td><a href="https://learn.microsoft.com/dotnet/api/system.object.gethashcode" target="_blank" rel="noopener noreferrer">GetHashCode</a></td>
<td>Serves as the default hash function.<br />(Inherited from <a href="https://learn.microsoft.com/dotnet/api/system.object" target="_blank" rel="noopener noreferrer">Object</a>)</td></tr>
<tr>
<td><a href="https://learn.microsoft.com/dotnet/api/system.object.gettype" target="_blank" rel="noopener noreferrer">GetType</a></td>
<td>Gets the <a href="https://learn.microsoft.com/dotnet/api/system.type" target="_blank" rel="noopener noreferrer">Type</a> of the current instance.<br />(Inherited from <a href="https://learn.microsoft.com/dotnet/api/system.object" target="_blank" rel="noopener noreferrer">Object</a>)</td></tr>
<tr>
<td><a href="M_Metar_Decoder_MetarDecoder_Parse.md">Parse</a></td>
<td>Decode a full metar string into a complete metar object while using global strict option.</td></tr>
<tr>
<td><a href="M_Metar_Decoder_MetarDecoder_ParseNotStrict.md">ParseNotStrict</a></td>
<td>Decode a full metar string into a complete metar object ith strict option disabled, meaning that decoding will continue even if metar is not compliant.</td></tr>
<tr>
<td><a href="M_Metar_Decoder_MetarDecoder_ParseStrict.md">ParseStrict</a></td>
<td>Decode a full metar string into a complete metar object with strict option, meaning decoding will stop as soon as a non-compliance is detected.</td></tr>
<tr>
<td><a href="M_Metar_Decoder_MetarDecoder_ParseWithMode.md">ParseWithMode</a></td>
<td>Decode a full metar string into a complete metar object.</td></tr>
<tr>
<td><a href="M_Metar_Decoder_MetarDecoder_SetStrictParsing.md">SetStrictParsing</a></td>
<td>Set global parsing mode (strict/not strict) for the whole object.</td></tr>
<tr>
<td><a href="https://learn.microsoft.com/dotnet/api/system.object.tostring" target="_blank" rel="noopener noreferrer">ToString</a></td>
<td>Returns a string that represents the current object.<br />(Inherited from <a href="https://learn.microsoft.com/dotnet/api/system.object" target="_blank" rel="noopener noreferrer">Object</a>)</td></tr>
</table>

## Fields
<table>
<tr>
<td><a href="F_Metar_Decoder_MetarDecoder_ExceptionKey.md">ExceptionKey</a></td>
<td> </td></tr>
<tr>
<td><a href="F_Metar_Decoder_MetarDecoder_RemainingMetarKey.md">RemainingMetarKey</a></td>
<td> </td></tr>
<tr>
<td><a href="F_Metar_Decoder_MetarDecoder_ResultKey.md">ResultKey</a></td>
<td> </td></tr>
</table>

## See Also


#### Reference
<a href="N_Metar_Decoder.md">Metar.Decoder Namespace</a>  
