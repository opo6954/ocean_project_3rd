<vrat.TriggerPrimitivesTemplate name="MonitorDistanceCaptured" type="TriggerPrimitivesTemplate">
  <PairwiseAsset name="OwnedAssetName" type="STRING" contents="" />
  <PossibleAttachedAssetType name="AttachedAssetType" type="STRING" contents="" />
  <CustomizedProperty>
    <vrat.ListOfXmlTemplate name="PropertiesList" type="ListOfXmlTemplate" idx="0">
      <vrat.ListOfXmlTemplate name="TriggerType" type="CHOICE" idx="0">
        <vrat.PrimitiveXmlTemplate name="TriggerType0" type="STRING" contents="MonitorValueCaptured" />
      </vrat.ListOfXmlTemplate>
      <vrat.ListOfXmlTemplate name="TriggerOperator" type="CHOICE" idx="0">
        <vrat.PrimitiveXmlTemplate name="TriggerOperator0" type="STRING" contents="Larger" />
        <vrat.PrimitiveXmlTemplate name="TriggerOperator1" type="STRING" contents="LargerOrEqual" />
        <vrat.PrimitiveXmlTemplate name="TriggerOperator2" type="STRING" contents="Equal" />
        <vrat.PrimitiveXmlTemplate name="TriggerOperator3" type="STRING" contents="SmallerOrEqual" />
        <vrat.PrimitiveXmlTemplate name="TriggerOperator4" type="STRING" contents="Smaller" />
      </vrat.ListOfXmlTemplate>
      <vrat.PrimitiveXmlTemplate name="Threshold" type="FLOAT" contents="0" />
    </vrat.ListOfXmlTemplate>
  </CustomizedProperty>
</vrat.TriggerPrimitivesTemplate>