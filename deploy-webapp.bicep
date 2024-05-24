param subscriptionName string
param resourceGroupName string
param webAppName string
param location string

resource subscription 'Microsoft.Resources/subscriptions@2021-04-01' = {
    name: subscriptionName
    location: location
    properties: {
        displayName: subscriptionName
    }
}

resource resourceGroup 'Microsoft.Resources/resourceGroups@2021-04-01' = {
    name: resourceGroupName
    location: location
}

resource appServicePlan 'Microsoft.Web/serverfarms@2021-02-01' = {
    name: '${webAppName}-plan'
    location: location
    properties: {
        reserved: true // Indicate it's a reserved instance (required for Free tier)
      }
      sku: {
        name: 'F1'
      }
}

resource webApp 'Microsoft.Web/sites@2021-02-01' = {
    name: webAppName
    location: location
    properties: {
        serverFarmId: appServicePlan.id
        siteConfig: {
            appSettings: [
                {
                    name: 'WEBSITE_RUN_FROM_PACKAGE'
                    value: '1'
                }
            ]
        }
    }
}

output webAppUrl string = webApp.properties.defaultHostName
