# TafDecoder Class


\[Missing &lt;summary&gt; documentation for "T:Taf.Decoder.TafDecoder"\]



## Definition
**Namespace:** <a href="N_Taf_Decoder.md">Taf.Decoder</a>  
**Assembly:** Taf.Decoder (in Taf.Decoder.dll) Version: 1.0.2+ad2a158933a1e4d81e31e9a7f1a0faf24fc95eed  
**XMLNS for XAML:** Not mapped to an xmlns.

**C#**
``` C#
public sealed class TafDecoder
```

<table><tr><td><strong>Inheritance</strong></td><td><a href="https://learn.microsoft.com/dotnet/api/system.object" target="_blank" rel="noopener noreferrer">Object</a>  →  TafDecoder</td></tr>
</table>



## Constructors
<table>
<tr>
<td><a href="M_Taf_Decoder_TafDecoder__ctor.md">TafDecoder</a></td>
<td>Initializes a new instance of the TafDecoder class</td></tr>
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
<td><a href="M_Taf_Decoder_TafDecoder_Parse.md">Parse</a></td>
<td>Decode a full taf string into a complete taf object while using global strict option.</td></tr>
<tr>
<td><a href="M_Taf_Decoder_TafDecoder_ParseNotStrict.md">ParseNotStrict</a></td>
<td>Decode a full taf string into a complete taf object with strict option disabled, meaning that decoding will continue even if taf is not compliant.</td></tr>
<tr>
<td><a href="M_Taf_Decoder_TafDecoder_ParseStrict.md">ParseStrict</a></td>
<td>Decode a full taf string into a complete taf object with strict option, meaning decoding will stop as soon as a non-compliance is detected.</td></tr>
<tr>
<td><a href="M_Taf_Decoder_TafDecoder_ParseWithMode.md">ParseWithMode</a></td>
<td>Decode a full taf string into a complete taf object.</td></tr>
<tr>
<td><a href="M_Taf_Decoder_TafDecoder_SetStrictParsing.md">SetStrictParsing</a></td>
<td>Set global parsing mode (strict/not strict) for the whole object.</td></tr>
<tr>
<td><a href="https://learn.microsoft.com/dotnet/api/system.object.tostring" target="_blank" rel="noopener noreferrer">ToString</a></td>
<td>Returns a string that represents the current object.<br />(Inherited from <a href="https://learn.microsoft.com/dotnet/api/system.object" target="_blank" rel="noopener noreferrer">Object</a>)</td></tr>
</table>

## Fields
<table>
<tr>
<td><a href="F_Taf_Decoder_TafDecoder_ExceptionKey.md">ExceptionKey</a></td>
<td> </td></tr>
<tr>
<td><a href="F_Taf_Decoder_TafDecoder_RemainingTafKey.md">RemainingTafKey</a></td>
<td> </td></tr>
<tr>
<td><a href="F_Taf_Decoder_TafDecoder_ResultKey.md">ResultKey</a></td>
<td> </td></tr>
</table>

## See Also


#### Reference
<a href="N_Taf_Decoder.md">Taf.Decoder Namespace</a>  
