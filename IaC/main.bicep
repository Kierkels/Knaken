param location string = resourceGroup().location
param webAppName string

@allowed([
  'Production'
])
param environmentName string
param sqlAdminLogin string
@secure()
param sqlAdminPassword string

// Key Vault
resource keyVault 'Microsoft.KeyVault/vaults@2023-07-01' = {
  name: '${webAppName}-${environmentName}-kv'
  location: location
  properties: {
    enabledForTemplateDeployment: true
    enableRbacAuthorization: true
    tenantId: tenant().tenantId
    sku: {
      family: 'A'
      name: 'standard'
    }
  }
}

// Managed Identity for Web App
// Creates a Managed Identity that allows the web app to securely access Azure services 
// (like Key Vault and SQL Database) without managing passwords or connection strings.
// Azure automatically handles all the security and credential management.
resource webAppIdentity 'Microsoft.ManagedIdentity/userAssignedIdentities@2023-01-31' = {
  name: '${webAppName}-${environmentName}-id'
  location: location
}


// App Service Plan
// Defines the hosting plan that specifies the compute resources and scale for the web app.
// Using Basic (B1) tier which provides dedicated compute resources suitable for development
// and light production workloads. Controls the pricing, performance, and features available
// to the hosted web application.
resource appServicePlan 'Microsoft.Web/serverfarms@2022-09-01' = {
  name: '${webAppName}-${environmentName}-plan'
  location: location
  sku: {
    name: 'B1'
    tier: 'Basic'
  }
}

// Application Insights
// Creates an Application Insights instance for monitoring and analyzing the web application.
// Provides detailed performance tracking, error logging, user behavior analytics, and 
// real-time monitoring capabilities. Helps identify and diagnose issues, track usage patterns,
// and ensure the application runs optimally.
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

// SQL Server
resource sqlServer 'Microsoft.Sql/servers@2022-05-01-preview' = {
  name: '${webAppName}-${environmentName}-sql'
  location: location
  properties: {
    administratorLogin: sqlAdminLogin
    administratorLoginPassword: sqlAdminPassword
    version: '12.0'
  }
}

// SQL Database
resource sqlDatabase 'Microsoft.Sql/servers/databases@2022-05-01-preview' = {
  parent: sqlServer
  name: '${webAppName}-${environmentName}-db'
  location: location
  sku: {
    name: 'Basic'
    tier: 'Basic'
  }
}

// Store Connection String in Key Vault
resource connectionStringSecret 'Microsoft.KeyVault/vaults/secrets@2023-07-01' = {
  parent: keyVault
  name: 'ConnectionStrings--DbConnection'
  properties: {
    contentType: 'text/plain'
    value: 'Server=tcp:${sqlServer.name}${environment().suffixes.sqlServerHostname},1433;Database=${sqlDatabase.name};Authentication=Active Directory Default;Encrypt=true;'
  }
}

// Allow Azure services to access SQL Server
resource sqlFirewallRule 'Microsoft.Sql/servers/firewallRules@2022-05-01-preview' = {
  parent: sqlServer
  name: 'AllowAzureServices'
  properties: {
    startIpAddress: '0.0.0.0'
    endIpAddress: '0.0.0.0'
  }
}

// Grant Web App access to Key Vault
// Assigns the "Key Vault Secrets User" role to the web app's managed identity.
// This role assignment is necessary because creating a managed identity alone doesn't grant any permissions.
// The assignment allows the web app to read secrets from Key Vault, which is required for accessing
// the stored SQL connection string. Following Azure's principle of least privilege, permissions
// must be explicitly granted even when using managed identities.
resource keyVaultRoleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  scope: keyVault
  name: guid(keyVault.id, webAppIdentity.id, 'Key Vault Secrets User')
  properties: {
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', '4633458b-17de-408a-b874-0445c86b69e6') // Key Vault Secrets User
    principalId: webAppIdentity.properties.principalId
    principalType: 'ServicePrincipal'
  }
}


// Web App
resource webApp 'Microsoft.Web/sites@2022-09-01' = {
  name: '${webAppName}-${environmentName}'
  location: location
  properties: {
    identity: {
        type: 'UserAssigned'
        userAssignedIdentities: {
          '${webAppIdentity.id}': {}
        }
    }
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
          {
            name: 'KeyVaultUri'
            value: keyVault.properties.vaultUri
          }
        ]
    }
  }
}

output appName string = webApp.name
output appUrl string = 'https://${webApp.properties.defaultHostName}'
output appInsightsInstrumentationKey string = appInsights.properties.InstrumentationKey
output appInsightsConnectionString string = appInsights.properties.ConnectionString
output sqlServerName string = sqlServer.name
output sqlDatabaseName string = sqlDatabase.name
