# DecodedTaf Class


\[Missing &lt;summary&gt; documentation for "T:Taf.Decoder.entity.DecodedTaf"\]



## Definition
**Namespace:** <a href="N_Taf_Decoder_entity.md">Taf.Decoder.entity</a>  
**Assembly:** Taf.Decoder (in Taf.Decoder.dll) Version: 1.0.2+ad2a158933a1e4d81e31e9a7f1a0faf24fc95eed  
**XMLNS for XAML:** Not mapped to an xmlns.

**C#**
``` C#
public sealed class DecodedTaf : AbstractEntity
```

<table><tr><td><strong>Inheritance</strong></td><td><a href="https://learn.microsoft.com/dotnet/api/system.object" target="_blank" rel="noopener noreferrer">Object</a>  →  <a href="T_Taf_Decoder_entity_AbstractEntity.md">AbstractEntity</a>  →  DecodedTaf</td></tr>
</table>



## Properties
<table>
<tr>
<td><a href="P_Taf_Decoder_entity_DecodedTaf_Cavok.md">Cavok</a></td>
<td> </td></tr>
<tr>
<td><a href="P_Taf_Decoder_entity_DecodedTaf_Clouds.md">Clouds</a></td>
<td>Cloud layers information</td></tr>
<tr>
<td><a href="P_Taf_Decoder_entity_DecodedTaf_Day.md">Day</a></td>
<td>Day of origin</td></tr>
<tr>
<td><a href="P_Taf_Decoder_entity_DecodedTaf_DecodingExceptions.md">DecodingExceptions</a></td>
<td>If the decoded taf is invalid, get all the exceptions that occurred during decoding Note that in strict mode, only the first encountered exception will be reported as parsing stops on error Else return null;.</td></tr>
<tr>
<td><a href="P_Taf_Decoder_entity_AbstractEntity_Evolutions.md">Evolutions</a></td>
<td>An evolution can contain embedded evolutions with different probabilities<br />(Inherited from <a href="T_Taf_Decoder_entity_AbstractEntity.md">AbstractEntity</a>)</td></tr>
<tr>
<td><a href="P_Taf_Decoder_entity_DecodedTaf_ForecastPeriod.md">ForecastPeriod</a></td>
<td>Forecast period</td></tr>
<tr>
<td><a href="P_Taf_Decoder_entity_DecodedTaf_Icao.md">Icao</a></td>
<td>ICAO code of the airport where the forecast has been made</td></tr>
<tr>
<td><a href="P_Taf_Decoder_entity_DecodedTaf_IsValid.md">IsValid</a></td>
<td>Check if the decoded taf is valid, i.e. if there was no error during decoding.</td></tr>
<tr>
<td><a href="P_Taf_Decoder_entity_DecodedTaf_MaximumTemperature.md">MaximumTemperature</a></td>
<td>Temperature information</td></tr>
<tr>
<td><a href="P_Taf_Decoder_entity_DecodedTaf_MinimumTemperature.md">MinimumTemperature</a></td>
<td>Temperature information</td></tr>
<tr>
<td><a href="P_Taf_Decoder_entity_DecodedTaf_RawTaf.md">RawTaf</a></td>
<td>Raw TAF</td></tr>
<tr>
<td><a href="P_Taf_Decoder_entity_DecodedTaf_Status.md">Status</a></td>
<td>Report status (AUTO or NIL)</td></tr>
<tr>
<td><a href="P_Taf_Decoder_entity_DecodedTaf_SurfaceWind.md">SurfaceWind</a></td>
<td>Surface wind information</td></tr>
<tr>
<td><a href="P_Taf_Decoder_entity_DecodedTaf_Time.md">Time</a></td>
<td>Time of origin, as string</td></tr>
<tr>
<td><a href="P_Taf_Decoder_entity_DecodedTaf_Type.md">Type</a></td>
<td>Report type</td></tr>
<tr>
<td><a href="P_Taf_Decoder_entity_DecodedTaf_Visibility.md">Visibility</a></td>
<td>Visibility information</td></tr>
<tr>
<td><a href="P_Taf_Decoder_entity_DecodedTaf_WeatherPhenomenons.md">WeatherPhenomenons</a></td>
<td>Weather phenomenon</td></tr>
</table>

## Methods
<table>
<tr>
<td><a href="M_Taf_Decoder_entity_DecodedTaf_AddDecodingException.md">AddDecodingException</a></td>
<td>Add an exception that occured during taf decoding.</td></tr>
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
<td><a href="M_Taf_Decoder_entity_DecodedTaf_ResetDecodingExceptions.md">ResetDecodingExceptions</a></td>
<td>Reset the whole list of Decoding Exceptions</td></tr>
<tr>
<td><a href="https://learn.microsoft.com/dotnet/api/system.object.tostring" target="_blank" rel="noopener noreferrer">ToString</a></td>
<td>Returns a string that represents the current object.<br />(Inherited from <a href="https://learn.microsoft.com/dotnet/api/system.object" target="_blank" rel="noopener noreferrer">Object</a>)</td></tr>
</table>

## See Also


#### Reference
<a href="N_Taf_Decoder_entity.md">Taf.Decoder.entity Namespace</a>  
