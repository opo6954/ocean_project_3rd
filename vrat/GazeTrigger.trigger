<vrat.TriggerPrimitivesTemplate name="GazeTrigger" type="TriggerPrimitivesTemplate">
  <PairwiseAsset name="OwnedAssetName" type="STRING" contents="" />
  <PossibleAttachedAssetType name="AttachedAssetType" type="STRING" contents="" />
  <CustomizedProperty>
    <vrat.ListOfXmlTemplate name="PropertiesList" type="ListOfXmlTemplate" idx="0">
      <vrat.ListOfXmlTemplate name="TriggerType" type="CHOICE" idx="0">
        <vrat.PrimitiveXmlTemplate name="TriggerType0" type="STRING" contents="GazeEnter" />
        <vrat.PrimitiveXmlTemplate name="TriggerType1" type="STRING" contents="GazeHold" />
        <vrat.PrimitiveXmlTemplate name="TriggerType2" type="STRING" contents="GazeExit" />
      </vrat.ListOfXmlTemplate>
    </vrat.ListOfXmlTemplate>
  </CustomizedProperty>
</vrat.TriggerPrimitivesTemplate>