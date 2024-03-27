# Evolution Properties




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

## See Also


#### Reference
<a href="T_Taf_Decoder_entity_Evolution.md">Evolution Class</a>  
<a href="N_Taf_Decoder_entity.md">Taf.Decoder.entity Namespace</a>  
