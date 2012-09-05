<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="PayMeBack" generation="1" functional="0" release="0" Id="1cc71ac7-b94c-409a-b7ba-3b136e237dad" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="PayMeBackGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="Glav.PayMeBack.Web:WebEndpoint" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/PayMeBack/PayMeBackGroup/LB:Glav.PayMeBack.Web:WebEndpoint" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="Glav.PayMeBack.Web:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/PayMeBack/PayMeBackGroup/MapGlav.PayMeBack.Web:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="Glav.PayMeBack.WebInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/PayMeBack/PayMeBackGroup/MapGlav.PayMeBack.WebInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:Glav.PayMeBack.Web:WebEndpoint">
          <toPorts>
            <inPortMoniker name="/PayMeBack/PayMeBackGroup/Glav.PayMeBack.Web/WebEndpoint" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapGlav.PayMeBack.Web:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/PayMeBack/PayMeBackGroup/Glav.PayMeBack.Web/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapGlav.PayMeBack.WebInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/PayMeBack/PayMeBackGroup/Glav.PayMeBack.WebInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="Glav.PayMeBack.Web" generation="1" functional="0" release="0" software="D:\Data\Development\Dev Projects\PayMeBack\PayMeBack\PayMeBack\csx\Debug\roles\Glav.PayMeBack.Web" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="1792" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="WebEndpoint" protocol="http" portRanges="80" />
            </componentports>
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;Glav.PayMeBack.Web&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;Glav.PayMeBack.Web&quot;&gt;&lt;e name=&quot;WebEndpoint&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/PayMeBack/PayMeBackGroup/Glav.PayMeBack.WebInstances" />
            <sCSPolicyFaultDomainMoniker name="/PayMeBack/PayMeBackGroup/Glav.PayMeBack.WebFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyFaultDomain name="Glav.PayMeBack.WebFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="Glav.PayMeBack.WebInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="788ac7d4-9a59-4620-99f1-392dc3ffdc9a" ref="Microsoft.RedDog.Contract\ServiceContract\PayMeBackContract@ServiceDefinition.build">
      <interfacereferences>
        <interfaceReference Id="37da1339-7971-4718-9ab3-1d0595ff087f" ref="Microsoft.RedDog.Contract\Interface\Glav.PayMeBack.Web:WebEndpoint@ServiceDefinition.build">
          <inPort>
            <inPortMoniker name="/PayMeBack/PayMeBackGroup/Glav.PayMeBack.Web:WebEndpoint" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>