// ***********************************************************************
// Assembly         : DeployLCS
// Author           : Jérôme Piquot
// Created          : 04-12-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 04-12-2023
// ***********************************************************************
// <copyright file="TokenLogicApp.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DeployLCS.Workflows;

using Hexalith.Infrastructure.AzureCloud.Builders;

/// <summary>
/// Class TokenLogicApp.
/// </summary>
internal class TokenLogicApp : LogicWorkflowBuilderData
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TokenLogicApp" /> class.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="keyVaultName">Name of the key vault.</param>
    /// <param name="tenantId">The tenant identifier.</param>
    /// <param name="location">The location.</param>
    public TokenLogicApp(string name, string keyVaultName, string tenantId, string? location)
        : base(name, location)
    {
        KeyVaultName = keyVaultName;
        TenantId = tenantId;
    }

    /// <summary>
    /// Gets the name of the key vault.
    /// </summary>
    /// <value>The name of the key vault.</value>
    public string KeyVaultName { get; }

    /// <summary>
    /// Gets the tenant identifier.
    /// </summary>
    /// <value>The tenant identifier.</value>
    public string TenantId { get; }

    /// <summary>
    /// Gets the definition.
    /// </summary>
    /// <returns>string.</returns>
    protected override string GetDefinition()
    {
        return $$"""
    {
        "$schema": "https://schema.management.azure.com/providers/Microsoft.Logic/schemas/2016-06-01/workflowdefinition.json#",
        "actions": {
            "Get_LCS_Application_Id": {
                "inputs": {
                    "host": {
                        "connection": {
                            "name": "@parameters('$connections')['keyvault']['connectionId']"
                        }
                    },
                    "method": "get",
                    "path": "/secrets/@{encodeURIComponent('LcsApplicationId')}/value"
                },
                "runAfter": {
                    "Get_LCS_Password": [
                        "Succeeded"
                    ]
                },
                "type": "ApiConnection"
            },
            "Get_LCS_Password": {
                "inputs": {
                    "host": {
                        "connection": {
                            "name": "@parameters('$connections')['keyvault']['connectionId']"
                        }
                    },
                    "method": "get",
                    "path": "/secrets/@{encodeURIComponent('LcsPassword')}/value"
                },
                "runAfter": {
                    "Get_LCS_User_Name": [
                        "Succeeded"
                    ]
                },
                "type": "ApiConnection"
            },
            "Get_LCS_Token": {
                "inputs": {
                    "body": "client_id=@{body('Get_LCS_Application_Id')?['value']}&scope=https://lcsapi.lcs.dynamics.com//.default&username=@{body('Get_LCS_User_Name')?['value']}&password=@{body('Get_LCS_Password')?['value']}&grant_type=password",
                    "headers": {
                        "Accept": "application/json",
                        "Content-Type": "application/x-www-form-urlencoded",
                        "Host": "login.microsoftonline.com"
                    },
                    "method": "POST",
                    "uri": "https://login.microsoftonline.com/@{parameters('TenantId')}/oauth2/v2.0/token"
                },
                "runAfter": {
                    "Get_LCS_Application_Id": [
                        "Succeeded"
                    ]
                },
                "type": "Http"
            },
            "Get_LCS_User_Name": {
                "inputs": {
                    "host": {
                        "connection": {
                            "name": "@parameters('$connections')['keyvault']['connectionId']"
                        }
                    },
                    "method": "get",
                    "path": "/secrets/@{encodeURIComponent('LcsUserName')}/value"
                },
                "runAfter": {},
                "type": "ApiConnection"
            },
            "Save_LCS_Token_in_Key_Vault": {
                "inputs": {
                    "authentication": {
                        "audience": "https://vault.azure.net",
                        "type": "ManagedServiceIdentity"
                    },
                    "body": {
                        "value": "@{body('Get_LCS_Token')}"
                    },
                    "method": "POST",
                    "uri": "https://{{KeyVaultName}}.vault.azure.net/secrets/LcsToken?api-version=7.2"
                },
                "runAfter": {
                    "Get_LCS_Token": [
                        "Succeeded"
                    ]
                },
                "type": "Http"
            }
        },
        "contentVersion": "1.0.0.0",
        "outputs": {},
        "parameters": {
            "$connections": {
                "defaultValue": {},
                "type": "Object"
            },
            "TenantId": {
                "defaultValue": {{TenantId}},
                "type": "String"
            }
        },
        "triggers": {
            "Recurrence": {
                "evaluatedRecurrence": {
                    "frequency": "Minute",
                    "interval": 55
                },
                "recurrence": {
                    "frequency": "Minute",
                    "interval": 55
                },
                "type": "Recurrence"
            }
        }
    }
    """;
    }

    /// <summary>
    /// Gets the parameters.
    /// </summary>
    /// <returns>string.</returns>
    protected override string GetParameters()
    {
        return $$"""
        {
            "$connections": {
                "value": {
                    "keyvault": {
                        "connectionId": "/subscriptions/f4236a6c-e305-48c7-abc2-09cc4d1452b1/resourceGroups/LCS/providers/Microsoft.Web/connections/keyvault",
                        "connectionName": "keyvault",
                        "connectionProperties": {
                            "authentication": {
                                "type": "ManagedServiceIdentity"
                            }
                        },
                        "id": "/subscriptions/f4236a6c-e305-48c7-abc2-09cc4d1452b1/providers/Microsoft.Web/locations/francecentral/managedApis/keyvault"
                    }
                }
            }
        }
        """;
    }
}