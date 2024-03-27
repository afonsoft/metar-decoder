# MetarChunkDecoderException Class


MetarChunkDecoderException



## Definition
**Namespace:** <a href="N_Metar_Decoder.md">Metar.Decoder</a>  
**Assembly:** Metar.Decoder (in Metar.Decoder.dll) Version: 1.0.5+ad2a158933a1e4d81e31e9a7f1a0faf24fc95eed  
**XMLNS for XAML:** Not mapped to an xmlns.

**C#**
``` C#
[SerializableAttribute]
public sealed class MetarChunkDecoderException : Exception
```

<table><tr><td><strong>Inheritance</strong></td><td><a href="https://learn.microsoft.com/dotnet/api/system.object" target="_blank" rel="noopener noreferrer">Object</a>  →  <a href="https://learn.microsoft.com/dotnet/api/system.exception" target="_blank" rel="noopener noreferrer">Exception</a>  →  MetarChunkDecoderException</td></tr>
</table>



## Constructors
<table>
<tr>
<td><a href="M_Metar_Decoder_MetarChunkDecoderException__ctor.md">MetarChunkDecoderException()</a></td>
<td>MetarChunkDecoderException</td></tr>
<tr>
<td><a href="M_Metar_Decoder_MetarChunkDecoderException__ctor_1.md">MetarChunkDecoderException(String)</a></td>
<td>MetarChunkDecoderException</td></tr>
<tr>
<td><a href="M_Metar_Decoder_MetarChunkDecoderException__ctor_2.md">MetarChunkDecoderException(String, String, String, MetarChunkDecoder)</a></td>
<td>MetarChunkDecoderException</td></tr>
</table>

## Properties
<table>
<tr>
<td><a href="P_Metar_Decoder_MetarChunkDecoderException_ChunkDecoder.md">ChunkDecoder</a></td>
<td> </td></tr>
<tr>
<td><a href="https://learn.microsoft.com/dotnet/api/system.exception.data" target="_blank" rel="noopener noreferrer">Data</a></td>
<td>Gets a collection of key/value pairs that provide additional user-defined information about the exception.<br />(Inherited from <a href="https://learn.microsoft.com/dotnet/api/system.exception" target="_blank" rel="noopener noreferrer">Exception</a>)</td></tr>
<tr>
<td><a href="https://learn.microsoft.com/dotnet/api/system.exception.helplink" target="_blank" rel="noopener noreferrer">HelpLink</a></td>
<td>Gets or sets a link to the help file associated with this exception.<br />(Inherited from <a href="https://learn.microsoft.com/dotnet/api/system.exception" target="_blank" rel="noopener noreferrer">Exception</a>)</td></tr>
<tr>
<td><a href="https://learn.microsoft.com/dotnet/api/system.exception.hresult" target="_blank" rel="noopener noreferrer">HResult</a></td>
<td>Gets or sets HRESULT, a coded numerical value that is assigned to a specific exception.<br />(Inherited from <a href="https://learn.microsoft.com/dotnet/api/system.exception" target="_blank" rel="noopener noreferrer">Exception</a>)</td></tr>
<tr>
<td><a href="https://learn.microsoft.com/dotnet/api/system.exception.innerexception" target="_blank" rel="noopener noreferrer">InnerException</a></td>
<td>Gets the <a href="https://learn.microsoft.com/dotnet/api/system.exception" target="_blank" rel="noopener noreferrer">Exception</a> instance that caused the current exception.<br />(Inherited from <a href="https://learn.microsoft.com/dotnet/api/system.exception" target="_blank" rel="noopener noreferrer">Exception</a>)</td></tr>
<tr>
<td><a href="https://learn.microsoft.com/dotnet/api/system.exception.message" target="_blank" rel="noopener noreferrer">Message</a></td>
<td>Gets a message that describes the current exception.<br />(Inherited from <a href="https://learn.microsoft.com/dotnet/api/system.exception" target="_blank" rel="noopener noreferrer">Exception</a>)</td></tr>
<tr>
<td><a href="P_Metar_Decoder_MetarChunkDecoderException_NewRemainingMetar.md">NewRemainingMetar</a></td>
<td> </td></tr>
<tr>
<td><a href="P_Metar_Decoder_MetarChunkDecoderException_RemainingMetar.md">RemainingMetar</a></td>
<td> </td></tr>
<tr>
<td><a href="https://learn.microsoft.com/dotnet/api/system.exception.source" target="_blank" rel="noopener noreferrer">Source</a></td>
<td>Gets or sets the name of the application or the object that causes the error.<br />(Inherited from <a href="https://learn.microsoft.com/dotnet/api/system.exception" target="_blank" rel="noopener noreferrer">Exception</a>)</td></tr>
<tr>
<td><a href="https://learn.microsoft.com/dotnet/api/system.exception.stacktrace" target="_blank" rel="noopener noreferrer">StackTrace</a></td>
<td>Gets a string representation of the immediate frames on the call stack.<br />(Inherited from <a href="https://learn.microsoft.com/dotnet/api/system.exception" target="_blank" rel="noopener noreferrer">Exception</a>)</td></tr>
<tr>
<td><a href="https://learn.microsoft.com/dotnet/api/system.exception.targetsite" target="_blank" rel="noopener noreferrer">TargetSite</a></td>
<td>Gets the method that throws the current exception.<br />(Inherited from <a href="https://learn.microsoft.com/dotnet/api/system.exception" target="_blank" rel="noopener noreferrer">Exception</a>)</td></tr>
</table>

## Methods
<table>
<tr>
<td><a href="https://learn.microsoft.com/dotnet/api/system.object.equals#system-object-equals(system-object)" target="_blank" rel="noopener noreferrer">Equals</a></td>
<td>Determines whether the specified object is equal to the current object.<br />(Inherited from <a href="https://learn.microsoft.com/dotnet/api/system.object" target="_blank" rel="noopener noreferrer">Object</a>)</td></tr>
<tr>
<td><a href="https://learn.microsoft.com/dotnet/api/system.exception.getbaseexception" target="_blank" rel="noopener noreferrer">GetBaseException</a></td>
<td>When overridden in a derived class, returns the <a href="https://learn.microsoft.com/dotnet/api/system.exception" target="_blank" rel="noopener noreferrer">Exception</a> that is the root cause of one or more subsequent exceptions.<br />(Inherited from <a href="https://learn.microsoft.com/dotnet/api/system.exception" target="_blank" rel="noopener noreferrer">Exception</a>)</td></tr>
<tr>
<td><a href="https://learn.microsoft.com/dotnet/api/system.object.gethashcode" target="_blank" rel="noopener noreferrer">GetHashCode</a></td>
<td>Serves as the default hash function.<br />(Inherited from <a href="https://learn.microsoft.com/dotnet/api/system.object" target="_blank" rel="noopener noreferrer">Object</a>)</td></tr>
<tr>
<td><a href="M_Metar_Decoder_MetarChunkDecoderException_GetObjectData.md">GetObjectData</a></td>
<td>GetObjectData<br />(Overrides <a href="https://learn.microsoft.com/dotnet/api/system.exception.getobjectdata" target="_blank" rel="noopener noreferrer">Exception.GetObjectData(SerializationInfo, StreamingContext)</a>)</td></tr>
<tr>
<td><a href="https://learn.microsoft.com/dotnet/api/system.exception.gettype" target="_blank" rel="noopener noreferrer">GetType</a></td>
<td>Gets the runtime type of the current instance.<br />(Inherited from <a href="https://learn.microsoft.com/dotnet/api/system.exception" target="_blank" rel="noopener noreferrer">Exception</a>)</td></tr>
<tr>
<td><a href="https://learn.microsoft.com/dotnet/api/system.exception.tostring" target="_blank" rel="noopener noreferrer">ToString</a></td>
<td>Creates and returns a string representation of the current exception.<br />(Inherited from <a href="https://learn.microsoft.com/dotnet/api/system.exception" target="_blank" rel="noopener noreferrer">Exception</a>)</td></tr>
</table>

## See Also


#### Reference
<a href="N_Metar_Decoder.md">Metar.Decoder Namespace</a>  
