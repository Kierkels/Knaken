param location string = resourceGroup().location
param webAppName string
param environmentName string


// App Service Plan
resource appServicePlan 'Microsoft.Web/serverfarms@2022-09-01' = {
  name: '${webAppName}-${environmentName}-plan'
  location: location
  sku: {
    name: 'B1'
    tier: 'Basic'
  }
}

// Application Insights
resource appInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: '${webAppName}-${environmentName}-ai'
  location: location
  kind: 'web'
  properties: {
    Application_Type: 'web'
    Request_Source: 'IbizaWebAppGallery'
    publicNetworkAccessForIngestion: 'Enabled'
    publicNetworkAccessForQuery: 'Enabled'
  }
}


// Web App
resource webApp 'Microsoft.Web/sites@2022-09-01' = {
  name: '${webAppName}-${environmentName}'
  location: location
  properties: {
    serverFarmId: appServicePlan.id
    httpsOnly: true
    siteConfig: {
      netFrameworkVersion: 'v9.0'
      windowsFxVersion: 'DOTNETCORE|9.0'
      appSettings: [
          {
            name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
            value: appInsights.properties.ConnectionString
          }
          {
            name: 'ApplicationInsightsAgent_EXTENSION_VERSION'
            value: '~3'
          }
        ]
    }
  }
}

output appName string = webApp.name
output appUrl string = 'https://${webApp.properties.defaultHostName}'
output appInsightsInstrumentationKey string = appInsights.properties.InstrumentationKey
output appInsightsConnectionString string = appInsights.properties.ConnectionString
