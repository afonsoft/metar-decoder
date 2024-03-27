# Evolution Class


\[Missing &lt;summary&gt; documentation for "T:Taf.Decoder.entity.Evolution"\]



## Definition
**Namespace:** <a href="N_Taf_Decoder_entity.md">Taf.Decoder.entity</a>  
**Assembly:** Taf.Decoder (in Taf.Decoder.dll) Version: 1.0.2+ad2a158933a1e4d81e31e9a7f1a0faf24fc95eed  
**XMLNS for XAML:** Not mapped to an xmlns.

**C#**
``` C#
public class Evolution : AbstractEntity, ICloneable
```

<table><tr><td><strong>Inheritance</strong></td><td><a href="https://learn.microsoft.com/dotnet/api/system.object" target="_blank" rel="noopener noreferrer">Object</a>  →  <a href="T_Taf_Decoder_entity_AbstractEntity.md">AbstractEntity</a>  →  Evolution</td></tr>
<tr><td><strong>Implements</strong></td><td><a href="https://learn.microsoft.com/dotnet/api/system.icloneable" target="_blank" rel="noopener noreferrer">ICloneable</a></td></tr>
</table>



## Constructors
<table>
<tr>
<td><a href="M_Taf_Decoder_entity_Evolution__ctor.md">Evolution</a></td>
<td>Initializes a new instance of the Evolution class</td></tr>
</table>

## Properties
<table>
<tr>
<td><a href="P_Taf_Decoder_entity_Evolution_Cavok.md">Cavok</a></td>
<td>weather entity (i.e. SurfaceWind, Temperature, Visibility, etc.) public Entity entity { get; set; }</td></tr>
<tr>
<td><a href="P_Taf_Decoder_entity_Evolution_Entity.md">Entity</a></td>
<td> </td></tr>
<tr>
<td><a href="P_Taf_Decoder_entity_AbstractEntity_Evolutions.md">Evolutions</a></td>
<td>An evolution can contain embedded evolutions with different probabilities<br />(Inherited from <a href="T_Taf_Decoder_entity_AbstractEntity.md">AbstractEntity</a>)</td></tr>
<tr>
<td><a href="P_Taf_Decoder_entity_Evolution_FromDay.md">FromDay</a></td>
<td>day when the evolution occurs (FM) or starts (BECMG/TEMPO)</td></tr>
<tr>
<td><a href="P_Taf_Decoder_entity_Evolution_FromTime.md">FromTime</a></td>
<td>hour and minute UTC (as string) when the evolution occurs (FM) or hour UTC (as string) when the evolution starts (BECMG/TEMPO)</td></tr>
<tr>
<td><a href="P_Taf_Decoder_entity_Evolution_Probability.md">Probability</a></td>
<td>optional annotation corresponding to the probability (PROBnn)</td></tr>
<tr>
<td><a href="P_Taf_Decoder_entity_Evolution_ToDay.md">ToDay</a></td>
<td>day when the evolution ends (BECMG/tEMPO)</td></tr>
<tr>
<td><a href="P_Taf_Decoder_entity_Evolution_ToTime.md">ToTime</a></td>
<td>hour UTC (as string) when the evolution ends (BECMG/TEMPO)</td></tr>
<tr>
<td><a href="P_Taf_Decoder_entity_Evolution_Type.md">Type</a></td>
<td>annotation corresponding to the type of evolution (FM, BECMG or TEMPO)</td></tr>
</table>

## Methods
<table>
<tr>
<td><a href="M_Taf_Decoder_entity_Evolution_Clone.md">Clone</a></td>
<td> </td></tr>
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
<td><a href="https://learn.microsoft.com/dotnet/api/system.object.gettype" target="_blank" rel="noopener noreferrer">GetType</a></td>
<td>Gets the <a href="https://learn.microsoft.com/dotnet/api/system.type" target="_blank" rel="noopener noreferrer">Type</a> of the current instance.<br />(Inherited from <a href="https://learn.microsoft.com/dotnet/api/system.object" target="_blank" rel="noopener noreferrer">Object</a>)</td></tr>
<tr>
<td><a href="https://learn.microsoft.com/dotnet/api/system.object.memberwiseclone" target="_blank" rel="noopener noreferrer">MemberwiseClone</a></td>
<td>Creates a shallow copy of the current <a href="https://learn.microsoft.com/dotnet/api/system.object" target="_blank" rel="noopener noreferrer">Object</a>.<br />(Inherited from <a href="https://learn.microsoft.com/dotnet/api/system.object" target="_blank" rel="noopener noreferrer">Object</a>)</td></tr>
<tr>
<td><a href="https://learn.microsoft.com/dotnet/api/system.object.tostring" target="_blank" rel="noopener noreferrer">ToString</a></td>
<td>Returns a string that represents the current object.<br />(Inherited from <a href="https://learn.microsoft.com/dotnet/api/system.object" target="_blank" rel="noopener noreferrer">Object</a>)</td></tr>
</table>

## See Also


#### Reference
<a href="N_Taf_Decoder_entity.md">Taf.Decoder.entity Namespace</a>  
