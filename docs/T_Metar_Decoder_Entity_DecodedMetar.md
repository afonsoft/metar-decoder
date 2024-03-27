# DecodedMetar Class


\[Missing &lt;summary&gt; documentation for "T:Metar.Decoder.Entity.DecodedMetar"\]



## Definition
**Namespace:** <a href="N_Metar_Decoder_Entity.md">Metar.Decoder.Entity</a>  
**Assembly:** Metar.Decoder (in Metar.Decoder.dll) Version: 1.0.5+ad2a158933a1e4d81e31e9a7f1a0faf24fc95eed  
**XMLNS for XAML:** Not mapped to an xmlns.

**C#**
``` C#
public sealed class DecodedMetar
```

<table><tr><td><strong>Inheritance</strong></td><td><a href="https://learn.microsoft.com/dotnet/api/system.object" target="_blank" rel="noopener noreferrer">Object</a>  →  DecodedMetar</td></tr>
</table>



## Properties
<table>
<tr>
<td><a href="P_Metar_Decoder_Entity_DecodedMetar_AirTemperature.md">AirTemperature</a></td>
<td>Temperature information</td></tr>
<tr>
<td><a href="P_Metar_Decoder_Entity_DecodedMetar_Cavok.md">Cavok</a></td>
<td> </td></tr>
<tr>
<td><a href="P_Metar_Decoder_Entity_DecodedMetar_Clouds.md">Clouds</a></td>
<td>Cloud layers information</td></tr>
<tr>
<td><a href="P_Metar_Decoder_Entity_DecodedMetar_Day.md">Day</a></td>
<td>Day of this observation</td></tr>
<tr>
<td><a href="P_Metar_Decoder_Entity_DecodedMetar_DecodingExceptions.md">DecodingExceptions</a></td>
<td>If the decoded metar is invalid, get all the exceptions that occurred during decoding Note that in strict mode, only the first encountered exception will be reported as parsing stops on error Else return null;.</td></tr>
<tr>
<td><a href="P_Metar_Decoder_Entity_DecodedMetar_DewPointTemperature.md">DewPointTemperature</a></td>
<td>Temperature information</td></tr>
<tr>
<td><a href="P_Metar_Decoder_Entity_DecodedMetar_ICAO.md">ICAO</a></td>
<td>ICAO code of the airport where the observation has been made</td></tr>
<tr>
<td><a href="P_Metar_Decoder_Entity_DecodedMetar_IsValid.md">IsValid</a></td>
<td>Check if the decoded metar is valid, i.e. if there was no error during decoding.</td></tr>
<tr>
<td><a href="P_Metar_Decoder_Entity_DecodedMetar_PresentWeather.md">PresentWeather</a></td>
<td>Present weather</td></tr>
<tr>
<td><a href="P_Metar_Decoder_Entity_DecodedMetar_Pressure.md">Pressure</a></td>
<td>Pressure information</td></tr>
<tr>
<td><a href="P_Metar_Decoder_Entity_DecodedMetar_RawMetar.md">RawMetar</a></td>
<td>Raw METAR</td></tr>
<tr>
<td><a href="P_Metar_Decoder_Entity_DecodedMetar_RecentWeather.md">RecentWeather</a></td>
<td>Recent weather</td></tr>
<tr>
<td><a href="P_Metar_Decoder_Entity_DecodedMetar_RunwaysVisualRange.md">RunwaysVisualRange</a></td>
<td>Runway visual range information</td></tr>
<tr>
<td><a href="P_Metar_Decoder_Entity_DecodedMetar_Status.md">Status</a></td>
<td>Report status (AUTO or NIL)</td></tr>
<tr>
<td><a href="P_Metar_Decoder_Entity_DecodedMetar_SurfaceWind.md">SurfaceWind</a></td>
<td>Surface wind information</td></tr>
<tr>
<td><a href="P_Metar_Decoder_Entity_DecodedMetar_Time.md">Time</a></td>
<td>Time of the observation, as a string</td></tr>
<tr>
<td><a href="P_Metar_Decoder_Entity_DecodedMetar_Type.md">Type</a></td>
<td>Report type (METAR, METAR COR or SPECI)</td></tr>
<tr>
<td><a href="P_Metar_Decoder_Entity_DecodedMetar_Visibility.md">Visibility</a></td>
<td>Visibility information</td></tr>
<tr>
<td><a href="P_Metar_Decoder_Entity_DecodedMetar_WindshearAllRunways.md">WindshearAllRunways</a></td>
<td>Windshear runway information (which runways, or "all")</td></tr>
<tr>
<td><a href="P_Metar_Decoder_Entity_DecodedMetar_WindshearRunways.md">WindshearRunways</a></td>
<td>Windshear runway information (which runways, or "all")</td></tr>
</table>

## Methods
<table>
<tr>
<td><a href="M_Metar_Decoder_Entity_DecodedMetar_AddDecodingException.md">AddDecodingException</a></td>
<td>Add an exception that occured during metar decoding.</td></tr>
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
<td><a href="M_Metar_Decoder_Entity_DecodedMetar_ResetDecodingExceptions.md">ResetDecodingExceptions</a></td>
<td>Reset the whole list of Decoding Exceptions</td></tr>
<tr>
<td><a href="https://learn.microsoft.com/dotnet/api/system.object.tostring" target="_blank" rel="noopener noreferrer">ToString</a></td>
<td>Returns a string that represents the current object.<br />(Inherited from <a href="https://learn.microsoft.com/dotnet/api/system.object" target="_blank" rel="noopener noreferrer">Object</a>)</td></tr>
</table>

## See Also


#### Reference
<a href="N_Metar_Decoder_Entity.md">Metar.Decoder.Entity Namespace</a>  
